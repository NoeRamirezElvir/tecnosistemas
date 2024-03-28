using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TECNOSISTEMAS.Data;
using TECNOSISTEMAS.Data.Entidades;
using TECNOSISTEMAS.Models;

namespace TECNOSISTEMAS.Controllers
{

    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private TecnosistemasDbContext _context;

        public UsuarioController(ILogger<UsuarioController> logger, TecnosistemasDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index(string Codigo)
        {
            //Filtros
            if (!string.IsNullOrEmpty(Codigo) && Codigo == "1")
            {
                ViewBag.Error = "Su sesion ha expirado";
                ViewBag.ClaseMensaje = "alert alert-danger alert-dismissable";
            }
            return View();
        }

        //Autentificacion

        [HttpPost]
        public IActionResult Index(UsuarioVm vm)
        {
            var usuario = _context.Usuario.Where(w => w.Eliminado == false & w.Email == vm.Email).ProjectToType<UsuarioVm>().FirstOrDefault();
            if (usuario == null)
            {
                ViewBag.Error = "Usuario o la Contraseña no Existen.";
                return View(new UsuarioVm());
            } if (string.IsNullOrEmpty(vm.Password) || string.IsNullOrEmpty(vm.Email))
            {

                ViewBag.Error = "Usuario o la Contraseña Incorrectos.";
                return View(new UsuarioVm());
            }
            if (usuario.Password != Utilidades.Utilidades.GetMD5(vm.Password))
            {
                ViewBag.Error = "Usuario o la Contraseña no Existen.";
                return View(new UsuarioVm());
            }
            var modulosRoles = _context.ModulosRoles.Where(w => w.Eliminado == false && w.RolId == usuario.Rol.Id).ProjectToType<ModulosRolesVm>().ToList();
            var agrupadosId = modulosRoles.Select(s => s.Modulo.AgrupadoModulosId).Distinct().ToList();
            var agrupados = _context.AgrupadoModulos.Where(w => agrupadosId.Contains(w.Id)).ProjectToType<AgrupadoModulosVm>().ToList();
            foreach (var item in agrupados)
            {
                var modulosActuales = modulosRoles.Where(w => w.Modulo.AgrupadoModulosId == item.Id).Select(s => s.Modulo.Id).Distinct().ToList();
                item.Modulos = item.Modulos.Where(w => modulosActuales.Contains(w.Id)).ToList();
            }
            usuario.Menu = agrupados;
            //Convertir el usuario a un JSON y enviarlo al cache del navegador
            var sesionJson = JsonConvert.SerializeObject(usuario);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(sesionJson);
            var sesionBas64 = System.Convert.ToBase64String(plainTextBytes);
            HttpContext.Session.SetString("UsuarioObjeto", sesionBas64);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Registro()
        {
            var roles =_context.Rol.Where(w => w.Eliminado==false).ProjectToType<RolVm>().ToList();
            List<SelectListItem> itemsroles = roles.ConvertAll(d => {
                return new SelectListItem
                {
                    Text = d.Descripcion,
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
            UsuarioVm usuario = new UsuarioVm();
            usuario.Roles = itemsroles;
            return View(usuario);
        }
        [HttpPost]
        public IActionResult Registro(UsuarioVm usuario)
        {
            if (!usuario.Validaciones_usuario())
            {
                TempData["mensaje"] = "Llene los campos";
                var roles = _context.Rol.Where(w => w.Eliminado == false).ProjectToType<RolVm>().ToList();
                List<SelectListItem> itemsRoles = roles.ConvertAll(d =>
                {
                    return new SelectListItem
                    {
                        Text = d.Descripcion,
                        Value = d.Id.ToString(),
                        Selected = false
                    };
                });

                usuario.Roles = itemsRoles;

                return View(usuario);
            }

            Usuario usuarioNuevo = new Usuario();
            usuarioNuevo.Id = Guid.NewGuid();
            usuarioNuevo.Nombre = usuario.Nombre;
            usuarioNuevo.Email = usuario.Email;
            usuarioNuevo.Password = Utilidades.Utilidades.GetMD5(usuario.Password);
            usuarioNuevo.apellido = usuario.apellido;
            usuarioNuevo.direccion = usuario.direccion;
            usuarioNuevo.DNI = usuario.DNI;
            usuarioNuevo.fechaNacimiento = usuario.fechaNacimiento;
            usuarioNuevo.telefono = usuario.telefono;
            usuarioNuevo.RolId = usuario.RolId;
            usuarioNuevo.usuario = "Usuario Nuevo";
            usuarioNuevo.CreatedDate = DateTime.Now;
            usuarioNuevo.CreatedBy = Guid.Empty;
            usuarioNuevo.Eliminado = false;

            _context.Usuario.Add(usuarioNuevo);
            _context.SaveChanges();
            TempData["mensaje"] = "Registro Exitoso";
            return RedirectToAction("Index");

        }
        //Guardar Usuario
    }
}
