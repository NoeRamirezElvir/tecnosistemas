using Microsoft.AspNetCore.Mvc.Rendering;
using TECNOSISTEMAS.Data.Entidades;

namespace TECNOSISTEMAS.Models
{
    public class UsuarioVm
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

        public Guid RolId { get; set; }
        public RolVm Rol { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public List<AgrupadoModulosVm> Menu { get; set; }


        public bool Validaciones_usuario()
        {


            if (string.IsNullOrEmpty(Nombre))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Email))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }
            if (string.IsNullOrEmpty(apellido))
            {
                return false;
            }
            if (string.IsNullOrEmpty(direccion))
            {
                return false;
            }

            if (string.IsNullOrEmpty(DNI))
            {
                return false;
            }
            if (string.IsNullOrEmpty(telefono))
            {
                return false;
            }
            if (RolId == Guid.Empty)
            {
                return false;
            }

            return true;
        }
    }
}
