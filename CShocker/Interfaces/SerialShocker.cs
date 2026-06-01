using System.IO.Ports;
using CShocker.Helpers;

namespace CShocker.Interfaces;

public abstract class SerialShocker(string serialPortName, int serialPortBaudRate) : IShocker
{
    protected readonly SerialPort SerialPort = new(serialPortName, serialPortBaudRate);

    public abstract void Control(ControlAction action, int intensity, int duration);

    public ValueTask DisposeAsync()
    {
        try
        {
            SerialPort.Dispose();
            return ValueTask.CompletedTask;
        }
        catch (Exception exception)
        {
            return ValueTask.FromException(exception);
        }
    }
}