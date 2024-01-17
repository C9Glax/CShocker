namespace CShocker.Ranges;

public abstract class RandomIntegerRange
{
    public short Min, Max;
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
}