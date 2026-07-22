using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

//using Microsoft.Rest;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// manejo de cuentas
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Account")]
    public class AccountController : Controller
    {
        private readonly NegocioContexto _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        /// <summary>
        /// injeccion de componentes
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        /// <param name="roleManager"></param>
        /// <param name="properties"></param>
        /// /// <param name="context"></param>
        public AccountController(

            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            NegocioContexto context

            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Post Es para Autenticarse
        /// </summary>
        /// <param name="model">LoginDto con User y Password</param>
        /// <returns>retorna un token para utilizarlo en futuros request</returns>

        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.User, model.Password, false, false);
                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.User);
                    var roles = await _userManager.GetRolesAsync(appUser);
                    return await GenerateJwtToken(model.User, appUser, roles);
                }
                return await Task.Run(() => BadRequest());
            } catch (Exception ex)
            {
                //string modelo_toma = string.Format("Mensaje del error: {0} Pista: {1}", ex.Message, ex.StackTrace);
                //Helper.Bitacora.Escribir(modelo_toma);
                return await Task.Run(() => BadRequest());
            }
        }

        /*administracion de los usuarios que se crean*/
        /// <summary>
        /// Post Se crea el usuario
        /// </summary>
        /// <param name="model">RegisterDto para registrar un usuario</param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task Register([FromBody] RegisterDto model)
        {
          await  Crear_Usuario(model);
        }

        [Authorize(Roles = "Asistente,Jefe,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Crear_Usuario_Socio(int id_persona)
        {
       
            Persona persona = await _context.Personas.AsNoTracking().SingleAsync(a => a.Id_Persona == id_persona);
            //obtener dui
            string cadena = persona.Dui;
            // separar dui por guiones
            string[] valores = cadena.Split("-");
            //primera dimencion
            string celda1 = valores[0].Trim();
            //segunda dimencion
            string celda2 = valores[1].Trim();
            //armar dui sin guiones
            string user = celda1 + celda2;
            string email = string.Empty;
            //validar email, sino tiene se crea con el numero de dui
            if (persona.Correo == "")
                email = persona.Dui + "@" + persona.Dui + ".com";
            //obtener contraseña generada
            string password = await Generar_Contraseña_Socio();
            if (string.IsNullOrEmpty(persona.Usuario))
            {
                // se crea una instancia del modelo y se llena con los campos
                var model = new RegisterDto();
                model.Email = email;
                model.Password = password;
                model.User = user;
                //se guarda el usuario
                await Crear_Usuario(model);
                await AddRole(user, "Socio");
                persona.Usuario = user;
                _context.Entry(persona).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                Registro_Socio account = new Registro_Socio
                {
                    Nombre_completo = persona.NombreCompleto,
                    user = persona.Usuario,
                    password = password
                };
                return Ok(account);
            }
            else
            {
                CambiarContrasenaAdmin nueContrasena = new CambiarContrasenaAdmin
                {
                    Id_Usuario = user,
                    Contrasena = password
                };
                await ChangePassword(nueContrasena);
                Registro_Socio account = new Registro_Socio
                {
                    Nombre_completo = persona.NombreCompleto,
                    user = persona.Usuario,
                    password = password
                };
                return Ok(account);
            }

          
        }
        /// <summary>
        /// Es para cambiar la contrasena de un usuario dado
        /// </summary>
        /// <param name="cambiar">CambiarContrasenaAdmin</param>
        /// <returns>200</returns>
        [Authorize(Roles = "Administrador,Asesor,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] CambiarContrasenaAdmin cambiar)
        {
            string usuario_autenticado = _userManager.GetUserId(HttpContext.User);
            IdentityUser appUser_enviado = _userManager.Users.SingleOrDefault(r => r.UserName == cambiar.Id_Usuario);
            IdentityUser appUser_autenticado = _userManager.Users.SingleOrDefault(r => r.UserName == usuario_autenticado);
            if (await _userManager.IsInRoleAsync(appUser_autenticado, "Jefe") && await _userManager.IsInRoleAsync(appUser_enviado, "Socio"))
            {
                if (await ManagePassWord(appUser_enviado, cambiar.Contrasena))
                    return Ok();
            }
            if (usuario_autenticado == cambiar.Id_Usuario)
            {
                if (await ManagePassWord(appUser_enviado, cambiar.Contrasena))
                    return Ok();
            }
            else {
                if (await _userManager.IsInRoleAsync(appUser_autenticado, "Administrador"))
                {
                    if (await ManagePassWord(appUser_enviado, cambiar.Contrasena))
                        return Ok("Contraseña Editada Exitosamente para " + appUser_enviado);
                }
            }
            return BadRequest("Error al cambiar la contraseña");
        }

        private async Task<bool> ManagePassWord(IdentityUser appUser, string contrasena)
        {
            string token_reset = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            IdentityResult result = await _userManager.ResetPasswordAsync(appUser, token_reset, contrasena);
            return result.Succeeded;
        }

        /// <summary>
        /// Get Se obtiene la lista de usuarios creados en el sistema
        /// </summary>
        /// <returns>Muestra la lista de usuarios</returns>
        /// 
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IQueryable<Usuario> ListaUsuarios(string busqueda)
        {
            return _userManager.Users
                .Select(a =>
                    new Usuario
                    {
                        UsuarioNombre = a.UserName,
                        Correo = a.Email
                    }).Where(r =>
                        r.UsuarioNombre.ToUpper().Contains(busqueda.ToUpper()) ||
                        r.Correo.ToUpper().Contains(busqueda.ToUpper()));
        }

        private async Task<object> GenerateJwtToken(string email, IdentityUser user, IList<string> role)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            /*agregar los roles del usuario logueado*/
            foreach (var item in role)
                claims.Add(new Claim(ClaimTypes.Role, item));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(9),
                signingCredentials: creds
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            //tokenHandler.TokenLifetimeInMinutes = (int) TimeSpan.FromMinutes(2).TotalMinutes;
            return tokenHandler.WriteToken(token);
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task Logout(string t)
        {
            AuthorizeAttribute au = new AuthorizeAttribute();
            au.TypeId.ToString();
            var headers = HttpContext.Request.Headers.ToList();
            string token = "";
            foreach (KeyValuePair<string, StringValues> header in headers)
            {
                if (header.Key == "Authorization")
                {
                    token = header.Value;
                    break;
                }
            }
            token = token.Replace("Bearer ", "");
            JwtSecurityToken seguridad = new JwtSecurityTokenHandler().ReadJwtToken(token) as JwtSecurityToken;
            //SecurityToken st = new JwtSecurityTokenHandler().ReadToken(token) as SecurityToken; 
            //st.ex
            //new JwtSecurityTokenHandler().WriteToken();
        }

        /*manejo de roles para los usuarios*/
        /// <summary>
        /// Post Pide el usuario y role para asignarlo
        /// </summary>
        /// <param name="usuario">string id de usuario</param>
        /// <param name="rol">string el rol que se agrega al usuario</param>
        ///
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> AddRole(string usuario, string rol) {
            IdentityUser user = await _userManager.FindByNameAsync(usuario);
            if (await _userManager.IsInRoleAsync(user, rol))
            {
                return BadRequest("Ya existe este Rol");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, rol);
                return Ok("El Rol Fue agregado");
            }

        }

        /// <summary>
        /// Get Lista de roles para el sistema
        /// </summary>
        /// <returns>Lista de Roles</returns>
        ///
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IQueryable<Role> Roles()
        {
            return _roleManager.Roles.Select(a => new Role { RoleNombre = a.Name, Descripcion = a.NormalizedName });
        }

        /// <summary>
        /// Get Los roles asignados a un usuario dado
        /// </summary>
        /// <param name="usuario">string id usuario</param>
        /// <returns>Lista de solo los nombres</returns>
        ///
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<List<Role>> RolesAsignados(string usuario)
        {
            var user = await _userManager.FindByNameAsync(usuario);
            List<Role> roles = new List<Role>();
            var rolnombres = await _userManager.GetRolesAsync(user);
            foreach (string nombre in rolnombres)
                roles.Add(new Role { RoleNombre = nombre });
            return roles;
        }

        /// <summary>
        /// Post Para borrar el rol del usuario enviado
        /// </summary>
        /// <param name="usuario">string id usuario al que se le borrará el rol</param>
        /// <param name="rol">string rol que se borrará</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> BorrarRolAsignado(string usuario, string rol)
        {
            var user = await _userManager.FindByNameAsync(usuario);
            await _userManager.RemoveFromRoleAsync(user, rol);
            return Ok("Rol Eliminado Exitosamente");
        }

        /// <summary>
        /// Post Asigna una agencia al usuario creado
        /// </summary>
        /// <param name="usuario">string id de usuario al que se le asigna una agencia</param>
        /// <param name="id_agencia">int id de la agencia que se agregará</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> AsignarAgencia(
            string usuario,
            int id_agencia)
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                if (!db.Usuario_Agencia.Any(a => a.Id_Usuario == usuario))
                    db.Usuario_Agencia.Add(new Usuario_Agencia
                    {
                        Id_Usuario = usuario,
                        Id_Agencia = id_agencia
                    });
                else
                {
                    Usuario_Agencia ua = db.Usuario_Agencia.Single(a => a.Id_Usuario == usuario);
                    ua.Id_Agencia = id_agencia;
                    db.Entry(ua).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                await db.SaveChangesAsync();
                return Ok("Agencia Asignada Exitosamente");
            }
        }

        /// <summary>
        /// Post Asigna nombre real al usuario creado
        /// </summary>
        /// <param name="usuario">string id de usuario al que se le asigna una agencia</param>
        /// <param name="nombre_completo">string id de la agencia que se agregará</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Nombre_Real_Usuario(
            string usuario,
            string nombre_completo)
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                if (!db.Usuario_Agencia.Any(a => a.Id_Usuario == usuario))
                    db.Usuario_Agencia.Add(new Usuario_Agencia
                    {
                        Id_Usuario = usuario,
                        Nombre_Completo = nombre_completo
                    });
                else
                {
                    Usuario_Agencia ua = db.Usuario_Agencia.Single(a => a.Id_Usuario == usuario);
                    ua.Nombre_Completo = nombre_completo;
                    db.Entry(ua).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                await db.SaveChangesAsync();
                return Ok("Agencia Asignada Exitosamente");
            }
        }

        /// <summary>
        /// obtener el nombre real del usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Obtener_Nombre_Real(string usuario)
        {
            var usuarioAgencia= await _context
                .Usuario_Agencia
                .SingleAsync(d=>d.Id_Usuario==usuario);
            return Ok(usuarioAgencia.Nombre_Completo);
        }

        /// <summary>
        /// Get Mostrará la agencia asignada de determinado usuario
        /// </summary>
        /// <param name="usuario">string id de usuario</param>
        /// <returns>muestra la agencia al que pertenece el usuario dado</returns>
        /// 
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public int AgenciaAsignadaUsuario(string usuario)
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                if (db.Usuario_Agencia.Any(a => a.Id_Usuario == usuario))
                    return db.Usuario_Agencia.Single(a => a.Id_Usuario == usuario).Id_Agencia;
                else
                    return 0;
            }
        }

        /// <summary>
        /// solo es para verificar
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Verificar()
        {
            string user = User.Identity.Name.ToString();
            return Ok(await Task.Run(() => user));
        }

        /// <summary>
        /// Get La lista de las agencias existentes
        /// </summary>
        /// <returns>muestra todas las agencias existentes</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public List<Agencia> Agencias()
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                return db.Agencia.ToList();
            }
        }
        /// <summary>
        /// Get La lista de las usuarios existentes
        /// </summary>
        /// <returns>muestra todas los usuarios existentes</returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public List<Usuario> Usuarios()
        {
            return _userManager.Users.Select(a =>
                new Usuario
                {
                    UsuarioNombre = a.UserName,
                    Correo = a.Email
                }).ToList();
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Nombre_Empresa()
        {
            string valor = "";

            bool tiene_empresa = await _context.Configuracion_General.AnyAsync(w => w.Nombre == "NOMBRE_EMPRESA");
            if (tiene_empresa)
            {
                Configuracion_General registro_empresa = await _context.Configuracion_General.SingleAsync(w => w.Nombre == "NOMBRE_EMPRESA");
                return Ok(registro_empresa.Valor.Replace('"', ' '));
            }
            return BadRequest("No se encontró un valor para la variable");
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> LogoGrande_Empresa()
        {
            string valor = "";

            bool tiene_empresa = await _context.Configuracion_General.AnyAsync(w => w.Nombre == "logo_grande");
            if (tiene_empresa)
            {
                Configuracion_General registro_empresa = await _context.Configuracion_General.SingleAsync(w => w.Nombre == "logo_grande");
                return Ok(registro_empresa.Valor);
            }
            return BadRequest("No se encontró un valor para la variable");
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> LogoPequeño_Empresa()
        {
            string valor = "";

            bool tiene_empresa = await _context.Configuracion_General.AnyAsync(w => w.Nombre == "logo_pequeño");
            if (tiene_empresa)
            {
                Configuracion_General registro_empresa = await _context.Configuracion_General.SingleAsync(w => w.Nombre == "logo_pequeño");
                return Ok(registro_empresa.Valor);
            }
            return BadRequest("No se encontró un valor para la variable");
        }

        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IActionResult Menu_Rol()
        {
            List<Menu_Categoria_vista> menu = _context.MenuCategoriaVista
                .Include(d => d.MenuRolCategorias).ToList();
            //List<Menu_Contenido_Vista> menu_contenido = menu.SelectMany(r => r.Menu_Contenido).ToList();

            List<Menu_Categoria_vista> menu_filtrado = new List<Menu_Categoria_vista>();
            string id_usuario = _userManager.GetUserId(HttpContext.User);
            foreach (Menu_Categoria_vista me in menu)
            {
                if (User.IsInRole("Administrador"))
                {
                    if (me.MenuRolCategorias.Any(d => d.Rol == "Administrador"))
                    {
                        bool tiene_caja = _context.Caja.Any(d=>d.Id_Usuario== id_usuario && d.Activa && !d.Boveda);
                        Func<Menu_Rol_Contenido, bool> filtrar_caja = new Func<Menu_Rol_Contenido, bool>(q => q.Rol == "Administrador");
                        
                        List<Menu_Contenido_Vista> contenido_filtrados = new List<Menu_Contenido_Vista>();

                        if (!tiene_caja)
                        {
                            contenido_filtrados = _context
                            .MenuContenidoVista
                            .Include(z => z.MenuRolContenidos)
                            .Where(a => a.MenuRolContenidos
                            .Any(
                                    q => q.Rol == "Administrador" && 
                                    q.Id_Menu_Contenido_Vista != 9 &&
                                    q.Id_Menu_Contenido_Vista != 10 &&
                                    q.Id_Menu_Contenido_Vista != 45)).ToList();
                        }
                        else {
                            contenido_filtrados = _context
                            .MenuContenidoVista
                            .Include(z => z.MenuRolContenidos)
                            .Where(a => a.MenuRolContenidos.Any(q => q.Rol == "Administrador")).ToList();
                        }
                        List<Menu_Contenido_Vista> contenido_filtrado_viendo_Caja = contenido_filtrados.ToList();
                        me.Menu_Contenido.ToList().AddRange(contenido_filtrado_viendo_Caja);
                    }
                    menu_filtrado.Add(me);
                }
                if (User.IsInRole("Supervisor"))
                {
                    if (me.MenuRolCategorias.Any(d => d.Rol == "Supervisor"))
                    {
                        List<Menu_Contenido_Vista> contenido_filtrados = _context
                            .MenuContenidoVista
                            .Include(z => z.MenuRolContenidos)
                            .Where(a => a.MenuRolContenidos.Any(q => q.Rol == "Supervisor")).ToList();
                        me.Menu_Contenido.ToList().AddRange(contenido_filtrados);
                    }
                    menu_filtrado.Add(me);
                }
                if (User.IsInRole("Asistente"))
                {
                    if (me.MenuRolCategorias.Any(d => d.Rol == "Asistente"))
                    {
                        List<Menu_Contenido_Vista> contenido_filtrados = _context
                            .MenuContenidoVista
                            .Include(z => z.MenuRolContenidos)
                            .Where(a => a.MenuRolContenidos.Any(q => q.Rol == "Asistente")).ToList();
                        me.Menu_Contenido.ToList().AddRange(contenido_filtrados);
                    }
                    menu_filtrado.Add(me);
                }
                if (User.IsInRole("Gerente"))
                {
                    if (me.MenuRolCategorias.Any(d => d.Rol == "Gerente"))
                    {
                        List<Menu_Contenido_Vista> contenido_filtrados = _context
                            .MenuContenidoVista
                            .Include(z => z.MenuRolContenidos)
                            .Where(a => a.MenuRolContenidos.Any(q => q.Rol == "Gerente")).ToList();
                        me.Menu_Contenido.ToList().AddRange(contenido_filtrados);
                    }
                    menu_filtrado.Add(me);
                }
                if (User.IsInRole("Jefe"))
                {
                    if (me.MenuRolCategorias.Any(d => d.Rol == "Jefe"))
                    {
                        List<Menu_Contenido_Vista> contenido_filtrados = _context
                            .MenuContenidoVista
                            .Include(z => z.MenuRolContenidos)
                            .Where(a => a.MenuRolContenidos.Any(q => q.Rol == "Jefe")).ToList();
                        me.Menu_Contenido.ToList().AddRange(contenido_filtrados);
                    }
                    menu_filtrado.Add(me);
                }
                if (User.IsInRole("Asesor"))
                {
                    if (me.MenuRolCategorias.Any(d => d.Rol == "Asesor"))
                    {
                        List<Menu_Contenido_Vista> contenido_filtrados = _context
                            .MenuContenidoVista
                            .Include(z => z.MenuRolContenidos)
                            .Where(a => a.MenuRolContenidos.Any(q => q.Rol == "Asesor")).ToList();
                        me.Menu_Contenido.ToList().AddRange(contenido_filtrados);
                    }
                    menu_filtrado.Add(me);
                }
            }
            //menu_filtrado.SelectMany(t => t.Menu_Contenido).ToList()
            //.RemoveAll(s=>s.MenuRolContenidos != null && s.MenuRolContenidos.Any(d=>d.Rol!="Asesor"));

            return Ok(menu_filtrado);
        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> MocapMenu()
        {

            try
            {
                if (User.IsInRole("Administrador"))
                {
                    List<Menu_Categoria_vista> menuCategoria = _context.MenuCategoriaVista.Include(
                        a => a.Menu_Contenido).ToList();
                    return Ok(menuCategoria);
                }

                return Ok("No tiene roles");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "Administrador,Gerente,Jefe,Asistente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> RolPorUsuario()
        {
            var id_usuario = _userManager.GetUserId(HttpContext.User);
            IdentityUser user = await _userManager.FindByNameAsync(id_usuario);
            var rolnombres = await _userManager.GetRolesAsync(user);
            return Ok(rolnombres.FirstOrDefault().ToString());
        }

        [Authorize(Roles = "Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IActionResult ObtenerAgencia()
        {
            var id_usuario = _userManager.GetUserId(HttpContext.User);
            Agencia agencia = _context.Usuario_Agencia
                .Include(a => a.Agencia)
                .SingleOrDefault(a => a.Id_Usuario == id_usuario).Agencia;
            return Ok(agencia);
        }
        [Authorize(Roles = "Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IActionResult ObtenerAgenciaPorCartera(int id_cartera)
        {
            Cartera cartera = _context.Cartera.AsNoTracking().Single(a=>a.Id_Cartera== id_cartera);
            Agencia agencia = _context.Usuario_Agencia
                .Include(a => a.Agencia)
                .SingleOrDefault(a => a.Id_Usuario == cartera.Id_Usuario)
                ?.Agencia;
            return Ok(agencia);
        }
        private async Task<object> Crear_Usuario([FromBody] RegisterDto model)
         {
         
             var user = new IdentityUser
             {
                 UserName = model.User,
                 Email = model.Email
             };
             var result = await _userManager.CreateAsync(user, model.Password);

             if (result.Succeeded)
             {
                 return Ok();
                 //await _signInManager.SignInAsync(user, false);
                 //return await GenerateJwtToken(model.Email, user);
             }

             throw new ApplicationException("UNKNOWN_ERROR");
        }

        private async Task<string> Generar_Contraseña_Socio()
        {
        
            Random rdn = new Random();
            string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string caracteres2 = "abcdefghijklmnopqrstuvwxyz";
            int longitud = caracteres.Length;
            char letra, letra2;
            int longitudPass = 3;
            string contrasenaSocio2 = string.Empty;
            string contrasenaSocio = string.Empty;
        
            DateTime dia = DateTime.Today.Date;
            for (int i = 0; i < longitudPass; i++)
            {
                //letras mayusculas
                letra = caracteres[rdn.Next(longitud)];
                contrasenaSocio += letra.ToString();
                //letras minusculas
                letra2 = caracteres2[rdn.Next(longitud)];
                contrasenaSocio2 += letra2.ToString();
            }
     
            return contrasenaSocio+contrasenaSocio2+"_"+dia.Month;
        }
    }

    public class LoginDto
        {
            [Required]
            public string User { get; set; }
            //[Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

        }
        
        public class RegisterDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string User { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }
        }

        
    }
