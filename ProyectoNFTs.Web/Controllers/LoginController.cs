using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoNFTs.Application.Services.Interfaces;
using ProyectoNFTs.Web.ViewModels;
using System.Security.Claims;

namespace ProyectoNFTs.Web.Controllers;


public class LoginController : Controller
{

    private readonly IServiceUsuario _serviceUsuario;
    private readonly ILogger<LoginController> _logger;
    public LoginController(IServiceUsuario serviceUsuario, ILogger<LoginController> logger)
    {
        _serviceUsuario = serviceUsuario;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(ViewModelLogin viewModelLogin)
    {

        if (!ModelState.IsValid)
        {
            // Lee del ModelState todos los errores que
            // vienen para el lado del server
            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            ViewBag.Message = $"Error de Acceso {errors}";

            _logger.LogInformation($"Error en login de {viewModelLogin}, Errores --> {errors}");
            return View("Index");
        }
        // User exist ?
        var usuarioDTO = await _serviceUsuario.LoginAsync(viewModelLogin.User, viewModelLogin.Password);
        if (usuarioDTO == null)
        {
            ViewBag.Message = "Error en acceso";
            _logger.LogInformation($"Error en login de {viewModelLogin.User}, Error --> {ViewBag.Message}");
            return View("Index");
        }

        // Agregar validación para el estado del usuario
        if (usuarioDTO.Estado == 0)
        {
            ViewBag.Message = "El usuario está inactivo";
            _logger.LogInformation($"Error en login de {viewModelLogin.User}, Error --> {ViewBag.Message}");
            return View("Index");
        }

        // Claim stores  User information like Name, role and others.  
        List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, usuarioDTO.Nombre+" "+usuarioDTO.Apellidos+" "+ usuarioDTO.Login),
                new Claim(ClaimTypes.Role, usuarioDTO.DescripcionRol!),
            };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties properties = new AuthenticationProperties()
        {
            AllowRefresh = true
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            properties);

        _logger.LogInformation($"Conexion correcta de {viewModelLogin.User}");

        //// Agrega el mensaje de bienvenida al ViewBag
        //ViewBag.Mensaje = $"¡Bienvenido(a) , {usuarioDTO.Nombre} {usuarioDTO.Apellidos}!";
        //ViewBag.UserName = usuarioDTO.Login;


        return RedirectToAction("Index", "Home");
    }

    /*Only user connected can logoff*/
    [Authorize]
    public async Task<IActionResult> LogOff()
    {
        _logger.LogInformation($"Desconexion correcta de {User!.Identity!.Name}");
        await HttpContext.SignOutAsync();

        return RedirectToAction("Index", "Login");
    }
}
