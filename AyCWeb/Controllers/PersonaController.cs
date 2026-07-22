using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Helper;
using AyCWeb.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using Orientation = Rotativa.AspNetCore.Options.Orientation;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AyCWeb.Controllers
{
    public class PersonaController : BaseController
    {
        /*nuevos methodos para manejar personas*/
        [HttpGet]
        public async Task<IActionResult> Manejo_Persona(bool esNuevo, int id_persona = 0)
        {
            var estados = from Estado_Familiar d in Enum.GetValues(typeof(Estado_Familiar))
                          select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };
            var sexos = from Sexo d in Enum.GetValues(typeof(Sexo))
                        select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };
            var residencias = from Residencia d in Enum.GetValues(typeof(Residencia))
                              select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };

            var carteras = await CarteraPorAgencia();
            var departamentos = await Departamentos();
            var municipios = await Municipios(0);

            ViewBag.EsNuevo = esNuevo;

            if (esNuevo)
            {
                ViewBag.EstadoFamiliar = new SelectList(estados, "Value", "Text");
                ViewBag.Sexo = new SelectList(sexos, "Value", "Text");
                ViewBag.Residencia = new SelectList(residencias, "Value", "Text");
                ViewBag.Id_Cartera = new SelectList(carteras, "Value", "Text");

                ViewBag.Id_Departamento_Pais_Domicilio = new SelectList(departamentos, "Value", "Text");
                ViewBag.Id_Departamento_Pais_Direccion_Negocio = new SelectList(departamentos, "Value", "Text");
                ViewBag.Id_Departamento_Pais_Direccion_Trabajo = new SelectList(departamentos, "Value", "Text");
                ViewBag.Id_Departamento_Pais_Nacimiento = new SelectList(departamentos, "Value", "Text");

                ViewBag.Id_Municipio_Departamento_Domicilio = new SelectList(municipios, "Value", "Text");
                ViewBag.Id_Municipio_Departamento_Direccion_Negocio = new SelectList(municipios, "Value", "Text");
                ViewBag.Id_Municipio_Departamento_Direccion_Trabajo = new SelectList(municipios, "Value", "Text");
                ViewBag.Id_Municipio_Departamento_Nacimiento = new SelectList(municipios, "Value", "Text");
                return PartialView();
            }
            Persona persona = await PersonaPorId(id_persona);
            ViewBag.EstadoFamiliar = new SelectList(estados, "Value", "Text", (int)persona.EstadoFamiliar);
            ViewBag.Sexo = new SelectList(sexos, "Value", "Text", (int)persona.Sexo);
            ViewBag.Residencia = new SelectList(residencias, "Value", "Text", (int)persona.Residencia);

            List<SelectListItem> carteras_vacia = new List<SelectListItem>
            {
                new SelectListItem { Value = persona.Cartera.Id_Cartera.ToString(), Text = persona.Cartera.Nombre }
            };

            ViewBag.Id_Cartera = new SelectList(carteras, "Value", "Text", persona.Id_Cartera);
            //ViewBag.Id_Cartera = new SelectList(carteras, "Value", "Text");
            ViewBag.Id_Departamento_Pais_Domicilio = new SelectList(departamentos, "Value", "Text", persona.Id_Departamento_Pais_Domicilio);
            ViewBag.Id_Departamento_Pais_Direccion_Negocio = new SelectList(departamentos, "Value", "Text", persona.Id_Departamento_Pais_Direccion_Negocio);
            ViewBag.Id_Departamento_Pais_Direccion_Trabajo = new SelectList(departamentos, "Value", "Text", persona.Id_Departamento_Pais_Direccion_Trabajo);
            ViewBag.Id_Departamento_Pais_Nacimiento = new SelectList(departamentos, "Value", "Text", persona.Id_Departamento_Pais_Nacimiento);

            var municipios_d = await Municipios(persona.Id_Departamento_Pais_Domicilio != null ? (int)persona.Id_Departamento_Pais_Domicilio : 0);
            var municipios_dn = await Municipios(persona.Id_Departamento_Pais_Direccion_Negocio != null ? (int)persona.Id_Departamento_Pais_Direccion_Negocio : 0);
            var municipios_dt = await Municipios(persona.Id_Departamento_Pais_Direccion_Trabajo != null ? (int)persona.Id_Departamento_Pais_Direccion_Trabajo : 0);
            var municipios_n = await Municipios(persona.Id_Departamento_Pais_Direccion_Negocio != null ? (int)persona.Id_Departamento_Pais_Nacimiento : 0);

            ViewBag.Id_Municipio_Departamento_Domicilio = new SelectList(municipios_d, "Value", "Text", persona.Id_Municipio_Departamento_Domicilio);
            ViewBag.Id_Municipio_Departamento_Direccion_Negocio = new SelectList(municipios_dn, "Value", "Text", persona.Id_Municipio_Departamento_Direccion_Negocio);
            ViewBag.Id_Municipio_Departamento_Direccion_Trabajo = new SelectList(municipios_dt, "Value", "Text", persona.Id_Municipio_Departamento_Direccion_Trabajo);
            ViewBag.Id_Municipio_Departamento_Nacimiento = new SelectList(municipios_n, "Value", "Text", persona.Id_Municipio_Departamento_Nacimiento);
            return PartialView(persona);
        }

        [HttpGet]
        public async Task<IActionResult> Municipios_Opciones(int id_departamento)
        {
            List<Municipio_Departamento> municipios = await Municipios_Comun(id_departamento);
            return PartialView(municipios);
        }

        private async Task<IEnumerable<SelectListItem>> CarteraPorAgencia()
        {
            string recuroyparametro = "Carteras/CarterasPorAgenciaEnUsuario";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Cartera> carteras = JsonConvert.DeserializeObject<List<Cartera>>(contenido);
            return carteras.Select(s => new SelectListItem { Value = s.Id_Cartera.ToString(), Text = s.Nombre });
        }

        private async Task<IEnumerable<SelectListItem>> Departamentos()
        {
            string recuroyparametro = "Personas/Departamentos";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Departamento_Pais> departamentos = JsonConvert.DeserializeObject<List<Departamento_Pais>>(contenido);
            return departamentos.Select(s => new SelectListItem { Value = s.Id_Departamento_Pais.ToString(), Text = s.Nombre_Departamento });
        }

        private async Task<IEnumerable<SelectListItem>> Municipios(int id_departamento)
        {
            List<Municipio_Departamento> municipios = await Municipios_Comun(id_departamento);
            return municipios.Select(s => new SelectListItem { Value = s.Id_Municipio_Departamento.ToString(), Text = s.Nombre_Municipio });
        }

        private async Task<List<Municipio_Departamento>> Municipios_Comun(int id_departamento)
        {
            string recuroyparametro = string.Format("Personas/Municipios?id_departamento={0}", id_departamento);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Municipio_Departamento> municipios = JsonConvert.DeserializeObject<List<Municipio_Departamento>>(contenido);
            return municipios;
        }


        private async Task<Persona> PersonaPorId(int id_persona)
        {
            string recuroyparametro = string.Format("Personas/PersonaDetalle?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<Persona>(contenido);
        }


        // GET: /<controller>/
        [HttpGet]
        public ActionResult Index(bool nuevo_socio=false,string dui = null)
        {
            ViewBag.Nuevo_Socio = nuevo_socio;
            ViewBag.DUI = dui;
            return View();
        }

        [HttpGet]
        public ActionResult MasDetalle()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Grid_Personas(string busqueda)
        {
            List<Persona> persona = await PersonasFuente(busqueda);
            return PartialView(persona);
        }

        [HttpGet]
        public async Task<IActionResult> Personas(DataSourceLoadOptions loadOptions)
        {
            string busqueda = "035";
            if (loadOptions.Filter != null)
                busqueda = FilterBusquedaDX(loadOptions.Filter[0].ToString());

            List<Persona> personas = await PersonasFuente(busqueda);
            try
            {
                DataSourceLoadOptions optionesLimpias = new DataSourceLoadOptions();
                var result = DataSourceLoader.Load(personas, optionesLimpias);
                var resultJson = JsonConvert.SerializeObject(result);
                return Content(resultJson, "application/json");
            }
            catch (Exception ex) {
                return Content(ex.StackTrace);
            }
        }

        private async Task<List<Persona>> PersonasFuente(string busqueda)
        {
            string recuroyparametro = string.Format("Personas/BuscarPersonas?busqueda={0}", busqueda);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Persona>>(contenido);
        }

        [HttpGet]
        public async Task<ActionResult> DetallePersona(string filtro, bool esnuevo)
        {
            if (esnuevo)
                return PartialView();
            Persona persona = await Persona(filtro);
            return PartialView(persona);
        }

        private async Task<Persona> Persona(string filtro)
        {
            string filtroParaDetalle = string.Format("Personas/PersonaEspecifica?filtro={0}", filtro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, filtroParaDetalle, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<Persona>(contenido);
        }

        [HttpGet]
        public async Task<IActionResult> JDetalle_Persona(string filtro)
        {
            Persona persona = await Persona(filtro);
            return Ok(persona);
        }

        [HttpPost]
        public async Task<ActionResult> CrearPersona(Persona persona)
        {
            persona.Id_Persona = 0;
            string jsonstring = JsonConvert.SerializeObject(persona).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Personas/CrearPersona?tipo_persona=1", jsonstring);//tipo de persona ya esta por defecto cliente
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPersona(Persona persona)
        {
            /*conectarse al api*/
            string url = string.Format("Personas/EditarPersona");
            string jsonstring = JsonConvert.SerializeObject(persona).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, url, jsonstring);
            /*Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            return PartialView(MensajeAsync, mensaje);*/
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }
       
        public async Task<IActionResult> Generar_Usuario_Socio(int id_persona)
        {
            string recuroyparametro = string.Format(
                "Account/Crear_Usuario_Socio?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Registro_Socio socio = JsonConvert.DeserializeObject<Registro_Socio>(contenido);
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            return new ViewAsPdf("Usuario_Socio", socio, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };
        }
        [HttpGet]
        public async Task<IActionResult> Tipos_Persona()
        {
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, "Tipo_Persona/Tipos_Personas", null);
            string contenido = responseApi.ContenidofromJson;
            List<Tipo_Persona> tipos = JsonConvert.DeserializeObject<List<Tipo_Persona>>(contenido);
            ViewBag.Tipo_persona = new SelectList(tipos, "Id_Tipo_Persona", "Tipo_Persona_definicion");
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> TiposPersonaPorPersona(int id_persona)
        {
            string filtroParaDetalle = string.Format("Persona_Tipo_Persona/Personas_Tipo_Persona?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, filtroParaDetalle, null);
            string contenido = responseApi.ContenidofromJson;
            List<Tipo_Persona> tippersonasos = JsonConvert.DeserializeObject<List<Tipo_Persona>>(contenido);
            return PartialView(tippersonasos);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoPersonaPorPersona(int id_persona, int id_tipo_persona)
        {
            /*conectarse al api*/
            Persona_Tipo_Persona tipoPorPersona = new Persona_Tipo_Persona {
                Id_Persona = id_persona,
                Id_Tipo_Persona = id_tipo_persona
            };
            string jsonstring = JsonConvert.SerializeObject(tipoPorPersona).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Persona_Tipo_Persona/CrearPersona_Tipo_Persona", jsonstring);
            return Ok();
        }

        /*see seguira desarrollando cuando se vea la parte de creditos*/
        private async Task<List<SelectListItem>> TiposRelacion(int id_seleccionado = 0)
        {
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, "Tipo_Relacion/Tipos_Relaciones", null);
            string contenido = responseApi.ContenidofromJson;
            List<Tipo_Relacion> tipos = JsonConvert.DeserializeObject<List<Tipo_Relacion>>(contenido);

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Tipo_Relacion tipo in tipos)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = tipo.Tipo_Relacion_Definicion,
                    Value = tipo.Id_Tipo_Relacion.ToString(),
                    Selected = id_seleccionado == tipo.Id_Tipo_Relacion ? true : false
                };
                items.Add(item);
            }
            return items;
        }

        /*todo para manejar las referencias de la persona.*/
        /*para los tipos de referencias*/
        [HttpGet]
        public async Task<IActionResult> DetalleReferencia(
                int id_referencia,
                int id_persona,
                bool esNuevo,
                int tipo,
                bool beneficiario = false)
        {
            var parentescos = from Parentesco d in Enum.GetValues(typeof(Parentesco))
                              select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };
            var tipos_referencia = from Tipo_Refencia_Option d in Enum.GetValues(typeof(Tipo_Refencia_Option))
                                   select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };

            if (esNuevo)
            {
                ViewBag.Beneficiario = beneficiario;
                ViewBag.Parentesco = new SelectList(parentescos, "Value", "Text");
                ViewBag.Tipo_Referencia = tipo; //new SelectList(tipos_referencia, "Value", "Text",tipo);
                return PartialView();
            }

            string filtroParaDetalle = string.Format("Referencias/Referencia?id_referencia={0}&id_persona={1}", id_referencia, id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, filtroParaDetalle, null);
            string contenido = responseApi.ContenidofromJson;
            ReferenciaVista referencia = JsonConvert.DeserializeObject<ReferenciaVista>(contenido);
            ViewBag.Parentesco = new SelectList(parentescos, "Value", "Text", referencia.Parentesco);
            ViewBag.Tipo_Referencia = referencia.Tipo_Referencia; //new SelectList(tipos_referencia, "Value", "Text",referencia.Tipo_Referencia);
            ViewBag.Beneficiario = referencia.Beneficiario;
            return PartialView(referencia);
        }

        [HttpPost]
        public async Task<IActionResult> CrearReferencia(ReferenciaVista referencia, int id_persona)
        {
            string jsonstring = JsonConvert.SerializeObject(referencia).Replace("\"", "'");
            string paraCrear = string.Format("Referencias/CrearReferencia?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> EditarReferencia(ReferenciaVista referencia, int id_persona)
        {
            string jsonstring = JsonConvert.SerializeObject(referencia).Replace("\"", "'");
            string paraeditar = string.Format("Referencias/EditarReferencia?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, paraeditar, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarReferencia(int idreferencia)
        {
            string recuroyparametro = string.Format("Referencias/BorrarReferencia?idreferencia={0}", idreferencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Referencias(int id_persona, int tipo, bool beneficiario = false)
        {
            try
            {
                string recuroyparametro = string.Format("Persona_Referencia/Personas_Referencias_Por_Persona?id_persona={0}&tipo_referencia={1}&beneficiario={2}", 
                    id_persona,
                    tipo,
                    beneficiario);
                ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
                /*convertir el json por ahora*/
                string contenido = responseApi.ContenidofromJson;
                List<ReferenciaVista> referenciavista = JsonConvert.DeserializeObject<List<ReferenciaVista>>(contenido);
                ViewBag.Tipo = tipo;
                referenciavista = referenciavista.Where(f => f.Tipo_Referencia == tipo.ToString()).ToList();
                /*if (beneficiario == false && tipo == 1)
                    referenciavista = referenciavista.Where(f => f.Tipo_Referencia == tipo.ToString()).ToList();*/
                if (beneficiario)
                    referenciavista = referenciavista.Where(f => f.Tipo_Referencia == tipo.ToString() && f.Beneficiario == beneficiario).ToList();

                return PartialView(referenciavista);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> PersonasPorUsuario(string id_usuario)
        {
            string recuroyparametro = string.Format("Personas/PersonasPorUsuario?id_usuario={0}", id_usuario);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Credito> personas = JsonConvert.DeserializeObject<List<Credito>>(contenido);
            return PartialView(personas);
        }
        
        /*Manejo de las relaciones*/
        [HttpGet]
        public async Task<IActionResult> Relaciones(int id_persona, string nombre_completo)
        {
            List<Persona_Relaciones> relaciones = await Relaciones_Principal(id_persona);
            ViewBag.Id_Persona = id_persona;
            ViewBag.Nombre_Completo = nombre_completo;
            return View(relaciones);
        }

        [HttpGet]
        public async Task<IActionResult> Codeudores(int id_persona,int id_credito)
        {
            List<Persona_Relaciones> relaciones =await Relaciones_Principal(id_persona,id_credito);
            return PartialView(relaciones.Where(d=>d.Id_Tipo_Relacion==5));
        }

        [HttpGet]
        public IActionResult Nuevo_Codeudor()
        {
            return PartialView();
        }

        private async Task<List<Persona_Relaciones>> Relaciones_Principal(int id_persona,int id_credito=0)
        {
            string recuroyparametro = string.Format("Persona_Relaciones/Relaciones?id_persona={0}&id_credito={1}", id_persona,id_credito);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Persona_Relaciones>>(contenido);
        }

        [HttpGet]
        public async Task<IActionResult> Nueva_Relacion(int id_persona)
        {
            ViewBag.Id_Persona = id_persona;
            ViewBag.Id_Tipo_Relacion = await TiposRelacion();
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Nueva_Relacion(Persona_Relaciones relaciones)
        {
            string jsonstring = JsonConvert.SerializeObject(relaciones).Replace("\"", "'");
            string paraeditar = string.Format("Persona_Relaciones/Crear_Relacion");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraeditar, jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> Activar_Codeudor(int id_relacion)
        {
            string paraeditar = string.Format("Persona_Relaciones/Activar_Relacion?id_relacion={0}", id_relacion);
            ResponseApi resultado = await OperarApi(HttpMethod.Put, paraeditar, null);
            if (resultado.Satisfactorio)
                return Ok("El codeudor se ha activado");
            else
                return BadRequest("El codeudor no pudo ser activado");
        }

        public IActionResult Editar_Relacion(int id_persona)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Relacion(Persona_Relaciones relaciones, int id_persona)
        {
            string jsonstring = JsonConvert.SerializeObject(relaciones).Replace("\"", "'");
            string paraeditar = string.Format("Persona_Relaciones/Editar_Relacion");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, paraeditar, jsonstring);
            return RedirectToAction("Relaciones", new { id_persona = id_persona });
        }
        /*Termina el manejo de la relación*/

        /*manejo de referencias bancarias*/
        [HttpGet]
        public async Task<IActionResult> Referencias_Bancarias(int id_persona)
        {
            string rquesting = string.Format("Referencias/Referencias_Bancarias?id_persona={0}", id_persona);
            ResponseApi resultado = await OperarApi(HttpMethod.Get, rquesting, null);
            string contenido = resultado.ContenidofromJson;
            List<Referencia_Bancaria> referencias = JsonConvert.DeserializeObject<List<Referencia_Bancaria>>(contenido);
            return PartialView(referencias);
        }

        [HttpGet]
        public IActionResult Nueva_Referencia_Bancaria(int id_persona)
        {
            ViewBag.Id_Persona = id_persona;
            var tipo_referencia_bancaria = from Tipo_Referencia_Num d in Enum.GetValues(typeof(Tipo_Referencia_Num))
                                           select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };
            ViewBag.Tipo_Referencia = new SelectList(tipo_referencia_bancaria, "Value", "Text");
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Detalle_Referencia_Bancaria(int id_referencia_bancaria)
        {
            string rquesting = string.Format("Referencias/Detalle_Referencia_Bancaria?id_referencia_bancaria={0}", id_referencia_bancaria);
            ResponseApi resultado = await OperarApi(HttpMethod.Get, rquesting, null);
            string contenido = resultado.ContenidofromJson;
            Referencia_Bancaria referencia = JsonConvert.DeserializeObject<Referencia_Bancaria>(contenido);

            var tipo_referencia_bancaria = from Tipo_Referencia_Num d in Enum.GetValues(typeof(Tipo_Referencia_Num))
                                           select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };

            ViewBag.Tipo_Referencia = new SelectList(tipo_referencia_bancaria, "Value", "Text", (int)referencia.Tipo_Referencia);
            return PartialView(referencia);
        }

        [HttpPost]
        public async Task<IActionResult> Nueva_Referencia_Bancaria(Referencia_Bancaria referencia)
        {
            string jsonstring = JsonConvert.SerializeObject(referencia).Replace("\"", "'");
            string rquesting = string.Format("Referencias/Nueva_Referencia_Bancaria");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, rquesting, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Referencia_Bancaria(Referencia_Bancaria referencia)
        {
            string recuroyparametro = string.Format("Referencias/Editar_Referencia_Bancaria");
            string jsonstring = JsonConvert.SerializeObject(referencia).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
           
        }

        [HttpGet]
        public async Task<IActionResult> Persona_Existe(string dui)
        {
            string rquesting = string.Format("Personas/Persona_Existe?dui={0}", dui);
            ResponseApi resultado = await OperarApi(HttpMethod.Get, rquesting, null);
            return Ok(resultado.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Generar_Solicitud_Socio(int id_persona)
        {
            Ubicacion_Politica_Etiqueta_Persona persona =await Persona_Completa(id_persona);
            Generar_Solicitud documento = new Generar_Solicitud();
            MemoryStream stream = documento.Generar_Solicitud_Socio(persona);
            return new FileStreamResult(stream, "application/msword")
            { FileDownloadName = "solicitud_socio.docx" };
        }
        

        [HttpGet]
        public async Task<IActionResult> Generar_Solicitud_Admision(int id_persona)
        {
            Ubicacion_Politica_Etiqueta_Persona persona = await Persona_Completa(id_persona);

            //cambiar el origen del documento
            Generar_Solicitud_Admision documento = new Generar_Solicitud_Admision();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Solicitud_Socio(persona);
            return new FileStreamResult(stream, "application/msword")
            { FileDownloadName = "solicitud_socio_admision.docx" };
        }

        private async Task<Ubicacion_Politica_Etiqueta_Persona> Persona_Completa(int id_persona)
        {
            string recuroyparametro = string.Format("Personas/Persona_Raiz?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            var departamentos = await Departamentos();
            
            string departamento_nacimeinto =persona.Id_Departamento_Pais_Nacimiento!=null || persona.Id_Departamento_Pais_Nacimiento>0 ? departamentos.SingleOrDefault(d=>d.Value==persona.Id_Departamento_Pais_Nacimiento.ToString()).Text:"--";
            string municipio_nacimiento= persona.Id_Municipio_Departamento_Nacimiento!=null || persona.Id_Municipio_Departamento_Nacimiento>0 ? (await Municipios((int)persona.Id_Departamento_Pais_Nacimiento)).SingleOrDefault(d => d.Value == persona.Id_Municipio_Departamento_Nacimiento.ToString()).Text:"--";

            string departamento_domicilio = persona.Id_Departamento_Pais_Domicilio!=null || persona.Id_Departamento_Pais_Domicilio>0 ? departamentos.SingleOrDefault(d => d.Value == persona.Id_Departamento_Pais_Domicilio.ToString()).Text:"--";
            string municipio_domicilio = persona.Id_Municipio_Departamento_Domicilio!=null || persona.Id_Municipio_Departamento_Domicilio>0 ? (await Municipios((int)persona.Id_Departamento_Pais_Domicilio)).SingleOrDefault(d => d.Value == persona.Id_Municipio_Departamento_Domicilio.ToString()).Text:"--";

            string departamento_trabajo = persona.Id_Departamento_Pais_Direccion_Trabajo!=null || persona.Id_Departamento_Pais_Direccion_Trabajo >0? departamentos.SingleOrDefault(d => d.Value == persona.Id_Departamento_Pais_Direccion_Trabajo.ToString()).Text:"--";
            string municipio_trabajo = persona.Id_Municipio_Departamento_Direccion_Trabajo!=null || persona.Id_Municipio_Departamento_Direccion_Trabajo>0 ? (await Municipios((int)persona.Id_Departamento_Pais_Direccion_Trabajo)).SingleOrDefault(d => d.Value == persona.Id_Municipio_Departamento_Direccion_Trabajo.ToString()).Text:"--";

            string departamento_negocio = persona.Id_Departamento_Pais_Direccion_Negocio!=null || persona.Id_Departamento_Pais_Direccion_Negocio >0? departamentos.SingleOrDefault(d => d.Value == persona.Id_Departamento_Pais_Direccion_Negocio.ToString()).Text:"--";
            string municipio_negocio = persona.Id_Municipio_Departamento_Direccion_Negocio!=null || persona.Id_Municipio_Departamento_Direccion_Negocio>0 ? (await Municipios((int)persona.Id_Departamento_Pais_Direccion_Negocio)).SingleOrDefault(d => d.Value == persona.Id_Municipio_Departamento_Direccion_Negocio.ToString()).Text:"--";

            Ubicacion_Politica_Etiqueta_Persona persona_etiquetas = new Ubicacion_Politica_Etiqueta_Persona {
                Persona = persona,
                Etiquetas = new Etiqueta_Municipio_Departamento {
                    /*departamentos*/
                    Departamento_Nacimiento =departamento_nacimeinto,
                    Departamento_Domicilio=departamento_domicilio,
                    Departamento_Negocio=departamento_negocio,
                    Departamento_Trabajo=departamento_trabajo,
                    /*Municipios*/
                    Municipio_Nacimiento =municipio_nacimiento,
                    Municipio_Domicilio=municipio_domicilio,
                    Municipio_Negocio=municipio_negocio,
                    Municipio_Trabajo=municipio_trabajo
                }
            };
            return persona_etiquetas;
        }

        /*para referencia adicional como el conyuge*/

        [HttpGet]
        public async Task<IActionResult> DetalleReferenciaAdicional(
                int id_referencia,
                bool esNuevo,
                int tipo,
                int id_referencia_adicional=0,
                bool beneficiario = false)
        {
            var parentescos = from Parentesco d in Enum.GetValues(typeof(Parentesco))
                              select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };
            var tipos_referencia = from Tipo_Refencia_Option d in Enum.GetValues(typeof(Tipo_Refencia_Option))
                                   select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };

            if (esNuevo)
            {
                ViewBag.Id_Referencia = id_referencia;
                ViewBag.Beneficiario = beneficiario;
                ViewBag.Parentesco = new SelectList(parentescos, "Value", "Text");
                ViewBag.Tipo_Referencia = tipo; //new SelectList(tipos_referencia, "Value", "Text",tipo);
                return PartialView();
            }

            string filtroParaDetalle = string.Format("Referencias/Referencia_Adicional_Detalle?id_referencia_adicional={0}", id_referencia_adicional);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, filtroParaDetalle, null);
            string contenido = responseApi.ContenidofromJson;
            ReferenciaVista referencia = JsonConvert.DeserializeObject<ReferenciaVista>(contenido);
            ViewBag.Parentesco = new SelectList(parentescos, "Value", "Text", referencia.Parentesco);
            ViewBag.Tipo_Referencia = referencia.Tipo_Referencia; //new SelectList(tipos_referencia, "Value", "Text",referencia.Tipo_Referencia);
            ViewBag.Beneficiario = referencia.Beneficiario;
            return PartialView(referencia);
        }

        [HttpPost]
        public async Task<IActionResult> CrearReferenciaAdicional(ReferenciaVista referencia)
        {
            string jsonstring = JsonConvert.SerializeObject(referencia).Replace("\"", "'");
            string paraCrear = string.Format("Referencias/Crear_Referencia_Adicional");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);

            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.Error);
        }

        [HttpPost]
        public async Task<IActionResult> EditarReferenciaAdicional(ReferenciaVista referencia)
        {
            string jsonstring = JsonConvert.SerializeObject(referencia).Replace("\"", "'");
            string paraeditar = string.Format("Referencias/Editar_Referencia_Adicional");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, paraeditar, jsonstring);
            if(resultado.Satisfactorio)
                return Ok("Se editó correctamente la referencia adicional");
            else
                return BadRequest("No se editó correctamente la referencia adicional");
        }

        [HttpGet]
        public async Task<IActionResult> ReferenciasAdicionales(int id_referencia, int tipo, bool beneficiario = false)
        {
            try
            {
                string recuroyparametro = string.Format("Referencias/Referencias_Adicionales?id_referencia={0}", id_referencia);
                ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
                /*convertir el json por ahora*/
                string contenido = responseApi.ContenidofromJson;
                List<ReferenciaVista> referenciavista = JsonConvert.DeserializeObject<List<ReferenciaVista>>(contenido);
                ViewBag.Tipo = tipo;
                referenciavista = referenciavista.Where(f => f.Tipo_Referencia == tipo.ToString()).ToList();
                /*if (beneficiario == false && tipo == 1)
                    referenciavista = referenciavista.Where(f => f.Tipo_Referencia == tipo.ToString()).ToList();*/
                if (beneficiario)
                    referenciavista = referenciavista.Where(f => f.Tipo_Referencia == tipo.ToString() && f.Beneficiario == beneficiario).ToList();

                return PartialView(referenciavista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Cambiar_Estado_Persona_Select(Estado_Socio estado_socio)
        {
            //ViewBag.Estado = Enum.Parse(typeof(Estado_Socio), estado_socio.ToString());
            var estado_socio_list = from Estado_Socio d in Enum.GetValues(typeof(Estado_Socio))
                          select new SelectListItem { Value = ((int)d).ToString(), Text = d.ToString() };
            ViewBag.Estado_Socio_Lista= new SelectList(estado_socio_list, "Value", "Text", (int)estado_socio);
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Cambiar_Estado_Persona(int id_persona, Estado_Socio estado)
        {
            string recuroyparametro = string.Format("Personas/Cambiar_Estado_Persona?id_persona={0}&estado={1}", id_persona,(int)estado);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson.ToString());
            else
                return BadRequest(responseApi.Error.ToString());
        }
        
    }
}
