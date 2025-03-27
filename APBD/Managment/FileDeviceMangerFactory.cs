namespace APBD;

public class FileDeviceMangerFactory : IManagerFactory
{
    public IDeviceManager CreateDeviceManager()
    {
        return new DeviceManager("input.txt");
    }
}