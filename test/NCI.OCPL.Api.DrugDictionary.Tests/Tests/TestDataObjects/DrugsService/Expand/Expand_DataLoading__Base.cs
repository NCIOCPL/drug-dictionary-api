namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// Base class for expand data loading test objects.
    /// </summary>
    public abstract class Expand_DataLoading_Base
    {
        /// <summary>
        /// The letter to expand on.
        /// </summary>
        public abstract char Letter { get; }

        /// <summary>
        /// Starting offset for the simulated request.
        /// </summary>
        public abstract int From { get; }

        /// <summary>
        /// Number of records for the simulated request.
        /// </summary>
        public abstract int Size { get; }

        /// <summary>
        /// Simulated response body.
        /// </summary>
        public abstract string ResponseBody { get; }

        /// <summary>
        /// /// The expected result.
        /// </summary>
        public abstract DrugTermResults ExpectedResult { get; }
    }
}