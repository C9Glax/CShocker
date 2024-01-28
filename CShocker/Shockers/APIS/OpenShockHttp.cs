using System.Net.Http.Headers;
using System.Text;
using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CShocker.Shockers.APIS;

public class OpenShockHttp : HttpShocker
{

    public List<string> GetShockers()
    {
        HttpRequestMessage requestDevices = new (HttpMethod.Get, $"{Endpoint}/2/devices")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };
        requestDevices.Headers.Add("OpenShockToken", ApiKey);
        this.Logger?.Log(LogLevel.Debug, $"Requesting {requestDevices.RequestUri}");
        HttpResponseMessage responseDevices = HttpClient.Send(requestDevices);
        
        StreamReader deviceStreamReader = new(responseDevices.Content.ReadAsStream());
        string deviceJson = deviceStreamReader.ReadToEnd();
        this.Logger?.Log(!responseDevices.IsSuccessStatusCode ? LogLevel.Critical : LogLevel.Debug,
            $"{requestDevices.RequestUri} response: {responseDevices.StatusCode}\n{deviceJson}");
        JObject deviceListJObj = JObject.Parse(deviceJson);
        List<string> deviceIds = new();
        deviceIds.AddRange(deviceListJObj["data"]!.Children()["id"].Values<string>()!);

        List<string> shockerIds = new();
        foreach (string deviceId in deviceIds)
        {
            HttpRequestMessage requestShockers = new (HttpMethod.Get, $"{Endpoint}/2/devices/{deviceId}/shockers")
            {
                Headers =
                {
                    UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
            requestShockers.Headers.Add("OpenShockToken", ApiKey);
            this.Logger?.Log(LogLevel.Debug, $"Requesting {requestShockers.RequestUri}");
            HttpResponseMessage response = HttpClient.Send(requestShockers);
        
            StreamReader shockerStreamReader = new(response.Content.ReadAsStream());
            string shockerJson = shockerStreamReader.ReadToEnd();
            this.Logger?.Log(!response.IsSuccessStatusCode ? LogLevel.Critical : LogLevel.Debug,
                $"{requestShockers.RequestUri} response: {response.StatusCode}\n{shockerJson}");
            JObject shockerListJObj = JObject.Parse(shockerJson);
            shockerIds.AddRange(shockerListJObj["data"]!.Children()["id"].Values<string>()!);
            
        }
        return shockerIds;
    }
    
    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        HttpRequestMessage request = new (HttpMethod.Post, $"{Endpoint}/2/shockers/control")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Content = new StringContent("{" +
                                            "\"shocks\": ["+
                                                "{"+
                                                    $"\"id\": \"{shockerId}\"," +
                                                    $"\"type\": {ControlActionToByte(action)},"+
                                                    $"\"intensity\": {intensity},"+
                                                    $"\"duration\": {duration}"+
                                                "}" +
                                            "]," +
                                            "\"customName\": CShocker" +
                                        "}", Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
        };
        request.Headers.Add("OpenShockToken", ApiKey);
        this.Logger?.Log(LogLevel.Debug, $"Request-Content: {request.Content}");
        HttpResponseMessage response = HttpClient.Send(request);
        this.Logger?.Log(!response.IsSuccessStatusCode ? LogLevel.Critical : LogLevel.Debug,
            $"{request.RequestUri} response: {response.StatusCode}");
    }

    private byte ControlActionToByte(ControlAction action)
    {
        return action switch
        {
            ControlAction.Beep => 3,
            ControlAction.Vibrate => 2,
            ControlAction.Shock => 1,
            _ => 0
        };
    }

    public OpenShockHttp(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, string apiKey, string endpoint = "https://api.shocklink.net", ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, apiKey, endpoint, ShockerApi.OpenShockHttp, logger)
    {
    }
}