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
    private readonly Queue<ValueTuple<ControlAction, Shocker, int, int>> _queue = new();
    private bool _workOnQueue = true;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly Thread _workQueueThread;
    private const short CommandDelay = 50;
    internal readonly IntegerRange ValidIntensityRange, ValidDurationRange;
    
    internal void Control(ControlAction action, int intensity, int duration, params Shocker[] shockers)
    {
        bool enqueueItem = true;
        if (action is ControlAction.Nothing)
        {
            this.Logger?.Log(LogLevel.Information, "No action defined.");
            enqueueItem = false;
        }
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
            _queue.Enqueue(new(action, shocker, intensity, duration));
        }
    }
    
    protected abstract void ControlInternal(ControlAction action, Shocker shocker, int intensity, int duration);

    protected Api(DeviceApi apiType, IntegerRange validIntensityRange, IntegerRange validDurationRange, ILogger? logger = null)
    {
        this.ApiType = apiType;
        this.Logger = logger;
        this.ValidIntensityRange = validIntensityRange;
        this.ValidDurationRange = validDurationRange;
        this._workQueueThread = new Thread(QueueThread);
        this._workQueueThread.Start();
    }

    private void QueueThread()
    {
        while (_workOnQueue)
            if (_queue.Count > 0 && _queue.Dequeue() is { } action)
            {
                this.Logger?.Log(LogLevel.Information, $"Executing: {Enum.GetName(action.Item1)} Intensity: {action.Item3} Duration: {action.Item4}\nShocker:\n{action.Item2.ToString()}");
                ControlInternal(action.Item1, action.Item2, action.Item3, action.Item4);
                Thread.Sleep(action.Item4 + CommandDelay);
            }
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
        _workOnQueue = false;
    }
}