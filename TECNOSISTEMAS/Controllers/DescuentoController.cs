using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TECNOSISTEMAS.Data;
using TECNOSISTEMAS.Data.Entidades;
using TECNOSISTEMAS.Filters;
using TECNOSISTEMAS.Models;
using TECNOSISTEMAS.Utilidades;

namespace TECNOSISTEMAS.Controllers
{
    public class DescuentoController : Controller
    {
        private readonly ILogger<DescuentoController> _logger;
        private TecnosistemasDbContext _context;

        public DescuentoController(ILogger<DescuentoController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Principal
        [ClaimRequirement("Descuento")] //Filtros
        public IActionResult Index()
        {
            var descuento = _context.Descuento.Where(w => w.Eliminado == false).ProjectToType<DescuentoVm>().ToList();
            return View(descuento);
        }
        //Insertar
        [HttpGet]
        [ClaimRequirement("Descuento")] //Filtros
        public IActionResult Insertar()
        {
            DescuentoVm descuento = new DescuentoVm();
            return View(descuento);
        }
        [HttpPost]
        [ClaimRequirement("Descuento")] //Filtros
        public IActionResult Insertar(DescuentoVm descuento)
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //
            if (!descuento.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(descuento);
            }
            Descuento nuevoRegistro = new Descuento();
            nuevoRegistro.Nombre = descuento.Nombre;
            nuevoRegistro.Valor = descuento.Valor;

            nuevoRegistro.Eliminado = false;
            nuevoRegistro.CreatedDate = DateTime.Now;
            nuevoRegistro.CreatedBy = UsuarioObjeto.Id;
            nuevoRegistro.Id = Guid.NewGuid();
            _context.Descuento.Add(nuevoRegistro);
            _context.SaveChanges();
            TempData["mensaje"] = "Registro Exitoso";
            return RedirectToAction("Index");
        }
        //Editar
        [HttpGet]
        [ClaimRequirement("Descuento")] //Filtros
        public IActionResult Editar(Guid DescuentoId)
        {
            var descuento = _context.Descuento.Where(w => w.Id == DescuentoId).ProjectToType<DescuentoVm>().FirstOrDefault();
            return View(descuento);
        }
        [HttpPost]
        [ClaimRequirement("Descuento")] //Filtros
        public IActionResult Editar(DescuentoVm descuento)
        {
            if (!descuento.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(descuento);
            }
            var nuevoRegistro = _context.Descuento.Where(w => w.Id == descuento.Id).FirstOrDefault();
            nuevoRegistro.Nombre = descuento.Nombre;
            nuevoRegistro.Valor = descuento.Valor;
            _context.SaveChanges();

            TempData["mensaje"] = "Actualizacion Exitosa";
            return RedirectToAction("Index");
        }
        //Eliminar
        [HttpGet]
        [ClaimRequirement("Descuento")] //Filtros
        public IActionResult Eliminar(Guid DescuentoId)
        {
            var descuento = _context.Descuento.Where(w => w.Id == DescuentoId).ProjectToType<DescuentoVm>().FirstOrDefault();
            return View(descuento);
        }
        [HttpPost]
        [ClaimRequirement("Descuento")] //Filtros
        public IActionResult Eliminar(DescuentoVm descuento)
        {
            var nuevoRegistro = _context.Descuento.Where(w => w.Id == descuento.Id).FirstOrDefault();
            nuevoRegistro.Eliminado = true;

            _context.SaveChanges();

            TempData["mensaje"] = "Registro Eliminado";
            return RedirectToAction("Index");
        }
        //Buscar Nombre
        [HttpGet]
        [ClaimRequirement("Descuento")] //Filtros
        public IActionResult BuscarNombre()
        {
            return View(new DescuentoVm());
        }
        [HttpPost]
        [ClaimRequirement("Descuento")] //Filtros
        public JsonResult BuscarDescuentoNombre([FromForm] DescuentoVm vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Nombre))
            {
                var errorMessage = "Escriba el nombre a buscar";
                var errorResult = AppResult.Error(errorMessage);
                return new JsonResult(BadRequest(errorResult));
            }
            var descuento = _context.Descuento.Where(w => w.Eliminado == false && w.Nombre.ToLower() == vm.Nombre.ToLower()).ProjectToType<DescuentoVm>().ToList();
            var result = descuento.Count == 0 ? AppResult.Error("No se encontro el descuento deseado") : AppResult.Success("Descuento encontrada", descuento);
            return new JsonResult(Ok(result));
        }
    }
}
