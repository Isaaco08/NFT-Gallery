using ProyectoNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryRol
{
    //Task<ICollection<Rol>> FindByDescriptionAsync(string description);
    Task<ICollection<Rol>> ListAsync();
    Task<Rol> FindByIdAsync(int id);
    Task<int> AddAsync(Rol entity);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, Rol entity);
}
