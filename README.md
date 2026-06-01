Library to interact with Shock-Collars that are remotely controllable via ESP32-Boards.

[![GitHub License](https://img.shields.io/github/license/c9glax/cshocker)](https://github.com/C9Glax/CShocker)
[![NuGet Version](https://img.shields.io/nuget/v/CShocker)](https://www.nuget.org/packages/CShocker)

# Usage

```csharp
List<(string Owner, string Name, Guid shockerId)> accessibleShockers = OpenShockHttpShocker.GetAccessibleShockers(apiKey);

OpenShockHttpShocker httpShocker = new(shockerId, apiKey);
httpShocker.Control(ControlAction.Beep, 100, 1000);
```

# EnvVars
| EnvVar             | default                     |                                                    |
|--------------------|-----------------------------|----------------------------------------------------|
| `OPENSHOCK_HOST`   | `https://api.openshock.app` | BaseUrl                                            |
| `OPENSHOCK_APIKEY` |                             | [ApiKey](https://openshock.app/#/dashboard/tokens) |
| `OPENSHOCK_NAME`   | CShocker                    |                                                    |