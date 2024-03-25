namespace TECNOSISTEMAS.Data.Entidades
{
    public class AgrupadoModulos
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
        public ICollection<Modulo> Modulos { set; get; }
        public AgrupadoModulos()
        {
            Modulos = new List<Modulo>();
        }
    }
}
