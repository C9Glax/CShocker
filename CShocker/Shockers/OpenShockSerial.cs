using CShocker.Shockers.Abstract;
using CShocker.Shockers.ShockerSettings;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers;

internal class OpenShockSerial : SerialShocker
{
    public OpenShockSerial(SerialShockerSettings shockerSettings, ILogger? logger = null) : base(shockerSettings, logger)
    {
        throw new NotImplementedException();
    }

    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        throw new NotImplementedException();
    }
}