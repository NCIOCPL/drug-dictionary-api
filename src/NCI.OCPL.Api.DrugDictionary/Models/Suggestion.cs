using System.Text.Json.Serialization;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Describes a single suggestion from autosuggest
    /// </summary>
    [JsonConverter(typeof(SuggestionConverter))]
    public class Suggestion
    {
        /// <summary>
        /// The term's CDR ID.
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Drug Dictionary Term.
        /// </summary>
        public string TermName { get; set; }
    }
}
