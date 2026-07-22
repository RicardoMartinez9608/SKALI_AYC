using ModelNucleo.Model.Evaluacion_Referencia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model
{
    public class Referencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Referencia { get; set; }
        public string Nombre { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Residencia { get; set; }

        public string Explicacion_Residencia { get; set; }

        public string Latitud_Residencia { get; set; }
        public string Longitud_Residencia { get; set; }

        /*para mostrar fecha y porcentaje*/
        private int _edad;
        public int Edad
        {
            get
            {
                DateTime? f_n = FechaNacimiento;
                if (FechaNacimiento != null)
                    _edad = DateTime.Now.Year - ((DateTime)FechaNacimiento).Year;
                else
                    _edad = 0;
                return _edad;
            }
            set { _edad = value; }
        }
        public decimal Porcentaje { get; set; }
        public bool Beneficiario { get; set; }
        
        public Valoracion_Referencia Valoracion_Referencia { get; set; }
        public decimal Ponderacion_Verficada { get; set; }//porcentaje que se arroja la consulta telefonica

        /*tentativo por si es cónyuge o hijo*/
        public string Direccion_Empleo { get; set; }
        public string Cargo_Empleo { get; set; }
        public decimal Salario_Empleo { get; set; }
        public string Nombre_Jefe_Inmediato { get; set; }
        public string Telefono_Empleo { get; set; }

        [DefaultValue(false)]
        public bool Esta_Estudiando { get; set; }
        [DefaultValue(false)]
        public bool Esta_Trabajando { get; set; }

        public string Direccion_Estudio { get; set; }


        [NotMapped]
        public ICollection<Persona_Referencia> Persona_Referencias { get; set; }

        [NotMapped]
        public ICollection<Evaluacion_Referencia_Persona> Evaluaciones_Referencias { get; set; }

        [NotMapped]
        public ICollection<Referencia_Adicional> Referencias_Adicionales { get; set; }

        [NotMapped]
        public ICollection<Llamada_Telefonica_Referencia> Llamadas_Telefonicas_Referencia { get; set; }
    }


    public enum Valoracion_Referencia
    {
        Indefinido=0,
        [Display(Name ="No contesta")]
        No_Contesta=1,
        [Display(Name = "Número equivocado")]
        Numero_Equivocado =2,
        [Display(Name = "Mala referencia")]
        Mala_Referencia =3,
        [Display(Name = "Buena referencia")]
        Buena_Referencia =4
    }
}
