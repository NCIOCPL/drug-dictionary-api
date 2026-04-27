using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Converter for polymorphic deserialization of IDrugResource.
    /// </summary>
    public class DrugResourceConverter : JsonConverter<IDrugResource>
    {
        /// <inheritdoc />
        public override IDrugResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            JsonElement root = doc.RootElement;

            string type = null;
            if (root.TryGetProperty("type", out JsonElement typeElement))
            {
                type = typeElement.GetString();
            }

            string rawJson = root.GetRawText();
            return type?.ToLowerInvariant() switch
            {
                "drugalias" => JsonSerializer.Deserialize<DrugAlias>(rawJson, options),
                "drugterm" => JsonSerializer.Deserialize<DrugTerm>(rawJson, options),
                _ => throw new JsonException($"Unknown type value '{type}' for IDrugResource.")
            };
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, IDrugResource value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
