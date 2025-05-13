using ABPD.Modells.DTOs;
using ABPD.Repository;
using APBD.Devices;
using Microsoft.Data.SqlClient;

namespace ABPD.Application;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<List<GetDevicesDTO>> DevicesAsync()
    {
        var devices = await _deviceRepository.GetDevicesAsync();
        return devices.Select(d => new GetDevicesDTO(d)).ToList();
    }


    public async Task<GetDevicesDTO?> GetDeviceByIdAsync(string id)
    {
        var device = await _deviceRepository.GetDeviceByIdAsync(id);
        return device is null ? null : new GetDevicesDTO(device);
    }

    public async Task<bool> AddDeviceAsync(ElectronicDevice device)
    {
       
        if (device is EmbeddedDevice embeddedDevice)
        {
            return await _deviceRepository.AddEmbeddedDeviceAsync(
                embeddedDevice.Id, 
                embeddedDevice.Name, 
                embeddedDevice.IsOn, 
                embeddedDevice.Ip, 
                embeddedDevice.NetworkName
            );
        }
        else if (device is SmartWatch smartWatch)
        {
            return await _deviceRepository.AddSmartWatchAsync(
                smartWatch.Id, 
                smartWatch.Name, 
                smartWatch.IsOn, 
                smartWatch.Battery
            );
        }
        else if (device is PersonalComputer personalComputer)
        {
            return await _deviceRepository.AddPersonalComputerAsync(
                personalComputer.Id, 
                personalComputer.Name, 
                personalComputer.IsOn, 
                personalComputer.OperatingSystem
            );
        }

        return false;
    }

    public async Task<bool> EditDeviceDataAsync(string id, string newName, string? newIp , string? newNetworkName, string? newOperatingSystem)
    {
        return await _deviceRepository.EditDeviceDataAsync(id, newName, newIp, newNetworkName, newOperatingSystem);
    }

    public async Task<bool> DeleteDeviceAsync(string id)
    {
        return await _deviceRepository.DeleteDeviceAsync(id);
    }
}
