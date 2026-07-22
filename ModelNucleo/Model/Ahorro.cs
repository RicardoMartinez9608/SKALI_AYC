using ModelNucleo.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Ahorro:ILinea_Ahorro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Ahorro { get; set; }

        public int Id_Linea_Ahorro { get; set; }
        public Linea_Ahorro Linea_Ahorro { get; set; }

        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }

        public string No_Ahorro { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Apertura { get; set; }

        /*campos para el manejo de traslados automáticos*/
        public decimal Saldo_Actual { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Ultima_Fecha_Traslado { get; set; }/*es un depósito automático en concepto de pago de interés*/

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Retiro { get; set; }

        /*por migracion de datos es el historial de la linea si esta se cambia su configuracion*/
        public decimal Tasa_Interes { get; set; }
        public int Mes_Capitalizable { get; set; }
        public int Frecuencia_Deposito { get; set; }
        public int Plazo { get; set; }
        public decimal Monto_Minimo_Ahorro { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CapitalizacionAhorro { get; set; }
        /*termina datos de la linea de ahorro*/

        [NotMapped]
        public ICollection<Transaccion_Ahorro> Transacciones_Ahorros { get; set; }

        [NotMapped]
        public ICollection<Firma_Autorizada_Ahorro> Firmas_Autorizada_Ahorro { get; set; }
        [NotMapped]
        public ICollection<Libreta_Transaccion> Libreta { get; set; }
        [NotMapped]
        public ICollection<Deposito_Plazo> Deposito_Plazo { get; set; }
    }
}
