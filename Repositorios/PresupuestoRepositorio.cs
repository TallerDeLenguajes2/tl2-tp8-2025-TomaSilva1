using System.Data.SqlTypes;
using System.Formats.Asn1;
using Microsoft.Data.Sqlite;
using SQLitePCL;

public class PresupuestoRepositorio
{
    private string _connexionDb = "Data Source=DB/nueva.db";
    //Crear presupuesto
    public int crearPresupuesto(Presupuestos P)
    {
        int nuevoID;

        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        string sql = "INSERT INTO presupuestos (nombreDestinatario, fechaCreacion) VALUES (@nombre, @fecha); SELECT last_insert_rowid();";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@nombre", P.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@fecha", P.FechaCreacion));

        nuevoID = Convert.ToInt32(comando.ExecuteScalar());

        return nuevoID;
    }
    //Obtener presupuesto
    public List<Presupuestos> obtenerPresupuestos()
    {
        string sql = "SELECT * FROM presupuestos";
        List<Presupuestos> presupuestos = [];

        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);

        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            var p = new Presupuestos
            {
                IdPresupuesto = Convert.ToInt32(lector["idPresupuesto"]),
                NombreDestinatario = lector["nombreDestinatario"].ToString(),
                FechaCreacion = lector["fechaCreacion"].ToString(),
                Detalle = new List<PresupuestoDetalle>()
            };

            string sql2 = "SELECT idProducto, descripcion, precio, cantidad FROM presupuestoDetalle pr INNER JOIN productos p ON p.id_prod = pr.idProducto WHERE idPresupuesto = @id";

            using var comando2 = new SqliteCommand(sql2, conexion);
            comando2.Parameters.Add(new SqliteParameter("@id", p.IdPresupuesto));
            using var lector2 = comando2.ExecuteReader();

            while (lector2.Read())
            {
                var pp = new Productos()
                {
                    IdProducto = Convert.ToInt32(lector2["idProducto"]),
                    Descripcion = lector2["descripcion"].ToString(),
                    Precio = Convert.ToInt32(lector2["precio"])
                };
                var pd = new PresupuestoDetalle()
                {
                    Producto = pp,
                    Cantidad = Convert.ToInt32(lector2["cantidad"])
                };
                p.Detalle.Add(pd);
            }

            presupuestos.Add(p);
        }

        return presupuestos;
    }
    //Obtener presupuesto por id
    public Presupuestos obtenerPresupuestoPorId(int id)
    {
        string sql = "SELECT * FROM presupuestos WHERE idPresupuesto = @id";

        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@id", id));

        using var lector = comando.ExecuteReader();

        if (!lector.Read())
        {
            return null;
        }

        var presupuesto = new Presupuestos
        {
            IdPresupuesto = Convert.ToInt32(lector["idPresupuesto"]),
            NombreDestinatario = lector["nombreDestinatario"].ToString(),
            FechaCreacion = lector["fechaCreacion"].ToString(),
            Detalle = new List<PresupuestoDetalle>()
        };

        string sql2 = "SELECT idProducto, descripcion, precio, cantidad FROM presupuestoDetalle INNER JOIN productos p ON p.id_prod = id_prod WHERE idPresupuesto = @id";

        using var comando2 = new SqliteCommand(sql2, conexion);
        comando2.Parameters.Add(new SqliteParameter("@id", presupuesto.IdPresupuesto));
        using var lector2 = comando2.ExecuteReader();

        while (lector2.Read())
        {
            var pp = new Productos()
            {
                IdProducto = Convert.ToInt32(lector2["idProducto"]),
                Descripcion = lector2["descripcion"].ToString(),
                Precio = Convert.ToInt32(lector2["precio"])
            };
            var pd = new PresupuestoDetalle()
            {
                Producto = pp,
                Cantidad = Convert.ToInt32(lector2["cantidad"])
            };
            presupuesto.Detalle.Add(pd);
        }

        return presupuesto;
    }

    //Agregar un producto y una cantidad a un presupuesto;
    public void crearYAgregarProductoAPresupuesto(Productos p, int cantidad, int idPresupuesto)
    {
        string sql = "INSERT INTO productos (descripcion, precio) VALUES (@1, @2); SELECT last_insert_rowid();";
        string sql2 = "INSERT INTO presupuestoDetalle (idPresupuesto, idProducto, cantidad) VALUES (@3, @4, @5)";

        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@1", p.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@2", p.Precio));

        int nuevoId = Convert.ToInt32(comando.ExecuteScalar());

        using var comando2 = new SqliteCommand(sql2, conexion);
        comando2.Parameters.Add(new SqliteParameter("@3", idPresupuesto));
        comando2.Parameters.Add(new SqliteParameter("@4", nuevoId));
        comando2.Parameters.Add(new SqliteParameter("@5", cantidad));
        var lector = comando2.ExecuteReader();
    }

    public int agregarProductoAPresupuesto(int idProd, int idPres, int cant)
    {
        string sql = "INSERT INTO presupuestoDetalle VALUES (@1, @2, @3)";

        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@1", idPres));
        comando.Parameters.Add(new SqliteParameter("@2", idProd));
        comando.Parameters.Add(new SqliteParameter("@3", cant));

        var lector = comando.ExecuteNonQuery();

        if(lector == 0)
        {
            return -1;
        }

        return 1;
    }

    public int eliminarPresupuesto(int idPres)
    {
        string sql1 = "DELETE FROM presupuestoDetalle WHERE idPresupuesto = @1";
        string sql2 = "DELETE FROM presupuestos WHERE idPresupuesto = @1";
        string sql3 = "BEGIN";
        string sql4 = "COMMIT";
        string sql5 = "ROLLBACK";

        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        using var begin = new SqliteCommand(sql3, conexion);
        begin.ExecuteNonQuery();

        using var comando = new SqliteCommand(sql1, conexion);
        comando.Parameters.Add(new SqliteParameter("@1", idPres));
        int first = Convert.ToInt32(comando.ExecuteNonQuery());

        if(first > 0)
        {
            using var comando2 = new SqliteCommand(sql2, conexion);
            comando2.Parameters.Add(new SqliteParameter("@1", idPres));
            int second = Convert.ToInt32(comando2.ExecuteNonQuery());

            if (second > 0)
            {
                using var commit = new SqliteCommand(sql4, conexion);
                commit.ExecuteNonQuery();
                return second + first;
            }
            else
            {
                using var rollback = new SqliteCommand(sql5, conexion);
                rollback.ExecuteNonQuery();
                return -2;
            }
        }
        else
        {
            using var rollback = new SqliteCommand(sql5, conexion);
            rollback.ExecuteNonQuery();
            return -1;
        }
    }
}