using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.DTOs;

public class UsuarioDTO
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string Login { get; set; } = null!;

    [Display(Name = "Tipo")]
    [Required(ErrorMessage = "{0} es requerido")]
    public int IdRol { get; set; }

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string? Password { get; set; } = default;

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string? Nombre { get; set; } = default;

    [Display(Name = "Apellidos")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string? Apellidos { get; set; } = default;

    [Display(Name = "Estado")]
    [Required(ErrorMessage = "{0} es requerido")]
    public int? Estado { get; set; }

    public string? DescripcionRol { get; set; } = default!;
}
