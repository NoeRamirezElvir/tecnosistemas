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
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private TecnosistemasDbContext _context;

        public ClienteController(ILogger<ClienteController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Principal
        [ClaimRequirement("Cliente")] //Filtros
        public IActionResult Index()
        {
            var cliente = _context.Cliente.Where(w => w.Eliminado == false).ProjectToType<ClienteVm>().ToList();
            return View(cliente);
        }
        //Insertar
        [HttpGet]
        [ClaimRequirement("Cliente")] //Filtros
        public IActionResult Insertar()
        {
            ClienteVm cliente = new ClienteVm();
            return View(cliente);
        }
        [HttpPost]
        [ClaimRequirement("Cliente")] //Filtros
        public IActionResult Insertar(ClienteVm cliente)
        {
            //obtener Usuario
            var sesionBase64 = HttpContext.Session.GetString("UsuarioObjeto");
            var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
            var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            UsuarioVm UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
            //
            if (!cliente.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(cliente);
            }
            Cliente nuevoRegistro = new Cliente();
            nuevoRegistro.Nombre = cliente.Nombre;
            nuevoRegistro.Apellido = cliente.Apellido;
            nuevoRegistro.Direccion = cliente.Direccion;
            nuevoRegistro.Telefono = cliente.Telefono;
            nuevoRegistro.RTN = cliente.RTN;

            nuevoRegistro.Eliminado = false;
            nuevoRegistro.CreatedDate = DateTime.Now;
            nuevoRegistro.CreatedBy = UsuarioObjeto.Id;
            nuevoRegistro.Id = Guid.NewGuid();

            _context.Cliente.Add(nuevoRegistro);
            _context.SaveChanges();

            TempData["mensaje"] = "Registro Exitoso";
            return RedirectToAction("Index");
        }
        //Editar
        [HttpGet]
        [ClaimRequirement("Cliente")] //Filtros
        public IActionResult Editar(Guid CLienteId)
        {
            var cliente = _context.Cliente.Where(w => w.Id == CLienteId).ProjectToType<ClienteVm>().FirstOrDefault();
            return View(cliente);
        }
        [HttpPost]
        [ClaimRequirement("Cliente")] //Filtros
        public IActionResult Editar(ClienteVm cliente)
        {
            if (!cliente.Validacion())
            {
                TempData["mensaje"] = "Llene todos los campos";
                return View(cliente);
            }
            var nuevoRegistro = _context.Cliente.Where(w => w.Id == cliente.Id).FirstOrDefault();
            nuevoRegistro.Nombre = cliente.Nombre;
            nuevoRegistro.Apellido = cliente.Apellido;
            nuevoRegistro.Direccion = cliente.Direccion;
            nuevoRegistro.Telefono = cliente.Telefono;
            nuevoRegistro.RTN = cliente.RTN;

            _context.SaveChanges();

            TempData["mensaje"] = "Actualizacion Exitosa";
            return RedirectToAction("Index");
        }
        //Eliminar
        [HttpGet]
        [ClaimRequirement("Cliente")] //Filtros
        public IActionResult Eliminar(Guid ClienteId)
        {
            var cliente = _context.Cliente.Where(w => w.Id == ClienteId).ProjectToType<ClienteVm>().FirstOrDefault();
            return View(cliente);
        }
        [HttpPost]
        [ClaimRequirement("Cliente")] //Filtros
        public IActionResult Eliminar(ClienteVm cliente)
        {
            var nuevoRegistro = _context.Cliente.Where(w => w.Id == cliente.Id).FirstOrDefault();
            nuevoRegistro.Eliminado = true;

            _context.SaveChanges();

            TempData["mensaje"] = "Registro Eliminado";
            return RedirectToAction("Index");
        }
        //Buscar Nombre
        [HttpGet]
        [ClaimRequirement("Cliente")] //Filtros
        public IActionResult BuscarNombre()
        {
            return View(new ClienteVm());
        }
        [HttpPost]
        [ClaimRequirement("Cliente")] //Filtros
        public JsonResult BuscarClienteNombre([FromForm] ClienteVm vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Nombre))
            {
                var errorMessage = "Escriba el nombre a buscar";
                var errorResult = AppResult.Error(errorMessage);
                return new JsonResult(BadRequest(errorResult));
            }
            var cliente = _context.Cliente.Where(w => w.Eliminado == false && w.Nombre.ToLower() == vm.Nombre.ToLower()).ProjectToType<ClienteVm>().ToList();
            var result = cliente.Count == 0 ? AppResult.Error("No se encontro el Cliente") : AppResult.Success("Cliente encontrado", cliente);
            return new JsonResult(Ok(result));
        }
    }
}
