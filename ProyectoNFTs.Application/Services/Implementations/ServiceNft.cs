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

public class ServiceNft : IServiceNft
{
    private readonly IRepositoryNft _repository;
    private readonly IMapper _mapper;

    public ServiceNft(IRepositoryNft repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Guid> AddAsync(NftDTO dto)
    {
        // Map NftDTO to Nft
        var objectMapped = _mapper.Map<Nft>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<ICollection<NftDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<NftDTO>>(list);
        return collection;

    }

    public async Task<NftDTO> FindByIdAsync(Guid id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<NftDTO>(@object);
        return objectMapped;
    }

    public async Task<ICollection<NftDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Nft> to ICollection<NftDTO>
        var collection = _mapper.Map<ICollection<NftDTO>>(list);
        // Return Data
        return collection;
    }

    public async Task UpdateAsync(Guid id, NftDTO dto)
    {
        var objectMapped = _mapper.Map<Nft>(dto);
        await _repository.UpdateAsync(id, objectMapped);
    }
}
