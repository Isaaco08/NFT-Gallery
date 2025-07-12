using System;
using System.Collections.Generic;

namespace ProyectoNFTs.Infraestructure.Models;

public partial class TipoTarjeta
{
    public int IdTipoTarjeta { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Tarjeta> Tarjeta { get; set; } = new List<Tarjeta>();
}
