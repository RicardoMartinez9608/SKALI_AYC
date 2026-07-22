using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class ComiteController : BaseController
    {
        // GET: Comite
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// esto es activado por el asesor que está cargando el crédito a la persona
        /// </summary>
        /// <param name="id_credito"></param>
        /// <returns></returns>
        public async Task<IActionResult> CargarPreComite(int id_credito)
        {
            /*crear las listas de precomite*/
            string recuroyparametro = string.Format("Comites/ComiteConformado?id_credito={0}", id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<string> ids_usuarios= JsonConvert.DeserializeObject<List<string>>(contenido);
            return Ok();
        }

        private async Task<List<Comite>> ComitesPorUsuarioPendientes(bool verificado=false)
        {
            string recuroyparametro = string.Format("Comites/ComitePorUsuarioPendiente?verificado={0}",verificado);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Comite>>(contenido);
        }

        public async Task<int> CantidadComitesPendientes()
        {
            return await Task.Run(()=> ComitesPorUsuarioPendientes(false).Result.Count);
        }
        
        public async Task<IActionResult> ComitesPendientes(bool verificado=false)
        {
            List<Comite> comites =await ComitesPorUsuarioPendientes(verificado);
            List<ComiteUsuarioCredito> comitesPendientes = new List<ComiteUsuarioCredito>();

            foreach (Comite comite in comites)
            {
                bool es_comite = true;
                /*obtener el credito*/
                int id_credito = comite.Id_Credito;
                string recursocredito = string.Format("Creditos/Detalle_Credito?id_credito={0}&es_comite={1}"
                    , id_credito,es_comite);
                ResponseApi respuestaCredito = await OperarApi(HttpMethod.Get, recursocredito, null);
                string contenidoCredito = respuestaCredito.ContenidofromJson;
                Credito credito = JsonConvert.DeserializeObject<Credito>(contenidoCredito);
                if (credito != null) {
                    /*obtener los datos del cliente*/
                    int id_persona = credito.Id_Persona;
                    string recursopersona = string.Format("Personas/PersonaDetalle?id_persona={0}", id_persona);
                    ResponseApi respuestaPersona = await OperarApi(HttpMethod.Get, recursopersona, null);
                    string contenidoPersona = respuestaPersona.ContenidofromJson;
                    Persona persona = JsonConvert.DeserializeObject<Persona>(contenidoPersona);

                    /*obtener los datos de la agencia*/
                    int id_cartera = credito.Id_Cartera;
                    string recursoagencia = string.Format("Account/ObtenerAgenciaPorCartera?id_cartera={0}", id_cartera);
                    ResponseApi respuestaAgencia = await OperarApi(HttpMethod.Get, recursoagencia, null);
                    string contenidoAgencia = respuestaAgencia.ContenidofromJson;
                    Agencia agencia = JsonConvert.DeserializeObject<Agencia>(contenidoAgencia);
                    comitesPendientes.Add(new ComiteUsuarioCredito
                    {
                        Comite = comite,
                        Credito = credito,
                        Cliente = persona,
                        Agencia = agencia
                    });
                }
            }
            return View(comitesPendientes);
        }
        public async Task<IActionResult> ComiteAmpliadoPendientes()
        {
            string  recuroyparametro = string.Format("Comites/ComiteAmpliadoAgencia");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<ComiteAmpliadoDetalle> ampliados = JsonConvert.DeserializeObject<List<ComiteAmpliadoDetalle>>(contenido);
            return View(ampliados);
        }
        public async Task<IActionResult> ComiteAmpliadoParaAprobar()
        {
            string recuroyparametro = string.Format("Comites/ComitesAmpliados");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<ComiteAmpliadoDetalle> ampliados = JsonConvert.DeserializeObject<List<ComiteAmpliadoDetalle>>(contenido);
            return View(ampliados);
        }
        [HttpGet]
        public async Task<IActionResult> Detalle_Comite_Ampliado(int IdComiteAmpliado)
        {
            string recuroyparametro = string.Format("Comites/FotografiasComite?IdComiteAmpliado={0}", IdComiteAmpliado);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Fotografia_Comite> fotografia = JsonConvert.DeserializeObject<List<Fotografia_Comite>>(contenido);
            return PartialView(fotografia);

        }
        [HttpGet]
        public async Task<IActionResult> Dev_Foto(int id_comite)
        {
            ViewBag.id_comite = id_comite;
            
            return PartialView();
        }
        public async Task<IActionResult> DetalleComitePendiente(int id_comite)
        {
            string recuroyparametro = string.Format("Comites/ComiteActivo?id_comite={0}", id_comite);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Comite comite = JsonConvert.DeserializeObject<Comite>(contenido);

            /*obtener el credito*/
            int id_credito = comite.Id_Credito;
            string recursocredito = string.Format("Creditos/Detalle_Credito?id_credito={0}", id_credito);
            ResponseApi respuestaCredito = await OperarApi(HttpMethod.Get, recursocredito, null);
            string contenidoCredito = respuestaCredito.ContenidofromJson;
            Credito credito=JsonConvert.DeserializeObject<Credito>(contenidoCredito);

            List<Comite> comites =  await ComitePorCredito(id_credito);
            
            /*obtener los datos del cliente*/
            int id_persona = credito.Id_Persona;
            string recursopersona = string.Format("Personas/PersonaDetalle?id_persona={0}", id_persona);
            ResponseApi respuestaPersona = await OperarApi(HttpMethod.Get, recursopersona, null);
            string contenidoPersona = respuestaPersona.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenidoPersona);

          
            DetalleComite detalle = new DetalleComite {
                ComiteActivo = comite,
                Comites=comites,
                Credito=credito,
                Cliente=persona
                
            };

            return View(detalle);
        }
        public async Task<IActionResult> DetalleModalComitePendiente(int id_comite)
        {
            string recuroyparametro = string.Format("Comites/ComiteActivo?id_comite={0}", id_comite);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Comite comite = JsonConvert.DeserializeObject<Comite>(contenido);

            /*obtener el credito*/
            int id_credito = comite.Id_Credito;
            string recursocredito = string.Format("Creditos/Detalle_Credito?id_credito={0}", id_credito);
            ResponseApi respuestaCredito = await OperarApi(HttpMethod.Get, recursocredito, null);
            string contenidoCredito = respuestaCredito.ContenidofromJson;
            Credito credito = JsonConvert.DeserializeObject<Credito>(contenidoCredito);

            List<Comite> comites = await ComitePorCredito(id_credito);

            /*obtener los datos del cliente*/
            int id_persona = credito.Id_Persona;
            string recursopersona = string.Format("Personas/PersonaDetalle?id_persona={0}", id_persona);
            ResponseApi respuestaPersona = await OperarApi(HttpMethod.Get, recursopersona, null);
            string contenidoPersona = respuestaPersona.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenidoPersona);


            DetalleComite detalle = new DetalleComite
            {
                ComiteActivo = comite,
                Comites = comites,
                Credito = credito,
                Cliente = persona

            };

            return PartialView(detalle);
        }
        public async Task<IActionResult> DetalleAptobacionComiteAmpliado(int id_credito)
        {
           
            string recursocredito = string.Format("Creditos/Detalle_Credito?id_credito={0}", id_credito);
            ResponseApi respuestaCredito = await OperarApi(HttpMethod.Get, recursocredito, null);
            string contenidoCredito = respuestaCredito.ContenidofromJson;
            Credito credito = JsonConvert.DeserializeObject<Credito>(contenidoCredito);
            var periodos = await CargarPeriodos();
            var lineas = await CargarLineas();
            var comportamientos = await Comportamientos_Credito();
            ViewBag.Id_Periodo = new SelectList(periodos, "Value", "Text", credito.Id_Periodo);
            ViewBag.Id_Linea = new SelectList(lineas, "Value", "Text", credito.Id_Linea);
            ViewBag.Id_Comportamiento_Credito =
                new SelectList(comportamientos, "Value", "Text", credito.Id_Comportamiento_Credito);



            DetalleComite detalle = new DetalleComite
            {
                Credito = credito
            };

            return PartialView(detalle);
        }
        private async Task<List<Comite>> ComitePorCredito(int id_credito)
        {
            string recursoComitesDelCredito = string.Format("Comites/ComitesPorCredito?id_credito={0}", id_credito);
            ResponseApi responseComitesDelCredito = await OperarApi(HttpMethod.Get, recursoComitesDelCredito, null);
            /*convertir el json por ahora*/
            string contenidoComiteDelCredito = responseComitesDelCredito.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Comite>>(contenidoComiteDelCredito);
        }

        public async Task<IActionResult> ComitePorCredito(DataSourceLoadOptions loadOptions, int id_credito)
        {
            List<Comite>comites= await ComitePorCredito(id_credito);
            var result = DataSourceLoader.Load(comites, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public async Task<IActionResult> AprobacionComite(int id_comite, bool aprobado, string comentario)
        {
            /*crear las listas de precomite*/
            string recuroyparametro = string.Format(
                "Comites/VerificadoComite?id_comite={0}&aprobado={1}&comentario={2}", 
                id_comite,aprobado,comentario);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            return Ok(); //RedirectToAction("DetalleComitePendiente",new { id_comite});
        }

        public async Task<IActionResult> CrearComiteAmpliado(int id_credito)
        {
            /*crear las listas de precomite*/
            string recuroyparametro = string.Format(
                "Comites/CrearComiteAmplido?id_credito={0}",
                id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AprobarAmpliado(int id_credito)
        {
            /*crear las listas de precomite*/
            string recuroyparametro = string.Format(
                "Comites/AprobarAmpliado?id_credito={0}",
                id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> RechazarAmpliado(int id_credito)
        {
            /*crear las listas de precomite*/
            string recuroyparametro = string.Format(
                "Comites/RechazarAmpliado?id_credito={0}",
                id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            return Ok();
        }
        public async Task<IActionResult> Subir_Foto_Comite_Ampliado(int id_comite)
        {

            var file = Request.Form.Files["Documento"];
            string ext = Path.GetExtension(file.FileName);

            if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
            {
                Fotografia_Comite doc = new Fotografia_Comite
                {
                    Id_Comite_Ampliado = id_comite,
                    
                    Foto = FileToBase64(file),
                    Descripcion = ""
                };
                string recuroyparametro = string.Format("Comites/CrearFotografiaComite");
                string jsonstring = JsonConvert.SerializeObject(doc).Replace("\"", "'");
                ResponseApi response = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
                return Ok();
            }
            else
                return BadRequest("el formato del archivo no es permitido");
        }
        public string FileToBase64(IFormFile archivo)
        {
            using (var memory = new MemoryStream())
            {
                archivo.CopyTo(memory);
                var filesBytes = memory.ToArray();
                return Convert.ToBase64String(filesBytes);
            }
        }
    }
}