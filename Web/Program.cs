using APBD;

namespace WebApplication2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var factory = new DatabaseDeviceMangerFactory();
        IDeviceManager manager = factory.CreateDeviceManager();
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IDeviceManager>(manager);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
          
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}