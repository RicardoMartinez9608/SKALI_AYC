using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Helper;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class BancoController : BaseController
    {
        public async Task<IActionResult> Index(string mensaje=null)
        {
            if (!string.IsNullOrEmpty(mensaje))
            {
                ViewBag.ConRespuesta = true;
                ViewBag.Respuesta = JsonConvert.DeserializeObject<Mensaje_Respuesta>(mensaje);
            }
            else
            {
                ViewBag.ConRespuesta = false;
                ViewBag.Respuesta = null;
            }
            return View(await JBanco());
        }

        public async Task<IActionResult> Bancos_Combo()
        {
            return PartialView(await JBanco());
        }

        private async Task<List<Banco>> JBanco()
        {
            string recuroyparametro = string.Format("Banco/Bancos");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Banco>>(contenido);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Banco banco)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(banco).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Banco/Crear_Banco", jsonstring);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Index",new { mensaje= msj_str });
        }

        public async Task<IActionResult> Editar(int id_banco)
        {
            string recuroyparametro = string.Format("Banco/Detalle_Banco?id_banco={0}", id_banco);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Banco banco = JsonConvert.DeserializeObject<Banco>(contenido);
            return View(banco);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Banco banco)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(banco).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, "Caja/Editar_Caja", jsonstring);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Index", new { mensaje = msj_str });
        }

        public async Task<IActionResult> Eliminar(int id_banco)
        {
            string recuroyparametro = string.Format("Banco/Eliminar_Banco?id_banco={0}", id_banco);
            ResponseApi resultado = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Index", new { mensaje = msj_str });
        }
    }
}