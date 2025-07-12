using System;
using System.Collections.Generic;

namespace ProyectoNFTs.Infraestructure.Models;

public partial class PropietarioNft
{
    public int Id { get; set; }

    public DateTime? FechaPropiedad { get; set; }

    public Guid? IdNft { get; set; }

    public Guid? IdCliente { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Nft? IdNftNavigation { get; set; }
}
