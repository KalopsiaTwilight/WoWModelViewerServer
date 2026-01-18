using ModelViewer.Core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ModelViewer.Core.JsonConverters
{
    public class SpellVisualKitEffectDataConverter : JsonConverter<SpellVisualKitEffectData>
    {
        public override SpellVisualKitEffectData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonDocument.TryParseValue(ref reader, out JsonDocument? doc))
            {
                var obj = doc.RootElement.EnumerateObject();
                while(obj.MoveNext())
                {
                    if (obj.Current.Name.ToLower() == nameof(SpellVisualKitEffectData.Type).ToLower())
                    {
                        var type = (SpellVisualKitEffectType) obj.Current.Value.GetInt32();
                        switch (type)
                        {
                            case SpellVisualKitEffectType.ModelAttach: return JsonSerializer.Deserialize<ModelAttachVisualKitEffectData>(doc, options);
                            case SpellVisualKitEffectType.Beam: return JsonSerializer.Deserialize<BeamVisualKitEffectData>(doc, options);
                            default: throw new Exception($"Unknown {nameof(SpellVisualKitEffectType)} encountered while parsing {nameof(SpellVisualKitEffectData)}");   
                        }
                    }
                }
                throw new Exception("Did not encounter a type for spell visual kit effect data.");
            }
            throw new JsonException("Could not parse effect as json document.");
        }

        public override void Write(Utf8JsonWriter writer, SpellVisualKitEffectData value, JsonSerializerOptions options)
        {
            if (value is ModelAttachVisualKitEffectData modelAttachEffect)
            {
                writer.WriteRawValue(JsonSerializer.Serialize(modelAttachEffect, options));
                return;
            }
            if (value is BeamVisualKitEffectData beamEffect)
            {
                writer.WriteRawValue(JsonSerializer.Serialize(beamEffect, options));
                return;
            }

            throw new Exception("Encountered unknown spell visual kit effect data type"); 
        }
    }
}
