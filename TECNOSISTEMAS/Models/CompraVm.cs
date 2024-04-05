namespace TECNOSISTEMAS.Models
{
    public class CompraVm
    {
        //Atributos
        public double Total { set; get; }


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
