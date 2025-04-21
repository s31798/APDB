using APBD;
using APBD.Devices;

namespace WebApplication2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers(); 
        var connectionString = builder.Configuration.GetConnectionString("local");
        if (connectionString != null)
        {
            builder.Services.AddSingleton<IDeviceManager,DeviceManager>(manager => new DeviceManager(new List<ElectronicDevice>(),connectionString));
        }
        else
        {
            throw new Exception("No connection string found");
        }
        
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment()){ }
        
        app.MapControllers();

        app.Run();
    }
}