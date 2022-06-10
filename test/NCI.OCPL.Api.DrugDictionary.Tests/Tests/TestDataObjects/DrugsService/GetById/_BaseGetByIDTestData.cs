namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Base class for testing the ability of GetById to retrieve Drug Terms.
    /// </summary>
    public abstract class BaseGetByIdTestData
    {
        /// <summary>
        /// The term's ID.
        /// </summary>

        public abstract int TermID { get; }

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