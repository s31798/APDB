namespace APBD;

class EmptyBatteryException : Exception
{
    public EmptyBatteryException() : base("Device is out of battery") { }
}

class EmptySystemException : Exception
{
    public EmptySystemException() : base("System not installed") { }
}