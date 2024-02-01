using System.Net.Http.Headers;
using System.Text;
using CShocker.Devices.Abstract;
using CShocker.Devices.Additional;
using CShocker.Ranges;
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

        HttpRequestMessage request = new (HttpMethod.Post, $"{Endpoint}/2/shockers/control")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Content = new StringContent("{" +
                                        "  \"shocks\": [" +
                                        "    {" +
                                        $"      \"id\": \"{openShockShocker.id}\"," +
                                        $"      \"type\": {ControlActionToByte(action)}," +
                                        $"      \"intensity\": {intensity}," +
                                        $"      \"duration\": {duration}" +
                                        "    }" +
                                        "  ]," +
                                        "  \"customName\": \"CShocker\"" +
                                        "}", Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
        };
        request.Headers.Add("OpenShockToken", ApiKey);
        this.Logger?.Log(LogLevel.Debug, $"Request-Content: {request.Content.ReadAsStringAsync().Result}");
        HttpResponseMessage response = HttpClient.Send(request);
        this.Logger?.Log(!response.IsSuccessStatusCode ? LogLevel.Error : LogLevel.Debug,
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

    public OpenShockHttp(IntensityRange intensityRange, DurationRange durationRange, string apiKey, string endpoint = "https://api.shocklink.net", ILogger? logger = null) : base(intensityRange, durationRange, DeviceApi.OpenShockHttp, apiKey, endpoint, logger)
    {
    }
}