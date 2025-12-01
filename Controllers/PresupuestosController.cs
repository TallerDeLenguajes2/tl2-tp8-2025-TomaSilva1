using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_TomaSilva1.Models;
using SistemaVentas.Web.ViewModels; //Necesario para poder llegar a los ViewModels
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaVenta.Web.ViewModels; // Necesario para SelectList

namespace tl2_tp8_2025_TomaSilva1.Controllers;

public class PresupuestosController : Controller
{
    private readonly PresupuestoRepositorio _presupuestoRepositorio;
    private readonly ProductoRepositorio _productoRepositorio;
    public PresupuestosController()
    {
        _presupuestoRepositorio = new PresupuestoRepositorio();
        _productoRepositorio = new ProductoRepositorio();
    }

    //A partir de aqui los get, post, ect.

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = _presupuestoRepositorio.obtenerPresupuestos();
        return View(presupuestos);
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