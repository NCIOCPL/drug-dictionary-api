namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// This class implements the properties defined in IDrugResource, however it
    /// expressly does *NOT* "implement" IDrugResource. The intent is to enforce the
    /// use of classes implementing IDrugResource for serialization, while providing
    /// a single class for maintaining the NEST attribute mapping data.
    /// </summary>
    public class DrugResource
    {
        /// <summary>
        /// The CDR ID of the full term for a DrugTerm, or the full term this alias represents.
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// The name of this resource as it should be displayed on the listing pages
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The first letter of the term as would be used by the expand endpoint
        /// </summary>
        public char FirstLetter { get; set; }

        /// <summary>
        /// The type of this resource.
        /// </summary>
        public DrugResourceType Type { get; set; }

        /// <summary>
        /// The type of name for this resource.
        /// </summary>
        public TermNameType TermNameType { get; set; }

        /// <summary>
        /// The url fragment used in the full definition URL of /def/&lt;PrettyUrlName&gt;.
        /// </summary>
        public string PrettyUrlName { get; set; }
    }
}