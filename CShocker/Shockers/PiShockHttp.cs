using CShocker.Shockers.Abstract;
using CShocker.Shockers.ShockerSettings;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers;

internal class PiShockHttp : HttpShocker
{
    public PiShockHttp(HttpShockerSettings settings, ILogger? logger = null) : base(settings, logger)
    {
        throw new NotImplementedException();
    }

    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        throw new NotImplementedException();
    }
}