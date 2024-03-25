namespace TECNOSISTEMAS.Data.Entidades
{
    public class Categoria
    {
        //Atributos 
        public string Nombre { set; get; }
        public string Descripcion { set; get; }
        //Relacion Producto
        public ICollection<Producto> Producto { set; get; }

        //Constructor
        public Categoria()
        {
            Producto = new HashSet<Producto>();
        }

        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
    }
}
