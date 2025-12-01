using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_TomaSilva1.Models;
using SistemaVentas.Web.ViewModels; //Necesario para poder llegar a los ViewModels
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaVenta.Web.ViewModels; // Necesario para SelectList

namespace tl2_tp8_2025_TomaSilva1.Controllers;

using tl2_tp8_2025_TomaSilva1.Interfaces;

public class PresupuestosController : Controller
{
    private readonly IPresupuestoRepositorio _presupuestoRepositorio;
    private readonly IProductoRepositorio _productoRepositorio;
    private readonly IAuthenticationService _auth;
    public PresupuestosController(IPresupuestoRepositorio pres, IProductoRepositorio prod, IAuthenticationService auth)
    {
       // _presupuestoRepositorio = new PresupuestoRepositorio();
       // _productoRepositorio = new ProductoRepositorio();
       //porque ya no se usa new presupuesto repo? porque lo hace el builder

       _presupuestoRepositorio = pres;
       _productoRepositorio = prod;
       _auth = auth;
    }

    //A partir de aqui los get, post, ect.

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
        if (!_auth.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        if (_auth.HasAccessLevel("Administrador") || _auth.HasAccessLevel("Cliente"))
        {
            List<Presupuestos> presupuestos = _presupuestoRepositorio.obtenerPresupuestos();
            return View(presupuestos);
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Alta(string nombre, string fecha)
    {
        Presupuestos pre = new Presupuestos()
        {
            NombreDestinatario = nombre,
            FechaCreacion = fecha
        };
        _presupuestoRepositorio.crearPresupuesto(pre);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        int real = _presupuestoRepositorio.eliminarPresupuesto(id);

        if(real == -1)
        {
            return BadRequest("First");
        }else if(real == -2)
        {
            return BadRequest("Second");
        }
        
        return RedirectToAction("Index");
    }

    [HttpGet] 
    public IActionResult Details(int id)
    {
        var p = _presupuestoRepositorio.obtenerPresupuestoPorId(id);
        return View(p);
    }

    [HttpGet] //Obtengo el presupuesto por id y devuelvo a Details
    public IActionResult CreateDetalle(int id)
    {
        List<Productos> productos = _productoRepositorio.GetAll();
        
        var presupuesto = new AgregarProductoViewModel
        {
            IdPresupuesto = id,
            ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
        };

        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult CreateDetalle(AgregarProductoViewModel model)
    {
        foreach (var kvp in ModelState)
        {
            foreach (var error in kvp.Value.Errors)
            {
                Console.WriteLine($"Error en {kvp.Key}: {error.ErrorMessage}");
            }
        }
        
        if (!ModelState.IsValid)
        {
            // LÓGICA CRÍTICA DE RECARGA: Si falla la validación,
            // debemos recargar el SelectList porque se pierde en el POST
            var productos = _productoRepositorio.GetAll();
            model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
            return View(model);
        }

        _presupuestoRepositorio.agregarProductoAPresupuesto(model.IdProducto, model.IdPresupuesto, model.Cantidad);
        return RedirectToAction(nameof(Index));
        
        //se puede hacer tambien: return RedirectToAction(nameof(Details), new {id = model.IdPresupuesto})
    }
}