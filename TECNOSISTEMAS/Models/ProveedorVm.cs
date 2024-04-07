using System.Text.RegularExpressions;

namespace TECNOSISTEMAS.Models
{
    public class ProveedorVm
    {
        //Atributos 
        public string Nombre { set; get; }
        public string Direccion { set; get; }
        public string Telefono { set; get; }
        public string CorreoElectronico { set; get; }

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
            //Telefono
            if (string.IsNullOrEmpty(Telefono))
            {
                return "El número de Teléfono está vacío";
            }
            if ((Telefono.ToString()).Length < 8)
            {
                return "El número de Teléfono es muy corto";
            }
            if ((Telefono.ToString()).Length < 8)
            {
                return "El número de Teléfono es muy largo";
            }
            if (!Regex.IsMatch(Telefono.ToString(), "^(2|9|8|3)"))
            {
                return "El número de Teléfono debe comenzar con 2, 9, 8 o 3";
            }
            //Direccion
            if (string.IsNullOrEmpty(Direccion))
            {
                return "La Dirección está vacía";
            }
            if (Direccion.Length < 3)
            {
                return "La Dirección es muy corta";
            }
            if (Direccion.Length > 30)
            {
                return "La Dirección es muy larga";
            }
            //CorreoElectronico
            if (string.IsNullOrEmpty(CorreoElectronico))
            {
                return "El correo electrónico está vacío";
            }
            if (CorreoElectronico.Length < 5)
            {
                return "El correo electrónico muy corto";
            }
            string patron = @"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,3}(\.[a-zA-Z]{2})?$";
            if (!Regex.IsMatch(CorreoElectronico, patron))
            {
                return "El formato del correo electrónico es inválido";
            }
            if (CorreoElectronico.Length > 30)
            {
                return "El correo electrónico es muy largo";
            }

            return string.Empty; //Si no hay errores manda la cadena vacia
        }
    }
}
