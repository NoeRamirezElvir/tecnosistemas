using Microsoft.AspNetCore.Mvc.Rendering;
using TECNOSISTEMAS.Data.Entidades;

namespace TECNOSISTEMAS.Models
{
    public class ProductoVm
    {
        public string Nombre { set; get; }
        public double Precio { set; get; }
        public double Costo { set; get; }
        public bool Activo { set; get; }
        //Relacion Categoria
        public Guid IdCategoria { set; get; }
        public Categoria Categoria { set; get; }
        public List<SelectListItem> CategoriaLista { get; set; }


        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }

        //Validaciones
        public string Validacion(bool existente)
        {
            //IdProducto
            if (IdCategoria == Guid.Empty)
            {
                return "El Id de la categoría es incorrecto";
            }
            //Nombre
            if (string.IsNullOrEmpty(Nombre))
            {
                return "El Nombre está vacío";
            }
            if (Nombre.Length < 2)
            {
                return "El Nombre es muy corto";
            }
            if (Nombre.Length > 25)
            {
                return "El Nombre es muy largo";
            }
            if (existente)
            {
                return "El Nombre ya está registrado";
            }
            // Precio
            if (Precio < 0)
            {
                return "El Precio no puede ser menor o igual a cero";
            }
            if (Precio < Costo)
            {
                return "El Precio adebe ser mayor al costo de compra";
            }
            // Precio
            if (Costo < 0)
            {
                return "El Costo no puede ser menor o igual a cero";
            }
            if (Costo > Precio)
            {
                return "El Costo adebe ser menor al precio de venta";
            }


            return string.Empty; //Si no hay errores manda la cadena vacia
        }

    }
}
