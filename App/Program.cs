using CShocker.Devices;
using CShocker.Helpers;

Console.Write("ApiKey: ");
string apiKey = EnsureRead();

List<(string Owner, string Name, Guid Id)> accessibleShockers = OpenShockHttpShocker.GetAccessibleShockers(apiKey);
for(int i = 0; i < accessibleShockers.Count; i++)
    Console.WriteLine($"{i}: Owner: {accessibleShockers[i].Owner} - Name: {accessibleShockers[i].Name} | {accessibleShockers[i].Id}");

while (true)
{
    Console.Write("Shocker: ");
    string shockerSelect = EnsureRead();
    (string Owner, string Name, Guid Id) shocker = accessibleShockers[int.Parse(shockerSelect)];

    OpenShockHttpShocker httpShocker = new(shocker.Id, apiKey);

    httpShocker.Control(ControlAction.Beep, 100, 1000);
}

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