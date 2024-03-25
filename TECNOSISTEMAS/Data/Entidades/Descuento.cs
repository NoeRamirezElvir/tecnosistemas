namespace TECNOSISTEMAS.Data.Entidades
{
    public class Descuento
    {
        //Atributos
        public string Nombre { set; get; }
        public double Valor { set; get; }
        
        //Relaciones


        //Obligatorios
        public Guid Id { get; set; }
        public bool Eliminado { set; get; }
        public DateTime CreatedDate { set; get; }
        public Guid CreatedBy { set; get; }
    }
}
