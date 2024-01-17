using CShocker.Ranges;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.Abstract;

public abstract class HttpShocker : Shocker
{
    protected readonly HttpClient HttpClient = new();
    protected string Endpoint { get; init; }
    protected string ApiKey { get; init; }

    protected HttpShocker(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, string endpoint, string apiKey, ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, logger)
    {
        Endpoint = endpoint;
        ApiKey = apiKey;
    }
}