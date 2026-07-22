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
    public class Linea_AhorroController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> TipoLineaAhorro(DataSourceLoadOptions loadOptions)
        {
            string recuroyparametro = string.Format("Linea_Ahorros/TipoLineaAhorro");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Tipo_Linea_Ahorro> tipos_lineas_ahorro = JsonConvert.DeserializeObject<List<Tipo_Linea_Ahorro>>(contenido);

            var result = DataSourceLoader.Load(tipos_lineas_ahorro, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public async Task<IActionResult> Lineas_Ahorro(DataSourceLoadOptions loadOptions)
        {
            string recuroyparametro = string.Format("Linea_Ahorros/LineasAhorro");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Linea_Ahorro> lienasahorro = JsonConvert.DeserializeObject<List<Linea_Ahorro>>(contenido);

            var result = DataSourceLoader.Load(lienasahorro, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> Create(string values)
        {
            /*hacer casting a su objeto por usar devexpress*/
            Linea_Ahorro lineaahorro = JsonConvert.DeserializeObject<Linea_Ahorro>(values);
            lineaahorro.Id_Linea_Ahorro = 0;
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(lineaahorro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Linea_Ahorros/NuevaLineaAhorro", jsonstring);
            return Ok();
        }

        public async Task<IActionResult> Edit(int key, string values)
        {
            /*hacer casting a su objeto por usar devexpress*/
            Linea_Ahorro lineaahorro = await Detalle(key);
            JsonConvert.PopulateObject(values, lineaahorro);
            /*conectarse al api*/
            string recuroyparametro = string.Format("Linea_Ahorros/Modificar?id_linea_ahorro={0}", lineaahorro.Id_Linea_Ahorro);
            string jsonstring = JsonConvert.SerializeObject(lineaahorro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            return Ok();
        }

        [HttpGet]
        public async Task<Linea_Ahorro> LineaDetalle(int id_linea_ahorro)
        {
            return await Detalle(id_linea_ahorro);
        }
        
        private async Task<Linea_Ahorro> Detalle(int id_linea_ahorro)
        {
            string recuroyparametro = string.Format("Linea_Ahorros/LineaAhorroDetalle?id_linea_ahorro={0}", id_linea_ahorro);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<Linea_Ahorro>(contenido);
        }

    }
}