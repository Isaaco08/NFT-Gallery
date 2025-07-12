using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.DTOs;

public record ClienteDTO
{
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Identificación")]
    public Guid IdCliente { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string Nombre { get; set; } = null!;

    [Display(Name = "1° Apellido")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string Apellido1 { get; set; } = null!;

    [Display(Name = "2° Apellido")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string Apellido2 { get; set; } = null!;

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato de correo electrónico válido.")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string Email { get; set; } = null!;

    [Display(Name = "Sexo")]
    [Required(ErrorMessage = "{0} es requerido")]
    public char Sexo { get; set; }

    [Display(Name = "Nacimiento")]
    [Required(ErrorMessage = "{0} es requerido")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime FechaNacimiento { get; set; }

    [Display(Name = "Nacionalidad")]
    [Required(ErrorMessage = "{0} es requerido")]
    public int IdPais { get; set; }


}
