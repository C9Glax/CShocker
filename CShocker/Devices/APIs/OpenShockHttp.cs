using CShocker.Devices.Abstract;
using CShocker.Devices.Additional;
using CShocker.Shockers;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Devices.APIs;

public class OpenShockHttp : OpenShockApi
{
    protected override void ControlInternal(ControlAction action, Shocker shocker, int intensity, int duration)
    {
        if (shocker is not OpenShockShocker openShockShocker)
        {
            this.Logger?.Log(LogLevel.Warning, $"Shocker {shocker} is not {typeof(OpenShockShocker).FullName}");
            return;
        }

        string json = "{" +
                      "  \"shocks\": [" +
                      "    {" +
                      $"      \"id\": \"{openShockShocker.ID}\"," +
                      $"      \"type\": \"{Enum.GetName(action)}\"," +
                      $"      \"intensity\": {intensity}," +
                      $"      \"duration\": {duration}" +
                      "    }" +
                      "  ]," +
                      "  \"customName\": \"CShocker\"" +
                      "}";
        
        ApiHttpClient.MakeAPICall(HttpMethod.Post, $"{Endpoint}/2/shockers/control", json, this.Logger, new ValueTuple<string, string>("OpenShockToken", ApiKey));
    }

    public OpenShockHttp(string apiKey, string? endpoint = null, ILogger? logger = null) : base(DeviceApi.OpenShockHttp, apiKey, endpoint??DefaultEndpoint, logger)
    {
    }
}