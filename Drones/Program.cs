using Drones.Api.ExceptionHandling;
using Drones.Api.Resource;
using Drones.Core;
using Drones.Core.Services;
using Drones.Data;
using Drones.Entities.Models;
using Drones.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.CreateLogger();

builder.Services.AddSingleton<Serilog.ILogger>(logger);
builder.Services.AddSingleton<PeriodicDroneCheckin>();

builder.Services.AddDbContext<DronesContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDroneService, DroneService>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IMedicationService, MedicationService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddHostedService(
    provider => provider.GetRequiredService<PeriodicDroneCheckin>());

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
