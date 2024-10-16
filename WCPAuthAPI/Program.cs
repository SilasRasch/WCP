using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using WCPShared.Interfaces;
using WCPShared.Models;
using WCPShared.Services;
using WCPShared.Services.StaticHelpers;
using Swashbuckle.AspNetCore.Filters;
using WCPShared.Interfaces.Auth;
using WCPShared.Interfaces.DataServices;
using WCPShared.Services.Converters;
using WCPShared.Services.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Auth API services
builder.Services.AddHttpContextAccessor(); // To get user in service-file instead of the controller (SOC)!
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICreatorService, CreatorService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<UserContextService>();
builder.Services.AddScoped<ViewConverter>();
if (Secrets.IsProd)
{
    builder.Services.AddDbContext<IWcpDbContext, WcpDbContext>(
        options => options.UseSqlServer(Secrets.GetConnectionString(builder.Configuration)));
}
else
{
    builder.Services.AddDbContext<IWcpDbContext, TestDbContext>(
        options => options.UseSqlServer(Secrets.GetConnectionString(builder.Configuration)));
}

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
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidIssuer = Secrets.Issuer,
        ValidAudiences = Secrets.GetAudiences(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secrets.GetJwtKey(builder.Configuration)))
    };
});

// CORS

var allowAll = "dev";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowAll, policy =>
    {
        policy.WithOrigins(Secrets.Origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(allowAll);

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
