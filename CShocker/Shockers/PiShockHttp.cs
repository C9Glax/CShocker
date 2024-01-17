using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers;

public class PiShockHttp : HttpShocker
{
    public PiShockHttp(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, string endpoint, string apiKey, ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, endpoint, apiKey, logger)
    {
        throw new NotImplementedException();
    }

    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        throw new NotImplementedException();
    }
}