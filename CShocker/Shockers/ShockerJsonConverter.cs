using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using CShocker.Shockers.APIS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CShocker.Shockers;

public class ShockerJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Shocker));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        ShockerApi? apiType = (ShockerApi?)jo.SelectToken("ApiType")?.Value<byte>();

        switch (apiType)
        {
            case ShockerApi.OpenShockHttp:
                return new OpenShockHttp(
                    jo.SelectToken("ShockerIds")!.ToObject<List<string>>()!,
                    jo.SelectToken("IntensityRange")!.ToObject<IntensityRange>()!,
                    jo.SelectToken("DurationRange")!.ToObject<DurationRange>()!,
                    jo.SelectToken("ApiKey")!.Value<string>()!,
                    jo.SelectToken("Endpoint")!.Value<string>()!
                );
            case ShockerApi.OpenShockSerial:
            case ShockerApi.PiShockHttp:
                return new PiShockHttp(
                    jo.SelectToken("ShockerIds")!.ToObject<List<string>>()!,
                    jo.SelectToken("IntensityRange")!.ToObject<IntensityRange>()!,
                    jo.SelectToken("DurationRange")!.ToObject<DurationRange>()!,
                    jo.SelectToken("ApiKey")!.Value<string>()!,
                    jo.SelectToken("Username")!.Value<string>()!,
                    jo.SelectToken("ShareCode")!.Value<string>()!,
                    jo.SelectToken("Endpoint")!.Value<string>()!
                );
            case ShockerApi.PiShockSerial:
                throw new NotImplementedException();
            default:
                throw new Exception();
        }
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