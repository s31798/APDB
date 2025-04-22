namespace APBD.Devices;
using APBD.Exceptions;

/// <summary>
/// Represents a personal computer device with an operating system.
/// </summary>
public class PersonalComputer : ElectronicDevice
{
    /// <summary>
    /// Gets or sets the operating system installed on the computer.
    /// </summary>
    public string OperatingSystem;

    /// <summary>
    /// Initializes a new instance of the PersonalComputer class.
    /// </summary>
    /// <param name="id">The unique identifier for the computer.</param>
    /// <param name="name">The name of the computer.</param>
    /// <param name="isOn">The initial power state of the computer.</param>
    /// <param name="operatingSystem">The operating system installed on the computer.</param>
    /// <exception cref="EmptySystemException">Thrown when the operating system parameter is null or empty.</exception>
    public PersonalComputer(string id, string name, bool isOn, string operatingSystem) : base(id, name, isOn)
    {
        if (string.IsNullOrEmpty(operatingSystem))
        {
            throw new EmptySystemException();
        }
        OperatingSystem = operatingSystem;
    }
    public PersonalComputer(){}

    /// <summary>
    /// Returns a string representation of the personal computer.
    /// </summary>
    /// <returns>A string containing the device type, ID, name, power state, and operating system.</returns>
    public override string ToString()
    {
        string on = IsOn ? "ON" : "OFF";
        return $"Personal Computer {Id}: {Name} is {on} with {OperatingSystem} operating system";
    }
}
