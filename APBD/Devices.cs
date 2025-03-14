namespace APBD;

using APBD;
abstract class ElectronicDevice
{
    public int Id{get;set;}
    public string Name{get;set;}
    public bool IsOn{get;set;}

    public virtual void TurnOn()
    {
        IsOn = true;
    }

    public virtual void TurnOff()
    {
        IsOn = false;
    }
    
}

interface IPowerNotifier
{
    public void Notify();
}
class SmartWatch : ElectronicDevice, IPowerNotifier
{
    private double _battery
    {
        get => _battery;
        set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (value < 20)
            {
                Notify();
            }
            _battery = value;
        }
    }

    public SmartWatch()
    {
        Name = "SmartWatch";
    }


    public override void TurnOn()
    {
        if (_battery == 0)
        {
            throw new EmptyBatteryException();
        }
        IsOn = true;
        _battery -= 10;

    }
    public void Notify()
    {
        Console.WriteLine($"battery is at {Math.Round(_battery, 2)}%");
    }
}

class PersonalComputer : ElectronicDevice
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









