using CShocker.Ranges;
using CShocker.Shockers.ShockerSettings.Abstract;

namespace CShocker.Shockers.ShockerSettings;

public record HttpShockerSettings(string[] ShockerIds, IntensityRange Intensity, DurationRange Duration, string ApiKey, string Endpoint) : AShockerSettings(ShockerIds, Intensity, Duration)
{
    internal readonly HttpClient HttpClient = new ();
    internal readonly string ApiKey = ApiKey, Endpoint = Endpoint;
}