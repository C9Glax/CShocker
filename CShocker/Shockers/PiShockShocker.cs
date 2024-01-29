using CShocker.Shockers.Abstract;

namespace CShocker.Shockers;

public struct PiShockShocker : IShocker
{
    public string Code;

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
}