global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using System;
global using System.Net.Http;
global using System.Threading.Tasks;
global using System.Net.Http.Headers;
global using Microsoft.AspNetCore.Mvc;
global using Newtonsoft.Json;
global using System.Diagnostics;
global using System.Security.Claims;
global using System.Text;

global using Models.common;
global using Models.model;
global using Web.Controllers;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var env = builder.Environment;
var config = builder.Configuration;
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("Route.json",
                       optional: true,
                       reloadOnChange: true);
});

{

    services.AddControllersWithViews();
    //builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
    services.AddControllers();

    services.AddLogging();
    // x => {
    // new LoggerConfiguration()
    //     .MinimumLevel.Debug()
    //     .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    //     .CreateLogger();});

}

var app = builder.Build();

{
    // Configure the HTTP request pipeline.

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    //app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=User}/{action=Login}/{id?}");
}

app.Run(); //"https://localhost:7022/User/Login"

