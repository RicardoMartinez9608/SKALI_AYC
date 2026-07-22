using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// para controlar las referencia de una persona
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Persona_Referencia")]
    public class Persona_ReferenciaController : Controller
    {
        private readonly NegocioContexto _context;

        public Persona_ReferenciaController(NegocioContexto context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Obtiene la lista de Referencia pertenecientes a una persona
        /// </summary>
        /// <param name="id_persona">int id de la persona</param>
        /// <param name="tipo_referencia"></param>
        /// <param name="beneficiario"></param>
        /// <returns>Lista de ReferenciaVista</returns>
        // GET: api/Persona_Referencia
        [Authorize(Roles = "Asesor, Asistente,Jefe,Gerente,Administrador", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IEnumerable<ReferenciaVista> Personas_Referencias_Por_Persona(int id_persona,int tipo_referencia=0,bool beneficiario=false)
        {
            Func<Persona_Referencia, bool> fitrando = new Func<Persona_Referencia, bool>(c => c.Id_Persona == id_persona);

            if (tipo_referencia == 1)//este filtro es para saber si es familia
                fitrando = new Func<Persona_Referencia, bool>(
                    c=>c.Id_Persona==id_persona && 
                    c.Tipo_Referencia==tipo_referencia.ToString() &&
                    c.Referencia.Beneficiario==beneficiario
                );

            return _context.Persona_Referencia
                .Include(e=>e.Referencia)
                .Where(fitrando).Select(q=>new ReferenciaVista {
                Id_Referencia=q.Id_Referencia,
                Nombre=q.Referencia.Nombre,
                FechaNacimiento=q.Referencia.FechaNacimiento,
                Telefono=q.Referencia.Telefono,
                Residencia=q.Referencia.Residencia,
                Parentesco=q.Parentesco,
                Tipo_Referencia=q.Tipo_Referencia,
                Porcentaje=q.Referencia.Porcentaje,
                Edad=q.Referencia.Edad,
                Beneficiario=q.Referencia.Beneficiario
            });
        }
    }
}