using System;
using System.Collections.Generic;

namespace ProyectoNFTs.Infraestructure.Models;

public partial class TempPropietarioNft
{
    public int Id { get; set; }

    public DateTime? FechaPropiedad { get; set; }

    public Guid? IdNft { get; set; }

    public Guid? IdCliente { get; set; }
}
