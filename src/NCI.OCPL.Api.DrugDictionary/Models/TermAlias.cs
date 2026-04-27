namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Data structure representing one alias for a drug term.
    /// </summary>
    public class TermAlias
    {
        /// <summary>
        /// The type of alias.
        /// </summary>
        public TermNameType Type { get; set; }

        /// <summary>
        /// The actual alias name.
        /// </summary>
        public string Name { get; set; }
    }
}