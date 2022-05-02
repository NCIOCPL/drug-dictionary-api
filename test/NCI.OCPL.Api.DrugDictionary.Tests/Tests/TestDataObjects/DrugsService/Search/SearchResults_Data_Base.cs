namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public abstract class SearchResults_Data_Base
    {
        /// <summary>
        /// What type of search?
        /// </summary>
        public abstract MatchType MatchType { get; }

        /// <summary>
        /// Simulated elasticsearch response body.
        /// </summary>
        public abstract string ResponseBody { get; }

        /// <summary>
        /// /// The expected result.
        /// </summary>
        public abstract DrugTermResults ExpectedResult { get; }
    }
}