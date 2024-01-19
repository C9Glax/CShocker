Library to interact with Shock-Collars that are remotely controllable via ESP32-Boards.

[![GitHub License](https://img.shields.io/github/license/c9glax/cshocker)](https://github.com/C9Glax/CShocker)
[![NuGet Version](https://img.shields.io/nuget/v/CShocker)](https://shields.io/badges/nu-get-version)

# Usage

```csharp
public static void Main(string[] args){
    List<string> shockerIds = new();
    IntensityRange intensityRange = new IntensityRange(30, 50);
    DurationRange durationRange = new DurationRange(1000, 2000);
    string apiKey = ":)";
    OpenShockHttp openShockHttp = new OpenShockHttp(shockerIds, intensityRange, durationRange, apiKey);
    openShockHttp.ShockerIds.AddRange(openShockHttp.GetShockers());
    
    string username = "username";
    string shareCode = "sharecode";
    PiShockHttp piShockHttp = new PiShockHttp(shockerIds, intensityRange, durationRange, apiKey, username, shareCode, apiUri);
    
    ControlAction action = ControlAction.Shock;
    openShockHttp.Control(action, shockerIds.First(), 20, 1000);
    piShockHttp.Control(action);
}
```
## `Shocker.Control`
```csharp
Control(ControlAction action, string? shockerId = null, int? intensity = null, int? duration = null)
```
If `shockerId` is `null`, all IDs will be used. If `intensity` or `duration` are `null`, a random value within the
configured range will be used.


### ControlAction
From [here](https://github.com/C9Glax/CShocker/blob/master/CShocker/Shockers/ControlAction.cs)

## Variables

### ApiKey
- For OpenShock (HTTP) get token [here](https://shocklink.net/#/dashboard/tokens)
- For PiShock (HTTP) get information [here](https://apidocs.pishock.com/#header-authenticating)

### ShockerIds
List of Shocker-Ids, comma seperated.

`[ "ID-1-asdasd", "ID-2-fghfgh" ]`

OpenShockHttp also can retrieve IDs automatically.

### Intensity Range
in percent

`0-100`

### Duration Range
in ms
- `0-30000` OpenShock
- `0-15000` PiShock

### Username (PiShockHttp only)
For PiShock (HTTP) get information [here](https://apidocs.pishock.com/#header-authenticating)

### Sharecode (PiShockHttp only)
For PiShock (HTTP) get information [here](https://apidocs.pishock.com/#header-authenticating)