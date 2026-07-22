using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using ModelNucleo.Model.Tablero;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class DocumentoController : BaseController
    {
        public IActionResult Index(int id_persona, string nombre_completo, int id_descripcion_documento=0)
        {
            ViewBag.Id_Persona = id_persona;
            ViewBag.Nombre_Completo = nombre_completo;
            ViewBag.Id_Descripcion_Documento = id_descripcion_documento;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Expedientes(string dui, int id_descripcion_documento = 0, int id_banco=0)
        {
            Expediente expediente = new Expediente();
            if (!string.IsNullOrEmpty(dui))
            {
                string recuroyparametro = string.Format("Documentos/Expediente?dui={0}", dui);
                ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
                /*convertir el json por ahora*/
                string contenido = responseApi.ContenidofromJson;
                expediente = JsonConvert.DeserializeObject<Expediente>(contenido);
            }
            ViewBag.Id_Descripcion_Documento = id_descripcion_documento;
            ViewBag.Id_Banco = id_banco;
            return View(expediente);
        }

        public async Task<IActionResult> Documentos(DataSourceLoadOptions loadOptions,int id_persona)
        {
            string recuroyparametro = string.Format("Documentos/Documentos?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Documento> documento = JsonConvert.DeserializeObject<List<Documento>>(contenido);

            var result = DataSourceLoader.Load(documento, loadOptions);
            var resultJson = JsonConvert.SerializeObject(result);
            return Content(resultJson, "application/json");
        }

        public async Task<IActionResult> Nuevo(int id_persona, int id_descripcion_documento)
        {
            var file = Request.Form.Files["Documento"];
            string ext = Path.GetExtension(file.FileName);

            if (ext == ".pdf" || ext == ".jpg" || ext == ".png")
            {
                Documento doc = new Documento
                {
                    Id_Persona = id_persona,
                    Id_Descripcion_Documento=id_descripcion_documento,
                    Fecha = DateTime.Now,
                    Archivo_Base64 = FileToBase64(file),
                    Es_Imagen = (ext != ".pdf"),
                    Es_PDF = (ext == ".pdf")
                };
                string recuroyparametro = string.Format("Documentos/Nuevo_Documento");
                string jsonstring = JsonConvert.SerializeObject(doc).Replace("\"", "'");
                ResponseApi response = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
                return Ok();
            }
            else
                return BadRequest("el formato del archivo no es permitido");
        }


        /*solo lo voy a ocupara para descargar pdfs*/
        [HttpGet]
        public async Task<IActionResult> Detalle(int id_documento)
        {
            string recuroyparametro = string.Format("Documentos/Documento_Detalle?id_documento={0}", id_documento);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Documento documento= JsonConvert.DeserializeObject<Documento>(contenido);

            byte[] data = Convert.FromBase64String(documento.Archivo_Base64);
            string nombre_archivo = "documento_persona.pdf";
            return File(data,System.Net.Mime.MediaTypeNames.Application.Octet,nombre_archivo);
        }

        public async Task<string> Imagen(string nombre_archivo)
        {
            string recuroyparametro = string.Format("Documentos/DescargarArchivo?nombre_archivo={0}", nombre_archivo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            Stream stream=responseApi.ContenidoDeStream;
            return ConvertirABase64(stream);
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

        private string ConvertirABase64(Stream stream)
        {
            byte[] bytes;
            using (var memoria=new MemoryStream())
            {
                stream.CopyTo(memoria);
                bytes = memoria.ToArray();
            }
            return Convert.ToBase64String(bytes);
        }
        [HttpGet]
        public async Task<IActionResult> Fotografia_Atm()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Dev_Foto(int id_banco)
        {
            ViewBag.id_banco = id_banco;
            return PartialView();
        }
        public async Task<IActionResult> Subir_Foto_Atm(int id_banco)
        {
           
            var file = Request.Form.Files["Documento"];
            string ext = Path.GetExtension(file.FileName);

            if (ext == ".pdf" || ext == ".jpg" || ext == ".png" || ext == ".jpeg")
            {
                Fotografia_Atm doc = new Fotografia_Atm
                {
                    Id_Banco = id_banco,
                    Creado_En = DateTime.Now,
                    fotografia = FileToBase64(file),
                    Verificado = false,
                    Verificado_En= DateTime.MinValue,
                    Verificado_Por = ""
                };
                string recuroyparametro = string.Format("Documentos/Nuevo_Documento_Atm");
                string jsonstring = JsonConvert.SerializeObject(doc).Replace("\"", "'");
                ResponseApi response = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
                return Ok();
            }
            else
                return BadRequest("el formato del archivo no es permitido");
        }
        [HttpGet]
        public async Task<IActionResult> Verificacion_Atm()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Listado_Verificacion(DateTime inicio, DateTime fin)
        {
            string fecha_inicio = inicio.ToString("MM/dd/yyyy");
            string Fecha_fin = fin.ToString("MM/dd/yyyy");
            string recuroyparametro = string.Format("Documentos/Listado_Verificaciones?fecha_inicio={0}&Fecha_fin={1}"
                , fecha_inicio, Fecha_fin);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Fotografia_Atm> atms = JsonConvert.DeserializeObject<List<Fotografia_Atm>>(contenido);
            return PartialView(atms);
        }
        [HttpGet]
        public async Task<IActionResult> Detalle_Fotografia_Atm(int id_fotografia_atm)
        {
            string recuroyparametro = string.Format("Documentos/Detalle_Atm?id_fotografia_atm={0}", id_fotografia_atm);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            Fotografia_Atm fotografia = JsonConvert.DeserializeObject<Fotografia_Atm>(contenido);
            ViewBag.id_fotografia_comprobante = fotografia.Id_Fotografia_Atm;
            ViewBag.estado_fotografia = fotografia.Verificado;
            ViewBag.foto = fotografia.fotografia;
            return PartialView(fotografia);

        }
        [HttpPost]
        public async Task<IActionResult> Validar_Fotografia(int id_fotografia_atm)
        {
            string recuroyparametro = string.Format("Documentos/Validar_Imagen_Atm?id_fotografia_atm={0}", id_fotografia_atm);
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, recuroyparametro);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);

        }
    }
}