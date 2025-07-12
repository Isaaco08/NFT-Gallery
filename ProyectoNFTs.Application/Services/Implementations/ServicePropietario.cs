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

public class ServicePropietario : IServicePropietario
{
    private readonly IRepositoryPropietario _repository;
    private readonly IMapper _mapper;

    public ServicePropietario(IRepositoryPropietario repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> AddAsync(PropietarioNftDTO dto)
    {
        // Map NftDTO to Nft
        var objectMapped = _mapper.Map<PropietarioNft>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<ICollection<PropietarioNftDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<PropietarioNftDTO>>(list);
        return collection;

    }

    public async Task<PropietarioNftDTO> FindByIdAsync(int id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<PropietarioNftDTO>(@object);
        return objectMapped;
    }

    public async Task<ICollection<PropietarioNftDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Nft> to ICollection<NftDTO>
        var collection = _mapper.Map<ICollection<PropietarioNftDTO>>(list);
        // Return Data
        return collection;
    }

    public async Task UpdateAsync(int id, PropietarioNftDTO dto)
    {
        var objectMapped = _mapper.Map<PropietarioNft>(dto);
        await _repository.UpdateAsync(id, objectMapped);
    }

    public async Task<ICollection<PropietarioNftDTO>> FindByIdNFT(Guid id)
    {
        var list = await _repository.FindByIdNFT(id);
        var colletion = _mapper.Map<ICollection<PropietarioNftDTO>>(list);
        return colletion;
    }
}
