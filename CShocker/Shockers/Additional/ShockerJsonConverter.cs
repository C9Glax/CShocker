using CShocker.Shockers.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CShocker.Shockers.Additional;

public class ShockerJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Shocker));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        if (jo.ContainsKey("model")) //OpenShockShocker
        {
            return jo.ToObject<OpenShockShocker>(serializer)!;
        }
        else //PiShockShocker
        {
            return jo.ToObject<PiShockShocker>(serializer)!;
        }
    }

    public override bool CanWrite => false;

    /// <summary>
    /// Don't call this
    /// </summary>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException("Dont call this");
    }
}