using System.IO.Ports;
using CShocker.Ranges;
using Microsoft.Extensions.Logging;
using System.Management;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace CShocker.Shockers.Abstract;

public abstract class SerialShocker : Shocker
{
    public SerialPortInfo SerialPortI;
    protected SerialPort serialPort;
    
    protected SerialShocker(List<string> shockerIds, IntensityRange intensityRange, DurationRange durationRange, SerialPortInfo serialPortI, int baudRate, ShockerApi apiType, ILogger? logger = null) : base(shockerIds, intensityRange, durationRange, apiType, logger)
    {
        this.SerialPortI = serialPortI;
        this.serialPort = new SerialPort(serialPortI.PortName, baudRate);
        this.serialPort.Open();
    }

    [SupportedOSPlatform("windows")]
    public static List<SerialPortInfo> GetSerialPorts()
    {
        List<SerialPortInfo> ret = new();
        using (ManagementClass entity = new("Win32_PnPEntity"))
        {
            // ReSharper disable once InconsistentNaming
            const string CUR_CTRL = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\";

            foreach (ManagementObject instance in entity.GetInstances())
            {
                object oGuid;
                oGuid = instance.GetPropertyValue("ClassGuid");
                if (oGuid == null || oGuid.ToString()?.ToUpper().Equals("{4D36E978-E325-11CE-BFC1-08002BE10318}") is false)
                    continue; // Skip all devices except device class "PORTS"

                string? caption  = instance.GetPropertyValue("Caption")?.ToString();
                string? manufacturer = instance.GetPropertyValue("Manufacturer")?.ToString();
                string? deviceID = instance.GetPropertyValue("PnpDeviceID")?.ToString();
                string regEnum  = CUR_CTRL + "Enum\\" + deviceID + "\\Device Parameters";
                string? portName = Registry.GetValue(regEnum, "PortName", "")?.ToString();

                int s32_Pos = caption.IndexOf(" (COM");
                if (s32_Pos > 0) // remove COM port from description
                    caption = caption.Substring(0, s32_Pos);
                
                ret.Add(new SerialPortInfo(
                    portName,
                    caption,
                    manufacturer,
                    deviceID));
            }
        }

        return ret;
    }

    public class SerialPortInfo
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
                $"PortName: {PortName}\nDescription: {Description}\nManufacturer: {Manufacturer}\nDeviceID: {DeviceID}";
        }
    }
}