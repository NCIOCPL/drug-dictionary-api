using System;
using System.Collections.Generic;
using System.Text;

using Elasticsearch.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
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
    ///  Tests to verify the functionality of the GetByName method.
    /// </summary>
    public class ESDrugsQueryServiceTest_GetByName : ESDrugsQueryServiceTest_Common
    {
        /// <summary>
        /// Test failures to successfully connect to Elasticsearch
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
                () => drugClient.GetByName("chicken")
            );
            Assert.Equal("errors occured.", ex.Message);
        }

        /// <summary>
        /// Test receiving an invalid response from ES in GetById.
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
                () => drugClient.GetByName("chicken")
            );
            Assert.Equal("errors occured.", ex.Message);
        }

        public static IEnumerable<object[]> GetByNameRequestScenarios = new[]
        {
            new object[] { new ExpandSvc_Olaparib() },
            new object[] { new ExpandSvc_WithDashes() },
            new object[] { new GetByName_LongPrettyURL() }
        };

        /// <summary>
        ///  Verify structure of the request for GetAll.
        /// </summary>
        [Theory, MemberData(nameof(GetByNameRequestScenarios))]
        public async void TestRequestSetup(BaseGetByNameSvcRequestScenario data)
        {
            Uri esURI = null;
            string esContentType = String.Empty;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.

            JToken requestBody = null;

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.SearchResponse<DrugTerm>>((req, res) =>
            {
                // We don't really care about the response for this test.
                res.Stream = MockSingleTermResponse;
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
            DrugTerm result = await query.GetByName(data.PrettyUrlName);

            Assert.Equal("/drugv1/_search", esURI.AbsolutePath);
            Assert.Equal("application/json", esContentType);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.Equal(data.ExpectedData, requestBody, new JTokenEqualityComparer());
        }

        /// <summary>
        /// Verify that GetByName returns in the expected manner when Elasticsearch reports that the
        /// term doesn't exist.
        /// </summary>
        [Fact]
        public async void TermNotFound()
        {
            InMemoryConnection conn = new InMemoryConnection(
                responseBody: Encoding.UTF8.GetBytes(
                    @"{
                        ""took"": 1,
                        ""timed_out"": false,
                        ""_shards"": {
                            ""total"": 1,
                            ""successful"": 1,
                            ""skipped"": 0,
                            ""failed"": 0
                        },
                        ""hits"": {
                            ""total"": {
                                ""value"": 0,
                                ""relation"": ""eq""
                            },
                            ""max_score"": null,
                            ""hits"": []
                        }
                    }"
                ),
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

            DrugTerm term = await drugClient.GetByName("this-pretty-url-does-not-exist");

            Assert.Null(term);
        }

        public static IEnumerable<object[]> DataLoadingData => new[] {
            new  object[] { new Name_NoDrugInfoSummary_TestData() },
            new object[] { new Name_WithDrugInfoSummary_TestData() }
        };

        /// <summary>
        /// Test loading variations of the DrugTerm structure.
        /// </summary>
        [Theory, MemberData(nameof(DataLoadingData))]
        public async void DataLoading(BaseGetByNameTestData data)
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

            DrugTerm actualTerm = await drugClient.GetByName(data.PrettyUrlName);

            Assert.Equal(data.ExpectedData.Definition, actualTerm.Definition, new DefinitionComparer());
            Assert.Equal(data.ExpectedData.DrugInfoSummaryLink, actualTerm.DrugInfoSummaryLink, new DrugInfoSummaryLinkComparer());
            Assert.Equal(data.ExpectedData.Aliases, actualTerm.Aliases, new ArrayComparer<TermAlias, TermAliasComparer>());
            Assert.Equal(data.ExpectedData, actualTerm, new DrugTermComparer());
        }

    }
}