using ProyectoNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryPropietario
{
    Task<ICollection<PropietarioNft>> FindByDescriptionAsync(string description);
    Task<ICollection<PropietarioNft>> ListAsync();
    Task<PropietarioNft> FindByIdAsync(int id);

    Task<int> AddAsync(PropietarioNft entity);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, PropietarioNft entity);

    Task<ICollection<PropietarioNft>> FindByIdNFT(Guid id);
    //Task<ICollection<Nft>> FindByNftAsync(string filtro);
}
