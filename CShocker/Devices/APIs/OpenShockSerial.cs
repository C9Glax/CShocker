using System.IO.Ports;
using CShocker.Devices.Abstract;
using CShocker.Devices.Additional;
using CShocker.Ranges;
using CShocker.Shockers;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Devices.APIs;

public class OpenShockSerial : OpenShockApi
{
    private const int BaudRate = 115200;
    public SerialPortInfo SerialPortI;
    private readonly SerialPort _serialPort;
    
    public OpenShockSerial(SerialPortInfo serialPortI, string apiKey, string endpoint = "https://api.shocklink.net", ILogger? logger = null) : base(DeviceApi.OpenShockSerial, apiKey, endpoint, logger)
    {
        this.SerialPortI = serialPortI;
        this._serialPort = new SerialPort(serialPortI.PortName, BaudRate);
        try
        {
            this._serialPort.Open();
        }
        catch (Exception e)
        {
            this.Logger?.Log(LogLevel.Error, e.Message);
            throw;
        }
    }

    protected override void ControlInternal(ControlAction action, Shocker shocker, int intensity, int duration)
    {
        if (shocker is not OpenShockShocker openShockShocker)
        {
            this.Logger?.Log(LogLevel.Warning, $"Shocker {shocker} is not {typeof(OpenShockShocker).FullName}");
            return;
        }
        string json = "rftransmit {" +
                        $"\"model\":\"{Enum.GetName(openShockShocker.model)!.ToLower()}\"," +
                        $"\"id\":{openShockShocker.rfId}," +
                        $"\"type\":\"{ControlActionToString(action)}\"," +
                        $"\"intensity\":{intensity}," +
                        $"\"durationMs\":{duration}" +
                      "}";
        _serialPort.WriteLine(json);
    }
    
    private static string ControlActionToString(ControlAction action)
    {
        return action switch
        {
            ControlAction.Beep => "sound",
            ControlAction.Vibrate => "vibrate",
            ControlAction.Shock => "shock",
            _ => "stop"
        };
    }
}