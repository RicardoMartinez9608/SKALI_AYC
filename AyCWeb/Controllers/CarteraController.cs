using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Helper;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class CarteraController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Rol = await Rol_Usuario();
            Agencia agencia= await Obtener_Agencia();
            ViewBag.IdAgencia = agencia.Id_Agencia;
            ViewBag.NombreAgencia = agencia.Nombre;

            return View();
        }

        public IActionResult Asignacion_Cartera()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AgregarCartera(string id_usuario, string nombre_cartera)
        {
            string recurso = string.Format("Carteras/Nueva_Cartera?id_usuario={0}&nombre_cartera={1}", id_usuario,nombre_cartera);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recurso, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        /*solo se podrá eliminar si está vacío*/
        public async Task<IActionResult> BorrarCartera(int id_cartera)
        {
            string recurso = string.Format("Carteras/BorrarCartera?id_cartera={0}", id_cartera);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recurso, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
            return PartialView(MensajeAsync, mensaje);
        }

        public async Task<IActionResult> CarterasPorAsesor(string id_usuario)
        {
            string recurso = string.Format("Carteras/CarterasPorUsuario?id_usuario={0}", id_usuario);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            List<Cartera> carteras = JsonConvert.DeserializeObject<List<Cartera>>(contenido);
            ViewBag.Carteras = new SelectList(carteras, "Id_Cartera", "Id_Cartera");
            return PartialView();
        }
        
        [HttpGet]
        public async Task<IActionResult> CarterasPorAgencia(int id_agencia)
        {
            string recurso = string.Format("Carteras/CarterasPorAgencia?id_agencia={0}", id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            List<Cartera> carteras = JsonConvert.DeserializeObject<List<Cartera>>(contenido);
            
            return PartialView(carteras);
        }

        public async Task<IActionResult> CambiarCartera(string[] values, string id_usuario)
        {
            for (int i=0;i<values.Length;i++) {
                int id_persona = Convert.ToInt32(values[i].ToString());
                await CambiarCarteraUno(id_usuario, id_persona);
            }
            return Ok();
        }

        private async Task CambiarCarteraUno(string id_usuario, int id_persona)
        {
            string recurso = string.Format("Carteras/CambiarCartera?id_usuario={0}&id_persona={1}", id_usuario, id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recurso, null);
            string contenido = responseApi.ContenidofromJson;
        }

        public async Task<IActionResult> CambiarUsuario(int id_cartera, string id_usuario)
        {
            string recurso = string.Format("Carteras/CambiarUsuario?id_cartera={0}&id_usuario={1}", id_cartera, id_usuario);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recurso, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
            return PartialView(MensajeAsync, mensaje);
        }

        public async Task<IActionResult> UsuariosPorAgencia(int id_agencia,string id_usuario)
        {
            string recurso = string.Format("Carteras/UsuariosPorAgencia?id_agencia={0}", id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            List<string> usuarios = JsonConvert.DeserializeObject<List<string>>(contenido);
            List<UsuarioSelect> selects = new List<UsuarioSelect>();
            foreach (string id in usuarios)
                selects.Add(new UsuarioSelect { Id_Usuario=id,Usuario=id});
            if (string.IsNullOrEmpty(id_usuario))
                id_usuario = "0";
            selects.Add(new UsuarioSelect { Id_Usuario = "0", Usuario = "Seleccione usuario" });/*es vacio*/
            ViewBag.Usurios = new SelectList(selects,"Id_Usuario","Usuario", id_usuario);
            return PartialView();
        }

        //crud de cartera
        [HttpGet]
        public IActionResult Form_Cartera(int id_agencia,bool es_nueva,string id_usuario,string nombre_cartera)
        {
            ViewBag.Id_Agencia = id_agencia;
            return PartialView();
        }

        //cartera por defecto
        [HttpGet]
        public async Task<IActionResult> Poner_Cartera_Defecto(int id_cartera)
        {
            string recurso = string.Format("Carteras/Poner_Cartera_Defecto?id_cartera={0}", id_cartera);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recurso, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> Editar_Cartera(Cartera cartera)
        {
            string recuroyparametro = string.Format("Carteras/EditarCartera");
            string jsonstring = JsonConvert.SerializeObject(cartera).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Contenido_Cartera(int idcartera)
        {
            string recuroyparametro = string.Format("Carteras/DetalleCartera?idcartera={0}", idcartera);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Cartera cartera = JsonConvert.DeserializeObject<Cartera>(contenido);
            return PartialView(cartera);
        }

  
        private async Task<string> Rol_Usuario()
        {
            string recurso = string.Format("Account/RolPorUsuario");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            string rol = JsonConvert.DeserializeObject<string>(contenido);
            return (rol);
        }
        private async Task<Agencia> Obtener_Agencia()
        {
            string recurso = string.Format("Account/ObtenerAgencia");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            Agencia agencia = JsonConvert.DeserializeObject<Agencia>(contenido);
            return (agencia);
        }
       
    }
}