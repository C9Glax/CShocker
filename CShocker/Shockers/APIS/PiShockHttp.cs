using System.Net.Http.Headers;
using System.Text;
using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.APIS;

public class PiShockHttp : HttpShocker
{
    public String Username, ShareCode;
    public PiShockHttp(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, string apiKey, string username, string shareCode, string endpoint = "https://do.pishock.com/api/apioperate", ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, endpoint, apiKey, ShockerApi.PiShockHttp, logger)
    {
        this.Username = username;
        this.ShareCode = shareCode;
    }

    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        HttpRequestMessage request = new (HttpMethod.Post, $"{Endpoint}")
        {
            Headers =
            {
                UserAgent = { new ProductInfoHeaderValue("CShocker", "1") },
                Accept = { new MediaTypeWithQualityHeaderValue("text/plain") }
            },
            Content = new StringContent("{" +
                                            $"\"Username\":\"{Username}\"," +
                                            "\"Name\":\"CShocker\"," +
                                            $"\"Code\":\"{ShareCode}\"," +
                                            $"\"Intensity\":\"{intensity}\"," +
                                            $"\"Duration\":\"{duration/1000}\"," + //duration is in seconds no ms
                                            $"\"Apikey\":\"{ApiKey}\"," +
                                            $"\"Op\":\"{ControlActionToByte(action)}\"" +
                                        "}", Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
        };
        HttpResponseMessage response = HttpClient.Send(request);
        this.Logger?.Log(LogLevel.Debug, $"{request.RequestUri} response: {response.StatusCode} {response.Content.ReadAsStringAsync()}");
    }
    
    private byte ControlActionToByte(ControlAction action)
    {
        return action switch
        {
            ControlAction.Beep => 2,
            ControlAction.Vibrate => 1,
            ControlAction.Shock => 0,
            _ => 2
        };
    }
}