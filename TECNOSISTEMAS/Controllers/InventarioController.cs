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
    public class InventarioController : Controller
    {
        private readonly ILogger<InventarioController> _logger;
        private TecnosistemasDbContext _context;

        public InventarioController(ILogger<InventarioController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        [ClaimRequirement("Inventario")] //Filtros
        public IActionResult Index()
        {
            var inventario = _context.Inventario.Where(w => w.Eliminado == false).ProjectToType<InventarioVm>().ToList();
            return View(inventario);
        }
        //Buscar Nombre Producto
        [HttpGet]
        [ClaimRequirement("Inventario")] //Filtros
        public IActionResult BuscarInventario()
        {
            return View(new InventarioVm());
        }
        [HttpPost]
        [ClaimRequirement("Inventario")] //Filtros
        public JsonResult BuscarInventarioProducto([FromForm] InventarioVm vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Producto.Nombre))
            {
                var errorMessage = "Escriba el nombre a buscar";
                var errorResult = AppResult.Error(errorMessage);
                return new JsonResult(BadRequest(errorResult));
            }
            var inventario = _context.Inventario.Where(w => w.Eliminado == false && w.Producto.Nombre.ToLower() == vm.Producto.Nombre.ToLower()).ProjectToType<InventarioVm>().ToList();
            var result = inventario.Count == 0 ? AppResult.Error("No se encontro el producto") : AppResult.Success("Producto encontrado", inventario);
            return new JsonResult(Ok(result));
        }
        //Insertar
        [HttpGet]
        [ClaimRequirement("Inventario")] //Filtros
        public IActionResult Insertar()
        {
            var producto = _context.Producto.Where(w => w.Eliminado == false).ProjectToType<ProductoVm>().ToList();
            List<SelectListItem> items = producto.ConvertAll(d => {
                return new SelectListItem
                {
                    Text = d.Nombre,
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
            InventarioVm inventario = new InventarioVm();
            inventario.ProductoLista = items;
            return View(inventario);
        }
        [HttpPost]
        [ClaimRequirement("Inventario")] //Filtros
        public IActionResult Insertar(InventarioVm inventario)
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //
            if (!inventario.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                var producto = _context.Producto.Where(w => w.Eliminado == false).ProjectToType<ProductoVm>().ToList();
                List<SelectListItem> items = producto.ConvertAll(d => {
                    return new SelectListItem
                    {
                        Text = d.Nombre,
                        Value = d.Id.ToString(),
                        Selected = false
                    };
                });
                inventario.ProductoLista = items;
                return View(inventario);
            }
            Inventario nuevoRegistro = new Inventario();
            nuevoRegistro.StockActual = inventario.StockActual;
            nuevoRegistro.StockMinimo = inventario.StockMinimo;
            nuevoRegistro.StockMaximo = inventario.StockMaximo;
            nuevoRegistro.IdProducto = inventario.IdProducto;


            nuevoRegistro.Eliminado = false;
            nuevoRegistro.CreatedDate = DateTime.Now;
            nuevoRegistro.CreatedBy = UsuarioObjeto.Id;
            nuevoRegistro.Id = Guid.NewGuid();
            _context.Inventario.Add(nuevoRegistro);
            _context.SaveChanges();
            TempData["mensaje"] = "Registro Exitoso";
            return RedirectToAction("Index");
        }
        //Editar
        [HttpGet]
        [ClaimRequirement("Inventario")] //Filtros
        public IActionResult Editar(Guid InventarioId)
        {
            var producto = _context.Producto.Where(w => w.Eliminado == false).ProjectToType<ProductoVm>().ToList();
            List<SelectListItem> items = producto.ConvertAll(d => {
                return new SelectListItem
                {
                    Text = d.Nombre,
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
            var inventario = _context.Inventario.Where(w => w.Id == InventarioId).ProjectToType<InventarioVm>().FirstOrDefault();
            inventario.ProductoLista = items;
            return View(inventario);
        }
        [HttpPost]
        [ClaimRequirement("Inventario")] //Filtros
        public IActionResult Editar(InventarioVm inventario)
        {
            if (!inventario.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                var producto = _context.Producto.Where(w => w.Eliminado == false).ProjectToType<ProductoVm>().ToList();
                List<SelectListItem> items = producto.ConvertAll(d => {
                    return new SelectListItem
                    {
                        Text = d.Nombre,
                        Value = d.Id.ToString(),
                        Selected = false
                    };
                });
                inventario.ProductoLista = items;
                return View(inventario);
            }
            var nuevoRegistro = _context.Inventario.Where(w => w.Id == inventario.Id).FirstOrDefault();
            nuevoRegistro.StockActual = inventario.StockActual;
            nuevoRegistro.StockMinimo = inventario.StockMinimo;
            nuevoRegistro.StockMaximo = inventario.StockMaximo;
            nuevoRegistro.IdProducto = inventario.IdProducto;
            _context.SaveChanges();

            TempData["mensaje"] = "Actualizacion Exitosa";
            return RedirectToAction("Index");
        }
        //Eliminar
        [HttpGet]
        [ClaimRequirement("Inventario")] //Filtros
        public IActionResult Eliminar(Guid InventarioId)
        {
            var producto = _context.Producto.Where(w => w.Eliminado == false).ProjectToType<ProductoVm>().ToList();
            List<SelectListItem> items = producto.ConvertAll(d => {
                return new SelectListItem
                {
                    Text = d.Nombre,
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
            var inventario = _context.Inventario.Where(w => w.Id == InventarioId).ProjectToType<InventarioVm>().FirstOrDefault();
            inventario.ProductoLista = items;
            return View(inventario);
        }
        [HttpPost]
        [ClaimRequirement("Inventario")] //Filtros
        public IActionResult Eliminar(InventarioVm inventario)
        {
            var nuevoRegistro = _context.Inventario.Where(w => w.Id == inventario.Id).FirstOrDefault();
            nuevoRegistro.Eliminado = true;

            _context.SaveChanges();

            TempData["mensaje"] = "Registro Eliminado";
            return RedirectToAction("Index");
        }
    }
}
