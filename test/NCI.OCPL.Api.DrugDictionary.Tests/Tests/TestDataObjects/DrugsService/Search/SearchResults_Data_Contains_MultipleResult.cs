namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class SearchResults_Data_Contains_MultipleResult : SearchResults_Data_Base
    {
        public override MatchType MatchType => MatchType.Contains;

        public override string ResponseBody =>
            @"{
                ""took"": 4,
                ""timed_out"": false,
                ""_shards"": {
                    ""total"": 1,
                    ""successful"": 1,
                    ""skipped"": 0,
                    ""failed"": 0
                },
                ""hits"": {
                    ""total"": {
                        ""value"": 335,
                        ""relation"": ""eq""
                    },
                    ""max_score"": null,
                    ""hits"": [
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""299488"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""299488"",
                                ""name"": ""abagovomab"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""abagovomab"",
                                ""aliases"": [
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""monoclonal antibody ACA125 anti-idiotype vaccine""
                                    },
                                    {
                                        ""type"": ""ForeignBrandName"",
                                        ""name"": ""VaccinOvar""
                                    }
                                ],
                                ""definition"": {
                                    ""text"": ""A murine IgG1 monoclonal anti-idiotype antibody, containing a variable antigen-binding region that functionally mimics the three-dimensional structure of a specific epitope on the ovarian cancer tumor-associated antigen CA-125, with potential antineoplastic activity. With a variable antigen-binding region that acts as a surrogate antigen for CA-125, abagovomab may stimulate the host immune system to elicit humoral and cellular immune responses against CA-125-positive tumor cells, resulting in inhibition of tumor cell proliferation."",
                                    ""html"": ""A murine IgG1 monoclonal anti-idiotype antibody, containing a variable antigen-binding region that functionally mimics the three-dimensional structure of a specific epitope on the ovarian cancer tumor-associated antigen CA-125, with potential antineoplastic activity. With a variable antigen-binding region that acts as a surrogate antigen for CA-125, abagovomab may stimulate the host immune system to elicit humoral and cellular immune responses against CA-125-positive tumor cells, resulting in inhibition of tumor cell proliferation.""
                                },
                                ""nci_concept_id"": ""C26449"",
                                ""nci_concept_name"": ""Abagovomab""
                            },
                            ""sort"": [
                                ""abagovomab""
                            ]
                        },
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""637163"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""637163"",
                                ""name"": ""actinium Ac 225 lintuzumab"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""actinium-ac-225-lintuzumab"",
                                ""aliases"": [
                                    {
                                        ""type"": ""Abbreviation"",
                                        ""name"": ""225Ac-HuM195""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""actinium-225-labeled humanized anti-CD33 monoclonal antibody HuM195""
                                    }
                                ],
                                ""definition"": {
                                    ""text"": ""A radioimmunoconjugate consisting of the humanized monoclonal antibody lintuzumab conjugated to the alpha-emitting radioisotope actinium Ac 225 with potential antineoplastic activity. The monoclonal antibody moiety of actinium Ac 225 lintuzumab specifically binds to the cell surface antigen CD33 antigen, delivering a cytotoxic dose of alpha radiation to cells expressing CD33. CD33 is a cell surface antigen expressed on normal non-pluripotent hematopoietic stem cells and overexpressed on myeloid leukemia cells."",
                                    ""html"": ""A radioimmunoconjugate consisting of the humanized monoclonal antibody lintuzumab conjugated to the alpha-emitting radioisotope actinium Ac 225 with potential antineoplastic activity. The monoclonal antibody moiety of actinium Ac 225 lintuzumab specifically binds to the cell surface antigen CD33 antigen, delivering a cytotoxic dose of alpha radiation to cells expressing CD33. CD33 is a cell surface antigen expressed on normal non-pluripotent hematopoietic stem cells and overexpressed on myeloid leukemia cells.""
                                },
                                ""nci_concept_id"": ""C82414"",
                                ""nci_concept_name"": ""Actinium Ac 225 Lintuzumab""
                            },
                            ""sort"": [
                                ""actinium ac 225 lintuzumab""
                            ]
                        },
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""791684"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""791684"",
                                ""name"": ""adalimumab"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""adalimumab"",
                                ""aliases"": [
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""D2E7""
                                    },
                                    {
                                        ""type"": ""USBrandName"",
                                        ""name"": ""Humira""
                                    },
                                    {
                                        ""type"": ""ChemicalStructureName"",
                                        ""name"": ""immunoglobulin G1, anti-(human tumor necrosis factor) (human monoclonal D2E7 heavy chain), disulfide with human monoclonal D2E7 light chain, dimer""
                                    },
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""LU200134""
                                    },
                                    {
                                        ""type"": ""CASRegistryName"",
                                        ""name"": ""331731-18-1""
                                    }
                                ],
                                ""definition"": {
                                    ""text"": ""A recombinant, human IgG1 monoclonal antibody directed against tumor necrosis factor-alpha (TNF-alpha), with immunomodulating activity. Upon administration, adalimumab binds to TNF-alpha, thereby preventing its binding to the p55 and p75 TNF cell surface receptors and inhibiting TNF-mediated immune responses. TNF-alpha, a pro-inflammatory cytokine, is upregulated in various autoimmune diseases."",
                                    ""html"": ""A recombinant, human IgG1 monoclonal antibody directed against tumor necrosis factor-alpha (TNF-alpha), with immunomodulating activity. Upon administration, adalimumab binds to TNF-alpha, thereby preventing its binding to the p55 and p75 TNF cell surface receptors and inhibiting TNF-mediated immune responses. TNF-alpha, a pro-inflammatory cytokine, is upregulated in various autoimmune diseases.""
                                },
                                ""nci_concept_id"": ""C65216"",
                                ""nci_concept_name"": ""Adalimumab""
                            },
                            ""sort"": [
                                ""adalimumab""
                            ]
                        }
                    ]
                }
            }";


        // Drug terms containing "mab".
        public override DrugTermResults ExpectedResult =>
            new DrugTermResults
            {
                Results = new DrugTerm[]
                {
                    new DrugTerm
                    {
                        TermId = 299488,
                        Name = "abagovomab",
                        FirstLetter = 'a',
                        Type = DrugResourceType.DrugTerm,
                        TermNameType = TermNameType.PreferredName,
                        PrettyUrlName = "abagovomab",
                        Aliases = new TermAlias[]
                        {
                            new TermAlias
                            {
                                Type = TermNameType.Synonym,
                                Name = "monoclonal antibody ACA125 anti-idiotype vaccine"
                            },
                            new TermAlias
                            {
                                Type = TermNameType.ForeignBrandName,
                                Name = "VaccinOvar"
                            }
                        },
                        Definition = new Definition
                        {
                            Html = "A murine IgG1 monoclonal anti-idiotype antibody, containing a variable antigen-binding region that functionally mimics the three-dimensional structure of a specific epitope on the ovarian cancer tumor-associated antigen CA-125, with potential antineoplastic activity. With a variable antigen-binding region that acts as a surrogate antigen for CA-125, abagovomab may stimulate the host immune system to elicit humoral and cellular immune responses against CA-125-positive tumor cells, resulting in inhibition of tumor cell proliferation.",
                            Text = "A murine IgG1 monoclonal anti-idiotype antibody, containing a variable antigen-binding region that functionally mimics the three-dimensional structure of a specific epitope on the ovarian cancer tumor-associated antigen CA-125, with potential antineoplastic activity. With a variable antigen-binding region that acts as a surrogate antigen for CA-125, abagovomab may stimulate the host immune system to elicit humoral and cellular immune responses against CA-125-positive tumor cells, resulting in inhibition of tumor cell proliferation."
                        },
                        NCIConceptId = "C26449",
                        NCIConceptName = "Abagovomab"
                    },
                    new DrugTerm
                    {
                        TermId = 637163,
                        Name = "actinium Ac 225 lintuzumab",
                        FirstLetter = 'a',
                        Type = DrugResourceType.DrugTerm,
                        TermNameType = TermNameType.PreferredName,
                        PrettyUrlName = "actinium-ac-225-lintuzumab",
                        Aliases = new TermAlias[]
                        {
                            new TermAlias
                            {
                                Type = TermNameType.Abbreviation,
                                Name = "225Ac-HuM195"
                            },
                            new TermAlias
                            {
                                Type = TermNameType.Synonym,
                                Name = "actinium-225-labeled humanized anti-CD33 monoclonal antibody HuM195"
                            }
                        },
                        Definition = new Definition
                        {
                            Html = "A radioimmunoconjugate consisting of the humanized monoclonal antibody lintuzumab conjugated to the alpha-emitting radioisotope actinium Ac 225 with potential antineoplastic activity. The monoclonal antibody moiety of actinium Ac 225 lintuzumab specifically binds to the cell surface antigen CD33 antigen, delivering a cytotoxic dose of alpha radiation to cells expressing CD33. CD33 is a cell surface antigen expressed on normal non-pluripotent hematopoietic stem cells and overexpressed on myeloid leukemia cells.",
                            Text = "A radioimmunoconjugate consisting of the humanized monoclonal antibody lintuzumab conjugated to the alpha-emitting radioisotope actinium Ac 225 with potential antineoplastic activity. The monoclonal antibody moiety of actinium Ac 225 lintuzumab specifically binds to the cell surface antigen CD33 antigen, delivering a cytotoxic dose of alpha radiation to cells expressing CD33. CD33 is a cell surface antigen expressed on normal non-pluripotent hematopoietic stem cells and overexpressed on myeloid leukemia cells."
                        },
                        NCIConceptId = "C82414",
                        NCIConceptName = "Actinium Ac 225 Lintuzumab"
                    },
                    new DrugTerm
                    {
                        TermId = 791684,
                        Name = "adalimumab",
                        FirstLetter =   'a',
                        Type = DrugResourceType.DrugTerm,
                        TermNameType = TermNameType.PreferredName,
                        PrettyUrlName = "adalimumab",
                        Aliases = new TermAlias[]
                        {
                            new TermAlias
                            {
                                Type = TermNameType.CodeName,
                                Name = "D2E7"
                            },
                            new TermAlias
                            {
                                Type = TermNameType.USBrandName,
                                Name = "Humira"
                            },
                            new TermAlias
                            {
                                Type = TermNameType.ChemicalStructureName,
                                Name = "immunoglobulin G1, anti-(human tumor necrosis factor) (human monoclonal D2E7 heavy chain), disulfide with human monoclonal D2E7 light chain, dimer"
                            },
                            new TermAlias
                            {
                                Type = TermNameType.CodeName,
                                Name = "LU200134"
                            },
                            new TermAlias
                            {
                                Type = TermNameType.CASRegistryName,
                                Name = "331731-18-1"
                            }
                        },
                        Definition = new Definition
                        {
                            Html = "A recombinant, human IgG1 monoclonal antibody directed against tumor necrosis factor-alpha (TNF-alpha), with immunomodulating activity. Upon administration, adalimumab binds to TNF-alpha, thereby preventing its binding to the p55 and p75 TNF cell surface receptors and inhibiting TNF-mediated immune responses. TNF-alpha, a pro-inflammatory cytokine, is upregulated in various autoimmune diseases.",
                            Text = "A recombinant, human IgG1 monoclonal antibody directed against tumor necrosis factor-alpha (TNF-alpha), with immunomodulating activity. Upon administration, adalimumab binds to TNF-alpha, thereby preventing its binding to the p55 and p75 TNF cell surface receptors and inhibiting TNF-mediated immune responses. TNF-alpha, a pro-inflammatory cytokine, is upregulated in various autoimmune diseases."
                        },
                        NCIConceptId = "C65216",
                        NCIConceptName = "Adalimumab"
                    }
                },
                Meta = new ResultsMetadata
                {
                    TotalResults = 335,
                    From = 0
                }
            };
    }
}