using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.DTOs;

public record PropietarioNftDTO
{
    [Display(Name = "No. Cambio Propietario")]
    public int Id { get; set; }

    [Display(Name = "Código Nft")]
    public Guid IdNft { get; set; }

    [Display(Name = "Código Cliente")]
    public Guid IdCliente { get; set; }

    
}
