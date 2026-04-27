using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Converter for serialization and deserialization of Suggestion.
    /// For serialization:
    ///     - TermId is serialized as "termId".
    ///     - TermName is serialized as "termName".
    /// For deserialization:
    ///     - "term_id" maps to TermId.
    ///     - "name" maps to TermName.
    /// </summary>
    public class SuggestionConverter : JsonConverter<Suggestion>
    {
        /// <inheritdoc />
        public override Suggestion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                long termId = root.TryGetProperty("term_id", out JsonElement termIdElement)
                    ? long.Parse(termIdElement.GetString())
                    : 0;

                string termName = root.TryGetProperty("name", out JsonElement nameElement)
                    ? nameElement.GetString()
                    : null;

                return new Suggestion
                {
                    TermId = termId,
                    TermName = termName
                };
            }
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Suggestion value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("termId", value.TermId);
            writer.WriteString("termName", value.TermName);
            writer.WriteEndObject();
        }
    }
}
