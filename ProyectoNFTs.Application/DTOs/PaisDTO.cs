using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.DTOs;

public record PaisDTO
{
    [Required(ErrorMessage = "{0} es requerido")]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "{0} debe tener exactamente 3 dígitos.")]
    [Display(Name = "Código")]
    public int IdPais { get; set; }

    [Display(Name = "Descripción")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string Descripcion { get; set; } = null!;

    [Display(Name = "ALFA-2")]
    [Required(ErrorMessage = "{0} es requerido")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "{0} debe tener al menos 2 caracteres.")]
    public string Alfa2 { get; set; } = null!;

    [Display(Name = "ALFA-3")]
    [Required(ErrorMessage = "{0} es requerido")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "{0} debe tener al menos 3 caracteres.")]
    public string Alfa3 { get; set; } = null!;

    public override string ToString()
    {
        return $"{IdPais}- {Descripcion}";
    }
}
