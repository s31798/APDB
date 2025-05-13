using APBD.Devices;

namespace ABPD.Repository;

public interface IDeviceRepository
{ 
    public Task<List<ElectronicDevice>> GetDevicesAsync();
    public Task<ElectronicDevice?> GetDeviceByIdAsync(string id);
    public Task<bool> AddEmbeddedDeviceAsync(string id, string name, bool isOn, string ipAddress, string? netWorkName);
    public Task<bool> AddSmartWatchAsync(string id, string name, bool isOn,int batteryPercentage);
    public Task<bool> AddPersonalComputerAsync(string id, string name, bool isOn, string operatingSystem);
    public Task<bool> EditDeviceDataAsync(string id, string newName, string? newIp, string? newNetworkName,
        string? newOperatingSystem);

    public Task<bool> DeleteDeviceAsync(string id);
   
    
}