using APBD.Database;

namespace APBD;

public class DatabaseDeviceManagerFactory : IManagerFactory
{
   
    public IDeviceManager CreateDeviceManager()
    {
        var database = new Database();
        return new DeviceManager(database.Devices);
    }
}