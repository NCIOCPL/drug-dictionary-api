using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Converter for deserialization of DrugInfoSummaryLink.
    /// For serialization:
    ///     - The Uri property is read from Elasticsearch as "url" but output on the front end as "uri".
    ///     - The Text appears as "text" in both contexts.
    /// No special handling is needed for deserialization, the application is expected to
    /// set the correct NamingPolicy for deserializing from Elasticsearch.
    /// </summary>
    public class DrugInfoSummaryLinkConverter : JsonConverter<DrugInfoSummaryLink>
    {
        /// <inheritdoc />
        public override DrugInfoSummaryLink Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // No name conversion is needed for deserialization, the application is expected to
            // set the correct NamingPolicy for deserializing from Elasticsearch.
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                string GetString(string propertyName) =>
                    root.TryGetProperty(propertyName, out JsonElement element) ? element.GetString() : null;

                string url = GetString("url");

                return new DrugInfoSummaryLink
                {
                    URI = (url != null) ? new Uri(url) : null,
                    Text = GetString("text")
                };
            }
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, DrugInfoSummaryLink value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("uri", value.URI?.ToString());
            writer.WriteString("text", value.Text);
            writer.WriteEndObject();
        }
    }
}