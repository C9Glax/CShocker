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
        ShockerApi? apiType = jo.SelectToken("ApiType")?.Value<ShockerApi>();

        switch (apiType)
        {
            case ShockerApi.OpenShockHttp:
                return new OpenShockHttp(
                    jo.SelectToken("ShockerIds")!.Value<List<string>>()!,
                    jo.SelectToken("IntensityRange")!.Value<IntensityRange>()!,
                    jo.SelectToken("DurationRange")!.Value<DurationRange>()!,
                    jo.SelectToken("Endpoint")!.Value<string>()!,
                    jo.SelectToken("ApiKey")!.Value<string>()!
                );
            case ShockerApi.OpenShockSerial:
            case ShockerApi.PiShockHttp:
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