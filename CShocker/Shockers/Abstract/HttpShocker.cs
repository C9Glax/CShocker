using CShocker.Ranges;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.Abstract;

public abstract class HttpShocker : Shocker
{
    protected readonly HttpClient HttpClient = new();
    public string Endpoint { get; init; }
    public string ApiKey { get; init; }

    protected HttpShocker(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, string endpoint, string apiKey, ShockerApi apiType, ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, apiType, logger)
    {
        Endpoint = endpoint;
        ApiKey = apiKey;
    }

    public override string ToString()
    {
        return $"{base.ToString()}\n" +
               $"Endpoint: {Endpoint}\n" +
               $"ApiKey: {ApiKey}";
    }
}