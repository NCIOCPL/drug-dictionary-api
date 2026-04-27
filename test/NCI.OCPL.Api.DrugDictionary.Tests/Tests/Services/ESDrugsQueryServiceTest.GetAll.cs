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
    public class ESDrugsQueryServiceTest_GetByAll : ESDrugsQueryServiceTest_Common
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
        public async Task BadConnection(int returnStatus)
        {
            InMemoryConnection connection = new InMemoryConnection(Array.Empty<byte>(), returnStatus);
            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(connection);
            ElasticsearchClient client = new ElasticsearchClient(settings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            Exception ex = await Assert.ThrowsAsync<APIInternalException>(
                () => drugClient.GetAll(
                    200,
                    100,
                    new DrugResourceType[]  {DrugResourceType.DrugTerm, DrugResourceType.DrugAlias},
                    new TermNameType[] {TermNameType.Synonym, TermNameType.USBrandName, TermNameType.PreferredName},
                    new TermNameType[] {TermNameType.ChemicalStructureName, TermNameType.CodeName, TermNameType.ObsoleteName}
                )
            );
            Assert.Equal(ESDrugsQueryService.INTERNAL_ERRORS_MESSAGE, ex.Message);
        }

        public static IEnumerable<object[]> GetAllRequestScenarios = new[]
        {
            new object[] { new GetAllSvc_Begins_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new GetAllSvc_Begins_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new GetAllSvc_Begins_SingleResourceType_SingleInclude_SingleExclude() },
            new object[] { new GetAllSvc_Contains_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new GetAllSvc_Contains_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new GetAllSvc_Contains_SingleResourceType_SingleInclude_SingleExclude() }
        };

        /// <summary>
        ///  Verify structure of the request for GetAll.
        /// </summary>
        [Theory, MemberData(nameof(GetAllRequestScenarios))]
        public async Task TestRequestSetup(BaseGetAllSvcRequestScenario data)
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
            DrugTermResults result = await query.GetAll(data.Size, data.From,
                data.IncludeResourceTypes, data.IncludeNameTypes, data.ExcludeNameTypes
                );

            Assert.Equal("/drugv1/_search", esURI.AbsolutePath);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.True(JsonNode.DeepEquals(data.ExpectedData, requestBody));
        }

        /// <summary>
        /// Test loading variations of the DrugTerm structure.
        /// </summary>
        [Fact]
        public async Task DataLoading()
        {
            MockGetAllData data = new MockGetAllData();

            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(data.ResponseBody, 200);
            ElasticsearchClient client = new ElasticsearchClient(settings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> apiOptions = GetMockOptions();

            ESDrugsQueryService drugClient = new ESDrugsQueryService(client, apiOptions, NullLogger<ESDrugsQueryService>.Instance);

            DrugTermResults actual = await drugClient.GetAll(
                3,
                100,
                new DrugResourceType[] { DrugResourceType.DrugTerm, DrugResourceType.DrugAlias },
                new TermNameType[] { TermNameType.Synonym, TermNameType.USBrandName, TermNameType.PreferredName },
                new TermNameType[] { TermNameType.ChemicalStructureName, TermNameType.CodeName, TermNameType.ObsoleteName }
            );

            Assert.Equal(data.Expected, actual, new DrugTermResultsComparer());
        }

    }
}