namespace APBD.Devices;

/// <summary>
/// Defines a contract for devices that can notify about their power status.
/// </summary>
public interface IPowerNotifier
{
    /// <summary>
    /// Sends a notification about the device's power status.
    /// </summary>
    public void Notify();
}