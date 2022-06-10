using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Elasticsearch.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json.Linq;
using Xunit;

using NCI.OCPL.Api.Common.Testing;
using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    ///  Tests to verify the structure of requests to Elasticsearch.
    /// </summary>
    public class ESDrugsQueryServiceTest_Common
    {

        /// <summary>
        /// Mock Elasticsearch configuraiton options.
        /// </summary>
        protected IOptions<DrugDictionaryAPIOptions> GetMockOptions()
        {
            Mock<IOptions<DrugDictionaryAPIOptions>> clientOptions = new Mock<IOptions<DrugDictionaryAPIOptions>>();
            clientOptions
                .SetupGet(opt => opt.Value)
                .Returns(new DrugDictionaryAPIOptions()
                {
                    AliasName = "drugv1"
                }
            );

            return clientOptions.Object;
        }

        protected Stream MockSingleTermResponse
        {
            get
            {
                string res = @"
{
    ""took"": 2,
    ""timed_out"": false,
    ""_shards"": {
        ""total"": 1,
        ""successful"": 1,
        ""skipped"": 0,
        ""failed"": 0
    },
    ""hits"": {
        ""total"": 1,
        ""max_score"": null,
        ""hits"": [
            {
                ""_index"": ""drugv1"",
                ""_type"": ""terms"",
                ""_id"": ""37780"",
                ""_score"": null,
                ""_source"": {
                    ""term_id"": ""37780"",
                    ""name"": ""iodinated contrast dye"",
                    ""first_letter"": ""i"",
                    ""type"": ""DrugTerm"",
                    ""term_name_type"": ""PreferredName"",
                    ""pretty_url_name"": ""iodinated-contrast-agent"",
                    ""aliases"": [
                        {
                            ""type"": ""Synonym"",
                            ""name"": ""contrast dye, iodinated""
                        }
                    ],
                    ""definition"": {
                        ""text"": ""A contrast agent containing an iodine-based dye used in many diagnostic imaging examinations, including computed tomography, angiography, and myelography. Check for active clinical trials using this agent. (NCI Thesaurus)"",
                        ""html"": ""A contrast agent containing an iodine-based dye used in many diagnostic imaging examinations, including computed tomography, angiography, and myelography. Check for <a ref=\""https://www.cancer.gov/about-cancer/treatment/clinical-trials/intervention/C28500\"">active clinical trials</a> using this agent. (<a ref=\""https://ncit.nci.nih.gov/ncitbrowser/ConceptReport.jsp?dictionary=NCI%20Thesaurus&code=C28500\"">NCI Thesaurus</a>)""
                    },
                    ""nci_concept_id"": ""C28500"",
                    ""nci_concept_name"": ""Iodinated Contrast Agent""
                },
                ""sort"": [ ""iodinated contrast dye"" ]
            }
        ]
    }
}
                ";

                byte[] byteArray = Encoding.UTF8.GetBytes(res);
                return new MemoryStream(byteArray);
            }
        }

        /// <summary>
        /// Simulates a "no results found" response from Elasticsearch so we
        /// have something for tests where we don't care about the response.
        /// </summary>
        protected Stream MockEmptyResponse
        {
            get
            {
                string empty = @"
{
    ""took"": 223,
    ""timed_out"": false,
    ""_shards"": {
        ""total"": 1,
        ""successful"": 1,
        ""skipped"": 0,
        ""failed"": 0
    },
    ""hits"": {
        ""total"": 0,
        ""max_score"": null,
        ""hits"": []
    }
}";
                byte[] byteArray = Encoding.UTF8.GetBytes(empty);
                return new MemoryStream(byteArray);
            }
        }



    }
}