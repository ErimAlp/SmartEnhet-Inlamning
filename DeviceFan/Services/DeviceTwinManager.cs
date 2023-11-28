using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceFan.Services
{
    internal class DeviceTwinManager
    {
        public static async Task<object> GetDesiredTwinPropertyAsync(DeviceClient deviceClient, string key)
        {
            try
            {
                var twin = await deviceClient.GetTwinAsync();
                if (twin.Properties.Desired.Contains(key))
                    return twin.Properties.Desired[key];
            }
            catch (Exception e) { Console.WriteLine($"Unable to update reported Twin. Error: {e.Message}"); }
            return null!;
        }

        public static async Task UpdateReportedTwinPropertyAsync(DeviceClient deviceClient, string key, object value)
        {
            try
            {
                var twinProperties = new TwinCollection();
                twinProperties[key] = value;
                await deviceClient.UpdateReportedPropertiesAsync(twinProperties);

                Console.WriteLine($"Reported Twin property: {key} updated with value: {value}");
            }
            catch (Exception e) { Console.WriteLine($"Unable to update reported Twin. Error: {e.Message}"); }
        }
    }
}
