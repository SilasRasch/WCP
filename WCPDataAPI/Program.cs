using System.Text.Json.Serialization;
using WCPShared.Services;
using WCPShared.Services.StaticHelpers;
using SlackNet.AspNetCore;
using WCPShared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigurePrometheus();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

string corsPolicyName = "dev";
builder.Services.ConfigureCors(corsPolicyName);

// Adds all EF custom services
builder.Services.AddDataServices(builder.Configuration);

builder.Services.AddScoped<SlackNotificationService>();
builder.Services.AddSlackNet(options =>
{
    options.UseApiToken(Secrets.GetSlackKey(builder.Configuration));
});

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapPrometheusScrapingEndpoint();

app.MapControllers();

await app.RunAsync();