using APBD.Devices;
using APBD.Exceptions;

namespace APBD;

public class DeviceTextFactory
{
    public ElectronicDevice? CreateElectronicDevice(string text)
    {
        try
        {
            var info = text.Trim().Split(",");
            var first = info[0].Split("-");
            var id = first[1];
            var type = first[0];
            var name = info[1];

            switch (type)
            {
                case "SW":
                    var battery = int.Parse(info[3].Replace("%", ""));
                    return new SmartWatch(id, name, bool.Parse(info[2]), battery);
                case "P":
                    var operatingSystem = info[3];
                    return new PersonalComputer(id, name, bool.Parse(info[2]), operatingSystem);
                case "ED":
                    var ip = info[2];
                    var networkName = info[3];
                    return new EmbeddedDevice(id, name, false, ip, networkName);
            }
        }
        catch (IOException e)
        {
            Console.Error.WriteLine("Couldn't open data file");
        }
        catch (ConnectionException e)
        {
            Console.Error.WriteLine("Device is on a different network");
        }
        catch (EmptySystemException e)
        {
            Console.Error.WriteLine("Operating system is not installed");
        }
        catch (EmptyBatteryException e)
        {
            Console.Error.WriteLine("Device is out of battery");
        }
        catch (Exception e) when (e is ArgumentException || e is ArgumentOutOfRangeException || e is FormatException || e is IndexOutOfRangeException)
        {
            //a line in file was corrupted, we ingnore it
        }
        return null;
    }
}