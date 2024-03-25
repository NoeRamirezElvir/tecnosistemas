namespace TECNOSISTEMAS.Data.Entidades
{
    public class Modulo
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Metodo { get; set; }
        public string Controller { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
        public ICollection<ModulosRoles> ModulosRoles { get; set; }
        public AgrupadoModulos AgrupadoModulos { get; set; }
        public Guid AgrupadoModulosId { get; set; }

        public Modulo()
        {
            ModulosRoles = new HashSet<ModulosRoles>();
        }
    }
}
