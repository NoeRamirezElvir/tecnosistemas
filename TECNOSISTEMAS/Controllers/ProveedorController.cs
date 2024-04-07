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
    public class ProveedorController : Controller
    {
        private readonly ILogger<ProveedorController> _logger;
        private TecnosistemasDbContext _context;

        public ProveedorController(ILogger<ProveedorController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Principal
        [ClaimRequirement("Proveedor")] //Filtros
        public IActionResult Index()
        {
            var proveedor = _context.Proveedor.Where(w => w.Eliminado == false).OrderBy(p => p.Nombre).ProjectToType<ProveedorVm>().ToList();
            return View(proveedor);
        }
        //Insertar
        [HttpGet]
        [ClaimRequirement("Proveedor")] //Filtros
        public IActionResult Insertar()
        {
            ProveedorVm proveedor = new ProveedorVm();
            return View(proveedor);
        }
        [HttpPost]
        [ClaimRequirement("Proveedor")] //Filtros
        public IActionResult Insertar(ProveedorVm proveedor)
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //
            bool existente = _context.Proveedor.Any(c => c.Nombre == proveedor.Nombre && !c.Eliminado && c.Id != proveedor.Id);
            if (!string.IsNullOrEmpty(proveedor.Validacion(existente)))
            {
                TempData["mensaje"] = proveedor.Validacion(existente); //Captura el mensaje del modelo
                return View(proveedor);
            }
            //
            Proveedor nuevoRegistro = new Proveedor();
            nuevoRegistro.Nombre = proveedor.Nombre;
            nuevoRegistro.Direccion = proveedor.Direccion;
            nuevoRegistro.Telefono = proveedor.Telefono;
            nuevoRegistro.CorreoElectronico = proveedor.CorreoElectronico;

            nuevoRegistro.Eliminado = false;
            nuevoRegistro.CreatedDate = DateTime.Now;
            nuevoRegistro.CreatedBy = UsuarioObjeto.Id;
            nuevoRegistro.Id = Guid.NewGuid();

            _context.Proveedor.Add(nuevoRegistro);
            _context.SaveChanges();

            TempData["mensaje"] = "Registro Exitoso";
            return RedirectToAction("Index");
        }
        //Editar
        [HttpGet]
        [ClaimRequirement("Proveedor")] //Filtros
        public IActionResult Editar(Guid ProveedorId)
        {
            var proveedor = _context.Proveedor.Where(w => w.Id == ProveedorId).OrderBy(p => p.Nombre).ProjectToType<ProveedorVm>().FirstOrDefault();
            return View(proveedor);
        }
        [HttpPost]
        [ClaimRequirement("Proveedor")] //Filtros
        public IActionResult Editar(ProveedorVm proveedor)
        {
            //
            bool existente = _context.Proveedor.Any(c => c.Nombre == proveedor.Nombre && !c.Eliminado && c.Id != proveedor.Id);
            if (!string.IsNullOrEmpty(proveedor.Validacion(existente)))
            {
                TempData["mensaje"] = proveedor.Validacion(existente); //Captura el mensaje del modelo
                return View(proveedor);
            }
            //
            var nuevoRegistro = _context.Proveedor.Where(w => w.Id == proveedor.Id).FirstOrDefault();
            nuevoRegistro.Nombre = proveedor.Nombre;
            nuevoRegistro.Direccion = proveedor.Direccion;
            nuevoRegistro.Telefono = proveedor.Telefono;
            nuevoRegistro.CorreoElectronico = proveedor.CorreoElectronico;

            _context.SaveChanges();

            TempData["mensaje"] = "Actualizacion Exitosa";
            return RedirectToAction("Index");
        }
        //Eliminar
        [HttpGet]
        [ClaimRequirement("Proveedor")] //Filtros
        public IActionResult Eliminar(Guid ProveedorId)
        {
            var proveedor = _context.Proveedor.Where(w => w.Id == ProveedorId).OrderBy(p => p.Nombre).ProjectToType<ProveedorVm>().FirstOrDefault();
            return View(proveedor);
        }
        [HttpPost]
        [ClaimRequirement("Proveedor")] //Filtros
        public IActionResult Eliminar(ProveedorVm proveedor)
        {
            var nuevoRegistro = _context.Proveedor.Where(w => w.Id == proveedor.Id).FirstOrDefault();
            nuevoRegistro.Eliminado = true;

            _context.SaveChanges();

            TempData["mensaje"] = "Registro Eliminado";
            return RedirectToAction("Index");
        }
        //Buscar Nombre
        [HttpGet]
        [ClaimRequirement("Proveedor")] //Filtros
        public IActionResult BuscarNombre()
        {
            return View(new ProveedorVm());
        }
        [HttpPost]
        [ClaimRequirement("Proveedor")] //Filtros
        public JsonResult BuscarProveedorNombre([FromForm] ProveedorVm vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Nombre))
            {
                var errorMessage = "Escriba el nombre a buscar";
                var errorResult = AppResult.Error(errorMessage);
                return new JsonResult(BadRequest(errorResult));
            }
            var proveedor = _context.Proveedor.Where(w => w.Eliminado == false && w.Nombre.ToLower() == vm.Nombre.ToLower()).OrderBy(p => p.Nombre).ProjectToType<ProveedorVm>().ToList();
            var result = proveedor.Count == 0 ? AppResult.Error("No se encontro el Proveedor") : AppResult.Success("Proveedor encontrado", proveedor);
            return new JsonResult(Ok(result));
        }
    }
}
