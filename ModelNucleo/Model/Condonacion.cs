using ModelNucleo.Model.ModelView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Condonacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Condonacione { get; set; }

        public int Id_Credito { get; set; }
        public Credito Credito { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        [DefaultValue("---")]
        public string Id_Usuario { get; set; }

        [Display(Name ="Condonar penalización")]
        [DefaultValue(false)]
        public bool Condonar_Penalizacion { get; set; }
        public decimal Valor_Dinero_Penalizacion { get; set; }

        [Display(Name = "Condonar recargos")]
        [DefaultValue(false)]
        public bool Condonar_Recargos { get; set; }
        public decimal Valor_Dinero_Recargos { get; set; }

        [Display(Name = "Permitir gracia")]
        [DefaultValue(false)]
        public bool Permitir_Gracia { get; set; }
        public decimal Valor_Dinero_Gracia { get; set; }
        [DefaultValue("---")]
        public string Lapso_Tiempo { get; set; }

        /*para los créditos vencidos*/
        [Display(Name = "Permitir gracia moratoria")]
        [DefaultValue(false)]
        public bool Permitir_Gracia_Moratoria { get; set; }
        public decimal Valor_Dinero_Gracia_Moratoria { get; set; }
        [DefaultValue("---")]
        public string Lapso_Tiempo_Moratoria { get; set; }
        /*para resultados*/
        [Display(Name = "Pago antes")]
        public decimal Pago_Antes { get; set; }
        [Display(Name = "Condonación total")]
        public decimal Condonacion_Total { get; set; }
        [Display(Name = "Pago después")]
        public decimal Pago_Despues { get; set; }
        
        [NotMapped]
        public ICollection<RecargoCondonacionVista> RecargoCondonacionVista { get; set; }

    }
}
