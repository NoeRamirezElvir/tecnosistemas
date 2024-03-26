using Microsoft.AspNetCore.Mvc.Rendering;
using TECNOSISTEMAS.Data.Entidades;

namespace TECNOSISTEMAS.Models
{
    public class ProductoVm
    {
        public string Nombre { set; get; }
        public double Precio { set; get; }
        public double Costo { set; get; }
        public bool Activo { set; get; }
        //Relacion Categoria
        public Guid IdCategoria { set; get; }
        public Categoria Categoria { set; get; }
        public List<SelectListItem> CategoriaLista { get; set; }


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
