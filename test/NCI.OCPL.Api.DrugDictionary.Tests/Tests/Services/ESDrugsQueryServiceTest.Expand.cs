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
    public class ESDrugsQueryServiceTest_Expand : ESDrugsQueryServiceTest_Common
    {
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

            Exception ex = await Assert.ThrowsAsync<APIInternalException>(
                () => drugClient.Expand(
                    'g', 0, 100,
                    new DrugResourceType[] { DrugResourceType.DrugTerm, DrugResourceType.DrugAlias },
                    new TermNameType[] { TermNameType.Synonym, TermNameType.USBrandName, TermNameType.PreferredName },
                    new TermNameType[] { TermNameType.ChemicalStructureName, TermNameType.CodeName, TermNameType.ObsoleteName }
                )
            );
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

            Exception ex = await Assert.ThrowsAsync<APIInternalException>(
                () => drugClient.Expand(
                    'g', 0, 100,
                    new DrugResourceType[] { DrugResourceType.DrugTerm, DrugResourceType.DrugAlias },
                    new TermNameType[] { TermNameType.Synonym, TermNameType.USBrandName, TermNameType.PreferredName },
                    new TermNameType[] { TermNameType.ChemicalStructureName, TermNameType.CodeName, TermNameType.ObsoleteName }
                )
            );
            Assert.Equal(ESDrugsQueryService.INTERNAL_ERRORS_MESSAGE, ex.Message);
        }

        public static IEnumerable<object[]> ExpandRequestScenarios = new[]
        {
            new object[] { new ExpandSvc_Begins_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new ExpandSvc_Begins_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new ExpandSvc_Begins_SingleResourceType_SingleInclude_SingleExclude() },
            new object[] { new ExpandSvc_Contains_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new ExpandSvc_Contains_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new ExpandSvc_Contains_SingleResourceType_SingleInclude_SingleExclude() }
        };

        /// <summary>
        ///  Verify structure of the request for Expand.
        /// </summary>
        [Theory, MemberData(nameof(ExpandRequestScenarios))]
        public async void Expand_TestRequestSetup(BaseExpandSvcRequestScenario data)
        {
            Uri esURI = null;
            string esContentType = String.Empty;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.

            JToken requestBody = null;

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.SearchResponse<IDrugResource>>((req, res) =>
            {
                // We don't really care about the response for this test.
                res.Stream = MockEmptyResponse;
                res.StatusCode = 200;

                esURI = req.Uri;
                esContentType = req.RequestMimeType;
                esMethod = req.Method;
                requestBody = conn.GetRequestPost(req);
            });

            // The URI does not matter, an InMemoryConnection never requests from the server.
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn, sourceSerializer: JsonNetSerializer.Default);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> clientOptions = GetMockOptions();

            ESDrugsQueryService query = new ESDrugsQueryService(client, clientOptions, new NullLogger<ESDrugsQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            DrugTermResults result = await query.Expand(data.Letter, data.Size, data.From,
                data.IncludeResourceTypes, data.IncludeNameTypes, data.ExcludeNameTypes
                );

            Assert.Equal("/drugv1/_search", esURI.AbsolutePath);
            Assert.Equal("application/json", esContentType);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.Equal(data.ExpectedData, requestBody, new JTokenEqualityComparer());
        }

        public static IEnumerable<object[]> DataLoadingScenarios = new []
        {
            new object[] { new Expand_DataLoading_NoResults()},
            new object[] { new Expand_DataLoading_SingleResults()},
            new object[] { new Expand_DataLoading_MultipleResults()},
        };

        /// <summary>
        /// Verify loading of differing search results.
        /// </summary>
        [Theory, MemberData(nameof(DataLoadingScenarios))]
        public async void NothingFound(Expand_DataLoading_Base data)
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

            DrugTermResults result = await drugClient.Expand(
                    data.Letter, data.Size, data.From,
                    new DrugResourceType[] { DrugResourceType.DrugTerm, DrugResourceType.DrugAlias },
                    new TermNameType[] { TermNameType.Synonym, TermNameType.USBrandName, TermNameType.PreferredName },
                    new TermNameType[] { TermNameType.ChemicalStructureName, TermNameType.CodeName, TermNameType.ObsoleteName }
                );

            Assert.Equal(data.ExpectedResult, result, new DrugTermResultsComparer());
        }

    }
}