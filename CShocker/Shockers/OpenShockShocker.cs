using CShocker.Devices.Abstract;
using CShocker.Shockers.Abstract;

namespace CShocker.Shockers;

public class OpenShockShocker : Shocker
{
    // ReSharper disable thrice MemberCanBePrivate.Global -> Exposed
    public readonly string Name, ID;
    public readonly short RfId;
    public readonly OpenShockModel Model;
    public readonly DateTime CreatedOn;
    public readonly bool IsPaused;
    public string name, id;
    public short rfId;
    public OpenShockModel model;
    public DateTime createdOn;
    public bool isPaused;

    public OpenShockShocker(Api api, string name, string id, short rfId, OpenShockModel model, DateTime createdOn, bool isPaused) : base (api)
    {
        if (api is not OpenShockApi)
            throw new Exception($"API-Type {api.GetType().FullName} is not usable with Shocker {this.GetType().FullName}");
        this.name = name;
        this.id = id;
        this.rfId = rfId;
        this.model = model;
        this.createdOn = createdOn;
        this.isPaused = isPaused;
    }

    public enum OpenShockModel : byte
    {
        CaiXianlin = 0,
        Petrainer = 1
    }

    public override string ToString()
    {
        return $"{GetType().Name}\n" +
               $"Name: {name}\n" +
               $"ID: {id}\n" +
               $"RF-ID: {rfId}\n" +
               $"Model: {Enum.GetName(model)}\n" +
               $"Created On: {createdOn}\n" +
               $"Paused: {isPaused}\n\r";
    }

    public override bool Equals(object? obj)
    {
        return obj is OpenShockShocker oss && Equals(oss);

    }

    private bool Equals(OpenShockShocker other)
    {
        return id == other.id && rfId == other.rfId && model == other.model && createdOn.Equals(other.createdOn);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(id, rfId, (int)model, createdOn);
    }
}