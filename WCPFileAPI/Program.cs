using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WCPFileAPI.Services.S3;
using WCPShared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<WCPShared.Interfaces.IWcpDbContext, WCPShared.Models.WcpDbContext>(options =>
    options.UseSqlServer("Data Source=172.232.142.14;Initial Catalog=WCP;User ID=sa;Password=Microsoftlmao!;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigurePrometheus();
//builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

var corsPolicyName = "dev";
builder.Services.ConfigureCors(corsPolicyName);

builder.Services.AddScoped<IS3Client, S3Client>();

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapPrometheusScrapingEndpoint();

app.MapControllers();

await app.RunAsync();
