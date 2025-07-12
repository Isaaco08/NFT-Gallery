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

public class RepositoryPais : IRepositoryPais
{
    private readonly ProyectoNFTsContext _context;

    public RepositoryPais(ProyectoNFTsContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(Pais entity)
    {
        await _context.Set<Pais>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.IdPais;
    }

    public async Task DeleteAsync(int id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        int rowAffected = _context.Database.ExecuteSql($"Delete Pais Where IdPais = {id}");
        await Task.FromResult(1);
    }

    public async Task<ICollection<Pais>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Pais>()
                                     .Where(p => p.Descripcion.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    public async Task<Pais> FindByIdAsync(int id)
    {
        var @object = await _context.Set<Pais>().FindAsync(id);
        return @object!;
    }

    public async Task<ICollection<Pais>> ListAsync()
    {
        var collection = await _context.Set<Pais>().AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task UpdateAsync(int id, Pais entity)
    {
        var @object = await FindByIdAsync(id);
        @object.IdPais = entity.IdPais;
        @object.Descripcion = entity.Descripcion;
        @object.Alfa2 = entity.Alfa2;
        @object.Alfa3 = entity.Alfa3;
        await _context.SaveChangesAsync();
    }
}
