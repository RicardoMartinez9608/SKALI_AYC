using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using ModelNucleo.Model.Tablero;
using NucleoAyC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Tablero")]
    public class TableroController : Base_TransaccionController
    {
        /// <summary>
        /// constructor e injección de credito
        /// </summary>
        /// <param name="context"></param>
        /// /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public TableroController(NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, userManager, roleManager)
        {
        }

        /// <summary>
        /// donde se procesa el tablero
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Procesar_Tablero([FromBody] List<Rancking> ranckings)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest();
                    foreach (Rancking rank in ranckings)
                    {
                        Func<Cartera, bool> filtro_cartera = new Func<Cartera, bool>(d => d.Colector == rank.Asesor);
                        if (_context.Cartera.Any(filtro_cartera))
                        {
                            Cartera cartera = _context.Cartera.Single(d => d.Colector == rank.Asesor);
                            rank.Id_Cartera = cartera.Id_Cartera;
                            bool yaexiste = _context
                                .Rancking
                                .Any(
                                    t => t.Id_Cartera == rank.Id_Cartera &&
                                    t.Fecha.Date == rank.Fecha.Date
                                );
                            if (!yaexiste)
                            {
                                _context.Rancking.Add(rank).State = EntityState.Added;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    dbContextTransaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest("Los ranckings no se pudieron procesar");
                }
            }
        }
        /// <summary>
        /// donde se procesa el reporte de mora
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Procesar_Mora([FromBody] List<Reporte_Mora> moras)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest();
                    foreach (Reporte_Mora rank in moras)
                    {
                        Func<Cartera, bool> filtro_cartera = new Func<Cartera, bool>(d => d.Colector == rank.Asesor);
                        if (_context.Cartera.Any(filtro_cartera))
                        {
                            Cartera cartera = _context.Cartera.Single(d => d.Colector == rank.Asesor);
                            rank.Id_Cartera = cartera.Id_Cartera;
                            bool yaexiste = _context
                                .Reporte_Mora
                                .Any(
                                    t => t.Codigo == rank.Codigo &&
                                   t.Fecha.Date == rank.Fecha.Date
                                );
                            if (!yaexiste)
                            {
                                _context.Reporte_Mora.Add(rank).State = EntityState.Added;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    dbContextTransaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest("La mora no se pudo subir");
                }
            }
        }
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Procesar_Tablero_Cosecha([FromBody] List<Cosecha> cosecha)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest();
                    foreach (Cosecha co in cosecha)
                    {
                        var suma_saldo = cosecha.Where(c => c.Asesor == co.Asesor).Sum(a => a.Saldo);
                        Func<Cartera, bool> filtro_cartera = new Func<Cartera, bool>(d => d.Colector == co.Asesor);
                        Func<Cosecha, bool> filtro_cosecha = new Func<Cosecha, bool>(d => d.Asesor == co.Asesor && d.Saldo > 0
                        && d.Fecha.Date == co.Fecha.Date);
                        if (_context.Cartera.Any(filtro_cartera))
                        {
                            Cartera cartera = _context.Cartera.Single(d => d.Colector == co.Asesor);
                            if (!_context.Cosecha.Any(filtro_cosecha))
                            {
                                co.Id_Cartera = cartera.Id_Cartera;
                                co.Saldo = suma_saldo;
                                _context.Cosecha.Add(co).State = EntityState.Added;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    dbContextTransaction.Commit();
                    return Ok("Procesado exitosamente");
                }
                catch (Exception ex)
                {
                    return BadRequest("Las cosechas no se pudieron procesar");
                }
            }
        }
        /// <summary>
        /// crar meta
        /// </summary>
        /// <param name="meta"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> Agregar_Meta([FromBody] Meta meta)
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                try
                {
                    if (db.Meta.Any(a => a.Id_Cartera == meta.Id_Cartera && a.Mes == meta.Mes))
                    {
                        return BadRequest("Ya existe Meta para este asesor en este mes");
                    }
                    else {
                        if (ModelState.IsValid)
                        {
                           
                            meta.Creado_En = DateTime.Now.Date;
                            string id_usuario = _userManager.GetUserId(HttpContext.User);
                            meta.Creado_Por = id_usuario;
                            _context.Meta.Add(meta).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                            return Ok("Valores Almacenados exitosamente");
                        }
                        else
                        {
                            return BadRequest(ModelState.ValidationState.ToString());
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    return await Task.Run(() => BadRequest());
                }
            }
            
        }

        /// <summary>
        /// un meta específico
        /// </summary>
        /// <param name="id_meta"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Meta(int id_meta)
        {
            Meta meta = await _context.Meta.SingleAsync(c => c.Id_Meta == id_meta);
            return Ok(meta);
        }

        /// <summary>
        /// editar meta
        /// </summary>
        /// <param name="meta"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Editar_Meta([FromBody] Meta meta)
        {
            try
            {
                if (meta != null)
                {
                    meta.Modificado_En = DateTime.Now.Date;
                    string id_usuario = _userManager.GetUserId(HttpContext.User);
                    meta.Modificado_Por = id_usuario;
                    _context.Entry(meta).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok("Se han actualizado los valores");
                }
                return Ok("No se cargaron los valores midficados");
            }
            catch (Exception ex)
            {
                return BadRequest("No se pudo actualizar");
            }

        }

        /// <summary>
        /// entrega acta con la lista de creditos aprobados
        /// </summary>
        /// <param name="id_acta">int id de acta</param>
        /// <returns>Lista de créditos</returns>
        [Authorize(Roles = "Jefe,Administrador,Asistente", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Metas_Por_Agencia(int id_agencia = 0, int mes = 0, int anio =0)
        {
            List<Meta> metas = new List<Meta>();
            NegocioContexto db = new NegocioContexto();

            var asesores = new List<Cartera>();
            if (mes == 0) {
                mes = DateTime.Now.Month;
            }
            if (User.IsInRole("Administrador") && id_agencia > 0)
            {
                asesores = (from c in _context.Cartera
                            join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                            where ua.Id_Agencia == id_agencia
                            select c).ToList();
            }
            foreach (var meta in asesores)
            {
                bool existe = db.Meta.AsNoTracking().Any(t => t.Id_Cartera == meta.Id_Cartera);
                if (existe)
                {
                    metas.AddRange(_context.Meta.AsNoTracking()
                        .Include(c => c.Cartera)
                        .Where(t => t.Id_Cartera == meta.Id_Cartera && t.Mes == mes && t.Anyo == anio).ToList());
                }
            }
            return Ok(metas);
        }
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<decimal> Desempeno_Asesor(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio = 0)
        {
            try
            {
                var asesores = new List<Cartera>();
                string userId = _userManager.GetUserId(HttpContext.User);
                int id_cartera_correcto = 0;
                int fecha_actual = 0;
                decimal desempeño = 0.00M;
                fecha_actual = month;
                if (User.IsInRole("Asesor"))
                {
                    Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                    id_cartera_correcto = ua.Id_Cartera;
                }
                else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                    id_agencia = ua.Id_Agencia;
                    id_cartera_correcto = id_cartera;
                }
                else
                {
                    id_cartera_correcto = id_cartera;
                }
                if (User.IsInRole("Administrador") | User.IsInRole("Gerente") | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia
                                select c).ToList();
                    decimal meta_ingresos = 0.00M;
                    decimal ranking_ingresos = 0.00M;
                    decimal ranking_desembolso = 0.00M;
                    foreach (var asesore in asesores)
                    {
                        bool t_meta = _context.Meta.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Mes == fecha_actual && a.Anyo == anio);
                        Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_meta);
                        Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera);

                        if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                            int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                                .OrderBy(a => a.Id_Meta).Last().Mes;
                            int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                                .OrderBy(a => a.Id_Meta).Last().Anyo;
                            if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                            {
                                meta_ingresos += _context.Meta.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                && a.Mes == fecha_meta && a.Anyo == anio).OrderBy(a => a.Id_Meta).Last().Ingresos;
                                ranking_ingresos += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                 b.Fecha.Month == fecha_ranking  ).OrderBy(a => a.Id_Rancking).Last().Ingresos;
                                //ranking_desembolso += _context.Rancking.Where(c => c.Id_Cartera == asesore.Id_Cartera
                                // && c.Fecha.Month == fecha_ranking).OrderBy(a => a.Id_Rancking).Last().Desembolso;
                            }
                        }
                    }
                    desempeño = ((ranking_ingresos) * 100) / meta_ingresos;
                }
                else if (User.IsInRole("Administrador") && id_agencia == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                select c).ToList();
                    decimal meta_ingresos = 0.00M;
                    decimal ranking_ingresos = 0.00M;
                    foreach (var asesore in asesores)
                    {
                        bool t_meta = _context.Meta.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Mes == fecha_actual && a.Anyo == anio);
                        Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_meta);
                        Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera);

                        if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month )
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                            int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                                .OrderBy(a => a.Id_Meta).Last().Mes;
                            int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                                .OrderBy(a => a.Id_Meta).Last().Anyo;
                            if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                            {
                                meta_ingresos += _context.Meta.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                && a.Mes == fecha_meta && a.Anyo == anio).OrderBy(a => a.Id_Meta).Last().Ingresos;
                                ranking_ingresos += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                 b.Fecha.Month == fecha_ranking).OrderBy(a => a.Id_Rancking).Last().Ingresos;
                                //ranking_desembolso += _context.Rancking.Where(c => c.Id_Cartera == asesore.Id_Cartera
                                // && c.Fecha.Month == fecha_ranking).OrderBy(a => a.Id_Rancking).Last().Desembolso;
                            }
                        }
                    }
                    desempeño = ((ranking_ingresos) * 100) / meta_ingresos;
                }
                else
                {
                    bool t_meta = _context.Meta.AsNoTracking()
                        .Any(a => a.Id_Cartera == id_cartera_correcto && a.Mes == fecha_actual);
                    Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == id_cartera_correcto && t_meta);
                    Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == id_cartera_correcto);

                    if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                    {
                        int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                        int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Mes;
                        int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                               .OrderBy(a => a.Id_Meta).Last().Anyo;

                        if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                        {
                            decimal meta_ingresos = _context.Meta.Where(a => a.Id_Cartera == id_cartera_correcto 
                            && a.Mes == fecha_meta && a.Anyo == anio)
                                .OrderBy(b => b.Id_Meta).Last().Ingresos;
                            decimal ranking_ingresos = _context.Rancking.Where(b => b.Id_Cartera == id_cartera_correcto && b.Fecha.Month == fecha_ranking)
                                .OrderBy(a => a.Id_Rancking).Last().Ingresos;
                            //decimal ranking_desembolso = _context.Rancking.Where(c => c.Id_Cartera == id_cartera_correcto && c.Fecha.Month == fecha_ranking)
                            //    .OrderBy(a => a.Id_Rancking).Last().Desembolso;
                            desempeño = ((ranking_ingresos) * 100) / meta_ingresos;
                        }
                    }
                    else
                    {
                        return 0.00M;
                    }
                }

                return Math.Round(desempeño, 2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        /// <summary>
        /// colecta asesor
        /// </summary>
        /// <param name="id_cartera"></param>
        /// <param name="month"></param>
        /// <param name="id_agencia"></param>
        /// <returns></returns>
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<decimal> Colecta_Asesor(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio =0)
        {
            try
            {
                var asesores = new List<Cartera>();
                string userId = _userManager.GetUserId(HttpContext.User);
                int id_cartera_correcto = 0;
                int fecha_actual = 0;
                decimal colocacion = 0.00M;
                fecha_actual = month;
                if (User.IsInRole("Asesor"))
                {
                    Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                    id_cartera_correcto = ua.Id_Cartera;
                }
                else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                    id_agencia = ua.Id_Agencia;
                    id_cartera_correcto = id_cartera;
                }
                else
                {
                    id_cartera_correcto = id_cartera;
                }
                if ((User.IsInRole("Administrador")) | (User.IsInRole("Gerente")) | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia
                                select c).ToList();
                    decimal ranking_colocacion = 0.00M;
                    foreach (var asesore in asesores)
                    {
                        if (!asesore.Excluido)
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month 
                            && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                            int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                                .OrderBy(a => a.Id_Meta).Last().Mes;
                            int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Anyo;
                            bool t_meta = _context.Meta.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Mes == fecha_actual && a.Anyo == anio);
                            if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta && t_meta)
                            {
                                ranking_colocacion += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                 b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Colecta;
                            }
                        }
                    }
                    colocacion = ranking_colocacion;
                } else if (User.IsInRole("Administrador") && id_agencia == 0) {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                select c).ToList();
                    decimal ranking_colocacion = 0.00M;
                    foreach (var asesore in asesores)
                    {
                       
                        if (!asesore.Excluido)
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month 
                            && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                            int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                                .OrderBy(a => a.Id_Meta).Last().Mes;
                            int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Anyo == anio && a.Anyo == anio)
                           .OrderBy(a => a.Id_Meta).Last().Anyo;
                            if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                            {
                                ranking_colocacion += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                 b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Colecta;
                            }
                        }
                    }
                    colocacion = ranking_colocacion;
                }
                else
                {
                    bool t_meta = _context.Meta.AsNoTracking()
                        .Any(a => a.Id_Cartera == id_cartera_correcto && a.Mes == fecha_actual);
                    Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == id_cartera_correcto && t_meta);
                    Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == id_cartera_correcto);

                    if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                    {
                        int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                            .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                        int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Mes;
                        int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Anyo == anio && a.Anyo == anio)
                           .OrderBy(a => a.Id_Meta).Last().Anyo;

                        if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                        {
                            decimal ranking_colocacion = _context.Rancking.Where(b => b.Id_Cartera == id_cartera_correcto 
                            && b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Colecta;

                            colocacion = ranking_colocacion;
                        }
                    }
                    else
                    {
                        return 0.00M;
                    }
                }
                return Math.Round(colocacion, 2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<Rancking> Metas_Asesor(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio = 0)
        {
            try
            {
                var asesores = new List<Cartera>();
                string userId = _userManager.GetUserId(HttpContext.User);
                int id_cartera_correcto = 0;
                int fecha_actual = 0;
                fecha_actual = month;
                Rancking porcentajes = new Rancking();
                if (User.IsInRole("Asesor"))
                {
                    Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                    id_cartera_correcto = ua.Id_Cartera;
                }
                else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                    id_agencia = ua.Id_Agencia;
                    id_cartera_correcto = id_cartera;
                }
                else
                {
                    id_cartera_correcto = id_cartera;
                }
                decimal meta_clientes = 0.00M; decimal meta_cartera = 0.00M; decimal meta_colecta = 0.00M;
                decimal ranking_clientes = 0.00M; decimal ranking_cartera = 0.00M; decimal ranking_colecta = 0.00M;
                if ((User.IsInRole("Administrador")) | (User.IsInRole("Gerente")) | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia
                                select c).ToList();
                    foreach (var asesore in asesores)
                    {
                        bool t_meta = _context.Meta.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Mes == fecha_actual);
                        Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_meta);
                        Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera);

                        if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month 
                            && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                            int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                                .OrderBy(a => a.Id_Meta).Last().Mes;
                            int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Anyo;
                            if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                            {
                                //clientes activos
                                meta_clientes += _context.Meta.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Mes == fecha_meta && a.Anyo == anio).OrderBy(b => b.Id_Meta).Last().No_Clientes;
                                ranking_clientes += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                 b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().No_Clientes_Activos;
                                //cartera
                                meta_cartera += _context.Meta.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                && a.Mes == fecha_meta && a.Anyo == anio)
                                .OrderBy(b => b.Id_Meta).Last().Capital;
                                ranking_cartera += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Saldo_Cartera;
                                //colecta
                                meta_colecta += _context.Meta.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                && a.Mes == fecha_meta && a.Anyo == anio)
                                .OrderBy(b => b.Id_Meta).Last().Colecta;
                                ranking_colecta += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Colecta;
                            }
                        }
                    }
                    porcentajes.Porcentaje_Clientes = Math.Round(((ranking_clientes) * 100) / meta_clientes, 2);
                    porcentajes.Porcentaje_Cartera = Math.Round(((ranking_cartera) * 100) / meta_cartera, 2);
                    porcentajes.Porcentaje_Colecta = Math.Round(((ranking_colecta) * 100) / meta_colecta, 2);
                } else if (User.IsInRole("Administrador") && id_agencia == 0) {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                select c).ToList();
                    foreach (var asesore in asesores)
                    {
                        bool t_meta = _context.Meta.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Mes == fecha_actual && a.Anyo == anio);
                        Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_meta);
                        Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera 
                        && a.Anyo == anio);

                        if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month 
                            && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                            int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                                .OrderBy(a => a.Id_Meta).Last().Mes;
                            int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Anyo;
                            if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                            {
                                //clientes activos
                                meta_clientes += _context.Meta.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Mes == fecha_meta && a.Anyo == anio).OrderBy(b => b.Id_Meta).Last().No_Clientes;
                                ranking_clientes += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                 b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().No_Clientes_Activos;
                                //cartera
                                meta_cartera += _context.Meta.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                && a.Mes == fecha_meta && a.Anyo == anio)
                                .OrderBy(b => b.Id_Meta).Last().Capital;
                                ranking_cartera += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Saldo_Cartera;
                                //colecta
                                meta_colecta += _context.Meta.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                && a.Mes == fecha_meta && a.Anyo == anio)
                                .OrderBy(b => b.Id_Meta).Last().Colecta;
                                ranking_colecta += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Colecta;
                            }
                        }
                    }
                    porcentajes.Porcentaje_Clientes = Math.Round(((ranking_clientes) * 100) / meta_clientes, 2);
                    porcentajes.Porcentaje_Cartera = Math.Round(((ranking_cartera) * 100) / meta_cartera, 2);
                    porcentajes.Porcentaje_Colecta = Math.Round(((ranking_colecta) * 100) / meta_colecta, 2);
                }
                else
                {
                    bool t_meta = _context.Meta.AsNoTracking()
                        .Any(a => a.Id_Cartera == id_cartera_correcto && a.Mes == fecha_actual);
                    Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == id_cartera_correcto && t_meta);
                    Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == id_cartera_correcto);

                    if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                    {
                        int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                            .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                        int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Mes;
                        int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Anyo;

                        if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                        {

                            //clientes activos
                            meta_clientes += _context.Meta.Where(a => a.Id_Cartera == id_cartera_correcto &&
                                a.Mes == fecha_meta && a.Anyo == anio).OrderBy(b => b.Id_Meta).Last().No_Clientes;
                            ranking_clientes += _context.Rancking.Where(b => b.Id_Cartera == id_cartera_correcto &&
                            b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().No_Clientes_Activos;
                            //cartera
                            meta_cartera += _context.Meta.Where(a => a.Id_Cartera == id_cartera_correcto &&
                                a.Mes == fecha_meta && a.Anyo == anio).OrderBy(b => b.Id_Meta).Last().Capital;
                            ranking_cartera += _context.Rancking.Where(b => b.Id_Cartera == id_cartera_correcto &&
                            b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Saldo_Cartera;
                            //colecta
                            meta_colecta += _context.Meta.Where(a => a.Id_Cartera == id_cartera_correcto &&
                                a.Mes == fecha_meta && a.Anyo == anio).OrderBy(b => b.Id_Meta).Last().Colecta;
                            ranking_colecta += _context.Rancking.Where(b => b.Id_Cartera == id_cartera_correcto &&
                            b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Colecta;

                            porcentajes.Porcentaje_Clientes = Math.Round(((ranking_clientes) * 100) / meta_clientes, 2);
                            porcentajes.Porcentaje_Cartera = Math.Round(((ranking_cartera) * 100) / meta_cartera, 2);
                            porcentajes.Porcentaje_Colecta = Math.Round(((ranking_colecta) * 100) / meta_colecta, 2);
                        }
                    }
                    else
                    {
                        return porcentajes;
                    }
                }
                return porcentajes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<decimal> Indice_Cosecha(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio =0)
        {
            try
            {
                var asesores = new List<Cartera>();
                string userId = _userManager.GetUserId(HttpContext.User);
                int id_cartera_correcto = 0;
                int fecha_actual = 0;
                fecha_actual = month;
                decimal indice_cosesha = 0.00M;
                if (User.IsInRole("Asesor"))
                {
                    Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                    id_cartera_correcto = ua.Id_Cartera;
                }
                else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                    id_agencia = ua.Id_Agencia;
                    id_cartera_correcto = id_cartera;
                }
                else
                {
                    id_cartera_correcto = id_cartera;
                }
                decimal valor_cosesha = 0.00M;
                decimal cartera_ranking = 0.00M;
                if ((User.IsInRole("Administrador")) | (User.IsInRole("Gerente")) | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia
                                select c).ToList();
                    foreach (var asesore in asesores)
                    {
                        bool t_meta = _context.Cosecha.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_actual && a.Fecha.Year == anio);
                        Func<Cosecha, bool> tiene_cosecha = new Func<Cosecha, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_meta);
                        Func<Rancking, bool> tiene_ranking = new Func<Rancking, bool>(a => a.Id_Cartera == asesore.Id_Cartera 
                        && a.Fecha.Year == anio);

                        if (_context.Cosecha.Any(tiene_cosecha) && _context.Rancking.Any(tiene_ranking))
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                            int fecha_cosesha = _context.Cosecha.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Cosecha).Last().Fecha.Month;
                            int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Anyo;
                            if (fecha_actual == fecha_ranking && fecha_actual == fecha_cosesha && anio == anio_meta)
                            {
                                // obtener indice cosesha
                                valor_cosesha += _context.Cosecha.Where(a => a.Id_Cartera == asesore.Id_Cartera
                             && a.Fecha.Month == fecha_cosesha && a.Fecha.Year == anio).OrderBy(a => a.Id_Cosecha).Last().Saldo;
                                cartera_ranking += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                 b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Saldo_Cartera;
                            }

                        }
                    }
                    indice_cosesha = valor_cosesha / cartera_ranking;
                } else if (User.IsInRole("Administrador") && id_agencia == 0) {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                select c).ToList();
                    foreach (var asesore in asesores)
                    {
                        bool t_meta = _context.Cosecha.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_actual && a.Fecha.Year == anio);
                        Func<Cosecha, bool> tiene_cosecha = new Func<Cosecha, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_meta);
                        Func<Rancking, bool> tiene_ranking = new Func<Rancking, bool>(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Year == anio);

                        if (_context.Cosecha.Any(tiene_cosecha) && _context.Rancking.Any(tiene_ranking))
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                            int fecha_cosesha = _context.Cosecha.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Cosecha).Last().Fecha.Month;
                            int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Anyo;
                            if (fecha_actual == fecha_ranking && fecha_actual == fecha_cosesha && anio == anio_meta)
                            {
                                // obtener indice cosesha
                                valor_cosesha += _context.Cosecha.Where(a => a.Id_Cartera == asesore.Id_Cartera
                             && a.Fecha.Month == fecha_cosesha && a.Fecha.Year == anio).OrderBy(a => a.Id_Cosecha).Last().Saldo;
                                cartera_ranking += _context.Rancking.Where(b => b.Id_Cartera == asesore.Id_Cartera &&
                                 b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Saldo_Cartera;
                            }

                        }
                    }
                    indice_cosesha = valor_cosesha / cartera_ranking;
                }
                else
                {
                    bool t_meta = _context.Cosecha.AsNoTracking()
                        .Any(a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == fecha_actual && a.Fecha.Year == anio);
                    Func<Cosecha, bool> tiene_cosecha = new Func<Cosecha, bool>(a => a.Id_Cartera == id_cartera_correcto && t_meta);
                    Func<Rancking, bool> tiene_ranking = new Func<Rancking, bool>(a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Year == anio);

                    if (_context.Cosecha.Any(tiene_cosecha) && _context.Rancking.Any(tiene_ranking))
                    {
                        int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                            .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                        int fecha_cosesha = _context.Cosecha.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                            .OrderBy(a => a.Id_Cosecha).Last().Fecha.Month;
                        int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Anyo;

                        if (fecha_actual == fecha_ranking && fecha_actual == fecha_cosesha && anio == anio_meta)
                        {

                            //clientes activos
                            valor_cosesha += _context.Cosecha.Where(a => a.Id_Cartera == id_cartera_correcto
                            && a.Fecha.Month == fecha_cosesha && a.Fecha.Year == anio).OrderBy(a => a.Id_Cosecha).Last().Saldo;
                            cartera_ranking += _context.Rancking.Where(b => b.Id_Cartera == id_cartera_correcto &&
                             b.Fecha.Month == fecha_ranking && b.Fecha.Year == anio).OrderBy(a => a.Id_Rancking).Last().Saldo_Cartera;
                            indice_cosesha = valor_cosesha / cartera_ranking;
                        }
                    }
                    else
                    {
                        return Math.Round(indice_cosesha * 100, 2);
                    }
                }
                return Math.Round(indice_cosesha*100,2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<List<decimal>> Colecta_Asesores(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio =0)
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                try
                {
                    int ano_actual = DateTime.Now.Year;
                    int id_cartera_correcto = 0;
                    var asesores = new List<Cartera>();
                    string userId = _userManager.GetUserId(HttpContext.User);
                    int fecha_actual = month;
                    List<decimal> prueba = new List<decimal>();
                    List<Grafico_Colocacion_Asesores> valores_por_fecha = new List<Grafico_Colocacion_Asesores>();
                    List<Rancking> valores_colecta = new List<Rancking>();
                    if (User.IsInRole("Asesor"))
                    {
                        Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                        id_cartera_correcto = ua.Id_Cartera;
                    }
                    else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                    {
                        string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                        Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                        id_agencia = ua.Id_Agencia;
                        id_cartera_correcto = id_cartera;
                    }
                    else
                    {
                        id_cartera_correcto = id_cartera;
                    }

                    Func<Rancking, bool> tiene_ranking = new Func<Rancking, bool>
                        (a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == month && a.Fecha.Year == anio);
                    if ((User.IsInRole("Administrador")) | (User.IsInRole("Gerente")) | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                    {
                        asesores = (from c in _context.Cartera
                                    join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                    where ua.Id_Agencia == id_agencia
                                    select c).ToList();
                        IEnumerable<IGrouping<DateTime, Rancking>> personas_ahorros = _context.Rancking
                           .Where(d => d.Fecha.Month == month && d.Fecha.Year == anio)
                           .OrderBy(c => c.Id_Rancking)
                           .AsEnumerable()
                           .GroupBy(d => d.Fecha);
                        foreach (var f_ch in personas_ahorros)
                        {
                            foreach (var asesore in asesores)
                            {
                                Func<Rancking, bool> tiene_ranking_asesor = new Func<Rancking, bool>
                                    (a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Day == f_ch.Key.Day && a.Fecha.Year == anio);
                                if (_context.Rancking.Any(tiene_ranking_asesor))
                                {
                                    valores_colecta = _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                     && a.Fecha.Date == f_ch.Key.Date && a.Fecha.Year == anio).OrderBy(a => a.Fecha).ToList();
                                    valores_por_fecha.Add(new Grafico_Colocacion_Asesores {
                                        fecha = f_ch.Key.Day,
                                        valor = valores_colecta.Sum(a => a.Colocacion)
                                    });
                                }
                            }
                        }
                        var query = (from cc in valores_por_fecha
                                     group cc by cc.fecha into newgroup
                                     select new Grafico_Colocacion_Asesores
                                     {
                                         valor = newgroup.Sum(c => c.valor)
                                     }).OrderBy(c => c.fecha).ToList();
                        foreach (var ff in query) {
                            prueba.Add(ff.valor);
                        }

                    }
                    else if(User.IsInRole("Administrador") && id_agencia == 0) {
                        asesores = (from c in _context.Cartera
                                    join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                    select c).ToList();
                        IEnumerable<IGrouping<DateTime, Rancking>> personas_ahorros = _context.Rancking
                           .Where(d => d.Fecha.Month == month && d.Fecha.Year == anio)
                           .OrderBy(c => c.Id_Rancking)
                           .AsEnumerable()
                           .GroupBy(d => d.Fecha);
                        foreach (var f_ch in personas_ahorros)
                        {
                            foreach (var asesore in asesores)
                            {
                                Func<Rancking, bool> tiene_ranking_asesor = new Func<Rancking, bool>
                                    (a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Day == f_ch.Key.Day &&a.Fecha.Year == anio);
                                if (_context.Rancking.Any(tiene_ranking_asesor))
                                {
                                    valores_colecta = _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                     && a.Fecha.Date == f_ch.Key.Date && a.Fecha.Year == anio).OrderBy(a => a.Fecha).ToList();
                                    valores_por_fecha.Add(new Grafico_Colocacion_Asesores
                                    {
                                        fecha = f_ch.Key.Day,
                                        valor = valores_colecta.Sum(a => a.Colocacion)
                                    });
                                }
                            }
                        }
                        var query = (from cc in valores_por_fecha
                                     group cc by cc.fecha into newgroup
                                     select new Grafico_Colocacion_Asesores
                                     {
                                         valor = newgroup.Sum(c => c.valor)
                                     }).OrderBy(c => c.fecha).ToList();
                        foreach (var ff in query)
                        {
                            prueba.Add(ff.valor);
                        }
                    }
                    else
                    {
                        if (_context.Rancking.Any(tiene_ranking))
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month 
                            && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;
                            int anio_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Year;
                            if (fecha_actual == fecha_ranking && anio_ranking == anio)
                            {
                                valores_colecta = _context.Rancking.Where(a => a.Id_Cartera == id_cartera_correcto
                                && a.Fecha.Month == fecha_ranking && a.Fecha.Year == ano_actual).ToList();
                                foreach (var data in valores_colecta)
                                {
                                    if (data.Colocacion > 0)
                                    {
                                        prueba.Add(data.Colocacion);
                                    }
                                };
                            }
                        }
                    }
                    return prueba.OrderBy(x => x).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
           
        }

        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<Grafico_Colocacion_Asesores> Mora_Asesores(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio =0)
        {
            using (NegocioContexto db = new NegocioContexto())
            {
                try
                {
                    int ano_actual = DateTime.Now.Year;
                    int id_cartera_correcto = 0;
                    var asesores = new List<Cartera>();
                    string userId = _userManager.GetUserId(HttpContext.User);
                    int fecha_actual = month;
                    decimal mora_mes_pasado = 0.00M;
                    DateTime fecha_mes_pasado = DateTime.Now ;
                    List<decimal> valores_decimal = new List<decimal>();
                    List<string> valores_date = new List<string>();
                    Grafico_Colocacion_Asesores data = new Grafico_Colocacion_Asesores();
                    List<Grafico_Colocacion_Asesores> valores_por_fecha = new List<Grafico_Colocacion_Asesores>();
                    List<Rancking> valores_colecta = new List<Rancking>();
                    if (User.IsInRole("Asesor"))
                    {
                        Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                        Usuario_Agencia uag = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == userId);
                        id_agencia = uag.Id_Agencia;
                        id_cartera_correcto = ua.Id_Cartera;
                    }
                    else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                    {
                        string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                        Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                        id_agencia = ua.Id_Agencia;
                        id_cartera_correcto = id_cartera;
                    }
                    else
                    {
                        id_cartera_correcto = id_cartera;
                    }

                    Func<Rancking, bool> tiene_ranking = new Func<Rancking, bool>
                        (a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == month && a.Fecha.Year == anio);
                    if ((User.IsInRole("Administrador")) | (User.IsInRole("Gerente")) | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                    {
                        asesores = (from c in _context.Cartera
                                    join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                    where ua.Id_Agencia == id_agencia && !c.Excluido
                                    select c).ToList();
                        IEnumerable<IGrouping<DateTime, Rancking>> personas_ahorros = _context.Rancking
                           .Where(d => d.Fecha.Month == month && d.Fecha.Year == anio )
                           .OrderBy(c => c.Id_Rancking)
                           .AsEnumerable()
                           .GroupBy(d => d.Fecha);
                        foreach (var f_ch in personas_ahorros)
                        {
                            foreach (var asesore in asesores)
                            {
                                Func<Rancking, bool> tiene_ranking_asesor = new Func<Rancking, bool>
                                    (a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Day == f_ch.Key.Day && a.Fecha.Year == anio);

                                if (_context.Rancking.Any(tiene_ranking_asesor))
                                {
                                    valores_colecta = _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                     && a.Fecha.Date == f_ch.Key.Date && a.Fecha.Year == anio)
                                        .OrderBy(a => a.Fecha).ToList();
                                    valores_por_fecha.Add(new Grafico_Colocacion_Asesores
                                    {
                                        fecha = f_ch.Key.Day,
                                        fechas = f_ch.Key,
                                        valor = valores_colecta.Sum(a => a.Capital_Mora)
                                    });
                                }
                            }
                        }
                        foreach (var asesore in asesores)
                        {
                            Func<Rancking, bool> tiene_ranking_mes_pasado = new Func<Rancking, bool>
                                (a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == month - 1 );
                            if (_context.Rancking.Any(tiene_ranking_mes_pasado))
                            {
                                mora_mes_pasado += _context.Rancking.AsNoTracking().Where(
                                a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == month - 1  )
                               .OrderBy(a => a.Id_Rancking).Last().Capital_Mora;

                                fecha_mes_pasado = _context.Rancking.AsNoTracking().Where(
                                a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == month - 1)
                               .OrderBy(a => a.Id_Rancking).Last().Fecha.Date;
                            }
                        }
                        string fech1a = fecha_mes_pasado.ToString("dd/MM/yyyy");
                        valores_decimal.Add(mora_mes_pasado);
                        valores_date.Add(fech1a);
                        data.valores = valores_decimal;
                        data.fecha_completa = valores_date;

                        var query = (from cc in valores_por_fecha
                                     group cc by cc.fecha into newgroup
                                     select new Grafico_Colocacion_Asesores
                                     {
                                         fechas = newgroup.Max(a => a.fechas.Date),
                                         valor = newgroup.Sum(c => c.valor)
                                     }).OrderBy(c => c.fecha)
                                     .ToList();
                        foreach (var ff in query)
                        {
                            string fecha = ff.fechas.ToString("dd/MM/yyyy");
                            valores_decimal.Add(ff.valor);
                            data.valores = valores_decimal;
                            valores_date.Add(fecha);
                            data.fecha_completa = valores_date;
                        }
                        data.Diferencia_Mora = data.valores.Last() - data.valores.First();
                        return data;
                    } else if (User.IsInRole("Administrador") && id_agencia == 0) {
                        asesores = (from c in _context.Cartera
                                    join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                    where !c.Excluido
                                    select c).ToList();
                        IEnumerable<IGrouping<DateTime, Rancking>> personas_ahorros = _context.Rancking
                           .Where(d => d.Fecha.Month == month && d.Fecha.Year == anio)
                           .OrderBy(c => c.Id_Rancking)
                           .AsEnumerable()
                           .GroupBy(d => d.Fecha);
                        foreach (var f_ch in personas_ahorros)
                        {
                            foreach (var asesore in asesores)
                            {
                                Func<Rancking, bool> tiene_ranking_asesor = new Func<Rancking, bool>
                                    (a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Day == f_ch.Key.Day && a.Fecha.Year == anio);

                                if (_context.Rancking.Any(tiene_ranking_asesor))
                                {
                                    valores_colecta = _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera
                                     && a.Fecha.Date == f_ch.Key.Date && a.Fecha.Year == anio)
                                        .OrderBy(a => a.Fecha).ToList();
                                    valores_por_fecha.Add(new Grafico_Colocacion_Asesores
                                    {
                                        fecha = f_ch.Key.Day,
                                        fechas = f_ch.Key,
                                        valor = valores_colecta.Sum(a => a.Capital_Mora)
                                    });

                                }
                            }
                        }
                        foreach (var asesore in asesores)
                        {
                            Func<Rancking, bool> tiene_ranking_mes_pasado = new Func<Rancking, bool>
                                (a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == month - 1);
                            if (_context.Rancking.Any(tiene_ranking_mes_pasado))
                            {

                                mora_mes_pasado += _context.Rancking.AsNoTracking().Where(
                                a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == month - 1)
                               .OrderBy(a => a.Id_Rancking).Last().Capital_Mora;

                                fecha_mes_pasado = _context.Rancking.AsNoTracking().Where(
                                a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == month - 1)
                               .OrderBy(a => a.Id_Rancking).Last().Fecha.Date;
                            }
                        }
                        string fech1a = fecha_mes_pasado.ToString("dd/MM/yyyy");
                        valores_decimal.Add(mora_mes_pasado);
                        valores_date.Add(fech1a);
                        data.valores = valores_decimal;
                        data.fecha_completa = valores_date;

                        var query = (from cc in valores_por_fecha
                                     group cc by cc.fecha into newgroup
                                     select new Grafico_Colocacion_Asesores
                                     {
                                         fechas = newgroup.Max(a => a.fechas.Date),
                                         valor = newgroup.Sum(c => c.valor)
                                     }).OrderBy(c => c.fecha)
                                     .ToList();
                        foreach (var ff in query)
                        {
                            string fecha = ff.fechas.ToString("dd/MM/yyyy");
                            valores_decimal.Add(ff.valor);
                            data.valores = valores_decimal;
                            valores_date.Add(fecha);
                            data.fecha_completa = valores_date;
                        }
                        data.Diferencia_Mora = data.valores.Last() - data.valores.First();
                        return data;
                    }
                    else
                    {
                        if (_context.Rancking.Any(tiene_ranking))
                        {
                            int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;
                            int anio_rancking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Year == anio)
                               .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Year;
                            Func<Rancking, bool> tiene_ranking_mes_pasado = new Func<Rancking, bool>
                                (a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == month - 1);
                            if (_context.Rancking.Any(tiene_ranking_mes_pasado))
                            {

                                mora_mes_pasado += _context.Rancking.AsNoTracking().Where(
                                a => a.Id_Cartera == id_cartera_correcto &&
                                a.Fecha.Month == month - 1)
                               .OrderBy(a => a.Id_Rancking).Last().Capital_Mora;

                                fecha_mes_pasado = _context.Rancking.AsNoTracking().Where(
                                a => a.Id_Cartera == id_cartera_correcto &&
                                a.Fecha.Month == month - 1)
                               .OrderBy(a => a.Id_Rancking).Last().Fecha.Date;
                            }
                            string fech1a = fecha_mes_pasado.ToString("dd/MM/yyyy");
                            valores_decimal.Add(mora_mes_pasado);
                            valores_date.Add(fech1a);
                            data.valores = valores_decimal;
                            data.fecha_completa = valores_date;
                            if (fecha_actual == fecha_ranking && anio == anio_rancking)
                            {
                                valores_colecta = _context.Rancking.Where(a => a.Id_Cartera == id_cartera_correcto
                                && a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).ToList();
                                foreach (var data_Asesor in valores_colecta)
                                {
                                    string fecha = data_Asesor.Fecha.ToString("dd/MM/yyyy");
                                    valores_decimal.Add(data_Asesor.Capital_Mora);
                                    data.valores = valores_decimal;
                                    valores_date.Add(fecha);
                                    data.fecha_completa = valores_date;
                                };
                            }
                        }
                        data.Diferencia_Mora = data.valores.Last() - data.valores.First();
                        return data;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

        }
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<Rancking> Fichero(int id_cartera = 0, int month = 0, int id_agencia = 0, int anio =0)
        {
            try
            {
                var asesores = new List<Cartera>();
                string userId = _userManager.GetUserId(HttpContext.User);
                int id_cartera_correcto = 0;
                int fecha_actual = 0;
                fecha_actual = month;
                Rancking porcentajes = new Rancking();
                if (User.IsInRole("Asesor"))
                {
                    Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                    id_cartera_correcto = ua.Id_Cartera;
                }
                else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                    id_agencia = ua.Id_Agencia;
                    id_cartera_correcto = id_cartera;
                }
                else
                {
                    id_cartera_correcto = id_cartera;
                }
                decimal abono_capital = 0.00M;
                int clientes_Activos = 0;
                decimal cartera = 0.00M;
                decimal indice_mora = 0.00M;
                if ((User.IsInRole("Administrador")) | (User.IsInRole("Gerente")) | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia
                                select c).ToList();
                    foreach (var asesore in asesores)
                    {
                        bool t_meta = _context.Meta.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Mes == fecha_actual && a.Anyo == anio);
                        Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_meta);
                        Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera && a.Anyo == anio);
                        int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;
                        int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Mes;
                        int anio_ranking = _context.Meta.AsNoTracking().Where(a => a.Anyo == anio)
                               .OrderBy(a => a.Id_Meta).Last().Anyo;
                        if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_ranking)
                        {
                            if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                            {
                                //clientes activos
                                clientes_Activos += _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().No_Clientes_Activos;
                            }
                            if (!asesore.Excluido && _context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                            {
                                //caida cartera
                                abono_capital += _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().Abono_Capital;
                                //valor cartera
                                cartera += _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().Saldo_Cartera;
                                //mora = capital en mora/ cartera
                                indice_mora += _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().Capital_Mora;
                            }
                        }
                    }
                    porcentajes.Caida_Cartera = Math.Round(abono_capital, 2);
                    porcentajes.No_Clientes_Activos = clientes_Activos;
                    porcentajes.Saldo_Cartera = Math.Round(cartera, 2);
                    porcentajes.Indice_Mora = Math.Round((indice_mora) / cartera, 2) * 100;
                } else if (User.IsInRole("Administrador") && id_agencia == 0) {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                select c).ToList();
                    foreach (var asesore in asesores)
                    {
                        bool t_meta = _context.Meta.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Mes == fecha_actual && a.Anyo == anio);
                        Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_meta);
                        Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == asesore.Id_Cartera && a.Anyo == anio);
                        int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                                .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;
                        int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Mes;
                        int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Anyo;
                        if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                        {
                            if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                            {
                                //clientes activos
                                clientes_Activos += _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().No_Clientes_Activos;
                            }
                            if (!asesore.Excluido && _context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                            {
                                //caida cartera
                                abono_capital += _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().Abono_Capital;
                                //valor cartera
                                cartera += _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().Saldo_Cartera;
                                //mora = capital en mora/ cartera
                                indice_mora += _context.Rancking.Where(a => a.Id_Cartera == asesore.Id_Cartera &&
                                a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().Capital_Mora;
                            }
                        }
                    }
                    porcentajes.Caida_Cartera = Math.Round(abono_capital, 2);
                    porcentajes.No_Clientes_Activos = clientes_Activos;
                    porcentajes.Saldo_Cartera = Math.Round(cartera, 2);
                    porcentajes.Indice_Mora = Math.Round((indice_mora) / cartera, 2) * 100;
                }
                else
                {
                    bool t_meta = _context.Meta.AsNoTracking()
                        .Any(a => a.Id_Cartera == id_cartera_correcto && a.Mes == fecha_actual && a.Anyo == anio);
                    Func<Meta, bool> tiene_meta = new Func<Meta, bool>(a => a.Id_Cartera == id_cartera_correcto && t_meta);
                    Func<Meta, bool> tiene_ranking = new Func<Meta, bool>(a => a.Id_Cartera == id_cartera_correcto && a.Anyo == anio);

                    if (_context.Meta.Any(tiene_meta) && _context.Meta.Any(tiene_ranking))
                    {
                        int fecha_ranking = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Fecha.Year == anio)
                            .OrderBy(a => a.Id_Rancking).Last().Fecha.Date.Month;

                        int fecha_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Mes;
                        int anio_meta = _context.Meta.AsNoTracking().Where(a => a.Mes == month && a.Anyo == anio)
                            .OrderBy(a => a.Id_Meta).Last().Anyo;

                        if (fecha_actual == fecha_ranking && fecha_actual == fecha_meta && anio == anio_meta)
                        {

                            //caida cartera
                            abono_capital += _context.Rancking.Where(a => a.Id_Cartera == id_cartera_correcto &&
                            a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().Abono_Capital;
                            //clientes activos
                            clientes_Activos += _context.Rancking.Where(a => a.Id_Cartera == id_cartera_correcto &&
                            a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().No_Clientes_Activos;
                            //valor cartera
                            cartera += _context.Rancking.Where(a => a.Id_Cartera == id_cartera_correcto &&
                            a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().Saldo_Cartera;
                            //mora = capital en mora/ cartera
                            indice_mora += _context.Rancking.Where(a => a.Id_Cartera == id_cartera_correcto &&
                            a.Fecha.Month == fecha_ranking && a.Fecha.Year == anio).OrderBy(b => b.Id_Rancking).Last().Capital_Mora;

                            porcentajes.Caida_Cartera = Math.Round(abono_capital, 2);
                            porcentajes.No_Clientes_Activos = clientes_Activos;
                            porcentajes.Saldo_Cartera = Math.Round(cartera, 2);
                            porcentajes.Indice_Mora = Math.Round((indice_mora) / cartera, 2) * 100;
                        }
                    }
                    else
                    {
                        return porcentajes;
                    }
                }
                return porcentajes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        //Procesos Mora
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<Reporte_Mora> Fichero_Mora(int id_cartera = 0, int month = 0, int id_agencia = 0)
        {
            try
            {
                var asesores = new List<Cartera>();
                string userId = _userManager.GetUserId(HttpContext.User);
                int id_cartera_correcto = 0;
                int fecha_actual = 0;
                fecha_actual = month;
                Reporte_Mora datos = new Reporte_Mora();
                if (User.IsInRole("Asesor"))
                {
                    Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                    id_cartera_correcto = ua.Id_Cartera;
                }
                else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                    id_agencia = ua.Id_Agencia;
                    id_cartera_correcto = id_cartera;
                }
                else
                {
                    id_cartera_correcto = id_cartera;
                }
                decimal mora_total = 0.00M;
                int clientes_mora = 0;
                decimal vencidos = 0.00M;
                if ((User.IsInRole("Administrador")) | (User.IsInRole("Gerente")) | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia && !c.Excluido
                                select c).ToList();
                    int contador = 0;
                    foreach (var asesore in asesores)
                    {
                        bool t_mora = _context.Reporte_Mora.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_actual);
                        Func<Reporte_Mora, bool> tiene_mora = new Func<Reporte_Mora, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_mora);
                     
                        int fecha_mora = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Month;
                        List<Reporte_Mora> lista_mora = _context.Reporte_Mora.AsNoTracking()
                            .Where(a=>a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_mora).ToList();
                        DateTime ultima_fecha = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Date;
                        foreach (var mora in lista_mora )
                        {
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha )
                            {
                                contador++;
                                mora_total += mora.Saldo_Mora;
                                clientes_mora = contador;
                            }
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                                && mora.Fecha_Vencimiento.Date < ultima_fecha) {
                                vencidos += mora.Saldo_Mora;
                            }
                        }
                    }
                    datos.Mora_Total = Math.Round(mora_total, 2);
                    datos.Clientes_Mora = clientes_mora;
                    datos.Vencidos = Math.Round(vencidos, 2);
                }
                    else if (User.IsInRole("Administrador") && id_agencia == 0)
                    {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                select c).ToList();
                    int contador = 0;
                    foreach (var asesore in asesores)
                    {
                        bool t_mora = _context.Reporte_Mora.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_actual);
                        Func<Reporte_Mora, bool> tiene_mora = new Func<Reporte_Mora, bool>(a => a.Id_Cartera == asesore.Id_Cartera 
                        && t_mora);

                        int fecha_mora = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Month;
                        List<Reporte_Mora> lista_mora = _context.Reporte_Mora.AsNoTracking()
                            .Where(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_mora).ToList();
                        DateTime ultima_fecha = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Date;
                        foreach (var mora in lista_mora)
                        {
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha)
                            {
                                contador++;
                                mora_total += mora.Saldo_Mora;
                                clientes_mora = contador;
                            }
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                                && mora.Fecha_Vencimiento.Date < ultima_fecha)
                            {
                                vencidos += mora.Saldo_Mora;
                            }
                        }
                    }
                    datos.Mora_Total = Math.Round(mora_total, 2);
                    datos.Clientes_Mora = clientes_mora;
                    datos.Vencidos = Math.Round(vencidos, 2);
                }
                else
                {
                    int contador = 0;
                    bool t_mora = _context.Reporte_Mora.AsNoTracking()
                               .Any(a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == fecha_actual);
                    Func<Reporte_Mora, bool> tiene_mora = new Func<Reporte_Mora, bool>(a => a.Id_Cartera == id_cartera_correcto
                    && t_mora);

                    int fecha_mora = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                        .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Month;
                    List<Reporte_Mora> lista_mora = _context.Reporte_Mora.AsNoTracking()
                        .Where(a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == fecha_mora).ToList();
                    DateTime ultima_fecha = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                        .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Date;
                    foreach (var mora in lista_mora)
                    {
                        if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha)
                        {
                            contador++;
                            mora_total += mora.Saldo_Mora;
                            clientes_mora = contador;
                        }
                        if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                            && mora.Fecha_Vencimiento.Date < ultima_fecha)
                        {
                            vencidos += mora.Saldo_Mora;
                        }
                    }
                    datos.Mora_Total = Math.Round(mora_total, 2);
                    datos.Clientes_Mora = clientes_mora;
                    datos.Vencidos = Math.Round(vencidos, 2);
                }
                return datos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<Reporte_Mora> Ultimos_Abonos_Mora(int id_cartera = 0, int month = 0, int id_agencia = 0)
        {
            try
            {
                var asesores = new List<Cartera>();
                string userId = _userManager.GetUserId(HttpContext.User);
                int id_cartera_correcto = 0;
                int fecha_actual = 0;
                fecha_actual = month;
                Reporte_Mora datos = new Reporte_Mora();
                if (User.IsInRole("Asesor"))
                {
                    Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                    id_cartera_correcto = ua.Id_Cartera;
                }
                else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                    id_agencia = ua.Id_Agencia;
                    id_cartera_correcto = id_cartera;
                }
                else
                {
                    id_cartera_correcto = id_cartera;
                }
                decimal saldo_menor_quince = 0.00M;
                int clientes_menor_quince = 0;
                decimal saldo_diesiseis_treinta = 0.00M;
                int clientes_diesiseis_treinta = 0;
                decimal saldo_treinauno_sesenta = 0.00M;
                int clientes_treinauno_sesenta = 0;
                decimal saldo_mayor_sesenta_uno = 0.00M;
                int clientes_mayor_sesenta_uno = 0;
                int diferencia_fecha = 0;
                if ((User.IsInRole("Administrador")) | (User.IsInRole("Gerente")) | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia && !c.Excluido
                                select c).ToList();
                    int contador = 0;
                    int contador2 = 0;
                    int contador3 = 0;
                    int contador4 = 0;
                    foreach (var asesore in asesores)
                    {
                        bool t_mora = _context.Reporte_Mora.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_actual);
                        Func<Reporte_Mora, bool> tiene_mora = new Func<Reporte_Mora, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_mora);

                        int fecha_mora = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Month;
                        List<Reporte_Mora> lista_mora = _context.Reporte_Mora.AsNoTracking()
                            .Where(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_mora).ToList();
                        DateTime ultima_fecha = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Date;
                        foreach (var mora in lista_mora)
                        {
                            TimeSpan dias = DateTime.Now - mora.Fecha_Ultimo_Movimiento;
                            diferencia_fecha = dias.Days;
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha && diferencia_fecha <=15 )
                            {
                                contador++;
                                saldo_menor_quince += mora.Saldo_Mora;
                                clientes_menor_quince = contador;
                            }
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha 
                                && diferencia_fecha >= 16 && diferencia_fecha <=30)
                            {
                                contador2++;
                                saldo_diesiseis_treinta += mora.Saldo_Mora;
                                clientes_diesiseis_treinta = contador2;
                            }
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                              && diferencia_fecha >= 31 && diferencia_fecha <= 60)
                            {
                                contador3++;
                                saldo_treinauno_sesenta += mora.Saldo_Mora;
                                clientes_treinauno_sesenta = contador3;
                            }
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                              &&  diferencia_fecha >= 61)
                            {
                                contador4++;
                                saldo_mayor_sesenta_uno += mora.Saldo_Mora;
                                clientes_mayor_sesenta_uno = contador4;
                            }
                        }
                    }
                    datos.Saldo_Menor_Quince = Math.Round(saldo_menor_quince, 2);
                    datos.Clientes_Menor_Quince = clientes_menor_quince;
                    datos.Saldo_Dieciseis_treina = Math.Round(saldo_diesiseis_treinta, 2);
                    datos.Clientes_Dieciseis_treina = clientes_diesiseis_treinta;
                    datos.Saldo_Treinta_Uno_Sesenta = Math.Round(saldo_treinauno_sesenta, 2);
                    datos.Clientes_Treinta_Uno_Sesenta = clientes_treinauno_sesenta;
                    datos.Saldo_Mayor_Sesenta_Uno = Math.Round(saldo_mayor_sesenta_uno, 2);
                    datos.Clientes_Mayor_Sesenta_Uno = clientes_mayor_sesenta_uno;
                }
                else if (User.IsInRole("Administrador") && id_agencia == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                select c).ToList();
                    int contador = 0;
                    int contador2 = 0;
                    int contador3 = 0;
                    int contador4 = 0;
                    foreach (var asesore in asesores)
                    {
                        bool t_mora = _context.Reporte_Mora.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_actual);
                        Func<Reporte_Mora, bool> tiene_mora = new Func<Reporte_Mora, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_mora);

                        int fecha_mora = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Month;
                        List<Reporte_Mora> lista_mora = _context.Reporte_Mora.AsNoTracking()
                            .Where(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_mora).ToList();
                        DateTime ultima_fecha = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Date;
                        foreach (var mora in lista_mora)
                        {
                            TimeSpan dias = DateTime.Now - mora.Fecha_Ultimo_Movimiento;
                            diferencia_fecha = dias.Days;
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha && diferencia_fecha <= 15)
                            {
                                contador++;
                                saldo_menor_quince += mora.Saldo_Mora;
                                clientes_menor_quince = contador;
                            }
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                                && diferencia_fecha >= 16 && diferencia_fecha <= 30)
                            {
                                contador2++;
                                saldo_diesiseis_treinta += mora.Saldo_Mora;
                                clientes_diesiseis_treinta = contador2;
                            }
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                              && diferencia_fecha >= 31 && diferencia_fecha <= 60)
                            {
                                contador3++;
                                saldo_treinauno_sesenta += mora.Saldo_Mora;
                                clientes_treinauno_sesenta = contador3;
                            }
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                              && diferencia_fecha >= 61)
                            {
                                contador4++;
                                saldo_mayor_sesenta_uno += mora.Saldo_Mora;
                                clientes_mayor_sesenta_uno = contador4;
                            }
                        }
                    }
                    datos.Saldo_Menor_Quince = Math.Round(saldo_menor_quince, 2);
                    datos.Clientes_Menor_Quince = clientes_menor_quince;
                    datos.Saldo_Dieciseis_treina = Math.Round(saldo_diesiseis_treinta, 2);
                    datos.Clientes_Dieciseis_treina = clientes_diesiseis_treinta;
                    datos.Saldo_Treinta_Uno_Sesenta = Math.Round(saldo_treinauno_sesenta, 2);
                    datos.Clientes_Treinta_Uno_Sesenta = clientes_treinauno_sesenta;
                    datos.Saldo_Mayor_Sesenta_Uno = Math.Round(saldo_mayor_sesenta_uno, 2);
                    datos.Clientes_Mayor_Sesenta_Uno = clientes_mayor_sesenta_uno;
                }
                else
                {
                    int contador = 0;
                    int contador2 = 0;
                    int contador3 = 0;
                    int contador4 = 0;
                    bool t_mora = _context.Reporte_Mora.AsNoTracking()
                                   .Any(a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == fecha_actual);
                    Func<Reporte_Mora, bool> tiene_mora = new Func<Reporte_Mora, bool>(a => a.Id_Cartera == id_cartera_correcto && t_mora);

                    int fecha_mora = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                        .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Month;
                    DateTime ultima_fecha = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                      .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Date;
                    List<Reporte_Mora> lista_mora = _context.Reporte_Mora.AsNoTracking()
                        .Where(a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == fecha_mora
                        && a.Fecha.Date == ultima_fecha).ToList();
                  
                    foreach (var mora in lista_mora)
                    {
                        TimeSpan dias = DateTime.Now - mora.Fecha_Ultimo_Movimiento;
                        diferencia_fecha = dias.Days;
                        if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha && diferencia_fecha <= 15)
                        {
                            contador++;
                            saldo_menor_quince += mora.Saldo_Mora;
                            clientes_menor_quince = contador;
                        }
                        if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                            && diferencia_fecha >= 16 && diferencia_fecha <= 30)
                        {
                            contador2++;
                            saldo_diesiseis_treinta += mora.Saldo_Mora;
                            clientes_diesiseis_treinta = contador2;
                        }
                        if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                          && diferencia_fecha >= 31 && diferencia_fecha <= 60)
                        {
                            contador3++;
                            saldo_treinauno_sesenta += mora.Saldo_Mora;
                            clientes_treinauno_sesenta = contador3;
                        }
                        if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha
                          && diferencia_fecha >= 61)
                        {
                            contador4++;
                            saldo_mayor_sesenta_uno += mora.Saldo_Mora;
                            clientes_mayor_sesenta_uno = contador4;
                        }
                    }
                    datos.Saldo_Menor_Quince = Math.Round(saldo_menor_quince, 2);
                    datos.Clientes_Menor_Quince = clientes_menor_quince;
                    datos.Saldo_Dieciseis_treina = Math.Round(saldo_diesiseis_treinta, 2);
                    datos.Clientes_Dieciseis_treina = clientes_diesiseis_treinta;
                    datos.Saldo_Treinta_Uno_Sesenta = Math.Round(saldo_treinauno_sesenta, 2);
                    datos.Clientes_Treinta_Uno_Sesenta = clientes_treinauno_sesenta;
                    datos.Saldo_Mayor_Sesenta_Uno = Math.Round(saldo_mayor_sesenta_uno, 2);
                    datos.Clientes_Mayor_Sesenta_Uno = clientes_mayor_sesenta_uno;
                }
                return datos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<List<Reporte_Mora_Deudores>> Principales_Deudores(int id_cartera = 0, int month = 0, int id_agencia = 0)
        {
            try
            {
                var asesores = new List<Cartera>();
                string userId = _userManager.GetUserId(HttpContext.User);
                int id_cartera_correcto = 0;
                int fecha_actual = 0;
                fecha_actual = month;
                List<Reporte_Mora_Deudores> datos = new List<Reporte_Mora_Deudores>();
                if (User.IsInRole("Asesor"))
                {
                    Cartera ua = _context.Cartera.AsNoTracking().Single(a => a.Id_Usuario == userId);
                    id_cartera_correcto = ua.Id_Cartera;
                }
                else if (User.IsInRole("Gerente") || User.IsInRole("Jefe"))
                {
                    string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                    Usuario_Agencia ua = _context.Usuario_Agencia.AsNoTracking().Single(a => a.Id_Usuario == Id_Usuario);
                    id_agencia = ua.Id_Agencia;
                    id_cartera_correcto = id_cartera;
                }
                else
                {
                    id_cartera_correcto = id_cartera;
                }
                decimal mora_total = 0.00M;
                int clientes_mora = 0;
                decimal vencidos = 0.00M;
                if ((User.IsInRole("Administrador")) | (User.IsInRole("Gerente")) | User.IsInRole("Jefe") && id_agencia > 0 && id_cartera == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                where ua.Id_Agencia == id_agencia
                                select c).ToList();
                    decimal cartera_agencia = 0.00M;
                    foreach (var ca in asesores) { 
                        cartera_agencia+= _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Id_Cartera == ca.Id_Cartera)
                            .OrderBy(a => a.Id_Rancking).Last().Saldo_Cartera;
                    }
                    foreach (var asesore in asesores)
                    {
                        bool t_mora = _context.Reporte_Mora.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_actual);
                        Func<Reporte_Mora, bool> tiene_mora = new Func<Reporte_Mora, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_mora);

                        int fecha_mora = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Month;
                        List<Reporte_Mora> lista_mora = _context.Reporte_Mora.AsNoTracking()
                            .Where(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_mora).ToList();
                        DateTime ultima_fecha = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Date;
                        foreach (var mora in lista_mora)
                        {
                            decimal porcentaje = 0.00M;
                     
                            porcentaje = mora.Saldo_Mora / cartera_agencia;
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha && mora.Dias_Mora >30 
                                && porcentaje *100 > 3 )
                            {
                                datos.Add(new Reporte_Mora_Deudores
                                {
                                   Codigo_Cliente = mora.Codigo,
                                   Nombre_Cliente = mora.Nombre_Cliente,
                                   Saldo_mora = mora.Saldo_Mora,
                                   Porcentaje = Math.Round(porcentaje*100),
                                   Dias = mora.Dias_Mora
                                });
                            }
                        
                        }
                    }
        
                }
                else if (User.IsInRole("Administrador") && id_agencia == 0)
                {
                    asesores = (from c in _context.Cartera
                                join ua in _context.Usuario_Agencia on c.Id_Usuario equals ua.Id_Usuario
                                select c).ToList();
                    foreach (var asesore in asesores)
                    {
                        bool t_mora = _context.Reporte_Mora.AsNoTracking()
                            .Any(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_actual);
                        Func<Reporte_Mora, bool> tiene_mora = new Func<Reporte_Mora, bool>(a => a.Id_Cartera == asesore.Id_Cartera && t_mora);

                        int fecha_mora = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Month;
                        List<Reporte_Mora> lista_mora = _context.Reporte_Mora.AsNoTracking()
                            .Where(a => a.Id_Cartera == asesore.Id_Cartera && a.Fecha.Month == fecha_mora).ToList();
                        DateTime ultima_fecha = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                            .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Date;
                        foreach (var mora in lista_mora)
                        {
                            decimal porcentaje = 0.00M;
                            decimal cartera = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Id_Cartera == mora.Id_Cartera)
                            .OrderBy(a => a.Id_Rancking).Last().Saldo_Cartera;
                            porcentaje = mora.Saldo_Mora / cartera;
                            if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha && mora.Dias_Mora > 15
                                && porcentaje >= 0.03M)
                            {
                                datos.Add(new Reporte_Mora_Deudores
                                {
                                    Codigo_Cliente = mora.Codigo,
                                    Nombre_Cliente = mora.Nombre_Cliente,
                                    Saldo_mora = mora.Saldo_Mora,
                                    Porcentaje = Math.Round(porcentaje * 100),
                                    Dias = mora.Dias_Mora
                                });
                            }

                        }
                    }
                }
                else
                {
                    bool t_mora = _context.Reporte_Mora.AsNoTracking()
                                 .Any(a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == fecha_actual);
                    Func<Reporte_Mora, bool> tiene_mora = new Func<Reporte_Mora, bool>(a => a.Id_Cartera == id_cartera_correcto && t_mora);

                    int fecha_mora = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                        .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Month;
                    List<Reporte_Mora> lista_mora = _context.Reporte_Mora.AsNoTracking()
                        .Where(a => a.Id_Cartera == id_cartera_correcto && a.Fecha.Month == fecha_mora).ToList();
                    DateTime ultima_fecha = _context.Reporte_Mora.AsNoTracking().Where(a => a.Fecha.Month == month)
                        .OrderBy(a => a.Id_Reporte_Mora).Last().Fecha.Date;
                    foreach (var mora in lista_mora)
                    {
                        decimal porcentaje = 0.00M;
                        decimal cartera = _context.Rancking.AsNoTracking().Where(a => a.Fecha.Month == month && a.Id_Cartera == mora.Id_Cartera)
                        .OrderBy(a => a.Id_Rancking).Last().Saldo_Cartera;
                        porcentaje = mora.Saldo_Mora / cartera;
                        if (mora.Fecha.Month == fecha_mora && mora.Fecha.Date == ultima_fecha && mora.Dias_Mora > 15
                            && porcentaje *100 > 3)
                        {
                            datos.Add(new Reporte_Mora_Deudores
                            {
                                Codigo_Cliente = mora.Codigo,
                                Nombre_Cliente = mora.Nombre_Cliente,
                                Saldo_mora = mora.Saldo_Mora,
                                Porcentaje = Math.Round(porcentaje * 100),
                                Dias = mora.Dias_Mora
                            });
                        }
                    }
                }
                return datos.OrderByDescending(x=>x.Porcentaje).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [Authorize(Roles = "Asesor,Administrador,Gerente,Jefe", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<List<Reporte_Mora_Deudores>> Reporte_mora( DateTime fecha,int id_cartera = 0)
        {
            try
            {
                List<Reporte_Mora_Deudores> datos = new List<Reporte_Mora_Deudores>();
                var datos_mora = new List<Reporte_Mora>();
                datos_mora = (from c in _context.Reporte_Mora
                              where  c.Id_Cartera == id_cartera && c.Fecha.Date == fecha.Date
                              select c).ToList();
                foreach (var mora in datos_mora)
                {
                    string variable = mora.Codigo.Substring(7,2);
                    if (variable == "26") {
                        datos.Add(new Reporte_Mora_Deudores
                        {
                            Codigo_Cliente = mora.Codigo,
                            Nombre_Cliente = mora.Nombre_Cliente,
                            Saldo_mora = mora.Saldo_Mora,
                            Dias = mora.Dias_Mora
                        });
                    }
                }
                    return datos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

}
