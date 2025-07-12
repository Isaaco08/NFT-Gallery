using System.ComponentModel.DataAnnotations;

namespace ProyectoNFTs.Web.ViewModels;
 
  
public record ViewModelInput
{
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "NFT")]
    public int IdNft { get; set; }

    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Cantidad")]
    [Range(0, 999999999, ErrorMessage = "Cantidad mínimo es {0}")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "{0} es requerido")]
    [Range(0, 999999999, ErrorMessage = "Precio mínimo es {0}")]    
    [Display(Name = "Precio")]
    public decimal Precio { get; set; }
}