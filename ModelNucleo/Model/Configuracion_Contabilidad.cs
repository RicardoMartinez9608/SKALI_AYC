using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Configuracion_Contabilidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Configuracion_Contabilidad { get; set; }

        public int Id_Operacion { get; set; }
        public Operacion Operacion { get; set; }

        public int Id_Definicion_Transaccion { get; set; }
        public Definicion_Transaccion Definicion_Transaccion { get; set; }

        //saber de que agencia es
        public int Id_Agencia { get; set; }
        public Agencia Agencia { get; set; }
        
        public int Id_Cuenta { get; set; }
        public string Concepto { get; set; }

        /*campos que faltan*/
        public string Identificador { get; set; }/*es el que se ocupara para identificar en el código*/
        public string Descripcion { get; set; }/*describa dónde necesitas y para qué el Identificador en el código*/
        public string Modo_Almacen_Valor { get; set; }/*solo será C:cargo y A:Abono*/

        //public decimal Valor { get; set; }/*probablemente no se use*/
    }
}
