using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Linq;
using TECNOSISTEMAS.Data;
using TECNOSISTEMAS.Data.Entidades;
using TECNOSISTEMAS.Filters;
using TECNOSISTEMAS.Migrations;
using TECNOSISTEMAS.Models;
using TECNOSISTEMAS.Utilidades;

namespace TECNOSISTEMAS.Controllers
{
    public class CompraProductoController : Controller
    {
        private readonly ILogger<CompraProductoController> _logger;
        private TecnosistemasDbContext _context;

        public CompraProductoController(ILogger<CompraProductoController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        [ClaimRequirement("Compra")] //Filtros
        public IActionResult Index()
        {
            var compra = _context.Compra.Where(w => w.Eliminado == false).Take(20).ProjectToType<CompraVm>().ToList();
            return View(compra);
        }
        //BUSCAR POR FECHA
        [HttpGet]
        [ClaimRequirement("Compra")] //Filtros
        public IActionResult BuscarCompra()
        {
            var compra = _context.Compra.Where(w => w.Eliminado == false).Take(20).ProjectToType<CompraVm>().ToList();

            DateTime fechaActual = DateTime.Today;
            DateTime fechaInicial = fechaActual.AddDays(-1);

            var modelo = new BuscarCompraVm
            {
                Compras = compra,
                NuevaCompra = new CompraVm(),
                FechaInicial = fechaInicial,
                FechaFinal = fechaActual
            };
            return View(modelo);
        }
        [HttpPost]
        [ClaimRequirement("Compra")] //Filtros
        public JsonResult BuscarCompraFecha([FromForm] BuscarCompraVm vm)
        {
            var compras = _context.Compra.Where(w => w.Eliminado == false && w.CreatedDate.Date >= vm.FechaInicial && w.CreatedDate.Date <= vm.FechaFinal).ProjectToType<CompraVm>().ToList();
            if (!string.IsNullOrEmpty(vm.Validacion()))
            {
                var resultv = compras.Count == 0 ? AppResult.Error(vm.Validacion()) : AppResult.Success("Compras encontradas", compras);
                return new JsonResult(Ok(resultv));
            }
            var result = compras.Count == 0 ? AppResult.Error("No se encontraron compras") : AppResult.Success("Compras encontradas", compras);
            return new JsonResult(Ok(result));
        }
        //Insertar
        [HttpGet]
        [ClaimRequirement("Compra")] //Filtros
        public IActionResult Insertar()
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //
            var producto = _context.Producto.Where(w => w.Eliminado == false).OrderBy(p => p.Nombre).ProjectToType<ProductoVm>().ToList();
            List<SelectListItem> itemsproducto = producto.ConvertAll(d => {
                return new SelectListItem
                {
                    Text = $"{d.Nombre} - ${d.Costo}",
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });

            var proveedor = _context.Proveedor.Where(w => w.Eliminado == false).OrderBy(p => p.Nombre).ProjectToType<ProveedorVm>().ToList();
            List<SelectListItem> itemsproveedor = proveedor.ConvertAll(d => {
                return new SelectListItem
                {
                    Text = d.Nombre,
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
            CompraProductoVm compraProducto = new CompraProductoVm();
            compraProducto.ProductoLista = itemsproducto;
            compraProducto.ProveedorLista = itemsproveedor;
            compraProducto.CreatedDate = DateTime.Now;
            compraProducto.usuario = $"{UsuarioObjeto.Nombre} {UsuarioObjeto.apellido}";
            return View(compraProducto);
        }
        [HttpPost]
        [ClaimRequirement("Compra")] //Filtros
        public IActionResult Insertar(CompraProductoVm compraProducto)
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //

            if (!string.IsNullOrEmpty(compraProducto.Validacion()))
            {
                var producto = _context.Producto.Where(w => w.Eliminado == false).OrderBy(p => p.Nombre).ProjectToType<ProductoVm>().ToList();
                List<SelectListItem> itemsproducto = producto.ConvertAll(d => {
                    return new SelectListItem
                    {
                        Text = $"{d.Nombre} - ${d.Costo}",
                        Value = d.Id.ToString(),
                        Selected = false
                    };
                });
                var proveedor = _context.Proveedor.Where(w => w.Eliminado == false).OrderBy(p => p.Nombre).ProjectToType<ProveedorVm>().ToList();
                List<SelectListItem> itemsproveedor = proveedor.ConvertAll(d => {
                    return new SelectListItem
                    {
                        Text = d.Nombre,
                        Value = d.Id.ToString(),
                        Selected = false
                    };
                });
                compraProducto.ProductoLista = itemsproducto;
                compraProducto.ProveedorLista = itemsproveedor;
                compraProducto.IdProductoLista = Request.Form["IdProductoLista"];
                compraProducto.CreatedDate = DateTime.Now;
                compraProducto.usuario = $"{UsuarioObjeto.Nombre} {UsuarioObjeto.apellido}";
                
                TempData["mensaje"] = compraProducto.Validacion();//Captura el mensaje del modelo
                return View(compraProducto);
            }
            //VALIDACIONES INVENTARIO
            var json = compraProducto.IdProductoLista;
            List<ProductoInfoCP> productos = JsonConvert.DeserializeObject<List<ProductoInfoCP>>(json);//Convertir el texto del input hidden a un json, para convertirlo en una lista de ProductoInfoCompraProducto
            foreach (var productov in productos)
            {
                var bproducto = _context.Producto.Where(w => w.Id == Guid.Parse(productov.Id)).FirstOrDefault();
                var binventario = _context.Inventario.Where(w => w.IdProducto == Guid.Parse(productov.Id)).FirstOrDefault();
                if (binventario == null){

                    binventario = new Inventario
                    {
                        IdProducto = Guid.Parse(productov.Id),
                        StockActual = 0,
                        StockMaximo = -100

                    };
                }
                if (!string.IsNullOrEmpty(compraProducto.ValidacionInventario(binventario.StockActual + productov.Cantidad, binventario.StockMaximo)))
                {
                    var producto = _context.Producto.Where(w => w.Eliminado == false).OrderBy(p => p.Nombre).ProjectToType<ProductoVm>().ToList();
                    List<SelectListItem> itemsproducto = producto.ConvertAll(d => {
                        return new SelectListItem
                        {
                            Text = $"{d.Nombre} - ${d.Costo}",
                            Value = d.Id.ToString(),
                            Selected = false
                        };
                    });
                    var proveedor = _context.Proveedor.Where(w => w.Eliminado == false).OrderBy(p => p.Nombre).ProjectToType<ProveedorVm>().ToList();
                    List<SelectListItem> itemsproveedor = proveedor.ConvertAll(d => {
                        return new SelectListItem
                        {
                            Text = d.Nombre,
                            Value = d.Id.ToString(),
                            Selected = false
                        };
                    });
                    compraProducto.ProductoLista = itemsproducto;
                    compraProducto.ProveedorLista = itemsproveedor;
                    compraProducto.IdProductoLista = Request.Form["IdProductoLista"];
                    compraProducto.CreatedDate = DateTime.Now;
                    compraProducto.usuario = $"{UsuarioObjeto.Nombre} {UsuarioObjeto.apellido}";

                    TempData["mensaje"] = $"{compraProducto.ValidacionInventario(binventario.StockActual + productov.Cantidad, binventario.StockMaximo)}  {bproducto.Nombre}";//Captura el mensaje del modelo
                    return View(compraProducto);
                }
            }
            //
            var total = 0.0;
            foreach (var producto in productos)
            {
                int cantidad = producto.Cantidad;
                decimal costo = producto.Costo;
                total += cantidad * (double)costo ;
            }
            //Crear la Compra
            Compra compraEncabezado = new Compra();
            compraEncabezado.Total = total;
            compraEncabezado.Eliminado = false;
            compraEncabezado.CreatedDate = DateTime.Now;
            compraEncabezado.CreatedBy = UsuarioObjeto.Id;
            compraEncabezado.Id = Guid.NewGuid();
            _context.Compra.Add(compraEncabezado);
            _context.SaveChanges();

            //Crear el CompraProducto
            foreach (var producto in productos)
            {
                var bproducto = _context.Producto.Where(w => w.Id == Guid.Parse(producto.Id)).FirstOrDefault();
                var bproveedor = _context.Proveedor.Where(w => w.Id == Guid.Parse(producto.Proveedor)).FirstOrDefault();
                var binventario = _context.Inventario.Where(w => w.IdProducto == Guid.Parse(producto.Id)).FirstOrDefault();

                CompraProducto compraDetalle = new CompraProducto();
                compraDetalle.Cantidad = producto.Cantidad;
                compraDetalle.CostoCompra = bproducto.Costo;
                compraDetalle.PrecioVenta = bproducto.Precio;
                compraDetalle.Subtotal = bproducto.Costo * producto.Cantidad; ;
                compraDetalle.IdProducto = bproducto.Id;
                compraDetalle.IdProveedor = bproveedor.Id;
                compraDetalle.IdCompra = compraEncabezado.Id;

                compraDetalle.Eliminado = false;
                compraDetalle.CreatedDate = DateTime.Now;
                compraDetalle.CreatedBy = UsuarioObjeto.Id;
                compraDetalle.Id = Guid.NewGuid();
                _context.CompraProducto.Add(compraDetalle);
                _context.SaveChanges();

                //Actualizar inventario
                binventario.StockActual += producto.Cantidad;
                _context.SaveChanges();
                //
            }

            TempData["mensaje"] = "Registro Exitoso - Inventario Actualizado";
            return RedirectToAction("BuscarCompra");
        }
        //Editar
        [HttpGet]
        [ClaimRequirement("Compra")] //Filtros
        public IActionResult Ver(Guid CompraId)
        {
            var compraEncabezado = _context.Compra.Where(w => w.Id == CompraId).ProjectToType<CompraVm>().FirstOrDefault();
            var compraDetalles = _context.CompraProducto.Where(w => w.IdCompra == CompraId).OrderBy(p => p.Producto.Nombre).ProjectToType<CompraProductoVm>().ToList();
            var usuario = _context.Usuario.Where(w => w.Id == compraEncabezado.CreatedBy).ProjectToType<UsuarioVm>().FirstOrDefault();


            var listaProductos = compraDetalles.Select(detalle => new
            {
                id = detalle.Id.ToString(),
                nombre = detalle.Producto.Nombre.Trim(),
                cantidad = detalle.Cantidad,
                proveedor = detalle.Proveedor.Id.ToString(),
                proveedorNombre = detalle.Proveedor.Nombre.Trim(),
                costo = detalle.CostoCompra
            }).ToList();

            string jsonResult = JsonConvert.SerializeObject(listaProductos);


            Console.WriteLine(jsonResult);
            var buscarCompra = new BuscarCompraVm();
            buscarCompra.NuevaCompra = compraEncabezado;
            buscarCompra.IdProductoLista = jsonResult;
           // buscarCompra.ListaCompras = compraDetalles;
            buscarCompra.usuario = usuario.Nombre;

            return View(buscarCompra);
        }

        //Eliminar
        [HttpGet]
        [ClaimRequirement("Compra")] //Filtros
        public IActionResult Eliminar(Guid CompraId)
        {
            var compraEncabezado = _context.Compra.Where(w => w.Id == CompraId).ProjectToType<CompraVm>().FirstOrDefault();
            var compraDetalles = _context.CompraProducto.Where(w => w.IdCompra == CompraId).OrderBy(p => p.Producto.Nombre).ProjectToType<CompraProductoVm>().ToList();
            var usuario = _context.Usuario.Where(w => w.Id == compraEncabezado.CreatedBy).ProjectToType<UsuarioVm>().FirstOrDefault();


            var listaProductos = compraDetalles.Select(detalle => new
            {
                id = detalle.Id.ToString(),
                nombre = detalle.Producto.Nombre.Trim(),
                cantidad = detalle.Cantidad,
                proveedor = detalle.Proveedor.Id.ToString(),
                proveedorNombre = detalle.Proveedor.Nombre.Trim(),
                costo = detalle.CostoCompra
            }).ToList();

            string jsonResult = JsonConvert.SerializeObject(listaProductos);


            Console.WriteLine(jsonResult);
            var buscarCompra = new BuscarCompraVm();
            buscarCompra.NuevaCompra = compraEncabezado;
            buscarCompra.IdProductoLista = jsonResult;
            //buscarCompra.ListaCompras = compraDetalles;
            buscarCompra.usuario = usuario.Nombre;

            return View(buscarCompra);
        }
        [HttpPost]
        [ClaimRequirement("Compra")] //Filtros
        public IActionResult Eliminar(BuscarCompraVm compra)
        {
            var nuevoRegistro = _context.Compra.Where(w => w.Id == compra.NuevaCompra.Id).FirstOrDefault();
            var compraDetalles = _context.CompraProducto.Where(w => w.IdCompra == compra.NuevaCompra.Id).OrderBy(p => p.Producto.Nombre).ToList();
            foreach (var item in compraDetalles)
            {
                item.Eliminado = true;
            }
            nuevoRegistro.Eliminado = true;
            _context.SaveChanges();
            TempData["mensaje"] = "Registro Eliminado - Detalles Eliminados";
            return RedirectToAction("BuscarCompra");
        }
    }
}
