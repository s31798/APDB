using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ABPD.Application;
using APBD;
using APBD.Devices;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

[ApiController]
[Route("devices")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    [HttpGet]
    public IResult Get()
    {
        List<ElectronicDevice> result = _deviceService.Devices();
        Console.WriteLine(result[0]);
        return Results.Ok(result);
    }
    [HttpGet("{id}")]
    public IResult Get(string id)
    {
        var device = _deviceService.GetDeviceById(id);

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
        var createdDevice = _deviceService.AddDevice(device);

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
    public IResult Put(string id, [FromBody] EditDeviceRequest updatedDevice)
    {
        if (updatedDevice == null)
        {
            return Results.BadRequest();
        }

        var result = _deviceService.EditDeviceData(id, updatedDevice.NewName, updatedDevice.NewIp, updatedDevice.NewNetworkName, updatedDevice.NewOperatingSystem);
        if (result == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }

    [HttpDelete("{id}")]
    public IResult Delete(string id)
    {
       var result =  _deviceService.DeleteDevice(id);
       if (result == false)
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