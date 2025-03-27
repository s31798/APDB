namespace APBD.Devices;

/// <summary>
/// Represents a base class for all electronic devices in the system.
/// </summary>
public abstract class ElectronicDevice
{
    /// <summary>
    /// Gets or sets the unique identifier of the device.
    /// </summary>
    public string Id{get;set;}
    
    /// <summary>
    /// Gets or sets the name of the device.
    /// </summary>
    public string Name{get;set;}
    
    /// <summary>
    /// Gets or sets a value indicating whether the device is currently powered on.
    /// </summary>
    public bool IsOn{get;set;}

    /// <summary>
    /// Initializes a new instance of the ElectronicDevice class.
    /// </summary>
    /// <param name="id">The unique identifier for the device.</param>
    /// <param name="name">The name of the device.</param>
    /// <param name="isOn">The initial power state of the device.</param>
    public ElectronicDevice(string id, string name, bool isOn)
    {
        Id = id;
        Name = name;
        IsOn = isOn;
    }

    /// <summary>
    /// Powers on the device.
    /// </summary>
    public virtual void TurnOn()
    {
        IsOn = true;
    }

    /// <summary>
    /// Powers off the device.
    /// </summary>
    public virtual void TurnOff()
    {
        IsOn = false;
    }
    
}