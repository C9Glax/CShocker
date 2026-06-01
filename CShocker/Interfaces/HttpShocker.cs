using System.Net.Http.Headers;
using CShocker.Helpers;

namespace CShocker.Interfaces;

public abstract class HttpShocker : IShocker
{
    protected readonly HttpClient HttpClient = new();

    protected HttpShocker()
    {
        HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CShocker", "2.0"));
    }
    
    public abstract void Control(ControlAction action, int intensity, int duration);

    public ValueTask DisposeAsync()
    {
        try
        {
            HttpClient.Dispose();
            return ValueTask.CompletedTask;
        }
        catch (Exception exception)
        {
            return ValueTask.FromException(exception);
        }
    }
}