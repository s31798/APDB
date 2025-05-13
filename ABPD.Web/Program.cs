using ABPD.Application;
using ABPD.Repository;
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
            builder.Services.AddTransient<IDeviceRepository,DeviceRepository>(manager => new DeviceRepository(connectionString));
            builder.Services.AddSingleton<IDeviceService,DeviceService>();
        }
        else
        {
            throw new Exception("No connection string found");
        }
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(); // optional: customize UI with .UseSwaggerUI(c => { ... })
        }
                                          
        
        if (app.Environment.IsDevelopment()){ }
        
        app.MapControllers();

        app.Run();
    }
}