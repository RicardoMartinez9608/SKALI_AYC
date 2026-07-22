using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model.Evaluacion_Tecnica
{
    public class Evaluacion : IAuditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Evaluacion { get; set; }

        public int Id_Persona { get; set; }
        public Persona Persona { get; set; }

        public int? Id_Credito { get; set; }
        public virtual Credito Credito { get; set; }
        
        public string Id_Asesor { get; set; }

        public int No_Dias_Buenos { get; set; }
        public int No_Dias_Malos { get; set; }
        public int No_Dias_Descanso { get; set; }

        /*datos que faltaban*/
        public decimal Venta_Diaria_Bueno { get; set; }
        public decimal Venta_Diaria_Mala { get; set; }

        public decimal Venta_Semanal_Bueno { get; set; }
        public decimal Venta_Semanal_Malo { get; set; }
        public decimal Venta_Mensual_Bueno { get; set; }
        public decimal Venta_Mensual_Malo { get; set; }/*falta migracion*/

        public int Antiguedad_Negocio { get; set; }
        public bool Reside_Zona_Peligrosa { get; set; }
        public bool Referencia_Comprobada { get; set; }
        public decimal Monto_Credito_Mayor_Historial { get; set; }
        public int Mora { get; set; }
        public int Riesgo_Info_Red { get; set; }

        /*propuesto por el asesor*/
        public decimal Monto { get; set; }
        public decimal Cuota { get; set; }
        public int Plazo { get; set; }

        public Sector_Economico Sector_Economico { get; set; }

        public DateTime Creado_En { get; set; }
        public DateTime Eliminado_En { get; set; }
        public DateTime Modificado_En { get; set; }
        public string Creado_Por { get; set; }
        public string Eliminado_Por { get; set; }
        public string Modificado_Por { get; set; }
        [DefaultValue("false")]
        public bool Modificado { get; set; }
        [DefaultValue("false")]
        public bool Borrado_Logico { get; set; }

        [NotMapped]
        public ICollection<Gasto> Gastos { get; set; }
        [NotMapped]
        public ICollection<Otro_Ingreso> Otros_Ingresos { get; set; }
        [NotMapped]
        public ICollection<Prestamo> Prestamos { get; set; }
        [NotMapped]
        public ICollection<Cuenta_Cobrar> Cuentas_Cobrar { get; set; }
        [NotMapped]
        public ICollection<Inventario_Negocio> Inventarios_Negocio { get; set; }
        [NotMapped]
        public ICollection<Costo_Venta> Costos_Ventas { get; set; }
        [NotMapped]
        public ICollection<Activo_Negocio> Activos_Negocio { get; set; }
    }
}
