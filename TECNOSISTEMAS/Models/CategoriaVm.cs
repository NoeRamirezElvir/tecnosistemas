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
        public bool Validacion()
        {
            return true;

        }

    }
}
