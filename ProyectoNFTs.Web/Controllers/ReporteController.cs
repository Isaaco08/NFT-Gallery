using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Application.Services.Implementations;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Infraestructure.Data;
using ProyectoNFTs.Infraestructure.Models;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using System;
using X.PagedList;

namespace ProyectoNFTs.Web.Controllers;

[Authorize(Roles = "Admin,Cashier")]
public class ReporteController : Controller
{
    private readonly IServiceReporte _servicioReporte;
    private readonly IServiceCliente _serviceCliente;
    private readonly IServiceNft _serviceNft;
    private readonly IRepositoryNft _repositoryNft;
    private readonly IServicePropietario _servicePropietario;
    private readonly ProyectoNFTsContext _context;

    public ReporteController(IServiceReporte servicioReporte, IServiceCliente serviceCliente, IServiceNft serviceNft, IRepositoryNft repositoryNft, ProyectoNFTsContext context, IServicePropietario servicePropietario)
    {
        _servicioReporte = servicioReporte;
        _serviceCliente = serviceCliente;
        _serviceNft = serviceNft;
        _repositoryNft = repositoryNft;
        _context = context;
        _servicePropietario = servicePropietario;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ClienteReport()
    {
        var collection = await _serviceCliente.ListAsync();
        return View(collection);
    }

    [HttpPost]
    [RequireAntiforgeryToken]
    public async Task<FileResult> ClienteReportPDF()
    {
        byte[] bytes = await _servicioReporte.ClienteReport();
        return File(bytes, "text/plain", "Reporte Clientes.pdf");

    }

    public async Task<IActionResult> NFTReport(int? page)
    {
        var collection = await _serviceNft.ListAsync();
        return View(collection.ToPagedList(page ?? 1, 5));
    }

    [HttpPost]
    [RequireAntiforgeryToken]
    public async Task<FileResult> NFTReportPDF()
    {
        byte[] bytes = await _servicioReporte.NFTReport();
        if (bytes == null)
        {
            return ViewBag.ErrorMessage = "No existen datos suficientes!";
        }
        return File(bytes, "text/plain", "Reporte NFTs.pdf");
    }

    public IActionResult VentasReport()
    {
        return View();
    }

    [HttpPost]
    [RequireAntiforgeryToken]
    public async Task<IActionResult> VentasReportPDF(DateTime fechaInicial, DateTime fechaFinal)
    {       
        if (fechaInicial > fechaFinal)
        {
            ViewBag.Message = "La fecha inicial no puede ser mayor que la fecha final";
            return View("VentasReport");
        }

        byte[] bytes = await _servicioReporte.VentasReport(fechaInicial, fechaFinal);
        if (bytes == null)
        {
            ViewBag.ErrorMessage = "No existen datos suficientes!";
            return View("VentasReport");
        }
        return File(bytes, "text/plain", "Reporte Ventas.pdf");
    }

    public IActionResult VentasGraficoReport()
    {
        return View();
    }

    public async Task<IActionResult> GraficoReport(DateTime fechaInicial, DateTime fechaFinal)
    {
        string etiquetas = "";
        string precios = "";
        decimal total = 0M;


        var lista = await _servicioReporte.FacturasFechas(fechaInicial, fechaFinal);

        if(lista.Count() == 0)
        {
            ViewBag.ErrorMessage = "No existen datos suficientes!";
            return View("VentasGraficoReport");
        }

        foreach (var item in lista)
        {
            // concatenade 
            etiquetas += "Factura N." + item.IdFactura + ",";
            // Has List Any data?
            var sigma = item.ListFacturaDetalle.Any() ? item.ListFacturaDetalle.Sum(p => (p.Cantidad * p.Precio)) : 0;
            total += sigma.Value;
            if (sigma.HasValue)
            {
                total += sigma.Value;
                precios += sigma.Value.ToString("##") + ",";
            }
        }
        etiquetas = etiquetas.Substring(0, etiquetas.Length - 1); // ultima coma
        precios = precios.Substring(0, precios.Length - 1);

        ViewBag.GraphTitle = $"Total de Ventas {total.ToString("###,###.00")} ";
        ViewBag.Etiquetas = etiquetas;
        ViewBag.Valores = precios;
        return View("VentasGraficoReport");
    }

    public IActionResult NFTPropietarioReport()
    {
        return View();
    }

    public IActionResult _ListNft()
    {
        return PartialView();
    }

    public async Task<IActionResult> NFTPropietarioResultado(string descripcion)
    {
        var listaClientes = await _serviceCliente.ListAsync();
        var collection = await _serviceNft.FindByDescriptionAsync(descripcion);

        ClienteDTO cliente = new ClienteDTO();
        if (collection.Any())
        {
            var NftId = collection.First().IdNft;
            var collectionPropietario = await _servicePropietario.FindByIdNFT(NftId);

            if (collection.Any())
            {
                var clientePropietario = ((IEnumerable<PropietarioNftDTO>)collectionPropietario).ToList();
                var clienteDict = clientePropietario.ToDictionary(c => c.IdCliente, c => c.IdCliente);

                foreach (var item in listaClientes)
                {
                    if (clienteDict.ContainsKey(item.IdCliente))
                    {
                        cliente = await _serviceCliente.FindByIdAsync(item.IdCliente);
                    }
                }

                ViewBag.nft = collection;
            }
            else
            {
                ViewBag.ErrorMessage = "El NFT buscado no existe!";
                return View("NFTPropietarioReport");
            }
        }
        else
        {
            ViewBag.ErrorMessage = "El NFT buscado no existe!";
            return View("NFTPropietarioReport");
        }
        
        return View("_ListNft", cliente);
    }

    
   
}
