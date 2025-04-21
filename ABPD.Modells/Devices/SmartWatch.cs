namespace APBD.Devices;
using APBD.Exceptions;

/// <summary>
/// Represents a smart watch device that can track battery life and notify when battery is low.
/// </summary>
public class SmartWatch : ElectronicDevice, IPowerNotifier
{
    private int _battery;
    
    /// <summary>
    /// Gets or sets the battery level of the smart watch.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the battery level is less than 0 or greater than 100.</exception>
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

    /// <summary>
    /// Initializes a new instance of the SmartWatch class.
    /// </summary>
    /// <param name="id">The unique identifier for the smart watch.</param>
    /// <param name="name">The name of the smart watch.</param>
    /// <param name="isOn">The initial power state of the smart watch.</param>
    /// <param name="battery">The initial battery level (0-100).</param>
    public SmartWatch(string id, string name, bool isOn, int battery) : base(id, name, isOn)
    {
        Battery = battery;
    }

    /// <summary>
    /// Powers on the smart watch if battery is not empty.
    /// </summary>
    /// <exception cref="EmptyBatteryException">Thrown when attempting to turn on the device with 0% battery.</exception>
    public override void TurnOn()
    {
        if (_battery == 0)
        {
            throw new EmptyBatteryException();
        }
        IsOn = true;
        _battery -= 10;
    }

    /// <summary>
    /// Notifies when the battery level is low (below 20%).
    /// </summary>
    public void Notify()
    {
        Console.WriteLine($"battery is at {Battery}%");
    }

    /// <summary>
    /// Returns a string representation of the smart watch.
    /// </summary>
    /// <returns>A string containing the device type, ID, name, power state, and battery level.</returns>
    public override string ToString()
    {
        string on = IsOn ? "ON" : "OFF";
        return $"SmartWatch {Id}: {Name} is {on} with {Battery}% battery";
    }
}
