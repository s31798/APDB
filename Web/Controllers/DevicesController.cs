using System.Text.Json.Serialization;
using APBD;
using APBD.Devices;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

[ApiController]
[Route("devices")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceManager _deviceManager;

    public DevicesController(IDeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
    }
    [HttpGet]
    public  ActionResult<IEnumerable<ElectronicDevice>> Get()
    {
        return Ok(_deviceManager.GetAllDevices());
    }
    [HttpGet("{id}")]
    public ActionResult<ElectronicDevice> Get(string id)
    {
        var device = _deviceManager.GetDeviceById(id);

        if (device == null)
        {
            return NotFound();
        }

        return Ok(device);
    }

    
    private ActionResult<ElectronicDevice> AddDevice(ElectronicDevice device)
    {
        if (device == null)
        {
            return BadRequest("Device data cannot be null.");
        }
        var createdDevice = _deviceManager.AddDevice(device);

        if (createdDevice == null)
        {
            return StatusCode(403, new { error = "Forbidden", message = "Max number of devices exceeded." });
        }
        return CreatedAtAction(nameof(Get), new { id = createdDevice.Id }, createdDevice);
    }
    
    [HttpPost("PersonalComputer")]
    public ActionResult<ElectronicDevice> Post([FromBody] PersonalComputer newComputer)
    {
        return AddDevice(newComputer);
    }
    [HttpPost("EmbededDevice")]
    public ActionResult<ElectronicDevice> Post([FromBody] EmbeddedDevice newEmbededDevice)
    {
        return AddDevice(newEmbededDevice);
    }
    [HttpPost("SmartWatch")]
    public ActionResult<ElectronicDevice> Post([FromBody] SmartWatch newSmartWatch)
    {
       return AddDevice(newSmartWatch);
    }

    [HttpPut("{id}")]
    public ActionResult<ElectronicDevice> Put(string id, [FromBody] EditDeviceRequest updatedDevice)
    {
        if (updatedDevice == null)
        {
            return BadRequest();
        }

        var result = _deviceManager.EditDeviceData(id, updatedDevice.NewName, updatedDevice.NewIp, updatedDevice.NewNetworkName, updatedDevice.NewOperatingSystem);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public ActionResult<ElectronicDevice> Delete(string id)
    {
       var result =  _deviceManager.RemoveDevice(id);
       if (result == null)
       {
           return NotFound();
       }
       return Ok(result);
    }

    public class EditDeviceRequest
    {
        [JsonPropertyName("newName")]
        public string? NewName { get; set; }

        [JsonPropertyName("newIp")]
        public string? NewIp { get; set; }

        [JsonPropertyName("newNetworkName")]
        public string? NewNetworkName { get; set; }

        [JsonPropertyName("newOperatingSystem")]
        public string? NewOperatingSystem { get; set; }
    }
}