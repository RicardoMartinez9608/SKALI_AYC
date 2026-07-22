using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ModelNucleo.Model.Interfaces;

namespace ModelNucleo.Model
{
    public class Linea_Credito_Historial : ILinea_Credito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Linea_Credito_Historial { get; set; }

        public int? Id_Credito { get; set; }
        //       public Credito Credito { get; set; }

        public string Codigo { get; set; }
        public string Descripcion_linea { get; set; }
        public int Tipo_Garantia { get; set; }
        public decimal Interes_Comision { get; set; }
        public decimal Tasa_Mora { get; set; }
        public decimal Recargo { get; set; }
        public int Dia_Gracia { get; set; }
        public decimal Cancelacion_anticipada { get; set; }
        public bool PlanAyC_Cobros_Periodico_Dinero { get; set; }
        public bool Activar_Deducciones { get; set; }
        public decimal Interes { get; set; }
        public int Plazo_Rango_Inicial { get; set; }
        public int Plazo_Rango_Final { get; set; }
        //public decimal Estructuracion { get; set; }
        public decimal Monto_Rango_Inicial { get; set; }
        public decimal Monto_Rango_Final { get; set; }
        public int Destino_Credito { get; set; }

        [NotMapped]
        public ICollection<Cobro_Periodico_Historial> Cobros_Periodicos_Historial { get; set; }

        [NotMapped]
        public ICollection<Deduccion_Historial> Deducciones_Historial { get; set; }
        [NotMapped]
        public virtual Credito Credito { get; set; }
    }
}