using Microsoft.EntityFrameworkCore;
using ProyectoNFTs.Infraestructure.Data;
using ProyectoNFTs.Infraestructure.Models;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Infraestructure.Repository.Implementations;

public class RepositoryPropietario : IRepositoryPropietario
{
    private readonly ProyectoNFTsContext _context;

    public RepositoryPropietario(ProyectoNFTsContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(PropietarioNft entity)
    {
        await _context.Set<PropietarioNft>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteAsync(int id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        int rowAffected = _context.Database.ExecuteSql($"Delete PropietarioNft Where Id = {id}");
        await Task.FromResult(1);
    }

    public async Task<ICollection<PropietarioNft>> FindByDescriptionAsync(string description)
    {
        var propietarios = await _context.PropietarioNft
            .Where(p => p.IdNft == _context.Nft.FirstOrDefault(n => n.Nombre == description).IdNft)
            .ToListAsync();

        return propietarios;
    }

    public async Task<PropietarioNft> FindByIdAsync(int id)
    {
        var @object = await _context.Set<PropietarioNft>().FindAsync(id);
        return @object!;
    }

    public async Task<ICollection<PropietarioNft>> ListAsync()
    {
        var collection = await _context.Set<PropietarioNft>().AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task UpdateAsync(int id, PropietarioNft entity)
    {
        var @object = await FindByIdAsync(id);
        @object.Id = entity.Id;
        @object.FechaPropiedad = entity.FechaPropiedad;
        @object.IdNft = entity.IdNft;
        @object.IdCliente = entity.IdCliente;
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<PropietarioNft>> FindByIdNFT(Guid id)
    {
        var colletion = await _context.Set<PropietarioNft>()
                                        .Where(p => p.IdNft == id)
                                        .ToListAsync();
        return colletion;
    }

    /*public async Task<ICollection<PropietarioNft>> FindByNftAsync(string filtro)
    {
        filtro = filtro.Replace(' ', '%');
        filtro = "%" + filtro + "%";
        FormattableString sql = $@"select * from PropietarioNft where Id like  {filtro}  ";

        var collection = await _context.Nft.FromSql(sql).AsNoTracking().ToListAsync();
        return collection;
    }*/
}
