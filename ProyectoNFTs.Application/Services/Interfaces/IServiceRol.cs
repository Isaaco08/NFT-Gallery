using ProyectoNFTs.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Services.Interfaces;

public interface IServiceRol
{
    Task<ICollection<RolDTO>> ListAsync();
    Task<RolDTO> FindByIdAsync(int id);
    Task<int> AddAsync(RolDTO dto);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, RolDTO dto);
}
