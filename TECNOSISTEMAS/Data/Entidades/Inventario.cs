namespace TECNOSISTEMAS.Data.Entidades
{
    public class Inventario
    {   //Atributos 
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        //Relacion Producto
        public Guid IdProducto { set; get; }
        public Producto Producto { set; get; }


        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
    }
}
