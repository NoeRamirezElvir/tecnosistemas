namespace TECNOSISTEMAS.Data.Entidades
{
    public class Cliente
    {
        //Atributos
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
    }
}
