namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class Expand_DataLoading_SingleResults : Expand_DataLoading_Base
    {
        public override char Letter => 'b';

        public override int From => 150;

        public override int Size => 1;

        public override string ResponseBody =>
            @"{
                ""took"": 1870,
                ""timed_out"": false,
                ""_shards"": {
                    ""total"": 1,
                    ""successful"": 1,
                    ""skipped"": 0,
                    ""failed"": 0
                },
                ""hits"": {
                    ""total"": {
                        ""value"": 512,
                        ""relation"": ""eq""
                    },
                    ""max_score"": null,
                    ""hits"": [
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""39324-4"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""39324"",
                                ""name"": ""B Cell Proliferating Factor"",
                                ""first_letter"": ""b"",
                                ""type"": ""DrugAlias"",
                                ""term_name_type"": ""Synonym"",
                                ""pretty_url_name"": ""binetrakin"",
                                ""preferred_name"": ""recombinant interleukin-4""
                            },
                            ""sort"": [
                                ""a2a receptor antagonist eos100850""
                            ]
                        }
                    ]
                }
            }";

        public override DrugTermResults ExpectedResult => new
            DrugTermResults
        {
            Meta = new ResultsMetadata
            {
                From = 150,
                TotalResults = 512
            },
            Results = new IDrugResource[] {
                new DrugAlias {
                    TermId = 39324,
                    Name = "B Cell Proliferating Factor",
                    FirstLetter = 'b',
                    Type = DrugResourceType.DrugAlias,
                    TermNameType = TermNameType.Synonym,
                    PrettyUrlName = "binetrakin",
                    PreferredName = "recombinant interleukin-4"
                },
            }
        };
    }
}