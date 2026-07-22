using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using ModelNucleo.Util;
using NucleoAyC.Entities;
using Org.BouncyCastle.Utilities;

namespace NucleoAyC.Controllers
{
    /// <summary>
    /// clase que posee método en común para
    /// transacción crédito y ahorro y simple
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true, GroupName = "Base_Transaccion")]
    public class Base_TransaccionController : Controller
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly decimal IVA = 13.00M;
        protected readonly NegocioContexto _context;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public Base_TransaccionController(
            NegocioContexto context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

            //información del iva
            string iva_str = _context.Configuracion_General.AsNoTracking().Single(d => d.Nombre == "IVA").Valor;
            IVA = Convert.ToDecimal(iva_str);
        }

        /*relativo a usuario, agencia y rol*/
        /// <summary>
        /// retorna el id de la agencia por el usuario que inició sesión
        /// </summary>
        /// <returns></returns>
        protected int Agencia_Usuario()
        {
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            return _context.Usuario_Agencia.AsNoTracking().Single(s => s.Id_Usuario == Id_Usuario).Id_Agencia;
        }

        /*relativo a usuario, agencia y rol*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaccion">cualquier tipo de las transaccines</param>
        /// <param name="id_caja">identificador de la caja</param>
        /// <param name="fecha">fecha que se efectúa la transacción</param>
        /// <param name="ingreso">se sabe si es o no un ingreso para la caja</param>
        /// <param name="comprobante">se sabe si es o no un ingreso para la caja</param>
        /// <returns></returns>
        protected Transaccion_Caja Registrar_Caja(object transaccion, int id_caja, DateTime fecha, bool ingreso, string comprobante = "")
        {
            string ingreso_str = "Egreso";
            if (ingreso)
                ingreso_str = "Ingreso";

            int id_transaccion_guardada = 0;
            decimal monto = 0.00M;
            if (transaccion is Transaccion_Credito)
            {
                Transaccion_Credito transaccion_credito = transaccion as Transaccion_Credito;
                id_transaccion_guardada = transaccion_credito.Id_Transaccion_Credito;
                monto = transaccion_credito.Valor_Cuota;
            }

            if (transaccion is Transaccion_Ahorro)
            {
                Transaccion_Ahorro transaccion_ahorro = transaccion as Transaccion_Ahorro;
                id_transaccion_guardada = transaccion_ahorro.Id_Transaccion_Ahorro;
                monto = transaccion_ahorro.Valor;
            }

            if (transaccion is Transaccion_Simple)
            {
                Transaccion_Simple transaccion_simple = transaccion as Transaccion_Simple;
                id_transaccion_guardada = transaccion_simple.Id_Transaccion_Simple;
                monto = transaccion_simple.Monto;
            }
            if (transaccion is Transaccion_Deposito_Plazo)
            {
                Transaccion_Deposito_Plazo transaccion_deposito = transaccion as Transaccion_Deposito_Plazo;
                id_transaccion_guardada = transaccion_deposito.Id_Transaccion_Deposito_Plazo;
                monto = transaccion_deposito.Valor;
            }
            //int id_caja = _context.Caja.FirstOrDefault(y => y.Id_Usuario == Id_Usuario && y.Activa && !y.Boveda).Id_Caja;
            int id_tipo_trans = _context.Tipo_Transaccion.SingleOrDefault(t => t.Descripcion == ingreso_str).Id_Tipo_Transaccion;
            
            Transaccion_Caja ultima_transaccion = new Transaccion_Caja();
            
            if (_context.Transaccion_Caja.Any(t => t.Id_Caja == id_caja && !t.Borrado_Logico))
            {
                ultima_transaccion = _context.
                    Transaccion_Caja.
                    Where(t => t.Id_Caja == id_caja && !t.Borrado_Logico).AsEnumerable().Last();
            }

            decimal monto_transaccion = transaccion != null ? monto : 0.00M;
            decimal saldo_anterior = ultima_transaccion != null ? ultima_transaccion.Saldo : 0.00M;
            string id_usuario = _userManager.GetUserId(HttpContext.User);

            return new Transaccion_Caja
            {
                Id_Transaccion_Credito = transaccion is Transaccion_Credito ? id_transaccion_guardada : 0,
                Id_Transaccion_Ahorro = transaccion is Transaccion_Ahorro ? id_transaccion_guardada : 0,
                Id_Transaccion_Simple = transaccion is Transaccion_Simple ? id_transaccion_guardada : 0,
                Id_Transaccion_Deposito_Plazo = transaccion is Transaccion_Deposito_Plazo ? id_transaccion_guardada : 0,
                Id_Caja = id_caja,
                Id_Tipo_Transaccion = id_tipo_trans,
                Monto = monto_transaccion,
                Saldo = ingreso ? saldo_anterior + monto_transaccion : saldo_anterior - monto_transaccion,
                Fecha = fecha.Date,
                No_Comprobante = comprobante,
                Creado_En = fecha,
                Creado_Por = id_usuario
            };
        }

        /// <summary>
        /// Asiento contable de un pago de crédito
        /// </summary>
        /// <param name="transaccion"></param>
        /// <param name="id_transaccion_caja"></param>
        /// <param name="Refinanciamiento"></param>
        /// <returns></returns>
        protected async Task Pago_Credito_Contabilidad(Transaccion_Credito transaccion, int id_transaccion_caja,bool Refinanciamiento = false)
        {
            //int id_agencia = Agencia_Usuario();
            Transaccion_Caja transaccion_caja = await _context
                .Transaccion_Caja
                .SingleAsync(n => n.Id_Transaccion_Caja == id_transaccion_caja);

            var transaccion_credito = _context.Transaccion_Credito.
                Include(t => t.Credito).
                Include(d => d.Credito.Linea_Credito).
                Single(s => s.Id_Transaccion_Credito == transaccion_caja.Id_Transaccion_Credito);

            int id_agencia = (from u in _context.Usuario_Agencia
                             join c in _context.Cartera on u.Id_Usuario equals c.Id_Usuario
                             where c.Id_Cartera==transaccion.Credito.Id_Cartera select u).Single().Id_Agencia;
            
            int id_linea=transaccion_credito.Credito.Id_Linea;

            Conf_Partida_Contable partida = await _context
                .Conf_Partida_Contable
                .Include(r => r.Conf_Detalles_Partidas_Contables)
                .SingleAsync(t => t.Identificador == "PAGO_CREDITO" && t.Id_Agencia == id_agencia && t.Id_Linea_Credito == id_linea);

            Caja caja = new Caja();
            if (Refinanciamiento)
            {
                caja = await obtener_Caja_Sistema();
            }
            else
            {
                caja = await Caja_Actual(transaccion_caja.Id_Caja);
            }

            if (partida != null)
            {
                //guardar el asiento
                Asiento_Contable asiento_contable = new Asiento_Contable
                {
                    Id_Transaccion_Caja = id_transaccion_caja,
                    Fecha = DateTime.Now,
                    Concepto = "Pago de cuota de crédito"
                };
                _context.Asiento_Contable.Add(asiento_contable).State = EntityState.Added;
                await _context.SaveChangesAsync();
                if (partida.Conf_Detalles_Partidas_Contables != null)
                    foreach (Conf_Detalle_Partida_Contable detalle in partida.Conf_Detalles_Partidas_Contables.ToList())
                    {
                        decimal monto_detalle = 0.00M;
                        if (detalle.Identificador == "CAPITAL")
                            monto_detalle = transaccion.Abono_capital;
                        if (detalle.Identificador == "INTERES")
                            monto_detalle = transaccion.Interes_Comision;

                        /*aquí se van a arreglar la comision e iva para que no falle*/
                        decimal IVACalculado = Math.Round((IVA / 100) * transaccion.Interes, 2);
                        decimal comisionsiniva = transaccion.Interes - IVACalculado;

                        if (detalle.Identificador == "COMISION")
                            monto_detalle = comisionsiniva;
                        if (detalle.Identificador == "IVA_COMISION")
                            monto_detalle = IVACalculado;
                        
                        if (monto_detalle>0)
                        {
                            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                    asiento_contable,
                                    monto_detalle,
                                    detalle.Cargo,
                                    detalle.Concepto,
                                    detalle.Cuenta_Contable,
                                    id_agencia)).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                        }
                    }

                /*manejar el recargo*/
                /*manejar el iva si no es refinanciamiento*/
                if (transaccion.Recargo > 0)
                {
                    decimal porcentaje_iva = (IVA / 100);
                    decimal recargo_sin_iva = transaccion.Recargo - transaccion.Recargo * porcentaje_iva;
                    decimal iva_del_recargo = transaccion.Recargo * porcentaje_iva;
                    var detalle_recargo = partida.Conf_Detalles_Partidas_Contables.Single(d => d.Identificador == "RECARGO");
                    if (detalle_recargo != null)
                    {
                        _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                        asiento_contable,
                                        recargo_sin_iva,
                                        detalle_recargo.Cargo,
                                        detalle_recargo.Concepto,
                                        detalle_recargo.Cuenta_Contable,
                                        id_agencia)).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                    }
                    var detalle_iva_recargo = partida.Conf_Detalles_Partidas_Contables.Single(d => d.Identificador == "IVA_RECARGO");
                    if (detalle_iva_recargo != null)
                    {
                        _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                        asiento_contable,
                                        iva_del_recargo,
                                        detalle_iva_recargo.Cargo,
                                        detalle_iva_recargo.Concepto,
                                        detalle_iva_recargo.Cuenta_Contable,
                                        id_agencia)).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                    }
                }

                _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                    asiento_contable,
                    transaccion.Valor_Cuota,
                    true,
                    partida.Descripcion,
                    caja.Cuenta_Contable,
                    id_agencia,
                    true)).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("No posee configuración contable para esa agencia");
        }


        /// <summary>
        /// guarda las partidas para el ahorro
        /// </summary>
        /// <returns></returns>
        protected async Task Manejo_Contable_Ahorro(bool retiro, bool interes, Transaccion_Ahorro transaccion, int id_transaccion_caja)
        {
            //int id_agencia = Agencia_Usuario();
            int id_cartera = _context
                .Ahorro
                .AsNoTracking()
                .Include(p => p.Persona)
                .Single(c => c.Id_Ahorro == transaccion.Id_Ahorro).Persona.Id_Cartera;

            int id_agencia = (from u in _context.Usuario_Agencia
                              join c in _context.Cartera on u.Id_Usuario equals c.Id_Usuario
                              where c.Id_Cartera == id_cartera
                              select u).Single().Id_Agencia;

            Ahorro ahorro = await _context.Ahorro.SingleAsync(t => t.Id_Ahorro == transaccion.Id_Ahorro);

            string identificador = "";
            string concepto = "";
            bool cargo = false;//solo para la caja
            if (retiro)
            {
                identificador = "RETIRO_AHORRO";
                concepto = "Retiro de cuenta de ahorro";
            }
            else
            {
                if (interes)
                {
                    identificador = "DEPOSITO_INTERES_AHORRO";
                    concepto = "Depósito de intereses a cuenta de ahorro";
                }
                else
                {
                    identificador = "DEPOSITO_AHORRO";
                    concepto = "Depósito de cuenta de ahorro";
                    cargo = true;
                }
            }

            Conf_Partida_Contable partida = await _context
                .Conf_Partida_Contable
                .Include(r => r.Conf_Detalles_Partidas_Contables)
                .SingleAsync(t => t.Identificador == identificador && t.Id_Agencia == id_agencia && t.Id_Linea_Ahorro == ahorro.Id_Linea_Ahorro);


            Asiento_Contable asiento_contable = new Asiento_Contable
            {
                Id_Transaccion_Caja = id_transaccion_caja,
                Fecha = DateTime.Now,
                Concepto = concepto
            };
            _context.Asiento_Contable.Add(asiento_contable).State = EntityState.Added;
            await _context.SaveChangesAsync();

            if (partida.Conf_Detalles_Partidas_Contables != null)
                foreach (Conf_Detalle_Partida_Contable detalle in partida.Conf_Detalles_Partidas_Contables.ToList())
                {
                    _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                        asiento_contable,
                        transaccion.Valor,
                        detalle.Cargo,
                        detalle.Concepto,
                        detalle.Cuenta_Contable,
                        id_agencia)).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                }
            Caja caja = await Caja_Actual();

            if (!interes)
            {
                _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                    asiento_contable,
                    transaccion.Valor,
                    cargo,
                    concepto,
                    caja.Cuenta_Contable,
                    id_agencia,
                    true)).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// manejo contable de las deducciones
        /// </summary>
        /// <param name="id_transaccion_simple"></param>
        /// <param name="id_linea_credito"></param>
        /// <param name="id_linea_ahorro"></param>
        /// <param name="id_factura_servicio"></param>
        /// <param name="id_otras_operaciones"></param>
        /// <param name="monto"></param>
        /// <param name="monto2"></param>
        /// <param name="otro"></param>
        /// <returns></returns>
        protected async Task Manejo_Contabilidad_Otros_Cobros(
            int id_transaccion_simple,
            Otro_Cobro otro,
            int id_linea_credito = 0,
            int id_linea_ahorro = 0,
            int id_factura_servicio =0,
            int id_otras_operaciones = 0,
            //variables para obtener valores de facturas si son de telefono
            decimal monto =0, 
            decimal monto2 =0)
        {
            Transaccion_Caja transaccion = await _context.
                Transaccion_Caja.
                AsNoTracking().
                Include(q => q.Caja).
                SingleAsync(f => f.Id_Transaccion_Simple == id_transaccion_simple);

            int id_caja = transaccion.Id_Caja;
            int id_agencia = transaccion.Caja.Id_Agencia;
            string Identificador = GetDisplayName(otro);

            if (Identificador == "COBRO_ESTRUCTURACION")
            {
                Caja cajausuario =await Caja_Actual();
                id_caja = cajausuario.Id_Caja;
                id_agencia = cajausuario.Id_Agencia;
            }

            string concepto = "";
            //validacion si la factura es telefono entra al if
            if (id_factura_servicio == 5)
            {
                List<Conf_Partida_Contable> partida_ = _context
                     .Conf_Partida_Contable
                     .Include(r => r.Conf_Detalles_Partidas_Contables)
                     .Where(
                         t => t.Identificador == Identificador &&
                              t.Id_Agencia == id_agencia &&
                              (
                                  (t.Id_Factura_Servicio == id_factura_servicio && id_factura_servicio > 0)
                              )
                     ).ToList();
                //se reciben las partidas y se dividen en las 2 partidas contables
                foreach (var str in partida_)
                {
                    concepto = str.Descripcion;
                    Asiento_Contable asiento_contable_ = new Asiento_Contable
                    {
                        Id_Transaccion_Caja = transaccion.Id_Transaccion_Caja,
                        Fecha = DateTime.Now,
                        Concepto = concepto
                    };
                    _context.Asiento_Contable.Add(asiento_contable_).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    //si el concepto de la configuracion es telefono 1 hace el asiento contable con esa cuenta
                    decimal monto_detalle = 0.00M;
                    if (concepto == "TELEFONO 1")
                    {
                       monto_detalle = monto;
                    }
                    //si el concepto de la configuracion es telefono 2 hace el asiento contable con esa cuenta
                    else if (concepto == "TELEFONO 2")
                    {
                        monto_detalle = monto;
                    }
                    if (str.Conf_Detalles_Partidas_Contables != null)
                        foreach (Conf_Detalle_Partida_Contable detalle in str.Conf_Detalles_Partidas_Contables.ToList())
                        {
                            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                asiento_contable_,
                                monto_detalle,
                                detalle.Cargo,
                                detalle.Concepto,
                                detalle.Cuenta_Contable,
                                id_agencia)).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                            Caja caja_ = await Caja_Actual();

                            _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                                asiento_contable_,
                                transaccion.Monto,
                                true,
                                concepto,
                                caja_.Cuenta_Contable,
                                id_agencia,
                                true)).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                        }
                }
            }
            //si la factura no es de tipo telefono se hace de manera normal
            else
            {
                Conf_Partida_Contable partida = await _context
                 .Conf_Partida_Contable
                 .Include(r => r.Conf_Detalles_Partidas_Contables)
                 .SingleAsync(
                     t => t.Identificador == Identificador &&
                     t.Id_Agencia == id_agencia &&
                     (
                         (t.Id_Linea_Credito == id_linea_credito && id_linea_credito > 0) ||
                         (t.Id_Linea_Ahorro == id_linea_ahorro && id_linea_ahorro > 0 ||
                          t.Id_Factura_Servicio == id_factura_servicio && id_factura_servicio > 0 ||
                          t.Id_Otras_Operaciones == id_otras_operaciones && id_otras_operaciones >0)
                     )
                 );
                concepto = partida.Descripcion;
                Asiento_Contable asiento_contable = new Asiento_Contable
                {
                    Id_Transaccion_Caja = transaccion.Id_Transaccion_Caja,
                    Fecha = DateTime.Now,
                    Concepto = concepto
                };

                _context.Asiento_Contable.Add(asiento_contable).State = EntityState.Added;
                await _context.SaveChangesAsync();

                /*manejar el iva si no es refinanciamiento*/
                decimal porcentaje_iva = (IVA / 100);
                decimal valor_sin_iva = transaccion.Monto - transaccion.Monto * porcentaje_iva;
                decimal iva = transaccion.Monto * porcentaje_iva;

                if (partida.Conf_Detalles_Partidas_Contables != null)
                    foreach (Conf_Detalle_Partida_Contable detalle in partida.Conf_Detalles_Partidas_Contables.ToList())
                    {
                        decimal monto_detalle = transaccion.Monto;
                        if (detalle.Identificador == "COBRO_ESTRUCTURACION" || detalle.Identificador == "PAGO_LIBRETA")
                            monto_detalle = valor_sin_iva;
                        if (detalle.Identificador == "IVA_ESTRUCTURACION" || detalle.Identificador == "IVA_LIBRETA")
                            monto_detalle = iva;

                        _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                            asiento_contable,
                            monto_detalle,
                            false,
                            detalle.Concepto,
                            detalle.Cuenta_Contable,
                            id_agencia)).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                    }

                Caja caja = await Caja_Actual();

                _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                    asiento_contable,
                    transaccion.Monto,
                    true,
                    concepto,
                    caja.Cuenta_Contable,
                    id_agencia,
                    true)).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
     
     
        }

        protected async Task Manejo_Contabilidad_Refinanciamientos(
            Credito credito,
            int id_transaccion_simple,
            int id_otras_operaciones,
            bool descuento = false)
        {
            Transaccion_Caja transaccion = await _context.
                Transaccion_Caja.
                AsNoTracking().
                Include(q => q.Caja).
                SingleAsync(f => f.Id_Transaccion_Simple == id_transaccion_simple);

            //int id_agencia = transaccion.Caja.Id_Agencia;

            int id_agencia = (from u in _context.Usuario_Agencia
                              join c in _context.Cartera on u.Id_Usuario equals c.Id_Usuario
                              where c.Id_Cartera == credito.Id_Cartera
                              select u).Single().Id_Agencia;

            string Identificador = "DESCUENTO_REFINANCIAMIENTO";

            string concepto = "";
            Conf_Partida_Contable partida = await _context
               .Conf_Partida_Contable
               .Include(r => r.Conf_Detalles_Partidas_Contables)
               .SingleAsync(
                   t => t.Identificador == Identificador &&
                   t.Id_Agencia == id_agencia &&
                   (
                       (t.Id_Otras_Operaciones == id_otras_operaciones && id_otras_operaciones > 0)
                   )
               );
            concepto = partida.Descripcion;
            Asiento_Contable asiento_contable = new Asiento_Contable
            {
                Id_Transaccion_Caja = transaccion.Id_Transaccion_Caja,
                Fecha = DateTime.Now,
                Concepto = concepto
            };

            _context.Asiento_Contable.Add(asiento_contable).State = EntityState.Added;
            await _context.SaveChangesAsync();
           Conf_Detalle_Partida_Contable obtener_cuenta_cargo = 
               _context.Conf_Detalle_Partida_Contable.Single(a=>a.Identificador == Identificador && a.Cargo );
           Conf_Detalle_Partida_Contable obtener_cuenta_abono =
               _context.Conf_Detalle_Partida_Contable.Single(a => a.Identificador == Identificador && a.Cargo);
           Caja caja = await Caja_Actual();
            if (descuento)
           {
               _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                   asiento_contable,
                   transaccion.Monto,
                   false,
                   concepto,
                   obtener_cuenta_abono.Cuenta_Contable,
                   id_agencia,
                   true)).State = EntityState.Added;

               _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                   asiento_contable,
                   transaccion.Monto,
                   true,
                   concepto,
                   obtener_cuenta_cargo.Cuenta_Contable,
                   id_agencia,
                   true)).State = EntityState.Added;
            }
           else
            {
                int id_linea = credito.Id_Linea;
                Conf_Partida_Contable intereses = await _context
                    .Conf_Partida_Contable
                    .Include(r => r.Conf_Detalles_Partidas_Contables)
                    .SingleAsync(t => t.Identificador == "PAGO_CREDITO" && t.Id_Agencia == id_agencia && t.Id_Linea_Credito == id_linea);
   
                _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                   asiento_contable,
                   transaccion.Monto,
                   false,
                   concepto,
                   intereses.Conf_Detalles_Partidas_Contables.Single(a=>a.Identificador == "INTERES").Cuenta_Contable,
                   id_agencia,
                   true)).State = EntityState.Added;

               _context.Detalle_Asiento_Contable.Add(Armar_Detalle_Asiento(
                   asiento_contable,
                   transaccion.Monto, 
                   true,
                   concepto,
                   caja.Cuenta_Contable,
                   id_agencia,
                   true)).State = EntityState.Added;
            }

          
            await _context.SaveChangesAsync();
        }

        protected string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        private Detalle_Asiento_Contable Armar_Detalle_Asiento(
            Asiento_Contable asiento_contable,
            decimal valor,
            bool cargo,
            string concepto,
            string cuenta_contable,
            int id_agencia,
            bool esCaja = false)
        {
            return new Detalle_Asiento_Contable
            {
                Id_Asiento_Contable = asiento_contable.Id_Asiento_Contable,
                Abono = !cargo ? valor : 0.00M,
                Cargo = cargo ? valor : 0.00M,
                Concepto = concepto,
                Cuenta_Contable = cuenta_contable,
                EsCaja = esCaja,
                Id_Agencia=id_agencia
            };
        }


        /// <summary>
        /// se efectúa una reversión
        /// </summary>
        /// <param name="id_transaccion"></param>
        /// <param name="es_credito"></param>
        /// <returns></returns>
        protected async Task Reversion(int id_transaccion, bool es_credito = true, bool es_deposito = false)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                if (es_credito)
                {
                    Transaccion_Credito tc = _context.Transaccion_Credito.SingleOrDefault(y => y.Id_Transaccion_Credito == id_transaccion);
                    tc.Borrado_Logico = true;
                    tc.Eliminado_En = fecha;
                    tc.Eliminado_Por = id_usuario;
                    _context.Entry(tc).State = EntityState.Modified;
                    await Bitacora("Transacción Crédito", "Reversión Pago Cuota", tc.Id_Transaccion_Credito, true,monto:tc.Valor_Cuota);
                }
                if(!es_credito && !es_deposito)
                {
                    Transaccion_Ahorro ta = _context.Transaccion_Ahorro.SingleOrDefault(y => y.Id_Transaccion_Ahorro == id_transaccion);
                    ta.Borrado_Logico = true;
                    ta.Eliminado_En = fecha;
                    ta.Eliminado_Por = id_usuario;
                    _context.Entry(ta).State = EntityState.Modified;
                    if (ta.Id_Tipo_Operacion_Ahorro== 1) {
                        await Bitacora("Transacción Ahorro", "Reversión Depósito", ta.Id_Transaccion_Ahorro, false,monto:ta.Valor);
                    }
                    if(ta.Id_Tipo_Operacion_Ahorro == 3){
                        await Bitacora("Transacción Ahorro", "Reversión Retiro", ta.Id_Transaccion_Ahorro, false,monto:ta.Valor);
                    }
                    
                }
                if (es_deposito)
                {
                    Transaccion_Deposito_Plazo ta = _context.Transaccion_Deposito_Plazo.SingleOrDefault(y => y.Id_Transaccion_Deposito_Plazo == id_transaccion);
                    ta.Borrado_Logico = true;
                    ta.Eliminado_En = fecha;
                    ta.Eliminado_Por = id_usuario;
                    _context.Entry(ta).State = EntityState.Modified;
                    if (ta.Id_Tipo_Operacion_Ahorro == 1)
                    {
                        await Bitacora("Transacción Depósito", "Reversión Depósito Plazo", ta.Id_Transaccion_Deposito_Plazo, false,monto:ta.Valor);
                    }
                    if (ta.Id_Tipo_Operacion_Ahorro == 3)
                    {
                        await Bitacora("Transacción Depósito", "Reversión Retiro Depósito Plazo", ta.Id_Transaccion_Deposito_Plazo, false,monto:ta.Valor);
                    }
                }
                /*cuando esta en transaccion de caja hay que eliminado*/
                Transaccion_Caja transaccion_caja = new Transaccion_Caja();
                if (es_credito)
                {
                    transaccion_caja = _context.Transaccion_Caja.SingleOrDefault(t => t.Id_Transaccion_Credito == id_transaccion);
                }
                if (!es_credito) {

                    transaccion_caja = _context.Transaccion_Caja.SingleOrDefault(t => t.Id_Transaccion_Ahorro == id_transaccion);
                }
                if (es_deposito) {

                    transaccion_caja = _context.Transaccion_Caja.SingleOrDefault(t => t.Id_Transaccion_Deposito_Plazo == id_transaccion);
                }

                if (transaccion_caja != null)
                {
                    transaccion_caja.Borrado_Logico = true;
                    transaccion_caja.Eliminado_En = fecha;
                    transaccion_caja.Eliminado_Por = id_usuario;
                }
                await _context.SaveChangesAsync();
                if (transaccion_caja != null)
                    await Contabilidad_Reversion_Transaccion(transaccion_caja.Id_Transaccion_Caja);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reversión solo de manejo de bóvedas
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected async Task Reversion_Simple(List<int> ids)
        {
            DateTime fecha = DateTime.Now;
            string id_usuario = _userManager.GetUserId(HttpContext.User);
            try
            {
                foreach (int id_transaccion in ids)
                {
                    Transaccion_Simple ts = _context.Transaccion_Simple.SingleOrDefault(y => y.Id_Transaccion_Simple == id_transaccion);
                    ts.Borrado_Logico = true;
                    ts.Eliminado_En = fecha;
                    ts.Eliminado_Por = id_usuario;
                    _context.Entry(ts).State = EntityState.Modified;
                    Transaccion_Caja transaccion_caja = _context.Transaccion_Caja.SingleOrDefault(t => t.Id_Transaccion_Simple == id_transaccion);
                    /*registro contable*/
                    if (transaccion_caja != null)
                    {
                        await Contabilidad_Reversion_Transaccion(transaccion_caja.Id_Transaccion_Caja);/*nuevo hay que probarlo*/
                        /*cuando esta en transaccion de caja hay que eliminado*/
                        transaccion_caja.Borrado_Logico = true;
                        transaccion_caja.Eliminado_En = fecha;
                        transaccion_caja.Eliminado_Por = id_usuario;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Registro Contable 
        /// </summary>
        /// <param name="id_transaccion">la transacción de la caja, no puede ser otra</param>
        /// <returns></returns>
        protected async Task Contabilidad_Reversion_Transaccion(int id_transaccion)
        {
            Func<Transaccion_Caja, bool> filtro_transaccion = new Func<Transaccion_Caja, bool>(
                    c => c.Id_Transaccion_Caja == id_transaccion);
            if (_context.Transaccion_Caja.Any(filtro_transaccion))
            {
                Transaccion_Caja transaccion = _context.Transaccion_Caja.Single(filtro_transaccion);
                Func<Asiento_Contable,bool> tiene_asiento = new Func<Asiento_Contable,bool>(
                    d => d.Id_Transaccion_Caja == transaccion.Id_Transaccion_Caja);
                if (_context.Asiento_Contable.Any(tiene_asiento))
                {
                    var asiento = _context
                        .Asiento_Contable
                        .Include(u => u.Detalles_Asientos_Contables)
                        .Single(tiene_asiento);
                    AutoMapper.MapperConfiguration config = new AutoMapper
                        .MapperConfiguration(cfg => cfg.CreateMap<Asiento_Contable, Asiento_Contable>());
                    AutoMapper.IMapper mapper = config.CreateMapper();
                    var asiento_revertido = mapper.Map<Asiento_Contable>(asiento);
                    asiento_revertido.Id_Asiento_Contable = 0;
                    asiento_revertido.Concepto = "Reversión de Liquidación de crédito";
                    _context.Add(asiento_revertido).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    if (asiento_revertido.Detalles_Asientos_Contables.Count > 0)
                    {
                        foreach (var detalle in asiento_revertido.Detalles_Asientos_Contables.ToList())
                        {
                            AutoMapper.MapperConfiguration configd = new AutoMapper
                                .MapperConfiguration(cfg => cfg.CreateMap<Detalle_Asiento_Contable, Detalle_Asiento_Contable>());
                            AutoMapper.IMapper mapperd = configd.CreateMapper();
                            var detalle_revertido = mapperd.Map<Detalle_Asiento_Contable>(detalle);
                            detalle_revertido.Id_Detalle_Asiento_Contable = 0;
                            if (detalle_revertido.Abono > 0)
                            {
                                detalle_revertido.Cargo = detalle_revertido.Abono;
                                detalle_revertido.Abono = 0.00M;
                            }
                            else {

                                detalle_revertido.Abono = detalle_revertido.Cargo;
                                detalle_revertido.Cargo = 0.00M;
                            }
                            detalle_revertido.Concepto = "Reversión de liquidación de crédito";
                            _context.Add(detalle_revertido).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        protected async Task<Caja> Boveda_Actual(int id_caja=0)
        {
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            Usuario_Agencia relacion = await _context.Usuario_Agencia.SingleOrDefaultAsync(c => c.Id_Usuario == Id_Usuario);

            if (id_caja == 0)
                return await _context.Caja.SingleOrDefaultAsync(k => k.Id_Usuario == Id_Usuario && k.Id_Agencia == relacion.Id_Agencia && k.Boveda && k.Activa);
            else
                return await _context.Caja.SingleOrDefaultAsync(d=>d.Id_Caja==id_caja && d.Boveda);
        }

        protected async Task<Caja> Caja_Actual(int id_caja = 0)
        {
            string Id_Usuario = _userManager.GetUserId(HttpContext.User);
            Usuario_Agencia relacion = await _context.Usuario_Agencia.SingleOrDefaultAsync(c => c.Id_Usuario == Id_Usuario);
            Caja caja = null;
            if (id_caja > 0)
                caja = await _context.Caja.SingleOrDefaultAsync(k => k.Id_Agencia == relacion.Id_Agencia && k.Id_Caja == id_caja && !k.Boveda && k.Activa);
            else
                caja = await _context.Caja.SingleOrDefaultAsync(k => k.Id_Usuario == Id_Usuario && k.Id_Agencia == relacion.Id_Agencia && !k.Boveda && k.Activa);
            if (caja == null)
                throw new Exception("El usuario no posee una caja");
            return caja;
        }

        protected async Task<DateTime> Fecha_Pago(int periodo_dias, int id_credito)
        {
            int dias = new CalculosYProcesoCredito().CalculoDias(periodo_dias);
            DateTime ultima_fecha = _context
                .Transaccion_Credito
                .Where(y => y.Id_Credito == id_credito && !y.Borrado_Logico)
                .AsEnumerable()
                .Last().FechaPago;

            return ultima_fecha.Date.AddDays(dias);
        }


        /*relacionado a la amortizacion*/
        /// <summary>
        /// para manejar la amortización a la fecha
        /// </summary>
        /// <param name="id_credito"></param>
        /// <param name="fecha"></param>
        /// <param name="valor_cuota"></param>
        /// <param name="para_refinanciar"></param>
        /// <param name="recargo"></param>
        /// <returns></returns>
        protected Transaccion_Credito AmortizacionActualProc(
            int id_credito, 
            DateTime fecha, 
            decimal? valor_cuota = null, 
            bool para_refinanciar = false, 
            bool recargo = false,
            bool para_reporte = false)
        {
            //DateTime fecha_inicio = DateTime.Now;
            AmortizacionVolatil a_retorna = null;
            Credito credito = _context.Credito
                .AsNoTracking()
                .Include(d => d.Linea_Credito_Historial)
                .Include(p => p.Periodo)
                .Include(s=>s.Linea_Credito)
                .Include(l => l.Linea_Credito_Historial.Cobros_Periodicos_Historial).SingleOrDefault(c => c.Id_Credito == id_credito);
            Periodo frecuencia = credito.Periodo;

            /*cambiar la fecha si hay congelación*/
            DateTime fecha_congelada = fecha;
            if (credito.Congelado)
                fecha_congelada = credito.Congelado_En;

            Linea_Credito_Historial historial = Historial(credito.Id_Credito);
            /*veremos si hay que cambiar el % a dinero*/
            CalculosYProcesoCredito cpc = new CalculosYProcesoCredito();
            decimal castigo_cancelacion_anticipada = para_refinanciar ? 0.00M : historial.Cancelacion_anticipada;
            decimal total = 0.00M;
            bool lleva_cobros = false;
            if (historial.PlanAyC_Cobros_Periodico_Dinero)
            {
                total = credito.Linea_Credito_Historial.Cobros_Periodicos_Historial.Count > 0 ? credito.Linea_Credito_Historial.Cobros_Periodicos_Historial.Sum(t => t.Valor) : 0.00M;
                lleva_cobros = true;
            }
            DateTime fecha_movible = Verificar_Credito_Fecha_Movible(credito);
            TimeSpan diferencia_dias = fecha_movible - credito.Liquidado_En;
            bool generar_amortizacion = !_context
                .Amortizacion
                .AsNoTracking()
                .Any(d => d.Id_Credito == credito.Id_Credito);
            cpc = new CalculosYProcesoCredito(
                historial.Interes_Comision,
                credito.Monto_pago,
                frecuencia,
                IVA,//no se había actualizado el iva
                credito.Plazo,
                historial.Interes,
                fecha_movible,//se mueve la fecha de liquidación si responde a la configuración
                total,
                lleva_cobros,
                pago_anticipado: castigo_cancelacion_anticipada,
                dias_movible:diferencia_dias.Days,
                generar_amortizacion: generar_amortizacion);
            
                a_retorna = CalcularPosicionAmortizacion(cpc, credito, fecha_congelada, valor_cuota, historial, para_refinanciar, recargo,para_reporte:para_reporte);

                return new Transaccion_Credito
                {
                    No = a_retorna.No,
                    Id_Transaccion_Credito = 0,
                    Id_Tipo_Operecion_Credito = 1,
                    Id_Credito = id_credito,
                    FechaPago = fecha,
                    Fecha_Amortizacion = a_retorna.Fecha_Amortizacion,
                    Interes_Comision = a_retorna.Interes,
                    Interes = a_retorna.Comision,
                    /*IvaSobreComision = a_retorna.IvaSobreComision,*/
                    Otros_Cobros = a_retorna.Otros_Cobros,
                    Abono_capital = a_retorna.Abono_capital,
                    Valor_Cuota = a_retorna.Valor_Cuota,
                    Saldo_Capital = a_retorna.Saldo_Capital,
                    Dias = a_retorna.DiasTranscurridos,
                    Anticipado = a_retorna.Anticipado,
                    Puntual = a_retorna.Puntual,
                    Mora = a_retorna.Mora,
                    Vencido = a_retorna.Vencido,
                    Pagos_Acumulado = a_retorna.Pagos_Acumulado,
                    Recargo = a_retorna.Recargo,
                    /*para detalle y reportes*/
                    Pago_para_cancelar = a_retorna.Pago_para_cancelar,
                    Capital_Mora = a_retorna.Capital_Mora,
                    Interes_Diario = a_retorna.Interes_Diario,
                    Interes_Pendiente = a_retorna.Interes_Pendiente,
                    Comsion_Pendiente = a_retorna.Comsion_Pendiente,
                    Valor_Al_Dia = a_retorna.Valor_Al_Dia,
                    Recargos_Acumulados=a_retorna.Recargos_Acumulados
                };
        }

        /// <summary>
        /// Regreso de Nio 
        /// </summary>
        /// <param name="cpc"></param>
        /// <param name="credito"></param>
        /// <param name="fecha"></param>
        /// <param name="valor_cuota"></param>
        /// <param name="historial"></param>
        /// <param name="para_refinanciar"></param>
        /// <param name="condonar_recargo"></param>
        protected AmortizacionVolatil CalcularPosicionAmortizacion(
            CalculosYProcesoCredito cpc, 
            Credito credito,
            DateTime fecha,
            decimal? valor_cuota,
            Linea_Credito_Historial historial,
            bool para_refinanciar,
            bool condonar_recargo=false,
            bool para_reporte = false)
        {
            bool puntual = false;bool mora = false;
            bool anticipado = false; bool vencido = false;
            List<Amortizacion> amortizacion_vencidas = new List<Amortizacion>();

            DateTime fecha_para_Vencido = fecha.AddDays(-1);/*fecha para amortización*/
            Amortizacion ultima_amortizacion_proVencer = null;

            Amortizacion ultima_amortizacion_credito = _context
                .Amortizacion
                .AsNoTracking()
                .Where(d=>d.Id_Credito == credito.Id_Credito).OrderBy(d=>d.No).Last();

            bool pagar_ahora = false;
            if (_context.Amortizacion.AsNoTracking().Any(d => d.FechaPago.Date <= fecha_para_Vencido.Date && d.Id_Credito == credito.Id_Credito))
            {
                amortizacion_vencidas = _context
                    .Amortizacion
                    .AsNoTracking()
                    .Where(d => d.FechaPago.Date <= fecha_para_Vencido.Date && d.Id_Credito == credito.Id_Credito)
                    .OrderBy(d => d.No).ToList();
            }
            if (_context.Amortizacion.AsNoTracking().Any(d => d.FechaPago.Date == fecha.Date && d.Id_Credito == credito.Id_Credito))
            {
                ultima_amortizacion_proVencer = _context
               .Amortizacion
               .AsNoTracking()
               .Single(d => d.FechaPago.Date == fecha.Date && d.Id_Credito == credito.Id_Credito);
                pagar_ahora = true;
            }
            Amortizacion ultima_amortizacion_vencida = null;/*si no existe transacción*/
            Amortizacion ultima_amortizacion_cubierta = null;
            int dias = 0;
            //List<Amortizacion> amortizacion_credito = _context
            //   .Amortizacion
            //   .AsNoTracking()
            //   .Where(d => d.Id_Credito == credito.Id_Credito).OrderBy(d => d.No).ToList();

            //DateTime ultima_fecha_amortizada = DateTime.MinValue;
            //if (credito.Id_Linea == 5)
            //{
            //    ultima_fecha_amortizada = ultima_amortizacion_credito.FechaPago;
            //}
            //else
            //{
            //    ultima_fecha_amortizada = Verificar_Credito_Fecha_Movible(credito);
            //}
            DateTime ultima_fecha_amortizada =Verificar_Credito_Fecha_Movible(credito);
            /*sacar los días que se mueve si lo hace*/
            TimeSpan diferenciasMobible = ultima_fecha_amortizada.Date - credito.Liquidado_En.Date;

            bool libre = false;
            bool completado = false;
            int pagos_acumulados = 0;
            decimal saldo_capital = credito.Monto_pago;

            bool tiene_transacciones = _context.Transaccion_Credito.AsNoTracking().Any(f => f.Id_Credito == credito.Id_Credito && !f.Borrado_Logico);

            decimal intereses_pagados = 0.00M;
            decimal comision_pagada = 0.00M;
            decimal ultimo_saldo = 0.00M;
            decimal pagos_realizados = 0.00M;
            decimal recargos_pagados = 0.00M;

            decimal intereses_vencidos = amortizacion_vencidas.Count() > 0 ? amortizacion_vencidas.Sum(v => v.Interes):0.00M;
            decimal comision_vencido = amortizacion_vencidas.Count() > 0 ? amortizacion_vencidas.Sum(t => t.Comision) /*+ amortizacion_vencidas.Sum(f => f.IvaSobreComision)*/:0.00M;
            /*si le toca pagar hoy que se le sume la comisión plan A&C*/
            if(ultima_amortizacion_proVencer!=null)
                comision_vencido += ultima_amortizacion_proVencer.Comision /*+ ultima_amortizacion_proVencer.IvaSobreComision*/;

            decimal ultimo_saldo_vencido = 0.00M;
            ultimo_saldo_vencido = amortizacion_vencidas.Count() > 0 ? amortizacion_vencidas.Min(d=>d.Saldo_Capital) : 0.00M;

            completado = ultimo_saldo_vencido == 0.00M;

            Transaccion_Credito ultima_transaccion_registrada = null;

            if (tiene_transacciones)
            {
                var tcs = _context.Transaccion_Credito
                .Where(f => f.Id_Credito == credito.Id_Credito && !f.Borrado_Logico).OrderBy(d=>d.FechaPago).ToList();
                /*2 amortizacion vencida completada por el pago*/
                intereses_pagados = tcs.Sum(c=>c.Interes_Comision);
                comision_pagada = tcs.Sum(c=>c.Interes) +   tcs.Sum(d => d.IvaSobreComision);
                ultimo_saldo = tcs.Min(u => u.Saldo_Capital);

                /*ver el saldo al día para saber si está en mora*/

                /*ver si debe o no comision*/
                libre = comision_pagada >= comision_vencido;

                //saber si hay una o más cuotas completas sin cancelar
                completado = (ultimo_saldo <= ultimo_saldo_vencido) /*intereses_pagados >= intereses_vencidos*/;

                if (!completado && amortizacion_vencidas.Count() > 0)//encontrar las cuotas vencidas sin pagar
                {
                    var amortizaciones_desc = amortizacion_vencidas.OrderByDescending(y => y.No);
                    var limite_amortizaciones = amortizaciones_desc.Where(c => c.Interes <= intereses_pagados);
                    foreach (var am in amortizaciones_desc)
                    {
                        decimal suma_intereses = amortizaciones_desc.Where(n => n.No <= am.No).Sum(i => i.Interes);
                        decimal suma_comision = amortizaciones_desc.Where(n => n.No <= am.No).Sum(i => i.Comision);
                        if (ultimo_saldo <= am.Saldo_Capital)
                        {
                            ultima_amortizacion_cubierta = amortizaciones_desc.Single(c => c.No == am.No);
                           // intereses_vencidos = suma_intereses;
                            break;
                        }

                        //if (suma_intereses <= intereses_pagados)
                        //{
                        //    ultima_amortizacion_cubierta = amortizaciones_desc.Single(c => c.No == am.No);
                        //    intereses_vencidos = suma_intereses;
                        //    break;
                        //}
                    }
                }
                else
                { 
                    if(amortizacion_vencidas.Count > 0)
                        ultima_amortizacion_cubierta = amortizacion_vencidas.Last();
                }

                ultima_transaccion_registrada = tcs.Last();
                ultima_fecha_amortizada = ultima_transaccion_registrada.FechaPago.Date;
                saldo_capital = ultima_transaccion_registrada.Saldo_Capital;
                pagos_realizados = tcs.Sum(d=>d.Valor_Cuota);
                recargos_pagados = tcs.Sum(c=>c.Recargo);
            }
            
            /*SOLO PARA OBTENER VENCIDOS PARA REPORTE DE MORA*/
            bool es_vencido = false;
            if (tiene_transacciones && ultima_amortizacion_vencida != null)
                es_vencido = ultima_amortizacion_vencida.FechaPago.Date > ultima_amortizacion_cubierta.FechaPago.Date ||
                    ultima_amortizacion_vencida.FechaPago.Date>fecha.Date;

            /*encontrar los pagos acumulados a partir del que sí se complementó*/
            Amortizacion ultima_amortizacion_grabada = amortizacion_vencidas.Count() > 0 ? amortizacion_vencidas.Last():null;

            int diferencia_amortizada = 0; 
                
            if(ultima_amortizacion_grabada!=null)
            {
                if(ultima_amortizacion_cubierta!=null)
                    diferencia_amortizada = ultima_amortizacion_grabada.No - ultima_amortizacion_cubierta.No;
                else
                    diferencia_amortizada = ultima_amortizacion_grabada.No - 0;
            }
            
            pagos_acumulados = diferencia_amortizada > 0 ? diferencia_amortizada : 0;
            decimal castigo_cancelacion_anticipada = para_refinanciar ? 0.00M : historial.Cancelacion_anticipada;
            //calcular los recargos
            decimal recargo = condonar_recargo? 0.00M : historial.Recargo * pagos_acumulados;/*no funcionaba el condonar el recargo*/

            TimeSpan diferencia = fecha.Date - ultima_fecha_amortizada.Date;

            dias = diferencia.Days + diferenciasMobible.Days;
            dias = ExcluirFinSemana(credito.Id_Linea, ultima_fecha_amortizada.Date, fecha.Date,dias);

            int dias_mora = dias;
            if (!tiene_transacciones)
            {
                DateTime UltimaFecha=credito.Liquidado_En.Date;
                if (amortizacion_vencidas.Count > 0)
                    UltimaFecha = amortizacion_vencidas.First().FechaPago;

                TimeSpan diferencia_mora = fecha.Date - UltimaFecha.Date;
                if (pagos_acumulados > 0)
                    dias_mora = diferencia_mora.Days;
                else
                    dias_mora = 0;

            }

            /*hacer las divisiones por semana*/
            int veces = (int)Math.Floor(Convert.ToDecimal(dias_mora) / 7.00M);

            /*evaluar al estilo bancopres */
            //if (dias_mora > 7 && dias_mora <= 32)
            //    recargo = veces * historial.Recargo;
            if (dias_mora > 37)
                recargo = 16;
            /*quitar los recargos ya condonados*/
            decimal recargos_condonados = _context
                .Amortizacion
                .Where(d => d.Id_Credito == credito.Id_Credito && d.Recargo > 0)
                .Sum(c=>c.Recargo);
            recargo -= recargos_condonados;
            recargo = Math.Abs(recargo);
            /*heredado de bancopre no culpar a Alfredo*/


            /*saber si es puntual*/
            puntual = libre && completado && pagos_acumulados==0;
            mora = !puntual;
            /*saber si es vencido*/
            Periodo frecuencia = credito.Periodo;
            if (credito.Id_Periodo == 1 && credito.Id_Linea == 17)
                frecuencia = new Periodo { Id_Periodo = 0, Valor = 20, dias = 1, Descripcion = "Ágil-Efectivo" };

            /*si el primer pago no se ha vencido ese voy a consultar al menos para llenar los datos*/
            Amortizacion amortizacion_defecto = ultima_amortizacion_cubierta;
            if (ultima_amortizacion_cubierta != null)
                amortizacion_defecto = ultima_amortizacion_cubierta;
            else
                amortizacion_defecto = _context.Amortizacion.AsNoTracking().Single(d => d.Id_Credito == credito.Id_Credito && d.No == 1);
            //valor de cuota para reportes y otras consultas
            decimal v_cuota = 0.00M;
            if (valor_cuota > 0 && !para_reporte)
            {
                v_cuota = Convert.ToDecimal(valor_cuota);
            }
            else if (para_reporte) {
                v_cuota = 0.00M;
            }
            else{
                v_cuota = amortizacion_defecto.Valor_Cuota;
            }
            Amortizacion fila = cpc.CalculoAmortizacionFila(
                    amortizacion_defecto.No==1? 0 : amortizacion_defecto.No,
                    saldo_capital,
                    v_cuota,
                    Math.Abs(dias),
                    historial.Interes == 0 ? historial.Interes_Comision : historial.Interes, 
                    fecha,
                    libre ? 0.00M : amortizacion_defecto.Comision,
                    /*libre ? 0.00M : amortizacion_defecto.IvaSobreComision,*/
                    frecuencia.Id_Periodo,
                    recargo,/*problemas del recargo*/
                    true,
                    true,
                    ultimo_pago: para_refinanciar,
                    pago_anticipado: castigo_cancelacion_anticipada,
                    fecha_amortizacion: amortizacion_defecto.FechaPago,
                    fecha_anterior: amortizacion_defecto.FechaPago);
            //se hace porque si el valor del penultimo saldo a capital es mas bajo que el ultimo abono tiene que igualarce 
            decimal diferencia_abonos = 0.00M;
            if (fila.Saldo_Capital == 0.00M) {
                decimal abono_anterior = ultima_amortizacion_grabada!=null? ultima_amortizacion_grabada.Abono_capital:0.00M;
                decimal cuota_actual = fila.Valor_Cuota;
                diferencia_abonos = cuota_actual-abono_anterior;
            }
            //decimal capital_mora = ultima_amortizacion_cubierta.Saldo_Capital - ultima_amortizacion_vencida.Saldo_Capital;
            decimal capital_mora = 0.00M;
            if (ultima_amortizacion_grabada != null)
            {
                decimal capital_mora_operando = saldo_capital - ultima_amortizacion_grabada.Saldo_Capital;
                capital_mora = capital_mora_operando < 0 ? 0.00M : capital_mora_operando;
            }

            decimal comision_pendiente_operando = comision_vencido - comision_pagada;
            decimal comision_pendiente = comision_pendiente_operando < 0 ? 0.00M : comision_pendiente_operando;

            decimal interes_pendiente = 0.00M;
            if (tiene_transacciones && capital_mora > 0)
            {
                interes_pendiente = intereses_vencidos - intereses_pagados;
                if (interes_pendiente < 0)
                    interes_pendiente = 0.00M;
            }
            decimal interes_diario_operando = 0.00M;

            /*manejar los interes para crédito vencido falta la fecha de vencimiento en la migración*/
            bool estavencido = fecha.Date > ultima_amortizacion_credito.FechaPago.Date;
            if (estavencido)
            {
                int soloDiasCorriente = dias - dias_mora;

                decimal interes_diario_dias_normales= InteresDiario(
                    credito, 
                    ultima_transaccion_registrada, soloDiasCorriente);
                decimal interes_diario_dias_mora = InteresDiario(
                    credito,
                    ultima_transaccion_registrada,dias_mora,true);

                interes_diario_operando = interes_diario_dias_normales + interes_diario_dias_mora;
            }else
                interes_diario_operando = InteresDiario(credito, ultima_transaccion_registrada, fecha);

            decimal interes_diario = interes_diario_operando < 0? 0.00M: interes_diario_operando;
            decimal restanteRecargo = 0.00M;
            if (recargo > 16)
            {
              
                decimal excedente = recargo - 16;
                recargo = 16;

                fila.Interes += excedente;
            }
            decimal interes_total = interes_pendiente + comision_pendiente + interes_diario;

            decimal aldia = 0.00M;
            
            if (pagar_ahora && interes_total > 0)
                aldia += ultima_amortizacion_proVencer.Abono_capital + interes_total + recargo;
            
            if (capital_mora > 0)    
                aldia+=capital_mora + interes_pendiente + recargo;
            if (estavencido && credito.Id_Linea != 2)/*penalización por vencido del 25%*/
                aldia +=  recargo + interes_total;
            if (credito.Id_Linea == 2 && estavencido)/*es estacionario y ya está vencido*/
                aldia += interes_total + recargo;

            /*considerar para cancelar todo*/
            decimal pago_para_cancelar = 0.00M;

            /*restar lo que ya pagó*/
            pago_para_cancelar = _context.Amortizacion
                .AsNoTracking()
                .First(d => d.Id_Credito == credito.Id_Credito).Valor_Cuota * credito.Numero_cuotas;

            pago_para_cancelar -= pagos_realizados;
            pago_para_cancelar -= recargos_pagados;
            if(interes_pendiente>0)
                pago_para_cancelar += interes_diario;
            pago_para_cancelar += recargo;

            return new AmortizacionVolatil
            {
                No = fila.No,
                FechaPago = fila.FechaPago,
                Fecha_Amortizacion = ultima_amortizacion_cubierta!=null? ultima_amortizacion_cubierta.FechaPago:credito.Liquidado_En,
                Interes = fila.Interes /*+ diferencia_abonos*/,
                Comision = fila.Comision,
                /*IvaSobreComision = fila.IvaSobreComision,*/
                Otros_Cobros = fila.Otros_Cobros,
                Abono_capital = fila.Abono_capital /*- diferencia_abonos*/,
                Valor_Cuota = fila.Valor_Cuota,
                Saldo_Capital = fila.Saldo_Capital,
                Puntual = puntual,
                Mora = mora,
                Vencido = es_vencido,
                Anticipado = anticipado,
                DiasTranscurridos = Math.Abs(dias),
                Pagos_Acumulado = pagos_acumulados,
                Recargo = recargo,
                /*para detalle y reportes*/
                Pago_para_cancelar = pago_para_cancelar,
                Capital_Mora = capital_mora,
                Interes_Diario = interes_diario,
                Interes_Pendiente = interes_pendiente,
                Comsion_Pendiente = comision_pendiente,
                Valor_Al_Dia =Math.Round(aldia,2),
                /*para los recargos*/
                Recargos_Acumulados = recargos_pagados
            };
        }

        private decimal InteresDiario(Credito cr,Transaccion_Credito last,DateTime fecha)
        {
            decimal interes = cr.Linea_Credito.Interes > 0 ? cr.Linea_Credito.Interes : cr.Linea_Credito.Interes_Comision;
            decimal tasa_convertida = interes / 365;
            decimal interes_diario;
            if (last != null)
            {
                TimeSpan dias = fecha.Date - last.FechaPago.Date;
                interes_diario = Math.Round((last.Saldo_Capital * (dias.Days * tasa_convertida)) / 100, 2);
            }
            else
            {
                TimeSpan dias = fecha.Date - cr.Liquidado_En.Date;
                interes_diario = Math.Round((cr.Monto_pago * (dias.Days * tasa_convertida)) / 100, 2);
            }
            return interes_diario;
        }

        /*para resolver vencidos/morosos*/
        private decimal InteresDiario(Credito cr, Transaccion_Credito last,int dias,bool mora=false)
        {
            decimal interes = cr.Linea_Credito.Interes > 0 ? cr.Linea_Credito.Interes : cr.Linea_Credito.Interes_Comision;
            if (mora)
                interes = 60.00M;
                
            decimal tasa_convertida = interes / 365;
            decimal interes_diario;
            if (last != null)
                interes_diario = Math.Round((last.Saldo_Capital * (dias * tasa_convertida)) / 100, 2);
            else
                interes_diario = Math.Round((cr.Monto_pago * (dias * tasa_convertida)) / 100, 2);
            return interes_diario;
        }

        private int ExcluirFinSemana(int id_linea_credito,DateTime inicio, DateTime final,int dias)
        {
            var configuracion = _context.Configuracion_General.Single(d=>d.Nombre== "EXCLUIR_FIN_SEMANA");
            if (configuracion != null && configuracion.Tipo == "INT")
            {
                int linea = Convert.ToInt32(configuracion.Valor);
                if (linea == id_linea_credito)
                {
                    DateTime nextDate = inicio;
                    while (nextDate <= final.Date)
                    {
                        if (nextDate.DayOfWeek != DayOfWeek.Saturday && nextDate.DayOfWeek != DayOfWeek.Sunday)
                            dias--;
                        nextDate = nextDate.AddDays(1);
                    }

                   


                    ////dias++;
                    //if (inicio.DayOfWeek != DayOfWeek.Saturday && inicio.DayOfWeek != DayOfWeek.Sunday)
                    //    dias--;
                }
            }
            return dias;
        }


        protected DateTime Fecha_Cercana_Simple(int id_credito, DateTime fecha_comparar,DateTime? fechanueva = null)
        {
            try
            {
                Linea_Credito_Historial historial = Historial(id_credito);
                DateTime fecha_n;
                Credito credito = _context.Credito.AsNoTracking()
                    .Include(s => s.Linea_Credito_Historial)
                    .Include(q => q.Linea_Credito)
                    .Include(s => s.Periodo)
                    .Include(t => t.Linea_Credito_Historial.Cobros_Periodicos_Historial)
                    .FirstOrDefault(f => f.Id_Credito == id_credito);
                if (fechanueva == null)
                    fecha_n = Verificar_Credito_Fecha_Movible(credito);
                else
                    fecha_n = (DateTime) fechanueva;

                ResultadoCalculoCreditio cpc = new CalculosYProcesoCredito(
                    historial.Interes_Comision,
                    credito.Monto_pago,
                    credito.Periodo,
                    IVA,
                    credito.Plazo,
                    historial.Interes,
                    fecha_n,
                    historial.Cobros_Periodicos_Historial.Count > 0 ? credito.Linea_Credito_Historial.Cobros_Periodicos_Historial.Sum(v => v.Valor) : 0.00M,
                    historial.PlanAyC_Cobros_Periodico_Dinero).Resultado;
                List<DateTime> fechas_desordenadas = cpc.Amortizacion.Select(l => l.FechaPago).ToList();
                if (fechas_desordenadas.Any(t => t.Date == fecha_comparar.Date))
                    return fecha_comparar;
                return EncontrarFechaCercana(fechas_desordenadas, fecha_comparar);
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }

        protected DateTime EncontrarFechaCercana(List<DateTime> fechas, DateTime hoy)
        {
            List<Fecha_Ordenada> fechas_ordenadas = new List<Fecha_Ordenada>();
            foreach (DateTime item in fechas)
            {
                /*sacar la la diferencia de fechas*/
                fechas_ordenadas.Add(new Fecha_Ordenada
                {
                    No = fechas_ordenadas.Count > 0 ? fechas_ordenadas.Count + 1 : 1,
                    Fecha = item.Date,
                    Dias = (hoy.Date - item.Date).Days
                });
            }

            int dias_menor_futuro = fechas_ordenadas.Any(s => s.Dias < 0) ? fechas_ordenadas.Where(s => s.Dias < 0).Max(m => m.Dias) : 0;
            int dias_menor_pasado = fechas_ordenadas.Any(s => s.Dias > 0) ? fechas_ordenadas.Where(s => s.Dias > 0).Min(m => m.Dias) : 0;
            int dias_menor = 0;
            if (dias_menor_futuro < dias_menor_pasado && dias_menor_futuro != 0)
                dias_menor = dias_menor_futuro;
            else
                dias_menor = dias_menor_pasado;

            return fechas_ordenadas.FirstOrDefault(q => q.Dias == dias_menor).Fecha;
        }

        protected Linea_Credito_Historial Historial(int id_credito)
        {
            return _context
                .Linea_Credito_Historial
                .Include(l => l.Cobros_Periodicos_Historial)
                .Single(c => c.Id_Credito == id_credito);
        }

        protected async Task<string> Generar_No_Comprobante()
        {
            //generador de comprobante
            int id_ultima_transaccion = 0;
            if (await _context.Transaccion_Caja.AnyAsync())
                id_ultima_transaccion = _context.Transaccion_Caja.AsEnumerable().Last().Id_Transaccion_Caja;
            return GeneradorCodigo.GenerarSoloPorEntidad(id_ultima_transaccion);
            //termina generador de comprobante
        }

        ///*con las acciones*/


        protected async Task<bool> Nueva_Accion(Accion accion)
        {
            if (!ModelState.IsValid)
                return false;
            try
            {
                bool ya_tiene = _context.Accion.Any(f => f.Id_Persona == accion.Id_Persona);
                if (!ya_tiene)
                {
                    int numero = UltimoIndice + 1;
                    accion.Codigo = numero.ToString();
                    accion.Cantidad_Accion = 1;//por defecto solo se compra una
                    //accion.No_Certificado = numero;
                    accion.Valor_Total = accion.Valor_Unitario * accion.Cantidad_Accion;
                    _context.Accion.Add(accion);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected int UltimoIndice
        {
            get
            {
                if (_context.Accion.AsNoTracking().Any())
                    return _context.Accion.AsNoTracking().Max(m => m.Id_Accion);
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EsCredito">es o no crédito</param>
        /// <param name="id"> id_credito o id_ahorro</param>
        /// <param name="dias">días de antiguedad</param>
        /// <returns></returns>
        protected async Task<DateTime> LapsoDias(bool EsCredito, int id, int dias, bool es_plazo = false)
        {
            DateTime ultima_fecha = DateTime.Now;
            if (EsCredito)
            {
                var credito = await _context.Credito.AsNoTracking().SingleAsync(f => f.Id_Credito == id);
                if (_context.Transaccion_Credito.AsNoTracking().Any(d => d.Id_Credito == credito.Id_Credito))
                    ultima_fecha = _context.Transaccion_Credito
                    .AsNoTracking()
                    .Where(d => d.Id_Credito == credito.Id_Credito)
                    .OrderBy(c=>c.Id_Transaccion_Credito)
                    .AsEnumerable().Last().FechaPago;
            }
            else
            {
                var ahorro = await _context.Ahorro.AsNoTracking().SingleAsync(f => f.Id_Ahorro == id);
                if (_context.Transaccion_Ahorro.AsNoTracking().Any(d => d.Id_Ahorro == ahorro.Id_Ahorro))
                    ultima_fecha = _context
                        .Transaccion_Ahorro
                    .AsNoTracking()
                    .Where(d => d.Id_Ahorro == ahorro.Id_Ahorro)
                    .OrderBy(c=>c.Id_Transaccion_Ahorro)
                    .AsEnumerable().Last().Fecha;
            }
            if (es_plazo) {
                var deposito = await _context.Deposito_Plazo.AsNoTracking().SingleAsync(f => f.Id_Deposito_Plazo == id);
                if (_context.Transaccion_Deposito_Plazo.AsNoTracking().Any(d => d.Id_Deposito_Plazo == deposito.Id_Deposito_Plazo))
                    ultima_fecha = _context
                        .Transaccion_Deposito_Plazo
                    .AsNoTracking()
                    .Where(d => d.Id_Deposito_Plazo == deposito.Id_Deposito_Plazo)
                    .OrderBy(c => c.Id_Transaccion_Deposito_Plazo)
                    .AsEnumerable().Last().Fecha;
            }
            return ultima_fecha.AddDays(-dias);
        }

        /// <summary>
        /// es para guardar la transacción
        /// de una cuenta de ahorro
        /// </summary>
        /// <param name="id_ahorro"></param>
        /// <param name="valor"></param>
        /// <param name="id_tipo_ahorro"></param>
        /// <param name="retiro"></param>
        /// <param name="interes"></param>
        /// <param name="comprobante"></param>
        /// <param name="es_traslado"></param>
        /// <param name="es_salario"></param>
        /// <param name="fecha_cambiada"></param>
        /// <param name="cambiar_fecha"></param>
        /// <returns></returns>
        protected async Task<Transaccion_Ahorro> GuardarSimple(
            int id_ahorro, 
            decimal valor, 
            int id_tipo_ahorro, 
            bool retiro,
            bool interes,
            string comprobante = "",
            bool es_traslado = false,/*es para traslado en pago a créditos, supongo*/
            bool es_salario=false,
            DateTime? fecha_cambiada = null,
            bool cambiar_fecha=false,
            bool esCapitalizacion = false,
            int id_depositoPlazo = 0)
        {
            /*verificar el último saldo*/
            decimal saldo = 0.00M;
            decimal interes_diario_aplicar = 0.00M;
            decimal interes_diario_acumulado = 0.00M;
            int dias = 0;
            DateTime fecha = DateTime.Now;
            if (esCapitalizacion && cambiar_fecha && fecha_cambiada.HasValue)
            {
                fecha = fecha_cambiada.Value;
            }
            else if ((User.IsInRole("Administrador") || User.IsInRole("Jefe")) && cambiar_fecha && fecha_cambiada.HasValue)
            {
                fecha = fecha_cambiada.Value;
            }
            if (esCapitalizacion)
            {
                var transaccionAnterior = _context.Transaccion_Ahorro
                    .Where(w => w.Id_Ahorro == id_ahorro && !w.Borrado_Logico)
                   .OrderByDescending(d => d.Fecha)
                    .ThenByDescending(d => d.Id_Transaccion_Ahorro)
                    .FirstOrDefault();

                saldo = transaccionAnterior?.Saldo ?? 0.00M;

                string Id_Usuario = _userManager.GetUserId(HttpContext.User);

                // Crear transacción sin cálculo de intereses ni contabilidad
                Transaccion_Ahorro transaccion = new Transaccion_Ahorro
                {
                    Id_Ahorro = id_ahorro,
                    Id_Tipo_Operacion_Ahorro = id_tipo_ahorro,
                    Saldo = retiro ? saldo - valor : saldo + valor,
                    Valor = valor,
                    Fecha = fecha,
                    Creado_En = fecha,
                    Creado_Por = Id_Usuario,
                    Interes_Aplicado = 0,
                    Interes_Acumulado = transaccionAnterior?.Interes_Acumulado ?? 0,
                    Dias = 0
                };

                _context.Transaccion_Ahorro.Add(transaccion);

                // Actualizar fecha de capitalización en Ahorro
                if(id_depositoPlazo > 0)
                {
                    var ahorro = await _context.Deposito_Plazo.FindAsync(id_depositoPlazo);
                    if (ahorro != null)
                    {
                        ahorro.Fecha_Ultima_Capitalizacion = fecha;
                        _context.Update(ahorro);
                    }

                    //await _context.SaveChangesAsync();
                    //return transaccion;
                }
                else
                {
                    var ahorro = await _context.Ahorro.FindAsync(id_ahorro);
                    if (ahorro != null)
                    {
                        ahorro.CapitalizacionAhorro = fecha;
                        _context.Update(ahorro);
                    }

                    //await _context.SaveChangesAsync();
                    //return transaccion;
                }

                return transaccion;
            }
            var transacciones = _context.Transaccion_Ahorro
              .Include(d => d.Ahorro)
              .Where(w => w.Id_Ahorro == id_ahorro && !w.Borrado_Logico)
              .OrderByDescending(d => d.Fecha)
               .ThenByDescending(d => d.Id_Transaccion_Ahorro)
              .ToList();
            var transaccion_anterior = transacciones.FirstOrDefault();
            if (transacciones.Any())
            {

                saldo = _context.Transaccion_Ahorro
                        .Where(w => w.Id_Ahorro == id_ahorro && !w.Borrado_Logico)
                        .OrderByDescending(d => d.Fecha)
                        .ThenByDescending(d => d.Id_Transaccion_Ahorro)
                        .Select(s => s.Saldo)
                        .FirstOrDefault();

                bool hayOtraTransaccionHoy = transacciones.Any(t => t.Fecha.Date == fecha.Date);

                if (!hayOtraTransaccionHoy)
                {
                    dias = (fecha.Date - transaccion_anterior.Fecha.Date).Days;

                    CalculosYProcesoAhorro calculo = new CalculosYProcesoAhorro();
                    interes_diario_aplicar = calculo.CalcularInteres(
                        transaccion_anterior.Fecha.Date,
                        fecha.Date,
                        saldo,
                        transaccion_anterior.Ahorro.Tasa_Interes);
                    interes_diario_acumulado = interes_diario_aplicar + transaccion_anterior.Interes_Acumulado;
                }
                else
                {
                    dias = 0;
                    interes_diario_aplicar = 0.00M;
                    interes_diario_acumulado = transaccion_anterior.Interes_Acumulado;
                }
            }

            try
            {
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                if (interes)/*se depositan los intereses ganados a la fecha*/
                {
                    valor = interes_diario_acumulado;
                    interes_diario_aplicar = 0.00M;
                    interes_diario_acumulado = 0.00M;
                    /*actualizo la útima fecha que hice el traslado*/
                    Ahorro ahorro = _context.Ahorro.Single(c=>c.Id_Ahorro==id_ahorro);
                    ahorro.Ultima_Fecha_Traslado = fecha;
                    if (esCapitalizacion)
                        ahorro.CapitalizacionAhorro = fecha;
                    _context.Update(ahorro).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                // 1. Crear transacción con valores iniciales en 0 (intereses/días)
                Transaccion_Ahorro transaccion = new Transaccion_Ahorro
                {
                    Id_Ahorro = id_ahorro,
                    Id_Tipo_Operacion_Ahorro = id_tipo_ahorro,
                    Saldo = retiro ? saldo - valor : saldo + valor,
                    Valor = valor,
                    Fecha = fecha,
                    Creado_En = fecha,
                    Creado_Por = Id_Usuario,
                    Interes_Aplicado = 0,
                    Interes_Acumulado = transacciones.FirstOrDefault()?.Interes_Acumulado ?? 0,
                    Dias = 0
                };
                _context.Transaccion_Ahorro.Add(transaccion);
                await _context.SaveChangesAsync();
                // Paso 1: Obtener todas las transacciones del día actual
                var transaccionesHoy = _context.Transaccion_Ahorro
                    .Include(t => t.Ahorro)
                    .Where(t => t.Id_Ahorro == id_ahorro
                        && !t.Borrado_Logico
                        && t.Fecha.Date == fecha.Date)
                    .OrderBy(t => t.Fecha)
                    .ThenBy(t => t.Id_Transaccion_Ahorro)
                    .ToList();

                // Paso 2: Identificar la última transacción del día (a esta le vamos a asignar el interés)
                var ultimaTransaccionHoy = transaccionesHoy.Last();

                // Paso 3: Obtener la última transacción del día anterior
                var transaccionDiaAnterior = _context.Transaccion_Ahorro
                    .Include(t => t.Ahorro)
                    .Where(t => t.Id_Ahorro == id_ahorro
                        && !t.Borrado_Logico
                        && t.Fecha.Date < fecha.Date)
                    .OrderByDescending(t => t.Fecha)
                    .ThenByDescending(t => t.Id_Transaccion_Ahorro)
                    .FirstOrDefault();

                // Paso 4: Verificar que haya transacción anterior
                if (transaccionDiaAnterior != null)
                {
                    int diast = (fecha.Date - transaccionDiaAnterior.Fecha.Date).Days;
                    decimal saldoActual = transaccion.Saldo;
                    decimal tasaInteres = transaccionDiaAnterior.Ahorro.Tasa_Interes;
                    decimal interesCalculado = 0;

                    if (diast > 0 && saldoActual > 0 && tasaInteres > 0)
                    {
                        CalculosYProcesoAhorro calculo = new CalculosYProcesoAhorro();
                        interesCalculado = calculo.CalcularInteres(
                            transaccionDiaAnterior.Fecha.Date,
                            fecha.Date,
                            saldoActual,
                            tasaInteres
                        );
                    }

                    // Aplicar el interés solo a la última transacción del día
                    ultimaTransaccionHoy.Dias = diast;
                    ultimaTransaccionHoy.Interes_Aplicado = interesCalculado;
                    ultimaTransaccionHoy.Interes_Acumulado = interesCalculado + transaccionDiaAnterior.Interes_Acumulado;
                    _context.Update(ultimaTransaccionHoy);
                }

                // Paso 5: Resetear las demás transacciones del día
                foreach (var t in transaccionesHoy.Where(t => t.Id_Transaccion_Ahorro != ultimaTransaccionHoy.Id_Transaccion_Ahorro))
                {
                    t.Dias = 0;
                    t.Interes_Aplicado = 0;
                    int idx = transaccionesHoy.IndexOf(t);
                    t.Interes_Acumulado = idx == 0
                        ? t.Interes_Acumulado
                        : transaccionesHoy[idx - 1].Interes_Acumulado;

                    _context.Update(t);
                }


                // await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();
                Caja caja = await Caja_Actual();
                Transaccion_Caja trans_caja = new Transaccion_Caja();
                if (es_traslado || es_salario)
                {
                    trans_caja = Registrar_Caja(transaccion, 26, fecha, !retiro, comprobante);
                }
                else
                {
                    trans_caja = Registrar_Caja(transaccion, caja.Id_Caja, fecha, !retiro, comprobante);
                }
               
                _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
                await _context.SaveChangesAsync();
                //guardar la partida contable
                await Manejo_Contable_Ahorro(retiro, interes, transaccion, trans_caja.Id_Transaccion_Caja);
                return transaccion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task RegistrarCapitalizacionDepositoPlazo(
            int idDepositoPlazo,
            decimal valor,
            DateTime fecha)
                {
                    string idUsuario = _userManager.GetUserId(HttpContext.User);

                    var ultimaTransaccion = await _context.Transaccion_Deposito_Plazo
                        .Where(x => x.Id_Deposito_Plazo == idDepositoPlazo
                                 && !x.Borrado_Logico)
                        .OrderByDescending(x => x.Fecha)
                        .ThenByDescending(x => x.Id_Transaccion_Deposito_Plazo)
                        .FirstOrDefaultAsync();

                    decimal saldoAnterior = ultimaTransaccion?.Saldo ?? 0m;

                    // ===== DEPÓSITO =====

                    var deposito = new Transaccion_Deposito_Plazo
                    {
                        Id_Deposito_Plazo = idDepositoPlazo,
                        Id_Tipo_Operacion_Ahorro = 1, // depósito
                        Valor = valor,
                        Saldo = saldoAnterior + valor,
                        Fecha = fecha,

                        Dias = 0,
                        Interes_Aplicado = 0,
                        Interes_Acumulado = ultimaTransaccion?.Interes_Acumulado ?? 0,

                        Creado_En = fecha,
                        Creado_Por = idUsuario
                    };

                    _context.Transaccion_Deposito_Plazo.Add(deposito);

                    // ===== RETIRO =====

                    var retiro = new Transaccion_Deposito_Plazo
                    {
                        Id_Deposito_Plazo = idDepositoPlazo,
                        Id_Tipo_Operacion_Ahorro = 3, // retiro
                        Valor = valor,
                        Saldo = saldoAnterior,
                        Fecha = fecha,

                        Dias = 0,
                        Interes_Aplicado = 0,
                        Interes_Acumulado = deposito.Interes_Acumulado,

                        Creado_En = fecha,
                        Creado_Por = idUsuario
                    };

                    _context.Transaccion_Deposito_Plazo.Add(retiro);
        }
        /// <summary>
        /// es para guardar la transacción
        /// de una cuenta de ahorro
        /// </summary>
        /// <param name="id_deposito"></param>
        /// <param name="valor"></param>
        /// <param name="id_tipo_ahorro"></param>
        /// <param name="retiro"></param>
        /// <param name="interes"></param>
        /// <param name="comprobante"></param>
        /// <param name="es_traslado"></param>
        /// <param name="es_salario"></param>
        /// <param name="fecha_cambiada"></param>
        /// <param name="cambiar_fecha"></param>
        /// <returns></returns>
        protected async Task<Transaccion_Deposito_Plazo> GuardarSimpleDepositoPlazo(
            int id_deposito,
            decimal valor,
            int id_tipo_ahorro,
            bool interes,
            bool retiro,
            string comprobante = "",
            bool es_salario = false,
            DateTime? fecha_cambiada = null,
            bool cambiar_fecha = false)
        {
            /*verificar el último saldo*/
            decimal saldo = 0.00M;
            decimal interes_diario_aplicar = 0.00M;
            decimal interes_diario_acumulado = 0.00M;

            DateTime fecha = DateTime.Now;
            if (User.IsInRole("Administrador") || User.IsInRole("Jefe"))
            {
                if (cambiar_fecha)
                    fecha = Convert.ToDateTime(fecha_cambiada);
            }

            if (_context.Transaccion_Deposito_Plazo.Any(w => w.Id_Deposito_Plazo == id_deposito))
            {
                CalculosYProcesoAhorro calculo = new CalculosYProcesoAhorro();
                Transaccion_Deposito_Plazo transaccion_anterior = _context
                        .Transaccion_Deposito_Plazo
                        .Include(d => d.Deposito_Plazo)
                        .Where(w => w.Id_Deposito_Plazo == id_deposito && !w.Borrado_Logico)
                        .OrderBy(d => d.Id_Transaccion_Deposito_Plazo)
                        .AsEnumerable()
                        .Last();//saldo erróneo
                saldo = transaccion_anterior.Saldo;
                interes_diario_aplicar = calculo.CalcularInteres(
                    transaccion_anterior.Fecha.Date,
                    fecha.Date,
                    saldo,
                    transaccion_anterior.Deposito_Plazo.Interes);
                interes_diario_acumulado = interes_diario_aplicar + transaccion_anterior.Interes_Acumulado;
            }
            try
            {
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                if (interes)/*se depositan los intereses ganados a la fecha*/
                {
                    valor = interes_diario_acumulado;
                    interes_diario_aplicar = 0.00M;
                    interes_diario_acumulado = 0.00M;
                    /*actualizo la útima fecha que hice el traslado*/
                    Deposito_Plazo deposito = _context.Deposito_Plazo.Single(c => c.Id_Ahorro == id_deposito);
                    //ahorro.Ultima_Fecha_Traslado = fecha;
                    _context.Update(deposito).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                Transaccion_Deposito_Plazo transaccion = new Transaccion_Deposito_Plazo
                {
                    Id_Deposito_Plazo = id_deposito,
                    Id_Tipo_Operacion_Ahorro = id_tipo_ahorro,
                    Saldo = retiro ? saldo - valor : saldo + valor,
                    Valor = valor,
                    Fecha = fecha,
                    Creado_En = fecha,
                    Creado_Por = Id_Usuario,
                    /*manejo de los intereses*/
                    Interes_Aplicado = interes_diario_aplicar,
                    Interes_Acumulado = interes_diario_acumulado
                };
                _context.Transaccion_Deposito_Plazo.Add(transaccion).State = EntityState.Added;

                await _context.SaveChangesAsync();
                Caja caja = await Caja_Actual();
                Transaccion_Caja trans_caja = new Transaccion_Caja();

                trans_caja = Registrar_Caja(transaccion, caja.Id_Caja, fecha, !retiro, comprobante);

                _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
                await _context.SaveChangesAsync();
                //guardar la partida contable
          //      await Manejo_Contable_Ahorro(retiro, interes, transaccion, trans_caja.Id_Transaccion_Caja);

                return transaccion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// es el pago de créditos
        /// </summary>
        /// <param name="transaccion">la transacción que se va a guardar</param>
        /// <param name="comprobante">número de comprobante</param>
        /// <param name="refinanciamiento">banderita que indica que es un refinanciamiento</param>
        /// <param name="recargo">indica que hay recargo que hay que aplicar</param>
        /// <param name="ya_cancelado"></param>
        /// <returns></returns>
        protected async Task<Transaccion_Credito> EfectuarPago(
            Transaccion_Credito transaccion, 
            string comprobante = "", 
            bool refinanciamiento = false,
            bool recargo = false, 
            bool ya_cancelado = false)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                /*cambiar la fecha de la transacción*/
                if (User.IsInRole("Asesor") || User.IsInRole("Asistente"))
                {
                    var diferente = transaccion.FechaPago.Date - fecha;
                    var dias = diferente.Days;
                    if (dias != 0)
                        transaccion.FechaPago = fecha;
                }

                /*bitacora transaccion*/
                string Id_Usuario = _userManager.GetUserId(HttpContext.User);
                /*se registra como un ingreso a caja*/
                Caja caja = await Caja_Actual();
                if (refinanciamiento)
                    caja = await obtener_Caja_Sistema();

                transaccion.Creado_En = transaccion.FechaPago;
                transaccion.Creado_Por = Id_Usuario;
                _context.Transaccion_Credito.Add(transaccion).State = EntityState.Added;
                _context.SaveChanges();
                Transaccion_Caja trans_caja = Registrar_Caja(transaccion, caja.Id_Caja, transaccion.FechaPago, true, comprobante);
                _context.Transaccion_Caja.Add(trans_caja).State = EntityState.Added;
                //detalle asiento contable
                _context.SaveChanges();
                await Pago_Credito_Contabilidad(transaccion, trans_caja.Id_Transaccion_Caja, refinanciamiento);
                //ver si cancela el crédito
                if (ya_cancelado || transaccion.Saldo_Capital ==0.00M)
                    await Cancelar_Credito(transaccion.Id_Credito, transaccion.Saldo_Capital);
                if (transaccion.Recargo > 0)
                {
                    await ActualizarRecargoAmortizacion(transaccion.Id_Credito, transaccion.FechaPago, transaccion.Recargo);
                }
                return transaccion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task ActualizarRecargoAmortizacion(int idCredito, DateTime fechaPago, decimal recargo)
        {
            // Buscar la cuota exacta para la fecha de pago
            var amortizacion = await _context.Amortizacion
                .Where(a => a.Id_Credito == idCredito && a.FechaPago.Date == fechaPago.Date)
                .FirstOrDefaultAsync();

            // Si no hay cuota exacta, buscar la más cercana anterior
            if (amortizacion == null)
            {
                amortizacion = await _context.Amortizacion
                    .Where(a => a.Id_Credito == idCredito && a.FechaPago.Date <= fechaPago.Date)
                    .OrderByDescending(a => a.FechaPago) // Obtener la fecha más cercana pero anterior
                    .FirstOrDefaultAsync();
            }

            if (amortizacion != null)
            {
                // Si el recargo ya tiene un valor, no lo sobreescribimos
                if (amortizacion.Recargo == 0)
                {
                    amortizacion.Recargo = recargo;
                    _context.Amortizacion.Update(amortizacion);
                    await _context.SaveChangesAsync();
                }
            }
        }
    
        

        /// <summary>
        /// evalua y cancela un crédito
        /// cada vez que se paga
        /// </summary>
        /// <param name="id_credito">registro del crédito que desaa actualizar</param>
        /// <returns></returns>
        private async Task<bool> Cancelar_Credito(int id_credito, decimal saldo_actual)
        {
            bool respuesta=false;
            Credito credito = await _context.Credito
                .Include(f => f.Persona)
                .Include(c => c.Comites)
                .Include(a => a.Persona.Evaluaciones)
                .Include(h => h.Transacciones_Creditos)
                .Include(p => p.Periodo)
                .Include(p => p.Comportamiento_Credito)
                .SingleOrDefaultAsync(d => d.Id_Credito == id_credito);

            if (saldo_actual == 0.00M)
            {
                /*se actualizar el estado del crédito*/
                credito.Cancelado = true;
                credito.Cancelado_En = DateTime.Now;
                _context.Entry(credito).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                respuesta= true;
            }
            return respuesta;
        }

        /// <summary>
        /// para manejar los créditos
        /// colocado en un rango de fecha
        /// o fecha única
        /// </summary>
        /// <param name="inicio">fecha inicial del rango</param>
        /// <param name="fin">fecha final del rango</param>
        /// <param name="id_agencia">identificador de la fecha</param>
        /// <param name="id_comportamiento"></param>
        /// <param name="fecha_unica"></param>
        /// <param name="es_rango"></param>
        /// <returns></returns>
        protected async Task<List<Creditos_Colocados>> Obtener_Creditos_Colocados(
            DateTime inicio,
            DateTime fin,
            int id_agencia,
            int id_comportamiento,
            DateTime fecha_unica,
            bool es_rango=false)
        {
            try
            {
                Func<Credito, bool> filtro = null;

                if (es_rango)
                {
                    if (id_comportamiento > 0)
                        filtro = new Func<Credito, bool>(c =>
                                    c.Liquidado_En.Date >= inicio.Date &&
                                    c.Liquidado_En.Date <= fin.Date &&
                                    c.Id_Comportamiento_Credito == id_comportamiento);
                    if (id_comportamiento == 0)
                        filtro = new Func<Credito, bool>(c =>
                                    c.Liquidado_En.Date >= inicio.Date &&
                                    c.Liquidado_En.Date <= fin.Date);
                }
                else
                {
                    if (id_comportamiento > 0)
                        filtro = new Func<Credito, bool>(c =>
                                    c.Liquidado_En.Date == fecha_unica.Date &&
                                    c.Id_Comportamiento_Credito == id_comportamiento);
                    if (id_comportamiento == 0)
                        filtro = new Func<Credito, bool>(c =>
                                    c.Liquidado_En.Date == fecha_unica.Date);
                }
                
                
                List<Credito> creditos = (from c in _context.Credito
                        .AsNoTracking()
                        .Include(d => d.Persona)
                        .Include(f => f.Linea_Credito_Historial)
                        .Include(t => t.Comportamiento_Credito)
                    .Where(filtro).ToList()
                                          join cr in
                                              _context.Cartera.AsNoTracking().ToList() on
                                              c.Id_Cartera equals cr.Id_Cartera
                                          join u in
                                              _context.Usuario_Agencia.AsNoTracking().ToList() on
                                              cr.Id_Usuario equals u.Id_Usuario
                                          join a in
                                              _context.Agencia.AsNoTracking().ToList() on
                                              u.Id_Agencia equals a.Id_Agencia
                                          where u.Id_Agencia == id_agencia
                                          select c).ToList();
                List<Creditos_Colocados> colocacion = new List<Creditos_Colocados>();
                foreach (Credito creditos_c in creditos)
                {
                    decimal total = 0.00M;
                    decimal ref_credito = 0.00M;
                    decimal totalGravados = 0.00M;
                    decimal totalExcentos = 0.00M;
                    decimal total_entregado = 0.00M;
                    bool reestructura = false;
                    Linea_Credito_Historial lch = _context.Linea_Credito_Historial
                        .AsNoTracking()
                        .Single(a => a.Id_Credito == creditos_c.Id_Credito);
                    Func<Deduccion_Historial, bool> validarFunc =new Func<Deduccion_Historial, bool> (d => d.Id_Linea_Credito_Historial == lch.Id_Linea_Credito_Historial && d.Id_Credito_Financiado > 0);
                    if (_context.Deduccion_Historial.Any(validarFunc))
                    {
                        List<Deduccion_Historial> dh = _context.Deduccion_Historial.AsNoTracking()
                            .Where(validarFunc).ToList();
                        foreach (Deduccion_Historial creditos_de in dh) {
                            
                            var pagoRefinanciamiento = _context.Transaccion_Credito
                            .Where(a => a.Id_Credito == creditos_de.Id_Credito_Financiado)
                            .OrderByDescending(a => a.FechaPago) 
                            .FirstOrDefault();
                            if (pagoRefinanciamiento != null)
                            {
                                totalGravados += pagoRefinanciamiento.Interes  + pagoRefinanciamiento.Recargo ;
                                totalExcentos += pagoRefinanciamiento.Interes_Comision ;
                            }

                            ref_credito += creditos_de.Valor;
                            if (creditos_de.Nombre == "Reestructura")
                            {
                                reestructura = true;
                            }
                        }
                    }
                    total = creditos_c.Monto_pago;
                    if (reestructura == true)
                    {
                        total_entregado = 0.00M;
                    }
                    else
                    {
                        total_entregado = total - ref_credito;

                    }
                    colocacion.Add(new Creditos_Colocados
                    {
                        Codigo_Credito = creditos_c.Codigo_Credito,
                        Nombre_Cliente = creditos_c.Persona.NombreCompleto,
                        Monto = creditos_c.Monto_pago,
                        Tramites = 0.00M,
                        Refinanciamiento = ref_credito,
                        totalGravados = totalGravados,
                        totalExcentos = totalExcentos,
                        Total = total_entregado
                    });
                }

                return await Task.Run(()=> colocacion);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        protected DateTime Verificar_Credito_Fecha_Movible(Credito credito)
        {
            DateTime fecha_inicio = DateTime.Now;
            DateTime fecha2;
            Configuracion_General fecha_proxima;
            fecha_proxima = _context.Configuracion_General.AsNoTracking()
                .Single(a => a.Nombre == "DIA_PAGO");
            if (credito.Id_Linea ==5 && credito.Fecha_ProntoPago != DateTime.MinValue)
            {
                fecha2 = credito.Fecha_ProntoPago;
                int dia = fecha2.Day;
                int mes = fecha2.Month;
                int año = fecha2.Year;
                if (credito.Linea_Credito.Movible)
                {
                    if (dia >= int.Parse(fecha_proxima.Valor))
                    {
                        var ok = Procesar_Meses(mes, año, dia);
                        fecha_inicio = credito.Fecha_ProntoPago.AddDays(Convert.ToDouble(ok));
                    }
                    else
                    {
                        fecha_inicio = credito.Fecha_ProntoPago;
                    }
                }
                else
                {
                    fecha_inicio = credito.Fecha_ProntoPago;
                }
            }
            else
            {
                fecha2 = credito.Liquidado_En;
                int dia = fecha2.Day;
                int mes = fecha2.Month;
                int año = fecha2.Year;
                if (credito.Linea_Credito.Movible)
                {
                    if (dia >= int.Parse(fecha_proxima.Valor))
                    {
                        var ok = Procesar_Meses(mes, año, dia);
                        fecha_inicio = credito.Liquidado_En.AddDays(Convert.ToDouble(ok));
                    }
                    else
                    {
                        fecha_inicio = credito.Liquidado_En;
                    }
                }
                else
                {
                    fecha_inicio = credito.Liquidado_En;
                }
            }
           

            return fecha_inicio;
        }
        protected bool VerificarPlanAyC(Credito credito)
        {
            var lineaCredito = _context.Linea_Credito.FirstOrDefault(a => a.Id_Linea == credito.Id_Linea)?.Codigo;
            var codigosInvalidos = new List<int> { 13, 14, 15, 25 };
            if (string.IsNullOrEmpty(lineaCredito) || !int.TryParse(lineaCredito, out int codigo))
            {
                return false; 
            }
            return !codigosInvalidos.Contains(codigo);

        }
        protected int Procesar_Meses(int month, int year,int day)
        {
            int dias_aplicados = 0;
            int bisiesto = 0;
            if (Convert.ToInt32(year) % 4 == 0 && Convert.ToInt32(year) % 100 != 0 || Convert.ToInt32(year) % 400 == 0)
            {
                bisiesto = day switch
                {
                    25 => 5,
                    26 => 4,
                    27 => 3,
                    28 => 2,
                    29 => 1,
                    _ => bisiesto
                };
            }
            else
            {
                bisiesto = day switch
                {
                    25 => 4,
                    26 => 3,
                    27 => 2,
                    28 => 1,
                    _ => bisiesto
                };
            }

            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                dias_aplicados = day switch
                {
                    25 => 7,
                    26 => 6,
                    27 => 5,
                    28 => 4,
                    29 => 3,
                    30 => 2,
                    31 => 1,
                    _ => dias_aplicados
                };
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                dias_aplicados = day switch
                {
                    25 => 6,
                    26 => 5,
                    27 => 4,
                    28 => 3,
                    29 => 2,
                    30 => 1,
                    _ => dias_aplicados
                };
            }
            else if (month == 2)
            {
                dias_aplicados = bisiesto;
            }

            return dias_aplicados;
        }

        protected async Task<Caja> obtener_Caja_Sistema()
        {
            try
            {
                Configuracion_General Obtener_Caja = _context.Configuracion_General.AsNoTracking().Single(a =>a.Nombre =="CAJA_MAQUINA");
                int id_caja = Int32.Parse(Obtener_Caja.Valor);
                Caja caja_sistema = _context.Caja.AsNoTracking().Single(b => b.Id_Caja == id_caja);
                return caja_sistema;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected ResultadoCalculoCreditio Amortizacion(int id_linea, decimal monto, int id_periodo, int plazo, int id_credito = 0)
        {
            Periodo periodo = _context.Periodo.AsNoTracking().SingleOrDefault(d => d.Id_Periodo == id_periodo);
            /*obtener el dato de la línea de credito*/
            Linea_Credito lc = _context.Linea_Credito.AsNoTracking().Include(f => f.Cobros_Credito).Single(t => t.Id_Linea == id_linea);
            decimal total = lc.Cobros_Credito.Count > 0 ? lc.Cobros_Credito.Sum(v => v.Valor) : 0.00M;
            DateTime fecha_inicio = DateTime.Now;
            /*traer el iva legislado de las configuraciones*/
            decimal iva = IVA;
            if (id_credito > 0)
            {
                Credito credito = _context
                    .Credito
                    .AsNoTracking()
                    .Include(d=>d.Linea_Credito)
                    .Include(t => t.Linea_Credito_Historial)
                    .Include("Linea_Credito_Historial.Cobros_Periodicos_Historial")
                    .SingleOrDefault(t => t.Id_Credito == id_credito);
                if (credito.Liquidado)
                    fecha_inicio = Verificar_Credito_Fecha_Movible(credito);
                Linea_Credito_Historial lch = _context
                    .Linea_Credito_Historial
                    .AsNoTracking()
                    .Single(s => s.Id_Credito == credito.Id_Credito);

                total = _context.Cobro_Periodico_Historial.AsNoTracking().Where(a => a.Id_Linea_Credito_Historial == lch.Id_Linea_Credito_Historial).Sum(d => d.Valor);
                return new CalculosYProcesoCredito(lch.Interes_Comision,
                    monto,
                    periodo,
                    iva,
                    plazo,
                    lch.Interes,
                    fecha_inicio,
                    total,
                    lch.PlanAyC_Cobros_Periodico_Dinero).Resultado;
            }
            return new CalculosYProcesoCredito(lc.Interes_Comision,
                monto,
                periodo,
                iva,
                plazo,
                lc.Interes,
                fecha_inicio,
                total,
                lc.PlanAyC_Cobros_Periodico_Dinero).Resultado;
        }

        /// <summary>
        /// genera el código complejo
        /// que se necesita
        /// </summary>
        /// <param name="id_persona">id del cliente</param>
        /// <param name="id_linea">id de la línea de crédito o ahorro</param>
        /// <param name="es_credito">se indica si es crédito o ahorro</param>
        /// <returns></returns>
        protected string GenerarCodigo(
            int id_persona,
            int id_linea,
            bool es_credito
            )
        {
            int id_ultimo_codigo = 0;
            var persona = _context
                .Personas
                .Include(f=>f.Cartera)
                .Single(b=>b.Id_Persona==id_persona);
            var usuario_agencia = _context
                .Usuario_Agencia
                .Single(m=>m.Id_Usuario==persona.Cartera.Id_Usuario);

            Func<Codigo_Agencia, bool> filtro_codigo_persona = new Func<Codigo_Agencia, bool>(
                    d=>d.Id_Persona==id_persona && 
                    d.Es_Ahorro);

            if(es_credito)
                filtro_codigo_persona = new Func<Codigo_Agencia, bool>(
                        d => d.Id_Persona == id_persona && 
                        d.Es_Credito);

            List<Codigo_Agencia> codigo_persona = _context
                .Codigo_Agencia
                .Where(filtro_codigo_persona).ToList();

            if (codigo_persona.Count > 0)
            {
                Func<Codigo_Agencia, bool> filtro_codigo_linea = new Func<Codigo_Agencia, bool>(
                        d => d.Id_Agencia == usuario_agencia.Id_Agencia && 
                        d.Es_Ahorro);
                if(es_credito)
                    filtro_codigo_linea = new Func<Codigo_Agencia, bool>(
                            d => d.Id_Agencia == usuario_agencia.Id_Agencia && 
                            d.Es_Credito);

                List<Codigo_Agencia> codigo_linea = codigo_persona
                    .Where(filtro_codigo_linea).ToList();
                if (codigo_linea.Count > 0)
                    id_ultimo_codigo = codigo_linea.Max(c => c.Id_Codigo_Agencia);
                else
                    id_ultimo_codigo = codigo_persona.Max(d => d.Id_Codigo_Agencia);
            }
            else
            {
                Func<Codigo_Agencia, bool> filtro_codigo_agencia = new Func<Codigo_Agencia, bool>(
                        d => d.Id_Agencia == usuario_agencia.Id_Agencia && 
                        d.Es_Ahorro);
                if (es_credito)
                    filtro_codigo_agencia = new Func<Codigo_Agencia, bool>(
                            d => d.Id_Agencia == usuario_agencia.Id_Agencia && 
                            d.Es_Credito);

                List<Codigo_Agencia> codigo_agencias = _context
                    .Codigo_Agencia
                    .Where(filtro_codigo_agencia).ToList();
                if (codigo_agencias.Count > 0)
                    id_ultimo_codigo = codigo_agencias
                    .Max(d => d.Id_Codigo_Agencia);
            }
            string nuevo_codigo = null;
            Codigo_Agencia codigo = null;
            if (id_ultimo_codigo > 0)
            {
                codigo = _context
                  .Codigo_Agencia
                  .Single(d => d.Id_Codigo_Agencia == id_ultimo_codigo);
                int nuevo_correlativo = codigo.Correlativo + 1;
                int nuevo_correlativo_producto = codigo.Correlativo_Producto + 1;
                nuevo_codigo = ArmarCodigo(
                    persona.Dui,
                    nuevo_correlativo,
                    codigo.Id_Agencia,
                    codigo.Id_Linea,
                    nuevo_correlativo_producto);

                Codigo_Agencia nuevo_codigo_agencia = new Codigo_Agencia 
                { 
                    Id_Agencia=codigo.Id_Agencia,
                    Es_Ahorro=!es_credito,
                    Es_Credito=es_credito,
                    Id_Linea=codigo.Id_Linea,
                    Id_Persona=codigo.Id_Persona,
                    Correlativo= nuevo_correlativo,
                    Correlativo_Producto= nuevo_correlativo_producto
                };
                _context.Codigo_Agencia.Add(nuevo_codigo_agencia).State=EntityState.Added;
                _context.SaveChanges();
            }
            else {
                nuevo_codigo = ArmarCodigo(
                    persona.Dui,
                    1,
                    usuario_agencia.Id_Agencia,
                    id_linea,
                    1);

                Codigo_Agencia nuevo_codigo_agencia = new Codigo_Agencia
                {
                    Id_Agencia = usuario_agencia.Id_Agencia,
                    Es_Ahorro = !es_credito,
                    Es_Credito = es_credito,
                    Id_Linea = id_linea,
                    Id_Persona = id_persona,
                    Correlativo = 1,
                    Correlativo_Producto = 1
                };
                _context.Codigo_Agencia.Add(nuevo_codigo_agencia).State = EntityState.Added;
                _context.SaveChanges();
            }
            return nuevo_codigo;
        }

        /// <summary>
        /// armar el código como tal
        /// </summary>
        /// <param name="dui"></param>
        /// <param name="correlativo"></param>
        /// <param name="id_agencia"></param>
        /// <param name="id_linea"></param>
        /// <param name="correlativo_producto"></param>
        /// <returns></returns>
        protected string ArmarCodigo(
            string dui,
            int correlativo,
            int id_agencia,
            int id_linea,
            int correlativo_producto)
        {
            string duisinguion = DUIsinGuion(dui);
            string codigo = string.Format(
                "{0}-{1}-{2}-{3}-{4}",
                duisinguion,
                correlativo,
                id_agencia,
                id_linea,
                correlativo_producto
            );
            return codigo;
        }

        /// <summary>
        /// quitar el guion del dui
        /// </summary>
        /// <param name="dui"></param>
        /// <returns></returns>
        protected string DUIsinGuion(string dui)
        {
            string[] dividido = dui.Split("-");
            return dividido[0] + dividido[1];
        }


        /// <summary>
        /// la devastación se acerca
        /// para los ahorros existentes
        /// </summary>
        protected void GenerarAhorroAgenciaCodigo(int id_agencia)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    int centinela = 0;
                    var carteras = (from ua in _context.Usuario_Agencia
                                    join ca in _context.Cartera on ua.Id_Usuario equals ca.Id_Usuario
                                    where ua.Id_Agencia == id_agencia
                                    select ca).ToList();
                    foreach (var cartera in carteras)
                    {
                        IEnumerable<IGrouping<int, Ahorro>> personas_ahorros = _context.Ahorro
                            .Include(t => t.Persona)
                            .Where(d => d.Persona.Id_Cartera == cartera.Id_Cartera)
                            .OrderBy(c => c.Id_Persona)
                            .AsEnumerable()
                            .GroupBy(d => d.Id_Persona);

                        foreach (var pa in personas_ahorros)
                        {
                            IEnumerable<IGrouping<int, Ahorro>> lineas_ahorros = pa
                                .OrderBy(c => c.Id_Linea_Ahorro)
                                .AsEnumerable()
                                .GroupBy(d => d.Id_Linea_Ahorro);

                            centinela += 1;
                            foreach (var lc in lineas_ahorros)
                            {
                                int centinela_producto = 0;
                                foreach (var ahorro in lc)
                                {
                                    centinela_producto += 1;
                                    /*creo el objeto*/
                                    Codigo_Agencia action_tabla = new Codigo_Agencia
                                    {
                                        Id_Agencia = id_agencia,
                                        Es_Ahorro=true,
                                        Id_Linea = ahorro.Id_Linea_Ahorro,
                                        Id_Persona = ahorro.Id_Persona,
                                        Correlativo = centinela,
                                        Correlativo_Producto = centinela_producto
                                    };
                                    _context.Add(action_tabla).State = EntityState.Added;
                                    _context.SaveChanges();
                                    /*actualizar codigo del crédito*/
                                    string codigo = ArmarCodigo(
                                        ahorro.Persona.Dui, 
                                        centinela, 
                                        id_agencia, 
                                        ahorro.Id_Linea_Ahorro, 
                                        centinela_producto);
                                    ahorro.No_Ahorro = codigo;
                                    _context.Entry(ahorro).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                    dbContextTransaccion.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaccion.Rollback();
                }
            }
        }


        /// <summary>
        /// la devastación se acerca
        /// para los creditos existentes
        /// </summary>
        protected void GenerarCreditoAgenciaCodigo(int id_agencia)
        {
            using (var dbContextTransaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    int centinela = 0;
                    var carteras = (from ua in _context.Usuario_Agencia
                                   join ca in _context.Cartera on ua.Id_Usuario equals ca.Id_Usuario
                                   where ua.Id_Agencia==id_agencia select ca).ToList();
                    foreach (var cartera in carteras)
                    {
                        IEnumerable<IGrouping<int,Credito>> personas_creditos = _context.Credito
                            .Include(t=>t.Persona)
                            .Where(d => d.Id_Cartera == cartera.Id_Cartera)
                            .OrderBy(c=> c.Id_Persona)
                            .AsEnumerable()
                            .GroupBy(d => d.Id_Persona);

                        foreach (var pc in personas_creditos)
                        {
                            IEnumerable<IGrouping<int,Credito>> lineas_creditos = pc
                                .OrderBy(c=>c.Id_Linea)
                                .AsEnumerable()
                                .GroupBy(d=>d.Id_Linea);

                            centinela += 1;
                            foreach (var lc in lineas_creditos)
                            {
                                int centinela_producto = 0;
                                foreach (var credito in lc)
                                {
                                    centinela_producto += 1;
                                    /*creo el objeto*/
                                    Codigo_Agencia action_tabla = new Codigo_Agencia
                                    {
                                        Id_Agencia = id_agencia,
                                        Es_Credito=true,
                                        Id_Linea = credito.Id_Linea,
                                        Id_Persona=credito.Id_Persona,
                                        Correlativo = centinela,
                                        Correlativo_Producto=centinela_producto
                                    };
                                    _context.Add(action_tabla).State = EntityState.Added;
                                    _context.SaveChanges();
                                    /*actualizar codigo del crédito*/
                                    string codigo = ArmarCodigo(credito.Persona.Dui, centinela, id_agencia, credito.Id_Linea, centinela_producto);
                                    credito.Codigo_Credito = codigo;
                                    _context.Entry(credito).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                    dbContextTransaccion.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaccion.Rollback();
                }
            }
        }

        protected async Task<decimal> Obtener_Saldo_Ahorro(int id_persona)
        {
            decimal saldo_ahorro = 0.00M;
            try
            {
                int ah = _context.Ahorro.AsNoTracking().First(a => a.Id_Persona == id_persona && a.Id_Linea_Ahorro ==3).Id_Ahorro;
                List<Transaccion_Ahorro> t_ahorro = _context.Transaccion_Ahorro.AsNoTracking()
                    .Where(a => a.Id_Ahorro == ah).ToList();
                if (t_ahorro.Count >0)
                {
                    saldo_ahorro = t_ahorro.Last().Saldo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return saldo_ahorro;
        }
        private async Task<List<Transaccion_Credito>> Transaccion_Credito(int id_credito)
        {
            List<Transaccion_Credito> lista = new List<Transaccion_Credito>();
            try
            {
                if (_context.Transaccion_Credito.Any(a => a.Id_Credito == id_credito))
                    lista = await _context.Transaccion_Credito.Where(tc => tc.Id_Credito == id_credito && !tc.Borrado_Logico).OrderByDescending(w => w.Id_Transaccion_Credito).ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return lista;
        }
        /// <summary>
        /// se efectúa una bitacora por Reversiones
        /// <param name="id_transaccion"></param>
        /// <param name="es_credito"></param>
        /// <returns></returns>
        protected async Task Bitacora(
            string Tabla_Operacion, 
            string Proceso, 
            int id_transaccion, 
            bool es_credito, 
            bool reversion_credito = false,
            decimal monto=0.00M)
        {
            try
            {
                Persona persona = new Persona();
                if (es_credito)
                {
                    persona = (from p in _context.Personas
                               join c in _context.Credito on p.Id_Persona equals c.Id_Persona
                               join t in _context.Transaccion_Credito on c.Id_Credito equals t.Id_Credito
                               where t.Id_Transaccion_Credito == id_transaccion
                               select p).FirstOrDefault();
                }
                if (!es_credito) {
                    persona = (from p in _context.Personas
                               join a in _context.Ahorro on p.Id_Persona equals a.Id_Persona
                               join t in _context.Transaccion_Ahorro on a.Id_Ahorro equals t.Id_Ahorro
                               where t.Id_Transaccion_Ahorro == id_transaccion
                               select p).FirstOrDefault();
                }
                if (reversion_credito)
                {
                    persona = (from p in _context.Personas
                               join c in _context.Credito on p.Id_Persona equals c.Id_Persona
                               where c.Id_Credito == id_transaccion
                               select p).FirstOrDefault();
                }
             

                DateTime fecha = DateTime.Now;
                string id_usuario = _userManager.GetUserId(HttpContext.User);
                Bitacora Bitacora = new Bitacora();
                Bitacora.Tabla_Operacion = Tabla_Operacion;
                Bitacora.Proceso = Proceso;
                Bitacora.Fecha_Proceso = DateTime.Now;
                Bitacora.Id_Usuario = id_usuario;
                Bitacora.Id_Transaccion =  id_transaccion.ToString();
                Bitacora.Cliente = persona.NombreCompleto;
                Bitacora.Codigo = persona.Codigo;
                Bitacora.Monto_Afectado = monto;
                _context.Bitacora.Add(Bitacora).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public enum Otro_Cobro
    {
        [Display(Name = "COBRO_ESTRUCTURACION")]
        Deduccion = 1,
        [Display(Name = "PAGO_ACCION")]
        Accion = 2,
        [Display(Name = "PAGO_LIBRETA")]
        Libreta = 3,
        [Display(Name = "FACTURA_SERVICIO")] 
        Servicio = 4,
    }
}