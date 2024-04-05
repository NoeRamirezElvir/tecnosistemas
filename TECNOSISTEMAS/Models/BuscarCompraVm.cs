namespace TECNOSISTEMAS.Models
{
    public class BuscarCompraVm
    {
        public List<CompraVm>? Compras { get; set; }
        public CompraVm? NuevaCompra { get; set; }
        public string? IdProductoLista { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string? usuario { set; get; }

        public string Validacion(){
            if (FechaInicial == DateTime.MinValue || FechaInicial == DateTime.Parse("01/01/0001"))
            {
                return "Debe seleccionar una fecha inicial";
            }
            if (FechaFinal == DateTime.MinValue || FechaFinal == DateTime.Parse("01/01/0001"))
            {
                return "Debe seleccionar una fecha final";
            }
            if (FechaInicial > FechaFinal){
                return "La fecha inicial no puede ser mayor que la fecha final";
            }
            if (FechaFinal > DateTime.Now){
                return "La fecha final no puede ser mayor que la fecha actual";
            }
            if (FechaFinal < FechaInicial){
                return "La fecha final no puede ser menor que la fecha inicial";
            }

            return string.Empty; // Si no hay errores, devuelve una cadena vacía
        }
    }
}
