
using ProyectoNFTs.Infraestructure.Repository.Implementations;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using ProyectoNFTs.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using ProyectoNFTs.Infraestructure.Models;

namespace MyApp
{
    public class Principal
    {
        /// <summary>
        /// Taken from https://www.c-sharpcorner.com/article/using-dependency-injection-in-net-console-apps/
        /// How to D.I. using Console
        /// </summary>
        /// <returns></returns>
        private static ServiceProvider CreateServices()
        {
            var serviceProvider = new ServiceCollection()
                   .AddLogging(options =>
                   {
                       options.ClearProviders();
                       options.AddConsole();
                   })
                   // Add D.I.
                   .AddTransient<IRepositoryCliente, RepositoryCliente>()
                   .AddTransient<IRepositoryFactura, RepositoryFactura>()
                   .AddTransient<IRepositoryNft, RepositoryNft>()
                   .AddTransient<IRepositoryPais, RepositoryPais>()
                   .AddTransient<IRepositoryPropietario, RepositoryPropietario>()
                   .AddTransient<IRepositoryRol, RepositoryRol>()
                   .AddTransient<IRepositoryTarjeta, RepositoryTarjeta>()
                   .AddTransient<IRepositoryUsuario, RepositoryUsuario>()

                   // Add SQLServer Connection
                   .AddDbContext<ProyectoNFTsContext>(options =>
                   {
                       // "Server=localhost;Database=ProyectoNFTs;Integrated Security=false;user id=sa;password=123456;Encrypt=false;"
                       options.UseSqlServer("Server=localhost;Database=ProyectoNFTs;Integrated Security=True;TrustServerCertificate=True;",
                       sqlOptions =>
                       {
                            sqlOptions.EnableRetryOnFailure(); 
                       });

                       options.EnableSensitiveDataLogging();
                   })
                   .AddTransient<MyApplication>()
                   .BuildServiceProvider();

            return serviceProvider;
        }

        /// <summary>
        /// Main 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Config Services (D.I., DataBases, etc)
            var services = CreateServices();
            MyApplication app = services.GetRequiredService<MyApplication>();

            // Call Reports
            app.ProductReport();

            app.Bill();
        }

        // Class resposible to create reports
        public class MyApplication
        {
            private readonly ILogger<MyApplication> _logger;
            private readonly IRepositoryNft _repositoryNft;
            private readonly IRepositoryFactura _repositoryFactura;

            public MyApplication(ILogger<MyApplication> logger,
                                 IRepositoryNft repositoryNft,
                                 IRepositoryFactura repositoryFactura)
            {
                _logger = logger;
                _repositoryNft = repositoryNft;
                _repositoryFactura = repositoryFactura;
            }

            public void Bill()
            {
                // Not async calling. 
                //var collection = _repositoryNft.ListAsync().GetAwaiter();

                var factura = _repositoryFactura.FindByIdAsync(14).GetAwaiter().GetResult();

                // License config ******  IMPORTANT ******
                QuestPDF.Settings.License = LicenseType.Community;

                Document.Create(document =>
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
                                col.Item().AlignLeft().Text("Gallería de NFTs S.A. ").Bold().FontSize(14).Bold();
                                col.Item().AlignLeft().Text($"Fecha: {DateTime.Now} ").FontSize(9);
                                col.Item().LineHorizontal(1f);
                            });
                        });

                        page.Content().PaddingVertical(10).Column(col1 =>
                        {
                            col1.Item().AlignLeft().Text($"Factura : {factura.IdFactura}").FontSize(12);
                            col1.Item().AlignLeft().Text($"Cliente : {factura.IdCliente.ToString().Trim()}- {factura.IdClienteNavigation.Nombre} {factura.IdClienteNavigation.Apellido1}").FontSize(12);
                            col1.Item().AlignLeft().Text($"Fecha   : {factura.FechaFacturacion}").FontSize(12);
                            col1.Item().LineHorizontal(0.5f);
                            col1.Item().Text("");
                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(10);
                                    columns.RelativeColumn(10);
                                    columns.RelativeColumn(10);
                                    columns.RelativeColumn(10);
                                    columns.RelativeColumn(10);
                                    columns.RelativeColumn(10);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#4666FF")
                                    .Padding(2).AlignCenter().Text("Línea").FontColor("#fff");

                                    header.Cell().Background("#4666FF")
                                   .Padding(2).AlignCenter().Text("Código").FontColor("#fff");

                                    header.Cell().Background("#4666FF")
                                   .Padding(2).AlignCenter().Text("NFT").FontColor("#fff");

                                    header.Cell().Background("#4666FF")
                                    .Padding(2).AlignCenter().Text("Cantidad").FontColor("#fff");

                                    header.Cell().Background("#4666FF")
                                   .Padding(2).AlignCenter().Text("Precio").FontColor("#fff");

                                    header.Cell().Background("#4666FF")
                                   .Padding(2).AlignCenter().Text("Total").FontColor("#fff");

                                });


                                foreach (var item in factura.FacturaDetalle)
                                {
                                    // Column 1
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).AlignCenter().Text(item!.Secuencia.ToString()).FontSize(10);

                                    // Column 2
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                   .Padding(2).Text(item.IdNftNavigation.IdNft.ToString()).FontSize(10);

                                    // Column 2
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                   .Padding(2).Text(item.IdNftNavigation.Nombre.ToString()).FontSize(10);

                                    // Column 3
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                   .Padding(2).AlignCenter().Text(item.Cantidad.ToString()).FontSize(10);

                                    // Column 4
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                   .Padding(2).AlignRight().Text(item.Precio.ToString("###,###.00")).FontSize(10);
                                
                                    // Column 6
                                    var totalLinea = (item.Cantidad * item.Precio);
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                   .Padding(2).AlignCenter().Text(totalLinea.ToString("###,###.00")).FontSize(10);
                                }

                            });

                            var granTotal = factura.FacturaDetalle.Sum(p => (p.Cantidad * p.Precio));
                            col1.Item().AlignCenter().Text("Total " + granTotal.ToString("###,###.00")).FontSize(12).Bold();
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
                }).GeneratePdfAndShow();
            }

            public void ProductReport()
            {
                // Not async calling. 
                var collection = _repositoryNft.ListAsync().GetAwaiter();

                // License config ******  IMPORTANT ******
                QuestPDF.Settings.License = LicenseType.Community;

                Document.Create(document =>
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
                                col.Item().AlignLeft().Text("Gallería de NFTs S.A. ").Bold().FontSize(14).Bold();
                                col.Item().AlignLeft().Text($"Fecha: {DateTime.Now} ").FontSize(9);
                                col.Item().LineHorizontal(1f);
                            });

                        });

                        page.Content().PaddingVertical(10).Column(col1 =>
                        {
                            col1.Item().AlignCenter().Text("** Reporte de NFTs **").FontSize(14).Bold();
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
                                   .Padding(2).AlignCenter().Text("Imagen").FontColor("#fff");

                                    header.Cell().Background("#4666FF")
                                   .Padding(2).AlignCenter().Text("Cantidad").FontColor("#fff");

                                    header.Cell().Background("#4666FF")
                                   .Padding(2).AlignCenter().Text("Precio").FontColor("#fff");

                                    header.Cell().Background("#4666FF")
                                   .Padding(2).AlignCenter().Text("Total").FontColor("#fff");
                                });

                                foreach (var item in collection.GetResult())
                                {
                                    var total = item.Cantidad * item.Precio;

                                    // Column 1
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text(item.IdNft.ToString() + "-" + item.Nombre.PadRight(50, '.').Substring(0, 15)).FontSize(10);

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
                                                         .Padding(2).AlignCenter().Text(total.ToString("###,###.00")).FontSize(10);
                                }

                            });

                            var granTotal = collection.GetResult().Sum(p => p.Cantidad * p.Precio);

                            col1.Item().AlignCenter().Text("Total " + granTotal.ToString("###,###.00")).FontSize(12).Bold();

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
                }).GeneratePdfAndShow();
            }
        }
    }
}