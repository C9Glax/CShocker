using CShocker.Devices.Abstract;
using CShocker.Ranges;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CShocker.Devices.Additional;

public class DeviceJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Device));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        DeviceApi? apiType = (DeviceApi?)jo.SelectToken("ApiType")?.Value<byte>();

        switch (apiType)
        {
            case DeviceApi.OpenShockHttp:
                return new OpenShockHttp(
                    jo.SelectToken("IntensityRange")!.ToObject<IntensityRange>()!,
                    jo.SelectToken("DurationRange")!.ToObject<DurationRange>()!,
                    jo.SelectToken("ApiKey")!.Value<string>()!,
                    jo.SelectToken("Endpoint")!.Value<string>()!
                );
            case DeviceApi.OpenShockSerial:
                return new OpenShockSerial(
                    jo.SelectToken("IntensityRange")!.ToObject<IntensityRange>()!,
                    jo.SelectToken("DurationRange")!.ToObject<DurationRange>()!,
                    jo.SelectToken("SerialPortI")!.ToObject<SerialPortInfo>()!,
                    jo.SelectToken("ApiKey")!.Value<string>()!,
                    jo.SelectToken("Endpoint")!.Value<string>()!
                );
            case DeviceApi.PiShockHttp:
                return new PiShockHttp(
                    jo.SelectToken("IntensityRange")!.ToObject<IntensityRange>()!,
                    jo.SelectToken("DurationRange")!.ToObject<DurationRange>()!,
                    jo.SelectToken("ApiKey")!.Value<string>()!,
                    jo.SelectToken("Username")!.Value<string>()!,
                    jo.SelectToken("Endpoint")!.Value<string>()!
                );
            case DeviceApi.PiShockSerial:
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