using System.ComponentModel.DataAnnotations;
using System.Net;
using CShocker.GeneratedClients.OpenShockClient.Api;
using CShocker.GeneratedClients.OpenShockClient.Client;
using CShocker.GeneratedClients.OpenShockClient.Model;
using CShocker.GeneratedClients.OpenShockClientv1.Model;
using CShocker.Helpers;
using CShocker.Interfaces;
using Control = CShocker.GeneratedClients.OpenShockClient.Model.Control;
using ControlType = CShocker.GeneratedClients.OpenShockClient.Model.ControlType;

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
        try
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
        catch (ApiException exception)
        {
            if(exception.ErrorCode == (int)HttpStatusCode.PreconditionFailed)
                Console.WriteLine("Shocker paused");
            else
                Console.WriteLine(exception);
        }
    }

    public static List<(string Owner, string Name, Guid Id)> GetAccessibleShockers(string? apiKey = null)
    {
        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("OpenShockToken", apiKey ?? EnvVars.OpenShockApiKey ?? throw new Exception("Missing OPENSHOCK_APIKEY"));
        GeneratedClients.OpenShockClientv1.Api.ShockersApi v1API = new (httpClient, EnvVars.OpenShockApiHost);
        
        DeviceWithShockersResponseArrayLegacyDataResponse resultOwn = v1API.ShockerListShockers();
        List<(string, string, Guid)> result = resultOwn.Data.SelectMany(d => d.Shockers.Select(s => new ValueTuple<string, string, Guid>("Own", s.Name, s.Id))).ToList();
        
        OwnerShockerResponseArrayLegacyDataResponse resultShared = v1API.ShockerListSharedShockers();
        result.AddRange(resultShared.Data.SelectMany(o => o.Devices.Select(s => new ValueTuple<string, string, Guid>(o.Name, s.Name, s.Id))));
        return result;
    }
}