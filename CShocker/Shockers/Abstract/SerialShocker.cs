using CShocker.Shockers.ShockerSettings;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.Abstract;

public abstract class SerialShocker : Shocker
{
    protected SerialShocker(SerialShockerSettings shockerSettings, ILogger? logger = null) : base(shockerSettings, logger)
    {
        throw new NotImplementedException();
    }
}