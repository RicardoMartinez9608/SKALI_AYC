using AyCWeb.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelNucleo.Model.Evaluacion_Tecnica;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace AyCWeb.Controllers
{
    public class Evaluacion_TecnicaController : BaseController
    {
        // GET: Evaluacion_Tecnica
        [HttpGet]
        public async Task<IActionResult> Index(int id_persona,string nombre_completo)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Evaluaciones_Persona?id_persona={0}", id_persona);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Evaluacion> evaluaciones = JsonConvert.DeserializeObject<List<Evaluacion>>(contenido);

            ViewBag.Id_Persona = id_persona;
            ViewBag.Nombre_Completo = nombre_completo;
            return View(evaluaciones);
        }

        [HttpGet]
        public IActionResult Nuevo_Inicial(int id_persona, string nombre_completo)
        {
            ViewBag.Id_Persona = id_persona;
            ViewBag.Nombre_Completo = nombre_completo;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Modificar_Evaluacion(int id_evaluacion)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Detalle_Evaluacion?id_evaluacion={0}", id_evaluacion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Evaluacion evaluacion = JsonConvert.DeserializeObject<Evaluacion>(contenido);
            return View(evaluacion);
        }

        public async Task<IActionResult> Modificar_Evaluacion(Evaluacion evaluacion)
        {
            string jsonstring = JsonConvert.SerializeObject(evaluacion).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Modificar_Evaluacion");
            ResponseApi resultado = await OperarApi(HttpMethod.Put, paraCrear, jsonstring);

            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = evaluacion.Id_Evaluacion });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar_Evaluacion(Evaluacion evaluacion)
        {
            string jsonstring = JsonConvert.SerializeObject(evaluacion).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Nueva_Evaluacion");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);

            return RedirectToAction("Index",new {id_persona=evaluacion.Id_Persona,nombre_completo=""});
        }

        [HttpGet]
        public async Task<IActionResult> Gestionar_Detalle(int id_evaluacion)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Detalle_Evaluacion_Tecnica?id_evaluacion={0}", id_evaluacion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Evaluacion_Tecnica_Vista evaluacion = JsonConvert.DeserializeObject<Evaluacion_Tecnica_Vista>(contenido);

            return View(evaluacion);
        }


        /*el resto de los detalles para agregar y editar*/
        #region Gastos
        public async Task<IActionResult> Nuevo_Gasto_Form(int id_evaluacion,int id_categoria_gasto)
        {
            ViewBag.Id_Evaluacion = id_evaluacion;
            ViewBag.Id_Tipo_Gasto =await Tipos_Gastos(id_categoria_gasto);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo_Gasto(Gasto gasto)
        {
            string jsonstring = JsonConvert.SerializeObject(gasto).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Nuevo_Gasto");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new {id_evaluacion= gasto.Id_Evaluacion});
        }

        public async Task<IActionResult> Modificar_Gasto_Form(int id_gasto,int id_categoria_gasto)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Detalle_Gasto?id_gasto={0}", id_gasto);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Gasto gasto = JsonConvert.DeserializeObject<Gasto>(contenido);
            /*arreglar el select*/
            ViewBag.Id_Tipo_Gasto =await Tipos_Gastos(id_categoria_gasto,gasto.Id_Tipo_Gasto);
            return View(gasto);
        }

        [HttpPost]
        public async Task<IActionResult> Modificar_Gasto(Gasto gasto)
        {
            string jsonstring = JsonConvert.SerializeObject(gasto).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Modificar_Gasto");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = gasto.Id_Evaluacion });
        }

        #endregion
        #region Otros_Ingresos
        public async Task<IActionResult> Nuevo_Otro_Ingreso_Form(int id_evaluacion)
        {
            ViewBag.Id_Evaluacion = id_evaluacion;
            ViewBag.Id_Tipo_Ingreso =await Tipos_Otros_Ingresos();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo_Otro_Ingreso(Otro_Ingreso otro_ingreso)
        {
            string jsonstring = JsonConvert.SerializeObject(otro_ingreso).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Nuevo_Otro_Ingreso");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = otro_ingreso.Id_Evaluacion });
        }

        public async Task<IActionResult> Modificar_Otro_Ingreso_Form(int id_otro_ingreso)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Detalle_Otro_Ingreso?id_otro_ingreso={0}", id_otro_ingreso);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Otro_Ingreso otro_ingreso = JsonConvert.DeserializeObject<Otro_Ingreso>(contenido);
            /*arreglar el select*/
            ViewBag.Id_Tipo_Ingreso =await Tipos_Otros_Ingresos(otro_ingreso.Id_Tipo_Ingreso);
            return View(otro_ingreso);
        }

        [HttpPost]
        public async Task<IActionResult> Modificar_Otro_Ingreso(Otro_Ingreso otro_ingreso)
        {
            string jsonstring = JsonConvert.SerializeObject(otro_ingreso).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Modificar_Otro_Ingreso");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = otro_ingreso.Id_Evaluacion });
        }
        #endregion
        #region Prestamos
        public IActionResult Nuevo_Prestamo_Form(int id_evaluacion)
        {
            ViewBag.Id_Evaluacion = id_evaluacion;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo_Prestamo(Prestamo prestamo)
        {
            string jsonstring = JsonConvert.SerializeObject(prestamo).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Nuevo_Prestamo");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = prestamo.Id_Evaluacion });
        }

        public async Task<IActionResult> Modificar_Prestamo_Form(int id_prestamo)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Detalle_Prestamo?id_prestamo={0}", id_prestamo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Prestamo prestamo = JsonConvert.DeserializeObject<Prestamo>(contenido);
            return View(prestamo);
        }

        [HttpPost]
        public async Task<IActionResult> Modificar_Prestamo(Prestamo prestamo)
        {
            string jsonstring = JsonConvert.SerializeObject(prestamo).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Modificar_Prestamo");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = prestamo.Id_Evaluacion });
        }
        #endregion
        #region Cuentas_cobrar
        public IActionResult Nuevo_Cuenta_Cobrar_Form(int id_evaluacion)
        {
            ViewBag.Id_Evaluacion = id_evaluacion;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo_Cuenta_Cobrar(Cuenta_Cobrar cuenta_cobrar)
        {
            string jsonstring = JsonConvert.SerializeObject(cuenta_cobrar).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Nueva_Cuenta_Cobrar");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = cuenta_cobrar.Id_Evaluacion });
        }

        public async Task<IActionResult> Modificar_Cuenta_Cobrar_Form(int id_cuenta_cobrar)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Detalle_Cuenta_Cobrar?id_cuenta_cobrar={0}", id_cuenta_cobrar);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Cuenta_Cobrar cuenta_cobrar = JsonConvert.DeserializeObject<Cuenta_Cobrar>(contenido);
            return View(cuenta_cobrar);
        }

        [HttpPost]
        public async Task<IActionResult> Modificar_Cuenta_Cobrar(Cuenta_Cobrar cuenta_cobrar)
        {
            string jsonstring = JsonConvert.SerializeObject(cuenta_cobrar).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Modificar_Cuenta_Cobrar");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = cuenta_cobrar.Id_Evaluacion });
        }
        #endregion
        #region Inventarios
        public IActionResult Nuevo_Inventario_Form(int id_evaluacion)
        {
            ViewBag.Id_Evaluacion = id_evaluacion;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo_Inventario(Inventario_Negocio inventario)
        {
            string jsonstring = JsonConvert.SerializeObject(inventario).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Nuevo_Inventario");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = inventario.Id_Evaluacion });
        }

        public async Task<IActionResult> Modificar_Inventario_Form(int id_inventario)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Detalle_Inventario?id_inventario={0}", id_inventario);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Inventario_Negocio inventario = JsonConvert.DeserializeObject<Inventario_Negocio>(contenido);
            return View(inventario);
        }

        [HttpPost]
        public async Task<IActionResult> Modificar_Inventario(Inventario_Negocio inventario)
        {
            string jsonstring = JsonConvert.SerializeObject(inventario).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Modificar_Inventario");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = inventario.Id_Evaluacion });
        }
        #endregion
        #region Costos_Ventas
        public IActionResult Nuevo_Costo_Venta_Form(int id_evaluacion)
        {
            ViewBag.Id_Evaluacion = id_evaluacion;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo_Costo_Venta(Costo_Venta costo_venta)
        {
            string jsonstring = JsonConvert.SerializeObject(costo_venta).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Nuevo_Costo_Venta");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = costo_venta.Id_Evaluacion });
        }

        public async Task<IActionResult> Modificar_Costo_Venta_Form(int id_costo_venta)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Detalle_Costo_Venta?id_costo_venta={0}", id_costo_venta);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Costo_Venta costo_venta = JsonConvert.DeserializeObject<Costo_Venta>(contenido);
            return View(costo_venta);
        }

        [HttpPost]
        public async Task<IActionResult> Modificar_Costo_Venta(Costo_Venta costo_venta)
        {
            string jsonstring = JsonConvert.SerializeObject(costo_venta).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Modificar_Costo_venta");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = costo_venta.Id_Evaluacion });
        }
        #endregion
        #region Activos_Negocio
        public IActionResult Nuevo_Activo_Negocio_Form(int id_evaluacion)
        {
            ViewBag.Id_Evaluacion = id_evaluacion;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo_Activo_Negocio(Activo_Negocio activo_negocio)
        {
            string jsonstring = JsonConvert.SerializeObject(activo_negocio).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Nuevo_Activo_Negocio");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = activo_negocio.Id_Evaluacion });
        }

        public async Task<IActionResult> Modificar_Activo_Negocio_Form(int id_activo_negocio)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Detalle_Activo_Negocio?id_activo_negocio={0}", id_activo_negocio);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Activo_Negocio activo_negocio = JsonConvert.DeserializeObject<Activo_Negocio>(contenido);
            return View(activo_negocio);
        }

        [HttpPost]
        public async Task<IActionResult> Modificar_Activo_Negocio(Activo_Negocio activo_negocio)
        {
            string jsonstring = JsonConvert.SerializeObject(activo_negocio).Replace("\"", "'");
            string paraCrear = string.Format("Evaluacion_Tecnica/Modificar_Activo_Negocio");
            ResponseApi resultado = await OperarApi(HttpMethod.Post, paraCrear, jsonstring);
            return RedirectToAction("Gestionar_Detalle", new { id_evaluacion = activo_negocio.Id_Evaluacion });
        }
        #endregion

        #region Tipo_Gasto
        private async Task<List<SelectListItem>> Tipos_Gastos(int id_categoria_gasto,int id_seleccionado=0)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Tipos_Gastos?id_categoria_gasto={0}", id_categoria_gasto);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Tipo_Gasto> tipos= JsonConvert.DeserializeObject<List<Tipo_Gasto>>(contenido);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Tipo_Gasto tipo in tipos)
            {
                SelectListItem item = new SelectListItem {
                    Text = tipo.Descripcion,
                    Value = tipo.Id_Tipo_Gasto.ToString(),
                    Selected = id_seleccionado == tipo.Id_Tipo_Gasto ? true : false
                };
                items.Add(item);
            }
            return items;
        }
        #endregion
        #region Tipo_Otro_Ingreso
        private async Task<List<SelectListItem>> Tipos_Otros_Ingresos(int id_seleccionado = 0)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Tipos_Otro_Ingreso");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Tipo_Ingreso> tipos = JsonConvert.DeserializeObject<List<Tipo_Ingreso>>(contenido);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Tipo_Ingreso tipo in tipos)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = tipo.Descripcion,
                    Value = tipo.Id_Tipo_Ingreso.ToString(),
                    Selected = id_seleccionado == tipo.Id_Tipo_Ingreso ? true : false
                };
                items.Add(item);
            }
            return items;
        }
        #endregion


        public async Task<IActionResult> Calificacion(int id_evaluacion)
        {
            string recuroyparametro = string.Format("Evaluacion_Tecnica/Calificacion?id_evaluacion={0}", id_evaluacion);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            Calificacion_Vista calificacion = JsonConvert.DeserializeObject<Calificacion_Vista>(contenido);
            Encabezado_Pie_Pagina epp = new Encabezado_Pie_Pagina();
            return new ViewAsPdf("Calificacion", calificacion) { CustomSwitches = epp.Encabezado()};
        }
    }
}