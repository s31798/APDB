namespace APBD.Devices;
using APBD.Exceptions;

public class SmartWatch : ElectronicDevice, IPowerNotifier
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
