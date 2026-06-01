using CShocker.Devices;
using CShocker.Helpers;

Console.Write("ApiKey: ");
string apiKey = EnsureRead();

Console.Write("ShockerId: ");
string shockerIdStr = EnsureRead();
Guid shockerId = Guid.Parse(shockerIdStr);

OpenShockHttpShocker httpShocker = new(shockerId, apiKey);

httpShocker.Control(ControlAction.Beep, 100, 1000);

return;

string EnsureRead()
{
    string? result;
    do
    {
        result = Console.ReadLine();
    } while (result is null);
    return result;
}