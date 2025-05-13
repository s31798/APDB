using System.Text.Json.Serialization;

namespace ABPD.Modells.DTOs;

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