namespace TECNOSISTEMAS.Models
{
    public class ImpuestoVm
    {
        public string Nombre { set; get; }
        public double Valor { set; get; }

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
