using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.APIS;

public class PiShockSerial : SerialShocker
{
    private const int BaudRate = 115200;
    public PiShockSerial(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, SerialPortInfo serialPortI, ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, serialPortI, BaudRate, ShockerApi.PiShockSerial, logger)
    {
        throw new NotImplementedException();
    }

    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
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
        SerialPort.WriteLine(json);
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