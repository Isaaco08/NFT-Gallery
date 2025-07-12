using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.DTOs;

public record FacturaEncabezadoDTO
{
   
    [Display(Name = "No. Factura")]
    [ValidateNever]
    public int IdFactura { get; set; }

    [Display(Name = "Tipo Tarjeta")]
    [Required(ErrorMessage = "{0} es requerido")]
    public int IdTarjeta { get; set; }

    [Display(Name = "Cliente")]
    [Required(ErrorMessage = "{0} es requerido")]
    public Guid IdCliente { get; set; }

    [RegularExpression(@"^\d{1,8}$", ErrorMessage = "El número de tarjeta debe tener hasta 8 dígitos")]
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "No. Tarjeta")]
    public string TarjetaNumero { get; set; } = null!;

    [Display(Name = "Estado")]
    public int EstadoFactura { get; set; }

    [Display(Name = "Fecha")]
    public DateTime FechaFacturacion { get; set; }

    public List<FacturaDetalleDTO> ListFacturaDetalle = null!;

    [Display(Name = "Nombre")]
    public string NombreCliente { get; set; } = null!;
}
