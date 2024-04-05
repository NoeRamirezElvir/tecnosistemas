namespace TECNOSISTEMAS.Data.Entidades
{
    public class Compra
    {
        //Atributos
        public double Total { set; get; }

        //Relacion CompraProducto
        public ICollection<CompraProducto> CompraProducto { set; get; }

        //Constructor
        public Compra()
        {
            CompraProducto = new HashSet<CompraProducto>();
        }

        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
    }
}
