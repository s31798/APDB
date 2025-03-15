namespace APBD.Devices;
using APBD.Exceptions;

public class PersonalComputer : ElectronicDevice
{
    private string _operatingSystem;

    public PersonalComputer(int id, string name, bool isOn, string operatingSystem) : base(id, name, isOn)
    {
        if (string.IsNullOrEmpty(operatingSystem))
        {
            throw new EmptySystemException();
        }
        _operatingSystem = operatingSystem;
    }
    
}
