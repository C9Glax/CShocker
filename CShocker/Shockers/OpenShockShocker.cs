using System.Diagnostics.CodeAnalysis;
using CShocker.Shockers.Abstract;

namespace CShocker.Shockers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
[SuppressMessage("ReSharper", "InconsistentNaming")]
public struct OpenShockShocker : IShocker
{
    public string name, id;
    public short rfId;
    public OpenShockModel model;
    public DateTime createdOn;
    public bool isPaused;

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
}