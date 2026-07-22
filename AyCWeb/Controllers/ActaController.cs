using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class ActaController : BaseController
    {
        public async Task<IActionResult> Index(FechasFiltro filtro)
        {
            DateTime ahora = DateTime.Now;
            string inicio = ahora.ToString("MM/dd/yyyy");
            string final = ahora.ToString("MM/dd/yyyy");
            if (filtro.inicio != null && filtro.final != null)
            {
                inicio = filtro.inicio.Value.ToString("MM/dd/yyyy");
                final = filtro.final.Value.ToString("MM/dd/yyyy");
            }
            string recuroyparametro = string.Format("Actas/Actas?inicio={0}&final={1}", inicio, final);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Acta> actas = JsonConvert.DeserializeObject<List<Acta>>(contenido);
            return View(actas);
        }

        public async Task<IActionResult> Detalle_Acta(int id_acta)
        {
            string recuroyparametro = string.Format("Actas/Detalle_Acta?id_acta={0}", id_acta);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Acta acta = JsonConvert.DeserializeObject<Acta>(contenido);
            return View(acta);
        }
    }
}