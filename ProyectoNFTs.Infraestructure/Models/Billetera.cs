using System;
using System.Collections.Generic;

namespace ProyectoNFTs.Infraestructure.Models;

public partial class Billetera
{
    public Guid IdNft { get; set; }

    public Guid IdCliente { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Nft IdNftNavigation { get; set; } = null!;
}
