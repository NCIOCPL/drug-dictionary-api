namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class SearchResults_Data_Begins_SingleResult : SearchResults_Data_Base
    {
        public override MatchType MatchType => MatchType.Begins;

        public override string ResponseBody =>
            @"{
                ""took"": 3,
                ""timed_out"": false,
                ""_shards"": {
                    ""total"": 1,
                    ""successful"": 1,
                    ""skipped"": 0,
                    ""failed"": 0
                },
                ""hits"": {
                    ""total"": {
                        ""value"": 1,
                        ""relation"": ""eq""
                    },
                    ""max_score"": null,
                    ""hits"": [
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""729590"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""729590"",
                                ""name"": ""bevacizumab-IRDye 800CW"",
                                ""first_letter"": ""b"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""bevacizumab-irdye-800cw"",
                                ""definition"": {
                                    ""text"": ""An immunoconjugate and a fluorescent tracer consisting of the recombinant humanized anti-vascular endothelial growth factor (VEGF) monoclonal antibody bevacizumab conjugated to the N-hydroxysuccinamide (NHS) ester form of the near-infrared (NIR) fluorescent dye IRDye 800CW, that may be used for VEGF-specific tumor imaging. Upon administration, the bevacizumab moiety of bevacizumab-IRDye 800CW binds to VEGF and the fluorescent signal can be visualized using NIR fluorescence imaging (700–1,000 nm)."",
                                    ""html"": ""An immunoconjugate and a fluorescent tracer consisting of the recombinant humanized anti-vascular endothelial growth factor (VEGF) monoclonal antibody bevacizumab conjugated to the N-hydroxysuccinamide (NHS) ester form of the near-infrared (NIR) fluorescent dye IRDye 800CW, that may be used for VEGF-specific tumor imaging. Upon administration, the bevacizumab moiety of bevacizumab-IRDye 800CW binds to VEGF and the fluorescent signal can be visualized using NIR fluorescence imaging (700–1,000 nm).""
                                },
                                ""nci_concept_id"": ""C101261"",
                                ""nci_concept_name"": ""Bevacizumab-IRDye 800CW""
                            },
                            ""sort"": [
                                ""bevacizumab-irdye 800cw""
                            ]
                        }
                    ]
                }
            }";


        public override DrugTermResults ExpectedResult =>
            new DrugTermResults
            {
                Results = new DrugTerm[]
                {
                    new DrugTerm
                    {
                        TermId = 729590,
                        Name = "bevacizumab-IRDye 800CW",
                        FirstLetter = 'b',
                        Type = DrugResourceType.DrugTerm,
                        TermNameType = TermNameType.PreferredName,
                        PrettyUrlName = "bevacizumab-irdye-800cw",
                        Definition = new Definition
                        {
                            Html = "An immunoconjugate and a fluorescent tracer consisting of the recombinant humanized anti-vascular endothelial growth factor (VEGF) monoclonal antibody bevacizumab conjugated to the N-hydroxysuccinamide (NHS) ester form of the near-infrared (NIR) fluorescent dye IRDye 800CW, that may be used for VEGF-specific tumor imaging. Upon administration, the bevacizumab moiety of bevacizumab-IRDye 800CW binds to VEGF and the fluorescent signal can be visualized using NIR fluorescence imaging (700–1,000 nm).",
                            Text = "An immunoconjugate and a fluorescent tracer consisting of the recombinant humanized anti-vascular endothelial growth factor (VEGF) monoclonal antibody bevacizumab conjugated to the N-hydroxysuccinamide (NHS) ester form of the near-infrared (NIR) fluorescent dye IRDye 800CW, that may be used for VEGF-specific tumor imaging. Upon administration, the bevacizumab moiety of bevacizumab-IRDye 800CW binds to VEGF and the fluorescent signal can be visualized using NIR fluorescence imaging (700–1,000 nm)."
                        },
                        NCIConceptId = "C101261",
                        NCIConceptName = "Bevacizumab-IRDye 800CW"
                    }
                },
                Meta = new ResultsMetadata
                {
                    TotalResults = 1,
                    From = 0
                }
            };
    }
}