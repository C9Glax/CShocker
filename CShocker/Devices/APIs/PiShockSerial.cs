using System.IO.Ports;
using CShocker.Devices.Abstract;
using CShocker.Devices.Additional;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Devices.APIs;

public class PiShockSerial : PiShockApi
{
    private const int BaudRate = 115200;
    // ReSharper disable once MemberCanBePrivate.Global -> Exposed
    public readonly SerialPortInfo SerialPortI;
    private readonly SerialPort _serialPort;
    
    public PiShockSerial(DeviceApi apiType, SerialPortInfo serialPortI, ILogger? logger = null) : base(apiType, logger)
    {
        this.SerialPortI = serialPortI;
        this._serialPort = new SerialPort(this.SerialPortI.PortName, BaudRate);
        throw new NotImplementedException();
    }
    
    protected override void ControlInternal(ControlAction action, Shocker shocker, int intensity, int duration)
    {
        string json = "{" +
                      "\"cmd\": \"operate\"," +
                      "\"value\":{" +
                        $"\"op\": \"{ControlActionToOp(action)}\"," +
                        $"\"duration\": {duration}," +
                        $"\"intensity\": {intensity}," +
                        $"\"id\": " +
                        "}" +
                      "}";
        _serialPort.WriteLine(json);
    }

    private static string ControlActionToOp(ControlAction action)
    {
        return action switch
        {
            ControlAction.Beep => "",
            ControlAction.Vibrate => "vibrate",
            ControlAction.Shock => "",
            _ => ""
        };
    }
}