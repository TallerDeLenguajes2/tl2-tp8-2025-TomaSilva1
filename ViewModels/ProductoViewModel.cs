using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Web.ViewModels
{
    public class ProductoViewModel
    {
        //Se incluye id para la accion de edicion.
        public int IdProducto {get; set;}

        //Validacion: Maximo 250 caracteres. Es opcional por defecto si no tiene [required]
        [Display(Name = "Descripción del Producto")]
        [StringLength(250, ErrorMessage = "La descripcion no puede superar los 250 caracteres.")]
        public string Descripcion {get; set;}

        //Validación: Requerido y debe ser positivo.
        [Display(Name = "Precio Unitario")]
        [Required(ErrorMessage ="El precio es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage ="El precio debe ser un valor positivo")]
        public int Precio {get; set;}
    }
}