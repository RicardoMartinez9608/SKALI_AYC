using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Helper;
using AyCWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace AyCWeb.Controllers
{
    public class CajaController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        //separación de operaciones de pagos de crédito con ahorros
        public async Task<IActionResult> Pago_Credito()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        //separación de operaciones de cuenta de ahorro
        public async Task<IActionResult> Deposito_Ahorro()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        public async Task<IActionResult> Retiro_Ahorro()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }


        public IActionResult Operaciones_Boveda()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transaccion_Boveda_Caja(int Id_Caja, bool Asignar, decimal Monto)
        {
            string recuroyparametro;
            if (Asignar)
                recuroyparametro = string.Format("Transaccion_Simple/Asignar_Fondos?id_caja={0}&monto={1}", Id_Caja, Monto);
            else
                recuroyparametro = string.Format("Transaccion_Simple/Recibir_Fondos_Cierre?id_caja={0}&monto={1}", Id_Caja, Monto);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
            return PartialView(MensajeAsync, mensaje);
        }

        [HttpPost]
        public async Task<IActionResult> Revertir_Transaccion(int ids, bool es_credito, bool es_deposito = false)
        {
            string json_ids = JsonConvert.SerializeObject(ids).Replace("\"", "'");
            string recuroyparametro = null;
            if (es_credito) {
                recuroyparametro = string.Format("Transacciones_Credito/Reversion_Credito");
            }
            if (!es_credito && !es_deposito) {
                recuroyparametro = string.Format("Transacciones_Ahorro/Reversion_Ahorro");
            }
            if (es_deposito) {
                recuroyparametro = string.Format("Transacciones_Deposito_Plazo/Reversion_Deposito");
            }
        
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, json_ids);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
            return PartialView(MensajeAsync, mensaje);
        }

        public IActionResult Seleccionar_Reporte_Boveda_Cierre()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Transacciones_Boveda(DateTime? fecha)
        {
            string recuroyparametro = "";
            if (fecha != null)
            {
                DateTime fecha_Convertida = (DateTime)fecha;
                string fecha_transformada = fecha_Convertida.ToString("MM/dd/yyyy");
                recuroyparametro = string.Format("Transacciones_Caja/Transacciones_Como_Boveda?fecha={0}", fecha_transformada);
            }
            else
                recuroyparametro = string.Format("Transacciones_Caja/Transacciones_Como_Boveda");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Transacciones_Caja_Vista transacciones = JsonConvert.DeserializeObject<Transacciones_Caja_Vista>(contenido);

            return new ViewAsPdf("Transacciones_Boveda", transacciones)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };
        }

        [HttpGet]
        public async Task<IActionResult> Transacciones_Detalle_Ejercicio(DateTime fecha)
        {
            string inicio = fecha.ToString("MM/dd/yyyy");
            ViewBag.Fecha = inicio;
            string recuroyparametro = string.Format("Transacciones_Caja/Transacciones_Detalle_Ejercicio_Caja?fecha={0}", inicio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                CajaVista transacciones = JsonConvert.DeserializeObject<CajaVista>(contenido);
                return View(transacciones);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Deposito_Caja(DateTime fecha)
        {
            string inicio = fecha.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transacciones_Caja/Depositos?fecha={0}", inicio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                decimal depositoresumen = JsonConvert.DeserializeObject<decimal>(contenido);
                return Ok(depositoresumen);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Pagos_Caja(DateTime fecha)
        {
            string inicio = fecha.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transacciones_Caja/Pagos?fecha={0}", inicio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                TransaccionCreditoResumen pagosresumen = JsonConvert.DeserializeObject<TransaccionCreditoResumen>(contenido);
                return Ok(pagosresumen);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Pagos_Caja_Detalle(DateTime fecha)
        {
            string inicio = fecha.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transacciones_Caja/Pagos_Detalle?fecha={0}", inicio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                List<Transaccion_Credito> pagosdetalles = JsonConvert.DeserializeObject<List<Transaccion_Credito>>(contenido);
                return PartialView(pagosdetalles);
            }
            return PartialView();
        }

        /*consulta de transacciones*/
        [HttpGet]
        public async Task<IActionResult> Transacciones_Caja( DateTime fecha, int id_caja=0)
        {
            string inicio = fecha.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Transacciones_Caja/Transacciones_Como_Caja?fecha={0}&id_caja={1}", inicio, id_caja);
 
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Transacciones_Caja_Vista transacciones = JsonConvert.DeserializeObject<Transacciones_Caja_Vista>(contenido);
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            return new ViewAsPdf("Transacciones_Caja", transacciones, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };
        }

        [HttpGet]
        public async Task<IActionResult> Estructuraciones(int id_persona, DateTime fecha)
        {
            string fecha_formato = fecha.ToString("MM/dd/yyyy");
            string recuroyparametro = string
                .Format("Transaccion_Simple/Estructuraciones?id_persona={0}&fecha={1}", 
                id_persona,
                fecha_formato);

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);string contenido = responseApi.ContenidofromJson;
            List<Deduccion_Estructuriacion> transacciones= JsonConvert.DeserializeObject<List<Deduccion_Estructuriacion>>(contenido);
            return PartialView(transacciones);
        }

        /// <summary>
        /// facturas o comprobantes solo para transacciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Mostrar_Comprobante_Factura(int id_transaccion_simple)
        {
            string recuroyparametro = string.Format("Transaccion_Simple/Mostrar_Comprobante_Factura?id_transaccion_simple={0}", id_transaccion_simple);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Impresion factura= JsonConvert.DeserializeObject<Impresion>(contenido);
            return new ViewAsPdf("_Comprobante_Factura", factura)
            {
                PageSize = Size.Letter,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 80, Right = 10 }
            };
        }

        public async Task<IActionResult> Flujo_Efectivo()
        {
            string recuroyparametro = string.Format("ReporteFlujoEfectivo/Vista_Flujo_Efectivo");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Detalle_Efectivo_Vista> flujo = JsonConvert.DeserializeObject<List<Detalle_Efectivo_Vista>>(contenido);
            return View(flujo);
        }

        [HttpGet]
        public IActionResult Filtrar_Liquidez() 
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> Mostrar_Liquidez_Agencia(DateTime fecha, int Id_Agencia)
        {
            string inicio = fecha.ToString("MM/dd/yyyy");
            string parametro = string.Format("Transacciones_Caja/Liquidez_Agencia_Lista?fecha={0}&id_agencia={1}",inicio, Id_Agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, parametro, null);
            string contenido = responseApi.ContenidofromJson;
            Liquidez_Vista reporte = JsonConvert.DeserializeObject<Liquidez_Vista>(contenido);
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            return new ViewAsPdf("Mostrar_Liquidez_Agencia", reporte, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };
        }

        [HttpGet]
        public async Task<IActionResult> Ejecutar_Cierre_Mes(DateTime fecha, int Id_Agencia)
        {
            string inicio = fecha.ToString("MM/dd/yyyy");
            string parametro = string.Format("Transacciones_Caja/Cierre_Mes?fecha={0}&id_agencia={1}", inicio, Id_Agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, parametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }


        [HttpGet]
        public async Task<IActionResult> Saldo_Anterior()
        {
            string recuroyparametro = string.Format("Transacciones_Caja/Saldo_Anterior");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            decimal saldo = JsonConvert.DeserializeObject<decimal>(contenido);
            return Ok(saldo);
        }

        [HttpPost]
        public async Task<IActionResult> Procesar_Efectivo(
            [FromForm]decimal[] denominacion, 
            int[] cantidad, 
            int numero_cheque, 
            decimal cantidad_cheque,
            decimal cantidad_aki)
        {
            try
            {
                List<Efectivo> efectivos = new List<Efectivo>();
                for (int i = 0; i < denominacion.Count(); i++)
                {
                    efectivos.Add(new Efectivo
                    {
                        id_denominacion = (int)denominacion[i],
                        cantidad = (int)cantidad[i]
                    });
                }
                string json_efectivo = JsonConvert.SerializeObject(efectivos).Replace("\"", "'");
                string recuroyparametro = string.Format(
                    "Transacciones_Caja/Recibir_Detalle_Efectivo?numero_cheque={0}&cantidad_cheque={1}&cantidad_aki={2}",
                    numero_cheque,
                    cantidad_cheque,
                    cantidad_aki);
                ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, json_efectivo);
                if (responseApi.Satisfactorio)
                {
                    string contenido = responseApi.ContenidofromJson;
                    Faltante_Caja faltante = JsonConvert.DeserializeObject<Faltante_Caja>(contenido);
                    return PartialView(faltante);
                }
                else
                    return BadRequest(responseApi.ContenidofromJson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
         
        }
        private async Task<string> Rol_Usuario()
        {
            string recurso = string.Format("Account/RolPorUsuario");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            string rol = JsonConvert.DeserializeObject<string>(contenido);
            return (rol);
        }

        [HttpGet]
        public async Task<IActionResult> Corte_Movimiento_Caja(bool fechas, DateTime fecha1, DateTime fecha2, int id_caja)
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        private async Task<List<Caja>> Cajas()
        {
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, "Caja/Cajas", null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<List<Caja>>(contenido);
        }

        [HttpGet]
        public async Task<IActionResult> Cajas_Agencia()
        {
            return PartialView(await Cajas());
        }

        [HttpGet]
        public async Task<IActionResult> Mostrar_Corte_Caja(Filtro_Corte_Movimiento_Caja filtro)
        {
            string fecha_inicio = filtro.fecha_inicio.ToString("MM/dd/yyyy");
            string fecha_final = filtro.fecha_final.ToString("MM/dd/yyyy");

            Corte_de_Caja corte = new Corte_de_Caja();
            string parametros = string.Format(
                "Transacciones_Caja/Corte_Movimiento_Caja?rango={0}&fecha_inicio={1}&fecha_final={2}&id_caja={3}",
                filtro.rango,
                fecha_inicio,
                fecha_final,
                filtro.id_caja);
            ResponseApi respuesta = await OperarApi(HttpMethod.Get, parametros, null);
            corte= JsonConvert.DeserializeObject<Corte_de_Caja>(respuesta.ContenidofromJson);
            string valor = HttpContext.Session.GetString("empresa");
            ViewBag.nombreEmpresa = valor.Replace('"', ' ');
            
                return new ViewAsPdf("Corte_Caja", corte, ViewData)
                {
                    PageSize = Size.Letter,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = { Left = 10, Right = 10 }
                };
        }

        [HttpGet]
        public async Task<IActionResult> Consulta_Movimientos()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Mostrar_Movimientos_Partida(Filtro_Corte_Movimiento_Caja filtro)
        {
            string fecha_inicio = filtro.fecha_inicio.ToString("MM/dd/yyyy");
            string parametros = string.Format(
                "Transacciones_Caja/Monitor_Movimiento?fecha={0}&id_caja={1}",
                fecha_inicio,
                filtro.id_caja);
            ResponseApi respuesta = await OperarApi(HttpMethod.Get, parametros, null);
            List<Monitor_Movimientos> detalle= JsonConvert.DeserializeObject<List<Monitor_Movimientos>>(respuesta.ContenidofromJson);
            return PartialView(detalle);
        }

        [HttpGet]
        public async Task<IActionResult> Mostrar_Libreta(int id_persona)
        {
           
            string recuroyparametro = string.Format("Transaccion_Simple/Lista_Libretas?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null); string contenido = responseApi.ContenidofromJson;
            List<Libreta_Transaccion> transacciones = JsonConvert.DeserializeObject<List<Libreta_Transaccion>>(contenido);
            return PartialView(transacciones);
        }
        [HttpGet]
        public async Task<IActionResult> Mostrar_Factura_Libreta(int id_ahorro, bool retiro_cuenta = false)
        {
            string recuroyparametro = string.Format("Transaccion_Simple/Pago_Libreta?id_ahorro={0}&retiro_cuenta={1}", id_ahorro,retiro_cuenta);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);

            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                Impresion factura = JsonConvert.DeserializeObject<Impresion>(contenido);
                return new ViewAsPdf("Cobro_Libreta", factura)
                {
                    PageSize = Size.Letter,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = { Left = 80, Right = 10 }
                };
            }
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public IActionResult Otros_Cobros()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Ingreso_Movimientos_Caja()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Ingreso_Egreso_Caja(bool ingreso = false, decimal cantidad = 0.00M,decimal excedente=0.00M)
        {
            string parametro = string.Format("Caja/Ingreso_Egreso_Caja?ingreso={0}&cantidad={1}", ingreso, cantidad);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, parametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Ingreso_Excedente_Caja(decimal excedente = 0.00M)
        {
            string parametro = string.Format("Caja/Ingreso_Excedente_Caja?excedente={0}", excedente);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, parametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        //--------------------------------------------------- depositos a plazo ------------------------------------------------------
        public async Task<IActionResult> Deposito_Plazo()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        public async Task<IActionResult> Retiro_Plazo()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }

    }
}