using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WCPShared.Interfaces;
using WCPShared.Models;
using WCPShared.Services.Converters;
using WCPShared.Services.EntityFramework;
using WCPShared.Services;
using WCPShared.Services.StaticHelpers;
using WCPShared.Interfaces.Auth;
using SlackNet.AspNetCore;

namespace WCPShared.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigurePrometheus(this IServiceCollection services)
        {
            services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("WCP"))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                metrics.AddPrometheusExporter();
                metrics.AddOtlpExporter(opt => opt.Endpoint = Secrets.OtlpEndpoint);
            });

            return services;
        }

        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration config)
        {
            if (Secrets.IsProd)
            {
                services.AddDbContextFactory<WcpDbContext>(options =>
                    options.UseSqlServer(Secrets.GetConnectionString(config)));

                services.AddDbContext<IWcpDbContext, WcpDbContext>(options =>
                    options.UseSqlServer(Secrets.GetConnectionString(config)));

                services.AddSingleton<IWcpDbContextFactory, WcpDbContextFactory>();
            }
            else
            {
                //services.AddDbContextFactory<TestDbContext>(options =>
                //{
                //    options.EnableSensitiveDataLogging();
                //    options.UseSqlServer(Secrets.GetConnectionString(config));
                //});

                services.AddDbContext<IWcpDbContext, TestDbContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(Secrets.GetConnectionString(config));
                });

                //services.AddDbContext<V2Context>(options =>
                //{
                //    options.EnableSensitiveDataLogging();
                //    options.UseSqlServer("");
                //});

                //services.AddSingleton<IWcpDbContextFactory, TestDbContextFactory>();
            }

            return services;
        }

        public static IServiceCollection ConfigureDbContextPostgres(this IServiceCollection services, IConfiguration config)
        {
            if (Secrets.IsProd)
            {
                services.AddDbContextFactory<WcpDbContext>(options =>
                    options.UseNpgsql(Secrets.GetConnectionString(config)));

                services.AddDbContext<IWcpDbContext, WcpDbContext>(options =>
                    options.UseNpgsql(Secrets.GetConnectionString(config)));

                services.AddSingleton<IWcpDbContextFactory, WcpDbContextFactory>();
            }
            else
            {
                services.AddDbContextFactory<TestDbContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseNpgsql(Secrets.GetConnectionString(config));
                });

                services.AddDbContext<IWcpDbContext, TestDbContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseNpgsql(Secrets.GetConnectionString(config));
                });

                services.AddSingleton<IWcpDbContextFactory, TestDbContextFactory>();
            }

            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme  // Makes it possible to authorize in Swagger using JWT-tokens -> not necessary for normal use (for development only)
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }

        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = Secrets.Issuer,
                    ValidAudiences = Secrets.GetAudiences(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secrets.GetJwtKey(config)))
                };
            });

            return services;
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services, string policyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: policyName, policy =>
                {
                    policy.WithOrigins(Secrets.Origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
                });
            });

            return services;
        }

        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<UserService>();
            services.AddScoped<OrderService>();
            services.AddScoped<OrganizationService>();
            services.AddScoped<CreatorService>();
            services.AddScoped<BrandService>();
            services.AddScoped<StaticTemplateService>();
            services.AddScoped<IEmailService, SendGridEmailService>();
            services.AddScoped<LanguageService>();
            services.AddScoped<UserContextService>();
            services.AddScoped<ViewConverter>();

            services.AddScoped<SlackNotificationService>();
            services.AddSlackNet(options =>
            {
                options.UseApiToken(Secrets.GetSlackKey(configuration));
            });

            return services;
        }

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
