using AyCWeb.Helper;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Dto;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AyCWeb.Controllers
{
    public class CapitalizacionController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarListaTrasladoActuales(int id_linea_ahorro, DateTime fecha)
        {
            string recuroyparametro = string.Format(
                   "Transacciones_Ahorro/ConsultarCapitalizacionAhorro?id_linea_ahorro={0}&fecha={1:yyyy-MM-dd}",
                   id_linea_ahorro,fecha);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Capitalizacion> ahorros = JsonConvert.DeserializeObject<List<Capitalizacion>>(contenido);
            ViewBag.Respuesta = new Mensaje_Respuesta(responseApi.Satisfactorio, ahorros.Count(), responseApi.Error);
            List<int> ids = new List<int>();
            foreach (var ahorro in ahorros)
                ids.Add(ahorro.Id_Ahorro);
            ViewBag.Ids = JsonSerializer.Serialize(ids);
            return PartialView(ahorros);
        }
        [HttpGet]
        public async Task<IActionResult> ConsultarCapitalizacionAhorroIndividual(int id_ahorro, DateTime fecha)
        {
            string recuroyparametro = string.Format(
                   "Transacciones_Ahorro/ConsultarCapitalizacionPorAhorro?id_ahorro={0}&fecha={1:yyyy-MM-dd}",
                   id_ahorro, fecha);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;

            List<Capitalizacion> ahorros = JsonConvert.DeserializeObject<List<Capitalizacion>>(contenido);
            return PartialView(ahorros); 
        }
        [HttpPost]
        public async Task<IActionResult> EjecutarTraslados([FromBody] List<CapitalizacionDto> datos)
        {
            string recurso = string.Format("Transacciones_Ahorro/EjecutarCapitalizacion");
            string jsonstring = JsonConvert.SerializeObject(datos).Replace("\"", "'"); 
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recurso, jsonstring);

            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            return BadRequest(responseApi.ContenidofromJson);
        }

        private async Task<Mensaje_Respuesta> EjecutarTrasladoIndividual(int id_ahorro)
        {
            string recurso = string.Format("Transacciones_Ahorro/EjecutarTrasladoIndividual?id_ahorro={0}", id_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recurso, null);
            return new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
        }
        public IActionResult CapitalizarDepositosPlazo()
        {
            return View();
        } 

        [HttpGet]
        public async Task<IActionResult> ConsultarListaCapitalizacion( DateTime fecha)
        {
            string recuroyparametro = string.Format(
                                                   "Transacciones_Ahorro/ConsultarCapitalizacionDepositoPlazo?fecha={0}",
                                                   fecha.ToString("yyyy-MM-dd")
                                               );
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Capitalizacion> ahorros = JsonConvert.DeserializeObject<List<Capitalizacion>>(contenido);
            ViewBag.Respuesta = new Mensaje_Respuesta(responseApi.Satisfactorio, ahorros.Count(), responseApi.Error);
            return PartialView(ahorros);
        }
        [HttpGet]
        public async Task<IActionResult> ReporteCapitalizacion(DateTime fecha)
        {
            string recurso = string.Format(
                "Transacciones_Ahorro/ConsultarCapitalizacionDepositoPlazo?fecha={0}",
                fecha.ToString("yyyy-MM-dd")
            );

            ResponseApi responseApi = await OperarApi(
                HttpMethod.Get,
                recurso,
                null
            );

            if (responseApi.Satisfactorio)
            {
                List<Capitalizacion> lista =
                    JsonConvert.DeserializeObject<List<Capitalizacion>>
                    (responseApi.ContenidofromJson);

                ViewBag.Fecha = fecha;

                return new ViewAsPdf("ReporteCapitalizacionPdf", lista)
                {
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                    PageMargins = new Margins
                    {
                        Top = 10,
                        Bottom = 10,
                        Left = 10,
                        Right = 10
                    },
                    FileName = $"Capitalizacion_{fecha:yyyyMMdd}.pdf"
                };
            }

            return BadRequest(responseApi.Error);
        }
        [HttpGet]
        public async Task<IActionResult> TicketCapitalizacion(
            int idDepositoPlazo,
            DateTime fecha)
        {
            string recurso =
                $"Transacciones_Ahorro/ConsultarTicketCapitalizacion?idDepositoPlazo={idDepositoPlazo}&fecha={fecha:yyyy-MM-dd}";

            ResponseApi responseApi = await OperarApi(
                HttpMethod.Get,
                recurso,
                null);

            if (!responseApi.Satisfactorio)
                return BadRequest(responseApi.Error);

            TicketCapitalizacionDto ticket =
                JsonConvert.DeserializeObject<TicketCapitalizacionDto>(
                    responseApi.ContenidofromJson);

            return new ViewAsPdf("TicketCapitalizacionPdf", ticket)
            {
                PageSize = Size.A5,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,

                PageMargins = new Margins
                {
                    Top = 10,
                    Bottom = 10,
                    Left = 10,
                    Right = 10
                },

                //FileName =
                //    $"Capitalizacion_{idDepositoPlazo}_{ticket.FechaCapitalizacion:yyyyMMdd}.pdf"
            };
        }
        public async Task<IActionResult> ConsultarDepositosPorVencer()
        {
            string recurso = "Transacciones_Ahorro/ConsultarDepositosPorVencer";

            ResponseApi respuesta = await OperarApi(HttpMethod.Get, recurso, null);

            List<DepositoPorVencer> lista =
                JsonConvert.DeserializeObject<List<DepositoPorVencer>>
                (respuesta.ContenidofromJson);

            return PartialView(lista);
        }
        [HttpPost]
        public async Task<IActionResult> RenovarDeposito(
        int idDepositoPlazo,
        int nuevoPlazo,
        DateTime fechaRenovacion)
        {
            string recurso =
                $"Transacciones_Ahorro/RenovarDepositoPlazo?idDepositoPlazo={idDepositoPlazo}&nuevoPlazo={nuevoPlazo}&fechaRenovacion={fechaRenovacion:yyyy-MM-dd}";

            ResponseApi responseApi = await OperarApi(
                HttpMethod.Post,
                recurso,
                null);

            if (!responseApi.Satisfactorio)
                return BadRequest(responseApi.Error);

            return Ok("Depósito renovado correctamente.");
        }

        [HttpPost]
        public async Task<IActionResult> CancelarDeposito(
        int idDepositoPlazo,
        DateTime fechaCancelacion)
        {
            string recurso =
                $"Transacciones_Ahorro/CancelarDepositoPlazo?idDepositoPlazo={idDepositoPlazo}&fechaCancelacion={fechaCancelacion:yyyy-MM-dd}";

            ResponseApi responseApi = await OperarApi(
                HttpMethod.Post,
                recurso,
                null);

            if (!responseApi.Satisfactorio)
                return BadRequest(responseApi.Error);

            return Ok("Depósito cancelado correctamente.");
        }
    }
}