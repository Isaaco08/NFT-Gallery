using System;
using System.Collections.Generic;

namespace ProyectoNFTs.Infraestructure.Models;

public partial class Pais
{
    public int IdPais { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Alfa2 { get; set; } = null!;

    public string Alfa3 { get; set; } = null!;

    public virtual ICollection<Cliente> Cliente { get; set; } = new List<Cliente>();
}
