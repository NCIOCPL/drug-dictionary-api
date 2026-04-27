using System.Text.Json.Serialization;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Identifies the type of an IDrugResource descendant.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DrugResourceType
    {
        /// <summary>
        /// Identifies an IDrugResource instance as a concrete term in the
        /// drug dictionary.
        /// </summary>
        DrugTerm,

        /// <summary>
        /// Identifies an IDrugResource instance as an alias for a term.
        /// </summary>
        DrugAlias
    }
}