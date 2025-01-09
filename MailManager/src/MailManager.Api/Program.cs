using Asp.Versioning;
using mail_manager.Configuration;
using mail_manager.Extensions;
using MailManager.Application.Dtos;

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
var app = builder.Build();

var versionedGroup = app.VersionedGroup();

app.MapEndpoints(versionedGroup);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //builder.Configuration
    //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
}

app.UseHttpsRedirection();

app.Run();

public partial class Program { }
