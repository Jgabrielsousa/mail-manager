using Asp.Versioning;
using mail_manager.Configuration;
using mail_manager.Extensions;
using MailManager.Application.Dtos;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddRefitConfiguration(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddEndpoints(typeof(Program).Assembly);
builder.Services.AddOptions();
builder.Services.AddHealthChecks();

var app = builder.Build();
var versionedGroup = app.VersionedGroup();
app.UseHealthChecks("/health");
app.MapEndpoints(versionedGroup);
app.UseSwagger();
app.UseSwaggerUI();


if (app.Environment.IsDevelopment())
{
    builder.Configuration
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
}
else {
     builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

}

app.UseHttpsRedirection();

app.Run();

public partial class Program { }
