using System.Net.Http.Headers;
using CShocker.Devices.Additional;
using CShocker.Ranges;
using CShocker.Shockers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CShocker.Devices.Abstract;

public abstract class OpenShockDevice : Device
{
    protected readonly HttpClient HttpClient = new();
    public string Endpoint { get; init; }
    public string ApiKey { get; init; }

    public OpenShockDevice(IntensityRange intensityRange, DurationRange durationRange, DeviceApi apiType, string apiKey, string endpoint = "https://api.shocklink.net", ILogger? logger = null) : base(intensityRange, durationRange, apiType, logger)
    {
        this.Endpoint = endpoint;
        this.ApiKey = apiKey;
    }

    public override bool Equals(object? obj)
    {
        return obj is OpenShockDevice osd && Equals(osd);
    }

    private bool Equals(OpenShockDevice other)
    {
        return base.Equals(other) && Endpoint == other.Endpoint && ApiKey == other.ApiKey;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Endpoint, ApiKey);
    }

    public List<OpenShockShocker> GetShockers()
    {
        List<OpenShockShocker> shockers = new();
            
        HttpClient httpClient = new();
        HttpRequestMessage requestOwnShockers = new (HttpMethod.Get, $"{Endpoint}/1/shockers/own")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };
        requestOwnShockers.Headers.Add("OpenShockToken", ApiKey);
        this.Logger?.Log(LogLevel.Debug, $"Requesting {requestOwnShockers.RequestUri}");
        HttpResponseMessage ownResponse = httpClient.Send(requestOwnShockers);
        this.Logger?.Log(!ownResponse.IsSuccessStatusCode ? LogLevel.Error : LogLevel.Debug,
            $"{requestOwnShockers.RequestUri} response: {ownResponse.StatusCode}");
        if (!ownResponse.IsSuccessStatusCode)
            return shockers;
        
        StreamReader ownShockerStreamReader = new(ownResponse.Content.ReadAsStream());
        string ownShockerJson = ownShockerStreamReader.ReadToEnd();
        this.Logger?.Log(LogLevel.Debug,ownShockerJson);
        JObject ownShockerListJObj = JObject.Parse(ownShockerJson);
        shockers.AddRange(ownShockerListJObj.SelectTokens("$.data..shockers[*]").Select(t => t.ToObject<OpenShockShocker>()));
        
        HttpRequestMessage requestSharedShockers = new (HttpMethod.Get, $"{Endpoint}/1/shockers/shared")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };
        requestSharedShockers.Headers.Add("OpenShockToken", ApiKey);
        this.Logger?.Log(LogLevel.Debug, $"Requesting {requestSharedShockers.RequestUri}");
        HttpResponseMessage sharedResponse = httpClient.Send(requestSharedShockers);
        this.Logger?.Log(!sharedResponse.IsSuccessStatusCode ? LogLevel.Error : LogLevel.Debug,
            $"{requestSharedShockers.RequestUri} response: {sharedResponse.StatusCode}");
        if (!sharedResponse.IsSuccessStatusCode)
            return shockers;
        
        StreamReader sharedShockerStreamReader = new(sharedResponse.Content.ReadAsStream());
        string sharedShockerJson = sharedShockerStreamReader.ReadToEnd();
        this.Logger?.Log(LogLevel.Debug, sharedShockerJson);
        JObject sharedShockerListJObj = JObject.Parse(sharedShockerJson);
        shockers.AddRange(sharedShockerListJObj.SelectTokens("$.data..shockers[*]").Select(t => t.ToObject<OpenShockShocker>()));
        return shockers;
    }
}