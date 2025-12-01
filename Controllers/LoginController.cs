using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_TomaSilva1.Interfaces;
using tl2_tp8_2025_TomaSilva1.Models;
using tl2_tp8_2025_TomaSilva1.Services;
using tl2_tp8_2025_TomaSilva1.Repositorios;
using tl2_tp8_2025_TomaSilva1.ViewModels;

namespace tl2_tp8_2025_TomaSilva1.Controllers;

public class LoginController : Controller
{
    private readonly IAuthenticationService _auth;

    public LoginController(IAuthenticationService auntetificacion)
    {

        _auth = auntetificacion;
    }

    [HttpGet]
    public IActionResult Index()
    {

        return View(new LoginViewModel());
    }


    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {

        if (!ModelState.IsValid)
    {
        // Los mensajes de error de los Data Annotations viajan con el 'model'.
        return View("Index", model); 
    }

        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
           // model.ErrorMessage = "Debe ingresar usuario y contraseña.";

            return View("Index", model);
        }
        if (_auth.Login(model.Username, model.Password))
        {
            return RedirectToAction("Index", "Home");
        }


        
        model.ErrorMessage = "Usuario o contraseña incorrecto.";
        
        return View("Index", model);
    }
    // [HttpGet] Cierra sesión
    public IActionResult Logout()
    {
        _auth.Logout();
        return RedirectToAction("Index");
    }
}