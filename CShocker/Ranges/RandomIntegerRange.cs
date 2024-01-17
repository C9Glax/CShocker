namespace CShocker.Ranges;

public abstract class RandomIntegerRange
{
    public Range Range { get; init; }
    internal RandomIntegerRange(Range range, Range limits)
    {
        if (range.Max - range.Min < 0)
            throw new ArgumentException("Min has to be less or equal Max");
        if (range.Min < limits.Min || range.Min > limits.Max)
            throw new ArgumentOutOfRangeException(nameof(limits.Min), "Min has to be withing Range 0-100");
        if (range.Max < limits.Min || range.Max > limits.Max)
            throw new ArgumentOutOfRangeException(nameof(range.Max), "Max has to be withing Range 0-100");
        this.Range = range;
    }

    public int GetRandomRangeValue()
    {
        return Random.Shared.Next(Range.Min, Range.Max);
    }
}