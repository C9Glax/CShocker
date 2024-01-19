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
        HttpRequestMessage requestDevices = new (HttpMethod.Post, $"{Endpoint}/2/devices")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };
        requestDevices.Headers.Add("OpenShockToken", ApiKey);
        HttpResponseMessage responseDevices = HttpClient.Send(requestDevices);
        
        StreamReader deviceStreamReader = new(responseDevices.Content.ReadAsStream());
        string deviceJson = deviceStreamReader.ReadToEnd();
        this.Logger?.Log(LogLevel.Debug, $"{requestDevices.RequestUri} response: {responseDevices.StatusCode}\n{deviceJson}");
        JObject deviceListJObj = JObject.Parse(deviceJson);
        List<string> deviceIds = new();
        foreach(JToken device in deviceListJObj.SelectTokens("data"))
            deviceIds.Add(device.SelectToken("id")!.Value<string>()!);

        List<string> shockerIds = new();
        foreach (string deviceId in deviceIds)
        {
            HttpRequestMessage requestShockers = new (HttpMethod.Post, $"{Endpoint}/2/devices/{deviceId}/shockers")
            {
                Headers =
                {
                    UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
            requestShockers.Headers.Add("OpenShockToken", ApiKey);
            HttpResponseMessage response = HttpClient.Send(requestShockers);
        
            StreamReader shockerStreamReader = new(response.Content.ReadAsStream());
            string shockerJson = shockerStreamReader.ReadToEnd();
            this.Logger?.Log(LogLevel.Debug, $"{requestShockers.RequestUri} response: {response.StatusCode}\n{shockerJson}");
            JObject shockerListJObj = JObject.Parse(shockerJson);
            foreach(JToken shocker in shockerListJObj.SelectTokens("data"))
                shockerIds.Add(shocker.SelectToken("id")!.Value<string>()!);
            
        }
        return shockerIds;
    }
    
    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        HttpRequestMessage request = new (HttpMethod.Post, $"{Endpoint}/1/shockers/control")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Content = new StringContent("{ shocks: ["+
                                            "{ "+
                                                $"\"id\": \"{shockerId}\"," +
                                                $"\"type\": {ControlActionToByte(action)},"+
                                                $"\"intensity\": {intensity},"+
                                                $"\"duration\": {duration}"+
                                            "}" +
                                            "]," +
                                            "customName: CShocker" +
                                        "}", Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
        };
        request.Headers.Add("OpenShockToken", ApiKey);
        HttpResponseMessage response = HttpClient.Send(request);
        this.Logger?.Log(LogLevel.Debug, $"{request.RequestUri} response: {response.StatusCode}");
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

    public OpenShockHttp(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, string apiKey, string endpoint = "https://api.shocklink.net", ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, endpoint, apiKey, ShockerApi.OpenShockHttp, logger)
    {
    }
}