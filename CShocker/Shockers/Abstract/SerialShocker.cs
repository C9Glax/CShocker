using CShocker.Ranges;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.Abstract;

public abstract class SerialShocker : Shocker
{
    protected SerialShocker(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, ShockerApi apiType, ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, apiType, logger)
    {
    }
}