using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_TomaSilva1.Models;
using SistemaVentas.Web.ViewModels;

namespace tl2_tp8_2025_TomaSilva1.Controllers;

using tl2_tp8_2025_TomaSilva1.Interfaces;
using tl2_tp8_2025_TomaSilva1.Models;
using tl2_tp8_2025_TomaSilva1.Repositorios;

public class ProductosController : Controller
{
    private IProductoRepositorio _productosRepositorio;
    private IAuthenticationService _auth;
    public ProductosController(IProductoRepositorio prod, IAuthenticationService auth)
    {
        //_productosRepositorio = new ProductoRepositorio();
        _productosRepositorio = prod;
        _auth = auth;
    }

    private IActionResult CheckAdminPermissions()
    {
        // 1. No logueado? -> vuelve al login
        if (!_auth.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. No es Administrador? -> Da Error
        if (!_auth.HasAccessLevel("Administrador"))
        {
            // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
            return RedirectToAction(nameof(AccesoDenegado));
        }
        return null; // Permiso concedido
    }

    public IActionResult AccesoDenegado()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        List<Productos> productos = _productosRepositorio.GetAll();
        return View(productos);
    }
    

    public IActionResult Details(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        Productos p = _productosRepositorio.obtenerProductoPorId(id);
        return View(p);        
    }

    public IActionResult Delete(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        _productosRepositorio.borrarProducto(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet] //Devuelve la vista del formulario de creacion.
    public IActionResult Create()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        return View();
    }

    [HttpPost] //Recibe los datos del formulario de creacion, los valida y guarda.
    public IActionResult Alta(ProductoViewModel p)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        if (!ModelState.IsValid)
        {
            return View(p);
        }

        var nuevoProducto = new Productos
        {
            Descripcion = p.Descripcion,
            Precio = p.Precio
        };
        
        _productosRepositorio.InsertarProducto(nuevoProducto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet] //Devuelve un formulario para editar un producto.
    public IActionResult AuxiliarEdit(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        Productos p = _productosRepositorio.obtenerProductoPorId(id);
        ProductoViewModel prod = new ProductoViewModel
        {
            IdProducto = id,
            Descripcion = p.Descripcion,
            Precio = p.Precio
        };
        return View(prod);
    }

    [HttpPost] //Valida el formulario para editar y guarda.
    public IActionResult AuxiliarEdit(int id, ProductoViewModel PVM)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        if (!ModelState.IsValid)
        {
            return View(PVM); //Si falla, devolvemos el ViewModel con los datos y error de la vista.
        }

        var nuevoProducto = new Productos()
        {
            Descripcion = PVM.Descripcion,
            Precio = PVM.Precio
        };
        int real = _productosRepositorio.editarProducto(id, nuevoProducto);

        if(real > 0)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return BadRequest("NOO");
        }
    }
}