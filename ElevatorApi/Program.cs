// Adding signalR
// https://learn.microsoft.com/en-us/aspnet/core/tutorials/signalr-typescript-webpack?view=aspnetcore-7.0&tabs=visual-studio

using ElevatorApi.Hubs;
using ElevatorApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials();
    });
});

builder.Services.AddSignalR(opts => {
    opts.EnableDetailedErrors = true;
});

builder.Services.AddElevatorService();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("CorsPolicy");

app.MapHub<PlayerHub>("/hub");

app.Run();
