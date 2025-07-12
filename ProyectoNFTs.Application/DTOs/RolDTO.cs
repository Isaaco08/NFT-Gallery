using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.DTOs;

public class RolDTO
{
    [Display(Name = "Código")]
    public int IdRol { get; set; }

    [Display(Name = "Descripción")]
    public string DescripcionRol { get; set; } = null!;

    public override string ToString()
    {
        return $"{IdRol}- {DescripcionRol}";
    }
}
