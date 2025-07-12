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

public class ServicePais : IServicePais
{
    private readonly IRepositoryPais _repository;
    private readonly IMapper _mapper;

    public ServicePais(IRepositoryPais repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> AddAsync(PaisDTO dto)
    {
        // Map PaisDTO to Pais
        var objectMapped = _mapper.Map<Pais>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<ICollection<PaisDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<PaisDTO>>(list);
        return collection;

    }

    public async Task<PaisDTO> FindByIdAsync(int id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<PaisDTO>(@object);
        return objectMapped;
    }

    public async Task<ICollection<PaisDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Pais> to ICollection<PaisDTO>
        var collection = _mapper.Map<ICollection<PaisDTO>>(list);
        // Return Data
        return collection;
    }

    public async Task UpdateAsync(int id, PaisDTO dto)
    {
        var objectMapped = _mapper.Map<Pais>(dto);
        await _repository.UpdateAsync(id, objectMapped);
    }
}
