using Microsoft.OpenApi.ApiManifest.Helpers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.OpenApi.ApiManifest;

public class AccessRequest
{
    private const string TypeProperty = "type";
    private const string ContentProperty = "content";

    public string? Type { get; set; }
    public JsonObject? Content { get; set; }
    public Dictionary<string, JsonObject?> AdditionalProperties { get; set; } = new Dictionary<string, JsonObject?>(StringComparer.OrdinalIgnoreCase);

    internal static AccessRequest Load(JsonElement content)
    {
        var accessRequest = new AccessRequest();
        ParsingHelpers.ParseMap(content, accessRequest, handlers);
        return accessRequest;

    }
    public void Write(Utf8JsonWriter writer)
    {
        ValidationHelpers.ThrowIfNull(writer, nameof(writer));
        writer.WriteStartObject();
        if (!String.IsNullOrWhiteSpace(Type)) writer.WriteString(TypeProperty, Type);
        if (Content != null)
        {
            writer.WritePropertyName(ContentProperty);
            writer.WriteRawValue(Content.ToJsonString());
        }
        if (AdditionalProperties.Count != 0)
        {
            foreach (var property in AdditionalProperties)
            {
                if (property.Value != null)
                {
                    writer.WritePropertyName(property.Key);
                    writer.WriteRawValue(property.Value.ToJsonString());
                }
            }
        }
        writer.WriteEndObject();
    }

    private static readonly FixedFieldMap<AccessRequest> handlers = new()
    {
        { TypeProperty, (o,v) => { o.Type = v.GetString(); } },
        { ContentProperty, (o,v) => { o.Content = JsonSerializer.Deserialize<JsonObject>(v); } },
        { Constants.AdditionalPropertiesProperty, (o,v) => { o.AdditionalProperties = ParsingHelpers.GetMap(v, (v) => JsonSerializer.Deserialize<JsonObject>(v)); } }
    };
}