using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelNucleo.Model.ModelView;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// Para registrar los dias de asueto y otras actividades importantes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Actividad_GeneralController : ControllerBase
    {

        private readonly NegocioContexto _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Constructor de la actividad general
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public Actividad_GeneralController(NegocioContexto context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        #region parte de pruebas para actividades como dias asueto
        private async Task<List<Actividad_General>> Actividades()
        {
            List<Actividad_General> ag = new List<Actividad_General>();
            ag.Add(new Actividad_General {Id=1,Nombre_Actividad="Fiesta de Agosto", Fecha_Actividad=DateTime.Parse("2019-08-06") });
            ag.Add(new Actividad_General { Id = 1, Nombre_Actividad = "Independencia", Fecha_Actividad = DateTime.Parse("2019-09-15") });

            return ag;
        }

        /// <summary>
        /// para separar de fechas de asueta
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Fechas_Libres_Feriados()
        {
            List<Actividad_General> ag = await Actividades();
            //listas de fechas para probar
            List<DateTime> fechas = new List<DateTime>();
            fechas.Add(DateTime.Now.AddDays(1));
            fechas.Add(DateTime.Now);

            List<DateTime> fechitas= fechas.Intersect(ag.Select(d => d.Fecha_Actividad)).ToList();

            return Ok(fechitas);
        }
        #endregion
    }
}