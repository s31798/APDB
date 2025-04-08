using APBD.Devices;

namespace APBD;

public interface IDeviceManager
{
    public List<ElectronicDevice> GetAllDevices();
    public ElectronicDevice? GetDeviceById(string id);
    public ElectronicDevice? RemoveDevice(string id);
    public ElectronicDevice? AddDevice(ElectronicDevice device);
    public void TurnOnDevice(string id, string name);
    public void TurnOffDevice(string id, string name);
    public void SaveDataToFile(string fileName);
    public ElectronicDevice? EditDeviceData(string id, string? newName, string? newIp, string? newNetworkName,
        string? newOperatingSystem);
}