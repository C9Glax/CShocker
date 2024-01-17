namespace CShocker.Ranges;

public class DurationRange : RandomIntegerRange
{
    public DurationRange(short min, short max) : base(min ,max , 0, 30000)
    {
        
    }
}