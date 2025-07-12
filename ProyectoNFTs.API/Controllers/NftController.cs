using Microsoft.AspNetCore.Mvc;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Application.Services.Interfaces;

namespace ProyectoNfts.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NftController : Controller
{
    private readonly IServiceNft _serviceNft;
    private readonly IServicePropietario _servicePropietario;
    private readonly IServiceCliente _serviceCliente;

    public NftController(IServiceNft servicioNft, IServicePropietario servicePropietario, IServiceCliente serviceCliente)
    {
        _serviceNft = servicioNft;
        _servicePropietario = servicePropietario;
        _serviceCliente = serviceCliente;
    }

    [HttpGet("Nft")]
    public async Task<IActionResult> GetNft()
    {
        var collection = await _serviceNft.ListAsync();
        var result = collection.Select(nft => new
        {
            nft.IdNft,
            nft.Nombre,
            nft.Imagen
        })
        .Where(nft => nft.IdNft != null && nft.Nombre != null && nft.Imagen != null);

        return Ok(result);
    }

    [HttpGet("Nft/nombre/{nombre}")]
    public async Task<IActionResult> GetNftByDescription(string nombre)
    {
        var collection = await _serviceNft.FindByDescriptionAsync(nombre);
        var collectionPropietario = await _servicePropietario.ListAsync();

        var result = new List<object>();

        foreach (var nft in collection)
        {
            if (nft.IdNft != null && nft.Nombre != null && nft.Imagen != null)
            {
                var propietario = collectionPropietario.FirstOrDefault(p => p.IdNft == nft.IdNft);

                if (propietario != null)
                {
                    var cliente = await _serviceCliente.FindByIdAsync(propietario.IdCliente);
                    result.Add(new
                    {
                        nft.IdNft,
                        nft.Nombre,
                        nft.Imagen,
                        Cliente = cliente
                    });
                }
                else
                {
                    result.Add(new
                    {
                        nft.IdNft,
                        nft.Nombre,
                        nft.Imagen,

                    });
                }
            }
        }

        if (result.Any())
            return Ok(result);
        else
            return NotFound($"No existen elementos con la descripción {nombre}");
    }



    //[HttpGet("Nft/{id}")]
    //public async Task<IActionResult> GetNftById(Guid id)
    //{
    //    var @object = await _serviceNft.FindByIdAsync(id);

    //    if (@object != null)
    //        return Ok(@object);
    //    else
    //        return NotFound($"No existe {id}");

    //}

    //[HttpPost("Nft/create")]
    //public async Task<IActionResult> create(NftDTO dto)
    //{
    //    _ = await _serviceNft.AddAsync(dto);
    //    return Ok();
    //}

    //[HttpDelete("Nft/{id}")]
    //public async Task<IActionResult> Delete(Guid id)
    //{
    //    await _serviceNft.DeleteAsync(id);
    //    return Ok();
    //}

    //[HttpPut("Nft/{id}")]
    //public async Task<IActionResult> Update(Guid id, NftDTO dto)
    //{
    //    await _serviceNft.UpdateAsync(id, dto);
    //    return Ok();
    //}
}
