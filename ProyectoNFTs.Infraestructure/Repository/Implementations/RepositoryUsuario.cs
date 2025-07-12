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

public class RepositoryUsuario : IRepositoryUsuario
{
    private readonly ProyectoNFTsContext _context;

    public RepositoryUsuario(ProyectoNFTsContext context)
    {
        _context = context;
    }

    public async Task<string> AddAsync(Usuario entity)
    {
        await _context.Set<Usuario>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Login;
    }

    public async Task DeleteAsync(string id)
    {

        var @object = await FindByIdAsync(id);
        _context.Remove(@object);
        _context.SaveChanges();
    }

    public async Task<ICollection<Usuario>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Usuario>()
                                     .Where(p => p.Nombre.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    public async Task<Usuario> FindByIdAsync(string id)
    {
        var @object = await _context.Set<Usuario>().FindAsync(id);

        return @object!;
    }

    public async Task<ICollection<Usuario>> ListAsync()
    {
        var collection = await _context.Set<Usuario>()
                                       .Include(b => b.IdRolNavigation)
                                       .AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task<Usuario> LoginAsync(string id, string password)
    {
        var @object = await _context.Set<Usuario>()
                                    .Include(b => b.IdRolNavigation)
                                    .Where(p => p.Login == id && p.Password == password)
                                    .FirstOrDefaultAsync();
        return @object!;
    }

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }
}
