namespace Sensade;
using DAL;
using Interface;
using MODEL;
public class Program
{
    public static void Main(string[] args)
    {
        
        
        

        
        
        //Need to figure out correct way to deal with exeptions
        // use chatgpt to optimize code

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IParkingAreaDao<ParkingArea>>(_ => new ParkingAreaDAO("Host=localhost;Port=5432;Database=Sensade;Username=postgres;Password=lol123")); ;
        builder.Services.AddScoped<IParkingSpaceDao<ParkingSpace>>(_ => new ParkingSpaceDAO("Host=localhost;Port=5432;Database=Sensade;Username=postgres;Password=lol123"));
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}