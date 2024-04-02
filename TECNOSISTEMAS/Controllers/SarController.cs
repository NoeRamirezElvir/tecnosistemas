using Microsoft.AspNetCore.Mvc;
using TECNOSISTEMAS.Data.Entidades;
using TECNOSISTEMAS.Data;
using TECNOSISTEMAS.Filters;
using TECNOSISTEMAS.Models;
using TECNOSISTEMAS.Utilidades;
using Mapster;
using Newtonsoft.Json;
namespace TECNOSISTEMAS.Controllers
{
    public class SarController : Controller
    {
        private readonly ILogger<SarController> _logger;
        private TecnosistemasDbContext _context;

        public SarController(ILogger<SarController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Principal
        [ClaimRequirement("Sar")] //Filtros
        public IActionResult Index()
        {
            var sar = _context.Sar.Where(w => w.Eliminado == false).ProjectToType<SarVm>().ToList();
            return View(sar);
        }
        //Insertar
        [HttpGet]
        [ClaimRequirement("Sar")] //Filtros
        public IActionResult Insertar()
        {
            SarVm sar = new SarVm();
            return View(sar);
        }
        [HttpPost]
        [ClaimRequirement("Sar")] //Filtros
        public IActionResult Insertar(SarVm sar)
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //
            if (!sar.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(sar);
            }
            Sar nuevoRegistro = new Sar();
            nuevoRegistro.CAI = sar.CAI;
            nuevoRegistro.RangoInicial = sar.RangoInicial;
            nuevoRegistro.RangoFinal = sar.RangoFinal;
            nuevoRegistro.LimitEmision = sar.LimitEmision;

            nuevoRegistro.Eliminado = false;
            nuevoRegistro.CreatedDate = DateTime.Now;
            nuevoRegistro.CreatedBy = UsuarioObjeto.Id;
            nuevoRegistro.Id = Guid.NewGuid();

            _context.Sar.Add(nuevoRegistro);
            _context.SaveChanges();

            TempData["mensaje"] = "Registro Exitoso";
            return RedirectToAction("Index");
        }
        //Editar
        [HttpGet]
        [ClaimRequirement("Sar")] //Filtros
        public IActionResult Editar(Guid SarId)
        {
            var sar = _context.Sar.Where(w => w.Id == SarId).ProjectToType<SarVm>().FirstOrDefault();
            return View(sar);
        }
        [HttpPost]
        [ClaimRequirement("Sar")] //Filtros
        public IActionResult Editar(SarVm sar)
        {
            if (!sar.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(sar);
            }
            var nuevoRegistro = _context.Sar.Where(w => w.Id == sar.Id).FirstOrDefault();
            nuevoRegistro.CAI = sar.CAI;
            nuevoRegistro.RangoInicial = sar.RangoInicial;
            nuevoRegistro.RangoFinal = sar.RangoFinal;
            nuevoRegistro.LimitEmision = sar.LimitEmision;

            _context.SaveChanges();

            TempData["mensaje"] = "Actualizacion Exitosa";
            return RedirectToAction("Index");
        }
        //Eliminar
        [HttpGet]
        [ClaimRequirement("Sar")] //Filtros
        public IActionResult Eliminar(Guid SarId)
        {
            var sar = _context.Sar.Where(w => w.Id == SarId).ProjectToType<SarVm>().FirstOrDefault();
            return View(sar);
        }
        [HttpPost]
        [ClaimRequirement("Sar")] //Filtros
        public IActionResult Eliminar(SarVm sar)
        {
            var nuevoRegistro = _context.Sar.Where(w => w.Id == sar.Id).FirstOrDefault();
            nuevoRegistro.Eliminado = true;

            _context.SaveChanges();

            TempData["mensaje"] = "Registro Eliminado";
            return RedirectToAction("Index");
        }
        //Buscar Nombre
        [HttpGet]
        [ClaimRequirement("Sar")] //Filtros
        public IActionResult BuscarNombre()
        {
            return View(new SarVm());
        }
        [HttpPost]
        [ClaimRequirement("Sar")] //Filtros
        public JsonResult BuscarSarNombre([FromForm] SarVm vm)
        {
            if (vm == null || vm.CAI == 0)
            {
                var errorMessage = "Ingrese el CAI a buscar";
                var errorResult = AppResult.Error(errorMessage);
                return new JsonResult(BadRequest(errorResult));
            }
            var sar = _context.Sar
                              .Where(w => !w.Eliminado && w.CAI.ToString().ToLower() == vm.CAI.ToString().ToLower())
                              .ProjectToType<SarVm>()
                              .ToList();

            var result = sar.Count == 0 ? AppResult.Error("No se encontró el registro SAR") : AppResult.Success("Registro SAR encontrado", sar);
            return new JsonResult(Ok(result));
        }
    }
}
