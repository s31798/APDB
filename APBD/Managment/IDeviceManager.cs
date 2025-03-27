using APBD.Devices;

namespace APBD;

public interface IDeviceManager
{
    public void ShowAllDevices();
    public void RemoveDevice(string id, string name);
    public void AddDevice(ElectronicDevice device);
    public void TurnOnDevice(string id, string name);
    public void TurnOffDevice(string id, string name);
    public void SaveDataToFile(string fileName);
    public void EditDeviceData(string id, string name, string? newName, string? newIp, string? newNetworkName,
        string? newOperatingSystem);
}