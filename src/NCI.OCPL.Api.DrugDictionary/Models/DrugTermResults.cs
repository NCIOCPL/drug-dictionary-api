namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Represents the results of a search operation.
    /// </summary>
    public class DrugTermResults
    {
        /// <summary>
        /// Metadata about the results.
        /// </summary>
        public ResultsMetadata Meta { get; set; }

        /// <summary>
        /// Array of Drug dictionary entries matching the search. May be empty.
        /// </summary>
        public IDrugResource[] Results { get; set; }

        /// <summary>
        /// Link to ????
        /// </summary>
        public Metalink Links { get; set; }
    }
}