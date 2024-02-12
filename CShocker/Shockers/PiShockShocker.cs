using CShocker.Devices.Abstract;
using CShocker.Shockers.Abstract;

namespace CShocker.Shockers;

public class PiShockShocker : Shocker
{
    public readonly string Code;

    public override bool Equals(object? obj)
    {
        return obj is PiShockShocker pss && Equals(pss);
    }

    private bool Equals(PiShockShocker other)
    {
        return Code == other.Code;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }

    public PiShockShocker(Api api, string code) : base(api)
    {
        if (api is not PiShockApi)
            throw new Exception($"API-Type {api.GetType().FullName} is not usable with Shocker {this.GetType().FullName}");
        Code = code;
    }
}