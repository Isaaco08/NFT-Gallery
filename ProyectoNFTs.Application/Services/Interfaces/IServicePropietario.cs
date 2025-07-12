using ProyectoNFTs.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Services.Interfaces;

public interface IServicePropietario
{
    Task<ICollection<PropietarioNftDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<PropietarioNftDTO>> ListAsync();
    Task<PropietarioNftDTO> FindByIdAsync(int id);
    Task<int> AddAsync(PropietarioNftDTO dto);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, PropietarioNftDTO dto);
    Task<ICollection<PropietarioNftDTO>> FindByIdNFT(Guid id);
}
