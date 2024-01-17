namespace CShocker.Ranges;

public class DurationRange : RandomIntegerRange
{
    public DurationRange(Range range) : base(range, new Range(0, 30000))
    {
        
    }
}