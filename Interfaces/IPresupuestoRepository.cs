namespace tl2_tp8_2025_TomaSilva1.Interfaces;

using tl2_tp8_2025_TomaSilva1.Models;

public interface IPresupuestoRepositorio{
    int crearPresupuesto(Presupuestos P);
    List<Presupuestos> obtenerPresupuestos();
    Presupuestos obtenerPresupuestoPorId(int id);
    void crearYAgregarProductoAPresupuesto(Productos p, int cantidad, int idPresupuesto);
    int agregarProductoAPresupuesto(int idProd, int idPres, int cant);
    int eliminarPresupuesto(int idPres);
}