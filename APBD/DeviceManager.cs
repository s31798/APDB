using APBD.Devices;
using APBD.Exceptions;

namespace APBD;

public class DeviceManager
{
    public List<ElectronicDevice> Devices { get; set; }
    private int _maxCount = 15;

    public DeviceManager(string filePath)
    {
        Devices = new List<ElectronicDevice>();
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
                        AddDevice(new SmartWatch(id, name, bool.Parse(info[2]), battery));
                        break;
                    case "P":
                        var operatingSystem = info[3];
                        AddDevice(new PersonalComputer(id, name, bool.Parse(info[2]), operatingSystem));
                        break;
                    case "ED":
                        var ip = info[2];
                        var networkName = info[3];
                        AddDevice(new EmbeddedDevice(id, name, false, ip, networkName));
                        break;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                continue;
            }
            catch (FormatException e)
            {
                continue;
            }
            catch (ConnectionException e)
            {
            }
            catch (EmptySystemException e)
            {
                continue;
            }
            catch (ArgumentOutOfRangeException e)
            {
                continue;
            }
            catch (EmptyBatteryException)
            {
                continue;
            }
            catch (ArgumentException e)
            {
                continue;
            }
        }
    }

    public void ShowAllDevices()
    {
        foreach (var device in Devices)
        {
            Console.WriteLine(device);
        }
    }

    public void RemoveDevice(int id, string name)
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

    public void TurnOnDevice(int id, string name)
    { 
        foreach (ElectronicDevice device in Devices)
        {
            if (device.Id == id && device.Name == name) device.TurnOn();
        }
    }

    public void TurnOffDevice(int id, string name)
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

    public void EditDeviceData(int id, string name, string? newName, string? newIp, string? newNetworkName, string? newOperatingSystem)
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