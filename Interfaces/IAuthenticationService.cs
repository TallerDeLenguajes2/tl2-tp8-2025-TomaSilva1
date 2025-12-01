namespace tl2_tp8_2025_TomaSilva1.Interfaces;

using tl2_tp8_2025_TomaSilva1.Models;

public interface IAuthenticationService
{
    bool Login(string username, string password);
    void Logout();
    bool IsAuthenticated();
    bool HasAccessLevel(string requiredAccessLevel); //Verificia si el usuario tiene el rol para cierta tarea.
}