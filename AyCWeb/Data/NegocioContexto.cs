using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using ModelNucleo.Model.Contabilidad;

namespace NucleoAyC.Entities
{
    public class NegocioContexto : DbContext
    {
        public NegocioContexto (DbContextOptions<NegocioContexto> options)
            : base(options)
        {
        }

        public DbSet<ModelNucleo.Model.Tipo_Relacion> Tipo_Relacion { get; set; }

        public DbSet<ModelNucleo.Model.Tipo_Persona> Tipo_Persona { get; set; }

        public DbSet<ModelNucleo.Model.Persona_Tipo_Persona> Persona_Tipo_Persona { get; set; }

        public DbSet<ModelNucleo.Model.Persona_Relaciones> Persona_Relaciones { get; set; }

        public DbSet<ModelNucleo.Model.Referencia> Referencia { get; set; }

        public DbSet<ModelNucleo.Model.Persona_Referencia> Persona_Referencia { get; set; }

        public DbSet<ModelNucleo.Model.ModelView.ReferenciaVista> ReferenciaVista { get; set; }

        public DbSet<ModelNucleo.Model.Credito> Credito { get; set; }
        

        public DbSet<ModelNucleo.Model.Linea_Credito> Linea_Credito { get; set; }

        public DbSet<ModelNucleo.Model.Agencia> Agencia { get; set; }

        public DbSet<ModelNucleo.Model.Comite> Comite { get; set; }

        public DbSet<ModelNucleo.Model.Configuracion_Comite_Rol> Configuracion_Comite_Rol { get; set; }

        public DbSet<ModelNucleo.Model.Persona> Persona { get; set; }

        public DbSet<ModelNucleo.Model.Cartera> Cartera { get; set; }

        public DbSet<ModelNucleo.Model.Acta> Acta { get; set; }

        public DbSet<ModelNucleo.Model.Linea_Ahorro> Linea_Ahorro { get; set; }

        public DbSet<ModelNucleo.Model.Ahorro> Ahorro { get; set; }

        public DbSet<ModelNucleo.Model.Transaccion_Ahorro> Transaccion_Ahorro { get; set; }

        public DbSet<ModelNucleo.Model.Documento> Documento { get; set; }

        public DbSet<ModelNucleo.Model.Transaccion_Credito> Transaccion_Credito { get; set; }

        //public DbSet<ModelNucleo.Model.Contabilidad.Detalle_Partida> Detalle_Partida { get; set; }

        //public DbSet<ModelNucleo.Model.Contabilidad.Partida_Contable> Partida_Contable { get; set; }

        //public DbSet<ModelNucleo.Model.Contabilidad.Tipo> Tipo { get; set; }

        //public DbSet<ModelNucleo.Model.Configuracion_Contabilidad> Configuracion_Contabilidad { get; set; }
    }
}
