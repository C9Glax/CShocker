using System.Net.Http.Headers;
using System.Text;
using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.APIS;

public class OpenShockHttp : HttpShocker
{
    
    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        HttpRequestMessage request = new (HttpMethod.Post, $"{Endpoint}/1/shockers/control")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("OpenCS2hock", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            },
            Content = new StringContent(@"[ { "+
                                        $"\"id\": \"{shockerId}\"," +
                                        $"\"type\": {ControlActionToByte(action)},"+
                                        $"\"intensity\": {intensity},"+
                                        $"\"duration\": {duration}"+
                                        "}]", Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
        };
        request.Headers.Add("OpenShockToken", ApiKey);
        HttpResponseMessage response = (HttpClient.Send(request));
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

    public OpenShockHttp(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, string endpoint, string apiKey, ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, endpoint, apiKey, ShockerApi.OpenShockHttp, logger)
    {
    }
}