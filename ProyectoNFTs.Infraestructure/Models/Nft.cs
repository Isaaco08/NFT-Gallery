using System;
using System.Collections.Generic;

namespace ProyectoNFTs.Infraestructure.Models;

public partial class Nft
{
    public Guid IdNft { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Autor { get; set; }

    public decimal Precio { get; set; }

    public int Cantidad { get; set; }

    public byte[] Imagen { get; set; } = null!;

    public virtual Billetera? Billetera { get; set; }

    public virtual ICollection<FacturaDetalle> FacturaDetalle { get; set; } = new List<FacturaDetalle>();

    public virtual ICollection<PropietarioNft> PropietarioNft { get; set; } = new List<PropietarioNft>();

}
