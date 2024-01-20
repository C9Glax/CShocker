using System.Text.RegularExpressions;
using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using Microsoft.Extensions.Logging;

namespace CShocker.Shockers.APIS;

public class OpenShockSerial : SerialShocker
{
    // ReSharper disable once MemberCanBePrivate.Global external usage
    public readonly Dictionary<string, ShockerModel> Model;
    private const int BaudRate = 115200;
    public OpenShockSerial(Dictionary<string, ShockerModel> shockerIds, IntensityRange intensityRange, DurationRange durationRange, SerialPortInfo serialPortI, ILogger? logger = null) : base(shockerIds.Keys.ToList(), intensityRange, durationRange, serialPortI, BaudRate, ShockerApi.OpenShockSerial, logger)
    {
        this.Model = shockerIds;
    }

    protected override void ControlInternal(ControlAction action, string shockerId, int intensity, int duration)
    {
        string json = "rftransmit {" +
                        $"\"model\":\"{Enum.GetName(Model[shockerId])!.ToLower()}\"," +
                        $"\"id\":{shockerId}," +
                        $"\"type\":\"{ControlActionToString(action)}\"," +
                        $"\"intensity\":{intensity}," +
                        $"\"durationMs\":{duration}" +
                      "}";
        SerialPort.WriteLine(json);
    }

    public Dictionary<string, ShockerModel> GetShockers()
    {
        Dictionary<string, ShockerModel> ret = new();
        Regex shockerRex = new (@".*FetchDeviceInfo\(\): \[GatewayConnectionManager\]   \[[a-z0-9\-]+\] rf=([0-9]{1,5}) model=([0,1])");
        this.Logger?.Log(LogLevel.Debug, "Restart");
        SerialPort.WriteLine("restart");
        while (SerialPort.ReadLine() is { } line && !line.Contains("Successfully verified auth token"))
        {
            this.Logger?.Log(LogLevel.Trace, line);
            Match match = shockerRex.Match(line);
            if (match.Success)
                ret.Add(match.Groups[1].Value, Enum.Parse<ShockerModel>(match.Groups[2].Value));
        }
        this.Logger?.Log(LogLevel.Debug, $"Shockers found: \n\t{string.Join("\n\t", ret)}");

        return ret;
    }

    public enum ShockerModel : byte
    {
        Caixianlin = 0,
        Petrainer = 1
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