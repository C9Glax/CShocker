namespace CShocker.Ranges;

public abstract class RandomIntegerRange
{
    public readonly short Min, Max;
    internal RandomIntegerRange(short min, short max, short minLimit, short maxLimit)
    {
        if (max - min < 0)
            throw new ArgumentException("Min has to be less or equal Max");
        if (min < minLimit || min > maxLimit)
            throw new ArgumentOutOfRangeException(nameof(min), $"Min has to be withing Range {minLimit}-{maxLimit}");
        if (max < minLimit || max > maxLimit)
            throw new ArgumentOutOfRangeException(nameof(max), $"Max has to be withing Range {minLimit}-{maxLimit}");
        this.Min = min;
        this.Max = max;
    }

    public int GetRandomRangeValue()
    {
        return Random.Shared.Next(this.Min, this.Max);
    }

    public override string ToString()
    {
        return $"Min: {Min} Max: {Max}";
    }

    public override bool Equals(object? obj)
    {
        return obj is RandomIntegerRange rir && Equals(rir);
    }

    private bool Equals(RandomIntegerRange other)
    {
        return Min == other.Min && Max == other.Max;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Min, Max);
    }
}