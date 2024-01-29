using CShocker.Devices.Additional;
using CShocker.Ranges;
using Microsoft.Extensions.Logging;

namespace CShocker.Devices.Abstract;

public abstract class PiShockDevice : Device
{
    protected PiShockDevice(IntensityRange intensityRange, DurationRange durationRange, DeviceApi apiType, ILogger? logger = null) : base(intensityRange, durationRange, apiType, logger)
    {
    }
}