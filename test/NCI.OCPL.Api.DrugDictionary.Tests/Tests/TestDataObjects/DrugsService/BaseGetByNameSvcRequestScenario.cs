using System.Text.Json.Nodes;
using Xunit.Abstractions;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    // Need to implement IXunitSerializable in order for tests to be properly
    // discoverable by xunit. No "real" serialization is needed, but this
    // prevents JsonNode properties from causing discoverability issues.
    public abstract class BaseGetByNameSvcRequestScenario : IXunitSerializable
    {
        /// <summary>
        /// Gets the expected response data object.  Typically a JSON string
        /// wrapped in a call to JsonNode.Parse().
        /// </summary>
        public abstract JsonNode ExpectedData { get; }

        /// <summary>
        /// Array of term name types to exclude.
        /// </summary>
        public abstract string PrettyUrlName { get; }


        // All data is baked into hardcoded property return values on each concrete subclass,
        // so there is no instance state to serialize or deserialize.
        public void Deserialize(IXunitSerializationInfo info) { }
        public void Serialize(IXunitSerializationInfo info) { }
    }
}