namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Data structure representing a single drug dictionary entry.
    /// </summary>
    public class DrugTerm : DrugResource, IDrugResource
    {
        /// <summary>
        /// Array of aliases for the drug.
        /// </summary>
        public TermAlias[] Aliases { get; set; }

        /// <summary>
        /// The drug's definition.
        /// </summary>
        public Definition Definition { get; set; }

        /// <summary>
        /// Link to a matching Drug Information Summary.
        /// </summary>
        public DrugInfoSummaryLink DrugInfoSummaryLink { get; set; }

        /// <summary>
        /// The NCI Concept ID.
        /// </summary>
        public string NCIConceptId { get; set; }

        /// <summary>
        /// The NCI Concept Name.
        /// </summary>
        public string NCIConceptName { get; set; }
    }
}