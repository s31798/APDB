using APBD;
using APBD.Devices;

namespace Test;

public class DeviceManagerTests

{
    private readonly string _testFile = "input.txt";
    [Fact]
    public void Constructor_LoadsDevicesFromFile()
    {
        var factory = new FileDeviceMangerFactory();
        IDeviceManager manager = factory.CreateDeviceManager();
        Assert.Equal(4, manager.GetAllDevices().Count);
    }
    [Fact]
    public void AddDevice_AddsDevice()
    {
        var factory = new FileDeviceMangerFactory();
        IDeviceManager manager = factory.CreateDeviceManager();
        int initialCount = manager.GetAllDevices().Count;
        var newDevice = new SmartWatch("2", "Apple Watch", false, 90);
        
        manager.AddDevice(newDevice);
        Assert.Equal(initialCount + 1, manager.GetAllDevices().Count);
        Assert.Contains(manager.GetAllDevices(), d => d.Id == "2" && d.Name == "Apple Watch");
    }
    [Fact]
    public void RemoveDevice_RemovesDeviceFromCollection()
    {
        var factory = new FileDeviceMangerFactory();
        IDeviceManager manager = factory.CreateDeviceManager();
        Assert.Contains(manager.GetAllDevices(), d => d.Id == "1" && d.Name == "Apple Watch SE2");
        
        int initialCount = manager.GetAllDevices().Count;
        manager.RemoveDevice("1","Apple Watch SE2");
        
        Assert.Equal(initialCount - 1, manager.GetAllDevices().Count);
        Assert.DoesNotContain(manager.GetAllDevices(), d =>  d.Id == "1" && d.Name == "Apple Watch SE2");
        
    }
    [Fact]
    public void TurnOnDevice_SetsDeviceIsOnToTrue()
    {
        var factory = new FileDeviceMangerFactory();
        IDeviceManager manager = factory.CreateDeviceManager();
        
        var device = manager.GetAllDevices().FirstOrDefault(d => d.Name == "LinuxPC");
        Assert.NotNull(device);
        Assert.False(device.IsOn);
        
        manager.TurnOnDevice("1","LinuxPC");
        
        Assert.True(device.IsOn);
    }
    [Fact]
    public void TurnOffDevice_SetsDeviceIsOnToFalse()
    {
        var factory = new FileDeviceMangerFactory();
        IDeviceManager manager = factory.CreateDeviceManager();
        
        var device = manager.GetAllDevices().FirstOrDefault(d => d.Id == "1" && d.Name == "Apple Watch SE2");
        Assert.NotNull(device);
        Assert.True(device.IsOn);
        
        manager.TurnOffDevice("1","Apple Watch SE2");
        
        Assert.False(device.IsOn);
    }
}