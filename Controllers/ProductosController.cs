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

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Alta(string Desc, int Pre)
    {
        var p = new Productos()
        {
            Descripcion = Desc,
            Precio = Pre
        };
        _productosRepositorio.InsertarProducto(p);
        return RedirectToAction("Index");
    }

    public IActionResult Details(int id)
    {
        Productos p = _productosRepositorio.obtenerProductoPorId(id);
        return View(p);        
    }

    public IActionResult Delete(int id)
    {
        _productosRepositorio.borrarProducto(id);
        return RedirectToAction("Index");
    }

    public IActionResult AuxiliarEdit(int id)
    {
        Productos p = _productosRepositorio.obtenerProductoPorId(id);
        return View(p);
    }

    public IActionResult Edit(int id, string Desc, int Pre)
    {
        var p = new Productos()
        {
            Descripcion = Desc,
            Precio = Pre
        };
        int real = _productosRepositorio.editarProducto(id, p);

        if(real > 0)
        {
            return RedirectToAction("Index");
        }
        else
        {
            return BadRequest();
        }
    }
}