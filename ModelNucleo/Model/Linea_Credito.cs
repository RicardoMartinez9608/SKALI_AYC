using ModelNucleo.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Linea_Credito : ILinea_Credito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Linea { get; set; }
        public string Codigo { get; set; }
        public string Descripcion_linea { get; set; }
        public int Tipo_Garantia { get; set; }
        public TipoGarantia TipoGarantia { get; set; }

        public decimal Interes_Comision { get; set; }
        public decimal Tasa_Mora { get; set; }
        public decimal Recargo { get; set; }
        public int Dia_Gracia { get; set; }
        public decimal Cancelacion_anticipada { get; set; }

        /*agregado para manejar como dinero los cargos*/
        [DefaultValue(false)]
        public bool PlanAyC_Cobros_Periodico_Dinero { get; set; }

        [DefaultValue(false)]
        public bool Activar_Deducciones { get; set; }

        public decimal Interes { get; set; }/*el % para el dato, no el calculado*/

        /*nuevos campos que hacen falta de acuerdo a reuniones*/
        /*rango de los plazos que se permitirán*/
        public int Plazo_Rango_Inicial { get; set; }
        public int Plazo_Rango_Final { get; set; }

        //public decimal Estructuracion { get; set; }/*es un cobro al momento de la liquidación*/

        /*rango de los montos permitidos para esta línea de creditos*/
        public decimal Monto_Rango_Inicial { get; set; }
        public decimal Monto_Rango_Final { get; set; }

        public int Destino_Credito { get; set; }
        public TipoDestinoCredito TipoDestinoCredito { get; set; }

        //campo para verificar si una cuota sera movible despues del 25 de cada mes
        [DefaultValue(false)]
        public bool Movible { get; set; }
        [DefaultValue(false)]
        public bool Calculo_Interes { get; set; }
        [NotMapped]
        public ICollection<Credito> Creditos { get; set; }
        [NotMapped]
        public ICollection<Cobro_Periodico> Cobros_Credito { get; set; }
        [NotMapped]
        public ICollection<Deduccion_Desembolso> Deducciones_Desembolso { get; set; }

        [NotMapped]
        public ICollection<Interes_Periodo_Linea_Credito> InteresPeriodoLineaCreditos { get; set; }

        [NotMapped]
        public virtual ICollection<Tipo_Mutuo> Tipo_Mutuos { get; set; }
       

    }
}