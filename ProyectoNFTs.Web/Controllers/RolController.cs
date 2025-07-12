using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Application.Services.Interfaces;

namespace ProyectoNFTs.Web.Controllers;

[Authorize(Roles = "Admin")]
public class RolController : Controller
{
    private readonly IServiceRol _serviceRol;

    public RolController(IServiceRol serviceRol)
    {
        _serviceRol = serviceRol;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var collection = await _serviceRol.ListAsync();
        return View(collection);
    }

    // GET: RolController/Create
    public IActionResult Create()
    {
        return View();
    }


    // POST: RolController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RolDTO dto)
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

        var existingRol = await _serviceRol.FindByIdAsync(dto.IdRol);
        if (existingRol != null)
        {
            //// El Rol ya existe, enviar un mensaje de error
            return Json(new { success = false, IdRol = dto.IdRol }); // Incluir el ID en la respuesta
        }

        // La Rol no existe, agregarla normalmente
        await _serviceRol.AddAsync(dto);
        return Json(new { success = true }); // Devolver un JSON para indicar que la operación fue exitosa

    }

    // GET: RolController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var @object = await _serviceRol.FindByIdAsync(id);

        return PartialView(@object);
    }

    // GET: RolController/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var @object = await _serviceRol.FindByIdAsync(id);

        return View(@object);
    }

    // POST: RolController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RolDTO dto)
    {

        await _serviceRol.UpdateAsync(id, dto);

        return RedirectToAction("Index");

    }

    // GET: RolController/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        await _serviceRol.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    // POST: RolController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, IFormCollection collection)
    {
        await _serviceRol.DeleteAsync(id);

        return RedirectToAction("Index");
    }

    public async Task<JsonResult> GetRolById(int id)
    {
        var Rol = await _serviceRol.FindByIdAsync(id);

        if (Rol != null)
        {
            // Aquí, estás devolviendo directamente el objeto 'Rol' como JSON.
            return Json(Rol);
        }
        else
        {
            // Si no se encuentra el país, puedes devolver un objeto JSON indicando un mensaje de error o algo similar.
            return Json(new { error = "No se encontró el país con el ID especificado." });
        }
    }
}
