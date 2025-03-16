using APBD.Devices;
using APBD.Exceptions;

namespace APBD;

public class DeviceManager
{
    private List<ElectronicDevice> _devices = [];
    private int _maxCount = 15;

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
        foreach (var device in _devices)
        {
            Console.WriteLine(device);
        }
    }

    private void AddDevice(ElectronicDevice device)
    {
        if (_devices.Count < _maxCount)
        {
             _devices.Add(device);
        }
       
    }
}