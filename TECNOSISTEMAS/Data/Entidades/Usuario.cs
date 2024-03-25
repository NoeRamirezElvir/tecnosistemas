namespace TECNOSISTEMAS.Data.Entidades
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string apellido { get; set; }

        public string direccion { get; set; }

        public string DNI { get; set; }

        public DateTime fechaNacimiento { get; set; }

        public string telefono { get; set; }

        public string usuario { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Eliminado { get; set; }
        public Rol Rol { get; set; }
        public Guid RolId { get; set; }
    }
}
