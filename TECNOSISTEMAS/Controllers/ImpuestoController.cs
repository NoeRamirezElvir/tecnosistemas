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
    public class ImpuestoController : Controller
    {
        private readonly ILogger<ImpuestoController> _logger;
        private TecnosistemasDbContext _context;

        public ImpuestoController(ILogger<ImpuestoController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Principal
        [ClaimRequirement("Impuesto")] //Filtros
        public IActionResult Index()
        {
            var impuesto = _context.Impuesto.Where(w => w.Eliminado == false).ProjectToType<ImpuestoVm>().ToList();
            return View(impuesto);
        }
        //Insertar
        [HttpGet]
        [ClaimRequirement("Impuesto")] //Filtros
        public IActionResult Insertar()
        {
            ImpuestoVm impuesto = new ImpuestoVm();
            return View(impuesto); 
        }
        [HttpPost]
        [ClaimRequirement("Impuesto")] //Filtros
        public IActionResult Insertar(ImpuestoVm impuesto)
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //
            if (!impuesto.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(impuesto);
            }
            Impuesto nuevoRegistro = new Impuesto();
            nuevoRegistro.Nombre = impuesto.Nombre;
            nuevoRegistro.Valor = impuesto.Valor;

            nuevoRegistro.Eliminado = false;
            nuevoRegistro.CreatedDate = DateTime.Now;
            nuevoRegistro.CreatedBy = UsuarioObjeto.Id;
            nuevoRegistro.Id = Guid.NewGuid();
            _context.Impuesto.Add(nuevoRegistro);
            _context.SaveChanges();
            TempData["mensaje"] = "Registro Exitoso";
            return RedirectToAction("Index");
        }
        //Editar
        [HttpGet]
        [ClaimRequirement("Impuesto")] //Filtros
        public IActionResult Editar(Guid ImpuestoId)
        {
            var impuesto = _context.Impuesto.Where(w => w.Id == ImpuestoId).ProjectToType<ImpuestoVm>().FirstOrDefault();
            return View(impuesto);
        }
        [HttpPost]
        [ClaimRequirement("Impuesto")] //Filtros
        public IActionResult Editar(ImpuestoVm impuesto)
        {
            if (!impuesto.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(impuesto);
            }
            var nuevoRegistro = _context.Impuesto.Where(w => w.Id == impuesto.Id).FirstOrDefault();
            nuevoRegistro.Nombre = impuesto.Nombre;
            nuevoRegistro.Valor = impuesto.Valor;
            _context.SaveChanges();

            TempData["mensaje"] = "Actualizacion Exitosa";
            return RedirectToAction("Index");
        }
        //Eliminar
        [HttpGet]
        [ClaimRequirement("Impuesto")] //Filtros
        public IActionResult Eliminar(Guid ImpuestoId)
        {
            var impuesto = _context.Impuesto.Where(w => w.Id == ImpuestoId).ProjectToType<ImpuestoVm>().FirstOrDefault();
            return View(impuesto);
        }
        [HttpPost]
        [ClaimRequirement("Impuesto")] //Filtros
        public IActionResult Eliminar(ImpuestoVm impuesto)
        {
            var nuevoRegistro = _context.Impuesto.Where(w => w.Id == impuesto.Id).FirstOrDefault();
            nuevoRegistro.Eliminado = true;

            _context.SaveChanges();

            TempData["mensaje"] = "Registro Eliminado";
            return RedirectToAction("Index");
        }
        //Buscar Nombre
        [HttpGet]
        [ClaimRequirement("Impuesto")] //Filtros
        public IActionResult BuscarNombre()
        {
            return View(new ImpuestoVm());
        }
        [HttpPost]
        [ClaimRequirement("Impuesto")] //Filtros
        public JsonResult BuscarImpuestoNombre([FromForm] ImpuestoVm vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Nombre))
            {
                var errorMessage = "Escriba el nombre a buscar";
                var errorResult = AppResult.Error(errorMessage);
                return new JsonResult(BadRequest(errorResult));
            }
            var impuesto = _context.Impuesto.Where(w => w.Eliminado == false && w.Nombre.ToLower() == vm.Nombre.ToLower()).ProjectToType<ImpuestoVm>().ToList();
            var result = impuesto.Count == 0 ? AppResult.Error("No se encontro el impuesto deseado") : AppResult.Success("Impuesto encontrada", impuesto);
            return new JsonResult(Ok(result));
        }
    }
}
