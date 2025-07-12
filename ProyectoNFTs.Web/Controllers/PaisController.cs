using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Application.Services.Implementations;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Infraestructure.Models;
using X.PagedList;

namespace ProyectoNFTs.Web.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class PaisController : Controller
{
    private readonly IServicePais _servicePais;

    public PaisController(IServicePais servicePais)
    {
        _servicePais = servicePais;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        var collection = await _servicePais.ListAsync();
        return View(collection.ToPagedList(page ?? 1, 5));
    }

    // GET: PaisController/Create
    public IActionResult Create()
    {
        return View();
    }


    // POST: PaisController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaisDTO dto)
    {

        if (!ModelState.IsValid)
        {
            // Lee del ModelState todos los errores que
            // vienen para el lado del server
            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            return BadRequest(errors);
        }

        var existingPais = await _servicePais.FindByIdAsync(dto.IdPais);
        if (existingPais != null)
        {
            //// El Pais ya existe, enviar un mensaje de error
            return Json(new { success = false, IdPais = dto.IdPais }); // Incluir el ID en la respuesta
        }

        // La Pais no existe, agregarla normalmente
        await _servicePais.AddAsync(dto);
        return Json(new { success = true }); // Devolver un JSON para indicar que la operación fue exitosa

    }

    // GET: PaisController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var @object = await _servicePais.FindByIdAsync(id);

        return PartialView(@object);
    }

    // GET: PaisController/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var @object = await _servicePais.FindByIdAsync(id);

        return View(@object);
    }

    // POST: PaisController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PaisDTO dto)
    {

        await _servicePais.UpdateAsync(id, dto);

        return RedirectToAction("Index");

    }

    // GET: PaisController/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        await _servicePais.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    // POST: PaisController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, IFormCollection collection)
    {
        await _servicePais.DeleteAsync(id);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> List(string descripcion)
    {
        if (string.IsNullOrEmpty(descripcion))
            return RedirectToAction("Index");

        var collection = await _servicePais.FindByDescriptionAsync(descripcion);
        return View("index", collection.ToPagedList(1, 10));
    }

    public async Task<JsonResult> GetPaisById(int id)
    {
        var pais = await _servicePais.FindByIdAsync(id);

        if (pais != null)
        {
            // Aquí, estás devolviendo directamente el objeto 'pais' como JSON.
            return Json(pais);
        }
        else
        {
            // Si no se encuentra el país, puedes devolver un objeto JSON indicando un mensaje de error o algo similar.
            return Json(new { error = "No se encontró el país con el ID especificado." });
        }
    }

}
