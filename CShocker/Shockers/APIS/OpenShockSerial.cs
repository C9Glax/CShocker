using System.Net.Http.Headers;
using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CShocker.Shockers.APIS;

public class OpenShockSerial : SerialShocker
{
    // ReSharper disable once MemberCanBePrivate.Global external usage
    public readonly Dictionary<string, ShockerModel> Model;
    private const int BaudRate = 115200;
    public OpenShockSerial(Dictionary<string, ShockerModel> shockerIds, IntensityRange intensityRange, DurationRange durationRange, SerialPortInfo serialPortI, ILogger? logger = null) : base(shockerIds.Keys.ToList(), intensityRange, durationRange, serialPortI, BaudRate, ShockerApi.OpenShockSerial, logger)
    {
        this.Model = shockerIds;
    }

    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        string json = "rftransmit {" +
                        $"\"model\":\"{Enum.GetName(Model[shockerId])!.ToLower()}\"," +
                        $"\"id\":{shockerId}," +
                        $"\"type\":\"{ControlActionToString(action)}\"," +
                        $"\"intensity\":{intensity}," +
                        $"\"durationMs\":{duration}" +
                      "}";
        SerialPort.WriteLine(json);
    }

    public Dictionary<string, ShockerModel> GetShockers(string apiEndpoint, string apiKey)
    {
        HttpClient httpClient = new();
        HttpRequestMessage requestDevices = new (HttpMethod.Get, $"{apiEndpoint}/2/devices")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };
        requestDevices.Headers.Add("OpenShockToken", apiKey);
        HttpResponseMessage responseDevices = httpClient.Send(requestDevices);
        
        StreamReader deviceStreamReader = new(responseDevices.Content.ReadAsStream());
        string deviceJson = deviceStreamReader.ReadToEnd();
        this.Logger?.Log(LogLevel.Debug, $"{requestDevices.RequestUri} response: {responseDevices.StatusCode}\n{deviceJson}");
        JObject deviceListJObj = JObject.Parse(deviceJson);
        List<string> deviceIds = new();
        deviceIds.AddRange(deviceListJObj["data"]!.Children()["id"].Values<string>()!);

        Dictionary<string, ShockerModel> models = new();
        foreach (string deviceId in deviceIds)
        {
            HttpRequestMessage requestShockers = new (HttpMethod.Get, $"{apiEndpoint}/2/devices/{deviceId}/shockers")
            {
                Headers =
                {
                    UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
            requestShockers.Headers.Add("OpenShockToken", apiKey);
            HttpResponseMessage response = httpClient.Send(requestShockers);
        
            StreamReader shockerStreamReader = new(response.Content.ReadAsStream());
            string shockerJson = shockerStreamReader.ReadToEnd();
            this.Logger?.Log(LogLevel.Debug, $"{requestShockers.RequestUri} response: {response.StatusCode}\n{shockerJson}");
            JObject shockerListJObj = JObject.Parse(shockerJson);
            for (int i = 0; i < shockerListJObj["data"]!.Children().Count(); i++)
            {
                models.Add(
                    shockerListJObj["data"]![i]!["rfId"]!.Value<int>().ToString(),
                    Enum.Parse<ShockerModel>(shockerListJObj["data"]![i]!["model"]!.Value<string>()!)
                    );
            }
        }

        return models;
    }

    public enum ShockerModel : byte
    {
        CaiXianlin = 0,
        Petrainer = 1
    }
    
    private static string ControlActionToString(ControlAction action)
    {
        return action switch
        {
            ControlAction.Beep => "sound",
            ControlAction.Vibrate => "vibrate",
            ControlAction.Shock => "shock",
            _ => "stop"
        };
    }
}