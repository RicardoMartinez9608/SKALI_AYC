using ModelNucleo.Model.Cuestionario_Socio;
using ModelNucleo.Model.Evaluacion_Referencia;
using ModelNucleo.Model.Evaluacion_Tecnica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Persona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Persona { get; set; }

        public int Id_Cartera { get; set; }
        public Cartera Cartera { get; set; }

        public string Codigo { get; set; }

        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [NotMapped]
        public string NombreCompleto { get { return string.Format("{0} {1}", Nombres, Apellidos); } }
        public string Telefono { get; set; }
        public string Nit { get; set; }
        
        [Required]
        public string Dui { get; set; }
        public string Pasaporte { get; set; }

        [DefaultValue("false")]
        public bool Pone_Huella { get; set; }
        

        public int? Id_Departamento_Pais_Domicilio { get; set; }
        public int? Id_Municipio_Departamento_Domicilio { get; set; }
        public string Domicilio { get; set; }
        /*datos para openstreetmaps*/
        public string Latitud_Domicilio { get; set; }
        public string Longitud_Domicilio { get; set; }
        #region actividad_economica
        #region paraEmpleado
        [DefaultValue("false")]
        public bool Es_Empleado { get; set; }

        public int? Id_Departamento_Pais_Direccion_Trabajo { get; set; }
        public int? Id_Municipio_Departamento_Direccion_Trabajo { get; set; }
        public string Direccion_Trabajo { get; set; }
        public string Lugar_Trabajo { get; set; }
        public decimal Salario_Mensual { get; set; }
        /*datos para openstreetmaps*/
        public string Latitud_Direccion_Trabajo { get; set; }
        public string Longitud_Direccion_Trabajo { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //[Required(ErrorMessage = "El teléfono de trabajo es obligatorio")]
        public string Telefono_Trabajo { get; set; }
        public string Cargo_Trabajo { get; set; }
        public string Nombre_Jefe_Inmediato { get; set; }


        #endregion
        #region paraComerciante
        [DefaultValue("false")]
        public bool Es_Comerciante { get; set; }
        
        public int? Id_Departamento_Pais_Direccion_Negocio { get; set; }
        public int? Id_Municipio_Departamento_Direccion_Negocio { get; set; }
        public string Direccion_Negocio { get; set; }
        /*datos para openstreetmaps*/
        public string Latitud_Direccion_Negocio { get; set; }
        public string Longitud_Direccion_Negocio { get; set; }

        public string Giro_Comercial { get; set; }
        
        #endregion

        public decimal Ingresos_Mensuales { get; set; }

        #endregion



        public int? Id_Departamento_Pais_Nacimiento { get; set; }
        public int? Id_Municipio_Departamento_Nacimiento { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }
        /*fecha_nacimiento con su departamento y municipio*/

        public string ExpedicionDocumentoIdentifcacion { get; set; }
        public string ExpiracionDocumentoIdentificacion { get; set; }
        
        /*pueden ser nullos*/
        public string ISSS { get; set; }
        public string NUP { get; set; }
        /*terminan los nullables*/

        public Estado_Familiar EstadoFamiliar { get; set; }/*deberia ser combo*/
        public Sexo Sexo { get; set; }/*debería ser combo*/
        public string Ocupacion { get; set; }
        public string Profesion { get; set; }

        [DefaultValue("Salvadoreño")]
        public string Nacionalidad { get; set; }
        public string ConocidoPor { get; set; }

        /*datos como usuario*/
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Correo { get; set; }

        /*datos necesarios para scoring inherentes a la persona*/
        public int Personas_Cargo { get; set; }
        public bool Referencia_Crediticia_Comprobable { get; set; }
        public Residencia Residencia { get; set; }/*debe ser combobox Propio, Familiar y Alquilada*/
        
        private int _edad;
        public int Edad
        {
            get {
                DateTime? f_n = FechaNacimiento;
                if (FechaNacimiento != null)
                    _edad = DateTime.Now.Year - ((DateTime)FechaNacimiento).Year;
                else
                    _edad = 0;
                return _edad;
            }
            set { _edad = value; }
        }

        //[DefaultValue(true)]
        //public bool Activo { get; set; }

        /*incluir los PEPS*/
        public bool Tiene_Cargo { get; set; }
        public bool Familiar_Cargo { get; set; }
        public bool Asociado_Cargo { get; set; }

        public string Comentario { get; set; }

        /*verificar referencia*/
        [DefaultValue(0.00)]
        public decimal Ponderacion { get; set; }

        [DefaultValue(0)]
        public Estado_Socio Estado_Socio { get; set; }///cambiar para manejar correctamente el estado del socio

        [NotMapped]
        public ICollection<Persona_Tipo_Persona> Persona_Tipo_Persona { get; set; }
        [NotMapped]
        public ICollection<Persona_Relaciones> Persona_Relaciones { get; set; }
        [NotMapped]
        public ICollection<Persona_Referencia> Persona_Referencias { get; set; }
        [NotMapped]
        public ICollection<Credito> Creditos { get; set; }
        [NotMapped]
        public ICollection<Ahorro> Ahorros { get; set; }
        [NotMapped]
        public ICollection<Documento> Documentos { get; set; }
        [NotMapped]
        public ICollection<Evaluacion> Evaluaciones { get; set; }
        [NotMapped]
        public ICollection<Referencia_Bancaria> Referencia_Bancaria { get; set; }

        [NotMapped]
        public ICollection<Accion> Acciones { get; set; }

        [NotMapped]
        public ICollection<Evaluacion_Referencia_Persona> Evaluaciones_Referencias { get; set; }

        [NotMapped]
        public ICollection<Catalogo_Socio> Catalogos_Socio { get; set; }

        [NotMapped]
        public ICollection<Calificacion_Crediticia> Calificacion_Crediticias { get; set; }
        [NotMapped]
        public ICollection<Deposito_Plazo> Deposito_Plazo { get; set; }
    }

    /*Enums para Persona mejoras*/
    public enum Sexo {
        Indefinido=0,
        Masculino=1,
        Femenino=2
    }

    public enum Estado_Familiar {
        Seleccionar=0,
        Soltero=1,
        Casado=2,
        [Display(Name ="Unión Libre")]
        Union_Libre=3,
        Viudo=4,
        Divorciado=5
    }

    public enum Residencia {
        Seleccionar=0,
        Propio=1,
        Familiar=2,
        Alquilada=3
    }

    public enum Sector_Economico {
        Comercio,
        Servicios,
        Consumo,
        Industria
    }

    public enum Estado_Socio {
        [Display(Name ="En suscripción")]
        En_Suscripcion = 0,
        Negado = 1,
        Inactivo = 2,
        Activo = 3
    }
}
