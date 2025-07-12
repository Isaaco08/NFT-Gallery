using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.DTOs;

public record NftDTO
{
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Código")]
    public Guid IdNft { get; set; }
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = null!;
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Autor")]
    public string? Autor { get; set; }

    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Precio")]
    public decimal? Precio { get; set; }
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Cantidad")]
    public int? Cantidad { get; set; }

    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Imagen")]
    public byte[] Imagen { get; set; } = null!;
}
