using ProyectoNFTs.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Services.Interfaces;

public interface IServiceCliente
{
    Task<ICollection<ClienteDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<ClienteDTO>> ListAsync();
    Task<ClienteDTO> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(ClienteDTO dto);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, ClienteDTO dto);

}
