using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Elasticsearch.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json.Linq;
using Xunit;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.Common.Testing;
using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    ///  Tests to verify the structure of requests to Elasticsearch.
    /// </summary>
    public class ESDrugsQueryServiceTest_Search : ESDrugsQueryServiceTest_Common
    {
        public static IEnumerable<object[]> SearchRequestScenarios = new[]
        {
            new object[] { new SearchSvc_Begins_Olaparib() },
            new object[] { new SearchSvc_Contains_Cetuximab() },
            new object[] { new SearchSvc_ZeroOffset_Trametinib() },
            new object[] { new SearchSvc_Contains_LongName() },
            new object[] { new SearchSvc_Begins_LongName() },
            new object[] { new SearchSvc_Contains_Paclitaxel() }
        };

        /// <summary>
        ///  Verify structure of the request for Search.
        /// </summary>
        [Theory, MemberData(nameof(SearchRequestScenarios))]
        public async void Search_TestRequestSetup(BaseSearchSvcRequestScenario data)
        {
            Uri esURI = null;
            string esContentType = String.Empty;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.

            JToken requestBody = null;

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.SearchResponse<DrugTerm>>((req, res) =>
            {
                // We don't really care about the response for this test.
                res.Stream = MockEmptyResponse;
                res.StatusCode = 200;

                esURI = req.Uri;
                esContentType = req.RequestMimeType;
                esMethod = req.Method;
                requestBody = conn.GetRequestPost(req);
            });

            // The URI does not matter, this connection never requests from the server.
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> clientOptions = GetMockOptions();

            ESDrugsQueryService query = new ESDrugsQueryService(client, clientOptions, new NullLogger<ESDrugsQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            DrugTermResults result = await query.Search(data.SearchText, data.MatchType, data.Size, data.From);

            Assert.Equal("/drugv1/_search", esURI.AbsolutePath);
            Assert.Equal("application/json", esContentType);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.Equal(data.ExpectedData, requestBody, new JTokenEqualityComparer());
        }

        /// <summary>
        /// Graceful handling of failures to connect to Elasticsearch
        /// </summary>
        /// <param name="returnStatus"></param>
        [Theory]
        [InlineData(401)]
        [InlineData(403)]
        [InlineData(500)]
        [InlineData(502)]
        [InlineData(503)]
        public async void BadConnection(int returnStatus)
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes("An error message"),
                statusCode: returnStatus,
                exception: null,
                contentType: "text/plain"
            );

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            APIErrorException ex = await Assert.ThrowsAsync<APIErrorException>(
                () => drugClient.Search("cancer", MatchType.Begins, 50, 0)
            );

            Assert.Equal(500, ex.HttpStatusCode);
            Assert.Equal(ESDrugsQueryService.INTERNAL_ERRORS_MESSAGE, ex.Message);
        }


        /// <summary>
        /// Graceful handling of an invalid response from Elaticsearch.
        /// </summary>
        [Theory]
        [InlineData("Not the server you were looking for")] // Bad server
        [InlineData("{")] // Interrupted connection
        public async void InvalidResponse(string responseBody)
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes(responseBody),
                statusCode: 200,
                exception: null,
                contentType: "text/plain"
            );

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            APIErrorException ex = await Assert.ThrowsAsync<APIErrorException>(
                () => drugClient.Search("cancer", MatchType.Begins, 50, 0)
            );

            Assert.Equal(500, ex.HttpStatusCode);
            Assert.Equal(ESDrugsQueryService.INTERNAL_ERRORS_MESSAGE, ex.Message);
        }

        public static IEnumerable<object[]> SearchResults_Data = new []
        {
            new object[]{ new SearchResults_Data_Begins_NoResults() },
            new object[]{ new SearchResults_Data_Contains_NoResults() },
            new object[]{ new SearchResults_Data_Begins_SingleResult() },
            new object[]{ new SearchResults_Data_Contains_MultipleResult() },
        };

        /// <summary>
        /// Verify loading of differing search results.
        /// </summary>
        [Theory, MemberData(nameof(SearchResults_Data))]
        public async void GetSearchResults(SearchResults_Data_Base data)
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes(data.ResponseBody),
                statusCode: 200,
                exception: null,
                contentType: "application/json"
            );

            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            DrugTermResults result = await drugClient.Search("doesn't matter", data.MatchType, 50, 0);

            Assert.Equal(data.ExpectedResult, result, new DrugTermResultsComparer());
        }

    }
}

