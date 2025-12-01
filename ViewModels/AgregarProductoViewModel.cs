//Debe incluir ListaProductos de tipo SelectList
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList

namespace SistemaVentas.Web.ViewModels
{
    public class AgregarProductoViewModel
    {
        //id del presupuesto al que se va a agregar (viene de la URL o campo oculto)
        public int IdPresupuesto {get; set;}

        //Id del producto seleccionado en el dropdown
        //[Display(Name ="Producto a agregar")]
        public int IdProducto {get; set;}

        //Validacion requerido y debe ser positivo
        [Display(Name ="Cantidad")]
        [Required(ErrorMessage ="La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage ="La cantidad debe ser mayoa a 0")]
        public int Cantidad {get; set;}

        //Propiedad adicional para el Dropdown (No se valida, solo se usa en la vista)
        [ValidateNever]
        public SelectList ListaProductos {get; set;}
    }
}