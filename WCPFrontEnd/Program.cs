using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using WCPFrontEnd.Components;
using WCPShared.Extensions;
using WCPShared.Interfaces;
using WCPShared.Models;
using WCPShared.Services;
using Stripe;
using WCPShared.Services.StaticHelpers;
using WCPShared.Models.Enums;
using WCPFrontEnd.Services;
using Microsoft.AspNetCore.ResponseCompression;
using WCPFrontEnd.Hubs;
using SlackNet;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// For chat
//builder.Services.AddSignalR();
//builder.Services.AddResponseCompression(options =>
//{
//    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
//});

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
//builder.Services.AddScoped<ShippingService>();

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

app.MapHub<ChatHub>("/chathub");

app.Run();
