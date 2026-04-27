namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Represents a record containing an alias for a <see cref="T:NCI.OCPL.Api.DrugDictionary.DrugTerm"/>.
    /// </summary>
    public class DrugAlias : DrugResource, IDrugResource
    {
        /// <summary>
        /// The drug's preferred name.
        /// </summary>
        public string PreferredName { get; set; }
    }
}