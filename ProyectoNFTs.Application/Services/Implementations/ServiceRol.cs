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

public class ServiceRol : IServiceRol
{
    private readonly IRepositoryRol _repository;
    private readonly IMapper _mapper;

    public ServiceRol(IRepositoryRol repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> AddAsync(RolDTO dto)
    {
        // Map RolDTO to Rol
        var objectMapped = _mapper.Map<Rol>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    /*public async Task<ICollection<RolDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<RolDTO>>(list);
        return collection;

    }*/

    public async Task<RolDTO> FindByIdAsync(int id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<RolDTO>(@object);
        return objectMapped;
    }

    public async Task<ICollection<RolDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Rol> to ICollection<RolDTO>
        var collection = _mapper.Map<ICollection<RolDTO>>(list);
        // Return Data
        return collection;
    }

    public async Task UpdateAsync(int id, RolDTO dto)
    {
        var objectMapped = _mapper.Map<Rol>(dto);
        await _repository.UpdateAsync(id, objectMapped);
    }
}
