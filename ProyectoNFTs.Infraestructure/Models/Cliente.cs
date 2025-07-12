using System;
using System.Collections.Generic;

namespace ProyectoNFTs.Infraestructure.Models;

public partial class Cliente
{
    public Guid IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Sexo { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public int IdPais { get; set; }

    public virtual ICollection<Billetera> Billetera { get; set; } = new List<Billetera>();

    public virtual ICollection<FacturaEncabezado> FacturaEncabezado { get; set; } = new List<FacturaEncabezado>();

    public virtual Pais IdPaisNavigation { get; set; } = null!;

    public virtual ICollection<PropietarioNft> PropietarioNft { get; set; } = new List<PropietarioNft>();
}
