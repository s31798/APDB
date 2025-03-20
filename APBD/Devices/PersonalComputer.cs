namespace APBD.Devices;
using APBD.Exceptions;

public class PersonalComputer : ElectronicDevice
{
    public string OperatingSystem;

    public PersonalComputer(int id, string name, bool isOn, string operatingSystem) : base(id, name, isOn)
    {
        if (string.IsNullOrEmpty(operatingSystem))
        {
            throw new EmptySystemException();
        }
        OperatingSystem = operatingSystem;
    }
    public override string ToString()
    {
        string on = IsOn ? "ON" : "OFF";
        return $"Personal Computer {Id}: {Name} is {on} with {OperatingSystem} operating system";
    }
}
