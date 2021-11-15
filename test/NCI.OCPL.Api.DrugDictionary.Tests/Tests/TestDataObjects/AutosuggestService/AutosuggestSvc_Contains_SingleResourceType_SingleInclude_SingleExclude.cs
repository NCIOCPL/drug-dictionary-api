using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Test scenario
    ///     Match Begins With
    ///     20 results
    ///     Single DrugResourceType values.
    ///     Include single TermNameType values.
    ///     Exclude one TermNameType.
    /// </summary>
    public class AutosuggestSvc_Contains_SingleResourceType_SingleInclude_SingleExclude : BaseAutosuggestServiceScenario
    {
        public override string SearchText => "gadavi";

        public override MatchType MatchType => MatchType.Contains;

        public override int MaxSuggestionLength => 30;

        public override int Size => 20;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] { DrugResourceType.DrugTerm };

        public override TermNameType[] IncludeNameTypes => new TermNameType[] { TermNameType.USBrandName };

        public override TermNameType[] ExcludeNameTypes => new TermNameType[] { TermNameType.Synonym };

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""query"": {
                ""bool"": {
                    ""must"": [
                        { ""match_phrase"": { ""name._autocomplete"": { ""query"": ""gadavi"" } } },
                        { ""match"": {""name._contain"": { ""query"": ""gadavi"" } } },
                        { ""terms"": { ""type"": [ ""DrugTerm"" ] } },
                        { ""terms"": { ""term_name_type"": [ ""USBrandName"" ] } }
                    ],
                    ""must_not"": [
                        { ""prefix"": { ""name"": { ""value"": ""gadavi"" } } },
                        { ""terms"": { ""term_name_type"": [ ""Synonym"" ] } }
                    ],
                    ""filter"": [
                        {
                            ""script"": {
                                ""script"": {
                                    ""source"": ""doc['name'].value.length() <= 30""
                                }
                            }
                        }
                    ]
                }
            },
            ""size"": 20,
            ""sort"": [ { ""name"": {} } ],
            ""_source"": { ""includes"": [ ""term_id"", ""name"" ] }
        }
        ");

    }
}