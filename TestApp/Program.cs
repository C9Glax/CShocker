using CShocker.Ranges;
using CShocker.Shockers.Abstract;
using CShocker.Shockers.APIS;
using Microsoft.Extensions.Logging;
using TestApp;

Logger logger = new (LogLevel.Trace);

#pragma warning disable CA1416
List<SerialShocker.SerialPortInfo> serialPorts = SerialShocker.GetSerialPorts();

if (serialPorts.Count < 1)
    return;

for(int i = 0; i < serialPorts.Count; i++)
    Console.WriteLine($"{i}) {serialPorts[i]}");

Console.WriteLine($"Select Serial Port [0-{serialPorts.Count-1}]:");
string? selectedPortStr = Console.ReadLine();
int selectedPort = -1;
while (!int.TryParse(selectedPortStr, out selectedPort) || selectedPort < 0 || selectedPort > serialPorts.Count - 1)
{
    Console.WriteLine($"Select Serial Port [0-{serialPorts.Count-1}]:");
    selectedPortStr = Console.ReadLine();
}

OpenShockSerial shockSerial = new (new Dictionary<string, OpenShockSerial.ShockerModel>(), new IntensityRange(30,50), new DurationRange(1000,1000), serialPorts[selectedPort], logger);
Dictionary<string, OpenShockSerial.ShockerModel> shockers = shockSerial.GetShockers();
Console.ReadKey();