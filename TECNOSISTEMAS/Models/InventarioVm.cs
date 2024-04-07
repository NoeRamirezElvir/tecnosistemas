using Microsoft.AspNetCore.Mvc.Rendering;
using TECNOSISTEMAS.Data.Entidades;

namespace TECNOSISTEMAS.Models
{
    public class InventarioVm
    {
        //Atributos 
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        //Relacion Producto
        public Guid IdProducto { set; get; }
        public Producto Producto { set; get; }
        public List<SelectListItem> ProductoLista { get; set; }


        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }

        //Validaciones
        //Validaciones
        public string Validacion(bool existente)
        {
            //IdProducto
            if (IdProducto == Guid.Empty)
            {
                return "El Id del producto es incorrecto";
            }
            if (existente)
            {
                return "Ya existe un registro de inventario para este producto";
            }
            // StockActual
            if (StockActual < 0)
            {
                return "El stock actual no puede ser menor a cero";
            }
            if (StockActual < StockMinimo)
            {
                return "El stock actual es menor al stock mínimo";
            }
            if (StockActual > StockMaximo)
            {
                return "El stock actual es mayor al stock máximo";
            }
            // StockMinimo
            if (StockMinimo < 0)
            {
                return "El stock mínimo no puede ser menor a cero";
            }
            if (StockMinimo > StockMaximo)
            {
                return "El stock mínimo no puede ser mayor al stock máximo";
            }
            // StockMaximo
            if (StockMaximo == 0)
            {
                return "El stock máximo no puede ser igual o menor a cero";
            }
            if (StockMaximo < StockMinimo)
            {
                return "El stock máxomo no puede ser menor al stock mínimo";
            }
            return string.Empty; //Si no hay errores manda la cadena vacia
        }
    }
}
