using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Services.Interfaces;

public interface IServiceReporte
{
    Task<byte[]> ClienteReport();
    Task<byte[]> NFTReport();
    Task<byte[]> VentasReport(DateTime fechaInicial, DateTime fechaFinal);
    Task<ICollection<FacturaEncabezadoDTO>> FacturasFechas(DateTime fechaInicial, DateTime fechaFinal);
    //Task<ICollection<PropietarioNftDTO>> Propietario(string descripcion);
}
