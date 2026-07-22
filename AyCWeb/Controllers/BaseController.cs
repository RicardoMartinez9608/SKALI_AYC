  using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using AyCWeb.Models;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using ModelNucleo.Model;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace AyCWeb.Controllers
{
    public class BaseController : Controller
    {
        private const string Token = "Token";
        private const string Usuario = "Usuario";
        private const string Autenticado = "esautenticado";
        private static HttpClient cliente;
        private HttpClientHandler handler;
        private string Url_Api = null;
        /*constantes publicas*/
        public const string MensajeAsync = "~/Views/Shared/MensajeAsync.cshtml";

        public BaseController()
        {
            if (string.IsNullOrEmpty(Url_Api))
                Url_Api = ObtenerUrl();
            handler = new HttpClientHandler() { AllowAutoRedirect = true, Proxy = null, UseProxy = false };
            cliente = new HttpClient(handler);
            cliente.BaseAddress = new Uri(Url_Api);
            cliente.DefaultRequestHeaders.ExpectContinue = false;

            cliente.Timeout = TimeSpan.FromMinutes(60);

            if (cliente.DefaultRequestHeaders.Accept.Count==0)
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private string ObtenerUrl()
        {
            var file = Path.GetFullPath("appsettings.json");
            var config = new ConfigurationBuilder().AddJsonFile(file, optional: false).Build();
            string url=config.GetValue<string>("Nucleo");
            return url;
        }

        /*método para loguearse*/
        public async Task<bool> LoginBack(string user,string contrasena)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "Account/Login");
            var jsonstring = "{'User':'" + user+"','Password':'"+contrasena+"'}";
            request.Content = new StringContent(jsonstring,
                Encoding.UTF8,
                "application/json");
            HttpResponseMessage response =cliente.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString(Token, token);
                HttpContext.Session.SetString(Usuario,user);
                HttpContext.Session.SetInt32(Autenticado, 1);
                await Nombre_Empresa();
                await LogoP_Empresa();
            }
            //cliente.Dispose();
            return response.IsSuccessStatusCode;
        }

        private async Task Nombre_Empresa()
        {
            string recuroyparametro = string.Format("Account/Nombre_Empresa");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
            {
                HttpContext.Session.SetString("empresa", responseApi.ContenidofromJson);
            }

            //         return Ok(responseApi.ContenidofromJson);
      //      return Ok("");
        }
        private async Task LogoP_Empresa()
        {
            string recuroyparametro = string.Format("Account/LogoPequeño_Empresa");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            if (responseApi.Satisfactorio)
            {
                HttpContext.Session.SetString("LEmpresa", responseApi.ContenidofromJson);
            }

            //         return Ok(responseApi.ContenidofromJson);
            //      return Ok("");
        }

        /*método para salir de la session*/
        public void LogoutBack()
        {
            HttpContext.Session.Remove(Token);
            HttpContext.Session.Remove(Usuario);
            HttpContext.Session.SetInt32(Autenticado, 0);
          
        }
        
        public async Task<ResponseApi> OperarApi(HttpMethod metodo,string recurso,string jsonstring=null)
        {
            
                HttpRequestMessage requesting = new HttpRequestMessage(metodo, recurso);
                string obtener_token = HttpContext.Session.GetInt32(Autenticado) == 1? HttpContext.Session.GetString(Token):"";
                requesting.Headers.Add("Authorization", string.Concat("Bearer ", obtener_token.ToString().Replace('"', ' ').Trim()));
                requesting.Properties.Add("Encoding", "UTF-8");
                /*obtener el request local*/
                if (!string.IsNullOrEmpty(jsonstring))
                    requesting.Content = new StringContent(jsonstring.ToString().Replace('"', ' ').Trim(), Encoding.UTF8, "application/json");
                //if (archivo != null)
                //    requesting.Content = ManejarArchivos(archivo);
                try
                {
                    using (HttpResponseMessage response = await cliente.SendAsync(requesting))
                    {
                        string error = "OK";
                        bool satisfactorio = true;
                        string status = "";
                        if (response.StatusCode.ToString() != "OK")
                        {
                            error = string.Format("{0}: {1}", response.StatusCode, await response.Content.ReadAsStringAsync());
                            satisfactorio = false;
                            status = response.StatusCode.ToString();
                        }

                        if (response.StatusCode.ToString() == "Unauthorized")
                        {
                            LogoutBack();
                            status = response.StatusCode.ToString();
                        }

                        //cliente.Dispose();
                        return new ResponseApi()
                        {
                            Satisfactorio = satisfactorio,
                            Error = error,
                            ContenidofromJson = await response.Content.ReadAsStringAsync(),
                            ContenidoDeStream = await response.Content.ReadAsStreamAsync(),
                            Status = status
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseApi()
                    {
                        Satisfactorio = false,
                        Error = ex.Message,
                        ContenidofromJson = null
                    };
                }
        }

        public MultipartFormDataContent ManejarArchivos(IFormFile archivo)
        {
            /*considerar si hay que subir archivos*/
            var contenido = new MultipartFormDataContent();
            /*verificar que tipo de archivo se subira*/
            using (var reader = new StreamReader(archivo.OpenReadStream()))
            {
                string contentAsString = reader.ReadToEnd();
                byte[] bytes = new byte[contentAsString.Length * sizeof(char)];
                var archivoContenido = new ByteArrayContent(bytes);
                archivoContenido.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                contenido.Add(archivoContenido, archivo.Name, archivo.FileName);
                return contenido;
            }
        }

        public string FilterBusquedaDX(string filter)
        {
            string[] data = filter.TrimStart('[', ' ').TrimEnd(']', ' ').Split(',');
            string sinMasCarateres=data[2].Trim().TrimStart('"',' ').TrimEnd('"',' ').Trim();
            return sinMasCarateres;
        }

        /*es repetido de credito*/
        public async Task<IEnumerable<SelectListItem>> CargarPeriodos()
        {
            string recuroyparametro = "Creditos/Periodos";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Periodo> periodos = JsonConvert.DeserializeObject<List<Periodo>>(contenido);
            return periodos.Select(s => new SelectListItem { Value = s.Id_Periodo.ToString(), Text = s.Descripcion });
        }

        public async Task<IEnumerable<SelectListItem>> CargarLineas()
        {
            string recuroyparametro = "Linea_Credito/LineasCredito";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Linea_Credito> lineas = JsonConvert.DeserializeObject<List<Linea_Credito>>(contenido);
            lineas.Add(new Linea_Credito{Id_Linea=0,Descripcion_linea="Seleccionar una Línea" });
            lineas=lineas.OrderBy(d => d.Id_Linea).ToList();
            return lineas.Select(s => new SelectListItem { Value = s.Id_Linea.ToString(), Text =s.Codigo+" "+s.Descripcion_linea });
        }

        public async Task<IEnumerable<SelectListItem>> CargarAgencias()
        {
            string recuroyparametro = "Account/Agencias";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Agencia> agencias = JsonConvert.DeserializeObject<List<Agencia>>(contenido);
            agencias.Add(new Agencia { Id_Agencia = 0, Nombre = "Seleccionar una agencia" });
            agencias=agencias.OrderBy(d => d.Id_Agencia).ToList();
            return agencias.Select(s => new SelectListItem { Value = s.Id_Agencia.ToString(), Text = s.Nombre });
        }


        public async Task<IEnumerable<SelectListItem>> CargarComportamientoCredito()
        {
            string recuroyparametro = "Creditos/Comportamiento_Credito";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Comportamiento_Credito> comportamientos = JsonConvert.DeserializeObject<List<Comportamiento_Credito>>(contenido);
            comportamientos.Add(new Comportamiento_Credito { Id_Comportamiento_Credito = 0, Descripcion_Comportamiento_Credito = "Seleccionar un comportamiento" });
            comportamientos = comportamientos.OrderBy(d => d.Id_Comportamiento_Credito).ToList();
            return comportamientos.Select(s => 
                new SelectListItem { 
                    Value = s.Id_Comportamiento_Credito.ToString(), 
                    Text = s.Descripcion_Comportamiento_Credito });
        }

        public async Task<IEnumerable<SelectListItem>> CargarLineaAhorro()
        {
            string recuroyparametro = "Linea_Ahorros/LineasAhorro";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Linea_Ahorro> lineas = JsonConvert.DeserializeObject<List<Linea_Ahorro>>(contenido);
            lineas.Add(new Linea_Ahorro { Id_Linea_Ahorro = 0, Descripcion = "Seleccionar una Línea" });
            lineas=lineas.OrderBy(d => d.Id_Linea_Ahorro).ToList();
            return lineas.Select(s => new SelectListItem { Value = s.Id_Linea_Ahorro.ToString(), Text = s.Codigo +" "+s.Descripcion });
        }

        public async Task<IEnumerable<SelectListItem>> Comportamientos_Credito()
        {
            string recuroyparametro = "Creditos/Comportamientos_Credito";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Comportamiento_Credito> comportamientos = JsonConvert.DeserializeObject<List<Comportamiento_Credito>>(contenido);
            return comportamientos.Select(s => new SelectListItem { Value = s.Id_Comportamiento_Credito.ToString(), Text = s.Descripcion_Comportamiento_Credito });
        }
        public async Task<IEnumerable<SelectListItem>> Porcentaje_Ahorro_Credito()
        {
            string recuroyparametro = "Creditos/Porcentaje_Ahorro";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Porcentaje_Ahorro_Credito> comportamientos = JsonConvert.DeserializeObject<List<Porcentaje_Ahorro_Credito>>(contenido);
            return comportamientos.Select(s => new SelectListItem { Value = s.Id_Porcentaje_Ahorro_Credito.ToString(), Text = s.Porcentaje.ToString() });
        }
        public async Task<IEnumerable<SelectListItem>> Comportamientos_Credito_Reportes()
        {
            string recuroyparametro = "Creditos/Comportamientos_Credito";
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Comportamiento_Credito> comportamientos = JsonConvert.DeserializeObject<List<Comportamiento_Credito>>(contenido);
            var c = comportamientos.Select(s => new SelectListItem { Value = s.Id_Comportamiento_Credito.ToString(), Text = s.Descripcion_Comportamiento_Credito }).ToList();
            c.Insert(0, new SelectListItem { Text = "Todos", Value = "0" });
            return c;
        }
        /*termina la repetición*/
    }
}