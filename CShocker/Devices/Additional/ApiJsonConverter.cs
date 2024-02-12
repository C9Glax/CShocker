using CShocker.Devices.Abstract;
using CShocker.Devices.APIs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CShocker.Devices.Additional;

public class ApiJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Api));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        DeviceApi apiType = (DeviceApi)jo.SelectToken("ApiType")!.Value<byte>();
        
        switch (apiType)
        {
            case DeviceApi.OpenShockHttp:
                return jo.ToObject<OpenShockHttp>()!;
            case DeviceApi.OpenShockSerial:
                return jo.ToObject<OpenShockSerial>()!;
            case DeviceApi.PiShockHttp:
                return jo.ToObject<PiShockHttp>()!;
            case DeviceApi.PiShockSerial:
                return jo.ToObject<PiShockSerial>()!;
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