using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ABPD.Application;
using ABPD.Modells.DTOs;
using APBD;
using APBD.Devices;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

[ApiController]
[Route("/api/devices")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    [HttpGet]
    public async Task<IResult> Get()
    {
        var result = await _deviceService.DevicesAsync();
        Console.WriteLine(result[0]);
        return Results.Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IResult> Get(string id)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id);

        if (device == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(device);
    }

    
    private async Task<IResult> AddDevice(ElectronicDevice device)
    {
        if (device == null)
        {
            return Results.BadRequest("Device data cannot be null.");
        }
        var createdDevice = await _deviceService.AddDeviceAsync(device);

        if (!createdDevice)
        {
            return Results.StatusCode(403);
        }
        return Results.StatusCode(201);
    }
    
    [HttpPost()]
    public async Task<IResult> Post()
    {
       
        var request = HttpContext.Request;
        using var reader = new StreamReader(request.Body);
        var rawJson = await reader.ReadToEndAsync();
        var device = DeviceFromJson(rawJson);
        Console.WriteLine(device);
        if (device != null)
        {
            AddDevice(device);
            return Results.StatusCode(201);
        }
        return Results.BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<IResult> Put(string id, [FromBody] EditDeviceRequest updatedDevice)
    {
        if (updatedDevice == null)
        {
            return Results.BadRequest();
        }

        var result =  await _deviceService.EditDeviceDataAsync(id, updatedDevice.NewName, updatedDevice.NewIp, updatedDevice.NewNetworkName, updatedDevice.NewOperatingSystem);
        if (result == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(string id)
    {
       var result =await  _deviceService.DeleteDeviceAsync(id);
       if (result == false)
       {
           return Results.NotFound();
       }
       return Results.Ok(result);
    }

  

   private ElectronicDevice? DeviceFromJson(string rawJson)
{
    Console.WriteLine(rawJson);
    var json = JsonNode.Parse(rawJson);

    if (json == null)
        return null;
    
    if (json is not JsonObject jsonObj)
        return null;
    var id = jsonObj["id"]?.ToString();
    var name = jsonObj["name"]?.ToString();
    var isOn = jsonObj["isOn"]?.GetValue<bool>() ?? false;
    if (jsonObj.TryGetPropertyValue("battery", out var batteryNode))
    {
        var battery = batteryNode.GetValue<int>();
        return new SmartWatch(id, name, isOn, battery);
    }
    if (jsonObj.TryGetPropertyValue("operatingSystem", out var osNode))
    {
        var operatingSystem = osNode.ToString();
        return new PersonalComputer(id, name, isOn, operatingSystem);
    }
    
    if (jsonObj.TryGetPropertyValue("ip", out var ipNode) && jsonObj.TryGetPropertyValue("networkName", out var networkNode))
    {
        var ip = ipNode?.ToString();
        var networkName = networkNode.ToString();
        return new EmbeddedDevice(id, name, isOn, ip, networkName);
    }
    
    return null;
}


}