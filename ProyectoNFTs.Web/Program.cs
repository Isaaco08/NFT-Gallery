using Serilog.Events;
using Serilog;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using ProyectoNFTs.Infraestructure.Repository.Implementations;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Application.Services.Implementations;
using ProyectoNFTs.Application.Profiles;
using ProyectoNFTs.Infraestructure.Data;
using ProyectoNFTs.Application.Config;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProyectoNFTs.Web;

var builder = WebApplication.CreateBuilder(args);

// Mapping AppConfig Class to read  appsettings.json
builder.Services.Configure<AppConfig>(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure D.I.
builder.Services.AddTransient<IRepositoryCliente, RepositoryCliente>();
builder.Services.AddTransient<IServiceCliente, ServiceCliente>();

builder.Services.AddTransient<IRepositoryPais, RepositoryPais>();
builder.Services.AddTransient<IServicePais, ServicePais>();

builder.Services.AddTransient<IRepositoryNft, RepositoryNft>();
builder.Services.AddTransient<IServiceNft, ServiceNft>();

builder.Services.AddTransient<IRepositoryTarjeta, RepositoryTarjeta>();
builder.Services.AddTransient<IServiceTarjeta, ServiceTarjeta>();

builder.Services.AddTransient<IRepositoryFactura, RepositoryFactura>();
builder.Services.AddTransient<IServiceFactura, ServiceFactura>();

builder.Services.AddTransient<IServicePropietario, ServicePropietario>();
builder.Services.AddTransient<IRepositoryPropietario, RepositoryPropietario>();

builder.Services.AddTransient<IServiceReporte, ServiceReporte>();

builder.Services.AddTransient<IServiceRol, ServiceRol>();
builder.Services.AddTransient<IRepositoryRol, RepositoryRol>();

builder.Services.AddTransient<IServiceUsuario, ServiceUsuario>();
builder.Services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();

// Security
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
            new ResponseCacheAttribute
            {
                NoStore = true,
                Location = ResponseCacheLocation.None,
            }
        );
});

// config Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ClienteProfile>();
    config.AddProfile<PaisProfile>();
    config.AddProfile<NftProfile>();
    config.AddProfile<TarjetaProfile>();
    config.AddProfile<FacturaProfile>();
    config.AddProfile<PropietarioNftProfile>();
    config.AddProfile<RolProfile>();
    config.AddProfile<UsuarioProfile>();
});

// Config Connection to SQLServer DataBase
builder.Services.AddDbContext<ProyectoNFTsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Get Log Path when is running in either Production or Development
string pathLogs = builder.Configuration.GetSection("LogPath").Value!;

// Logger
var logger = new LoggerConfiguration()
                    //.MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(LogEventLevel.Information)
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .CreateLogger();

builder.Host.UseSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Error control Middleware
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

//Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
Console.WriteLine($"**********************************************");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName.ToString()}  ");
Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}  ");
Console.WriteLine($"OS: {Environment.OSVersion.Platform}  ");

Console.WriteLine($"**********************************************");

// Error access control 
app.UseStatusCodePagesWithReExecute("/error/{0}");

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Activate Antiforgery DEBE COLOCARSE ACA!
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
