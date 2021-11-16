namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// A (very) cutdown test of getting "all" the data.
    /// </summary>
    public class MockGetAllData
    {
        public DrugTermResults Expected =>
            new DrugTermResults
            {
                Meta = new ResultsMetadata
                {
                    TotalResults = 6013,
                    From = 100
                },
                Results = new IDrugResource[] {
                    new DrugTerm
                    {
                        TermId = 765514,
                        Name = "activin type 2B receptor Fc fusion protein STM 434",
                        FirstLetter = 'a',
                        Type = DrugResourceType.DrugTerm,
                        TermNameType = TermNameType.PreferredName,
                        PrettyUrlName = "activin-type-2b-receptor-fc-fusion-protein-stm-434",
                        Aliases = new TermAlias[] {
                            new TermAlias
                            {
                                Name = "STM 434",
                                Type = TermNameType.CodeName
                            },
                            new TermAlias
                            {
                                Name = "activin A inhibitor STM 434",
                                Type = TermNameType.Synonym
                            }
                        },
                        Definition = new Definition
                        {
                            Text = "A soluble fusion protein containing the extracellular domain of the activin receptor type 2B (ACVR2B or ActRIIB) fused to a human Fc domain, with potential antineoplastic activity. Upon intravenous administration, STM 434 selectively binds to the growth factor activin A, thereby preventing its binding to and the activation of endogenous ActRIIB. This prevents activin A/ActRIIB-mediated signaling and inhibits the proliferation of activin A-overexpressing tumor cells. Activin A, a member of the transforming growth factor beta (TGF-beta) superfamily, is overexpressed in a variety of cancers and plays a key role in promoting cancer cell proliferation, migration, and survival.",
                            Html = "A soluble fusion protein containing the extracellular domain of the activin receptor type 2B (ACVR2B or ActRIIB) fused to a human Fc domain, with potential antineoplastic activity. Upon intravenous administration, STM 434 selectively binds to the growth factor activin A, thereby preventing its binding to and the activation of endogenous ActRIIB. This prevents activin A/ActRIIB-mediated signaling and inhibits the proliferation of activin A-overexpressing tumor cells. Activin A, a member of the transforming growth factor beta (TGF-beta) superfamily, is overexpressed in a variety of cancers and plays a key role in promoting cancer cell proliferation, migration, and survival."
                        },
                        NCIConceptId = "C118625",
                        NCIConceptName =  "Activin Type 2B Receptor Fc Fusion Protein STM 434"
                    },
                    new DrugAlias
                    {
                        TermId = 791414,
                        Name = "(((4-hydroxy-1-methyl-7-phenoxyisoquinolin-3-yl)carbonyl)amino)acetic acid",
                        FirstLetter = '#',
                        Type = DrugResourceType.DrugAlias,
                        TermNameType = TermNameType.ChemicalStructureName,
                        PreferredName = "roxadustat",
                        PrettyUrlName = "roxadustat"
                    },
                    new DrugTerm
                    {
                        TermId = 793142,
                        Name = "acyclic nucleoside phosphonate prodrug ABI-1968",
                        FirstLetter = 'a',
                        Type = DrugResourceType.DrugTerm,
                        TermNameType = TermNameType.PreferredName,
                        PrettyUrlName = "acyclic-nucleoside-phosphonate-prodrug-abi-1968",
                        Aliases = new TermAlias[] {
                            new TermAlias
                            {
                                Name = "ABI-1968",
                                Type = TermNameType.CodeName
                            },
                            new TermAlias
                            {
                                Name = "HTI 1968",
                                Type = TermNameType.CodeName
                            },
                            new TermAlias
                            {
                                Name = "ABI1968",
                                Type = TermNameType.CodeName
                            }
                        },
                        Definition = new Definition
                        {
                            Text = "A prodrug of an acyclic nucleoside phosphonate, with potential anti-viral and antineoplastic activities. Upon administration, acyclic nucleoside phosphonate prodrug ABI-1968 is taken up by viral-infected cells and converted to its active metabolite. The metabolite is incorporated into DNA chains by DNA polymerases, which results in the termination of DNA synthesis, inhibits viral replication and induces apoptosis and inhibits the proliferation of susceptible virally-infected tumor cells.",
                            Html = "A prodrug of an acyclic nucleoside phosphonate, with potential anti-viral and antineoplastic activities. Upon administration, acyclic nucleoside phosphonate prodrug ABI-1968 is taken up by viral-infected cells and converted to its active metabolite. The metabolite is incorporated into DNA chains by DNA polymerases, which results in the termination of DNA synthesis, inhibits viral replication and induces apoptosis and inhibits the proliferation of susceptible virally-infected tumor cells."
                        },
                        NCIConceptId = "C151941",
                        NCIConceptName = "Acyclic Nucleoside Phosphonate Prodrug ABI-1968"
                    }
                }
            };

        public string ResponseBody =>
            @"{
                ""took"": 11,
                ""timed_out"": false,
                ""_shards"": {
                    ""total"": 1,
                    ""successful"": 1,
                    ""skipped"": 0,
                    ""failed"": 0
                },
                ""hits"": {
                    ""total"": {
                        ""value"": 6013,
                        ""relation"": ""eq""
                    },
                    ""max_score"": null,
                    ""hits"": [
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""765514"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""765514"",
                                ""name"": ""activin type 2B receptor Fc fusion protein STM 434"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""activin-type-2b-receptor-fc-fusion-protein-stm-434"",
                                ""aliases"": [
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""STM 434""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""activin A inhibitor STM 434""
                                    }
                                ],
                                ""definition"": {
                                    ""text"": ""A soluble fusion protein containing the extracellular domain of the activin receptor type 2B (ACVR2B or ActRIIB) fused to a human Fc domain, with potential antineoplastic activity. Upon intravenous administration, STM 434 selectively binds to the growth factor activin A, thereby preventing its binding to and the activation of endogenous ActRIIB. This prevents activin A/ActRIIB-mediated signaling and inhibits the proliferation of activin A-overexpressing tumor cells. Activin A, a member of the transforming growth factor beta (TGF-beta) superfamily, is overexpressed in a variety of cancers and plays a key role in promoting cancer cell proliferation, migration, and survival."",
                                    ""html"": ""A soluble fusion protein containing the extracellular domain of the activin receptor type 2B (ACVR2B or ActRIIB) fused to a human Fc domain, with potential antineoplastic activity. Upon intravenous administration, STM 434 selectively binds to the growth factor activin A, thereby preventing its binding to and the activation of endogenous ActRIIB. This prevents activin A/ActRIIB-mediated signaling and inhibits the proliferation of activin A-overexpressing tumor cells. Activin A, a member of the transforming growth factor beta (TGF-beta) superfamily, is overexpressed in a variety of cancers and plays a key role in promoting cancer cell proliferation, migration, and survival.""
                                },
                                ""nci_concept_id"": ""C118625"",
                                ""nci_concept_name"": ""Activin Type 2B Receptor Fc Fusion Protein STM 434""
                            },
                            ""sort"": [
                                ""activin type 2b receptor fc fusion protein stm 434""
                            ]
                        },
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""791414-4"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""791414"",
                                ""name"": ""(((4-hydroxy-1-methyl-7-phenoxyisoquinolin-3-yl)carbonyl)amino)acetic acid"",
                                ""first_letter"": ""#"",
                                ""type"": ""DrugAlias"",
                                ""term_name_type"": ""ChemicalStructureName"",
                                ""pretty_url_name"": ""roxadustat"",
                                ""preferred_name"": ""roxadustat""
                            },
                            ""sort"": [
                                ""(((4-hydroxy-1-methyl-7-phenoxyisoquinolin-3-yl)carbonyl)amino)acetic acid""
                            ]
                        },
                        {
                            ""_index"": ""drugv1"",
                            ""_type"": ""_doc"",
                            ""_id"": ""793142"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""793142"",
                                ""name"": ""acyclic nucleoside phosphonate prodrug ABI-1968"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""acyclic-nucleoside-phosphonate-prodrug-abi-1968"",
                                ""aliases"": [
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""ABI-1968""
                                    },
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""HTI 1968""
                                    },
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""ABI1968""
                                    }
                                ],
                                ""definition"": {
                                    ""text"": ""A prodrug of an acyclic nucleoside phosphonate, with potential anti-viral and antineoplastic activities. Upon administration, acyclic nucleoside phosphonate prodrug ABI-1968 is taken up by viral-infected cells and converted to its active metabolite. The metabolite is incorporated into DNA chains by DNA polymerases, which results in the termination of DNA synthesis, inhibits viral replication and induces apoptosis and inhibits the proliferation of susceptible virally-infected tumor cells."",
                                    ""html"": ""A prodrug of an acyclic nucleoside phosphonate, with potential anti-viral and antineoplastic activities. Upon administration, acyclic nucleoside phosphonate prodrug ABI-1968 is taken up by viral-infected cells and converted to its active metabolite. The metabolite is incorporated into DNA chains by DNA polymerases, which results in the termination of DNA synthesis, inhibits viral replication and induces apoptosis and inhibits the proliferation of susceptible virally-infected tumor cells.""
                                },
                                ""nci_concept_id"": ""C151941"",
                                ""nci_concept_name"": ""Acyclic Nucleoside Phosphonate Prodrug ABI-1968""
                            },
                            ""sort"": [
                                ""acyclic nucleoside phosphonate prodrug abi-1968""
                            ]
                        }
                    ]
                }
            }";
    }
}