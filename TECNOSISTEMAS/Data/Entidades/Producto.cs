namespace TECNOSISTEMAS.Data.Entidades
{
    public class Producto
    {
        //Atributos 
        public string Nombre { set; get; }
        public double Precio { set; get; }
        public double Costo { set; get; }
        public bool Activo { set; get; }
        //Relacion Categoria
        public Guid IdCategoria { set; get; }
        public Categoria Categoria { set; get; }
        //Relacion Inventario
        public ICollection<Inventario> Inventario { set; get; }

        //Constructor
        public Producto()
        {
            Inventario = new HashSet<Inventario>();
        }

        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
    }
}
