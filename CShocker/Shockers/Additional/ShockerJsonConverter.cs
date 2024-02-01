using CShocker.Devices.Abstract;
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
            return new OpenShockShocker(
                jo.SelectToken("api")!.ToObject<Api>()!,
                jo.SelectToken("name")!.Value<string>()!,
                jo.SelectToken("id")!.Value<string>()!,
                jo.SelectToken("rfId")!.Value<short>(),
                (OpenShockShocker.OpenShockModel)jo.SelectToken("model")!.Value<byte>(),
                jo.SelectToken("createdOn")!.Value<DateTime>(),
                jo.SelectToken("isPaused")!.Value<bool>()
            );
        }
        else //PiShockShocker
        {
            return new PiShockShocker(
                jo.SelectToken("api")!.ToObject<Api>()!,
                jo.SelectToken("Code")!.Value<string>()!
            );
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