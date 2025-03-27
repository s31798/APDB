namespace APBD.Devices;
using APBD.Exceptions;

public class SmartWatch : ElectronicDevice, IPowerNotifier
{
    private int _battery;
    public int Battery
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

    public SmartWatch(string id, string name, bool isOn, int battery) : base(id, name, isOn)
    {
        Battery = battery;
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
        Console.WriteLine($"battery is at {Battery}%");
    }

    public override string ToString()
    {
        string on = IsOn ? "ON" : "OFF";
        return $"SmartWatch {Id}: {Name} is {on} with {Battery}% battery";
    }
}
