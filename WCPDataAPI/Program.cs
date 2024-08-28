using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;
using WCPShared.Interfaces;
using WCPShared.Services;
using WCPShared.Services.StaticHelpers;
using Microsoft.EntityFrameworkCore;
using WCPShared.Models;
using WCPShared.Services.Databases.MSSQL;
using WCPShared.Interfaces.Mongo;
using WCPShared.Services.Databases.Mongo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<MongoDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<ICreatorService, CreatorService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<UserContextService>();
builder.Services.AddDbContext<AuthDbContext>(
    options => options.UseSqlServer(Secrets.GetConnectionString(builder.Configuration)));

string allowAll = "dev";

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
    options.AddPolicy(allowAll, policy =>
    {
        policy.WithOrigins(Secrets.Origins).AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
    });
});

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(allowAll);

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();