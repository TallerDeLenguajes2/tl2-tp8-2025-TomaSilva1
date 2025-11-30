using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_TomaSilva1.Models;

namespace tl2_tp8_2025_TomaSilva1.Controllers;

public class ProductosController : Controller
{
    private ProductoRepositorio _productosRepositorio;
    public ProductosController()
    {
        _productosRepositorio = new ProductoRepositorio();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = _productosRepositorio.GetAll();
        return View(productos);
    }
}