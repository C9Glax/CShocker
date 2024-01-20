using System.Reflection.Metadata;
using CShocker.Ranges;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.Abstract;

public abstract class Shocker
{
    // ReSharper disable 4 times MemberCanBePrivate.Global external use
    public readonly List<string> ShockerIds;
    public readonly IntensityRange IntensityRange;
    public readonly DurationRange DurationRange;
    protected ILogger? Logger;
    public readonly ShockerApi ApiType;
    
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

    protected Shocker(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, ShockerApi apiType, ILogger? logger = null)
    {
        this.ShockerIds = shockerIds;
        this.IntensityRange = intensityRange;
        this.DurationRange = durationRange;
        this.ApiType = apiType;
        this.Logger = logger;
    }

    public void SetLogger(ILogger? logger)
    {
        this.Logger = logger;
    }

    public override string ToString()
    {
        return $"ShockerType: {Enum.GetName(typeof(ShockerApi), this.ApiType)}\n" +
               $"Shocker-IDs: {string.Join(", ", this.ShockerIds)}\n" +
               $"IntensityRange: {IntensityRange}\n" +
               $"DurationRange: {DurationRange}";
    }
}