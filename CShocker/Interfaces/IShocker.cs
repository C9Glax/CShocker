using CShocker.Helpers;

namespace CShocker.Interfaces;

public interface IShocker : IAsyncDisposable
{
    public void Control(ControlAction action, int intensity, int duration);
}