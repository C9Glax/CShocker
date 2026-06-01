using System.Management;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace CShocker.Helpers;

internal static class WindowsSerialHelper
{
    [SupportedOSPlatform("windows")]
    public static List<(string? Port, string? Caption, string? Manufacturer, string? DeviceId)> GetSerialPorts()
    {
        List<(string? Port, string? Caption, string? Manufacturer, string? DeviceId)> ret = [];
        using ManagementClass entity = new("Win32_PnPEntity");
        // ReSharper disable once InconsistentNaming
        const string CUR_CTRL = @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\";
        foreach (ManagementObject instance in entity.GetInstances().OfType<ManagementObject>())
        {
            object oGuid = instance.GetPropertyValue("ClassGuid");
            if (oGuid.ToString()?.ToUpper().Equals("{4D36E978-E325-11CE-BFC1-08002BE10318}") is false or null)
                continue; // Skip all devices except device class "PORTS"
            string? caption = instance.GetPropertyValue("Caption")?.ToString();
            string? manufacturer = instance.GetPropertyValue("Manufacturer")?.ToString();
            string? deviceID = instance.GetPropertyValue("PnpDeviceID")?.ToString();
            string regEnum = CUR_CTRL + "Enum\\" + deviceID + "\\Device Parameters";
            string? portName = Registry.GetValue(regEnum, "PortName", "")?.ToString();
            int? s32Pos = caption?.IndexOf(" (COM");
            if (s32Pos > 0) // remove COM port from description
                caption = caption?.Substring(0, (int)s32Pos);
            ret.Add(new ValueTuple<string?, string?, string?, string?>(portName, caption, manufacturer, deviceID));
        }

        return ret;
    }
}