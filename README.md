Library to interact with Shock-Collars that are remotely controllable via ESP32-Boards.

[![GitHub License](https://img.shields.io/github/license/c9glax/cshocker)](https://github.com/C9Glax/CShocker)
[![NuGet Version](https://img.shields.io/nuget/v/CShocker)](https://www.nuget.org/packages/CShocker)

# Usage

```csharp
public static void Main(string[] args){
    string apiKey = ":)";
    
    OpenShockHttp openShockHttp = new (apiKey);
    OpenShockShocker shocker1 = openShockHttp.GetShockers().First();
    shocker1.Control(ControlAction.Vibrate, 20, 1000);
    
    shocker1.Dispose();
    
    List<SerialPortInfo> serialPorts = SerialHelper.GetSerialPorts();
    int selectedPort = 1;
    OpenShockSerial openShockSerial = new(serialPorts[selectedPort], apiKey);
    OpenShockShocker shocker2 = openShockSerial.GetShockers().First();
    shocker2.Control(ControlAction.Vibrate, 20, 1000);
    
    shocker2.Dispose();
}
```
## `Shocker.Control`
```csharp
Control(ControlAction action, int intensity, int duration)
```


### ControlAction
From [here](https://github.com/C9Glax/CShocker/blob/master/CShocker/Devices/Additional/ControlActionEnum.cs)

## Variables

### ApiKey
- For OpenShock (HTTP) get token [here](https://shocklink.net/#/dashboard/tokens)

### Intensity Range
in percent

`0-100`

### Duration Range
in ms
- `0-30000` OpenShock
- `0-15000` PiShock

## Future
### ~~Username (PiShockHttp only)~~
~~For PiShock (HTTP) get information [here](https://apidocs.pishock.com/#header-authenticating)~~

### ~~Sharecode (PiShockHttp only)~~
~~For PiShock (HTTP) get information [here](https://apidocs.pishock.com/#header-authenticating)~~
