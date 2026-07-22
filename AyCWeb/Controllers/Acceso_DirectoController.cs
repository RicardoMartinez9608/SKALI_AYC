using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model;
using Newtonsoft.Json;

namespace AyCWeb.Controllers
{
    public class Acceso_DirectoController : BaseController
    {
        [HttpGet]
        public IActionResult Index()/*Es el menu disponible para el usuario*/
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Opciones()
        {
            string recuroyparametro = string.Format("Account/Menu_Rol");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            List<Menu_Categoria_vista> menu_filtrado = JsonConvert.DeserializeObject<List<Menu_Categoria_vista>>(contenido);
            return PartialView(menu_filtrado);
        }

        [HttpGet]
        public IActionResult NuevaCategoria()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> NuevaCategoria(Categoria_Acceso_Directo categoria)
        {
            string jsonstring = JsonConvert.SerializeObject(categoria).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, "Acceso_Directo/Crear_Categoria_Acceso_Directo", jsonstring);
            if (responseApi.Satisfactorio)
                return Ok("Se ha agregado nuevo categoría de atajos");
            else
                return BadRequest("No se pudo crear la categoría de atajos");
        }


        private async Task<List<Categoria_Acceso_Directo>> Atajos()
        {
            string recuroyparametro = string.Format("Acceso_Directo/Accesos_Directos");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            string contenido = responseApi.ContenidofromJson;
            return JsonConvert
                .DeserializeObject<List<Categoria_Acceso_Directo>>(contenido);
        }

        [HttpGet]
        public async Task<IActionResult> Accesos()
        {
            List<Categoria_Acceso_Directo> atajos = await Atajos();
            return PartialView(atajos);
        }

        [HttpGet]
        public async Task<IActionResult> Atajos_Detalle() 
        {
            List<Categoria_Acceso_Directo> atajos = await Atajos();
            return PartialView(atajos);
        }

        [HttpGet]
        public async Task<IActionResult> Mis_Atajos()
        {
            List<Categoria_Acceso_Directo> atajos = await Atajos();
            return PartialView(atajos);
        }

        [HttpPost]
        public async Task<IActionResult> Crear_Acceso(Acceso_Directo acceso)
        {
            string jsonstring = JsonConvert.SerializeObject(acceso).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, "Acceso_Directo/Crear_Acceso_Directo", jsonstring);
            if (responseApi.Satisfactorio)
                return Ok("Se ha agregado nuevo atajo");
            else
                return BadRequest("No se pudo crear un atajo");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar_Acceso(int id_acceso_directo)
        {
            string recuroyparametro = string
                .Format("Acceso_Directo/Eliminar_Acceso_Directo?id_acceso_directo={0}", id_acceso_directo);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok("Se ha eliminado el atajo");
            else
                return BadRequest("No se pudo eliminar el atajo");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar_Categoria(int id_categoria)
        {
            string recuroyparametro = string
                .Format("Acceso_Directo/Eliminar_Categoria_Acceso_Directo?id_categoria={0}", id_categoria);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok("Se ha eliminado la categoría");
            else
                return BadRequest("No se pudo eliminar la categoría");
        }
    }
}
