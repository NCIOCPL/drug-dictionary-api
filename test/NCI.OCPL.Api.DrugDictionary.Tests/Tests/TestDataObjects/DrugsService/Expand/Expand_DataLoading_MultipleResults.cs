namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class Expand_DataLoading_MultipleResults : Expand_DataLoading_Base
    {
        public override char Letter => 'a';

        public override int From => 100;

        public override int Size => 4;

        public override string ResponseBody =>
            @"{
                ""took"": 55,
                ""timed_out"": false,
                ""_shards"": {
                    ""total"": 1,
                    ""successful"": 1,
                    ""skipped"": 0,
                    ""failed"": 0
                },
                ""hits"": {
                    ""total"": {
                        ""value"": 2800,
                        ""relation"": ""eq""
                    },
                    ""max_score"": null,
                    ""hits"": [
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""792737-2"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""792737"",
                                ""name"": ""A-101 solution"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugAlias"",
                                ""term_name_type"": ""Synonym"",
                                ""pretty_url_name"": ""a-101-topical-solution"",
                                ""preferred_name"": ""A-101 topical solution""
                            },
                            ""sort"": [
                                ""a-101 solution""
                            ]
                        },
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""792737"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""792737"",
                                ""name"": ""A-101 topical solution"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""a-101-topical-solution"",
                                ""aliases"": [
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""A-101""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""A-101 solution""
                                    }
                                ],
                                ""definition"": {
                                    ""text"": ""A proprietary topical formulation consisting of a high-concentration of hydrogen peroxide (H2O2), with potential keratolytic activity. Upon administration of the A-101 topical solution to an affected area of skin, the hydrogen peroxide penetrates into the cells, increases oxygen content, produces reactive oxygen species (ROS), causes oxidative stress and induces apoptosis through oxidative damage. This may clear the affected skin cells and remove common warts (verruca vulgaris) or seborrheic keratosis (SK)."",
                                    ""html"": ""A proprietary topical formulation consisting of a high-concentration of hydrogen peroxide (H2O2), with potential keratolytic activity. Upon administration of the A-101 topical solution to an affected area of skin, the hydrogen peroxide penetrates into the cells, increases oxygen content, produces reactive oxygen species (ROS), causes oxidative stress and induces apoptosis through oxidative damage. This may clear the affected skin cells and remove common warts (verruca vulgaris) or seborrheic keratosis (SK).""
                                },
                                ""nci_concept_id"": ""C150374"",
                                ""nci_concept_name"": ""A-101 Topical Solution""
                            },
                            ""sort"": [
                                ""a-101 topical solution""
                            ]
                        },
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""588895-3"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""588895"",
                                ""name"": ""A-dmDT390-bisFv(UCHT1) fusion protein"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugAlias"",
                                ""term_name_type"": ""Synonym"",
                                ""pretty_url_name"": ""anti-cd3-immunotoxin-a-dmdt390-bisfvucht1"",
                                ""preferred_name"": ""anti-CD3 immunotoxin A-dmDT390-bisFv(UCHT1)""
                            },
                            ""sort"": [
                                ""a-dmdt390-bisfv(ucht1) fusion protein""
                            ]
                        },
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""801361"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""801361"",
                                ""name"": ""A2A receptor antagonist EOS100850"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""a2a-receptor-antagonist-eos100850"",
                                ""aliases"": [
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""EOS100850""
                                    },
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""EOS-100850""
                                    },
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""EOS 100850""
                                    }
                                ],
                                ""definition"": {
                                    ""text"": ""An orally bioavailable immune checkpoint inhibitor and antagonist of the adenosine A2A receptor (A2AR; ADORA2A), with potential immunomodulating and antineoplastic activities. Upon administration, A2AR antagonist EOS100850 selectively binds to and inhibits A2AR expressed on T-lymphocytes. This prevents tumor-released adenosine from interacting with the A2A receptors, thereby blocking the adenosine/A2AR-mediated inhibition of T-lymphocytes. This results in the proliferation and activation of T-lymphocytes, and stimulates a T-cell-mediated immune response against tumor cells. A2AR, a G protein-coupled receptor, is highly expressed on the cell surfaces of T-cells and, upon activation by adenosine, inhibits their proliferation and activation. Adenosine is often overproduced by cancer cells and plays a key role in immunosuppression."",
                                    ""html"": ""An orally bioavailable immune checkpoint inhibitor and antagonist of the adenosine A2A receptor (A2AR; ADORA2A), with potential immunomodulating and antineoplastic activities. Upon administration, A2AR antagonist EOS100850 selectively binds to and inhibits A2AR expressed on T-lymphocytes. This prevents tumor-released adenosine from interacting with the A2A receptors, thereby blocking the adenosine/A2AR-mediated inhibition of T-lymphocytes. This results in the proliferation and activation of T-lymphocytes, and stimulates a T-cell-mediated immune response against tumor cells. A2AR, a G protein-coupled receptor, is highly expressed on the cell surfaces of T-cells and, upon activation by adenosine, inhibits their proliferation and activation. Adenosine is often overproduced by cancer cells and plays a key role in immunosuppression.""
                                }
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
                From = 100,
                TotalResults = 2800
            },
            Results = new IDrugResource[] {
                new DrugAlias {
                    TermId = 792737,
                    Name = "A-101 solution",
                    FirstLetter = 'a',
                    Type = DrugResourceType.DrugAlias,
                    TermNameType = TermNameType.Synonym,
                    PrettyUrlName = "a-101-topical-solution",
                    PreferredName = "A-101 topical solution"
                },
                new DrugTerm {
                    TermId = 792737,
                    Name = "A-101 topical solution",
                    FirstLetter = 'a',
                    Type = DrugResourceType.DrugTerm,
                    TermNameType = TermNameType.PreferredName,
                    PrettyUrlName = "a-101-topical-solution",
                    Aliases = new TermAlias[] {
                        new TermAlias
                        {
                            Type = TermNameType.CodeName,
                            Name = "A-101"
                        },
                        new TermAlias
                        {
                            Type = TermNameType.Synonym,
                            Name = "A-101 solution"
                        }
                    },
                    Definition = new Definition
                    {
                        Text = "A proprietary topical formulation consisting of a high-concentration of hydrogen peroxide (H2O2), with potential keratolytic activity. Upon administration of the A-101 topical solution to an affected area of skin, the hydrogen peroxide penetrates into the cells, increases oxygen content, produces reactive oxygen species (ROS), causes oxidative stress and induces apoptosis through oxidative damage. This may clear the affected skin cells and remove common warts (verruca vulgaris) or seborrheic keratosis (SK).",
                        Html = "A proprietary topical formulation consisting of a high-concentration of hydrogen peroxide (H2O2), with potential keratolytic activity. Upon administration of the A-101 topical solution to an affected area of skin, the hydrogen peroxide penetrates into the cells, increases oxygen content, produces reactive oxygen species (ROS), causes oxidative stress and induces apoptosis through oxidative damage. This may clear the affected skin cells and remove common warts (verruca vulgaris) or seborrheic keratosis (SK)."
                    },
                    NCIConceptId = "C150374",
                    NCIConceptName = "A-101 Topical Solution"
                },
                new DrugAlias
                {
                    TermId = 588895,
                    Name = "A-dmDT390-bisFv(UCHT1) fusion protein",
                    FirstLetter = 'a',
                    Type = DrugResourceType.DrugAlias,
                    TermNameType = TermNameType.Synonym,
                    PrettyUrlName = "anti-cd3-immunotoxin-a-dmdt390-bisfvucht1",
                    PreferredName = "anti-CD3 immunotoxin A-dmDT390-bisFv(UCHT1)"
                },
                new DrugTerm
                {
                    TermId = 801361,
                    Name = "A2A receptor antagonist EOS100850",
                    FirstLetter = 'a',
                    Type = DrugResourceType.DrugTerm,
                    TermNameType = TermNameType.PreferredName,
                    PrettyUrlName = "a2a-receptor-antagonist-eos100850",
                    Aliases = new TermAlias[] {
                        new TermAlias
                        {
                            Type = TermNameType.CodeName,
                            Name = "EOS100850"
                        },
                        new TermAlias
                        {
                            Type = TermNameType.CodeName,
                            Name = "EOS-100850"
                        },
                        new TermAlias
                        {
                            Type = TermNameType.CodeName,
                            Name = "EOS 100850"
                        }
                    },
                    Definition = new Definition
                    {
                        Text = "An orally bioavailable immune checkpoint inhibitor and antagonist of the adenosine A2A receptor (A2AR; ADORA2A), with potential immunomodulating and antineoplastic activities. Upon administration, A2AR antagonist EOS100850 selectively binds to and inhibits A2AR expressed on T-lymphocytes. This prevents tumor-released adenosine from interacting with the A2A receptors, thereby blocking the adenosine/A2AR-mediated inhibition of T-lymphocytes. This results in the proliferation and activation of T-lymphocytes, and stimulates a T-cell-mediated immune response against tumor cells. A2AR, a G protein-coupled receptor, is highly expressed on the cell surfaces of T-cells and, upon activation by adenosine, inhibits their proliferation and activation. Adenosine is often overproduced by cancer cells and plays a key role in immunosuppression.",
                        Html = "An orally bioavailable immune checkpoint inhibitor and antagonist of the adenosine A2A receptor (A2AR; ADORA2A), with potential immunomodulating and antineoplastic activities. Upon administration, A2AR antagonist EOS100850 selectively binds to and inhibits A2AR expressed on T-lymphocytes. This prevents tumor-released adenosine from interacting with the A2A receptors, thereby blocking the adenosine/A2AR-mediated inhibition of T-lymphocytes. This results in the proliferation and activation of T-lymphocytes, and stimulates a T-cell-mediated immune response against tumor cells. A2AR, a G protein-coupled receptor, is highly expressed on the cell surfaces of T-cells and, upon activation by adenosine, inhibits their proliferation and activation. Adenosine is often overproduced by cancer cells and plays a key role in immunosuppression."
                    }
                }
            }
        };
    }
}