using APBD.Devices;

namespace APBD.Database;

public class Database
{
    public List<ElectronicDevice> Devices { get; set; }

    public Database()
    {
        Devices = new List<ElectronicDevice>()  {
            new EmbeddedDevice("EMB001", "Smart Thermostat", true, "192.168.1.100", "Home Network"),
            new EmbeddedDevice("EMB002", "Security Camera", true, "192.168.1.101", "Home Network"),
            new EmbeddedDevice("EMB003", "Smart Lighting Hub", false, "192.168.1.105", "Home Network"),
            new EmbeddedDevice("EMB004", "Industrial Sensor", true, "10.0.0.50", "Factory Network"),
            new EmbeddedDevice("EMB005", "Wireless Gateway", true, "10.0.0.55", "Factory Network"),
            new PersonalComputer("PC001", "Home Desktop", true, "Windows 10"),
            new PersonalComputer("PC002", "Work Laptop", true, "macOS Ventura"),
            new PersonalComputer("PC003", "Gaming Rig", false, "Windows 11"),
            new PersonalComputer("PC004", "Old Server", false, "Ubuntu Server 20.04"),
            new PersonalComputer("PC005", "Media Center", true, "Linux Mint"),
            new SmartWatch("SW001", "FitTrack Pro", true, 85),
            new SmartWatch("SW002", "TimeMaster X", true, 60),
            new SmartWatch("SW003", "HealthPlus Active", true, 92),
            new SmartWatch("SW004", "StyleWear Elite", false, 15),
            new SmartWatch("SW005", "SportPulse Runner", true, 78)
        };
       
        
        
        
        
        
        
    }
}