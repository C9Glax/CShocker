namespace CShocker.Ranges;

public readonly struct IntegerRange
{
    // ReSharper disable twice MemberCanBePrivate.Global -> Exposed
    public readonly int Min, Max;

    public IntegerRange(int min, int max)
    {
        this.Min = min;
        this.Max = max;
    }

    public bool ValueWithinLimits(int value)
    {
        return value >= this.Min && value <= this.Max;
    }

    internal string RangeString()
    {
        return $"{this.Min}-{this.Max}";
    }
}