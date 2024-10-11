using WCPShared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigurePrometheus();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

var corsPolicyName = "dev";
builder.Services.ConfigureCors(corsPolicyName);

// Adds all EF and custom services
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddAuthenticationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapPrometheusScrapingEndpoint();

app.MapControllers();

await app.RunAsync();
