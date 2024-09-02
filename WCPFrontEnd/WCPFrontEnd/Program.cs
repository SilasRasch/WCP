using WCPFrontEnd.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WCPShared.Interfaces.Auth;
using WCPShared.Interfaces;
using WCPShared.Services;
using WCPShared.Services.Databases.EntityFramework;
using WCPShared.Models;
using Microsoft.EntityFrameworkCore;
using WCPShared.Services.StaticHelpers;
using WCPShared.Interfaces.DataServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor(); // To get user in service-file instead of the controller (SOC)!
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<UserContextService>();

builder.Services.AddDbContext<AuthDbContext>(
    options => options.UseSqlServer(Secrets.GetConnectionString(builder.Configuration)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
