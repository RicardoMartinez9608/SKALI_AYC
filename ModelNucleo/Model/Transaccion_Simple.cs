using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Transaccion_Simple : IAuditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Transaccion_Simple { get; set; }

        public int Id_Tipo_Operacion_Simple { get; set; }
        public Tipo_Operacion_Simple Tipo_Operacion_Simple { get; set; }
        public decimal Monto { get; set; }

        [DefaultValue(0)]
        public int Id_Caja { get; set; }/*si fue con una caja la transacción si no se deja en cero*/

        //si hay terceros o responsables hay que escribirlo y si es un cliente registrado más razón
        [DefaultValue(0)]
        public int Id_Persona { get; set; }//por cobros de servicios hechos a los clientes
        [DefaultValue(0)]
        public int Id_Deduccion { get; set; }//para deducciones
        [DefaultValue(0)]
        public int Id_Cobro_Acciones { get; set; }//para cobro de acciones
        [DefaultValue(0)]
        public int Id_Libreta { get; set; }//para cobro de una libreta

        [DefaultValue("--")]
        public string ANombreDe { get; set; }//para persona a ajena

        [DefaultValue(0)]
        public int Id_Caja_Boveda { get; set; }/*es el id de la caja que como boveda hace la transacción*/

        public int Id_Credito { get; set; }/*para rastrear los desembolsos*/

        /*datos del banco para rastrear remesas*/
        [DefaultValue(0)]
        public int Id_Banco { get; set; }/*si la operación es una remesa*/
        public string Nombre_Banco { get; set; }
        public string No_Cuenta { get; set; }

        public string Comprobante_Banco { get; set; }

        /*para facturas de servicios*/
        public decimal Iva_Monto { get; set; }//cuando se trate de un crédito fiscal

        public int Id_Factura_Servicio { get; set; }

        /*termina datos del banco para rastrear remesas*/

        /*falta manejar saldos*/
        public decimal Saldo { get; set; }
        
        public DateTime Creado_En { get; set; }
        public DateTime Eliminado_En { get; set; }
        public DateTime Modificado_En { get; set; }
        public string Creado_Por { get; set; }
        public string Eliminado_Por { get; set; }
        public string Modificado_Por { get; set; }
        [DefaultValue("false")]
        public bool Modificado { get; set; }
        [DefaultValue("false")]
        public bool Borrado_Logico { get; set; }
        public string Detalle_Factura_Servicio { get; set; }
    }
}
