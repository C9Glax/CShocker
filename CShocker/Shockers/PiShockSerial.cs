using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers;

public class PiShockSerial : SerialShocker
{
    public PiShockSerial(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, logger)
    {
        throw new NotImplementedException();
    }

    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        throw new NotImplementedException();
    }
}