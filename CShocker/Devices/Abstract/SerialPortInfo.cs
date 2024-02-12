namespace CShocker.Devices.Abstract;

public readonly struct SerialPortInfo
{
    // ReSharper disable thrice MemberCanBePrivate.Global -> Exposed
    public readonly string? PortName, Description, Manufacturer, DeviceID;

    public SerialPortInfo(string? portName, string? description, string? manufacturer, string? deviceID)
    {
        this.PortName = portName;
        this.Description = description;
        this.Manufacturer = manufacturer;
        this.DeviceID = deviceID;
    }

    public override string ToString()
    {
        return $"{string.Join("\n\t",
            $"{GetType().Name}",
            $"PortName: {PortName}",
            $"Description: {Description}",
            $"Manufacturer: {Manufacturer}",
            $"DeviceID: {DeviceID}")}" +
               $"\n\r";
    }
}
