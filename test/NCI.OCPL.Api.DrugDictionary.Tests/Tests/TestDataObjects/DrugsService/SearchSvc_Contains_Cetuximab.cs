using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class SearchSvc_Contains_Cetuximab : BaseSearchSvcRequestScenario
    {
        public override string SearchText => "cetuximab";

        public override MatchType MatchType => MatchType.Contains;

        public override int From => 100;

        public override int Size => 50;

        public override JObject ExpectedData => JObject.Parse(@"
{
    ""from"": 100,
    ""size"": 50,
    ""_source"": {
        ""includes"": [
            ""aliases"",
            ""definition"",
            ""term_id"",
            ""name"",
            ""first_letter"",
            ""type"",
            ""term_name_type"",
            ""pretty_url_name"",
            ""preferred_name""
        ]
    },
    ""sort"": [ { ""name"": {} } ],
    ""query"": {
        ""bool"": {
            ""should"": [
                {
                    ""bool"": {
                        ""must"": [
                            { ""match"": { ""name._contain"": { ""query"": ""cetuximab"" } } },
                            { ""term"": { ""type"": { ""value"": ""DrugTerm"" } } }
                        ]
                    }
                },
                {
                    ""nested"": {
                        ""query"": { ""match"": { ""aliases.name._contain"": { ""query"": ""cetuximab"" } } },
                        ""path"": ""aliases""
                    }
                }
            ]
        }
    }
}        ");

    }
}