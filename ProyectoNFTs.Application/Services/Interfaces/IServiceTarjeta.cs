using ProyectoNFTs.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Services.Interfaces;

public interface IServiceTarjeta
{
    Task<ICollection<TarjetaDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<TarjetaDTO>> ListAsync();
    Task<TarjetaDTO> FindByIdAsync(int id);
    Task<int> AddAsync(TarjetaDTO dto);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, TarjetaDTO dto);
}
