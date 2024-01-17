namespace CShocker.Ranges;

public class IntensityRange : RandomIntegerRange
{
    public IntensityRange(Range range) : base(range, new Range(0, 100))
    {
        
    }
}