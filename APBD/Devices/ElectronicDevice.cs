namespace APBD.Devices;

public abstract class ElectronicDevice
{
    public int Id{get;set;}
    public string Name{get;set;}
    public bool IsOn{get;set;}

    public virtual void TurnOn()
    {
        IsOn = true;
    }

    public virtual void TurnOff()
    {
        IsOn = false;
    }
    
}