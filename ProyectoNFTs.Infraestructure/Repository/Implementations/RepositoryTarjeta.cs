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

public class RepositoryTarjeta : IRepositoryTarjeta
{
    private readonly ProyectoNFTsContext _context;

    public RepositoryTarjeta(ProyectoNFTsContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(Tarjeta entity)
    {
        await _context.Set<Tarjeta>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.IdTarjeta;
    }

    public async Task DeleteAsync(int id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        int rowAffected = _context.Database.ExecuteSql($"Delete Tarjeta Where IdTarjeta = {id}");
        await Task.FromResult(1);
    }

    public async Task<ICollection<Tarjeta>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Tarjeta>()
                                     .Where(p => p.Descripcion.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    public async Task<Tarjeta> FindByIdAsync(int id)
    {
        var @object = await _context.Set<Tarjeta>().FindAsync(id);
        return @object!;
    }

    public async Task<ICollection<Tarjeta>> ListAsync()
    {
        var collection = await _context.Set<Tarjeta>().AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task UpdateAsync(int id, Tarjeta entity)
    {
        var @object = await FindByIdAsync(id);
        @object.IdTarjeta = entity.IdTarjeta;
        @object.Descripcion = entity.Descripcion;
        @object.Estado = entity.Estado;
        await _context.SaveChangesAsync();
    }
}
