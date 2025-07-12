using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Application.Services.Implementations;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using X.PagedList;

namespace ProyectoNFTs.Web.Controllers;

[Authorize(Roles = "Admin,Manager")]

public class NftController : Controller
{
    private readonly IServiceNft _serviceNft;

    public NftController(IServiceNft serviceNft)
    {
        _serviceNft = serviceNft;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {

        var collection = await _serviceNft.ListAsync();
        return View(collection.ToPagedList(page ?? 1, 5));
    }

    // GET: NftController/Create
    public async Task<IActionResult> Create()
    {
        
        return View();
    }

    // POST: NftController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NftDTO dto, IFormFile imageFile)
    {
        dto.IdNft = Guid.NewGuid();

        MemoryStream target = new MemoryStream();

        // Cuando es Insert Image viene en null porque se pasa diferente
        if (dto.Imagen == null)
        {
            if (imageFile != null)
            {
                imageFile.OpenReadStream().CopyTo(target);

                dto.Imagen = target.ToArray();
                ModelState.Remove("Imagen");
            }
        }

        if (!ModelState.IsValid)
        {
            // Lee del ModelState todos los errores que
            // vienen para el lado del server
            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            // Response errores
            return BadRequest(errors);
        }
        // Verificar si la Nft ya existe en la base de datos
        var existingNft = await _serviceNft.FindByIdAsync(dto.IdNft);
        if (existingNft != null)
        {
            //// La Nft ya existe, enviar un mensaje de error
            return Json(new { success = false, idNft = dto.IdNft }); // Incluir el ID en la respuesta
        }

        // La Nft no existe, agregarla normalmente
        await _serviceNft.AddAsync(dto);
        return Json(new { success = true }); // Devolver un JSON para indicar que la operación fue exitosa
    }


    // GET: NftController/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var @object = await _serviceNft.FindByIdAsync(id);
        return PartialView(@object);
    }

    // GET: NftController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var @object = await _serviceNft.FindByIdAsync(id);

        return View(@object);
    }

    // POST: NftController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NftDTO dto, IFormFile imageFile)
    {
        MemoryStream target = new MemoryStream();

        // Cuando es Insert Image viene en null porque se pasa diferente
        if (imageFile != null && imageFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                dto.Imagen = memoryStream.ToArray();
            }
        }
        else
        {
            NftDTO nft = await _serviceNft.FindByIdAsync(id);
            dto.Imagen = nft.Imagen;
        }

        //if (!ModelState.IsValid)
        //{
        //    // Lee del ModelState todos los errores que
        //    // vienen para el lado del server
        //    string errors = string.Join("; ", ModelState.Values
        //                       .SelectMany(x => x.Errors)
        //                       .Select(x => x.ErrorMessage));
        //    // Response errores
        //    return BadRequest(errors);
        //}

        await _serviceNft.UpdateAsync(id, dto);
        return RedirectToAction("Index");
    }

    // GET: NftController/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        //var @object = await _serviceNft.FindByIdAsync(id);

        //return View(@object);
        await _serviceNft.DeleteAsync(id);

        return RedirectToAction("Index");
    }

    // POST: NftController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
    {
        await _serviceNft.DeleteAsync(id);

        return RedirectToAction("Index");
    }

    //Busca el Nft segun lo que entre en el filtro
    public IActionResult GetNftByName(string filtro)
    {
        var collections = _serviceNft.FindByDescriptionAsync(filtro).GetAwaiter().GetResult();
        return Json(collections);
    }

    public async Task<IActionResult> List(string descripcion)
    {
        if (string.IsNullOrEmpty(descripcion))
            return RedirectToAction("Index");

        var collection = await _serviceNft.FindByDescriptionAsync(descripcion);
        return View("index", collection.ToPagedList(1, 10));
    }

}
