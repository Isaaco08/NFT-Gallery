using ProyectoNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryNft
{
    Task<ICollection<Nft>> FindByDescriptionAsync(string description);
    Task<ICollection<Nft>> ListAsync();
    Task<Nft> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(Nft entity);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, Nft entity);
    
}
