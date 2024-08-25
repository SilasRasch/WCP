using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Runtime;
using System.Text;
using System.Text.Json.Serialization;
using WCPFileAPI.Services.S3;
using WCPShared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string devPolicy = "dev";

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme  // Makes it possible to authorize in Swagger using JWT-tokens -> not necessary for normal use (for development only)
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Authentication services
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secrets.GetJwtKey(builder.Configuration)))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(devPolicy, policy =>
    {
        policy.WithOrigins(Secrets.Origins).AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
    });
});

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom settings
builder.Services.Configure<S3Settings>(builder.Configuration.GetSection(nameof(S3Settings)));

// Custom services
builder.Services.AddScoped<IS3Client, S3Client>();
builder.Services.AddSingleton<UserContextService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age{60 * 60 * 24 * 7}"); // Local cache for one week
    }
});

app.UseCors(devPolicy);

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
