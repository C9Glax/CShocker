namespace CShocker.Devices.Abstract;

public struct SerialPortInfo
{
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
        return
            $"{GetType().Name}\nPortName: {PortName}\nDescription: {Description}\nManufacturer: {Manufacturer}\nDeviceID: {DeviceID}\n\r";
    }
}
