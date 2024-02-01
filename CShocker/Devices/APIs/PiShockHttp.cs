using System.Net.Http.Headers;
using System.Text;
using CShocker.Devices.Abstract;
using CShocker.Devices.Additional;
using CShocker.Ranges;
using CShocker.Shockers;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Devices.APIs;

public class PiShockHttp : PiShockApi
{
    // ReSharper disable twice MemberCanBePrivate.Global external usage
    public string Username, Endpoint, ApiKey;
    protected readonly HttpClient HttpClient = new();

    public PiShockHttp(IntensityRange intensityRange, DurationRange durationRange, string apiKey, string username, string endpoint = "https://do.pishock.com/api/apioperate", ILogger? logger = null) : base(intensityRange, durationRange, DeviceApi.PiShockHttp, logger)
    {
        this.Username = username;
        this.Endpoint = endpoint;
        this.ApiKey = apiKey;
    }

    protected override void ControlInternal(ControlAction action, Shocker shocker, int intensity, int duration)
    {
        if (shocker is not PiShockShocker piShockShocker)
        {
            this.Logger?.Log(LogLevel.Warning, $"Shocker {shocker} is not {typeof(OpenShockShocker).FullName}");
            return;
        }
        
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
                                        $"\"Code\":\"{piShockShocker.Code}\"," +
                                        $"\"Intensity\":\"{intensity}\"," +
                                        $"\"Duration\":\"{duration/1000}\"," + //duration is in seconds no ms
                                        $"\"Apikey\":\"{ApiKey}\"," +
                                        $"\"Op\":\"{ControlActionToByte(action)}\"" +
                                        "}", Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
        };
        HttpResponseMessage response = HttpClient.Send(request);
        this.Logger?.Log(!response.IsSuccessStatusCode ? LogLevel.Error : LogLevel.Debug,
            $"{request.RequestUri} response: {response.StatusCode}");
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