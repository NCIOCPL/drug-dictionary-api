using System;
using System.Collections.Generic;
using System.Text;

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Testing;
using Xunit;

using NCI.OCPL.Api.Common.Testing;
using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public partial class ESAutosuggestQueryServiceTest
    {
        public static IEnumerable<object[]> AutosuggestRequestScenarios = new[]
        {
            new object[] { new AutosuggestSvc_Begins_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new AutosuggestSvc_Contains_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new AutosuggestSvc_Begins_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new AutosuggestSvc_Contains_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new AutosuggestSvc_Begins_SingleResourceType_SingleInclude_SingleExclude() },
            new object[] { new AutosuggestSvc_Contains_SingleResourceType_SingleInclude_SingleExclude() }
        };

        /// <summary>
        /// Test to verify that Elasticsearch requests are being assembled as expected.
        /// </summary>
        [Theory, MemberData(nameof(AutosuggestRequestScenarios))]
        public async Task GetSuggestions_TestRequestSetup(BaseAutosuggestServiceScenario data)
        {
            Uri esURI = null;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.
            JsonNode requestBody = null;

            ElasticsearchClientSettings settings = TestingElasticsearchClientSettingsFactory.Create(
                MockEmptyResponseString,
                200, details =>
                {
                    esURI = details.Uri;
                    esMethod = details.HttpMethod;
                    requestBody = JsonNode.Parse(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                });
            ElasticsearchClient client = new ElasticsearchClient(settings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> clientOptions = ESQueryServiceTest_Helper.MockSearchOptions;
            clientOptions.Value.Autosuggest.MaxSuggestionLength = data.MaxSuggestionLength;

            ESAutosuggestQueryService query = new ESAutosuggestQueryService(client, clientOptions, new NullLogger<ESAutosuggestQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            Suggestion[] result = await query.GetSuggestions(data.SearchText, data.MatchType, data.Size,
                data.IncludeResourceTypes, data.IncludeNameTypes, data.ExcludeNameTypes);

            Assert.Equal("/drugv1/_search", esURI.AbsolutePath);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.True(JsonNode.DeepEquals(data.ExpectedData, requestBody));
        }

        /// <summary>
        /// Simulates a "no results found" response from Elasticsearch so we
        /// have something for tests where we don't care about the response.
        /// </summary>
        private string MockEmptyResponseString =>
            @"
{
    ""took"" : 3,
    ""timed_out"" : false,
    ""_shards"" : {
        ""total"" : 1,
        ""successful"" : 1,
        ""skipped"" : 0,
        ""failed"" : 0
    },
    ""hits"" : {
        ""total"" : {
            ""value"" : 0,
            ""relation"" : ""eq""
        },
        ""max_score"" : null,
        ""hits"" : [ ]
    }
}";

    }
}