using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceFan.Services
{
    internal class DeviceConfiguration
    {
        public DeviceConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
            Initialize();
        }

        public string ConnectionString { get; private set; } = null!;
        public DeviceClient DeviceClient { get; private set; } = null!;
        public int TelemetryInterval { get; set; } = 60 * 1000;
        public bool AllowSending { get; set; } = true;

        public string? DeviceId { get; private set; } = null!;
        public string? deviceName { get; set; }
        public string? deviceType { get; set; }
        public string? Location { get; set; }

        public void Initialize()
        {
            if (ConnectionString != null)
            {
                DeviceId = ConnectionString.Split(";")[1].Split("=")[1];
                DeviceClient = DeviceClient.CreateFromConnectionString(ConnectionString, TransportType.Mqtt);
            }

        }

    }
}
