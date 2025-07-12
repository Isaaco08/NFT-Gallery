using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Application.Services.Implementations;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using X.PagedList;

namespace ProyectoNFTs.Web.Controllers;

public class ClienteController : Controller
{
    private readonly IServicePais _servicePais;
    private readonly IServiceCliente _serviceCliente;
    private IRepositoryPais _repostoryPais;

    public ClienteController(IServiceCliente serviceCliente, IRepositoryPais repositoryPais, IServicePais servicePais)
    {
        _serviceCliente = serviceCliente;
        _repostoryPais = repositoryPais;
        _servicePais = servicePais;
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {

        var collection = await _serviceCliente.ListAsync();
        return View(collection.ToPagedList(page ?? 1, 5));
    }

    // GET: ClienteController/Create
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create()
    {
        ViewBag.ListPais = await _repostoryPais.ListAsync();
        return View();
    }

    // POST: ClienteController/Create
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClienteDTO dto)
    {
        dto.IdCliente = Guid.NewGuid();

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

        //// Verificar si la Nft ya existe en la base de datos
        //var existingNft = await _serviceCliente.FindByIdAsync(dto.IdCliente);
        //if (existingNft != null)
        //{
        //    //// La Nft ya existe, enviar un mensaje de error
        //    return Json(new { success = false, idCliente = dto.IdCliente }); // Incluir el ID en la respuesta
        //}

        // La cliente no existe, agregarla normalmente
        await _serviceCliente.AddAsync(dto);
        return Json(new { success = true }); // Devolver un JSON para indicar que la operación fue exitosa
    }

    // GET: ClienteController/Details/5
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Details(Guid id)
    {
        var @object = await _serviceCliente.FindByIdAsync(id);
        var pais = await _servicePais.FindByIdAsync(@object.IdPais);
        ViewBag.Pais = pais;
        return PartialView(@object);
    }

    [Authorize(Roles = "Admin,Manager")]
    // GET: ClienteController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.ListPais = await _repostoryPais.ListAsync();
        var @object = await _serviceCliente.FindByIdAsync(id);

        return View(@object);
    }

    // POST: ClienteController/Edit/5
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ClienteDTO dto)
    {

        await _serviceCliente.UpdateAsync(id, dto);
        return RedirectToAction("Index");
    }

    // GET: ClienteController/Delete/5
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(Guid id)
    {
        //var @object = await _serviceCliente.FindByIdAsync(id);

        //return View(@object);
        await _serviceCliente.DeleteAsync(id);

        return RedirectToAction("Index");
    }

    // POST: ClienteController/Delete/5
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
    {
        await _serviceCliente.DeleteAsync(id);

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> List(string descripcion)
    {
        if (string.IsNullOrEmpty(descripcion))
            return RedirectToAction("Index");

        var collection = await _serviceCliente.FindByDescriptionAsync(descripcion);
        return View("index", collection.ToPagedList(1, 10));
    }

    [Authorize(Roles = "Admin,Manager")]
    public async Task<JsonResult> GetClienteById(Guid id)
    {
        var cliente = await _serviceCliente.FindByIdAsync(id);

        if (cliente != null)
        {
            // Aquí, estás devolviendo directamente el objeto 'Rol' como JSON.
            return Json(cliente);
        }
        else
        {
            // Si no se encuentra el país, puedes devolver un objeto JSON indicando un mensaje de error o algo similar.
            return Json(new { error = "No se encontró el país con el ID especificado." });
        }
    }

    [Authorize(Roles = "Admin,Manager,Cashier")]
    //Busca el cliente segun lo que entre en el filtro
    public IActionResult GetClienteByName(string filtro)
    {
        var collections = _serviceCliente.FindByDescriptionAsync(filtro).GetAwaiter().GetResult();
        return Json(collections);
    }
}
