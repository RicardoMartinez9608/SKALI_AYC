using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelNucleo.CalculosYProceos
{
    //public class Partida_Automatica
    //{
    //    public IEnumerable<Configuracion_Contabilidad> Configuraciones_Contabilidad { get; set; }
    //    public IEnumerable<Identificador_Valor> Conceptos_Valor { get; set; }

    //    public Partida_Automatica(IEnumerable<Configuracion_Contabilidad> configuraciones, IEnumerable<Identificador_Valor> conceptos)
    //    {
    //        Configuraciones_Contabilidad = configuraciones;
    //        Conceptos_Valor = conceptos;
    //    }

    //    public Partida_Automatica()
    //    {

    //    }

    //    public Partida_Contable Ejecutar_Partida(string nuevo_codigo)
    //    {
    //        return new Partida_Contable {
    //            Comprobante = nuevo_codigo,
    //            Saldo = 0.00M,
    //            Fecha=DateTime.Now,
    //            Creado_En=DateTime.Now,
    //            Creado_Por="Nio"
    //        };
    //    }

    //    /// <summary>
    //    /// creo los detalles de la partida
    //    /// </summary>
    //    public List<Detalle_Partida> Ejecutar_Detalles(Partida_Contable pc)
    //    {
    //        List<Detalle_Partida> detalles = new List<Detalle_Partida>();
    //        foreach (Identificador_Valor cv in Conceptos_Valor)
    //        {
    //            bool existe = Configuraciones_Contabilidad.Any(c=>c.Identificador==cv.Identificador);
    //            if (existe)
    //            {
    //                Configuracion_Contabilidad cc= Configuraciones_Contabilidad.FirstOrDefault(c => c.Identificador == cv.Identificador);
    //                if (!detalles.Any(p => p.Id_Cuenta_Contable == cc.Id_Cuenta))/*si aun no está lo guardo*/
    //                {
    //                    detalles.Add(new Detalle_Partida {
    //                        Id_Cuenta_Contable = cc.Id_Cuenta,
    //                        Id_Partida_Contable = pc.Id_Partida_Contable,
    //                        Concepto = cc.Concepto,
    //                        Cargo = cc.Modo_Almacen_Valor == "C" ? cv.Valor : 0,
    //                        Abono = cc.Modo_Almacen_Valor == "A" ? cv.Valor : 0,
    //                        Saldo=0,
    //                        Fecha=DateTime.Now,
    //                        Creado_En=DateTime.Now,
    //                        Creado_Por="Nio",
    //                    });
    //                }
    //            }
    //        }
    //        return detalles;
    //    }

    //}
}
