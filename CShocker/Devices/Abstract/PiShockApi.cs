using CShocker.Devices.Additional;
using CShocker.Ranges;
using Microsoft.Extensions.Logging;

namespace CShocker.Devices.Abstract;

public abstract class PiShockApi : Api
{
    protected PiShockApi(DeviceApi apiType, ILogger? logger = null) : base(apiType, new IntegerRange(0, 100), new IntegerRange(1000, 15000), logger)
    {
    }
}