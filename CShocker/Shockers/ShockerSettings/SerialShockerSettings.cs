using CShocker.Ranges;
using CShocker.Shockers.ShockerSettings.Abstract;

namespace CShocker.Shockers.ShockerSettings;

public record SerialShockerSettings(string[] ShockerIds, IntensityRange Intensity, DurationRange Duration) : AShockerSettings(ShockerIds, Intensity, Duration)
{
    
}