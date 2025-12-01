using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_TomaSilva1.Models;
using SistemaVentas.Web.ViewModels; //Necesario para poder llegar a los ViewModels
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList

namespace tl2_tp8_2025_TomaSilva1.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestoRepositorio _presupuestoRepositorio;
    public PresupuestosController()
    {
        _presupuestoRepositorio = new PresupuestoRepositorio();
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

    [HttpGet]
    public IActionResult Details(int id)
    {
        var p = _presupuestoRepositorio.obtenerPresupuestoPorId(id);
        return View(p);
    }

    [HttpGet]
    public IActionResult CreateDetalle(int id)
    {
        var P = _presupuestoRepositorio.obtenerPresupuestoPorId(id);
        return View(P);
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

    [HttpPost]
    public IActionResult AltaProd(int idPres, int idProd, int cant)
    {
        _presupuestoRepositorio.agregarProductoAPresupuesto(idProd, idPres, cant);
        return RedirectToAction("Index");
    }
}