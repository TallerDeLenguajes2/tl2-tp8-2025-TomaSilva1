using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_TomaSilva1.Models;

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
    public IActionResult Details()
    {
        List<Presupuestos> presupuestos = _presupuestoRepositorio.obtenerPresupuestos();
        return View(presupuestos);
    }
}