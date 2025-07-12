using AutoMapper;
using Microsoft.AspNetCore.Html;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Services.Implementations;

public class ServiceReporte : IServiceReporte
{
    private readonly IRepositoryCliente _repositoryCliente;
    private readonly IRepositoryNft _repositoryNft;
    private readonly IRepositoryFactura _repositoryFactura;
    private readonly IRepositoryPropietario _repositoryPropietario;
    private readonly IMapper _mapper;


    public ServiceReporte(IRepositoryCliente repositoryCliente, IRepositoryNft repositoryNft, IRepositoryFactura repositoryFactura, IMapper mapper, IRepositoryPropietario repositoryPropietario)
    {
        _repositoryCliente = repositoryCliente;
        _repositoryNft = repositoryNft;
        _repositoryFactura = repositoryFactura;
        _mapper = mapper;
        _repositoryPropietario = repositoryPropietario;
    }

    public async Task<byte[]> ClienteReport()
    {
        // Get Data
        var collection = await _repositoryCliente.ListAsync();

        // License config ******  IMPORTANT ******
        QuestPDF.Settings.License = LicenseType.Community;

        // return ByteArrays
        var pdfByteArray = QuestPDF.Fluent.Document.Create(document =>
        {
            document.Page(page =>
            {

                page.Size(PageSizes.Letter);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.Margin(30);

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignLeft().Text("NFT Gallery").Bold().FontSize(14).Bold();
                        col.Item().AlignLeft().Text($"Fecha: {DateTime.Now} ").FontSize(9);
                        col.Item().LineHorizontal(1f);
                    });

                });


                page.Content().PaddingVertical(10).Column(col1 =>
                {
                    col1.Item().AlignCenter().Text("Reporte de Clientes").FontSize(14).Bold();
                    col1.Item().Text("");
                    col1.Item().LineHorizontal(0.5f);

                    col1.Item().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();

                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Background("#4666FF")
                            .Padding(2).AlignCenter().Text("Nombre").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Primer Apellido").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Segundo Apellido").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Correo").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Sexo").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("F. Nacimiento").FontColor("#fff");
                        });

                        foreach (var item in collection)
                        {

                            string sexo = item.Sexo.Equals('F') ? "Femenino" : "Masculino";
                            string fechaNacimiento = item.FechaNacimiento.ToString("dd/MM/yyyy");

                            // Column 1
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.Nombre.ToString()).FontSize(10);

                            // Column 2
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.Apellido1.ToString()).FontSize(10);

                            // Column 3
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.Apellido2.ToString()).FontSize(10);
                            // Column 4
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.Email.ToString()).FontSize(10);
                            // Column 5
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(sexo.ToString()).FontSize(10);

                            // Column 6
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(fechaNacimiento.ToString()).FontSize(10);
                        }

                    });

                });


                page.Footer()
                .AlignRight()
                .Text(txt =>
                {
                    txt.Span("Página ").FontSize(10);
                    txt.CurrentPageNumber().FontSize(10);
                    txt.Span(" de ").FontSize(10);
                    txt.TotalPages().FontSize(10);
                });
            });
        }).GeneratePdf();

        //File.WriteAllBytes(@"C:\temp\ProductReport.pdf", pdfByteArray);
        return pdfByteArray;

    }

    public async Task<byte[]> NFTReport()
    {
        // Get Data
        var collection = await _repositoryNft.ListAsync();

        // License config ******  IMPORTANT ******
        QuestPDF.Settings.License = LicenseType.Community;

        // return ByteArrays
        var pdfByteArray = QuestPDF.Fluent.Document.Create(document =>
        {
            document.Page(page =>
            {

                page.Size(PageSizes.Letter);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.Margin(30);

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignLeft().Text("NFT Gallery").Bold().FontSize(14).Bold();
                        col.Item().AlignLeft().Text($"Fecha: {DateTime.Now} ").FontSize(9);
                        col.Item().LineHorizontal(1f);
                    });

                });


                page.Content().PaddingVertical(10).Column(col1 =>
                {
                    col1.Item().AlignCenter().Text("Reporte de NFT's").FontSize(14).Bold();
                    col1.Item().Text("");
                    col1.Item().LineHorizontal(0.5f);

                    col1.Item().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();

                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Background("#4666FF")
                            .Padding(2).AlignCenter().Text("NFT").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Foto").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Cantidad").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Precio").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Autor").FontColor("#fff");
                        });

                        foreach (var item in collection)
                        {

                            var total = item.Cantidad * item.Precio;

                            // Column 1
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.Nombre.ToString()).FontSize(10);

                            // Column 2
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).Image(item.Imagen).UseOriginalImage();

                            // Column 3
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.Cantidad.ToString()).FontSize(10);
                            // Column 4
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                   .Padding(2).AlignCenter().Text(item.Precio.ToString("###,###.00")).FontSize(10);
                            // Column 5
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.Autor.ToString()).FontSize(10);
                        }

                    });

                    var granTotal = collection.Sum(p => p.Cantidad * p.Precio);

                    col1.Item().AlignRight().Text("Total " + granTotal.ToString("###,###.00")).FontSize(12).Bold();

                });


                page.Footer()
                .AlignRight()
                .Text(txt =>
                {
                    txt.Span("Página ").FontSize(10);
                    txt.CurrentPageNumber().FontSize(10);
                    txt.Span(" de ").FontSize(10);
                    txt.TotalPages().FontSize(10);
                });
            });
        }).GeneratePdf();

        //File.WriteAllBytes(@"C:\temp\ProductReport.pdf", pdfByteArray);
        return pdfByteArray;

    }

    /*public async Task<ICollection<PropietarioNftDTO>> Propietario(string descripcion)
    {
        // Get Data
        var collection = await _repositoryPropietario.FindByDescriptionAsync(descripcion);

        var nombresClientes = new Dictionary<Guid, string>();  // Diccionario para almacenar nombres de clientes por ID

        // Recuperar los nombres de los clientes correspondientes a la tabla propietario
        foreach (var propietarioNft in collection)
        {
            // Obtener el nombre del cliente por ID y almacenarlo en el diccionario
            var cliente = await _repositoryCliente.FindByIdAsync((Guid)propietarioNft.IdCliente);
            if (cliente != null)
            {
                string nombreCompleto = $"{cliente.Nombre} {cliente.Apellido1} {cliente.Apellido2}";
                nombresClientes[(Guid)propietarioNft.IdCliente] = nombreCompleto;
            }
        }
        
    }*/

    public async Task<byte[]> VentasReport(DateTime fechaInicial, DateTime fechaFinal)
    {
        // Get Data
        var collection = await _repositoryFactura.ListByFechaAsync(fechaInicial, fechaFinal);

        if (collection.Count() == 0)
        {
            return null;
        }

        // Obtener todos los nombres de los clientes correspondientes a las facturas
        var nombresClientes = new Dictionary<Guid, string>();  // Diccionario para almacenar nombres de clientes por ID

        // Recuperar los nombres de los clientes correspondientes a las facturas
        foreach (var factura in collection)
        {
            // Obtener el nombre del cliente por ID y almacenarlo en el diccionario
            var cliente = await _repositoryCliente.FindByIdAsync(factura.IdCliente);
            if (cliente != null)
            {
                string nombreCompleto = $"{cliente.Nombre} {cliente.Apellido1} {cliente.Apellido2}";
                nombresClientes[factura.IdCliente] = nombreCompleto;
            }
        }

        // License config ******  IMPORTANT ******
        QuestPDF.Settings.License = LicenseType.Community;

        // return ByteArrays
        var pdfByteArray = QuestPDF.Fluent.Document.Create(document =>
        {
            document.Page(page =>
            {

                page.Size(PageSizes.Letter);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.Margin(30);

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignLeft().Text("NFT Gallery").Bold().FontSize(14).Bold();
                        col.Item().AlignLeft().Text($"Fecha: {DateTime.Now} ").FontSize(9);
                        col.Item().LineHorizontal(1f);
                    });

                });


                page.Content().PaddingVertical(10).Column(col1 =>
                {
                    col1.Item().AlignCenter().Text("Reporte de Ventas").FontSize(14).Bold();
                    col1.Item().Text("");
                    col1.Item().LineHorizontal(0.5f);

                    col1.Item().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();

                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Background("#4666FF")
                            .Padding(2).AlignCenter().Text("N. Factura").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Cliente").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Fecha de Facturación").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Estado").FontColor("#fff");

                            header.Cell().Background("#4666FF")
                           .Padding(2).AlignCenter().Text("Tarjeta").FontColor("#fff");
                        });


                        foreach (var item in collection)
                        {
                            // Obtener el nombre del cliente mediante JavaScript
                            string nombreCliente = nombresClientes.ContainsKey(item.IdCliente) ? nombresClientes[item.IdCliente] : "Desconocido";

                            string estado = item.EstadoFactura.Equals(0) ? "Anulada" : "Activa";

                            string fecha = item.FechaFacturacion.ToString("dd/MM/yyyy");

                            // Column 1
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.IdFactura.ToString()).FontSize(10);

                            // Column 2
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(nombreCliente).FontSize(10);

                            // Column 3
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(fecha.ToString()).FontSize(10);
                            // Column 4
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(estado.ToString()).FontSize(10);
                            // Column 5
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                                .Padding(2).AlignCenter().Text(item.TarjetaNumero.ToString()).FontSize(10);
                        }

                    });

                });


                page.Footer()
                .AlignRight()
                .Text(txt =>
                {
                    txt.Span("Página ").FontSize(10);
                    txt.CurrentPageNumber().FontSize(10);
                    txt.Span(" de ").FontSize(10);
                    txt.TotalPages().FontSize(10);
                });
            });
        }).GeneratePdf();

        //File.WriteAllBytes(@"C:\temp\ProductReport.pdf", pdfByteArray);
        return pdfByteArray;

    }

    public async Task<ICollection<FacturaEncabezadoDTO>> FacturasFechas(DateTime fechaInicial, DateTime fechaFinal)
    {
        var collection = await _repositoryFactura.ListByFechaAsync(fechaInicial, fechaFinal);
        var collectionMapped = _mapper.Map<ICollection<FacturaEncabezadoDTO>>(collection);
        return collectionMapped;
    }
}


