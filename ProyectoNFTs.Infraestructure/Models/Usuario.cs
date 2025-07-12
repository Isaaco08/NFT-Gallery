using System;
using System.Collections.Generic;

namespace ProyectoNFTs.Infraestructure.Models;

public partial class Usuario
{
    public string Login { get; set; } = null!;

    public int? IdRol { get; set; }

    public string? Password { get; set; }

    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public int? Estado { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
