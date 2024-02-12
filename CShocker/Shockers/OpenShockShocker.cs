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

    public OpenShockShocker(Api api, string name, string id, short rfId, OpenShockModel model, DateTime createdOn, bool isPaused) : base (api)
    {
        if (api is not OpenShockApi)
            throw new Exception($"API-Type {api.GetType().FullName} is not usable with Shocker {this.GetType().FullName}");
        this.Name = name;
        this.ID = id;
        this.RfId = rfId;
        this.Model = model;
        this.CreatedOn = createdOn;
        this.IsPaused = isPaused;
    }

    public enum OpenShockModel : byte
    {
        CaiXianlin = 0,
        Petrainer = 1
    }

    public override string ToString()
    {
        const int tabWidth = -12;
        return $"\t{string.Join("\n\t",
            $"\u251c {"Type",tabWidth}: {GetType().Name}",
            $"\u251c {"Name",tabWidth}: {Name}",
            $"\u251c {"ID",tabWidth}: {ID}",
            $"\u251c {"RF-ID",tabWidth}: {RfId}",
            $"\u251c {"Model",tabWidth}: {Enum.GetName(Model)}",
            $"\u251c {"Created On",tabWidth}: {CreatedOn}",
            $"\u2514 {"Paused",tabWidth}: {IsPaused}")}" +
               $"\n\r";
    }

    public override bool Equals(object? obj)
    {
        return obj is OpenShockShocker oss && Equals(oss);

    }

    private bool Equals(OpenShockShocker other)
    {
        return ID == other.ID;
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }
}