using CShocker.Shockers.ShockerSettings.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.Abstract
;

internal abstract class Shocker
{
    protected readonly AShockerSettings ShockerSettings;
    protected readonly ILogger? Logger;

    internal enum ControlAction { Beep, Vibrate, Shock, Nothing }

    internal void Control(ControlAction action, string? shockerId = null, int? intensity = null, int? duration = null)
    {
        int i = intensity ?? ShockerSettings.Intensity.GetRandomRangeValue();
        int d = duration ?? ShockerSettings.Duration.GetRandomRangeValue();
        this.Logger?.Log(LogLevel.Information, $"{action} {(intensity is not null ? $"Overwrite {i}" : $"{i}")} {(duration is not null ? $"Overwrite {d}" : $"{d}")}");
        if (action is ControlAction.Nothing)
            return;
        if(shockerId is null)
            foreach (string shocker in ShockerSettings.ShockerIds)
                ControlInternal(action, shocker, i, d);
        else
            ControlInternal(action, shockerId, i, d);
    }
    
    protected abstract void ControlInternal(ControlAction action, string shockerId, int intensity, int duration);

    protected Shocker(AShockerSettings shockerSettings, ILogger? logger = null)
    {
        this.ShockerSettings = shockerSettings;
        this.Logger = logger;
    }
}