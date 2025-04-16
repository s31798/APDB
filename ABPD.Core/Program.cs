using APBD;
using APBD.Devices;

class Program
{
    public static void Main(string[] args)
    {
        var factory = new FileDeviceMangerFactory();
        IDeviceManager manager = factory.CreateDeviceManager();
        manager.AddDevice(new EmbeddedDevice("2","t phone",false,"192.168.1.23","mynet"));
        manager.AddDevice(new EmbeddedDevice("3","t phone",false,"192.168.12.23","mynet"));
        manager.TurnOnDevice("1","Pi3");
        manager.SaveDataToFile("test.txt");
    }
}