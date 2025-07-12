using ProyectoNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryCliente
{
    Task<ICollection<Cliente>> FindByDescriptionAsync(string description);
    Task<ICollection<Cliente>> ListAsync();
    Task<Cliente> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(Cliente entity);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, Cliente entity);

}
