using System.ComponentModel.DataAnnotations;

namespace ProyectoNFTs.Web.ViewModels;

public class ViewModelLogin
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "{0} es requerido")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Solamente números y letras")]
    public string User { get; set; } = default!;
    [StringLength(15, MinimumLength = 6, ErrorMessage = "Error en política de largo de contraseña")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Solamente números y letras")]
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = default!;
}
