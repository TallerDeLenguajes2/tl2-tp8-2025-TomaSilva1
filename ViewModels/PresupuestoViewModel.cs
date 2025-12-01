using System.ComponentModel.DataAnnotations;
using System;

namespace SistemaVenta.Web.ViewModels
{
    public class PresupuestoViewModel
    {
        public int IdPresupuesto {get; set;}

        //validacion requerido.
        [Display(Name ="Nombre o Email del destinatario")]
        [Required(ErrorMessage ="El nombre es obligatorio")]
        //opcional [EmailAddress(ErrorMessage="El formado del email no es valido.")]
        public string NombreDestinatario {get; set;}

        //Validacion requerido y de tipo de dato.
        [Display(Name = "Fecha de creación")]
        [Required(ErrorMessage ="La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime fechaCreacion {get; set;}
    }
}