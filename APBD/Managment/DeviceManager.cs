using APBD.Devices;


namespace APBD;

public class DeviceManager : IDeviceManager
{
    public List<ElectronicDevice> Devices { get; set; }
    private int _maxCount = 15;

    public DeviceManager(string filePath)
    {
        Devices = new List<ElectronicDevice>();
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found ", filePath);
        
        var contents = File.ReadLines(filePath);
        var factory = new DeviceTextFactory();
        foreach (var line in contents)
        {
            var device = factory.CreateElectronicDevice(line);
            if(device != null) Devices.Add(device);
        }
    }

    public void ShowAllDevices()
    {
        foreach (var device in Devices)
        {
            Console.WriteLine(device);
        }
    }

    public void RemoveDevice(string id, string name)
    {
        Devices.RemoveAll(device => device.Id == id && device.Name == name);
    }
    public void AddDevice(ElectronicDevice device)
    {
        if (Devices.Count < _maxCount)
        {
             Devices.Add(device);
        }
       
    }

    public void TurnOnDevice(string id, string name)
    { 
        foreach (ElectronicDevice device in Devices)
        {
            if (device.Id == id && device.Name == name) device.TurnOn();
        }
    }

    public void TurnOffDevice(string id, string name)
    {
        foreach (ElectronicDevice device in Devices)
        {
            if (device.Id == id && device.Name == name) device.TurnOff();
        }
    }

    public void SaveDataToFile(string fileName)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(fileName);
            foreach (ElectronicDevice device in Devices)
            {
                string type;
                string id = "-" + device.Id;
                string name = device.Name;
                string isOn = device.IsOn.ToString();
                string line = "";
         
                switch (device)
                {
                    case SmartWatch sw:
                        type = "SW";
                        string battery = sw.Battery.ToString() + "%";
                        line = type + id + "," + name + "," + isOn + "," + battery;
                        break;
                    case PersonalComputer pc:
                        type = "P";
                        string os = pc.OperatingSystem;
                        line = type + id + "," + name + "," + isOn + "," + os;
                        break;
                    case EmbeddedDevice ed:
                        type = "ED";
                        string ip = ed.Ip;
                        string network = ed.NetworkName;
                        line = type + id + "," + name + "," + ip + "," + network;
                        break;
                }
                writer.WriteLine(line);
            }

        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public void EditDeviceData(string id, string name, string? newName, string? newIp, string? newNetworkName, string? newOperatingSystem)
    {
        ElectronicDevice? deviceToEdit = null;
    
        foreach (var device in Devices)
        {
            if (device.Id == id && device.Name == name)
            {
                deviceToEdit = device;
                break;
            }
        }
        if (deviceToEdit == null) return;
        if (newName != null)
        {
            deviceToEdit.Name = newName;
        }
        if (deviceToEdit is EmbeddedDevice edn && newNetworkName != null)
        {
            edn.NetworkName = newNetworkName;
        }
        if (deviceToEdit is EmbeddedDevice edip && newIp != null)
        {
            edip.Ip = newIp;
        }
        if (deviceToEdit is PersonalComputer pc && newOperatingSystem != null)
        {
            pc.OperatingSystem = newOperatingSystem;
        }
    }
}