namespace CShocker.Ranges;

public class DurationRange : RandomIntegerRange
{
    public DurationRange(short min, short max) : base(min ,max , 0, 30000)
    {
        
    }
    
    public override bool Equals(object? obj)
    {
        return obj is IntensityRange && base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine("DR", base.GetHashCode());
    }
}