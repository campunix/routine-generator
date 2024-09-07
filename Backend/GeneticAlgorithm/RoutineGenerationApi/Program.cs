
using Microsoft.AspNetCore.Mvc;
using RoutineLibrary;

namespace RoutineGenerationApi;

public class Program
{
    private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<RoutineGenerator>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin();
                });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(MyAllowSpecificOrigins);

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/routine", (HttpContext httpContext, [FromServices]RoutineGenerator routineGenerator) =>
        {
            var routine = routineGenerator.Generate();
            return routine;
        })
        .WithName("GetRoutine")
        .WithOpenApi();

        app.Run();
    }
}
