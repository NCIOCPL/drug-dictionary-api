namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Base class for testing the ability of GetById to retrieve Drug Terms.
    /// </summary>
    public abstract class BaseGetByNameTestData
    {
        /// <summary>
        /// The term's pretty-url name.
        /// </summary>

        public abstract string PrettyUrlName { get; }

        /// <summary>
        /// The drug term which is expected to be retrieved.
        /// </summary>
        public abstract DrugTerm ExpectedData { get; }

        /// <summary>
        /// The simulated elasticsearch response.
        /// </summary>
        /// <value></value>
        public abstract string ResponseBody { get; }
    }
}