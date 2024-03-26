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
        public bool Validacion()
        {
            return true;

        }
    }
}
