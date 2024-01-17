using CShocker.Ranges;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.Abstract;

public abstract class Shocker
{
    protected readonly List<string> ShockerIds;
    protected readonly IntensityRange IntensityRange;
    protected readonly DurationRange DurationRange;
    protected readonly ILogger? Logger;
    
    public void Control(ControlAction action, string? shockerId = null, int? intensity = null, int? duration = null)
    {
        int i = intensity ?? IntensityRange.GetRandomRangeValue();
        int d = duration ?? DurationRange.GetRandomRangeValue();
        this.Logger?.Log(LogLevel.Information, $"{action} {(intensity is not null ? $"Overwrite {i}" : $"{i}")} {(duration is not null ? $"Overwrite {d}" : $"{d}")}");
        if (action is ControlAction.Nothing)
            return;
        if(shockerId is null)
            foreach (string shocker in ShockerIds)
                ControlInternal(action, shocker, i, d);
        else
            ControlInternal(action, shockerId, i, d);
    }
    
    protected abstract void ControlInternal(ControlAction action, string shockerId, int intensity, int duration);

    protected Shocker(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, ILogger? logger = null)
    {
        this.ShockerIds = shockerIds;
        this.IntensityRange = intensityRange;
        this.DurationRange = durationRange;
        this.Logger = logger;
    }
}