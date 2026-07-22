using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AyCWeb.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using Newtonsoft.Json;
using NucleoAyC.Entities;

namespace AyCWeb.Controllers
{
    public class OpcionesController : BaseController
    {
        public async  Task<IActionResult> Index()
        {
            string recuroyparametro = null;
            recuroyparametro = string.Format("Opciones_Web/GetMenuCategoriaVista");
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Menu_Categoria_vista> menuContenido = JsonConvert.DeserializeObject<List<Menu_Categoria_vista>>(contenido);
            return View(menuContenido);
        }
        [HttpGet]
        public async Task<IActionResult> Contenidos(int idcategoria)
        {
            string recuroyparametro = null;
            recuroyparametro = string.Format("Opciones_Web/GetContenidoporID?idcategoria={0}", idcategoria);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Menu_Contenido_Vista> menuContenido = JsonConvert.DeserializeObject<List<Menu_Contenido_Vista>>(contenido);
            return PartialView(menuContenido);

        }

        [HttpGet]
        public async Task<IActionResult> Editar_Categoria(int idcategoria)
        {
            string recuroyparametro = string.Format("Opciones_Web/DetalleCategoria?idcategoria={0}", idcategoria);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);

            string contenido = responseApi.ContenidofromJson;
            Menu_Categoria_vista menuCategoria = JsonConvert.DeserializeObject<Menu_Categoria_vista>(contenido);
            return PartialView(menuCategoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Categoria_Exe(Menu_Categoria_vista categoria)
        {
            string recuroyparametro = string.Format("Opciones_Web/Editar");
            string jsonstring = JsonConvert.SerializeObject(categoria).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<IActionResult> Editar_Contenido(int idcontenido)
        {
            string recuroyparametro = string.Format("Opciones_Web/DetalleContenido?idcontenido={0}", idcontenido);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);

            string contenido = responseApi.ContenidofromJson;
            Menu_Contenido_Vista menuContenido = JsonConvert.DeserializeObject<Menu_Contenido_Vista>(contenido);
            return PartialView(menuContenido);
        }

        [HttpPost]
        public async Task<IActionResult> Editar_Contenido_Exe(Menu_Contenido_Vista contenido)
        {
            string recuroyparametro = string.Format("Opciones_Web/EditarContenido");
            string jsonstring = JsonConvert.SerializeObject(contenido).Replace("\"", "'");
            ResponseApi responseApi = await OperarApi(HttpMethod.Put, recuroyparametro, jsonstring);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }

        [HttpGet]
        public async Task<ActionResult> AgregarRoles()
        {
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, "Account/Roles", null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(contenido);

            ViewBag.Rol = new SelectList(roles, "RoleNombre", "RoleNombre");
        //    ViewBag.Usuario = usuario;
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> RolporCategoria(int idcategoria)
        {
            string recuroyparametro = null;
            recuroyparametro = string.Format("Opciones_Web/RolesporCategoria?idcategoria={0}",idcategoria);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Menu_Rol_Categoria> menurol = JsonConvert.DeserializeObject<List<Menu_Rol_Categoria>>(contenido);
            return PartialView(menurol);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarRolCategoria(int idcategoria,string rol)
        {
            string recuroyparametro = null;
            recuroyparametro = string.Format("Opciones_Web/AgregarRolCategoria?idcategoria={0}&rol={1}", idcategoria,rol);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarRolCategoria(int idrol)
        {
            string recuroyparametro = string.Format("Opciones_Web/BorrarRolCategoria?idrol={0}", idrol);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        /*--------------------------------------------------metodos contenido--------------------------------------------------------- */
        [HttpGet]
        public async Task<IActionResult> RolporContenido(int idcontenido)
        {
            string recuroyparametro = null;
            recuroyparametro = string.Format("Opciones_Web/RolesporContenido?idcontenido={0}", idcontenido);
            ResponseApi responseApi = await OperarApi(HttpMethod.Get, recuroyparametro, null);
            /*convertir el json por ahora*/
            string contenido = responseApi.ContenidofromJson;
            List<Menu_Rol_Contenido> menurol = JsonConvert.DeserializeObject<List<Menu_Rol_Contenido>>(contenido);
            return PartialView(menurol);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarRolContenido(int idcontenido, string rol)
        {
            string recuroyparametro = null;
            recuroyparametro = string.Format("Opciones_Web/AgregarRolContenido?idcontenido={0}&rol={1}", idcontenido, rol);
            ResponseApi responseApi = await OperarApi(HttpMethod.Post, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarRolContenido(int idrol)
        {
            string recuroyparametro = string.Format("Opciones_Web/BorrarRolContenido?idrol={0}", idrol);
            ResponseApi responseApi = await OperarApi(HttpMethod.Delete, recuroyparametro, null);
            if (responseApi.Satisfactorio)
                return Ok(responseApi.ContenidofromJson);
            else
                return BadRequest(responseApi.ContenidofromJson);
        }
    }

}