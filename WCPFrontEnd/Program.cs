using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using Stripe;
using System.Net.Http.Headers;
using System.Text;
using WCPFrontEnd.Components;
using WCPFrontEnd.Hubs;
using WCPFrontEnd.Services;
using WCPShared.Extensions;
using WCPShared.Interfaces;
using WCPShared.Models;
using WCPShared.Models.Enums;
using WCPShared.Services;
using WCPShared.Services.StaticHelpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// For chat
builder.Services.AddSignalR();
builder.Services.AddHttpClient<ShippingService>(client =>
{
    client.BaseAddress = new Uri("https://app.shipmondo.com/api/public/v3/");
    string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"a30c7730-f37c-4b50-9894-d35193b6d0d6:{Secrets.GetShipmondoPassword(builder.Configuration)}"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
});

builder.Services.AddControllers();

builder.Services.AddMudServices();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddAuthenticationServices();
builder.Services.AddScoped<IS3Client, S3Client>();
builder.Services.AddScoped<S3Service>();
StripeConfiguration.ApiKey = Secrets.GetStripeApiKey(builder.Configuration);
builder.Services.AddScoped<StripeService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<CustomDialogService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromHours(8);
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsNotSubscribed", policy => policy.RequireClaim("IsNotSubscribed").RequireRole(UserRole.Bruger.ToString()))
    .AddPolicy("IsNotStripeConnected", policy => policy.RequireClaim("IsNotStripeConnected").RequireRole(UserRole.Creator.ToString()))
    .AddPolicy("OnboardingIncomplete", policy => policy.RequireClaim("OnboardingIncomplete").RequireRole(UserRole.Creator.ToString()));
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRCorsPolicy", builder =>
    {
        builder.WithOrigins("https://test.wcp.dk", "https://admin.wcp.dk", "https://wcp.dk", "https://platform.webcontent.dk") // Replace with the actual origin
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseCors("SignalRCorsPolicy");
app.MapHub<ChatHub>("/chathub").RequireCors("SignalRCorsPolicy");

app.Run();
