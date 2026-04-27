using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
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
        /// Graceful handling of failures to connect to Elasticsearch.
        /// </summary>
        /// <param name="returnStatus">The status code to return</param>
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
        public async Task Expand_TestRequestSetup(BaseExpandSvcRequestScenario data)
        {
            Uri esURI = null;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.
            JsonNode requestBody = null;

            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(
                MockEmptyResponseString,
                200,
                details =>
                {
                    esURI = details.Uri;
                    esMethod = details.HttpMethod;
                    requestBody = JsonNode.Parse(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                });
            ElasticsearchClient client = new ElasticsearchClient(settings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> clientOptions = GetMockOptions();

            ESDrugsQueryService query = new ESDrugsQueryService(client, clientOptions, new NullLogger<ESDrugsQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            DrugTermResults result = await query.Expand(data.Letter, data.Size, data.From,
                data.IncludeResourceTypes, data.IncludeNameTypes, data.ExcludeNameTypes
                );

            Assert.Equal("/drugv1/_search", esURI.AbsolutePath);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.True(JsonNode.DeepEquals(data.ExpectedData, requestBody));
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
        public async Task NothingFound(Expand_DataLoading_Base data)
        {
            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(data.ResponseBody, 200);
            ElasticsearchClient client = new ElasticsearchClient(settings);

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