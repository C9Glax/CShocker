using CShocker.Helpers;
using CShocker.Interfaces;

namespace CShocker.Devices;

public sealed class OpenShockSerialShocker(OpenShockSerialShocker.OpenShockModel model, short rfId, string serialPortName) : SerialShocker(serialPortName, 115200)
{
    private OpenShockModel Model { get; init; } = model;
    private short rfId { get; init; } = rfId;
    
    public override void Control(ControlAction action, int intensity, int duration)
    {
        string json = "rftransmit {" +
                      $"\"model\":\"{Enum.GetName(Model)!.ToLower()}\"," +
                      $"\"id\":{rfId}," +
                      $"\"type\":\"{ControlActionToString(action)}\"," +
                      $"\"intensity\":{intensity}," +
                      $"\"durationMs\":{duration}" +
                      "}";
        SerialPort.WriteLine(json);
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
    
    public enum OpenShockModel
    {
        CaiXianlin,
        Petrainer
    }
}