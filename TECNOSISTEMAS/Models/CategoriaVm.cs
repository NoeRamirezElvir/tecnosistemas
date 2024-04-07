using System.Text.RegularExpressions;
using TECNOSISTEMAS.Data.Entidades;

namespace TECNOSISTEMAS.Models
{
    public class CategoriaVm
    {
        public string Nombre { set; get; }
        public string Descripcion { set; get; }

        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }


        //Validaciones
        public string Validacion(bool existente)
        {
            //Nombre
            if (string.IsNullOrEmpty(Nombre))
            {
                return "El Nombre está vacío";
            }
            if (Nombre.Length < 2)
            {
                return "El Nombre es muy corto";
            }
            if (Nombre.Length > 20)
            {
                return "El Nombre es muy largo";
            }
            if (existente)
            {
                return "El Nombre ya está registrado";
            }
            //Descripcion
            if (string.IsNullOrEmpty(Descripcion))
            {
                return "La Descripción está vacía";
            }
            if (Descripcion.Length < 2)
            {
                return "La Descripción es muy corta";
            }
            if (Descripcion.Length > 25)
            {
                return "La Descripción es muy larga";
            }
            return string.Empty; //Si no hay errores manda la cadena vacia
        }

    }
}
