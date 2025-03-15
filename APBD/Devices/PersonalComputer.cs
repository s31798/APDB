namespace APBD.Devices;
using APBD.Exceptions;

public class PersonalComputer : ElectronicDevice
{
    private string _operatingSystem;

    public PersonalComputer(string operatingSystem)
    {
        if (string.IsNullOrEmpty(operatingSystem))
        {
            throw new EmptySystemException();
        }
        _operatingSystem = operatingSystem;
    }
    
}
