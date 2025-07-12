using ProyectoNFTs.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Services.Interfaces;

public interface IServicePais
{
    Task<ICollection<PaisDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<PaisDTO>> ListAsync();
    Task<PaisDTO> FindByIdAsync(int id);
    Task<int> AddAsync(PaisDTO dto);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, PaisDTO dto);
}