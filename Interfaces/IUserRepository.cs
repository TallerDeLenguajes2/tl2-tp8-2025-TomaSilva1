namespace tl2_tp8_2025_TomaSilva1.Interfaces;

using tl2_tp8_2025_TomaSilva1.Models;

public interface IUserRepositorio
{
    Usuario GetUser(string username, string password);
}