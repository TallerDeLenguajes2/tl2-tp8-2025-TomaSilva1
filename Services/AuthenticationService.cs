using tl2_tp8_2025_TomaSilva1.Interfaces;
namespace tl2_tp8_2025_TomaSilva1.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepositorio _userRepositorio;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService (IUserRepositorio userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepositorio = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    
    public bool Login(string username, string password)
    {
        var context = _httpContextAccessor.HttpContext;
        var user = _userRepositorio.GetUser(username, password);

        if (user != null)
        {

            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible.");
            }
            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("User", user.User);
            context.Session.SetString("UserNombre", user.Nombre);
            context.Session.SetString("Rol", user.Rol);
            //es el tipo de acceso/rol admin o cliente
            return true;
        }
        return false;
    }

    public void Logout()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext no está disponible.");

        }
        context.Session.Clear();

    }

    public bool IsAuthenticated()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext noestá disponible.");
        }


        return context.Session.GetString("IsAuthenticated") == "true";

    }
    public bool HasAccessLevel(string requiredAccessLevel)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext noestá disponible.");
        }
        return context.Session.GetString("Rol") == requiredAccessLevel;


    }

}