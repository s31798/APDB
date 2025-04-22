using APBD.Devices;

namespace ABPD.Application;

public interface IDeviceService
{
    public List<ElectronicDevice> Devices();
    public ElectronicDevice? GetDeviceById(string id);
    public bool AddDevice(ElectronicDevice device);

    public bool EditDeviceData(string id, string newName, string? newIp, string? newNetworkName,
        string? newOperatingSystem);

    public bool DeleteDevice(string id);
}