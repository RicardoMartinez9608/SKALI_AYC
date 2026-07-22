using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class Cobro_PeriodicoController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Cobros_Periodicos(int id_linea_credito)
        {
            string recuroyparametro = string.Format("Cobro_Periodico/Cobros_Periodicos?id_linea_credito={0}", id_linea_credito);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Cobro_Periodico> cobros = JsonConvert.DeserializeObject<List<Cobro_Periodico>>(contenido);

            return PartialView(cobros);
        }
        
        [HttpPost]
        public async Task<IActionResult> Nuevo_Cobro_Periodico(Cobro_Periodico cobro)
        {
            string recuroyparametro = string.Format("Cobro_Periodico/Nuevo_Cobro_Periodico");
            
            string jsonstring = JsonConvert.SerializeObject(cobro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Cobro_Periodico(Cobro_Periodico cobro)
        {
            string recuroyparametro = string.Format("Cobro_Periodico/Editar_Cobro_Periodico");
            string jsonstring = JsonConvert.SerializeObject(cobro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }
        
        [HttpGet]
        public async Task<IActionResult> Detalle(bool esNuevo, int id_linea_credito=0, int id_cobro_periodico = 0)
        {
            if (esNuevo)
            {
                ViewBag.Id_Linea_Credito = id_linea_credito;
                Cobro_Periodico cobro_peridico = new Cobro_Periodico { Id_Linea_Credito=id_linea_credito};
                return PartialView(cobro_peridico);
            }
            string recuroyparametro = string.Format("Cobro_Periodico/Cobro_Periodico?id_cobro_periodico={0}", id_cobro_periodico);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Cobro_Periodico cobro = JsonConvert.DeserializeObject<Cobro_Periodico>(contenido);
            return PartialView(cobro);
        }
    }
}