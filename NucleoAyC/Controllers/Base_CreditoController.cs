using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using NucleoAyC.Entities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// clase que posee método en común para
    /// transacción crédito y ahorro y simple
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true, GroupName = "Base_Credito")]
    public class Base_CreditoController : Controller
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly NegocioContexto _context;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Base_CreditoController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        /// <summary>
        /// saber si un crédito está aprobado desde cualquier
        /// de ramificaciones
        /// </summary>
        /// <param name="id_linea_credito_historial"></param>
        /// <param name="id_cobro_periodico_historial"></param>
        /// <param name="id_deduccion_desembolso_historial"></param>
        /// <returns></returns>
        protected async Task<bool> Credito_Aprobado(
            int id_linea_credito_historial = 0, 
            int id_cobro_periodico_historial = 0, 
            int id_deduccion_desembolso_historial = 0)
        {
            Linea_Credito_Historial linea =await _context
                .Linea_Credito_Historial.AsNoTracking()
                .Include(c=>c.Cobros_Periodicos_Historial)
                .Include(d=>d.Deducciones_Historial)
                .Include(t=>t.Credito)
                .SingleAsync(
                    f => f.Id_Linea_Credito_Historial == id_linea_credito_historial ||
                    f.Deducciones_Historial.Any(a => a.Id_Deduccion_Historial == id_deduccion_desembolso_historial) ||
                    f.Cobros_Periodicos_Historial.Any(t => t.Id_Cobro_Periodico_Historial == id_cobro_periodico_historial)
                    );
            
            return linea.Credito.Aprobado;
        }
        
    }
}