using AyCWeb.Helper;
using AyCWeb.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelNucleo.Dto;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace AyCWeb.Controllers
{
    public class AhorroController : BaseController
    {
        public IActionResult Index(AhorroFiltro filtro, string nombre, string dui)
        {
            if (filtro.id_persona != 0)
                filtro.porCliente = true;
            ViewBag.Id_Persona = filtro.id_persona;
            ViewBag.nombre = nombre;
            ViewBag.dui = dui;
            return View(filtro);
        }

        public async Task<IActionResult> Ahorros(DataSourceLoadOptions loadOptions, AhorroFiltro filtro)
        {
            /*convertir el json por ahora*/
            List<Ahorro> ahorrros = await Ahorros_Principal(filtro);
            var result = DataSourceLoader.Load(ahorrros, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        [HttpPost]
        public async Task<IActionResult> ProcesarRecalculo(int idAhorro, DateTime fechaInicio)
        {
            string recuroyparametro = string.Format(
                "Transacciones_Ahorro/RecalcularAhorroPorFecha?idAhorro={0}&fechaInicio={1:yyyy-MM-dd}",
                idAhorro,
                fechaInicio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        /*conección directa*/
        private async Task<List<Ahorro>> Ahorros_Principal(AhorroFiltro filtro)
        {
            string recuroyparametro = null;
            if (filtro.porCliente)
                recuroyparametro = string.Format("Ahorros/AhorrosPorPersona?id_persona={0}", filtro.id_persona);
            else
            {
                recuroyparametro = string.Format("Ahorros/Ahorros");
            }
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Ahorro> ahorrros = JsonConvert.DeserializeObject<List<Ahorro>>(contenido);
            return ahorrros;
        }

        private async Task<ResponseApi> Nuevo_Ahorro_Principal(Ahorro ahorro)
        {
            string jsonstring = JsonConvert.SerializeObject(ahorro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Ahorros/NuevoAhorro", jsonstring);
            return resultado;
        }

        private async Task<IEnumerable<SelectListItem>> Lineas_Ahorro(int id_linea_ahorro = 0)
        {
            string recuroyparametro = string.Format("Linea_Ahorros/LineasAhorro");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Linea_Ahorro> lienasahorro = JsonConvert.DeserializeObject<List<Linea_Ahorro>>(contenido);
            return lienasahorro.Select(s => new SelectListItem { Value = s.Id_Linea_Ahorro.ToString(), Text =s.Codigo+" "+s.Descripcion });
        }

        private async Task<ResponseApi> Editar_Ahorro_Principal(Ahorro ahorro)
        {
            string url = string.Format("Ahorros/Modificar");
            string jsonstring = JsonConvert.SerializeObject(ahorro).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, url, jsonstring);
            return resultado;
        }
        /*coneción directa a api*/

        //es el viejo se har[a uno mejor
        [HttpPost]
        public async Task<IActionResult> Create(string values, int id_persona)
        {
            /*hacer casting a su objeto por usar devexpress*/
            Ahorro ahorro = JsonConvert.DeserializeObject<Ahorro>(values);
            ahorro.Id_Ahorro = 0;
            ResponseApi resultado = await Nuevo_Ahorro_Principal(ahorro);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(Ahorro ahorro)
        {
            ResponseApi resultado = await Nuevo_Ahorro_Principal(ahorro);
            if (resultado.Satisfactorio)
            {
                return Ok(resultado.ContenidofromJson);
            }
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        // POST: Credito/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Edit(int key, string values)
        {
            /*hacer casting a su objeto por usar devexpress*/
            Ahorro ahorro = await Detalle(key);
            JsonConvert.PopulateObject(values, ahorro);
            /*conectarse al api*/
            ResponseApi resultado = await Editar_Ahorro_Principal(ahorro);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Ahorro_Manejo(bool esNuevo, int id_ahorro)
        {
            var lineas_ahorro = await Lineas_Ahorro();

            ViewBag.esNuevo = esNuevo;
            if (esNuevo)
            {
                ViewBag.Id_Linea_Ahorro = new SelectList(lineas_ahorro, "Value", "Text");
                return PartialView();
            }
            Ahorro ahorro = await Detalle(id_ahorro);
            ViewBag.Id_Linea_Ahorro = new SelectList(lineas_ahorro, "Value", "Text", ahorro.Id_Linea_Ahorro);
            return PartialView(ahorro);
        }
   
        public async Task<Ahorro> Detalle(int id_ahorro)
        {
            string recuroyparametro = string.Format("Ahorros/GetAhorroDetalle?id_ahorro={0}", id_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<Ahorro>(contenido);
        }

        /*Operaciones básicas para una cuenta de ahorro*/
        [HttpPost]
        public async Task<IActionResult> RegistrarDeposito(
            int id_ahorro,
            decimal valor,
            string comprobante = "",
            DateTime? fecha_cambiada = null,
            bool cambiar_fecha = false)
        {

            string fecha = cambiar_fecha? ((DateTime)fecha_cambiada).ToString("MM/dd/yyyy")
                 : DateTime.Now.ToString("MM/dd/yyyy");

            string url = string.Format("Transacciones_Ahorro/GuardarDeposito?id_ahorro={0}&valor={1}&comprobante={2}&fecha_cambiada={3}&cambiar_fecha={4}", 
                id_ahorro,
                valor,
                comprobante,
                fecha,
                cambiar_fecha);
            ResponseApi resultado = await OperarApi(HttpMethod.Put, url, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            return PartialView(MensajeAsync, mensaje);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarRetiro(
            int id_ahorro, 
            decimal valor, 
            string comprobante = "",
            DateTime? fecha_cambiada = null,
            bool cambiar_fecha = false)
        {

            string fecha = cambiar_fecha ? ((DateTime)fecha_cambiada).ToString("MM/dd/yyyy")
                 : DateTime.Now.ToString("MM/dd/yyyy");

            string url = string.Format(
                "Transacciones_Ahorro/GuardarRetiro?id_ahorro={0}&valor={1}&comprobante={2}&fecha_cambiada={3}&cambiar_fecha={4}", 
                id_ahorro, 
                valor, 
                comprobante,
                fecha,
                cambiar_fecha);
            ResponseApi resultado = await OperarApi(HttpMethod.Put, url, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            return PartialView(MensajeAsync, mensaje);
        }

        //public async Task<IActionResult> Transacciones2(DataSourceLoadOptions loadOptions, int id_ahorro)
        //{
        //    ResponseApi api = await Transacciones_Ahorro(id_ahorro);
        //    IEnumerable<Transaccion_Ahorro_Vista> transacciones = JsonConvert.DeserializeObject<List<Transaccion_Ahorro_Vista>>(api.ContenidofromJson);
        //    var result = DataSourceLoader.Load(transacciones, loadOptions);
        //    var resultJson = JsonConvert.SerializeObject(result);
        //    return Content(resultJson, "application/json");
        //}

        public async Task<PartialViewResult> Transacciones(int id_ahorro, int page = 1)
        {
            ResponseApi api = await Transacciones_Ahorro(id_ahorro, page);

            if (!api.Satisfactorio)
            {
                ViewBag.Respuesta = new Mensaje_Respuesta(false, 0, api.Error);
                return PartialView(new List<Transaccion_Ahorro_Vista>());
            }

            dynamic result = JsonConvert.DeserializeObject(api.ContenidofromJson);

            var transacciones = JsonConvert.DeserializeObject<List<Transaccion_Ahorro_Vista>>(result.data.ToString());

            ViewBag.Total = (int)result.total;
            ViewBag.Page = (int)result.page;
            ViewBag.PageSize = (int)result.pageSize;

            ViewBag.Respuesta = new Mensaje_Respuesta(true, transacciones.Count, null);

            return PartialView(transacciones);
        }

        private async Task<ResponseApi> Transacciones_Ahorro(int id_ahorro, int page)
        {
            string recuroyparametro = string.Format(
                "Transacciones_Ahorro/Transacciones?id_ahorro={0}&page={1}",
                id_ahorro,
                page
            );

            return await OperarApi(HttpMethod.Get, recuroyparametro, null);
        }

        /*postear transacción*/
        [HttpGet]
        public async Task<IActionResult> Mostrar_Recibo(int id_transaccion)
        {
            string direccion_credito = string.Format("Transacciones_Ahorro/Mostrar_Factura_Transaccion?id_transaccion_ahorro={0}", id_transaccion);
            ResponseApi respuesta_factura = await OperarApi(HttpMethod.Get, direccion_credito, null);
            if (respuesta_factura.Satisfactorio)
            {
                Impresion factura = JsonConvert.DeserializeObject<Impresion>(respuesta_factura.ContenidofromJson);
                return new ViewAsPdf("Mostrar_Factura_Abono", factura)
                {
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 80, Right = 10 }
                };
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(respuesta_factura.Satisfactorio, 0, respuesta_factura.Error, respuesta_factura.ContenidofromJson);
                return BadRequest(mensaje);
            }
        }
        /*postear libreta*/
        [HttpGet]
        public async Task<IActionResult> Postear_Libreta(int id_ahorro, int id_transaccion, int nrenglon)
        {
            string direccion_credito = string.Format("Transacciones_Ahorro/Rango_Transacciones?id_ahorro={0}&id_transaccion={1}", id_ahorro, id_transaccion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, direccion_credito, null);
            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                List<Transaccion_Ahorro_Vista> libreta = JsonConvert.DeserializeObject<List<Transaccion_Ahorro_Vista>>(contenido);
                ViewBag.renglon = nrenglon;
                return new ViewAsPdf("Postear_Libreta", libreta, ViewData)
                {
                    PageSize = Size.Legal,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 10, Right = 10 }
                };
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
                return BadRequest(mensaje);
            }

        }
        /*para las nuevas coneción diferentes renovación*/
        [HttpGet]
        public async Task<IActionResult> Ahorros_Manual(int id_persona)
        {
            AhorroFiltro af = new AhorroFiltro
            {
                porCliente = true,
                id_persona = id_persona
            };
            List<Ahorro> ahorros = await Ahorros_Principal(af);
            return PartialView(ahorros);
        }

        public async Task<IActionResult> Editar_Ahorro_Manual(Ahorro ahorro)
        {
            ResponseApi response = await Editar_Ahorro_Principal(ahorro);
            return Ok();
        }

        public async Task<IActionResult> Nuevo_Ahorro_Manual(Ahorro ahorro)
        {
            ResponseApi response = await Nuevo_Ahorro_Principal(ahorro);
            return Ok();
        }

        //firmas autorizadas
        [HttpGet]
        public IActionResult Detalle_Firma_Autorizada(bool esNuevo = false, int id_ahorro = 0)
        {
            ViewBag.Id_Ahorro = id_ahorro;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Nueva_Firma_Autorizada(Firma_Autorizada_Ahorro firma)
        {
            string jsonstring = JsonConvert.SerializeObject(firma).Replace("\"", "'");
            string url = string.Format("Ahorros/Nueva_Firma_Autorizada");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, url, jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Firmas_Autorizada(int id_ahorro)
        {
            string url = string.Format("Ahorros/Firmas_Autorizada?id_ahorro={0}", id_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, url, null);
            string contenido = responseApi.ContenidofromJson;
            List<Firma_Autorizada_Ahorro> firmas = JsonConvert.DeserializeObject<List<Firma_Autorizada_Ahorro>>(contenido);
            return PartialView(firmas);
        }

        //---------------------------------------- Libreta-----------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Nueva_Libreta(Libreta_Transaccion libreta)
        {
            /*conectarse al api*/
            string jsonstring = JsonConvert.SerializeObject(libreta).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Libreta/Crear_Libreta", jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            else
                return BadRequest(resultado.ContenidofromJson);
        }
        [HttpGet]
        public async Task<IActionResult> LibretaCliente(int id_ahorro)
        {
            string recurso = string.Format("Libreta/Libreta_Ahorro?id_ahorro={0}", id_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            Libreta_Transaccion libreta = JsonConvert.DeserializeObject<Libreta_Transaccion>(contenido);
            return Ok(libreta);
        }

        [HttpGet]
        public async Task<IActionResult> Rotulado_Libreta(int id_ahorro)
        {
            string recurso = string.Format("Libreta/Datos_Usuario?id_ahorro={0}", id_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            if (responseApi.Satisfactorio)
            {
                DateTime fecha = DateTime.Now;
                string contenido = responseApi.ContenidofromJson;
                Rotulado_Libreta rotulado = JsonConvert.DeserializeObject<Rotulado_Libreta>(contenido);
                ViewBag.fecha = fecha;
                return new ViewAsPdf("Rotulado_Libreta", rotulado, ViewData)
                {
                    PageSize = Size.Letter,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 80, Right = 10 }
                };
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
                return BadRequest(mensaje);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Cuenta_Ahorro_Libreta(int id_ahorro)
        {
            string recurso = string.Format("Libreta/Datos_Usuario?id_ahorro={0}", id_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            if (responseApi.Satisfactorio)
            {
                DateTime fecha = DateTime.Now;
                string contenido = responseApi.ContenidofromJson;
                Rotulado_Libreta rotulado = JsonConvert.DeserializeObject<Rotulado_Libreta>(contenido);
                ViewBag.fecha = fecha;
                return new ViewAsPdf("Numero_Cuenta_Ahorro", rotulado, ViewData)
                {
                    PageSize = Size.Letter,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 80, Right = 10 }
                };
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
                return BadRequest(mensaje);
            }
        }

        public IActionResult Mostrar_Planilla()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cargar_Planilla(bool procesar=false)
        {
            IFormFile file = Request.Form.Files[0];
            //string dato =Request.Form.Single(d=>d.Key=="procesar").Value;
            //bool procesar = false;
            string newPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UploadExcel");    ///Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            StringBuilder datos = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            List<Planilla> planilla = new List<Planilla>();

            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        decimal monto = Convert.ToDecimal(row.GetCell(2).ToString());
                        string codigo = Convert.ToString(row.GetCell(0).ToString());
                        string nombre = Convert.ToString(row.GetCell(1).ToString());
                        string mensaje_error = "";
                        bool procesado = false;
                        if (procesar)
                        {
                            ResponseApi respuesta= await Deposito_Individual(codigo, monto);
                            if (respuesta.Satisfactorio)
                                procesado = true;
                            else
                                mensaje_error = respuesta.ContenidofromJson;
                        }


                        planilla.Add(new Planilla
                        {
                            codigo = codigo,
                            nombre = nombre,
                            monto = monto,
                            mensaje_error=mensaje_error,
                            procesado=procesado
                        });

                    }

                }
            }
            ViewBag.Procesar = procesar;
            return PartialView(planilla);
        }


        private async Task<ResponseApi> Deposito_Individual(string cuenta, decimal valor)
        {
            string recuroyparametro = string.Format("Transacciones_Ahorro/Procesar_Planilla?cuenta={0}&valor={1}", cuenta,valor) ;
            return await OperarApi(HttpMethod.Post, recuroyparametro, null);
        }
        [HttpGet]
        public async Task<IActionResult> Generar_Solicitud_Socio(int id_persona)
        {
            string recuroyparametro = string.Format("Ahorros/Solicitud_Socio?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Solicitud_Socio(persona);
            return new FileStreamResult(stream, "application/msword")
                { FileDownloadName = "solicitud_socio.docx" };
        }
        public async Task<IActionResult> Solicitud_Cambio_Beneficiario(int id_persona,int id_ahorro)
        {
            string recuroyparametro = string.Format("Ahorros/Solicitud_Cambio_Beneficiario?id_persona={0}&id_ahorro={1}", id_persona,id_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Solicitud_Cambio_Beneficiario(persona);
            return new FileStreamResult(stream, "application/msword")
                { FileDownloadName = "solicitud_cambio_beneficiario.docx" };
        }

        public async Task<IActionResult> Perfil_Socio(int id_persona)
        {
            string recuroyparametro = string.Format("Ahorros/Perfil_Socio?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Perfil_Socio(persona);
            return new FileStreamResult(stream, "application/msword")
                { FileDownloadName = "perfil_socio.docx" };
        }
        public async Task<IActionResult> Declaracion_Jurada(int id_persona)
        {
            string recuroyparametro = string.Format("Ahorros/Declaracion_Jurada_Persona_Natural?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Solicitud_Declaracion_Jurada(persona);
            return new FileStreamResult(stream, "application/msword")
                { FileDownloadName = "declaracion_jurada.docx" };
        }
        public async Task<IActionResult> Contrato_Deposito(int id_persona, int id_ahorro)
        {
            string recuroyparametro = string.Format("Ahorros/Contrato_Deposito?id_persona={0}&id_ahorro={1}", id_persona,id_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Solicitud_Contrato_Deposito(persona);
            return new FileStreamResult(stream, "application/msword")
                { FileDownloadName = "contrato_deposito.docx" };
        }
        public async Task<IActionResult> Contrato_Deposito_Copia(int id_persona, int id_ahorro)
        {
            string recuroyparametro = string.Format("Ahorros/Contrato_Deposito?id_persona={0}&id_ahorro={1}", id_persona, id_ahorro);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Persona persona = JsonConvert.DeserializeObject<Persona>(contenido);

            Generar_Documento documento = new Generar_Documento();
            //documento.Guardar_Copia_Archivo();
            MemoryStream stream = documento.Generar_Solicitud_Contrato_Deposito_Copia(persona);
            return new FileStreamResult(stream, "application/msword")
                { FileDownloadName = "contrato_deposito_copia.docx" };
        }
       [HttpPost]
        public async Task<IActionResult> GenerarEstadoCuentaPDF(int idAhorro, DateTime fechaInicio, DateTime fechaFin,
        string textoPublicidad,
        string imagenArriba,
        string imagenAbajo)
        {
            string url = string.Format(
                "Transacciones_Ahorro/GetEstadoCuenta?idAhorro={0}&fechaInicio={1}&fechaFin={2}",
                idAhorro,
                fechaInicio.ToString("yyyy-MM-dd"),
                fechaFin.ToString("yyyy-MM-dd")
            );

            ResponseApi respuesta = await OperarApi(HttpMethod.Get, url, null);

            if (respuesta.Satisfactorio)
            {
                EstadoCuentaDTO data = JsonConvert.DeserializeObject<EstadoCuentaDTO>(respuesta.ContenidofromJson);
                data.TextoPublicidad = textoPublicidad;
                data.ImagenArriba = imagenArriba;
                data.ImagenAbajo = imagenAbajo;
                return new ViewAsPdf("EstadoCuentaPDF", data)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(15, 10, 15, 10),
                    CustomSwitches = "--footer-center \"Página [page] de [toPage]\" --footer-font-size 8"
                };
            }
            else
            {
                return BadRequest(respuesta.Error);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EnviarEstadoCuentaEmail(int idAhorro, DateTime fechaInicio, DateTime fechaFin,
            string textoPublicidad,
            string imagenArriba,
            string imagenAbajo)
        {
            try
            {
                // 🔹 consumir tu propio endpoint
                string url = string.Format(
                    "Transacciones_Ahorro/GetEstadoCuenta?idAhorro={0}&fechaInicio={1}&fechaFin={2}",
                    idAhorro,
                    fechaInicio.ToString("yyyy-MM-dd"),
                    fechaFin.ToString("yyyy-MM-dd")
                );

                ResponseApi respuesta = await OperarApi(HttpMethod.Get, url, null);

                if (!respuesta.Satisfactorio)
                    return BadRequest(respuesta.Error);

                EstadoCuentaDTO data = JsonConvert.DeserializeObject<EstadoCuentaDTO>(respuesta.ContenidofromJson);
                data.TextoPublicidad = textoPublicidad;
                data.ImagenArriba = imagenArriba;
                data.ImagenAbajo = imagenAbajo;
                // 🔥 GENERAR PDF EN MEMORIA
                var pdf = new ViewAsPdf("EstadoCuentaPDF", data)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4
                };

                byte[] pdfBytes = await pdf.BuildFile(ControllerContext);

                // ✉️ ENVIAR EMAIL
                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("sociedadcooperativacreditosayc@gmail.com", "xhew yexu zyhb cvhg");
                    smtp.EnableSsl = true;

                    var mail = new MailMessage();
                    mail.From = new MailAddress("sociedadcooperativacreditosayc@gmail.com");
                    mail.To.Add(data.EmailPersona);
                    mail.Subject = "Estado de Cuenta";
                    mail.Body = "Adjunto encontrarás tu estado de cuenta.";

                    mail.Attachments.Add(new Attachment(new MemoryStream(pdfBytes), "EstadoCuenta.pdf"));

                    await smtp.SendMailAsync(mail);
                    return Ok("Correo enviado correctamente");
                }
            }
            catch (Exception ex) { 
                return BadRequest(ex);
            }
           

           
        }
    }
}