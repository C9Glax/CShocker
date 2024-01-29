using CShocker.Devices;
using CShocker.Devices.Abstract;
using CShocker.Devices.Additional;
using CShocker.Ranges;
using CShocker.Shockers;
using CShocker.Shockers.Abstract;
using CShocker.Shockers.Additional;
using Newtonsoft.Json;
using TestApp;

Logger logger = new ();
List<OpenShockShocker> shockers = new();

Console.WriteLine("OpenShock API Key:");
string? apiKey = Console.ReadLine();
while(apiKey is null || apiKey.Length < 1)
    apiKey = Console.ReadLine();


OpenShockHttp openShockHttp = new (new IntensityRange(30, 50), new DurationRange(1000, 1000), apiKey, logger: logger);
shockers = openShockHttp.GetShockers();
openShockHttp.Control(ControlAction.Vibrate, 20, 1000, shockers.First());

File.WriteAllText("devices.json", JsonConvert.SerializeObject(openShockHttp));
OpenShockHttp deserialized = JsonConvert.DeserializeObject<OpenShockHttp>(File.ReadAllText("devices.json"))!;
Thread.Sleep(1100); //Wait for previous to end
deserialized.Control(ControlAction.Vibrate, 20, 1000, shockers.First());
openShockHttp.Dispose();
deserialized.Dispose();


/*
#pragma warning disable CA1416
List<SerialPortInfo> serialPorts = SerialHelper.GetSerialPorts();

if (serialPorts.Count < 1)
    return;

for(int i = 0; i < serialPorts.Count; i++)
    Console.WriteLine($"{i}) {serialPorts[i]}");

Console.WriteLine($"Select Serial Port [0-{serialPorts.Count-1}]:");
string? selectedPortStr = Console.ReadLine();
int selectedPort;
while (!int.TryParse(selectedPortStr, out selectedPort) || selectedPort < 0 || selectedPort > serialPorts.Count - 1)
{
    Console.WriteLine($"Select Serial Port [0-{serialPorts.Count-1}]:");
    selectedPortStr = Console.ReadLine();
}

OpenShockSerial openShockSerial = new(new IntensityRange(30, 50), new DurationRange(1000, 1000),serialPorts[selectedPort], apiKey, logger: logger);
shockers = openShockSerial.GetShockers();
openShockSerial.Control(ControlAction.Vibrate, 20, 1000, shockers.First());
File.WriteAllText("devices.json", JsonConvert.SerializeObject(openShockSerial));
OpenShockHttp deserialized = JsonConvert.DeserializeObject<OpenShockHttp>(File.ReadAllText("devices.json"))!;
openShockSerial.Dispose();
deserialized.Dispose();
*/

foreach(OpenShockShocker s in shockers)
    Console.Write(s);
File.WriteAllText("shockers.json", JsonConvert.SerializeObject(shockers));
List<IShocker> deserializedShockers = JsonConvert.DeserializeObject<List<IShocker>>(File.ReadAllText("shockers.json"), new ShockerJsonConverter())!;