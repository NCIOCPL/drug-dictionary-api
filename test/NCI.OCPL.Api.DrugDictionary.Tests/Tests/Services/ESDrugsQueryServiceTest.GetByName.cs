using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.Common.Testing;
using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;
using System.Text.Json.Nodes;

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
        public async Task BadConnection(int returnStatus)
        {
            InMemoryConnection connection = new InMemoryConnection(Array.Empty<byte>(), returnStatus);
            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(connection);
            ElasticsearchClient client = new ElasticsearchClient(settings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            Exception ex = await Assert.ThrowsAsync<APIInternalException>(
                () => drugClient.GetByName("chicken")
            );
            Assert.Equal(ESDrugsQueryService.INTERNAL_ERRORS_MESSAGE, ex.Message);
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
        public async Task TestRequestSetup(BaseGetByNameSvcRequestScenario data)
        {
            Uri esURI = null;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.
            JsonNode requestBody = null;

            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(
                MockSingleTermResponseString,
                200,
                details => {
                    requestBody = JsonNode.Parse(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                    esURI = details.Uri;
                    esMethod = details.HttpMethod;
                });
            ElasticsearchClient client = new ElasticsearchClient(settings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> clientOptions = GetMockOptions();

            ESDrugsQueryService query = new ESDrugsQueryService(client, clientOptions, new NullLogger<ESDrugsQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            DrugTerm result = await query.GetByName(data.PrettyUrlName);

            Assert.Equal("/drugv1/_search", esURI.AbsolutePath);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.True(JsonNode.DeepEquals(data.ExpectedData, requestBody));
        }

        /// <summary>
        /// Verify that GetByName returns in the expected manner when Elasticsearch reports that the
        /// term doesn't exist.
        /// </summary>
        [Fact]
        public async Task TermNotFound()
        {
            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(
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
                }",
                200
            );
            ElasticsearchClient client = new ElasticsearchClient(settings);

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
        public async Task DataLoading(BaseGetByNameTestData data)
        {
            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(data.ResponseBody, 200);
            ElasticsearchClient client = new ElasticsearchClient(settings);

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