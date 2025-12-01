using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_TomaSilva1.Models;
using SistemaVentas.Web.ViewModels;

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
    

    public IActionResult Details(int id)
    {
        Productos p = _productosRepositorio.obtenerProductoPorId(id);
        return View(p);        
    }

    public IActionResult Delete(int id)
    {
        _productosRepositorio.borrarProducto(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet] //Devuelve la vista del formulario de creacion.
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost] //Recibe los datos del formulario de creacion, los valida y guarda.
    public IActionResult Alta(ProductoViewModel p)
    {
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