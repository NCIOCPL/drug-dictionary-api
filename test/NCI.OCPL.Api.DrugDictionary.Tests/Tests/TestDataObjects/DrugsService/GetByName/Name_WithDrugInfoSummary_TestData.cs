namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class Name_WithDrugInfoSummary_TestData : BaseGetByNameTestData
    {
        /// <inheritdoc />
        public override string PrettyUrlName => "bevacizumab";

        /// <inheritdoc />
        public override DrugTerm ExpectedData => new DrugTerm
        {
            TermId = 43234,
            Name = "bevacizumab",
            FirstLetter = 'b',
            NCIConceptId = "C2039",
            NCIConceptName = "Bevacizumab",
            TermNameType = TermNameType.PreferredName,
            PrettyUrlName = "bevacizumab",
            Type = DrugResourceType.DrugTerm,
            DrugInfoSummaryLink = new DrugInfoSummaryLink
            {
                Text = "Bevacizumab",
                URI = new System.Uri("https://www.cancer.gov/about-cancer/treatment/drugs/bevacizumab")
            },
            Definition = new Definition
            {
                Html = "A recombinant humanized monoclonal antibody directed against the vascular endothelial growth factor (VEGF), a pro-angiogenic cytokine.  Bevacizumab binds to VEGF and inhibits VEGF receptor binding, thereby preventing the growth and maintenance of tumor blood vessels.",
                Text = "A recombinant humanized monoclonal antibody directed against the vascular endothelial growth factor (VEGF), a pro-angiogenic cytokine.  Bevacizumab binds to VEGF and inhibits VEGF receptor binding, thereby preventing the growth and maintenance of tumor blood vessels."
            },
            Aliases = new TermAlias[] {
                new TermAlias
                {
                    Type = TermNameType.USBrandName,
                    Name = "Avastin"
                },
                new TermAlias
                {
                    Type = TermNameType.USBrandName,
                    Name = "Mvasi"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "anti-VEGF monoclonal antibody"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "rhuMAb VEGF"
                },
                new TermAlias
                {
                    Type = TermNameType.Abbreviation,
                    Name = "rhuMAb VEGF"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "anti-VEGF humanized monoclonal antibody"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "anti-VEGF rhuMAb"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "recombinant humanized anti-VEGF monoclonal antibody"
                },
                new TermAlias
                {
                    Type = TermNameType.INDCode,
                    Name = "9877"
                },
                new TermAlias
                {
                    Type = TermNameType.INDCode,
                    Name = "11460"
                },
                new TermAlias
                {
                    Type = TermNameType.INDCode,
                    Name = "7921"
                },
                new TermAlias
                {
                    Type = TermNameType.NSCNumber,
                    Name = "704865"
                },
                new TermAlias
                {
                    Type = TermNameType.Abbreviation,
                    Name = "rhuMab-VEGF"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar BEVZ92"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar BI 695502"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar FKB238"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar GB-222"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar PF-06439535"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar QL 1101"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar CBT 124"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar MIL60"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar MB02"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar HD204"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar BAT1706"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar HLX04"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar IBI305"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar SCT510"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar LY01008"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar CT-P16"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar RPH-001"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "bevacizumab biosimilar TRS003"
                },
                new TermAlias
                {
                    Type = TermNameType.CodeName,
                    Name = "HD204"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "immunoglobulin G1 (human-mouse monoclonal rhuMab-VEGF gamma-chain anti-human vascular endothelial growth factor), disulfide with human-mouse monoclonal rhuMab-VEGF light chain, dimer"
                }
            }
        };

        /// <inheritdoc />
        public override string ResponseBody => @"
            {
                ""took"": 1,
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
                            ""_id"": ""43234"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""43234"",
                                ""name"": ""bevacizumab"",
                                ""first_letter"": ""b"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""bevacizumab"",
                                ""aliases"": [
                                    {
                                        ""type"": ""USBrandName"",
                                        ""name"": ""Avastin""
                                    },
                                    {
                                        ""type"": ""USBrandName"",
                                        ""name"": ""Mvasi""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""anti-VEGF monoclonal antibody""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""rhuMAb VEGF""
                                    },
                                    {
                                        ""type"": ""Abbreviation"",
                                        ""name"": ""rhuMAb VEGF""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""anti-VEGF humanized monoclonal antibody""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""anti-VEGF rhuMAb""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""recombinant humanized anti-VEGF monoclonal antibody""
                                    },
                                    {
                                        ""type"": ""INDCode"",
                                        ""name"": ""9877""
                                    },
                                    {
                                        ""type"": ""INDCode"",
                                        ""name"": ""11460""
                                    },
                                    {
                                        ""type"": ""INDCode"",
                                        ""name"": ""7921""
                                    },
                                    {
                                        ""type"": ""NSCNumber"",
                                        ""name"": ""704865""
                                    },
                                    {
                                        ""type"": ""Abbreviation"",
                                        ""name"": ""rhuMab-VEGF""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar BEVZ92""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar BI 695502""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar FKB238""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar GB-222""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar PF-06439535""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar QL 1101""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar CBT 124""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar MIL60""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar MB02""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar HD204""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar BAT1706""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar HLX04""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar IBI305""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar SCT510""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar LY01008""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar CT-P16""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar RPH-001""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""bevacizumab biosimilar TRS003""
                                    },
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""HD204""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""immunoglobulin G1 (human-mouse monoclonal rhuMab-VEGF gamma-chain anti-human vascular endothelial growth factor), disulfide with human-mouse monoclonal rhuMab-VEGF light chain, dimer""
                                    }
                                ],
                                ""definition"": {
                                    ""text"": ""A recombinant humanized monoclonal antibody directed against the vascular endothelial growth factor (VEGF), a pro-angiogenic cytokine.  Bevacizumab binds to VEGF and inhibits VEGF receptor binding, thereby preventing the growth and maintenance of tumor blood vessels."",
                                    ""html"": ""A recombinant humanized monoclonal antibody directed against the vascular endothelial growth factor (VEGF), a pro-angiogenic cytokine.  Bevacizumab binds to VEGF and inhibits VEGF receptor binding, thereby preventing the growth and maintenance of tumor blood vessels.""
                                },
                                ""drug_info_summary_link"": {
                                    ""url"": ""https://www.cancer.gov/about-cancer/treatment/drugs/bevacizumab"",
                                    ""text"": ""Bevacizumab""
                                },
                                ""nci_concept_id"": ""C2039"",
                                ""nci_concept_name"": ""Bevacizumab""
                            },
                            ""sort"": [
                                ""bevacizumab""
                            ]
                        }
                    ]
                }
            }
        ";
    }
}