using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Helper;
using AyCWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace AyCWeb.Controllers
{
    public class ColectaController : BaseController
    {
        public async Task<IActionResult> Colectas_Diarias(DateTime fecha,string mensaje=null, int id_agencia = 0)
        {
            string fecha_convertida=fecha.Date.ToString("MM/dd/yyyy");

            string recuroyparametro = string.Format("Colecta/Colectas_Diarias?fecha={0}&id_agencia={1}", fecha_convertida,id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Colecta_Diaria>colecta_diaria= JsonConvert.DeserializeObject<List<Colecta_Diaria>>(contenido);
            ViewBag.Rol = await Rol_Usuario();
            if (!string.IsNullOrEmpty(mensaje))
            {
                
                ViewBag.ConRespuesta = true;
                ViewBag.Respuesta = JsonConvert.DeserializeObject<Mensaje_Respuesta>(mensaje);
            }
            else
            {
                ViewBag.ConRespuesta = false;
                ViewBag.Respuesta = null;
            }
            return View(colecta_diaria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Colecta_Diaria(string no_comprobante_inicial, string no_comprobante_final, int id_colecta_diaria)
        {
            string recuroyparametro = string
                .Format("Colecta/Editar_Colecta_Diaria?no_comprobante_inicial={0}&no_comprobante_final={1}&id_colecta_diaria={2}",
                        no_comprobante_inicial,
                        no_comprobante_final,
                        id_colecta_diaria);
            ResponseApi resultado = await OperarApi(HttpMethod.Put, recuroyparametro, null);
            Mensaje_Respuesta mr = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            return PartialView(MensajeAsync, mr);
        }

        [HttpGet]
        public async Task<IActionResult> Nueva_Colecta_Diaria()
        {
            /*conectarse al api*/
            DateTime ahorita = DateTime.Now;
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Colecta/Nueva_Colecta_Diaria");
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Colectas_Diarias", new { year = ahorita.Year,month=ahorita.Month,mensaje=msj_str });
        }
        
        [HttpPost]
        public async Task<IActionResult> Borrar_Colecta_Diaria(int id_colecta_diaria)
        {
            /*conectarse al api*/
            DateTime ahorita = DateTime.Now;
            string recuroyparametro = string.Format("Colecta/Borrar_Colecta_Diaria?id_colecta_diaria={0}", id_colecta_diaria);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Colectas_Diarias", new { year = ahorita.Year, month = ahorita.Month, mensaje = msj_str });
        }

        /*colecta de detalle*/
        public async Task<IActionResult> Informe_Colecta_Diaria(int id_colecta_diaria)
        {
            Colecta_Diaria colecta = await Detalle_Colecta_Diaria(id_colecta_diaria);
            Colecta_Agrupada ca = new Colecta_Agrupada(colecta);
            return new ViewAsPdf("Informe_Colecta_Diaria", ca) {
                PageSize = Size.Letter,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 10, Right = 10 }
            };
        }

        private async Task<Colecta_Diaria> Detalle_Colecta_Diaria(int id_colecta_diaria = 0)
        {
            string recuroyparametro = "";
            if (id_colecta_diaria > 0)
                recuroyparametro = string.Format("Colecta/Detalle_Colecta_Diaria?id_colecta_diaria={0}", id_colecta_diaria);
            else
                recuroyparametro = string.Format("Colecta/Detalle_Colecta_Diaria_Actual");

            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<Colecta_Diaria>(contenido);
        }

        public async Task<IActionResult> Colectas(int id_colecta_diaria=0,string mensaje=null)
        {
            Colecta_Diaria colecta =await Detalle_Colecta_Diaria(id_colecta_diaria);

            if (!string.IsNullOrEmpty(mensaje))
            {
                ViewBag.ConRespuesta = true;
                ViewBag.Respuesta = JsonConvert.DeserializeObject<Mensaje_Respuesta>(mensaje);
            }
            else
            {
                ViewBag.ConRespuesta = false;
                ViewBag.Respuesta = null;
            }
            ViewBag.Rol = await Rol_Usuario();
            ViewBag.Id_Colecta_Diaria = id_colecta_diaria;
            return View(colecta);
        }

        public IActionResult Nueva_Colecta(int id_colecta_diaria)
        {
            ViewBag.Id_Colecta_Diaria = id_colecta_diaria;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Nueva_Colecta(Colecta colecta)
        {
            /*conectarse al api*/
            DateTime ahorita = DateTime.Now;
            string jsonstring = JsonConvert.SerializeObject(colecta).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, "Colecta/Nueva_Colecta", jsonstring);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Colectas", new { id_colecta_diaria = colecta.Id_Colecta_Diaria,mensaje=msj_str });
        }

        public async Task<IActionResult> Editar_Colecta(int id_colecta)
        {
            string recuroyparametro = string.Format("Colecta/Detalle_Colecta?id_colecta={0}", id_colecta);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Colecta colecta = JsonConvert.DeserializeObject<Colecta>(contenido);
            return View(colecta);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Colecta(Colecta colecta)
        {
            /*conectarse al api*/
            DateTime ahorita = DateTime.Now;
            string jsonstring = JsonConvert.SerializeObject(colecta).Replace("\"", "'");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, "Colecta/Editar_Colecta", jsonstring);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(resultado.Satisfactorio, 0, resultado.Error, resultado.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Colectas", new { id_colecta_diaria = colecta.Id_Colecta_Diaria, mensaje = msj_str });
        }

        [HttpGet]
        public async Task<IActionResult> Borrar_Colecta(int id_colecta,int id_colecta_diaria)
        {
            /*conectarse al api*/
            DateTime ahorita = DateTime.Now;
            string recuroyparametro = string.Format("Colecta/Borrar_Colecta?id_colecta={0}", id_colecta);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            Mensaje_Respuesta mensaje = new Mensaje_Respuesta(responseApi.Satisfactorio, 0, responseApi.Error, responseApi.ContenidofromJson);
            string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
            return RedirectToAction("Colectas", new { id_colecta_diaria, mensaje = msj_str });
        }

        [HttpGet]
        public async Task<IActionResult> Procesar_Colecta(int id_colecta_diaria)
        {
            string direccion_ahorro = string.Format("Transacciones_Ahorro/ProcesarColectaAhorro?id_colecta_diaria={0}", id_colecta_diaria);
            ResponseApi respuesta_ahorro = await OperarApi(HttpMethod.Put,direccion_ahorro,null);

            string direccion_credito = string.Format("Transacciones_Credito/ProcesarColectaCredito?id_colecta_diaria={0}",id_colecta_diaria);
            ResponseApi respuesta_credito = await OperarApi(HttpMethod.Put,direccion_credito,null);

            if (respuesta_ahorro.Satisfactorio || respuesta_credito.Satisfactorio)
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(true, 0, "", "Las colectas de ahorro y crédito se han procesado correctamente");
                string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
                return RedirectToAction("Colectas", new { id_colecta_diaria, mensaje = msj_str });
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(false, 0, "Primero Verifique las colectas para poder procesarlas", "");
                string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
                return RedirectToAction("Colectas", new { id_colecta_diaria, mensaje = msj_str });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Mostrar_Detalle_Factura_Creditos_Colecta(int id_colecta_diaria)
        {
            string direccion_ahorro = string.Format("Transacciones_Credito/Mostrar_Factura_Colecta?id_colecta_diaria={0}", id_colecta_diaria);
            ResponseApi respuesta_factura = await OperarApi(HttpMethod.Get, direccion_ahorro, null);
            if (respuesta_factura.Satisfactorio)
            {
                Impresion factura = JsonConvert.DeserializeObject<Impresion>(respuesta_factura.ContenidofromJson);
                return new ViewAsPdf("Mostrar_Detalle_Factura_Creditos_Colecta", factura)
                {
                    PageSize = Size.Letter,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = { Left = 10, Right = 10 }
                };
            }
            else {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(respuesta_factura.Satisfactorio, 0, respuesta_factura.Error, respuesta_factura.ContenidofromJson);
                return BadRequest(mensaje);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Mostrar_Detalle_Factura_Ahorro_Colecta(int id_colecta_diaria)
        {
            string direccion_ahorro = string.Format("Transacciones_Ahorro/Mostrar_Factura_Colecta?id_colecta_diaria={0}", id_colecta_diaria);
            ResponseApi respuesta_factura = await OperarApi(HttpMethod.Get, direccion_ahorro, null);

            if (respuesta_factura.Satisfactorio)
            {
                Impresion factura = JsonConvert.DeserializeObject<Impresion>(respuesta_factura.ContenidofromJson);
                return new ViewAsPdf("Mostrar_Detalle_Factura_Ahorro_Colecta", factura)
                {
                    PageSize = Size.Letter,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = { Left = 10, Right = 10 }
                };
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(respuesta_factura.Satisfactorio, 0, respuesta_factura.Error, respuesta_factura.ContenidofromJson);
                return BadRequest(mensaje);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Verificar_Colecta(int id_colecta_diaria)
        {
            string direccion_ahorro = string.Format("Colecta/Verificar_Colecta?id_colecta_diaria={0}", id_colecta_diaria);
            ResponseApi respuesta_ahorro = await OperarApi(HttpMethod.Put, direccion_ahorro, null);
            if (respuesta_ahorro.Satisfactorio )
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(true, 0, "", "Las colectas de ahorro y crédito se han verificado correctamente");
                string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
                return RedirectToAction("Colectas", new { id_colecta_diaria, mensaje = msj_str });
            }
            else
            {
                Mensaje_Respuesta mensaje = new Mensaje_Respuesta(false, 0, "Las colectas de ahorro y crédito ya fueron verificadas", "");
                string msj_str = JsonConvert.SerializeObject(mensaje).Replace("\"", "'");
                return RedirectToAction("Colectas", new { id_colecta_diaria, mensaje = msj_str });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detalle_Fotografia_Comprobante(int id_colecta)
        {
            string recuroyparametro = string.Format("Colecta/Imagen_Colecta_Detalle?id_colecta={0}", id_colecta);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Fotografia_Comprobante fotografia = JsonConvert.DeserializeObject<Fotografia_Comprobante>(contenido);
            ViewBag.id_fotografia_comprobante = fotografia.Id_Fotografia_Comprobante;
            ViewBag.estado_fotografia = fotografia.Verificado;
            ViewBag.foto = fotografia.Foto;
            return PartialView(fotografia);
            
        }

        
        [HttpPost]
        public async Task<IActionResult> Validar_Fotografia(int id_fotografia_imagen)
        {
           string recuroyparametro = string.Format("Colecta/Validar_Imagen_Colecta?id_fotografia_imagen={0}", id_fotografia_imagen);
           ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, recuroyparametro);
            if (responseApi.Satisfactorio)
               return Ok(responseApi.ContenidofromJson);
            else
               return BadRequest(responseApi.ContenidofromJson);
            
        }

        [HttpGet]
        public async Task<IActionResult> Dev_Foto(int id_colecta,int id_cliente, string nombre)
        {
            ViewBag.id_colecta = id_colecta;
            ViewBag.id_cliente = id_cliente;
            ViewBag.nombre = nombre;
            return PartialView();
        }
        public async Task<IActionResult> Subir_Foto_Colecta(int id_colecta, int id_cliente, string nombre)
        {

            var file = Request.Form.Files["Documento"];
            string ext = Path.GetExtension(file.FileName);

            if ( ext == ".jpg" || ext == ".png" || ext == ".jpeg")
            {
                Fotografia_Comprobante doc = new Fotografia_Comprobante
                {
                    Id_Colecta = id_colecta,
                    Id_Cliente = id_cliente,
                    Nombre_Completo = nombre,
                    //Creado_En = DateTime.Now,
                    Foto = FileToBase64(file),
                    Verificado = false,
                    Verificado_En = DateTime.MinValue,
                    Verificado_Por = ""
                };
                string recuroyparametro = string.Format("Colecta/Nuevo_Imagen_Colecta");
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