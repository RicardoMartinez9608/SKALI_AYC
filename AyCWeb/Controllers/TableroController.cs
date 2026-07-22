using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AyCWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using ModelNucleo.Model.Tablero;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AyCWeb.Controllers
{
    public class TableroController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Mostrar_Importar_Tablero()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cargar_Tablero(
            int tipo_tablero,
            bool procesar = false)
        {
                IFormFile file = Request.Form.Files[0];
                DateTime fechaAhora = DateTime.Now;
                string newPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UploadExcel");
                StringBuilder sb = new StringBuilder();
                StringBuilder datos = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                List<Rancking> ranckings = new List<Rancking>();
                List<Cosecha> cosecha = new List<Cosecha>();
                List<Reporte_Mora> mora = new List<Reporte_Mora>();

            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
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

                    if (tipo_tablero == 1)
                    {
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            string agencia = Convert.ToString(row.GetCell(0).ToString());
                            string asesor = Convert.ToString(row.GetCell(2).ToString());
                            decimal saldo_cartera = Convert.ToDecimal(row.GetCell(4).ToString());
                            decimal valor_pagado = Convert.ToDecimal(row.GetCell(5).ToString());
                            decimal abono_capital = Convert.ToDecimal(row.GetCell(6).ToString());
                            decimal capital_mora = Convert.ToDecimal(row.GetCell(18).ToString());
                            decimal interes = Convert.ToDecimal(row.GetCell(14).ToString());
                            decimal desembolsos = Convert.ToDecimal(row.GetCell(12).ToString());
                            decimal colocacion = Convert.ToDecimal(row.GetCell(16).ToString());
                            decimal colecta = Convert.ToDecimal(row.GetCell(5).ToString());
                            decimal refinanciado = Convert.ToDecimal(row.GetCell(15).ToString());
                            int clientes_activos = Convert.ToInt32(row.GetCell(20).ToString());
                            int credito = Convert.ToInt32(row.GetCell(21).ToString());


                            ranckings.Add(new Rancking
                            {
                                Agencia = agencia,
                                Asesor = asesor,
                                Saldo_Cartera = saldo_cartera,
                                Valor_Pagado = valor_pagado,
                                Abono_Capital = abono_capital,
                                Capital_Mora = capital_mora,
                                Ingresos = interes + desembolsos,
                                Desembolso = desembolsos,
                                No_Clientes_Activos = clientes_activos,
                                No_Creditos_Activos = credito,
                                Fecha = fechaAhora,
                                Colocacion = colocacion,
                                Colecta = colecta + refinanciado
                            });

                            //rancking_cosecha.Add(new Ranking_Cosecha
                            //{
                            //    ranking = ranckings
                            //});
                        }
                    }
                    else if (tipo_tablero == 2)
                    {
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            string asesor = Convert.ToString(row.GetCell(0).ToString());
                            decimal saldo = Convert.ToDecimal(row.GetCell(10).ToString());

                            cosecha.Add(new Cosecha
                            {
                                Agencia = "AC",
                                Asesor = asesor,
                                Saldo = saldo,
                                Fecha = fechaAhora
                            });
                        }
                    }
                    else if (tipo_tablero ==3) {
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            string asesor = Convert.ToString(row.GetCell(0).ToString());
                            string codigo = Convert.ToString(row.GetCell(1).ToString());
                            string nombre = Convert.ToString(row.GetCell(2).ToString());
                            decimal saldo_mora = Convert.ToDecimal(row.GetCell(10).ToString());
                            DateTime fecha_vencimiento = Convert.ToDateTime(row.GetCell(8).ToString());
                            DateTime fecha_ultimo_movimiento = Convert.ToDateTime(row.GetCell(7).ToString());
                            int dias_mora = Convert.ToInt32(row.GetCell(6).ToString());

                            mora.Add(new Reporte_Mora
                            {
                                Agencia = "AC",
                                Asesor = asesor,
                                Codigo = codigo,
                                Nombre_Cliente = nombre,
                                Saldo_Mora = saldo_mora,
                                Fecha_Vencimiento = fecha_vencimiento,
                                Fecha_Ultimo_Movimiento = fecha_ultimo_movimiento,
                                Dias_Mora = dias_mora,
                                Fecha = fechaAhora
                            });
                        }
                    }
                }
                Ranking_Cosecha rancking_cosecha = new Ranking_Cosecha { 
                    cosecha=cosecha,
                    ranking=ranckings,
                    mora = mora
                };
                if (procesar && tipo_tablero == 1)
                {
                    string jsonstring1 = JsonConvert.SerializeObject(ranckings).Replace("\"", "'");
                    string recuroyparametro1 = string.Format("Tablero/Procesar_Tablero");
                    ResponseApi responseApi1 = await OperarApi(HttpMethod.Post, recuroyparametro1, jsonstring1);
                }
                else if (procesar && tipo_tablero == 2)
                {
                    string jsonstring = JsonConvert.SerializeObject(cosecha).Replace("\"", "'");
                    string recuroyparametro = string.Format("Tablero/Procesar_Tablero_Cosecha");
                    ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
                }
                else if (procesar && tipo_tablero == 3)
                {
                    string jsonstring = JsonConvert.SerializeObject(mora).Replace("\"", "'");
                    string recuroyparametro = string.Format("Tablero/Procesar_Mora");
                    ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
                }
                ViewBag.Procesar = procesar;
                return PartialView(rancking_cosecha);
            
            }
            else
                return BadRequest("No existe archivo que cargar");
        }
        [HttpGet]
        public async Task<IActionResult> Meta()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear_Meta()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> Guardar_Meta(Meta meta)
        {
            string recuroyparametro = string.Format("Tablero/Agregar_Meta");
            string jsonstring = JsonConvert.SerializeObject(meta).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Metas_Por_Agencia(int id_agencia, int mes, int anio = 0)
        {
            string recuroyparametro = string.Format("Tablero/Metas_Por_Agencia?id_agencia={0}&mes={1}&anio={2}"
                , id_agencia, mes,anio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Meta> integrantes = JsonConvert.DeserializeObject<List<Meta>>(contenido);
            return PartialView(integrantes);
        }

        [HttpGet]
        public async Task<IActionResult> Editar_Meta(int id_meta)
        {
            string recuroyparametro = string.Format("Tablero/Meta?id_meta={0}", id_meta);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Meta meta = JsonConvert.DeserializeObject<Meta>(contenido);
            return PartialView(meta);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Meta(Meta meta)
        {
            string jsonstring = JsonConvert.SerializeObject(meta).Replace("\"", "'");
            string recuroyparametro = string.Format("Tablero/Editar_Meta");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpGet]
        public async Task<IActionResult> Tableo_Operaciones()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        [HttpGet]
        public async Task<decimal> Desempeno(int id_cartera=0,int month =0,int id_agencia = 0, int anio = 0)
        {
            string recuroyparametro = string.Format("Tablero/Desempeno_Asesor?id_cartera={0}&month={1}" +
                "&id_agencia={2}&anio={3}", 
                id_cartera,month, id_agencia,anio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<decimal>(contenido);
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
        public async Task<decimal> Colocacion(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio = 0)
        {
            string recuroyparametro = string.Format("Tablero/Colecta_Asesor?id_cartera={0}&month={1}&id_agencia={2}&anio={3}",
                id_cartera, month, id_agencia,anio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<decimal>(contenido);
        }
        [HttpGet]
        public async Task<Rancking> Metas(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio = 0)
        {
            string recuroyparametro = string.Format("Tablero/Metas_Asesor?id_cartera={0}&month={1}&id_agencia={2}&anio={3}", 
                id_cartera, month, id_agencia, anio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Rancking datos = JsonConvert.DeserializeObject<Rancking>(contenido);
            return datos;
        }
        [HttpGet]
        public async Task<decimal> Cosesha(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio = 0)
        {
            string recuroyparametro = string.Format("Tablero/Indice_Cosecha?id_cartera={0}&month={1}&id_agencia={2}&anio={3}",
                id_cartera, month, id_agencia, anio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert.DeserializeObject<decimal>(contenido);
        }

        [HttpGet]
        public async Task<List<decimal>> Colocacion_Asesores(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio = 0)
        {
            string recuroyparametro = string.Format("Tablero/Colecta_Asesores?id_cartera={0}&month={1}&id_agencia={2}&anio={3}",
                id_cartera, month, id_agencia,anio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
           
            string contenido = responseApi.ContenidofromJson; 
            List<decimal> lista =JsonConvert.DeserializeObject<List<decimal>>(contenido);
            return lista;
        }

        [HttpGet]
        public async Task<Grafico_Colocacion_Asesores> Mora_Asesores(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio = 0)
        {
            string recuroyparametro = string.Format("Tablero/Mora_Asesores?id_cartera={0}&month={1}&id_agencia={2}&anio={3}",
                id_cartera, month, id_agencia,anio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);

            string contenido = responseApi.ContenidofromJson;
            Grafico_Colocacion_Asesores lista = JsonConvert.DeserializeObject<Grafico_Colocacion_Asesores>(contenido);
  
            return lista;
        }
        [HttpGet]
        public async Task<Rancking> Fichero(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio = 0)
        {
            string recuroyparametro = string.Format("Tablero/Fichero?id_cartera={0}&month={1}&id_agencia={2}&anio={3}",
                id_cartera, month, id_agencia, anio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Rancking datos = JsonConvert.DeserializeObject<Rancking>(contenido);
            return datos;
        }
        [HttpGet]
        public async Task<IActionResult> Tableo_Mora()
        {
            ViewBag.Rol = await Rol_Usuario();
            return View();
        }
        [HttpGet]
        public async Task<Reporte_Mora> Fichero_Mora(int id_cartera = 0, int month = 0, int id_agencia = 0)
        {
            string recuroyparametro = string.Format("Tablero/Fichero_Mora?id_cartera={0}&month={1}&id_agencia={2}",
                id_cartera, month, id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reporte_Mora datos = JsonConvert.DeserializeObject<Reporte_Mora>(contenido);
            return datos;
        }
        [HttpGet]
        public async Task<Reporte_Mora> Ultimos_Abonos_Mora(int id_cartera = 0, int month = 0, int id_agencia = 0)
        {
            string recuroyparametro = string.Format("Tablero/Ultimos_Abonos_Mora?id_cartera={0}&month={1}&id_agencia={2}",
                id_cartera, month, id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Reporte_Mora datos = JsonConvert.DeserializeObject<Reporte_Mora>(contenido);
            return datos;
        }
        [HttpGet]
        public async Task<IActionResult> Principales_Deudores(int id_cartera = 0, int month = 0, int id_agencia = 0)
        {
            string recuroyparametro = string.Format("Tablero/Principales_Deudores?id_cartera={0}&month={1}&id_agencia={2}",
             id_cartera, month, id_agencia);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Reporte_Mora_Deudores> integrantes = JsonConvert.DeserializeObject<List<Reporte_Mora_Deudores>>(contenido);
            return PartialView(integrantes);
        }
    }
}
