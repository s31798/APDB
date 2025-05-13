using ABPD.Modells.DTOs;
using APBD.Devices;

namespace ABPD.Application;

public interface IDeviceService
{
    public Task<List<GetDevicesDTO>> DevicesAsync();
    public Task<GetDevicesDTO?> GetDeviceByIdAsync(string id);
    public Task<bool> AddDeviceAsync(ElectronicDevice device);

    public Task<bool> EditDeviceDataAsync(string id, string newName, string? newIp, string? newNetworkName,
        string? newOperatingSystem);

    public Task<bool> DeleteDeviceAsync(string id);
}