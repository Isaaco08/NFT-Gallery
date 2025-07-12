using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.DTOs;

public record FacturaDetalleDTO
{
    [Display(Name = "No. Factura")]
    public int IdFactura { get; set; }

    [Display(Name = "Línea")]
    public int Secuencia { get; set; }

    [Display(Name = "Código")]
    public Guid IdNft { get; set; }

    [Display(Name = "NFT")]
    public string NombreNFT { get; set; } = default!;

    [Display(Name = "Cantidad")]
    public int? Cantidad { get; set; }

    [DisplayFormat(DataFormatString = "{0:n2}")]
    [Display(Name = "Precio")]
    public decimal? Precio { get; set; }

    [DisplayFormat(DataFormatString = "{0:n2}")]
    [Display(Name = "Total")]
    public decimal TotalLinea { get; set; }
}
