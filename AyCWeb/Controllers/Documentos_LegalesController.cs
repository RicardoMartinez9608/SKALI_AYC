using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class Documentos_LegalesController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Documentos_Legales()
        {
            string recuroyparametro = string.Format("Documentos_Legales/GetDocumentosLegales");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Documentos_Legales_Vista> menuContenido = JsonConvert.DeserializeObject<List<Documentos_Legales_Vista>>(contenido);

            return PartialView(menuContenido);

        }
        [HttpGet]
        public async Task<IActionResult> Documentos_Legales_Ahorro()
        {
            string recuroyparametro = string.Format("Documentos_Legales/GetDocumentosLegalesAhorro");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Documentos_Legales_Vista> menuContenido = JsonConvert.DeserializeObject<List<Documentos_Legales_Vista>>(contenido);

            return PartialView(menuContenido);

        }

    }
}
