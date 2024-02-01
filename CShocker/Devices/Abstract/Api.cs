using CShocker.Devices.Additional;
using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Devices.Abstract;

public abstract class Api : IDisposable
{
    // ReSharper disable 4 times MemberCanBePrivate.Global external use
    public readonly IntensityRange IntensityRange;
    public readonly DurationRange DurationRange;
    protected ILogger? Logger;
    public readonly DeviceApi ApiType;
    private readonly Queue<ValueTuple<ControlAction, Shocker, int, int>> _queue = new();
    private bool _workQueue = true;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly Thread _workQueueThread;
    private const short CommandDelay = 50;
    
    internal void Control(ControlAction action, int? intensity = null, int? duration = null, params Shocker[] shockers)
    {
        int i = intensity ?? IntensityRange.GetRandomRangeValue();
        int d = duration ?? DurationRange.GetRandomRangeValue();
        if (action is ControlAction.Nothing)
        {
            this.Logger?.Log(LogLevel.Information, "Doing nothing");
            return;
        }
        foreach (Shocker shocker in shockers)
        {
            this.Logger?.Log(LogLevel.Debug, $"Enqueueing {action} {(intensity is not null ? $"Overwrite {i}" : $"{i}")} {(duration is not null ? $"Overwrite {d}" : $"{d}")}");
            _queue.Enqueue(new(action, shocker, i ,d));
        }
    }
    
    protected abstract void ControlInternal(ControlAction action, Shocker shocker, int intensity, int duration);

    protected Api(IntensityRange intensityRange, DurationRange durationRange, DeviceApi apiType, ILogger? logger = null)
    {
        this.IntensityRange = intensityRange;
        this.DurationRange = durationRange;
        this.ApiType = apiType;
        this.Logger = logger;
        this._workQueueThread = new Thread(QueueThread);
        this._workQueueThread.Start();
    }

    private void QueueThread()
    {
        while (_workQueue)
            if (_queue.Count > 0 && _queue.Dequeue() is { } action)
            {
                this.Logger?.Log(LogLevel.Information, $"{action.Item1} {action.Item2} {action.Item3} {action.Item4}");
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
        return $"ShockerType: {Enum.GetName(typeof(DeviceApi), this.ApiType)}\n" +
               $"IntensityRange: {IntensityRange}\n" +
               $"DurationRange: {DurationRange}\n\r";
    }

    public override bool Equals(object? obj)
    {
        return obj is Api d && Equals(d);
    }

    protected bool Equals(Api other)
    {
        return IntensityRange.Equals(other.IntensityRange) && DurationRange.Equals(other.DurationRange) && ApiType == other.ApiType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IntensityRange, DurationRange, (int)ApiType);
    }

    public void Dispose()
    {
        _workQueue = false;
    }
}