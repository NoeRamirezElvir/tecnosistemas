namespace TECNOSISTEMAS.Models
{
    public class AgrupadoModulosVm
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public List<ModuloVm> Modulos { get; set; }
    }
}
