using CShocker.Devices.Abstract;
using CShocker.Devices.Additional;

namespace CShocker.Shockers.Abstract;

public abstract class Shocker : IDisposable
{
    // ReSharper disable once MemberCanBePrivate.Global -> Exposed
    public Api Api { get; }

    internal Shocker(Api api)
    {
        this.Api = api;
    }

    public void Control(ControlAction action, int intensity, int duration)
    {
        this.Api.Control(action, intensity, duration, this);
    }

    public void Dispose()
    {
        Api.Dispose();
    }
}