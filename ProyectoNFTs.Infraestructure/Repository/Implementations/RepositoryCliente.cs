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

public class RepositoryCliente : IRepositoryCliente
{
    private readonly ProyectoNFTsContext _context;

    public RepositoryCliente(ProyectoNFTsContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Cliente entity)
    {
        await _context.Set<Cliente>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.IdCliente;
    }

    public async Task DeleteAsync(Guid id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        int rowAffected = _context.Database.ExecuteSql($"Delete Cliente Where IdCliente = {id}");
        await Task.FromResult(1);
    }

    public async Task<ICollection<Cliente>> FindByDescriptionAsync(string description)
    {
        description = description.Replace(' ', '%');
        description = "%" + description + "%";
        FormattableString sql = $@"select * from Cliente where Nombre+Apellido1+Apellido2 like  {description}  ";

        var collection = await _context.Cliente.FromSql(sql).AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task<Cliente> FindByIdAsync(Guid id)
    {
        var @object = await _context.Set<Cliente>().FindAsync(id);
        return @object!;
    }

    public async Task<ICollection<Cliente>> ListAsync()
    {
        var collection = await _context.Set<Cliente>().AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task UpdateAsync(Guid id, Cliente entity)
    {
        var @object = await FindByIdAsync(id);
        @object.Nombre = entity.Nombre;
        @object.Apellido1 = entity.Apellido1;
        @object.Apellido2 = entity.Apellido2;
        @object.Email = entity.Email;
        @object.Sexo = entity.Sexo;
        @object.FechaNacimiento = entity.FechaNacimiento;
        @object.IdPais = entity.IdPais;
        await _context.SaveChangesAsync();
    }
}
