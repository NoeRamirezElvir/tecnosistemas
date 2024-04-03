using System.Text.RegularExpressions;

namespace TECNOSISTEMAS.Models
{
    public class ClienteVm
    {//Atributos
        public string Nombre { set; get; }
        public string Apellido { set; get; }
        public double Telefono { set; get; }
        public string Direccion { set; get; }
        public string RTN { set; get; }

        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }


        //Validaciones
        public string Validacion()
        {
            //Nombre
            if (string.IsNullOrEmpty(Nombre))
            {
                return "El Nombre está vacío";
            }
            if (Nombre.Length < 3)
            {
                return "El Nombre es muy corto";
            }
            if (Nombre.Length > 25)
            {
                return "El Nombre es muy largo";
            }
            //Apellido
            if (string.IsNullOrEmpty(Apellido))
            {
                return "El Apellido está vacío";
            }
            if (Apellido.Length < 3)
            {
                return "El Apellido es muy corto";
            }
            if (Apellido.Length > 25)
            {
                return "El Apellido es muy largo";
            }
            //Telefono
            if (double.IsNaN(Telefono))
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
            if (Direccion.Length > 25)
            {
                return "La Dirección es muy larga";
            }
            //RTN
            if (string.IsNullOrEmpty(RTN))
            {
                return "El RTN está vacío";
            }
            if (RTN.Length < 14)
            {
                return "El RTN es muy corto";
            }
            if (RTN.Length > 14)
            {
                return "El RTN es muy largo";
            }

            return string.Empty; //Si no hay errores manda la cadena vacia
        }
    }
}
