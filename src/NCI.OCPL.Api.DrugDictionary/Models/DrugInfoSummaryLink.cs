using System;
using System.Text.Json.Serialization;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Contains information for creating a link to a drug information summary.
    /// </summary>
    [JsonConverter(typeof(DrugInfoSummaryLinkConverter))]
    public class DrugInfoSummaryLink
    {
        /// <summary>
        /// The URL of the drug information summary.
        /// </summary>
        public Uri URI { get; set; }

        /// <summary>
        /// Link text.
        /// </summary>
        public string Text { get; set; }
    }
}