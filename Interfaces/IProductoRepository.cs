namespace tl2_tp8_2025_TomaSilva1.Interfaces;

using tl2_tp8_2025_TomaSilva1.Models;

public interface IProductoRepositorio
{
    List<Productos> GetAll();
    int InsertarProducto(Productos Produc);
    int editarProducto(int idProduc, Productos produc);
    void borrarProducto(int id);
    Productos obtenerProductoPorId(int id);
}