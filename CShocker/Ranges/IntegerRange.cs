namespace CShocker.Ranges;

public readonly struct IntegerRange
{
    public readonly int Min, Max;

    public IntegerRange(int min, int max)
    {
        this.Min = min;
        this.Max = max;
    }
}