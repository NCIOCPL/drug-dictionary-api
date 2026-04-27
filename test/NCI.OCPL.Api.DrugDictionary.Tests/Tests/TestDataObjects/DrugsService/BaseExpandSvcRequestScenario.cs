using System.Text.Json.Nodes;
using Xunit.Abstractions;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    // Need to implement IXunitSerializable in order for tests to be properly
    // discoverable by xunit. No "real" serialization is needed, but this
    // prevents JsonNode properties from causing discoverability issues.
    public abstract class BaseExpandSvcRequestScenario : IXunitSerializable
    {
        /// <summary>
        /// Gets the expected response data object.  Typically a JSON string
        /// wrapped in a call to JsonNode.Parse().
        /// </summary>
        public abstract JsonNode ExpectedData { get; }

        /// <summary>
        /// The letter to expand.
        /// </summary>
        public abstract char Letter { get; }

        /// <summary>
        /// The type of match (Begins vs Contains) being tested.
        /// </summary>
        public abstract int From { get; }

        /// <summary>
        /// Contains the maximum number of suggestions the controller should return.
        /// </summary>
        public abstract int Size { get; }

        /// <summary>
        /// Array of DrugResourceTypes to include.
        /// </summary>
        public abstract DrugResourceType[] IncludeResourceTypes { get; }

        /// <summary>
        /// Array of term name types to include.
        /// </summary>
        public abstract TermNameType[] IncludeNameTypes { get; }

        /// <summary>
        /// Array of term name types to exclude.
        /// </summary>
        public abstract TermNameType[] ExcludeNameTypes { get; }

        // All data is baked into hardcoded property return values on each concrete subclass,
        // so there is no instance state to serialize or deserialize.
        public void Deserialize(IXunitSerializationInfo info) { }
        public void Serialize(IXunitSerializationInfo info) { }
    }
}