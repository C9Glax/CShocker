using CShocker.Ranges;

namespace CShocker.Shockers.ShockerSettings.Abstract;

public abstract record AShockerSettings(string[] ShockerIds, IntensityRange Intensity, DurationRange Duration)
{
    internal readonly string[] ShockerIds = ShockerIds;
    internal readonly IntensityRange Intensity = Intensity;
    internal readonly DurationRange Duration = Duration;
}