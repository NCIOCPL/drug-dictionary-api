using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
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
        public async Task Search_TestRequestSetup(BaseSearchSvcRequestScenario data)
        {
            Uri esURI = null;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.
            JsonNode requestBody = null;

            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(
                MockEmptyResponseString,
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
            DrugTermResults result = await query.Search(data.SearchText, data.MatchType, data.Size, data.From);

            Assert.Equal("/drugv1/_search", esURI.AbsolutePath);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.True(JsonNode.DeepEquals(data.ExpectedData, requestBody));
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
        public async Task BadConnection(int returnStatus)
        {
            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create("An error message", returnStatus);
            ElasticsearchClient client = new ElasticsearchClient(settings);

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
        public async Task GetSearchResults(SearchResults_Data_Base data)
        {
            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(data.ResponseBody, 200);
            ElasticsearchClient client = new ElasticsearchClient(settings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            DrugTermResults result = await drugClient.Search("doesn't matter", data.MatchType, 50, 0);

            Assert.Equal(data.ExpectedResult, result, new DrugTermResultsComparer());
        }

    }
}

