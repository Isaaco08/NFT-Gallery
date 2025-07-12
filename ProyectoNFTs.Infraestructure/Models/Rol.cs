using System;
using System.Collections.Generic;

namespace ProyectoNFTs.Infraestructure.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string? DescripcionRol { get; set; }

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
