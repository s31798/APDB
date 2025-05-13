using APBD.Devices;

namespace ABPD.Modells.DTOs;

public class GetDevicesDTO
{
    public string Id{get;set;}
    public string Name{get;set;}
    public bool IsEnabled{get;set;}

    public GetDevicesDTO(ElectronicDevice device)
    {
        Id = device.Id;
        Name = device.Name;
        IsEnabled = device.IsOn;
    }
}