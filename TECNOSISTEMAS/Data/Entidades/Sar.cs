namespace TECNOSISTEMAS.Data.Entidades
{
    public class Sar
    {
        //Atributos
        public double CAI { set; get; }
        public double RangoInicial { set; get; }
        public double RangoFinal { set; get; }
        public double LimitEmision { set; get; }
 
        //Relaciones


        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
    }
}
