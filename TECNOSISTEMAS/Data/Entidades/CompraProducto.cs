namespace TECNOSISTEMAS.Data.Entidades
{
    public class CompraProducto
    {
        //Atributos
        public int Cantidad {  get; set; }
        public double CostoCompra {  get; set; }
        public double PrecioVenta { get; set; }
        public double Subtotal { get; set; }

        //Relacion Producto
        public Guid IdProducto { set; get; }
        public Producto Producto { set; get; }

        //Relacion Proveedor
        public Guid IdProveedor { set; get; }
        public Proveedor Proveedor { set; get; }

        //Relacion Compra
        public Guid IdCompra { set; get; }
        public Compra Compra { set; get; }


        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
    }
}
