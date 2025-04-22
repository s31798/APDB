using APBD.Devices;

namespace APBD;

public class FileDeviceMangerFactory : IManagerFactory
{
    public IDeviceManager CreateDeviceManager()
    {
        var devices = new List<ElectronicDevice>();
        var file = new FileWorker("input.txt");
        var contents = file.GetFileContents();
        var parser = new DeviceTextParser();
        foreach (var line in contents)
        {
            var device = parser.CreateElectronicDevice(line);
            if(device != null) devices.Add(device);
        }
        return new DeviceManager(devices);
    }
}