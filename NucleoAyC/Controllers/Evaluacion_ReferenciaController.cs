using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.Evaluacion_Referencia;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Manejo de los evaluación crédito
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Evaluacion_Referencia")]
    public class Evaluacion_ReferenciaController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly NegocioContexto _context;
        /// <summary>
        /// injección de roles y usuario
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Evaluacion_ReferenciaController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// desplegar evaluacion con su bateria de preguntas
        /// que se realiza a las referencias
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Desplegar_Evaluacion(int id_referencia=0)
        {
            /*tener las preguntas respondidas si ya se inicio*/
            if (id_referencia > 0)
            {
                Evaluacion_Referencia_Catalogo cat = await _context
                .Evaluacion_Referencia_Catalogo
                .AsNoTracking()
                .Include(d => d.Preguntas)
                .Include("Preguntas.Respuestas")
                .FirstOrDefaultAsync();

                List<Evaluacion_Referencia_Persona> evaluacion_hecha = _context
                  .Evaluacion_Referencia_Persona
                  .AsNoTracking()
                  .Where(w => w.Id_Referencia == id_referencia).ToList();

                List<Catalogo_Evaluacion_Referencia_Guardada> respondidas_en_catalogo = new List<Catalogo_Evaluacion_Referencia_Guardada>();

                foreach(Pregunta pregunta in cat.Preguntas)
                {
                    Catalogo_Evaluacion_Referencia_Guardada respondida = new Catalogo_Evaluacion_Referencia_Guardada();
                    respondida.Pregunta_catalogo = pregunta;

                    if (evaluacion_hecha.Any(s => s.Id_Pregunta == pregunta.Id_Pregunta))
                        respondida.Id_Respuesta_Contestada = evaluacion_hecha.Single(s => s.Id_Pregunta == pregunta.Id_Pregunta).Id_Respuesta;

                    respondidas_en_catalogo.Add(respondida);
                }

                return Ok(respondidas_en_catalogo);
            }

            return BadRequest("No se pudo obtener la evaluación");
        }

        /// <summary>
        /// personas que tiene referencias
        /// pero que aun no tiene credito y que no tiene una evaluacion
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Personas_Referencias(string buscar)
        {
            List<Persona> personas =  _context
                .Personas
                .AsNoTracking()
                .Include(x=>x.Creditos)
                .Include(r=>r.Evaluaciones_Referencias)
                .Include(r => r.Persona_Referencias)
                .Include("Persona_Referencias.Referencia").AsEnumerable()
                .Where(c=>
                    (
                        c.Dui.Contains(buscar) ||
                        c.Nombres.ToUpper().Contains(buscar.ToUpper()) ||
                        c.Apellidos.ToUpper().Contains(buscar.ToUpper())
                    ) && 
                    c.Persona_Referencias.Count(d=>d.Tipo_Referencia=="1") > 0
                    && c.Persona_Referencias.Count(d => d.Tipo_Referencia == "2") > 0)
                .ToList();
            return Ok(personas);
        }

        /// <summary>
        /// se van guardando las preguntas a la referencia que se pide
        /// </summary>
        /// <param name="id_referencia"></param>
        /// <param name="id_pregunta"></param>
        /// <param name="id_respuesta"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Crear_Bateria(int id_referencia,int id_pregunta, int id_respuesta)
        {
            Persona_Referencia referencia =await _context
                .Persona_Referencia
                .Include(a=>a.Persona)
                .SingleOrDefaultAsync(d=>d.Id_Referencia==id_referencia);
            Evaluacion_Referencia_Catalogo catalogo = _context.Evaluacion_Referencia_Catalogo.SingleOrDefault();

            Respuesta respuesta_dada = _context.Respuesta.Single(r=>r.Id_Respuesta== id_respuesta);

            Evaluacion_Referencia_Persona evalucion = new Evaluacion_Referencia_Persona {
                Id_Evaluacion_Referencia_Catalogo=catalogo.Id_Evaluacion_Referencia_Catalogo,
                Id_Referencia=referencia.Id_Referencia,
                Id_Persona=referencia.Id_Persona,
                Id_Pregunta=id_pregunta,
                Id_Respuesta=id_respuesta,
                Ponderacion=respuesta_dada.Ponderacion
            };

            bool encontrado = _context
                .Evaluacion_Referencia_Persona
                .Any(e =>
                    e.Id_Evaluacion_Referencia_Catalogo == evalucion.Id_Evaluacion_Referencia_Catalogo
                    && e.Id_Persona == evalucion.Id_Persona
                    && e.Id_Referencia == evalucion.Id_Referencia
                    && e.Id_Pregunta == evalucion.Id_Pregunta
                    /*&& e.Id_Respuesta == evalucion.Id_Respuesta*/);

            if (encontrado)
                return BadRequest("Ya existe esta respuesta no se debe editar");

            _context.Evaluacion_Referencia_Persona.Add(evalucion);
            int result=await _context.SaveChangesAsync();
            if (result > 0)
            {
                await Actualizar_Estado_Verificacion_Referencial(
                             evalucion.Id_Evaluacion_Referencia_Catalogo,
                             evalucion.Id_Persona,
                             id_referencia);

                return Ok("La evaluación ha sido guardada");
            }
            else
                return BadRequest("No se pudo registrar la respuesta enviada");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_referencia"></param>
        /// <param name="no_equivocado"></param>
        /// <param name="no_contesta"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Notificar_Fallo_Referencia(int id_referencia,bool no_equivocado=false,bool no_contesta=false)
        {
            await Actualizar_Fallo_Referencia(id_referencia, no_equivocado, no_contesta);
            return Ok("Notificado la consulta faída");
        }

        private async Task Actualizar_Fallo_Referencia(int id_referencia, bool no_equivocado = false, bool no_contesta = false)
        {
            Referencia referencia = await _context.Referencia.SingleOrDefaultAsync(s => s.Id_Referencia == id_referencia);
            if (no_equivocado)
                referencia.Valoracion_Referencia = Valoracion_Referencia.Numero_Equivocado;
            if (no_contesta)
                referencia.Valoracion_Referencia = Valoracion_Referencia.No_Contesta;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task Actualizar_Estado_Verificacion_Referencial(int id_catalogo,int id_persona,int id_referencia)
        {
            int count_preguntas = _context
                .Evaluacion_Referencia_Catalogo
                .AsNoTracking()
                .Include(d=>d.Preguntas)
                .SingleOrDefault(d=>d.Id_Evaluacion_Referencia_Catalogo==id_catalogo).Preguntas.Count();

            int count_respondidas = _context
                .Evaluacion_Referencia_Persona
                .AsNoTracking()
                .Count(
                    c=>c.Id_Persona==id_persona && 
                    c.Id_Referencia==id_referencia && 
                    c.Id_Evaluacion_Referencia_Catalogo==id_catalogo);

            if (count_respondidas == count_preguntas)
            {
                Referencia referencia =await _context
                    .Referencia
                    .AsTracking()
                    .SingleAsync(d=>d.Id_Referencia==id_referencia);
                //referencia.Esta_Verificado = true;
                List<Evaluacion_Referencia_Persona> preguntas = _context
                    .Evaluacion_Referencia_Persona
                    .Where(t => t.Id_Persona == id_persona && t.Id_Referencia == id_referencia).ToList();
                CalculoPonderacionEvaluacion calculo = new CalculoPonderacionEvaluacion();
                decimal total_max_preguntas =await Total_Max_Pregunta();
                decimal promedio = calculo.Ponderar(preguntas, total_max_preguntas);
                Valoracion_Referencia valoracion = calculo.Definir_Matriz(promedio);
                referencia.Valoracion_Referencia = valoracion;
                referencia.Ponderacion_Verficada = promedio;
                await _context.SaveChangesAsync();
            }
        }

        private async Task<decimal> Total_Max_Pregunta()
        {
            List<Pregunta> preguntas =await _context.Pregunta.AsNoTracking().Where(f=>f.Id_Referencia_Evaluada_Catalogo==1).ToListAsync();
            List<Respuesta> respuesta_preguntas_mas_altas = new List<Respuesta>();
            foreach (Pregunta pregunta in preguntas)
            {
                decimal ponderacion_alta = _context
                    .Respuesta
                    .AsNoTracking()
                    .Where(s=>s.Id_Pregunta==pregunta.Id_Pregunta)
                    .Max(t=>t.Ponderacion);
                Respuesta respuesta = _context
                    .Respuesta
                    .AsNoTracking()
                    .SingleOrDefault(
                        k=>k.Id_Pregunta==pregunta.Id_Pregunta && 
                        k.Ponderacion==ponderacion_alta);
                if (!respuesta_preguntas_mas_altas.Any(a=>a.Id_Pregunta==respuesta.Id_Pregunta))
                    respuesta_preguntas_mas_altas.Add(respuesta);
            }
            return respuesta_preguntas_mas_altas.Sum(a=>a.Ponderacion);
        }

        /*manejo del registro de llamadas*/
        /// <summary>
        /// Se registra llamada para despues hacer verificacion
        /// </summary>
        /// <param name="llamada"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Log_Llamada_Referencia([FromBody] Llamada_Telefonica_Referencia llamada)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            llamada.Usuario = userId;
            llamada.Efectuado_En = DateTime.Now;
            llamada.Registrado_Para = DateTime.Now;
            if (ModelState.IsValid)
            {
                //verificar que no existe la misma referencia con la misma fecha
                bool existe = _context.Llamada_Telefonica_Referencia.Any(x=>x.Id_Referencia==llamada.Id_Referencia && x.Efectuado_En == llamada.Efectuado_En);
                int lapso= Convert.ToInt32(_context.Configuracion_General.SingleAsync(v => v.Nombre == "LAPSO_ENTRE_LLAMADA").Result.Valor);
                if (!existe)
                {
                    Llamada_Telefonica_Referencia ultima_llamada= _context.Llamada_Telefonica_Referencia.Where(w=>w.Id_Referencia==llamada.Id_Referencia).Last();
                    if (DateTime.Now >= ultima_llamada.Efectuado_En.AddMinutes(lapso))
                    {
                        _context.Llamada_Telefonica_Referencia.Add(llamada);
                        await _context.SaveChangesAsync();
                        return Ok("el registro se ha guardado correctamente");
                    }
                    else
                        return BadRequest("ha registrado otra llamada en menos de " + lapso);
                }
                else
                    return BadRequest("Ya existe llamada con el mismo tiempo para la referencia");
            }
            return BadRequest("No se pudo guardar el registro");
        }

        /// <summary>
        /// se obtienen el log de llamadas
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Logger_Llamada_Referencia(int id_referencia)
        {
            List<Llamada_Telefonica_Referencia> llamadas =await _context
                .Llamada_Telefonica_Referencia
                .Where(l=>l.Id_Referencia==id_referencia)
                .ToListAsync();

            return Ok(llamadas);
        }


        
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[HttpPut]
        //public async Task<IActionResult> Rastrear_Llamada(int id_llamada)
        //{
        //    DateTime ahora = DateTime.Now;

        //    Llamada_Telefonica_Referencia llamada =await _context
        //        .Llamada_Telefonica_Referencia
        //        .SingleAsync(
        //            f=>f.Id_Llamada_Telefonica_Referencia== id_llamada);

        //    if (llamada != null)
        //    {
        //        llamada.Intentos += 1;
        //        llamada.Efectuado_En = ahora;
        //        _context.Entry(llamada).State = EntityState.Modified;
        //        await _context.SaveChangesAsync();

        //        int veces = Convert.ToInt32(_context.Configuracion_General.SingleAsync(v=>v.Nombre== "INTENTOS_LLAMADA").Result.Valor);
        //        Referencia referencia =await _context.Referencia.SingleAsync(s=>s.Id_Referencia==llamada.Id_Referencia);
        //        if(llamada.Intentos==veces)
        //            await Actualizar_Fallo_Referencia(referencia.Id_Referencia, false, true);
                
        //        return Ok("Se ha rastreado correctamente la llamada");
        //    }

        //    return BadRequest("No se pudo rastrear la llamada");
        //}
    }
}