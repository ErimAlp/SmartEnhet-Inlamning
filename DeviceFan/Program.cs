using DeviceFan.Services;

namespace DeviceFan
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var device = new DeviceManager("HostName=kyh-iothubb-erim.azure-devices.net;DeviceId=Device_Fan;SharedAccessKey=5gsdRsIWSKgcnFHDMn45QVa8bR3bK/x5nAIoTGcWEbE=");
            device.Start();

            Console.WriteLine("Press [Enter] to close application\n");
            Console.ReadKey();
        }
    }
}