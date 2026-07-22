using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace AyCWeb.Controllers
{
    public class AccionController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Manejo_Accion()
        {

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Acciones(int id_persona)
        {
            List<Accion> acciones = await Acciones_Principal(id_persona);
            return PartialView(acciones);
        }

        [HttpGet]
        public IActionResult Imprimir_Accion()
        {
            return Ok();
        }

        public IActionResult Impresion_Acciones()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Acciones_Pendientes()
        {
            List<Accion> acciones = await Acciones_Pendientes_Directo();
            return PartialView(acciones);//no impreso, pero si cancelado
        }
        
        private async Task<List<Accion>> Acciones_Pendientes_Directo()
        {
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, "Accion/Acciones_Pendientes", null);
            string contenido = responseApi.ContenidofromJson;
            List<Accion> acciones = JsonConvert.DeserializeObject<List<Accion>>(contenido);
            return acciones;
        }

        [HttpGet]
        public async Task<IActionResult> Acciones_Cancelar(int id_persona)
        {
            List<Accion> acciones = await Acciones_Pendientes_Por_Persona_Principal(id_persona);
            return PartialView(acciones);
        }

        private async Task<List<Accion>> Acciones_Pendientes_Por_Persona_Principal(int id_persona)
        {
            string recurso = string.Format("Accion/Acciones_Pendiente_Por_Persona?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            List<Accion> acciones = JsonConvert.DeserializeObject<List<Accion>>(contenido);
            return acciones;
        }
        

        public async Task<IActionResult> Registrar_Certificado(int id_accion,string no_certificado, int id_persona)
        {
            string recurso = string.Format("Accion/Registrar_Certificado?id_accion={0}&no_certificado={1}&id_persona={2}", id_accion, no_certificado,id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            Imprimir_Accion accion = JsonConvert.DeserializeObject<Imprimir_Accion>(contenido);
            int numero_mes = accion.Fecha.Month;
            ViewBag.mes = Nombre_Mes(numero_mes);
            return new ViewAsPdf("Imprimir_Accion", accion, ViewData)
            {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Left = 10, Right = 10 }
            };
        }
        public async Task<IActionResult> Consultar_Accion( int id_persona)
        {
            string recurso = string.Format("Accion/Consultar_Accion?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            if (responseApi.Satisfactorio)
            {
                string contenido = responseApi.ContenidofromJson;
                Imprimir_Accion accion = JsonConvert.DeserializeObject<Imprimir_Accion>(contenido);
                int numero_mes = accion.Fecha.Month;
                ViewBag.mes = Nombre_Mes(numero_mes);
                return new ViewAsPdf("imprimir_accion", accion, ViewData)
                {
                    PageSize = Size.Letter,
                    PageOrientation = Orientation.Landscape,
                    PageMargins = { Left = 10, Right = 10 }
                };
            }
            else
            {
                return BadRequest(responseApi.Error);
            }
        }

        //[HttpPost]
        public async Task<IActionResult> Registrar_Cobro(int id_accion)
        {
            string recurso = string.Format("Transaccion_Simple/Cobrar_Accion?id_accion={0}",id_accion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            Impresion factura = JsonConvert.DeserializeObject<Impresion>(contenido);
            return new ViewAsPdf("Factura_Accion", factura)
            {
                PageSize = Size.Letter,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = { Left = 80, Right = 10 }
            };
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(bool esNuevo,int id_accion=0,int id_persona=0)
        {
            if (esNuevo)
            {
                ViewBag.Id_Persona = id_persona;
                return PartialView();
            }
            Accion accion =await Accion_Principal(id_accion);
            return PartialView(accion);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(Accion accion)
        {
            string jsonstring = JsonConvert.SerializeObject(accion).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Accion/Nuevo", jsonstring);
            if (resultado.Satisfactorio)
                return Ok(resultado.ContenidofromJson);
            return BadRequest(resultado.ContenidofromJson);
        }

        private async Task<Accion> Accion_Principal(int id_accion)
        {
            string recuroyparametro = string.Format("Accion/Detalle?id_accion={0}", id_accion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Accion accion = JsonConvert.DeserializeObject<Accion>(contenido);
            return accion;
        }


        private async Task<List<Accion>> Acciones_Principal(int id_persona)
        {
            string recuroyparametro = string.Format("Accion/Acciones_Persona?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Accion> acciones = JsonConvert.DeserializeObject<List<Accion>>(contenido);
            return acciones;
        }
        private string Nombre_Mes(int mes)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-Es", false).DateTimeFormat;
            return dtinfo.GetMonthName(mes);
        }
        [HttpGet]
        public async Task<IActionResult> Acciones_Dia(DateTime fecha_nueva, int id_agencia = 0)
        {
            string fecha = fecha_nueva.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Accion/Acciones_Por_Agencia?fecha={0}&id_agencia={1}", fecha, id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);

            string contenido = responseApi.ContenidofromJson;
            List<Accion> creditos_pendientes = JsonConvert.DeserializeObject<List<Accion>>(contenido);
            return PartialView(creditos_pendientes);
        }
        [HttpGet]
        public async Task<IActionResult> Acciones_Por_Dia()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        private async Task<string> Rol_Usuario()
        {
            string recurso = string.Format("Account/RolPorUsuario");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recurso, null);
            string contenido = responseApi.ContenidofromJson;
            string rol = JsonConvert.DeserializeObject<string>(contenido);
            return (rol);
        }
    }
}