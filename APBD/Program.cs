using APBD;
class Program
{
    public static void Main(string[] args)
    {
        var manager = new DeviceManager("input.txt");
        manager.SaveDataToFile("test.txt");
    }
}