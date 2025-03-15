using APBD.Devices;

namespace APBD;

public class DeviceManager
{
    private List<ElectronicDevice> _devices = [];

    public DeviceManager(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found ", filePath);
        
        var contents = File.ReadLines(filePath);
        foreach (var line in contents)
        {
            try
            {
                var info = line.Trim().Split(",");
                var first = info[0].Split("-");
                var id = int.Parse(first[1]);
                var type = first[0];
                var name = info[1];
                switch (type)
                {
                    case "SW": 
                        var battery = int.Parse(info[3].Replace("%", ""));
                        var isOn = bool.Parse(info[2]);
                        _devices.Add(new SmartWatch(id,name,isOn,battery));
                        break;
                }
            }
            catch (Exception e)
            {
                break;
            }
        }
    }

    public void ShowAllDevices()
    {
        foreach (var device in _devices)
        {
            Console.WriteLine(device);
        }
    }
  
}