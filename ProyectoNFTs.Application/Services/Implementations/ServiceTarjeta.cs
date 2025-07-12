using AutoMapper;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Infraestructure.Models;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.Services.Implementations;

public class ServiceTarjeta : IServiceTarjeta
{
    private readonly IRepositoryTarjeta _repository;
    private readonly IMapper _mapper;

    public ServiceTarjeta(IRepositoryTarjeta repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> AddAsync(TarjetaDTO dto)
    {
        // Map TarjetaDTO to Pais
        var objectMapped = _mapper.Map<Tarjeta>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<ICollection<TarjetaDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<TarjetaDTO>>(list);
        return collection;

    }

    public async Task<TarjetaDTO> FindByIdAsync(int id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<TarjetaDTO>(@object);
        return objectMapped;
    }

    public async Task<ICollection<TarjetaDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Tarjeta> to ICollection<TarjetaDTO>
        var collection = _mapper.Map<ICollection<TarjetaDTO>>(list);
        // Return Data
        return collection;
    }

    public async Task UpdateAsync(int id, TarjetaDTO dto)
    {
        var objectMapped = _mapper.Map<Tarjeta>(dto);
        await _repository.UpdateAsync(id, objectMapped);
    }
}
