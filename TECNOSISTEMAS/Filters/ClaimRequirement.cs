using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using TECNOSISTEMAS.Models;

namespace TECNOSISTEMAS.Filters
{
    public class ClaimRequirementAttribute : TypeFilterAttribute

    {
        public ClaimRequirementAttribute(string Nombre) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new ModuloVm { Nombre = Nombre } };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private UsuarioVm UsuarioObjeto;
        readonly ModuloVm _claim;

        public ClaimRequirementFilter(ModuloVm claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            try
            {
                string sesionBase64 = filterContext.HttpContext.Session.GetString("UsuarioObjeto");
                if (string.IsNullOrEmpty(sesionBase64))
                {
                    filterContext.Result = new RedirectResult("~/Usuario/Index?Codigo=1");
                    return;
                }
                var base64EncodedBytes = System.Convert.FromBase64String(sesionBase64);
                var sesion = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                UsuarioObjeto = JsonConvert.DeserializeObject<UsuarioVm>(sesion);
                if(UsuarioObjeto == null)
                {
                    filterContext.Result = new RedirectResult("~/Usuario/Index?Codigo=1");
                    return;
                }
                var encontro = false;
                foreach(var item in UsuarioObjeto.Menu)
                {
                    var modusloact = item.Modulos.FirstOrDefault(w => w.Nombre == _claim.Nombre);
                    encontro = modusloact != null;
                    if (encontro)
                    {
                        break;
                    }
                }
                if(!encontro && _claim.Nombre.ToLower() != "principal")
                {
                    filterContext.Result = new RedirectResult("~/Home/Index?Codigo=1");
                    return;
                }
            }catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Usuario/Index?Codigo=1");
            }
        }
    }
    
}
