using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Manejo de cómite
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Comites")]
    public class ComitesController : Controller
    {
        private readonly NegocioContexto _context;
        private readonly UserManager<IdentityUser> _userManager;
        /// <summary>
        /// Constructor del controlador Comités
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public ComitesController(NegocioContexto context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Get Obtiene el comité por id de crédito
        /// </summary>
        /// <param name="id_credito">int id de crédito</param>
        /// <returns>Comité</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetComitePorCredito(int id_credito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comite = await _context.Comite.SingleOrDefaultAsync(m => m.Id_Credito == id_credito);

            if (comite == null)
            {
                return NotFound();
            }

            return Ok(comite);
        }

        // PUT: api/Comites/5
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComite([FromRoute] int id, [FromBody] Comite comite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comite.Id_Comite)
            {
                return BadRequest("No se encontró el registro que se intenta editar");
            }

            _context.Entry(comite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComiteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Post Se creará el comité para un crédito
        /// </summary>
        /// <param name="id_credito">int id de crédito</param>
        /// <returns>Lista de usuarios</returns>
        /// 
        //[Authorize(Roles="Administrador,Asesor,Gerente,Jefe",AuthenticationSchemes ="Bearer")]
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> ComiteConformado(int id_credito)
        {
            Credito credito = _context.Credito.AsNoTracking().Single(q=>q.Id_Credito==id_credito);
            if (credito.En_Comite == false)
            {
                //Persona persona = _context.Personas.AsNoTracking().Single(s=>s.Id_Persona==credito.Id_Persona);
                //int id_cartera = credito.Id_Cartera;
                string id_usuario = _context.Cartera.SingleOrDefault(d => d.Id_Cartera == credito.Id_Cartera).Id_Usuario;/*es el asesor asignado*/
                /*agregar al asesor asignado*/
                await AgregarAsesorAsignadoComite(id_usuario, id_credito);
                int id_agencia = _context.Usuario_Agencia.SingleOrDefault(b => b.Id_Usuario == id_usuario).Id_Agencia;
               
                List<string> ids_usuarios = _context.Usuario_Agencia.Where(l => l.Id_Agencia == id_agencia).Select(d => d.Id_Usuario).ToList();
                foreach (string id in ids_usuarios)
                {
                    var user = await _userManager.FindByNameAsync(id);
                    if (user != null)
                    {
                        var role = await _userManager.GetRolesAsync(user);
                        if (role.FirstOrDefault() == "Jefe" || role.FirstOrDefault() == "Gerente")
                        {
                            /*falta la implementación de la entidad Configuracion_Comite_Rol*/
                            List<Configuracion_Comite_Rol> comite_roles = _context.Configuracion_Comite_Rol.AsNoTracking().Where(c => c.Ampliado == false).ToList();
                            foreach (Configuracion_Comite_Rol comite_rol in comite_roles)
                                await AgregarComiteComun(comite_rol, id_credito, id);
                        }
                    }

                }
                /*auxiliamos de la configuracion general para obtener el usuario asignado para celebrar comité ampliado*/
             //   await AgregarComiteAmpliadoConfiguracion(id_credito);

                /*actualizar el credito indicando que ya esta en documento*/
                Credito credito_actualizar = _context.Credito.SingleOrDefault(c => c.Id_Credito == id_credito);
                credito_actualizar.En_Comite = true;
                _context.Entry(credito_actualizar).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok( ids_usuarios);
            }

            return BadRequest("El Crédito ya esta sometido a Comité");
        }

        private async Task AgregarAsesorAsignadoComite(string id_usuario,int id_credito)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == id_usuario);
            
            if (/*await _userManager.IsInRoleAsync(appUser, "Asesor") &&*/
                !_context.Comite.Any(c => c.Id_Credito == id_credito && c.Id_Usuario == id_usuario))
            {
                _context.Comite.Add(new Comite
                {
                    Id_Credito = id_credito,
                    Id_Usuario = id_usuario,
                    Ampliado = false,
                    Verificado = false
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task AgregarComiteComun(Configuracion_Comite_Rol comite_rol, int id_credito,string id_usuario)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == id_usuario);
            if (appUser != null)
            {
                if (await _userManager.IsInRoleAsync(appUser, comite_rol.Rol_Nombre) &&
                    !_context.Comite.Any(c => c.Id_Credito == id_credito && c.Id_Usuario == id_usuario))
                {
                    _context.Comite.Add(new Comite
                    {
                        Id_Credito = id_credito,
                        Id_Usuario = id_usuario,
                        Ampliado = false,
                        Verificado = false
                    });
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task AgregarComiteAmpliadoConfiguracion(int id_credito)
        {
            /*auxiliamos de la configuracion general para obtener el usuario asignado para celebrar comité ampliado*/
            Configuracion_General valor_sistema =await _context
                .Configuracion_General
                .SingleOrDefaultAsync(w => w.Nombre == "COMITE_AMPLIADO");
            bool comite_ampliado_agregado = _context.Comite
                .AsNoTracking()
                .Any(
                    a=>a.Id_Credito==id_credito && 
                    a.Id_Usuario==valor_sistema.Valor &&
                    a.Ampliado);
            if (valor_sistema.Tipo == "STRING" && !comite_ampliado_agregado)
            {
                /*convertir lista*/
                string[] ampliados = valor_sistema.Valor.Split(";");
                for (int i = 0; i < ampliados.Length; i++)
                {
                    /*si ya existe se cambia a ampliado y no se agrega*/
                    string usuario_ampliado = ampliados[i].TrimEnd();
                    Func<Comite, bool> filtro_comite = new Func<Comite, bool>(d => d.Id_Usuario == usuario_ampliado && d.Id_Credito==id_credito);
                    bool ya_existe = _context.Comite.Any(filtro_comite);
                    if (ya_existe)/*la actualizo*/
                    {
                        Comite comite_existente = _context.Comite.Single(filtro_comite);
                        comite_existente.Ampliado = true;
                        _context.Comite.Update(comite_existente).State = EntityState.Modified;
                    }
                    else
                    {
                        _context.Comite.Add(new Comite
                        {
                            Id_Credito = id_credito,
                            Id_Usuario = usuario_ampliado,
                            Ampliado = true,
                            Verificado = false
                        }).State=EntityState.Added;
                    }
                }
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Get Mostrara la lista de los comite a aprobar
        /// </summary>
        /// <returns>Lista de comités</returns>
        /// 
        [Authorize(Roles = "Administrador,Asesor,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public List<Comite> ComitePorUsuarioPendiente(bool verificado=false)
        {
            /*se detecta el usuario logueado para mostrarle */
            var userId = _userManager.GetUserId(HttpContext.User);
            return _context.Comite.Where(t=>t.Id_Usuario==userId && t.Verificado == verificado).ToList();
        }

        /// <summary>
        /// Get Obtiene el detalle del comité con todos los datos necesarios para formar un juicio
        /// </summary>
        /// <param name="id_comite">int id de comité</param>
        /// <returns>Comité objeto json</returns>
        /// 
        [Authorize(Roles = "Administrador,Asesor,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public Comite ComiteActivo(int id_comite)
        {
            return _context.Comite.SingleOrDefault(t =>t.Id_Comite==id_comite);
        }

        /// <summary>
        /// Get Obtener los comités de un crédito
        /// </summary>
        /// <param name="id_credito">int id de crédito</param>
        /// <returns>Lista de comités</returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public List<Comite> ComitesPorCredito(int id_credito)
        {
            return _context.Comite.Where(t => t.Id_Credito == id_credito).ToList();
        }

        /// <summary>
        /// Post Es para aprobar el comité por usuario
        /// </summary>
        /// <param name="id_comite">int id de comite</param>
        /// <param name="aprobado">bool si es aprobado</param>
        /// <param name="comentario">string un comentario</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> VerificadoComite(int id_comite,bool aprobado,string comentario)
        {
            try
            {
                Comite comiteParaActualizar = _context.Comite.SingleOrDefault(d => d.Id_Comite == id_comite);
                Credito credito_comite = _context.Credito.Single(a=>a.Id_Credito == comiteParaActualizar.Id_Credito);
                List<Comite> lista_comites = _context.Comite
                    .Where(b => b.Id_Credito == credito_comite.Id_Credito).ToList();

                credito_comite.Aprobado_En = DateTime.Now;
                credito_comite.Aprobado = true;
                _context.Entry(credito_comite).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                //foreach (Comite comites in lista_comites)
                //{
                //    if (comites.Verificado != true)
                //    {
                //        comites.Verificado = true;
                //        comites.Fecha_Verificado = DateTime.Now;
                //        comites.Aprobado = aprobado;
                //        comites.Comentario = comentario;
                //        await _context.SaveChangesAsync();
                // await AprobandoCredito(credito_comite.Id_Credito);
                //    }
                //}


                return Ok("El registro fue actualizado correctamente");
            } catch (Exception ex )
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Post Es para aprobar el comité por usuario
        /// </summary>
        /// <param name="id_comite">int id de comite</param>
        /// <param name="aprobado">bool si es aprobado</param>
        /// <param name="comentario">string un comentario</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> CrearComiteAmplido(int id_credito)
        {
            try
            {
                bool tiene_comite = _context.Comite_Ampliados.Any(a=>a.Id_Credito ==id_credito);
                await AgregarComiteAmpliadoConfiguracion(id_credito);
                if (!tiene_comite) {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    Comite_Ampliado comite_Ampliado = new Comite_Ampliado
                    {
                        Id_Credito = id_credito,
                        Creado_En = DateTime.Now.Date,
                        Creado_Por = userId
                    };
                    _context.Comite_Ampliados.Add(comite_Ampliado);
                    await _context.SaveChangesAsync();
                    var id_comite_ampliado = _context.Comite_Ampliados.ToList().LastOrDefault();
                    Credito credito = _context.Credito
                        .Include(l=>l.Linea_Credito)
                        .Single(a=>a.Id_Credito == id_credito);
                    CreditoComiteAmpliado creditoComiteAmpliado = new CreditoComiteAmpliado { 
                    Id_Comite_Ampliado = id_comite_ampliado.Id_Comite_Ampliado,
                    Codigo_Credito = credito.Codigo_Credito,
                    Cuota_Total = credito.Valor_Cuota,
                    Linea = credito.Linea_Credito.Descripcion_linea,
                    Monto =credito.Monto_pago,
                    Numero_Cuotas = credito.Numero_cuotas
                    };

                    _context.CreditoComiteAmpliados.Add(creditoComiteAmpliado);
                    await _context.SaveChangesAsync();
                    return Ok("Comité Ampliado creado Exitosamente");
                }
                else
                {
                    return BadRequest("El crédito ya fue sometido a comité ampliado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Post Es para aprobar el comité por usuario
        /// </summary>
        /// <param name="id_comite">int id de comite</param>
        /// <param name="aprobado">bool si es aprobado</param>
        /// <param name="comentario">string un comentario</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Asistente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> ComiteAmpliadoAgencia()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            Usuario_Agencia usuario = _context
                .Usuario_Agencia
                .Include(a=>a.Agencia)
                .Single(a => a.Id_Usuario == userId);
            var asesores = new List<Cartera>();
            asesores = (from c in _context.Cartera
                        join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                        where ua.Id_Agencia == usuario.Id_Agencia
                        select c).ToList();
            List<Comite_Ampliado> comites = _context.Comite_Ampliados.AsNoTracking().Where(a => a.Aprobado_Por == null).ToList();
            List<ComiteAmpliadoDetalle> com = new List<ComiteAmpliadoDetalle>();
            foreach(Comite_Ampliado comite in comites)
            {
                foreach (var asesor in asesores)
                {
                    bool cartera_asesor = _context.Credito.Any(a => a.Id_Cartera == asesor.Id_Cartera && a.Id_Credito == comite.Id_Credito);
                    if (cartera_asesor)
                    {
                        Credito cr = _context.Credito
                            .Include(a=>a.Persona)
                            .Include(a=>a.Comite_Ampliados)
                            .Single(a => a.Id_Credito == comite.Id_Credito && !a.Liquidado);
                        com.Add(new ComiteAmpliadoDetalle
                        {
                            Cliente = cr.Persona,
                            Comite_Ampliado = comite,
                            Credito = cr
                        });
                    }
                }
               

            }

            return Ok(com);
        }
        /// <summary>
        /// Post Es para aprobar el comité por usuario
        /// </summary>
        /// <param name="id_comite">int id de comite</param>
        /// <param name="aprobado">bool si es aprobado</param>
        /// <param name="comentario">string un comentario</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Asistente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> ComitesAmpliados()
        {
           
            List<Comite_Ampliado> comites = _context.Comite_Ampliados.AsNoTracking().Where(a => a.Aprobado_Por == null).ToList();
            List<ComiteAmpliadoDetalle> com = new List<ComiteAmpliadoDetalle>();
            foreach (Comite_Ampliado comite in comites)
            {
                bool liquidado = _context.Credito.Any(a => a.Id_Credito == comite.Id_Credito && a.Liquidado);
                if (!liquidado)
                {
                    Credito cr = _context.Credito
                          .Include(a => a.Persona)
                          .Include(a => a.Comite_Ampliados)
                          .Include(a => a.Comites)
                          .Single(a => a.Id_Credito == comite.Id_Credito );
                    com.Add(new ComiteAmpliadoDetalle
                    {
                        Cliente = cr.Persona,
                        Credito = cr,
                        Comite_Ampliado = comite
                    });
                }
                   


            }

            return Ok(com);
        }

        /// <summary>
        /// Post Es para aprobar el comité por usuario
        /// </summary>
        /// <param name="id_comite">int id de comite</param>
        /// <param name="aprobado">bool si es aprobado</param>
        /// <param name="comentario">string un comentario</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Asistente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> DetalleComitesAmpliados()
        {

            List<Comite_Ampliado> comites = _context.Comite_Ampliados.AsNoTracking().Where(a => a.Aprobado_Por == null).ToList();
            List<ComiteAmpliadoDetalle> com = new List<ComiteAmpliadoDetalle>();
            foreach (Comite_Ampliado comite in comites)
            {
                Credito cr = _context.Credito
                    .Include(a => a.Persona)
                    .Include(a => a.Comite_Ampliados)
                    .Single(a => a.Id_Credito == comite.Id_Credito && !a.Liquidado);
                com.Add(new ComiteAmpliadoDetalle
                {
                    Cliente = cr.Persona,
                    Credito = cr,
                    Comite_Ampliado = comite
                });
            }

            return Ok(com);
        }
        /// <summary>
        /// Post Es para aprobar el comité por usuario
        /// </summary>
        /// <param name="id_comite">int id de comite</param>
        /// <param name="aprobado">bool si es aprobado</param>
        /// <param name="comentario">string un comentario</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Asistente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> DetallePreAprobacion(int idcomiteampliado)
        {

            Comite_Ampliado comite= _context.Comite_Ampliados.Single(a => a.Id_Comite_Ampliado == idcomiteampliado);


            return Ok(comite);
        }
        /// <summary>
        /// Post Es para aprobar el comité por usuario
        /// </summary>
        /// <param name="id_comite">int id de comite</param>
        /// <param name="aprobado">bool si es aprobado</param>
        /// <param name="comentario">string un comentario</param>
        /// <returns></returns>
        [Authorize(Roles = "Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CrearFotografiaCOmite([FromBody] Fotografia_Comite fotografia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Fotografia_Comites.Add(fotografia);
            await _context.SaveChangesAsync();
            return Ok("El la fotografia se ha cargado exitosamente");

        }
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> FotografiasComite(int IdComiteAmpliado)
        {
            List<Fotografia_Comite> roles = await _context.Fotografia_Comites.Where
                (g => g.Id_Comite_Ampliado == IdComiteAmpliado).ToListAsync();
            return Ok(roles);
        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> AprobarAmpliado(int id_credito)
        {
            /*de una vez se agrega al acta*/
            int id_acta = await ObtenerActa();
            Credito creditoAprobado = _context.Credito.SingleOrDefault(y => y.Id_Credito == id_credito);
            creditoAprobado.Aprobado = true;
            creditoAprobado.Aprobado_En = DateTime.Now;
            creditoAprobado.Id_Acta = id_acta;
            var entryCredito = _context.Entry(creditoAprobado);
            entryCredito.Property(f => f.Aprobado).IsModified = true;
            await _context.SaveChangesAsync();
            var userId = _userManager.GetUserId(HttpContext.User);
            Comite_Ampliado ampliado = _context.Comite_Ampliados.Single(a => a.Id_Credito == id_credito);
            ampliado.Aprobado_En = DateTime.Now;
            ampliado.Aprobado_Por = userId;
            _context.Entry(ampliado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Comité Ampliado Aprovado");


        }

        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> RechazarAmpliado(int id_credito)
        {
            /*de una vez se agrega al acta*/
            int id_acta = await ObtenerActa();
            Credito creditoAprobado = _context.Credito.SingleOrDefault(y => y.Id_Credito == id_credito);
            creditoAprobado.Cancelado = true;
            creditoAprobado.Cancelado_En = DateTime.Now;
            creditoAprobado.Id_Acta = id_acta;
            var entryCredito = _context.Entry(creditoAprobado);
            entryCredito.Property(f => f.Aprobado).IsModified = true;
            await _context.SaveChangesAsync();
            var userId = _userManager.GetUserId(HttpContext.User);
            Comite_Ampliado ampliado = _context.Comite_Ampliados.Single(a => a.Id_Credito == id_credito);
        
            ampliado.Aprobado_Por = userId;
            _context.Entry(ampliado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Comité Ampliado Rechazado");


        }
        private async Task AprobandoCredito(int id_credito)
        {
            Credito credito = _context.Credito
                .AsNoTracking()
                .SingleOrDefault(s=>s.Id_Credito==id_credito);
            int cuenta_comite_sin_ampliado = _context.Comite
                .AsNoTracking()
                .Count(c=>c.Id_Credito==id_credito && !c.Ampliado);
            int cuenta_comite_verificado_sin_ampliado = _context.Comite
                .AsNoTracking()
                .Count(c=>c.Id_Credito==id_credito && !c.Ampliado && c.Verificado);
            int cuenta_comite_verificado_aprobado = _context.Comite
                .AsNoTracking().Count(c=> c.Id_Credito == id_credito && c.Aprobado && !c.Ampliado);

            /*verificar si se aprueba el crédito ver si aui se crea acta del d[ia si no existe, y de una vez agregar a la acta*/
            bool existe=_context.Comite.AsNoTracking().Any(t => t.Id_Credito == id_credito);
            bool aprobado_ampliado = _context.Comite.AsNoTracking().Any(c=> c.Id_Credito == id_credito && c.Ampliado && c.Aprobado);
            bool aprobaciones_sin_ampliado = cuenta_comite_verificado_aprobado == cuenta_comite_sin_ampliado && !credito.Necesita_Comite_Ampliado;
            //bool aprobado_ampliado = credito.Necesita_Comite_Ampliado && aprobaciones_sin_ampliado;
            if (existe && aprobaciones_sin_ampliado)
                await AprobadoEnActa(id_credito);
            if (existe && aprobado_ampliado && credito.Necesita_Comite_Ampliado)
                await AprobadoEnActa(id_credito);
        }

        private async Task AprobadoEnActa(int id_credito)
        {
            /*de una vez se agrega al acta*/
            int id_acta = await ObtenerActa();
            Credito creditoAprobado = _context.Credito.SingleOrDefault(y => y.Id_Credito == id_credito);
            creditoAprobado.Aprobado = true;
            creditoAprobado.Aprobado_En = DateTime.Now;
            creditoAprobado.Id_Acta = id_acta;
            var entryCredito = _context.Entry(creditoAprobado);
            entryCredito.Property(f => f.Aprobado).IsModified = true;
            await _context.SaveChangesAsync();
        }

        private async Task<int> ObtenerActa()
        {
            DateTime fecha = DateTime.Now;
            Acta acta_existente = _context.Acta
                .SingleOrDefault(
                        d => d.Fecha_Celebrado.Day == fecha.Day &&
                        d.Fecha_Celebrado.Month == fecha.Month &&
                        d.Fecha_Celebrado.Year == fecha.Year);

            if (acta_existente != null)
            {
                return acta_existente.Id_Acta;
            }
            else
            {
                Acta acta = new Acta {
                    Fecha_Celebrado=fecha
                };
                _context.Acta.Add(acta).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return acta.Id_Acta;
            }
        }

        /// <summary>
        /// Post Para Crear el comité
        /// </summary>
        /// <param name="comite">Comité objeto json</param>
        /// <returns></returns>
        // POST: api/Comites
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> CrearComite(Comite comite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Comite.Add(comite);
            await _context.SaveChangesAsync();

            return Ok("El comité es creado correctamente");
        }

        // DELETE: api/Comites/5
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComite([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comite = await _context.Comite.SingleOrDefaultAsync(m => m.Id_Comite == id);
            if (comite == null)
            {
                return NotFound();
            }

            _context.Comite.Remove(comite);
            await _context.SaveChangesAsync();

            return Ok(comite);
        }

        private bool ComiteExists(int id)
        {
            return _context.Comite.Any(e => e.Id_Comite == id);
        }
    }
}