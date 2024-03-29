namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class Name_NoDrugInfoSummary_TestData : BaseGetByNameTestData
    {
        /// <inheritdoc />
        public override string PrettyUrlName => "autologous-anti-icam-1-car-cd28-4-1bb-cd3zeta-expressing-t-cells-aic100";

        /// <inheritdoc />
        public override DrugTerm ExpectedData => new DrugTerm
        {
            TermId = 801844,
            Name = "autologous anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T cells AIC100",
            FirstLetter = 'a',
            NCIConceptId = "C173378",
            NCIConceptName = "Autologous Anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T-cells AIC100",
            TermNameType = TermNameType.PreferredName,
            PrettyUrlName = "autologous-anti-icam-1-car-cd28-4-1bb-cd3zeta-expressing-t-cells-aic100",
            DrugInfoSummaryLink = null,
            Type = DrugResourceType.DrugTerm,
            Definition = new Definition
            {
                Html = "A preparation of autologous T lymphocytes that have been transduced with a lentiviral vector to express a chimeric antigen receptor (CAR) containing the Inserted (I) domain variant of lymphocyte function-associated antigen-1 (LFA-1) which targets intercellular adhesion molecule-1 (ICAM-1 or CD54), and the co-stimulatory signaling domains of CD28, 4-1BB (CD137) and CD3zeta, with potential immunostimulating and antineoplastic activities. Upon administration, autologous anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T cells AIC100 recognize and kill ICAM-1-expressing tumor cells. ICAM-1, normally expressed on leukocytes and endothelial cells, may be overexpressed in a variety of cancers. CAR-T cells AIC100 are also engineered to express somatostatin receptor subtype 2 (SSTR2), allowing the imaging of the CAR-T cells in patients.",
                Text = "A preparation of autologous T lymphocytes that have been transduced with a lentiviral vector to express a chimeric antigen receptor (CAR) containing the Inserted (I) domain variant of lymphocyte function-associated antigen-1 (LFA-1) which targets intercellular adhesion molecule-1 (ICAM-1 or CD54), and the co-stimulatory signaling domains of CD28, 4-1BB (CD137) and CD3zeta, with potential immunostimulating and antineoplastic activities. Upon administration, autologous anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T cells AIC100 recognize and kill ICAM-1-expressing tumor cells. ICAM-1, normally expressed on leukocytes and endothelial cells, may be overexpressed in a variety of cancers. CAR-T cells AIC100 are also engineered to express somatostatin receptor subtype 2 (SSTR2), allowing the imaging of the CAR-T cells in patients."
            },
            Aliases = new TermAlias[] {
                new TermAlias
                {
                    Type = TermNameType.CodeName,
                    Name = "AIC100"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "autologous ICAM-1-targeted CAR-T cells AIC100"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "autologous ICAM-1-targeted CAR T cells AIC100"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "autologous ICAM-1-targeted CAR-T lymphocytes AIC100"
                },
                new TermAlias
                {
                    Type = TermNameType.Synonym,
                    Name = "CAR-T cells AIC100"
                },
                new TermAlias
                {
                    Type = TermNameType.CodeName,
                    Name = "AIC-100"
                },
                new TermAlias
                {
                    Type = TermNameType.CodeName,
                    Name = "AIC 100"
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
                            ""_id"": ""801844"",
                            ""_score"": null,
                            ""_source"": {
                                ""term_id"": ""801844"",
                                ""name"": ""autologous anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T cells AIC100"",
                                ""first_letter"": ""a"",
                                ""type"": ""DrugTerm"",
                                ""term_name_type"": ""PreferredName"",
                                ""pretty_url_name"": ""autologous-anti-icam-1-car-cd28-4-1bb-cd3zeta-expressing-t-cells-aic100"",
                                ""aliases"": [
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""AIC100""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""autologous ICAM-1-targeted CAR-T cells AIC100""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""autologous ICAM-1-targeted CAR T cells AIC100""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""autologous ICAM-1-targeted CAR-T lymphocytes AIC100""
                                    },
                                    {
                                        ""type"": ""Synonym"",
                                        ""name"": ""CAR-T cells AIC100""
                                    },
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""AIC-100""
                                    },
                                    {
                                        ""type"": ""CodeName"",
                                        ""name"": ""AIC 100""
                                    }
                                ],
                                ""definition"": {
                                    ""text"": ""A preparation of autologous T lymphocytes that have been transduced with a lentiviral vector to express a chimeric antigen receptor (CAR) containing the Inserted (I) domain variant of lymphocyte function-associated antigen-1 (LFA-1) which targets intercellular adhesion molecule-1 (ICAM-1 or CD54), and the co-stimulatory signaling domains of CD28, 4-1BB (CD137) and CD3zeta, with potential immunostimulating and antineoplastic activities. Upon administration, autologous anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T cells AIC100 recognize and kill ICAM-1-expressing tumor cells. ICAM-1, normally expressed on leukocytes and endothelial cells, may be overexpressed in a variety of cancers. CAR-T cells AIC100 are also engineered to express somatostatin receptor subtype 2 (SSTR2), allowing the imaging of the CAR-T cells in patients."",
                                    ""html"": ""A preparation of autologous T lymphocytes that have been transduced with a lentiviral vector to express a chimeric antigen receptor (CAR) containing the Inserted (I) domain variant of lymphocyte function-associated antigen-1 (LFA-1) which targets intercellular adhesion molecule-1 (ICAM-1 or CD54), and the co-stimulatory signaling domains of CD28, 4-1BB (CD137) and CD3zeta, with potential immunostimulating and antineoplastic activities. Upon administration, autologous anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T cells AIC100 recognize and kill ICAM-1-expressing tumor cells. ICAM-1, normally expressed on leukocytes and endothelial cells, may be overexpressed in a variety of cancers. CAR-T cells AIC100 are also engineered to express somatostatin receptor subtype 2 (SSTR2), allowing the imaging of the CAR-T cells in patients.""
                                },
                                ""nci_concept_id"": ""C173378"",
                                ""nci_concept_name"": ""Autologous Anti-ICAM-1-CAR-CD28-4-1BB-CD3zeta-expressing T-cells AIC100""
                            },
                            ""sort"": [
                                ""autologous anti-icam-1-car-cd28-4-1bb-cd3zeta-expressing t cells aic100""
                            ]
                        }
                    ]
                }
            }
        ";
    }
}