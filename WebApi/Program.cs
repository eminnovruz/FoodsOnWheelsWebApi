using Application.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var cosmos = new CosmosConfiguration();
builder.Configuration.GetSection("Cosmos").Bind(cosmos);
builder.Services.AddDbContext<AppDbContext>(op => op.UseCosmos(cosmos.Uri, cosmos.Key, cosmos.DatabaseName));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/ApplicationLog-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddServices();
builder.Services.AddRepositories();
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
