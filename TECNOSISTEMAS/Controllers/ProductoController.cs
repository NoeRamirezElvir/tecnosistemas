using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TECNOSISTEMAS.Data;
using TECNOSISTEMAS.Data.Entidades;
using TECNOSISTEMAS.Filters;
using TECNOSISTEMAS.Models;
using TECNOSISTEMAS.Utilidades;

namespace TECNOSISTEMAS.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ILogger<ProductoController> _logger;
        private TecnosistemasDbContext _context;

        public ProductoController(ILogger<ProductoController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        [ClaimRequirement("Producto")] //Filtros
        public IActionResult Index()
        {
            var producto = _context.Producto.Where(w => w.Eliminado == false).ProjectToType<ProductoVm>().ToList();
            return View(producto);
        }
        //Buscar Nombre
        [HttpGet]
        [ClaimRequirement("Producto")] //Filtros
        public IActionResult BuscarProducto()
        {
            return View(new ProductoVm());
        }
        [HttpPost]
        [ClaimRequirement("Producto")] //Filtros
        public JsonResult BuscarProductoNombre([FromForm] ProductoVm vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Nombre))
            {
                var errorMessage = "Escriba el nombre a buscar";
                var errorResult = AppResult.Error(errorMessage);
                return new JsonResult(BadRequest(errorResult));
            }
            var producto = _context.Producto.Where(w => w.Eliminado == false && w.Nombre.ToLower() == vm.Nombre.ToLower()).ProjectToType<ProductoVm>().ToList();
            var result = producto.Count == 0 ? AppResult.Error("No se encontro el producto") : AppResult.Success("Producto encontrado", producto);
            return new JsonResult(Ok(result));
        }
        //Insertar
        [HttpGet]
        [ClaimRequirement("Producto")] //Filtros
        public IActionResult Insertar()
        {
            var categoria = _context.Categoria.Where(w => w.Eliminado == false).ProjectToType<CategoriaVm>().ToList();
            List<SelectListItem> items = categoria.ConvertAll(d => {
                return new SelectListItem
                {
                    Text = d.Nombre,
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
            ProductoVm producto = new ProductoVm();
            producto.CategoriaLista = items;
            return View(producto);
        }
        [HttpPost]
        [ClaimRequirement("Producto")] //Filtros
        public IActionResult Insertar(ProductoVm producto)
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //
            if (!producto.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                var categoria = _context.Categoria.Where(w => w.Eliminado == false).ProjectToType<CategoriaVm>().ToList();
                List<SelectListItem> items = categoria.ConvertAll(d => {
                    return new SelectListItem
                    {
                        Text = d.Nombre,
                        Value = d.Id.ToString(),
                        Selected = false
                    };
                });
                producto.CategoriaLista = items;
                return View(producto);
            }
            Producto nuevoRegistro = new Producto();
            nuevoRegistro.Nombre = producto.Nombre;
            nuevoRegistro.Precio = producto.Precio;
            nuevoRegistro.Costo = producto.Costo;
            nuevoRegistro.Activo = true;
            nuevoRegistro.IdCategoria = producto.IdCategoria;


            nuevoRegistro.Eliminado = false;
            nuevoRegistro.CreatedDate = DateTime.Now;
            nuevoRegistro.CreatedBy = UsuarioObjeto.Id;
            nuevoRegistro.Id = Guid.NewGuid();
            _context.Producto.Add(nuevoRegistro);
            _context.SaveChanges();
            TempData["mensaje"] = "Registro Exitoso";
            return RedirectToAction("Index");
        }
        //Editar
        [HttpGet]
        [ClaimRequirement("Producto")] //Filtros
        public IActionResult Editar(Guid ProductoId)
        {
            var categoria = _context.Categoria.Where(w => w.Eliminado == false).ProjectToType<CategoriaVm>().ToList();
            List<SelectListItem> items = categoria.ConvertAll(d => {
                return new SelectListItem
                {
                    Text = d.Nombre,
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
            var producto = _context.Producto.Where(w => w.Id == ProductoId).ProjectToType<ProductoVm>().FirstOrDefault();
            producto.CategoriaLista = items;
            return View(producto);
        }
        [HttpPost]
        [ClaimRequirement("Producto")] //Filtros
        public IActionResult Editar(ProductoVm producto)
        {
            if (!producto.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                var categoria = _context.Categoria.Where(w => w.Eliminado == false).ProjectToType<CategoriaVm>().ToList();
                List<SelectListItem> items = categoria.ConvertAll(d => {
                    return new SelectListItem
                    {
                        Text = d.Nombre,
                        Value = d.Id.ToString(),
                        Selected = false
                    };
                });
                producto.CategoriaLista = items;
                return View(producto);
            }
            var nuevoRegistro = _context.Producto.Where(w => w.Id == producto.Id).FirstOrDefault();
            nuevoRegistro.Nombre = producto.Nombre;
            nuevoRegistro.Precio = producto.Precio;
            nuevoRegistro.Costo = producto.Costo;
            nuevoRegistro.IdCategoria = producto.IdCategoria;
            _context.SaveChanges();

            TempData["mensaje"] = "Actualizacion Exitosa";
            return RedirectToAction("Index");
        }
        //Eliminar
        [HttpGet]
        [ClaimRequirement("Producto")] //Filtros
        public IActionResult Eliminar(Guid ProductoId)
        {
            var categoria = _context.Categoria.Where(w => w.Eliminado == false).ProjectToType<CategoriaVm>().ToList();
            List<SelectListItem> items = categoria.ConvertAll(d => {
                return new SelectListItem
                {
                    Text = d.Nombre,
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
            var producto = _context.Producto.Where(w => w.Id == ProductoId).ProjectToType<ProductoVm>().FirstOrDefault();
            producto.CategoriaLista = items;
            return View(producto);
        }
        [HttpPost]
        [ClaimRequirement("Producto")] //Filtros
        public IActionResult Eliminar(ProductoVm producto)
        {
            var nuevoRegistro = _context.Producto.Where(w => w.Id == producto.Id).FirstOrDefault();
            nuevoRegistro.Eliminado = true;

            _context.SaveChanges();

            TempData["mensaje"] = "Registro Eliminado";
            return RedirectToAction("Index");
        }

    }
}
