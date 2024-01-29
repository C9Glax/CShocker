using CShocker.Shockers.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CShocker.Shockers.Additional;

public class ShockerJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(IShocker));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        if (jo.ContainsKey("model")) //OpenShockShocker
        {
            return new OpenShockShocker()
            {
                name = jo.SelectToken("name")!.Value<string>()!,
                id = jo.SelectToken("id")!.Value<string>()!,
                rfId = jo.SelectToken("rfId")!.Value<short>(),
                model = (OpenShockShocker.OpenShockModel)jo.SelectToken("model")!.Value<byte>(),
                createdOn = jo.SelectToken("createdOn")!.Value<DateTime>(),
                isPaused = jo.SelectToken("isPaused")!.Value<bool>()
            };
        }
        else //PiShockShocker
        {
            return new PiShockShocker()
            {
                Code = jo.SelectToken("Code")!.Value<string>()!
            };
        }
        throw new Exception();
    }

    public override bool CanWrite => false;

    /// <summary>
    /// Don't call this
    /// </summary>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new Exception("Dont call this");
    }
}