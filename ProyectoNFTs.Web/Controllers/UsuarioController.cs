using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoNFTs.Application.DTOs;
using ProyectoNFTs.Application.Services.Implementations;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Infraestructure.Repository.Implementations;
using ProyectoNFTs.Infraestructure.Repository.Interfaces;
using System.Security.Cryptography;
using X.PagedList;

namespace ProyectoNFTs.Web.Controllers;

[Authorize(Roles = "Admin")]
public class UsuarioController : Controller
{
    private readonly IServiceRol _serviceRol;
    private readonly IServiceUsuario _serviceUsuario;

    public UsuarioController(IServiceUsuario serviceUsuario, IServiceRol serviceRol)
    {
        _serviceUsuario = serviceUsuario;
        _serviceRol = serviceRol;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        //Codigo usado para interceptar una nueva clave Secret
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] secretKey = new byte[32]; // 256 bits
            rng.GetBytes(secretKey);
            string secret = BitConverter.ToString(secretKey).Replace("-", "");
            Console.WriteLine("Secret: " + secret);
        }
        var collection = await _serviceUsuario.ListAsync();
        return View(collection.ToPagedList(page ?? 1, 5));
    }


    // GET:  
    public async Task<IActionResult> Login(string id, string password)
    {
        var @object = await _serviceUsuario.LoginAsync(id, password);
        if (@object == null)
        {
            ViewBag.Message = "Error en Login o Password";
            return View("Login");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: UsuarioController/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.ListRol = await _serviceRol.ListAsync();
        return View();
    }

    // POST: UsuarioController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioDTO dto)
    {
 

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
        var existingUser = await _serviceUsuario.FindByIdAsync(dto.Login);
        if (existingUser != null)
        {
            //// La Nft ya existe, enviar un mensaje de error
            return Json(new { success = false, login = dto.Login }); // Incluir el ID en la respuesta
        }

        // La Usuario no existe, agregarla normalmente
        await _serviceUsuario.AddAsync(dto);
        return Json(new { success = true }); // Devolver un JSON para indicar que la operación fue exitosa
    }


    // GET: UsuarioController/Details/5

    // GET: UsuarioController/Details/5
    public async Task<IActionResult> Details(string id)
    {
        var @object = await _serviceUsuario.FindByIdAsync(id);
        var Rol = await _serviceRol.FindByIdAsync(@object.IdRol);
        ViewBag.Rol = Rol;
        return PartialView(@object);
    }

    // GET: UsuarioController/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        ViewBag.ListRol = await _serviceRol.ListAsync();
        var @object = await _serviceUsuario.FindByIdAsync(id);

        return View(@object);
    }

    // POST: UsuarioController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, UsuarioDTO dto)
    {

        await _serviceUsuario.UpdateAsync(id, dto);
        return RedirectToAction("Index");
    }

    // GET: UsuarioController/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        //var @object = await _serviceUsuario.FindByIdAsync(id);

        //return View(@object);
        await _serviceUsuario.DeleteAsync(id);

        return RedirectToAction("Index");
    }

    // POST: UsuarioController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id, IFormCollection collection)
    {
        await _serviceUsuario.DeleteAsync(id);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> List(string descripcion)
    {
        if (string.IsNullOrEmpty(descripcion))
            return RedirectToAction("Index");

        var collection = await _serviceUsuario.FindByDescriptionAsync(descripcion);
        return View("index", collection.ToPagedList(1, 10));
    }

    //Busca el Usuario segun lo que entre en el filtro
    public IActionResult GetUsuarioByName(string filtro)
    {
        var collections = _serviceUsuario.FindByDescriptionAsync(filtro).GetAwaiter().GetResult();
        return Json(collections);
    }
}
