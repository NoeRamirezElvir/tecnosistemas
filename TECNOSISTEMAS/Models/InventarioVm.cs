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
        public bool Validacion()
        {
            return true;

        }
    }
}
