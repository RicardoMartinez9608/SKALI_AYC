using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class ReferenciaVista:Referencia
    {
        public string Parentesco { get; set; }
        public string Tipo_Referencia { get; set; }
        public int Id_Referencia_Adicional { get; set; }
    }

    public enum Parentesco
    {
        Seleccionar=0,
        [Display(Name ="Cónyuge")]
        Conyuge=1,
        Hijo=2,
        Hija=3,
        Hermano=4,
        Hermana=5,
        Madre=6,
        Padre=7,
        [Display(Name = "Tío")]
        Tio=8,
        [Display(Name = "Tía")]
        Tia=9,
        Abuela=10,
        Abuelo=11,
        Primo=12,
        Prima=13,
        Suegra=14,
        Suegro=15,
        [Display(Name = "Cuñada")]
        Cunada=16,
        [Display(Name = "Cuñado")]
        Cunado=17,
        Yerno=18,
        Yerna=19
    }

    public enum Tipo_Refencia_Option
    {
        Seleccionar=0,
        Familiar=1,
        Amistad=2,
        Laboral=3
    }
}
