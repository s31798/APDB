using APBD;
using APBD.Devices;

class Program
{
    public static void Main(string[] args)
    {
        var manager = new DeviceManager("input.txt");
        manager.AddDevice(new EmbeddedDevice(3,"t phone",false,"192.168.1.23","mynet"));
        manager.RemoveDevice(2,"Pi4");
        manager.ShowAllDevices();
        manager.TurnOnDevice(1,"Pi3");
        manager.SaveDataToFile("test.txt");
    }
}