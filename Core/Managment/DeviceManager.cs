using APBD.Devices;


namespace APBD;

/// <summary>
/// Manages a collection of electronic devices with operations for device control and data persistence.
/// </summary>
public class DeviceManager : IDeviceManager
{
    /// <summary>
    /// Gets or sets the list of electronic devices managed by this instance.
    /// </summary>
    public List<ElectronicDevice> Devices { get; set; }
    private int _maxCount = 15;

    /// <summary>
    /// Initializes a new instance of the DeviceManager class.
    /// </summary>
    /// <param name="filePath">The path to the file containing device data.</param>
    /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
    public DeviceManager(List<ElectronicDevice> devices)
    {
        Devices = devices;
       
      
    }

    /// <summary>
    /// Displays information about all devices in the collection to the console.
    /// </summary>
    public List<ElectronicDevice> GetAllDevices()
    {
        return Devices;
    }

    /// <summary>
    /// Removes a device from the collection based on its ID and name.
    /// </summary>
    /// <param name="id">The unique identifier of the device to remove.</param>
    /// <param name="name">The name of the device to remove.</param>
    public void RemoveDevice(string id, string name)
    {
        Devices.RemoveAll(device => device.Id == id && device.Name == name);
    }

    /// <summary>
    /// Adds a new device to the collection if the maximum device count has not been reached.
    /// </summary>
    /// <param name="device">The electronic device to add to the collection.</param>
    public void AddDevice(ElectronicDevice device)
    {
        if (Devices.Count < _maxCount)
        {
             Devices.Add(device);
        }
       
    }

    /// <summary>
    /// Turns on a specific device identified by its ID and name.
    /// </summary>
    /// <param name="id">The unique identifier of the device to turn on.</param>
    /// <param name="name">The name of the device to turn on.</param>
    public void TurnOnDevice(string id, string name)
    { 
        foreach (ElectronicDevice device in Devices)
        {
            if (device.Id == id && device.Name == name) device.TurnOn();
        }
    }

    /// <summary>
    /// Turns off a specific device identified by its ID and name.
    /// </summary>
    /// <param name="id">The unique identifier of the device to turn off.</param>
    /// <param name="name">The name of the device to turn off.</param>
    public void TurnOffDevice(string id, string name)
    {
        foreach (ElectronicDevice device in Devices)
        {
            if (device.Id == id && device.Name == name) device.TurnOff();
        }
    }

    /// <summary>
    /// Saves the current state of all devices to a specified file.
    /// </summary>
    /// <param name="fileName">The name of the file to save the device data to.</param>
    /// <exception cref="IOException">Thrown when an I/O error occurs while writing to the file.</exception>
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

    /// <summary>
    /// Updates the properties of a specific device identified by its ID and name.
    /// </summary>
    /// <param name="id">The unique identifier of the device to edit.</param>
    /// <param name="name">The name of the device to edit.</param>
    /// <param name="newName">The new name for the device (optional).</param>
    /// <param name="newIp">The new IP address for embedded devices (optional).</param>
    /// <param name="newNetworkName">The new network name for embedded devices (optional).</param>
    /// <param name="newOperatingSystem">The new operating system for personal computers (optional).</param>
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