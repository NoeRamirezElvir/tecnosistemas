using Microsoft.AspNetCore.Mvc;
using TECNOSISTEMAS.Filters;
using TECNOSISTEMAS.Models;
using System.Diagnostics;

namespace TECNOSISTEMAS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [ClaimRequirement("principal")] //Filtros
        public IActionResult Index(string Codigo)
        {
            //Filtros
            if (!string.IsNullOrEmpty(Codigo) && Codigo == "1")
            {
                ViewBag.Error = "No tiene acceso a ese modulo";
                ViewBag.ClaseMensaje = "alert alert-danger alert-dismissable";
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Usuario");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
