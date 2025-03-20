using APBD;
using APBD.Devices;

namespace Test;

public class DeviceManagerTests

{
    private readonly string _testFile = "input.txt";
    [Fact]
    public void Constructor_LoadsDevicesFromFile()
    {
        var manager = new DeviceManager(_testFile);
        Assert.Equal(4, manager.Devices.Count);
    }
    [Fact]
    public void Constructor_ThrowsExceptionForMissingFile()
    {
        Assert.Throws<FileNotFoundException>(() => new DeviceManager("missing.txt"));
    }
    [Fact]
    public void AddDevice_AddsDevice()
    {
        var manager = new DeviceManager("input.txt");
        int initialCount = manager.Devices.Count;
        var newDevice = new SmartWatch(2, "Apple Watch", false, 90);
        
        manager.AddDevice(newDevice);
        Assert.Equal(initialCount + 1, manager.Devices.Count);
        Assert.Contains(manager.Devices, d => d.Id == 2 && d.Name == "Apple Watch");
    }
    [Fact]
    public void RemoveDevice_RemovesDeviceFromCollection()
    {
        var manager = new DeviceManager(_testFile);
        Assert.Contains(manager.Devices, d => d.Id == 1 && d.Name == "Apple Watch SE2");
        
        int initialCount = manager.Devices.Count;
        manager.RemoveDevice(1,"Apple Watch SE2");
        
        Assert.Equal(initialCount - 1, manager.Devices.Count);
        Assert.DoesNotContain(manager.Devices, d =>  d.Id == 1 && d.Name == "Apple Watch SE2");
        
    }
    [Fact]
    public void TurnOnDevice_SetsDeviceIsOnToTrue()
    {
        var manager = new DeviceManager(_testFile);
        
        var device = manager.Devices.FirstOrDefault(d => d.Name == "LinuxPC");
        Assert.NotNull(device);
        Assert.False(device.IsOn);
        
        manager.TurnOnDevice(1,"LinuxPC");
        
        Assert.True(device.IsOn);
    }
    [Fact]
    public void TurnOffDevice_SetsDeviceIsOnToFalse()
    {
        var manager = new DeviceManager(_testFile);
        
        var device = manager.Devices.FirstOrDefault(d => d.Id == 1 && d.Name == "Apple Watch SE2");
        Assert.NotNull(device);
        Assert.True(device.IsOn);
        
        manager.TurnOnDevice(1,"LinuxPC");
        
        Assert.False(device.IsOn);
    }
}