using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AyCWeb.Models;
using System.Net.Http;
using Newtonsoft.Json;
using ModelNucleo.Model.ModelView;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using ModelNucleo.Model;
using System.Linq;
using RegisterDto = ModelNucleo.Model.ModelView.RegisterDto;

namespace AyCWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginView login)
        {
            bool auth = await LoginBack(login.Usuario, login.Password);
            if (auth)
                return RedirectToAction("Index");
            return RedirectToAction("Login");
        }

        public ActionResult RegistrarNuevoUsuario()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> RegistrarNuevoUsuario(RegisterDto registro )
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(registro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Account/Register", jsonstring);

            return Ok();
        }

        /*para el cambio de contrasena*/
        public IActionResult CambiarContrasena(string id_usuario)
        {
            ViewBag.Id_Usuario = id_usuario;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(CambiarContrasenaAdmin cambiar)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(cambiar).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, "Account/ChangePassword", jsonstring);
            if (responseApi.Satisfactorio)
            {
                return Ok(responseApi.ContenidofromJson);
                //return RedirectToAction("RegistrarNuevoUsuario");
            }
            else
            {
                return BadRequest(responseApi.ContenidofromJson);
            }
        }

        public async Task<ActionResult> ListaUsuarios(string busqueda)
        {
            string datos = string.Format("Account/ListaUsuarios?busqueda={0}", busqueda);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, datos, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(contenido);
            return PartialView(usuarios);
        }

        [HttpGet]
        public async Task<ActionResult> AgregarRoles(string usuario)
        {
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, "Account/Roles", null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(contenido);

            ViewBag.Rol = new SelectList(roles, "RoleNombre", "RoleNombre");
            ViewBag.Usuario = usuario;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AgregarRol(string usuario,string rol)
        {
            string recuroyparametro = string.Format("Account/AddRole?usuario={0}&rol={1}", usuario,rol);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
            {
                return Ok(responseApi.ContenidofromJson);
            }
            else
            {
                return BadRequest(responseApi.ContenidofromJson);
            }
  
        }

        [HttpGet]
        public async Task<ActionResult> RolesAsignados(string usuario)
        {
            string recuroyparametro = string.Format("Account/RolesAsignados?usuario={0}", usuario);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(contenido);

            return PartialView(roles);
        }

        [HttpPost]
        public async Task<ActionResult> BorrarRolAsignado(string usuario,string rol)
        {
            string recuroyparametro = string.Format("Account/BorrarRolAsignado?usuario={0}&rol={1}", usuario, rol);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }


        public async Task<ActionResult> Index()
        {
            //var periodos = await CargarPeriodos();
            //var lineas = await CargarLineas();
            //ViewBag.Id_Periodo = new SelectList(periodos, "Value", "Text");
            //ViewBag.Id_Linea = new SelectList(lineas, "Value", "Text");
            return View();
        }

        public async Task<IActionResult> Calculo_Credito()
        {
            var periodos = await CargarPeriodos();
            var lineas = await CargarLineas();
            ViewBag.Id_Periodo = new SelectList(periodos, "Value", "Text");
            ViewBag.Id_Linea = new SelectList(lineas, "Value", "Text");
            return View();
        }
        
        public async Task<ActionResult> LogOut()
        {
            string recuroyparametro = string.Format("Account/Logout");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);

            LogoutBack();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<ActionResult> ManejoUsuario()
        {
            /*obtain solo el nombre del usuario*/
             string usuario= HttpContext.Session.GetString("Usuario");
            string recuroyparametro = string.Format(
                "Account/Obtener_Nombre_Real?usuario={0}",
                usuario);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                usuario =JsonConvert.DeserializeObject<string>(responseApi.ContenidofromJson);
            ViewBag.Usuario = usuario;
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> AsignarAgencia(
            string usuario,
            int id_agencia)
        {
            string recuroyparametro = string.Format(
                "Account/AsignarAgencia?usuario={0}&id_agencia={1}", 
                usuario, 
                id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Nombre_Real_Usuario(
            string usuario,
            string nombre_completo)
        {
            string recuroyparametro = string.Format(
                "Account/Nombre_Real_Usuario?usuario={0}&nombre_completo={1}",
                usuario,
                nombre_completo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }


        [HttpGet]
        public async Task<string> AgenciaAsignada(string usuario)
        {
            string recuroyparametro = string.Format("Account/AgenciaAsignadaUsuario?usuario={0}", usuario);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            return responseApi.ContenidofromJson;
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> VerificarSession()
        {
            string recuroyparametro = string.Format("Account/Verificar");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if(responseApi.Status== "Unauthorized")
                return Ok(0);
            //if (HttpContext.Session.Keys.Count()>0)
            //    return Ok((int)HttpContext.Session.GetInt32("esautenticado"));
            return Ok((int)HttpContext.Session.GetInt32("esautenticado"));
        }

        [HttpGet]
        public async Task<IActionResult> VerificarNombreEmpresa()
        {
            //string recuroyparametro = string.Format("Account/Nombre_Empresa");
            //ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            //if (responseApi.Satisfactorio)
            //    return Ok(responseApi.ContenidofromJson);
            //return Ok("");

           string valor=  HttpContext.Session.GetString("empresa");
            return Ok(valor);
        }
        [HttpGet]
        public async Task<IActionResult> LogoEmpresa()
        {
            string recuroyparametro = string.Format("Account/LogoGrande_Empresa");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            return Ok("");
        }

        [HttpGet]
        public async Task<IActionResult> LogoLayout()
        {
            string valor = HttpContext.Session.GetString("LEmpresa");
            return Ok(valor);
        }

        [HttpGet]
        public async Task<IActionResult> Menu()
        {
            string recuroyparametro = string.Format("Account/Menu_Rol");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List <Menu_Categoria_vista> menu_filtrado= JsonConvert.DeserializeObject<List<Menu_Categoria_vista>>(contenido);
            return PartialView(menu_filtrado);
        }
    }
}
