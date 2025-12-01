using tl2_tp8_2025_TomaSilva1.Interfaces;
using Microsoft.Data.Sqlite;
namespace tl2_tp8_2025_TomaSilva1.Repositorios;


public class UsuarioRepositorio : IUserRepositorio
{
    private string _connexionDb = "Data Source=DB/nueva.db";
    //Logica para conectar a la db y buscar por Usuario y Contraseña
    public Usuario GetUser(string usuario, string contrasena)
    {
        Usuario user = null;

        //Consulta SQL que busca por Usuario Y Contrasena
        const string sql = @"
        SELECT Id, Nombre, User, Pass, Rol
        FROM Usuarios
        WHERE User = @Usuario AND Pass = @Contrasena";
        using var conexion = new SqliteConnection(_connexionDb);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.AddWithValue("@Usuario", usuario);
        comando.Parameters.AddWithValue("@Contrasena", contrasena);

        using var reader = comando.ExecuteReader();

        if (reader.Read())
        {
        // Si el lector encuentra una fila, el usuario existe y las credenciales son correctas
            user = new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }
        return user;
    }
}