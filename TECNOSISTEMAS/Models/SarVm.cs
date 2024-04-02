namespace TECNOSISTEMAS.Models
{
    public class SarVm
    {
        public double CAI { set; get; }
        public double RangoInicial { set; get; }
        public double RangoFinal { set; get; }
        public double LimitEmision { set; get; }

        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }

        public bool Validacion()
        {
            return true;

        }
    }
}
