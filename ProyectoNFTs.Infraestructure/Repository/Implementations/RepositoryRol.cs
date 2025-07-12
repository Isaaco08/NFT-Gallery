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

public class RepositoryRol : IRepositoryRol
{
    private readonly ProyectoNFTsContext _context;

    public RepositoryRol(ProyectoNFTsContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(Rol entity)
    {
        await _context.Set<Rol>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.IdRol;
    }

    public async Task DeleteAsync(int id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        int rowAffected = _context.Database.ExecuteSql($"Delete Rol Where IdRol = {id}");
        await Task.FromResult(1);
    }

    /*public async Task<ICollection<Rol>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Rol>()
                                     .Where(p => p.DescripcionRol.Contains(description))
                                     .ToListAsync();
        return collection;
    }*/

    public async Task<Rol> FindByIdAsync(int id)
    {
        var @object = await _context.Set<Rol>().FindAsync(id);
        return @object!;
    }

    public async Task<ICollection<Rol>> ListAsync()
    {
        var collection = await _context.Set<Rol>().AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task UpdateAsync(int id, Rol entity)
    {
        var @object = await FindByIdAsync(id);
        @object.IdRol = entity.IdRol;
        @object.DescripcionRol = entity.DescripcionRol;
        await _context.SaveChangesAsync();
    }
}
