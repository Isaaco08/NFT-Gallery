using Microsoft.EntityFrameworkCore;
using ProyectoNFTs.Infraestructure.Data;
using ProyectoNFTs.Infraestructure.Models;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using ProyectoNFTs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Infraestructure.Repository.Implementations;

public class RepositoryNft : IRepositoryNft
{
    private readonly ProyectoNFTsContext _context;

    public RepositoryNft(ProyectoNFTsContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Nft entity)
    {
        await _context.Set<Nft>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.IdNft;
    }

    public async Task DeleteAsync(Guid id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        int rowAffected = _context.Database.ExecuteSql($"Delete NFT Where IdNft = {id}");
        await Task.FromResult(1);
    }


    public async Task<ICollection<Nft>> FindByDescriptionAsync(string description)
    {
        description = description.Replace(' ', '%');
        description = "%" + description + "%";
        FormattableString sql = $@"select * from Nft where Nombre like  {description}  ";

        var collection = await _context.Nft.FromSql(sql).AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task<Nft> FindByIdAsync(Guid id)
    {
        var @object = await _context.Set<Nft>().FindAsync(id);
        return @object!;
    }

    public async Task<ICollection<Nft>> ListAsync()
    {
        var collection = await _context.Set<Nft>().AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task UpdateAsync(Guid id, Nft entity)
    {
        var @object = await FindByIdAsync(id);
        @object.Nombre = entity.Nombre;
        @object.Autor = entity.Autor;
        @object.Precio = entity.Precio;
        @object.Cantidad = entity.Cantidad;
        @object.Imagen = entity.Imagen;
        await _context.SaveChangesAsync();
    }

    
}
