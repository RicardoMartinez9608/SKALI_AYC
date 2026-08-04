using AyCWeb.Helper;
using AyCWeb.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Dto.Reportes_Ahorro;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AyCWeb.Controllers
{
    public class ReporteriaController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        /*es una copia del de crédito*/
        [HttpGet]
        public async Task<IActionResult> Amortizacion(int id_linea, decimal monto, int id_periodo, int plazo, int id_credito = 0,int numero_cuotas=0,string descripcion_linea="", decimal interes =0)
        {
            string recuroyparametro = string.Format("Creditos/CuotaPrimaria?id_linea={0}&monto={1}&id_periodo={2}&plazo={3}&id_credito={4}&numero_cuotas={5}", id_linea, monto, id_periodo, plazo, id_credito,numero_cuotas);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            ResultadoCalculoCreditioExtendido resultado = JsonConvert.DeserializeObject<ResultadoCalculoCreditioExtendido>(contenido);

            ViewBag.Codigo = resultado.Codigo_Credito;
            ViewBag.Cliente = resultado.Cliente;
            ViewBag.linea = descripcion_linea;
            ViewBag.monto = monto.ToString("c");
            ViewBag.fecha = DateTime.Now;
            ViewBag.interes = interes + " %";
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.plan = resultado.cuotaAyC + resultado.IVACalculado;   //resultado.Amortizacion.Max(a => a.Comision + a.IvaSobreComision);
            List<Amortizacion> amortizacion = resultado.Amortizacion;
            return new ViewAsPdf("Amortizacion", amortizacion, ViewData) {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };
        
        }

        [HttpGet]
        public IActionResult Parametros_Generales()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Generales_Credito(int Id_Agencia, string Nombre_Agencia, FechasFiltro filtro)
        {
            string inicio = filtro.inicio.Value.ToString("MM/dd/yyyy");
            string final = filtro.final.Value.ToString("MM/dd/yyyy");

            string recuroyparametro = string.Format("Reporte_Credito/Reporte_Generales_Credito?id_agencia={0}&fecha_inicio={1}&fecha_final={2}", Id_Agencia, inicio, final);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;

            List<Reporte_General_Credito> generales = JsonConvert.DeserializeObject<List<Reporte_General_Credito>>(contenido);

            Encabezado_Reporte encabezado = new Encabezado_Reporte
            {
                Agencia = Nombre_Agencia,
                Fecha_Inicio = inicio,
                Fecha_Final = final,
            };

            Reporte_Vista<IEnumerable<Reporte_General_Credito>> reporte = new Reporte_Vista<IEnumerable<Reporte_General_Credito>>();
            reporte.Encabezado = encabezado;
            reporte.Contenido = generales;
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.usuario = reporte.Contenido.Max(a => a.Generado_Por);
            ViewBag.emision = generales.Max(a => a.Fecha);
            return new ViewAsPdf("Generales_Credito", reporte, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Left = 10, Right = 10 }
            };
        }
        [HttpGet]
        public async Task<IActionResult> Filtrar_Rendimiento()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Resultado_Rendimiento(FechasFiltro filtro, int Id_Agencia, string Nombre_Agencia)
        {
            string inicio = filtro.inicio.Value.ToString("MM/dd/yyyy");
            string final = filtro.final.Value.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Reporte_Credito/Reporte_Generales_Rendimiento?fecha_inicio={0}&fecha_final={1}&id_agencia={2}", inicio, final, Id_Agencia);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Agencia_Rendimiento> rendimientos = JsonConvert.DeserializeObject<List<Agencia_Rendimiento>>(contenido);
        
            Encabezado_Reporte encabezado = new Encabezado_Reporte
            {
                Agencia = Nombre_Agencia,
                Fecha_Inicio = inicio,
                Fecha_Final = final,
            };
            // ViewBag.Encabezado = encabezado;
            Reporte_Vista<IEnumerable<Agencia_Rendimiento>> reporte = new Reporte_Vista<IEnumerable<Agencia_Rendimiento>>();
            reporte.Encabezado = encabezado;
            reporte.Contenido = rendimientos;
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.usuario = reporte.Contenido.Max(a=>a.Generado_Por) ;
            ViewBag.emision = rendimientos.Max(a => a.Fecha);
            return new ViewAsPdf("Resultado_Rendimiento", reporte, ViewData)
                {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Left = 10, Right = 10 }
            };

        }

        [HttpGet]
        public async Task<IActionResult> FlujoEfectivo()
        {
            string recuroyparametro = string.Format("ReporteFlujoEfectivo/Reporte_Flujo_Efectivo");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reporte_Flujo_Efectivo_Vista flujo = JsonConvert.DeserializeObject<Reporte_Flujo_Efectivo_Vista>(contenido);

            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            
            return new ViewAsPdf("FlujoEfectivo", flujo, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };

        }

        [HttpGet]
        public async Task<IActionResult> Filtrar_Flujo_Efectivo()
        {
            string recuroyparametro = string.Format("ReporteFlujoEfectivo/Vista_Flujo_Efectivo");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Detalle_Efectivo_Vista> flujo = JsonConvert.DeserializeObject<List<Detalle_Efectivo_Vista>>(contenido);
            ViewBag.Rol = await Rol_Usuario();
            Agencia agencia = await Obtener_Agencia();
            ViewBag.IdAgencia = agencia.Id_Agencia;
            ViewBag.NombreAgencia = agencia.Nombre;
            return View(flujo);
        }
        [HttpGet]
        public async Task<IActionResult> Obtener_Saldo(DateTime fecha_flujo, int Id_Agencia)
        {
            string fecha = fecha_flujo.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transacciones_Caja/Flujo_Efectivo_Agencia?fecha_flujo={0}&id_agencia={1}", fecha, Id_Agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reporte_Flujo_Efectivo_Vista flujo = JsonConvert.DeserializeObject<Reporte_Flujo_Efectivo_Vista>(contenido);
            return Ok(flujo.Disponible_a_la_fecha);
        }
        [HttpPost]
        public async Task<IActionResult> Reporte_Flujo_Efectivo(
            [FromForm] decimal[] denominacion,
            int[] cantidad,
            decimal[] dinero,
            string[] tipo,
            int NumeroCheque,
            decimal CantidadAki,
            decimal CantidadCheque,
            DateTime fecha_flujo, int Id_Agencia, string Nombre_Agencia)
        {
            List<Detalle_Efectivo_Vista> vista_efectivo = new List<Detalle_Efectivo_Vista>();
            List<Efectivo> efectivos = new List<Efectivo>();
            for (int i = 0; i < denominacion.Count(); i++)
            {
                
                vista_efectivo.Add(new Detalle_Efectivo_Vista { 
                    Efectivo=new Efectivo {
                        id_denominacion = (int)denominacion[i],
                        cantidad = cantidad[i],
                        monto= (cantidad[i]*dinero[i])
                    },
                    Denominacion=new Denominacion_efectivo { 
                        Id_denominacion= (int)denominacion[i],
                        denominacion=(decimal)dinero[i],
                        tipo= tipo[i].ToString()
                    }
                });
            }

            string fecha = fecha_flujo.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transacciones_Caja/Flujo_Efectivo_Agencia?fecha_flujo={0}&id_agencia={1}", fecha, Id_Agencia);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reporte_Flujo_Efectivo_Vista flujo = JsonConvert.DeserializeObject<Reporte_Flujo_Efectivo_Vista>(contenido);
            flujo.Cheques_Recibidos_Cantidad = NumeroCheque;
            flujo.Cheques_Recibidos_Monto = CantidadCheque;
            flujo.Detalle_bolsa_AKI = CantidadAki;
            flujo.Efectivo_Detallado = vista_efectivo;
            DateTime fecha_actual = DateTime.Now;
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.fecha = fecha_actual;
            ViewBag.agencia = Nombre_Agencia;
            //return View();
            return new ViewAsPdf("FlujoEfectivo", flujo, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };

        }

        [HttpPost]
        public async Task<IActionResult> Cerrar_Dia()
        {
            string recuroyparametro = string.Format("Transacciones_Caja/Cierre_Dia_Flujo");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public IActionResult Segmentacion_Cliente()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Segmentacion_Cliente_Principal(
            DevExtreme.AspNet.Mvc.DataSourceLoadOptions loadOptions,
            FechasFiltro filtro
            )
        {

            string inicio = filtro.inicio.Value.ToString("MM/dd/yyyy");
            string final = filtro.final.Value.ToString("MM/dd/yyyy");

            string recuroyparametro = string
                .Format("Reporte_Credito/Segmentacion_Cliente?inicio={0}&fin={1}", inicio, final);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Segmentacion_Cliente> segmentacions = JsonConvert.DeserializeObject<List<Segmentacion_Cliente>>(contenido);
            var result = DevExtreme.AspNet.Data.DataSourceLoader.Load(segmentacions, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }
        private async Task<string> Rol_Usuario()
        {
            string recurso = string.Format("Account/RolPorUsuario");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            string rol = JsonConvert.DeserializeObject<string>(contenido);
            return (rol);
        }
        private async Task<Agencia> Obtener_Agencia()
        {
            string recurso = string.Format("Account/ObtenerAgencia");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            Agencia agencia = JsonConvert.DeserializeObject<Agencia>(contenido);
            return (agencia);
        }
        [HttpGet]
        public async Task<IActionResult> Reporte_Creditos_Mora()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reporte_Mora_Cuotas_Vencidas(int id_cartera, DateTime inicio,DateTime final, bool es_mora = false)
        {
            string inicio_string=inicio.ToString("MM/dd/yyyy");
            string final_string = final.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Reporte_Credito/Reporte_Mora_Cuotas_Vencidas?id_cartera={0}&inicio={1}&final={2}&es_mora={3}", id_cartera, inicio_string,final_string, es_mora);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reporte_Vencimiento_Cuotas_Mora_Diaria data = JsonConvert.DeserializeObject<Reporte_Vencimiento_Cuotas_Mora_Diaria>(contenido);

            string valor = HttpContext.Session.GetString("empresa");
            string logoUrl = $"{Request.Scheme}://{Request.Host}/images/logo.png";
            ViewBag.LogoEmpresa = logoUrl;
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.es_mora = es_mora;
            ViewBag.inicio = inicio;
            ViewBag.final = final;
            return new ViewAsPdf("Vencimiento_Cuotas_Mora_Diaria", data, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Left = 10, Right = 10 }
            };


        }
        [HttpGet]
        public async Task<IActionResult> Reporte_Mora_Cuotas_Vencidas_Vista(int id_cartera, DateTime inicio, DateTime final, bool es_mora = false)
        {
            string inicio_string = inicio.ToString("MM/dd/yyyy");
            string final_string = final.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Reporte_Credito/Reporte_Mora_Cuotas_Vencidas?id_cartera={0}&inicio={1}&final={2}&es_mora={3}", id_cartera, inicio_string, final_string, es_mora);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reporte_Vencimiento_Cuotas_Mora_Diaria data = JsonConvert.DeserializeObject<Reporte_Vencimiento_Cuotas_Mora_Diaria>(contenido);

            return PartialView(data);


        }
        [HttpGet]
        public async Task<IActionResult> Movimiento_Diario_Prestamos()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Reporte_Movimiento_Diario_Prestamos_Vista(DateTime fecha_reporte,
           int id_cartera = 0, int id_agencia = 0)
        {
            string fecha = fecha_reporte.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transaccion_Simple/Movimiento_Diario_Prestamos?fecha={0}" +
                "&id_cartera={1}&id_agencia={2}", fecha, id_cartera, id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Movimiento_Diario_Prestamos> integrantes = JsonConvert.DeserializeObject<List<Movimiento_Diario_Prestamos>>(contenido);
            return PartialView(integrantes);

        }
       
        [HttpGet]
        public async Task<IActionResult> Reporte_Movimiento_Diario_Prestamos(DateTime fecha_reporte,
            int id_cartera = 0, int id_agencia = 0)
        {
            string fecha = fecha_reporte.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transaccion_Simple/Movimiento_Diario_Prestamos?fecha={0}" +
                "&id_cartera={1}&id_agencia={2}", fecha, id_cartera, id_agencia);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Movimiento_Diario_Prestamos> data = JsonConvert.DeserializeObject<List<Movimiento_Diario_Prestamos>>(contenido);

            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.fecha = fecha;
            ViewBag.emision = DateTime.Now.ToString("G");
            ViewBag.usuario = data.Max(a => a.Generado_Por);
           
            return new ViewAsPdf("Movimiento_Prestamos", data, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };


        }
        [HttpGet]
        public async Task<IActionResult> Bitacora_Reversiones()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Reporte_Bitacora(int id_agencia, DateTime inicio, DateTime final)
        {
            string fecha_inicio = inicio.ToString("MM/dd/yyyy");
            string fecha_final = final.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transaccion_Simple/Bitacora?id_agencia={0}" +
                "&fecha_inicio={1}&fecha_final={2}", id_agencia, fecha_inicio, fecha_final);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Bitacora> data = JsonConvert.DeserializeObject<List<Bitacora>>(contenido);

            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.emision = DateTime.Now.ToString("G");
            ViewBag.usuario = data.Max(a => a.Generado_Por);
            ViewBag.fecha = fecha_inicio;
            return new ViewAsPdf("Reporte_Bitacora_Reversiones", data, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };


        }
        [HttpGet]
        public async Task<IActionResult> Reporte_Creditos_Mora_Linea_Diario()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Reporte_Mora_Linea_Diario(int id_cartera, DateTime inicio, DateTime 
            final, bool es_mora = true)
        {
            string inicio_string = inicio.ToString("MM/dd/yyyy");
            string final_string = final.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Reporte_Credito/Reporte_Mora_Credito_Diario?id_cartera={0}" +
                "&inicio={1}&final={2}&es_mora={3}", id_cartera, inicio_string, final_string, es_mora);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reporte_Vencimiento_Cuotas_Mora_Diaria data = JsonConvert.DeserializeObject<Reporte_Vencimiento_Cuotas_Mora_Diaria>(contenido);

            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.es_mora = es_mora;
            ViewBag.inicio = inicio;
            ViewBag.final = final;
            return new ViewAsPdf("Reporte_Mora_Linea_Diario", data, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Left = 10, Right = 10 }
            };


        }
        [HttpGet]
        public async Task<IActionResult> Movimientos_Contabilidad()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Reporte_Movimiento_Diario_Contabilidad(DateTime fecha_reporte,int id_agencia = 0)
        {
            string fecha = fecha_reporte.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Reporte_Credito/Movimiento_Contabilidad?fecha={0}" +
                "&id_agencia={1}", fecha, id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Movimientos_Contabilidad> movimientos = JsonConvert.DeserializeObject<List<Movimientos_Contabilidad>>(contenido);
            return PartialView(movimientos);

        }
        [HttpPost]
        public async Task<IActionResult> Verificar_Partida(int id_asiento_contable)
        {
            string recuroyparametro = string.Format("Configuracion_Contabilidad/Verificar_Partida_Contable" +
                "?id_asiento_contable={0}", id_asiento_contable);
            string jsonstring = JsonConvert.SerializeObject(recuroyparametro).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpGet]
        public async Task<IActionResult> Exportar_Cartera_Asesor()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Cartera_Asesor_Vista(int id_cartera = 0)
        {
            string recuroyparametro = string.Format("Reporte_Credito/Cartera_Asesor?id_cartera={0}", id_cartera);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Cartera_Asesor> cartera = JsonConvert.DeserializeObject<List<Cartera_Asesor>>(contenido);
            return PartialView(cartera);

        }
        [HttpGet]
        public async Task<IActionResult> Cartera_Asesor_PDF(int id_cartera)
        {
            string inicio_string = DateTime.Now.ToString();
            string recuroyparametro = string.Format("Reporte_Credito/Cartera_Asesor?id_cartera={0}", id_cartera);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Cartera_Asesor> data = JsonConvert.DeserializeObject<List<Cartera_Asesor>>(contenido);

            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            ViewBag.inicio = inicio_string;
            ViewBag.asesor = data.Max(a => a.Nombre_Asesor);
            ViewBag.solicitado = data.Max(b => b.Solicitado_Por);
            return new ViewAsPdf("Reporte_Cartera_Asesor", data, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Left = 10, Right = 10 }
            };


        }
        [HttpGet]
        public async Task<IActionResult> ReporteDTE( DateTime? fechaInicio, DateTime? fechaFin)
        {
            string url = $"Reporte_Credito/ObtenerReporteDTE?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}";

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, url, null);

            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                var data = JsonConvert.DeserializeObject<List<Bitacora>>(contenido);
               
                return new ViewAsPdf("ReporteDTE", data, ViewData)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 10, Right = 10 }
                };
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(
                    responseApi.Satisfactorio,
                    0,
                    responseApi.Error,
                    responseApi.ContenidofromJson
                );
                return BadRequest(mensaje);
            }
        }
        [HttpGet]
        public async Task<IActionResult> BitacoraDTE()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ReporteConsolidadoAhorros()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ReporteConsolidadoAhorro(
        int idLineaAhorro,
        int idAgencia,
        DateTime fecha)
        {
            string url =
                $"Ahorros/ReporteConsolidado?idLineaAhorro={idLineaAhorro}" +
                $"&idAgencia={idAgencia}" +
                $"&fecha={fecha:yyyy-MM-dd}";

            ResponseApi responseApi =
                await OperarApi(HttpMethod.Get, url, null);

            if (responseApi.Satisfactorio)
            {
                var reporte = JsonConvert.DeserializeObject<ReporteConsolidadoAhorroDTO>(
                responseApi.ContenidofromJson);

                return new ViewAsPdf("ReporteConsolidadoAhorro", reporte, ViewData)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                };
            }

            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(
                responseApi.Satisfactorio,
                0,
                responseApi.Error,
                responseApi.ContenidofromJson);

            return BadRequest(mensaje);
        }
        [HttpGet]
        public async Task<IActionResult> ReporteConsolidadoAhorroExcel(
        int idLineaAhorro,
        int idAgencia,
        DateTime fecha)
        {
            string url =
                $"Ahorros/ReporteConsolidado?idLineaAhorro={idLineaAhorro}" +
                $"&idAgencia={idAgencia}" +
                $"&fecha={fecha:yyyy-MM-dd}";

            ResponseApi responseApi =
                await OperarApi(HttpMethod.Get, url, null);

            if (!responseApi.Satisfactorio)
                return BadRequest(responseApi.Error);

            var reporte = JsonConvert.DeserializeObject<ReporteConsolidadoAhorroDTO>(
                responseApi.ContenidofromJson);

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Consolidado");

                int fila = 1;

                // Empresa
                ws.Cell(fila, 1).Value = "SOCIEDAD COOPERATIVA CREDITOS AGIL Y CONFIABLE DE RL";
                ws.Range(fila, 1, fila, 5).Merge();
                ws.Range(fila, 1, fila, 5).Style.Font.Bold = true;
                ws.Range(fila, 1, fila, 5).Style.Font.FontSize = 16;
                ws.Range(fila, 1, fila, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                fila++;

                // Título
                ws.Cell(fila, 1).Value = "REPORTE CONSOLIDADO DE AHORROS";
                ws.Range(fila, 1, fila, 5).Merge();
                ws.Range(fila, 1, fila, 5).Style.Font.Bold = true;
                ws.Range(fila, 1, fila, 5).Style.Font.FontSize = 14;
                ws.Range(fila, 1, fila, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                fila += 2;

                ws.Cell(fila, 1).Value = "Línea de ahorro";
                ws.Cell(fila, 2).Value = reporte.NombreLineaAhorro;

                ws.Cell(fila, 4).Value = "Fecha de corte";
                ws.Cell(fila, 5).Value = reporte.FechaCorte;
                ws.Cell(fila, 5).Style.DateFormat.Format = "dd/MM/yyyy";

                fila++;

                ws.Cell(fila, 1).Value = "Fecha impresión";
                ws.Cell(fila, 2).Value = DateTime.Now;
                ws.Cell(fila, 2).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";

                ws.Cell(fila, 4).Value = "Total cuentas";
                ws.Cell(fila, 5).Value = reporte.TotalCuentas;

                fila += 2;

                // Encabezados
                ws.Cell(fila, 1).Value = "#";
                ws.Cell(fila, 2).Value = "Cuenta";
                ws.Cell(fila, 3).Value = "Socio";
                ws.Cell(fila, 4).Value = "DUI";
                ws.Cell(fila, 5).Value = "Saldo";

                var encabezado = ws.Range(fila, 1, fila, 5);

                encabezado.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                encabezado.Style.Font.FontColor = XLColor.White;
                encabezado.Style.Font.Bold = true;

                fila++;

                int correlativo = 1;

                foreach (var item in reporte.Detalle)
                {
                    ws.Cell(fila, 1).Value = correlativo++;
                    ws.Cell(fila, 2).Value = item.NoCuenta;
                    ws.Cell(fila, 3).Value = item.Socio;
                    ws.Cell(fila, 4).Value = item.Dui;
                    ws.Cell(fila, 5).Value = item.Saldo;

                    ws.Cell(fila, 5).Style.NumberFormat.Format = "$ #,##0.00";

                    fila++;
                }

                ws.Cell(fila, 4).Value = "TOTAL CONSOLIDADO";
                ws.Cell(fila, 5).Value = reporte.TotalSaldo;

                ws.Range(fila, 4, fila, 5).Style.Font.Bold = true;
                ws.Range(fila, 4, fila, 5).Style.Fill.BackgroundColor = XLColor.LightGreen;
                ws.Cell(fila, 5).Style.NumberFormat.Format = "$ #,##0.00";

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"ReporteConsolidado_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
                }
            }
        }
    }
}
