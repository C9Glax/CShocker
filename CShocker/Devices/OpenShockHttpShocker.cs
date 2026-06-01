using System.ComponentModel.DataAnnotations;
using CShocker.GeneratedClients.OpenShockClient.Api;
using CShocker.GeneratedClients.OpenShockClient.Model;
using CShocker.Helpers;
using CShocker.Interfaces;

namespace CShocker.Devices;

public sealed class OpenShockHttpShocker : HttpShocker
{
    private readonly ShockersApi _shockersApi;

    private Guid ShockerId { get; init; }
    
    private string ApiKey { get; init; }

    public OpenShockHttpShocker(Guid shockerId, string? apiKey = null)
    {
        ShockerId = shockerId;
        ApiKey = apiKey ?? EnvVars.OpenShockApiKey ?? throw new Exception("Missing OPENSHOCK_APIKEY");
        _shockersApi = new ShockersApi(HttpClient, EnvVars.OpenShockApiHost);
        HttpClient.DefaultRequestHeaders.Add("OpenShockToken", ApiKey);
    }
    
    public override void Control(ControlAction action, [Range(0,100)]int intensity, [Range(300, 65535)]int duration)
    {
        _shockersApi.ShockerSendControl(new ControlRequest([
            new Control(ShockerId, action switch
            {
                ControlAction.Beep => ControlType.Sound,
                ControlAction.Shock => ControlType.Shock,
                ControlAction.Vibrate => ControlType.Vibrate,
                _ => ControlType.Stop
            }, intensity, duration)
        ], EnvVars.Name));
    }
}