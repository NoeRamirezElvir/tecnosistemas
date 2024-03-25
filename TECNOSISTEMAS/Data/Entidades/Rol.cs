namespace TECNOSISTEMAS.Data.Entidades
{
    public class Rol
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
        public ICollection<Usuario> Usuarios { get; set; }
        public ICollection<ModulosRoles> ModulosRoles { get; set; }

        public Rol()
        {
            Usuarios = new HashSet<Usuario>();
            ModulosRoles = new HashSet<ModulosRoles>();
        }
    }
}
