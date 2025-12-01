using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace tl2_tp8_2025_TomaSilva1.ViewModels;

public class LoginViewModel
{
    
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public string Username{get; set;}
    
    [Required(ErrorMessage = "La contraseña es obligatoria.")]    
    public string  Password{get;set;}

    [ValidateNever]
    public string ErrorMessage{get;set;}
    
}