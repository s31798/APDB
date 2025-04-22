using System.Text.RegularExpressions;
using APBD.Exceptions;
using Microsoft.VisualBasic;

namespace APBD.Devices;

/// <summary>
/// Represents an embedded device that can connect to a network and has an IP address.
/// </summary>
public class EmbeddedDevice : ElectronicDevice
{
    private const string IpPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
    private const string NetworkNamePattern = @"\bMD Ltd\.\b";
    private string _ip;
    
    /// <summary>
    /// Gets or sets the IP address of the embedded device.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the IP address format is invalid.</exception>
    public string Ip
    {
        get=>_ip
        ;
        set
        {
            if (Regex.IsMatch(value, IpPattern))
            {
                _ip = value;
            }
            else
            {
                throw new ArgumentException("Invalid IP");
            }
        }
    }

    /// <summary>
    /// Gets or sets the name of the network the device is connected to.
    /// </summary>
    public string NetworkName{get;set;}

    /// <summary>
    /// Initializes a new instance of the EmbeddedDevice class.
    /// </summary>
    /// <param name="id">The unique identifier for the embedded device.</param>
    /// <param name="name">The name of the embedded device.</param>
    /// <param name="isOn">The initial power state of the device.</param>
    /// <param name="ip">The IP address of the device.</param>
    /// <param name="networkName">The name of the network the device is connected to.</param>
    public EmbeddedDevice(string id, string name, bool isOn,string ip, string networkName) : base( id, name, isOn)
    {
        NetworkName = networkName;
        Ip = ip;
    }

    public EmbeddedDevice()
    {
        
    }


    /// <summary>
    /// Attempts to connect the device to its network.
    /// </summary>
    /// <exception cref="ConnectionException">Thrown when the network name does not match the required pattern.</exception>
    public void Connect()
    {
        if (!Regex.IsMatch(NetworkName, NetworkNamePattern))
        {
            throw new ConnectionException();
        }   
    }

    /// <summary>
    /// Powers on the embedded device and connects it to the network.
    /// </summary>
    /// <exception cref="ConnectionException">Thrown when the device cannot connect to the network.</exception>
    public override void TurnOn()
    {
        Connect();
        base.TurnOn();
    }
    
    /// <summary>
    /// Returns a string representation of the embedded device.
    /// </summary>
    /// <returns>A string containing the device type, ID, name, power state, IP address, and network name.</returns>
    public override string ToString()
    {
        string on = IsOn ? "ON" : "OFF";
        return $"Embedded Device {Id}: {Name} is {on} with ip: {Ip} on the network: {NetworkName}";
    }
}