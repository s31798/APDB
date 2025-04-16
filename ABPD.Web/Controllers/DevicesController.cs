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
    public IResult Get()
    {
        return Results.Ok(_deviceManager.GetAllDevices());
    }
    [HttpGet("{id}")]
    public IResult Get(string id)
    {
        var device = _deviceManager.GetDeviceById(id);

        if (device == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(device);
    }

    
    private IResult AddDevice(ElectronicDevice device)
    {
        if (device == null)
        {
            return Results.BadRequest("Device data cannot be null.");
        }
        var createdDevice = _deviceManager.AddDevice(device);

        if (createdDevice == null)
        {
            return Results.StatusCode(403);
        }
        return Results.Created($"/devices/{createdDevice.Id}", createdDevice);
    }
    
    [HttpPost("PersonalComputer")]
    public IResult Post([FromBody] PersonalComputer newComputer)
    {
        return AddDevice(newComputer);
    }
    [HttpPost("EmbededDevice")]
    public IResult Post([FromBody] EmbeddedDevice newEmbededDevice)
    {
        return AddDevice(newEmbededDevice);
    }
    [HttpPost("SmartWatch")]
    public IResult Post([FromBody] SmartWatch newSmartWatch)
    {
       return AddDevice(newSmartWatch);
    }

    [HttpPut("{id}")]
    public IResult Put(string id, [FromBody] EditDeviceRequest updatedDevice)
    {
        if (updatedDevice == null)
        {
            return Results.BadRequest();
        }

        var result = _deviceManager.EditDeviceData(id, updatedDevice.NewName, updatedDevice.NewIp, updatedDevice.NewNetworkName, updatedDevice.NewOperatingSystem);
        if (result == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }

    [HttpDelete("{id}")]
    public IResult Delete(string id)
    {
       var result =  _deviceManager.RemoveDevice(id);
       if (result == null)
       {
           return Results.NotFound();
       }
       return Results.Ok(result);
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