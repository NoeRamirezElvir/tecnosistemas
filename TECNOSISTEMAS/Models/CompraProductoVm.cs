using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using TECNOSISTEMAS.Data.Entidades;

namespace TECNOSISTEMAS.Models
{
    public class CompraProductoVm
    {
        public int Cantidad { set; get; }
        public double PrecioVenta { set; get; }
        public double CostoCompra { set; get; }
        public double Subtotal { set; get; }

        //Relacion Compra
        public Guid IdCompra { set; get; }
        public Compra Compra { set; get; }
        public double Total { set; get; }

        //Relacion Proveedor
        public Guid IdProveedor { set; get; }
        public Proveedor Proveedor { set; get; }
        public List<SelectListItem> ProveedorLista { get; set; }

        //Relacion Producto
        public Guid IdProducto { set; get; }
        public Producto Producto { set; get; }
        public List<SelectListItem> ProductoLista { get; set; }

        //lista productos seleccionados
        public string IdProductoLista { get; set; }



        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
        public string usuario { set; get; }

        //Validaciones
        public string Validacion()
        {
            //Cantidad
            if (double.IsNaN(Cantidad))
            {
                return "La Cantidad está vacía";
            }
            if ((Cantidad.ToString()).Length < 1)
            {
                return "La Cantidad es muy corta";
            }
            //IdProductoLista
            if (string.IsNullOrEmpty(IdProductoLista))
            {
                return "La lista de productos está vacía";
            }
            if (IdProductoLista == "[]")
            {
                return "La lista de productos está vacía";
            }
            return string.Empty; //Si no hay errores manda la cadena vacia
        }
        public string ValidacionInventario(int stockActualMC, int stockMaximo)
        {
            //stockMaximo
            if (stockMaximo == -100)
            {
                return "El Producto no esta registrado en Inventario: ";
            }
            if (stockActualMC > stockMaximo)
            {
                return "El stock actual superara el stock maximo: ";
            }

            return string.Empty; //Si no hay errores manda la cadena vacia
        }


    }
}
