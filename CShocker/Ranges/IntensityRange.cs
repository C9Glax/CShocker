namespace CShocker.Ranges;

public class IntensityRange : RandomIntegerRange
{
    public IntensityRange(short min, short max) : base(min , max, 0, 100)
    {
        
    }

    public override bool Equals(object? obj)
    {
        return obj is IntensityRange && base.Equals(obj);
    }
}