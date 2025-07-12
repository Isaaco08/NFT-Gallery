using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProyectoNFTs.Infraestructure.Data;
using ProyectoNFTs.Infraestructure.Models;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Infraestructure.Repository.Implementations;

public class RepositoryFactura : IRepositoryFactura
{
    private readonly ProyectoNFTsContext _context;
    private readonly ILogger<RepositoryFactura> _logger;

    public RepositoryFactura(ProyectoNFTsContext context, ILogger<RepositoryFactura> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ICollection<FacturaEncabezado>> ListAsync()
    {
        var collection = await _context.Set<FacturaEncabezado>().AsNoTracking().ToListAsync();
        return collection;
    }


    public async Task UpdateAsync(int id, FacturaEncabezado entity)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        int rowAffected = _context.Database.ExecuteSql($"UPDATE FacturaEncabezado SET EstadoFactura = 0 WHERE IdFactura = {id}");
        await Task.FromResult(1);
    }

    public async Task<int> AddAsync(FacturaEncabezado entity)
    {
        try
        {
            // Get No Receipt
            entity.IdFactura = GetNoReceipt();

            //set FechaFacturacion and EstadoFactura
            entity.EstadoFactura = 1;
            entity.FechaFacturacion = DateTime.Now;
            // Reenumerate
            entity.FacturaDetalle.ToList().ForEach(p => p.IdFactura = entity.IdFactura);
            // Begin Transaction
            await _context.Database.BeginTransactionAsync();
            await _context.Set<FacturaEncabezado>().AddAsync(entity);

            //// Withdraw from inventory
            //foreach (var item in entity.FacturaDetalle)
            //{
            //    // find the product
            //    var product = _context.Set<Nft>().Find(item.IdNft);
            //    // update stock
            //    product!.Cantidad = product.Cantidad - item.Cantidad;
            //    // update entity product
            //    _context.Set<Nft>().Update(product);
            //}

            //SqlException: Violation of PRIMARY KEY constraint 'PK_FacturaEncabezado'.Cannot insert duplicate key in object 'dbo.FacturaEncabezado'.The duplicate key value is (8).
            //Violation of PRIMARY KEY constraint 'PK_FacturaDetalle'.Cannot insert duplicate key in object 'dbo.FacturaDetalle'.The duplicate key value is (8, 1).
            //The statement has been terminated.
            //The statement has been terminated.


            await _context.SaveChangesAsync();
            // Commit
            await _context.Database.CommitTransactionAsync();

            return entity.IdFactura;
        }
        catch (Exception ex)
        {
            Exception exception = ex;

            _logger.LogError(ex, "Error AddAsync");
            // Rollback 
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }

    /// <summary>
    /// Get current NoReceipt without increment
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetNextReceiptNumber()
    {

        int current = 0;

        string sql = string.Format("SELECT current_value FROM sys.sequences WHERE name = 'ReceiptNumber'");

        System.Data.DataTable dataTable = new System.Data.DataTable();

        System.Data.Common.DbConnection connection = _context.Database.GetDbConnection();
        System.Data.Common.DbProviderFactory dbFactory = System.Data.Common.DbProviderFactories.GetFactory(connection!)!;
        using (var cmd = dbFactory!.CreateCommand())
        {
            cmd!.Connection = connection;
            cmd.CommandText = sql;
            using (System.Data.Common.DbDataAdapter adapter = dbFactory.CreateDataAdapter()!)
            {
                adapter.SelectCommand = cmd;
                adapter.Fill(dataTable);
            }
        }
        current = Convert.ToInt32(dataTable.Rows[0][0].ToString());
        return await Task.FromResult(current);
    }

    /// <summary>
    /// Get sequence in order to assign Receipt number.   
    /// Automaticaly INCREMENT ++
    /// http://technet.microsoft.com/es-es/library/ff878091.aspx
    /// CREATE SEQUENCE  ReceiptNumber  START WITH 1 INCREMENT BY 1 ;
    /// </summary>
    /// <returns>Num. de factura</returns>
    private int GetNoReceipt()
    {
        var list = _context.Database.SqlQueryRaw<Int64>($"SELECT NEXT VALUE FOR ReceiptNumber").ToList();
        var next = list[0];
        return (int)next;
    }

    public async Task<FacturaEncabezado> FindByIdAsync(int id)
    {
        var response = await _context.Set<FacturaEncabezado>()
                    .Include(detalle => detalle.FacturaDetalle)
                    .ThenInclude(detalle => detalle.IdNftNavigation)
                    .Include(cliente => cliente.IdClienteNavigation)
                    .Where(p => p.IdFactura == id).FirstOrDefaultAsync();
        return response!;
    }

    public async Task<ICollection<FacturaEncabezado>> ListByFechaAsync(DateTime fechaInicial, DateTime fechaFinal)
    {
        var collection = await _context.Set<FacturaEncabezado>()
        .AsNoTracking()
        .Include(detalle => detalle.FacturaDetalle)
        .Where(f => f.FechaFacturacion >= fechaInicial && f.FechaFacturacion <= fechaFinal)
        .ToListAsync();

        return collection;
    }

    public async Task<ICollection<FacturaEncabezado>> BillsByClientIdAsync(Guid id)
    {
        var response = await _context.Set<FacturaEncabezado>()
                       .Include(p => p.FacturaDetalle)
                       .Where(p => p.IdCliente == id).ToListAsync();

        return response;
    }
}
