using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Caja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Caja { get; set; }
        public string Id_Usuario { get; set; }

        public int Id_Agencia { get; set; }
        public Agencia Agencia { get; set; }

        public string Nombre_PC{ get; set; }

        [DefaultValue(true)]
        public bool Activa { get; set; }

        [DefaultValue(false)]
        public bool Boveda { get; set; }

        public string Cuenta_Contable { get; set; }//es para abonar o cargar de acuerdo a la operación

        [NotMapped]
        public ICollection<Transaccion_Credito> Transaccion_Creditos { get; set; }

        [NotMapped]
        public ICollection<Transaccion_Ahorro> Transaccion_Ahorros { get; set; }

        [NotMapped]
        public ICollection<Transaccion_Caja> Transaccion_Cajas { get; set; }
        [NotMapped]
        public ICollection<Efectivo> Efectivos { get; set; }

        [NotMapped]
        public ICollection<SaldoDisponible> SaldoDisponibles { get; set; }

    }
}
