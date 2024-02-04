using System.Net.Http.Headers;
using CShocker.Devices.Additional;
using CShocker.Ranges;
using CShocker.Shockers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace CShocker.Devices.Abstract;

public abstract class OpenShockApi : Api
{
    protected readonly HttpClient HttpClient = new();
    public string Endpoint { get; init; }
    public string ApiKey { get; init; }
    private const string DefaultEndpoint = "https://api.shocklink.net";

    public OpenShockApi(IntensityRange intensityRange, DurationRange durationRange, DeviceApi apiType, string apiKey, string endpoint = DefaultEndpoint, ILogger? logger = null) : base(intensityRange, durationRange, apiType, logger)
    {
        this.Endpoint = endpoint;
        this.ApiKey = apiKey;
    }

    public override bool Equals(object? obj)
    {
        return obj is OpenShockApi osd && Equals(osd);
    }

    private bool Equals(OpenShockApi other)
    {
        return base.Equals(other) && Endpoint == other.Endpoint && ApiKey == other.ApiKey;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Endpoint, ApiKey);
    }

    public List<OpenShockShocker> GetShockers()
    {
        return GetShockers(this.ApiKey, this, this.Endpoint, this.Logger);
    }

    public static List<OpenShockShocker> GetShockers(string apiKey, OpenShockApi api, string apiEndpoint = DefaultEndpoint, ILogger? logger = null)
    {
        List<OpenShockShocker> shockers = new();
            
        HttpClient httpClient = new();
        HttpRequestMessage requestOwnShockers = new (HttpMethod.Get, $"{apiEndpoint}/1/shockers/own")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };
        requestOwnShockers.Headers.Add("OpenShockToken", apiKey);
        logger?.Log(LogLevel.Debug, $"Requesting {requestOwnShockers.RequestUri}");
        HttpResponseMessage ownResponse = httpClient.Send(requestOwnShockers);
        logger?.Log(!ownResponse.IsSuccessStatusCode ? LogLevel.Error : LogLevel.Debug,
            $"{requestOwnShockers.RequestUri} response: {ownResponse.StatusCode}");
        if (!ownResponse.IsSuccessStatusCode)
            return shockers;
        
        StreamReader ownShockerStreamReader = new(ownResponse.Content.ReadAsStream());
        string ownShockerJson = ownShockerStreamReader.ReadToEnd();
        logger?.Log(LogLevel.Debug,ownShockerJson);
        JObject ownShockerListJObj = JObject.Parse(ownShockerJson);
        shockers.AddRange(ownShockerListJObj.SelectTokens("$.data..shockers[*]").Select(t =>
        {
            return new OpenShockShocker(api, t["name"]!.Value<string>()!, t["id"]!.Value<string>()!, t["rfId"]!.Value<short>(), Enum.Parse<OpenShockShocker.OpenShockModel>(t["model"]!.Value<string>()!), t["createdOn"]!.ToObject<DateTime>(), t["isPaused"]!.Value<bool>());
        }));
        
        HttpRequestMessage requestSharedShockers = new (HttpMethod.Get, $"{apiEndpoint}/1/shockers/shared")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };
        requestSharedShockers.Headers.Add("OpenShockToken", apiKey);
        logger?.Log(LogLevel.Debug, $"Requesting {requestSharedShockers.RequestUri}");
        HttpResponseMessage sharedResponse = httpClient.Send(requestSharedShockers);
        logger?.Log(!sharedResponse.IsSuccessStatusCode ? LogLevel.Error : LogLevel.Debug,
            $"{requestSharedShockers.RequestUri} response: {sharedResponse.StatusCode}");
        if (!sharedResponse.IsSuccessStatusCode)
            return shockers;
        
        StreamReader sharedShockerStreamReader = new(sharedResponse.Content.ReadAsStream());
        string sharedShockerJson = sharedShockerStreamReader.ReadToEnd();
        logger?.Log(LogLevel.Debug, sharedShockerJson);
        JObject sharedShockerListJObj = JObject.Parse(sharedShockerJson);
        shockers.AddRange(sharedShockerListJObj.SelectTokens("$.data..shockers[*]").Select(t => 
        {
            return new OpenShockShocker(api, t["name"]!.Value<string>()!, t["id"]!.Value<string>()!, t["rfId"]!.Value<short>(),Enum.Parse<OpenShockShocker.OpenShockModel>(t["model"]!.Value<string>()!), t["createdOn"]!.ToObject<DateTime>(), t["isPaused"]!.Value<bool>());
        }));
        return shockers;
    }
}