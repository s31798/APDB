using System.Text.RegularExpressions;
using APBD.Exceptions;
using Microsoft.VisualBasic;

namespace APBD.Devices;

public class EmbeddedDevice : ElectronicDevice
{
    private const string IpPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
    private const string NetworkNamePattern = @"\bMD Ltd\.\b";
    private string _ip;
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
    private string _networkName{get;set;}

    public EmbeddedDevice(int id, string name, bool isOn,string ip, string networkName) : base( id, name, isOn)
    {
        _networkName = networkName;
        Ip = ip;
    }

    public void Connect()
    {
        if (!Regex.IsMatch(_networkName, NetworkNamePattern))
        {
            throw new ConnectionException();
        }   
    }

    public override void TurnOn()
    {
        Connect();
        base.TurnOn();
    }
    
    public override string ToString()
    {
        string on = IsOn ? "ON" : "OFF";
        return $"Embedded Device {Id}: {Name} is {on} with ip: {Ip} on the network: {_networkName}";
    }
}