using ModelNucleo.Model.Interfaces;
using ModelNucleo.Model.ModelView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Credito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Credito { get; set; }
        public string Codigo_Credito { get; set; }
        /*para la relacion con la persona que tomo el credito*/
        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }

        public int? Id_Acta { get; set; }
        public Acta Acta { get; set; }

        public int Id_Linea { get; set; }
        public Linea_Credito Linea_Credito { get; set; }
        
        public int Id_Cartera { get; set; }
        public Cartera Cartera { get; set; }

        //para ver el comportamiento del crédito
        public int Id_Comportamiento_Credito { get; set; }
        public Comportamiento_Credito Comportamiento_Credito { get; set; }

        public bool En_Comite { get; set; }
        [DefaultValue(false)]
        public bool Necesita_Comite_Ampliado { get; set; }

        public bool Aprobado { get; set; }
        [DefaultValue(false)]
        public bool Revertido { get; set; }
        public bool Liquidado { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Aprobado_En { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Liquidado_En { get; set; }/*La liquidación es diferente a la aprobación*/

        public string Comentario { get; set; }

        public decimal Interes_Comision { get; set; }
        public decimal Monto_pago { get; set; }
        public int Plazo { get; set; }/*saber que esta expresado en meses*/
        public int Numero_cuotas { get; set; }

        public int Id_Periodo { get; set; }/*preguntar por su uso*/
        public Periodo Periodo { get; set; }

        public decimal Valor_Cuota { get; set; }
        public decimal Interes { get; set; }
        public decimal Valor_IVA { get; set; }
        public decimal Cuota_Total { get; set; }

        [DefaultValue(false)]
        public bool Cancelado { get; set; }

        [DefaultValue(false)]
        public bool Congelado { get; set; }
        [DefaultValue(false)]
        public bool Vencido { get; set; }
        
        public DateTime Cancelado_En { get; set; }
        public DateTime Congelado_En { get; set; }
        public DateTime Vencido_En { get; set; }

        public Segmentacion_Moral segmentacion_Moral { get; set; }
        
        public int? Id_Linea_Credito_Historial { get; set; }
        public virtual Linea_Credito_Historial Linea_Credito_Historial { get; set; }

        [DefaultValue(0)]
        public int? Id_Manejo_Refinanciamiento { get; set; }
        public virtual Manejo_Refinanciamiento Manejo_Refinanciamiento { get; set; }

        public ICollection<Amortizacion> Amortizaciones { get; set; }

        [NotMapped]
        public ICollection<Comite> Comites { get; set; }
        [NotMapped]
        public ICollection<Transaccion_Credito> Transacciones_Creditos { get; set; }
        public ICollection<Detalle_Credito_Grupal> Detalle_Credito_Grupal { get; set; }
        public ICollection<Condonacion> Condonaciones { get; set; }
        //[NotMapped]
        //public ICollection<Persona_Relaciones> Codeudores { get; set; }
        [NotMapped]
        public virtual Evaluacion_Tecnica.Evaluacion Evaluacion { get; set; }
        [NotMapped]
        public ICollection<Comite_Ampliado> Comite_Ampliados { get; set; }
        [NotMapped]
        public ICollection<Calificacion_Crediticia> Calificacion_Crediticias { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_ProntoPago { get; set; } //fecha unicamente para prontopago
        public bool Credito_Con_Ahorro { get; set; }
        [NotMapped]
        public ICollection<Credito_Con_Ahorro> Credito_Con_Ahorros { get; set; }
        [NotMapped]
        public decimal Primer_Pago  { get; set; }
        [NotMapped]
        public DateTime Fecha_Primer_Pago { get; set; }
        [NotMapped]
        public decimal Cuota_Menos_Plan { get; set; }
        [NotMapped]
        public decimal Plan_AyC { get; set; }
        //para dos fechas en mutuos
        [NotMapped]
        public decimal Segundo_Pago{ get; set; }
        [NotMapped]
        public DateTime Fecha_Segundo_Pago { get; set; }
        [NotMapped]
        public bool mutuo_normal { get; set; }
        [NotMapped]
        public bool mutuo_dos_fechas { get; set; }
        [NotMapped]
        public bool mutuo_dos_codeudores { get; set; }
        [NotMapped]
        public bool mutuo_dos_codeudores_dos_fechas { get; set; }
        [NotMapped]
        public decimal Segunda_Cuota_Menos_Plan { get; set; }
        [NotMapped]
        public string Nombre_Ciudad_Mutuo { get; set; }
        [NotMapped]
        public decimal Monto_Credito_Ahorro { get; set; }
        [NotMapped]
        public int Id_Porcentaje_Ahorro { get; set; }
    }

    public enum Segmentacion_Moral {
        [Display(Name = "Sin Segmentar")]
        NDA =0,
        [Display(Name = "Quiere y Sí Puede")]
        QSP =1,
        [Display(Name = "Quiere y No Puede")]
        QNP =2,
        [Display(Name = "No Quiere y Sí Puede")]
        NSP =3,
        [Display(Name = "No Quiere y No Puede")]
        NNP =4
    }; 
}
