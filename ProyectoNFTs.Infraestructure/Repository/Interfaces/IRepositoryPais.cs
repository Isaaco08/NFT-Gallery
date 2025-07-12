using ProyectoNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryPais
{
    Task<ICollection<Pais>> FindByDescriptionAsync(string description);
    Task<ICollection<Pais>> ListAsync();
    Task<Pais> FindByIdAsync(int id);
    Task<int> AddAsync(Pais entity);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, Pais entity);
}
