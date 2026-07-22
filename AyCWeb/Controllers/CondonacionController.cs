using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AyCWeb.Controllers
{
    public class CondonacionController : BaseController
    {
        public async Task<IActionResult> Detalle_Condonacion(int id_credito)
        {
            string recuroyparametro = string.Format("Condonacion/Detalle_Condonacion?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Condonacion condonacion= JsonConvert.DeserializeObject<Condonacion>(contenido);
            return View(condonacion);
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarCondonacion(string ids, decimal recargo)
        {
            string recuroyparametro = string.Format("Condonacion/ProcesarCondonacion?ids={0}&recargo={1}", ids, recargo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if(responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
    }
}
