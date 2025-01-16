using CShocker.Devices.Additional;
using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Devices.Abstract;

public abstract class Api : IDisposable
{
    // ReSharper disable 4 times MemberCanBePrivate.Global -> Exposed
    protected ILogger? Logger;
    public readonly DeviceApi ApiType;
    private Queue<DateTime> order = new();
    private Dictionary<DateTime, Task> tasks = new();
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private const short CommandDelay = 50;
    internal readonly IntegerRange ValidIntensityRange, ValidDurationRange;
    
    internal void Control(ControlAction action, int intensity, int duration, params Shocker[] shockers)
    {
        bool enqueueItem = true;
        if (!ValidIntensityRange.IsValueWithinLimits(intensity))
        {
            this.Logger?.Log(LogLevel.Information, $"Value not within allowed {nameof(intensity)}-Range ({ValidIntensityRange.RangeString()}): {intensity}");
            enqueueItem = false;
        }
        if (!ValidDurationRange.IsValueWithinLimits(duration))
        {
            this.Logger?.Log(LogLevel.Information, $"Value not within allowed {nameof(duration)}-Range ({ValidIntensityRange.RangeString()}): {duration}");
            enqueueItem = false;
        }
        if (!enqueueItem)
        {
            this.Logger?.Log(LogLevel.Information, "Doing nothing.");
            return;
        }
        foreach (Shocker shocker in shockers)
        {
            this.Logger?.Log(LogLevel.Debug, $"Enqueueing {action} Intensity: {intensity} Duration: {duration}\nShocker:\n{shocker}");
            ValueTuple<ControlAction, Shocker, int, int> tuple = new(action, shocker, intensity, duration);
            DateTime now = DateTime.Now;
            Task t = new (() => ExecuteTask(now, tuple));
            order.Enqueue(now);
            tasks.Add(now, t);
            t.Start();
        }
    }
    
    protected abstract void ControlInternal(ControlAction action, Shocker shocker, int intensity, int duration);

    protected Api(DeviceApi apiType, IntegerRange validIntensityRange, IntegerRange validDurationRange, ILogger? logger = null)
    {
        this.ApiType = apiType;
        this.Logger = logger;
        this.ValidIntensityRange = validIntensityRange;
        this.ValidDurationRange = validDurationRange;
    }

    private void ExecuteTask(DateTime when, ValueTuple<ControlAction, Shocker, int, int> tuple)
    {
        while (order.First() != when)
            Thread.Sleep(CommandDelay);
        this.Logger?.Log(LogLevel.Information, $"Executing: {Enum.GetName(tuple.Item1)} Intensity: {tuple.Item3} Duration: {tuple.Item4}\nShocker:\n{tuple.Item2}");
        ControlInternal(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        Thread.Sleep(tuple.Item4);
        tasks.Remove(when);
        order.Dequeue();
    }

    public void SetLogger(ILogger? logger)
    {
        this.Logger = logger;
    }

    public override string ToString()
    {
        return $"ShockerType: {Enum.GetName(typeof(DeviceApi), this.ApiType)}\n\r";
    }

    public override bool Equals(object? obj)
    {
        return obj is Api d && Equals(d);
    }

    protected bool Equals(Api other)
    {
        return ApiType == other.ApiType;
    }

    public override int GetHashCode()
    {
        return ApiType.GetHashCode();
    }

    public void Dispose()
    {
        foreach ((DateTime when, Task? task) in tasks)
            task?.Dispose();
    }
}