using Microsoft.EntityFrameworkCore;
using ProyectoNFTs.Application.Profiles;
using ProyectoNFTs.Application.Services.Implementations;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Infraestructure.Data;
using ProyectoNFTs.Infraestructure.Repository.Implementations;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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

// Configure Swagger 
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Configure Swagger 
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
