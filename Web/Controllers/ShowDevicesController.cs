using APBD;
using APBD.Devices;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

[ApiController]
[Route("Devices")]
public class ShowDevicesController : ControllerBase
{
    private readonly IDeviceManager _deviceManager;

    public ShowDevicesController(IDeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
    }
    [HttpGet]
    public IEnumerable<ElectronicDevice> Get()
    {
        return _deviceManager.GetAllDevices();
    }
}