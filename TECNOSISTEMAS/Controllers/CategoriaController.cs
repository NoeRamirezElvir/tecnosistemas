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
    public class CategoriaController : Controller
    {
        private readonly ILogger<CategoriaController> _logger;
        private TecnosistemasDbContext _context;

        public CategoriaController(ILogger<CategoriaController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Principal
        [ClaimRequirement("Categoria")] //Filtros
        public IActionResult Index()
        {
            var categoria = _context.Categoria.Where(w => w.Eliminado == false).ProjectToType<CategoriaVm>().ToList();
            return View(categoria);
        }
        //Insertar
        [HttpGet]
        [ClaimRequirement("Categoria")] //Filtros
        public IActionResult Insertar()
        {
            CategoriaVm categoria = new CategoriaVm();
            return View(categoria);
        }
        [HttpPost]
        [ClaimRequirement("Categoria")] //Filtros
        public IActionResult Insertar(CategoriaVm categoria)
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //
            if (!categoria.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(categoria);
            }
            Categoria nuevoRegistro = new Categoria();
            nuevoRegistro.Nombre = categoria.Nombre;
            nuevoRegistro.Descripcion = categoria.Descripcion;

            nuevoRegistro.Eliminado = false;
            nuevoRegistro.CreatedDate = DateTime.Now;
            nuevoRegistro.CreatedBy = UsuarioObjeto.Id;
            nuevoRegistro.Id = Guid.NewGuid();
            _context.Categoria.Add(nuevoRegistro);
            _context.SaveChanges();
            TempData["mensaje"] = "Registro Exitoso";
            return RedirectToAction("Index");
        }
        //Editar
        [HttpGet]
        [ClaimRequirement("Categoria")] //Filtros
        public IActionResult Editar(Guid CategoriaId)
        {
            var categoria = _context.Categoria.Where(w => w.Id == CategoriaId).ProjectToType<CategoriaVm>().FirstOrDefault();
            return View(categoria);
        }
        [HttpPost]
        [ClaimRequirement("Categoria")] //Filtros
        public IActionResult Editar(CategoriaVm categoria)
        {
            if (!categoria.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(categoria);
            }
            var nuevoRegistro = _context.Categoria.Where(w => w.Id == categoria.Id).FirstOrDefault();
            nuevoRegistro.Nombre = categoria.Nombre;
            nuevoRegistro.Descripcion = categoria.Descripcion;
            _context.SaveChanges();

            TempData["mensaje"] = "Actualizacion Exitosa";
            return RedirectToAction("Index");
        }
        //Eliminar
        [HttpGet]
        [ClaimRequirement("Categoria")] //Filtros
        public IActionResult Eliminar(Guid CategoriaId)
        {
            var categoria = _context.Categoria.Where(w => w.Id == CategoriaId).ProjectToType<CategoriaVm>().FirstOrDefault();
            return View(categoria);
        }
        [HttpPost]
        [ClaimRequirement("Categoria")] //Filtros
        public IActionResult Eliminar(CategoriaVm categoria)
        {
            var nuevoRegistro = _context.Categoria.Where(w => w.Id == categoria.Id).FirstOrDefault();
            nuevoRegistro.Eliminado = true;

            _context.SaveChanges();

            TempData["mensaje"] = "Registro Eliminado";
            return RedirectToAction("Index");
        }
        //Buscar Nombre
        [HttpGet]
        [ClaimRequirement("Categoria")] //Filtros
        public IActionResult BuscarNombre()
        {
            return View(new CategoriaVm());
        }
        [HttpPost]
        [ClaimRequirement("Categoria")] //Filtros
        public JsonResult BuscarCategoriaNombre([FromForm] CategoriaVm vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Nombre))
            {
                var errorMessage = "Escriba el nombre a buscar";
                var errorResult = AppResult.Error(errorMessage);
                return new JsonResult(BadRequest(errorResult));
            }
            var categoria = _context.Categoria.Where(w => w.Eliminado == false && w.Nombre.ToLower() == vm.Nombre.ToLower()).ProjectToType<CategoriaVm>().ToList();
            var result = categoria.Count == 0 ? AppResult.Error("No se encontro la Categoría") : AppResult.Success("Categoría encontrada", categoria);
            return new JsonResult(Ok(result));
        }
    }
}
