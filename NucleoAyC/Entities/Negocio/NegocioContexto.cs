using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelNucleo.Model;
using ModelNucleo.Model.Contabilidad;
using ModelNucleo.Model.Cuestionario_Socio;
using ModelNucleo.Model.Evaluacion_Referencia;
using ModelNucleo.Model.Evaluacion_Tecnica;
using System.IO;
using ModelNucleo.Model.ModelView;
using System;
using ModelNucleo.Model.Tablero;

namespace NucleoAyC.Entities
{
    public class NegocioContexto : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConnectionString());
        }

        private static string GetConnectionString()
        {
            var file = Path.GetFullPath("appsettings.json");
            var config = new ConfigurationBuilder().AddJsonFile(file, optional: false).Build();
            ConnectionParams cp = new ConnectionParams();
            config.GetSection("databasenucleo").Bind(cp);

            return $"Server={cp.databaseServer};" +
                   $"database={cp.databaseName};" +
                   $"uid={cp.databaseUser};" +
                   $"pwd={cp.databasePass};" +
                   $"pooling=true; Convert Zero Datetime=True;default command timeout=12000";
        }
        public DbSet<Configuracion_Comite_Rol> Configuracion_Comite_Rol { get; set; }

        public DbSet<Departamento_Pais> Departamento_Pais { get; set; }
        public DbSet<Municipio_Departamento> Municipio_Departamento { get; set; }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Persona_Tipo_Persona> Persona_Tipo_Persona { get; set; }
        public DbSet<Tipo_Persona> Tipo_Persona { get; set; }
        public DbSet<Referencia_Bancaria> Referencia_Bancaria { get; set; }

        public DbSet<Tipo_Relacion> Tipo_Relacion { get; set; }
        public DbSet<Persona_Relaciones> Persona_Relaciones { get; set; }

        public DbSet<Referencia> Referencia { get; set; }
        public DbSet<Persona_Referencia> Persona_Referencia { get; set; }

        public DbSet<Referencia_Adicional> Referencia_Adicional { get; set; }

        /*para los creditos de las personas*/
        public DbSet<Linea_Credito> Linea_Credito { get; set; }

        public DbSet<Tipo_Mutuo> Tipo_Mutuo { get; set; }

        public DbSet<Cobro_Periodico> Cobro_Periodico { get; set; }
        public DbSet<Credito> Credito { get; set; }

        public DbSet<Condonacion> Condonacion { get; set; }

        public DbSet<Amortizacion> Amortizacion { get; set; }
        
        public DbSet<Periodo> Periodo { get; set; }

        public DbSet<Comite> Comite { get; set; }
        public DbSet<Deduccion_Desembolso> Deduccion_Desembolso { get; set; }

        /*manejo de la linea de credito en el credito y sus listas aderidas al credito como un historial*/
        public DbSet<Linea_Credito_Historial> Linea_Credito_Historial { get; set; }
        public DbSet<Cobro_Periodico_Historial> Cobro_Periodico_Historial { get; set; }
        public DbSet<Deduccion_Historial> Deduccion_Historial { get; set; }

        /*para el manejo de carteras y agencias*/
        public DbSet<Agencia> Agencia { get; set; }
        public DbSet<Usuario_Agencia> Usuario_Agencia { get; set; }
        public DbSet<Cartera> Cartera { get; set; }

        public DbSet<Configuracion_General> Configuracion_General { get; set; }

        public DbSet<Acta> Acta { get; set; }

        /*entidades completas para que funcione caja*/
        public DbSet<Tipo_Linea_Ahorro> Tipo_Linea_Ahorro { get; set; }
        public DbSet<Linea_Ahorro> Linea_Ahorro { get; set; }
        public DbSet<Ahorro> Ahorro { get; set; }
        public DbSet<Tipo_Operacion_Ahorro> Tipo_Operacion_Ahorro { get; set; }
        public DbSet<Transaccion_Ahorro> Transaccion_Ahorro { get; set; }
        public DbSet<Caja> Caja { get; set; }
        public DbSet<Transaccion_Caja> Transaccion_Caja { get; set; }
        public DbSet<Tipo_Transaccion> Tipo_Transaccion { get; set; }
        public DbSet<Transaccion_Credito> Transaccion_Credito { get; set; }
        public DbSet<Tipo_Operacion_Credito> Tipo_Operacion_Credito { get; set; }
        public DbSet<Transaccion_Simple> Transaccion_Simple { get; set; }
        public DbSet<Tipo_Operacion_Simple> Tipo_Operacion_Simple { get; set; }
        public DbSet<Banco> Banco { get; set; }
        public DbSet<Factura_Servicio> Factura_Servicios { get; set; }
        public DbSet<Otras_Operaciones> Otras_Operaciones { get; set; }
        public DbSet<Manejo_Refinanciamiento> Manejo_Refinanciamiento { get; set; }
        /*documentacion*/
        public DbSet<Documento> Documento { get; set; }
        public DbSet<Descripcion_Documento> Descripcion_Documento { get; set; }
        /*Flujo Efectivo*/
        public DbSet<Denominacion_efectivo> Denominacion_Efectivo { get; set; }
        public DbSet<Efectivo> Efectivo { get; set; }
        public DbSet<SaldoDisponible> SaldoDisponible { get; set; }
        // Interes linea de credito
        public DbSet<Interes_Periodo_Linea_Credito> InteresPeriodoLineaCreditos { get; set; }

        
        public DbSet<Conf_Partida_Contable> Conf_Partida_Contable { get; set; }
        public DbSet<Conf_Detalle_Partida_Contable> Conf_Detalle_Partida_Contable { get; set; }
        public DbSet<Asiento_Contable> Asiento_Contable { get; set; }
        public DbSet<Detalle_Asiento_Contable> Detalle_Asiento_Contable { get; set; }

        public DbSet<Operacion> Operacion { get; set; }
        public DbSet<Definicion_Transaccion> Definicion_Transaccion { get; set; }
        /*termina la entidades de contabilidad*/

        /*Entidades de la evaluacion tecnica*/
        public DbSet<Evaluacion> Evaluacion { get; set; }
        public DbSet<Activo_Negocio> Activo_Negocio { get; set; }
        public DbSet<Costo_Venta> Costo_Venta { get; set; }
        public DbSet<Cuenta_Cobrar> Cuenta_Cobrar { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Inventario_Negocio> Inventario_Negocio { get; set; }
        public DbSet<Otro_Ingreso> Otro_Ingreso { get; set; }
        public DbSet<Prestamo> Prestamo { get; set; }
        public DbSet<Tipo_Gasto> Tipo_Gasto { get; set; }
        public DbSet<Tipo_Ingreso> Tipo_Ingreso { get; set; }
        public DbSet<Categoria_Gasto> Categoria_Gasto { get; set; }
        /*Entidades de la evaluacion tecnica*/

        public DbSet<Colecta_Diaria> Colecta_Diaria { get; set; }
        public DbSet<Colecta> Colecta { get; set; }
        public DbSet<Accion> Accion { get; set; }
        public DbSet<Tipo_Accion> Tipo_Accion { get; set; }

        /*para la evaluacion de las referencias*/
        public DbSet<Evaluacion_Referencia_Catalogo> Evaluacion_Referencia_Catalogo { get; set; }
        public DbSet<Evaluacion_Referencia_Persona> Evaluacion_Referencia_Persona { get; set; }
        public DbSet<Pregunta> Pregunta { get; set; }
        public DbSet<Respuesta> Respuesta { get; set; }

        /*catalogo de preguntas directas para personsa socia*/
        public DbSet<Catalogo_Cuestionario> Catalogo_Cuestionario { get; set; }
        public DbSet<Pregunta_Catalogo> Pregunta_Catalogo { get; set; }
        public DbSet<Respuesta_Catalogo> Respuesta_Catalogo { get; set; }
        public DbSet<Catalogo_Socio> Catalogo_Socio { get; set; }
        
        //se manejan las llamadas a las referencias
        public DbSet<Llamada_Telefonica_Referencia> Llamada_Telefonica_Referencia { get; set; }

        //para las firmas autorizadas
        public DbSet<Firma_Autorizada_Ahorro> Firma_Autorizada_Ahorro { get; set; }

        //comportamiento de un crédito
        public DbSet<Comportamiento_Credito> Comportamiento_Credito { get; set; }
        //menu 
        public DbSet<Menu_Contenido_Vista> MenuContenidoVista { get; set; }
        public DbSet<Menu_Categoria_vista> MenuCategoriaVista { get; set; }
        public DbSet<Menu_Rol_Contenido> MenuRolContenido { get; set; }
        public DbSet<Menu_Rol_Categoria> MenuRolCategoria { get; set; }
        // libreta transacciones
        public DbSet<Libreta_Transaccion> LibretaTransacciones { get; set; }
        public DbSet<Documentos_Legales> DocumentosLegales { get; set; }

        /*tablas islas*/
        public DbSet<Ingreso_Cheque> Ingreso_Cheque { get; set; }

        /*Configuracion Buro*/
        public DbSet<Configuracion_Calificacion> Configuracion_Calificacion { get; set; }
        public DbSet<Buro> Buros { get; set; }
        public DbSet<Calificacion_Crediticia> Calificacion_Crediticias { get; set; }

        /*manejo de accesos directos*/
        public DbSet<Categoria_Acceso_Directo> Categoria_Acceso_Directo { get; set; }
        public DbSet<Acceso_Directo> Acceso_Directo { get; set; }

        /*islote para una locura de código único aislado de las llaves*/
        public DbSet<Codigo_Agencia> Codigo_Agencia { get; set; }
        /*Manejo creditos grupales*/
        public DbSet<Credito_Grupal> Credito_Grupal { get; set; }
        public DbSet<Detalle_Credito_Grupal> Detalle_Credito_Grupal { get; set; }

        /*manejo del tablero*/
        public DbSet<Rancking> Rancking { get; set; }
        public DbSet<Meta> Meta { get; set; }
        public DbSet<Cosecha> Cosecha { get; set; }
        public DbSet<Reporte_Mora> Reporte_Mora { get; set; }
        public DbSet<Bitacora> Bitacora { get; set; }
        public DbSet<Deposito_Plazo> Deposito_Plazo { get; set; }
        public DbSet<Autorizado> Autorizado { get; set; }
        public DbSet<Parentesco_Persona> Parentesco { get; set; }
        public DbSet<Beneficiarios_Deposito_Plazo> Beneficiarios_Deposito_Plazo { get; set; }
        public DbSet<Forma_Pago_DP> Forma_Pago { get; set; }
        public DbSet<Transaccion_Deposito_Plazo> Transaccion_Deposito_Plazo { get; set; }
        public DbSet<Linea_Deposito_Plazo> Linea_Deposito_Plazo { get; set; }
        public DbSet<Credito_Con_Ahorro> Credito_Con_Ahorros { get; set; }
        public DbSet<Porcentaje_Ahorro_Credito> Porcentaje_Ahorro_Creditos { get; set; }
        public DbSet<Fotografia_Comprobante> Fotografia_Comprobantes { get; set; }
        public DbSet<Fotografia_Atm> Fotografia_Atms { get; set; }
        public DbSet<Comite_Ampliado> Comite_Ampliados { get; set; }
        public DbSet<Fotografia_Comite> Fotografia_Comites { get; set; }
        public DbSet<CreditoComiteAmpliado> CreditoComiteAmpliados { get; set; }
        public DbSet<TipoGarantia> TipoGarantias { get; set; }
        public DbSet<TipoDestinoCredito> TipoDestinoCreditos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>().ToTable("Persona");
            modelBuilder.Entity<Persona_Tipo_Persona>().ToTable("Persona_Tipo_Persona");
            modelBuilder.Entity<Tipo_Persona>().ToTable("Tipo_Persona");

            modelBuilder.Entity<Tipo_Relacion>().ToTable("Tipo_Relacion");
            modelBuilder.Entity<Persona_Relaciones>().ToTable("Persona_Relaciones");
            //modelBuilder.Entity<Persona_Predicado_Relacion>().ToTable("Persona_Predicado_Relacion");
            
            modelBuilder.Entity<Municipio_Departamento>()
                .HasOne(h => h.Departamento_Pais)
                .WithMany(w => w.Municipios_Departamento)
                .HasForeignKey(k => k.Id_Departamento_Pais);

            /*realcionando tablas*/
            modelBuilder.Entity<Persona_Tipo_Persona>()
            .HasKey(bc => new { bc.Id_Persona, bc.Id_Tipo_Persona });

                modelBuilder.Entity<Persona_Tipo_Persona>()
                    .HasOne(bc => bc.Persona)
                    .WithMany(b => b.Persona_Tipo_Persona)
                    .HasForeignKey(bc => bc.Id_Persona);

                modelBuilder.Entity<Persona_Tipo_Persona>()
                    .HasOne(bc => bc.Tipo_Persona)
                    .WithMany(c => c.Persona_Tipo_Persona)
                    .HasForeignKey(bc => bc.Id_Tipo_Persona);

            /*Relaciones de tablas para denotar las referencias*/
            /*modelBuilder.Entity<Persona_Relaciones>()
                .HasKey(dc => new { dc.Id_Sujeto, dc.Id_Predicado });*/

            modelBuilder.Entity<Persona_Relaciones>()
                .HasOne(a=>a.Sujeto)
                .WithMany()
                .HasForeignKey(c => c.Id_Sujeto);

            modelBuilder.Entity<Persona_Relaciones>()
                .HasOne(dc => dc.Predicado)
                .WithMany()
                .HasForeignKey(c => c.Id_Predicado);

            modelBuilder.Entity<Persona_Relaciones>()
                .HasOne(dc => dc.Tipo_Relacion)
                .WithMany(d => d.Persona_Relaciones)
                .HasForeignKey(c => c.Id_Tipo_Relacion);
            
            modelBuilder.Entity<Referencia_Bancaria>()
                .HasOne(f=>f.Persona)
                .WithMany(u=>u.Referencia_Bancaria)
                .HasForeignKey(h=>h.Id_Persona);

            /*relaciones para las referencias*/
            modelBuilder.Entity<Persona_Referencia>()
            .HasKey(s => new { s.Id_Persona, s.Id_Referencia });

            modelBuilder.Entity<Persona_Referencia>()
                .HasOne(a => a.Persona)
                .WithMany(s => s.Persona_Referencias)
                .HasForeignKey(c => c.Id_Persona);

            modelBuilder.Entity<Persona_Referencia>()
                .HasOne(f => f.Referencia)
                .WithMany(s => s.Persona_Referencias)
                .HasForeignKey(l => l.Id_Referencia);

            /*cambios para tener referencia de una referencia*/
            modelBuilder.Entity<Referencia_Adicional>()
                .HasOne(d => d.Referencia)
                .WithMany(t => t.Referencias_Adicionales)
                .HasForeignKey(p=>p.Id_Referencia);


            modelBuilder.Entity<Accion>()
               .HasOne(f => f.Persona)
               .WithMany(s => s.Acciones)
               .HasForeignKey(l => l.Id_Persona);

            /*relaciones para los creditos*/
            modelBuilder.Entity<Credito>()
                .HasOne(d => d.Linea_Credito)
                .WithMany(s => s.Creditos)
                .HasForeignKey(f=>f.Id_Linea);

            modelBuilder.Entity<Condonacion>()
                .HasOne(a => a.Credito)
                .WithMany(a => a.Condonaciones)
                .HasForeignKey(d=>d.Id_Credito);

            /*ordenando los mutuos con la línea de crédito*/
            modelBuilder.Entity<Tipo_Mutuo>()
                .HasOne(t => t.Linea_Credito)
                .WithMany(c => c.Tipo_Mutuos)
                .HasForeignKey(c=>c.Id_Linea_Credito);

            modelBuilder.Entity<Cobro_Periodico>()
                .HasOne(c => c.Linea_Credito)
                .WithMany(l => l.Cobros_Credito)
                .HasForeignKey(c=>c.Id_Linea_Credito);
            //maneja el comportamiento del crédito
            modelBuilder.Entity<Credito>()
                .HasOne(d => d.Comportamiento_Credito)
                .WithMany(d => d.Creditos)
                .HasForeignKey(f=>f.Id_Comportamiento_Credito);

            modelBuilder.Entity<Credito>().
                HasOne(b => b.Persona).
                WithMany(d => d.Creditos).
                HasForeignKey(d => d.Id_Persona);

            modelBuilder.Entity<Credito>().
                HasOne(a => a.Acta).
                WithMany(w => w.Creditos).
                HasForeignKey(q=>q.Id_Acta);

            modelBuilder.Entity<Credito>().
                HasOne(c=>c.Periodo).
                WithMany(w=>w.Creditos).
                HasForeignKey(d=>d.Id_Periodo);

            modelBuilder.Entity<Credito>().
                HasOne(v => v.Cartera).
                WithMany(d => d.Creditos).
                HasForeignKey(c => c.Id_Cartera);
            
            modelBuilder.Entity<Comite>().
                HasOne(d => d.Credito).
                WithMany(d => d.Comites).
                HasForeignKey(d=>d.Id_Credito);

            modelBuilder.Entity<Credito_Con_Ahorro>().
                HasOne(d => d.Credito).
                WithMany(d => d.Credito_Con_Ahorros).
                HasForeignKey(d => d.Id_Credito);

            modelBuilder.Entity<Deduccion_Desembolso>().
                HasOne(d => d.Linea_Credito).
                WithMany(w => w.Deducciones_Desembolso).
                HasForeignKey(h=>h.Id_Linea_Credito);

            modelBuilder.Entity<Cobro_Periodico_Historial>().
                HasOne(a => a.Linea_Credito_Historial).
                WithMany(f => f.Cobros_Periodicos_Historial).
                HasForeignKey(c=>c.Id_Linea_Credito_Historial);

            modelBuilder.Entity<Deduccion_Historial>().
                HasOne(u => u.Linea_Credito_Historial).
                WithMany(d => d.Deducciones_Historial).
                HasForeignKey(l=>l.Id_Linea_Credito_Historial);

            modelBuilder.Entity<Credito>()
                .HasOne(b => b.Linea_Credito_Historial)
                .WithOne(f => f.Credito)
                .HasForeignKey<Linea_Credito_Historial>(b => b.Id_Linea_Credito_Historial);

            modelBuilder.Entity<Amortizacion>()
                .HasOne(d => d.Credito)
                .WithMany(d => d.Amortizaciones)
                .HasForeignKey(d => d.Id_Credito);

            //Manejo de refinanciamientos
            modelBuilder.Entity<Credito>()
                .HasOne(b => b.Manejo_Refinanciamiento)
                .WithOne(f => f.Credito)
                .HasForeignKey<Manejo_Refinanciamiento>(b => b.Id_Credito_Nuevo);
           
            /*manejo de agencia y usuarios con sus carteras*/
            modelBuilder.Entity<Usuario_Agencia>()
                .HasOne(a => a.Agencia)
                .WithMany(a => a.Usuario_Agencias)
                .HasForeignKey(h => h.Id_Agencia);

            modelBuilder.Entity<Persona>()
                .HasOne(p => p.Cartera)
                .WithMany(p => p.Personas)
                .HasForeignKey(q => q.Id_Cartera);

            modelBuilder.Entity<Persona>()
                .HasIndex(p => p.Dui)
                .IsUnique();/*/*

            /*ralcion de ahorro hasta caja*/
            modelBuilder.Entity<Linea_Ahorro>()
                .HasOne(c => c.Tipo_Linea_Ahorro)
                .WithMany(f => f.Linea_Ahorros)
                .HasForeignKey(k => k.Id_Tipo_Linea_Ahorro);

            modelBuilder.Entity<Ahorro>()
                .HasOne(h => h.Persona)
                .WithMany(w => w.Ahorros)
                .HasForeignKey(f=>f.Id_Persona);

            modelBuilder.Entity<Ahorro>()
                .HasOne(i => i.Linea_Ahorro)
                .WithMany(y => y.Ahorros)
                .HasForeignKey(v=>v.Id_Linea_Ahorro);

            modelBuilder.Entity<Transaccion_Ahorro>()
                .HasOne(o => o.Ahorro)
                .WithMany(t => t.Transacciones_Ahorros)
                .HasForeignKey(k => k.Id_Ahorro);

            modelBuilder.Entity<Transaccion_Ahorro>()
                .HasOne(l => l.Tipo_Operacion_Ahorro)
                .WithMany(e => e.Transaccion_Ahorros)
                .HasForeignKey(j => j.Id_Tipo_Operacion_Ahorro);

            modelBuilder.Entity<Transaccion_Simple>()
                .HasOne(j => j.Tipo_Operacion_Simple)
                .WithMany(w => w.Transacciones_Simple)
                .HasForeignKey(h => h.Id_Tipo_Operacion_Simple);

            modelBuilder.Entity<Transaccion_Caja>()
                .HasAlternateKey(c => new {c.No_Comprobante, c.Fecha});

            modelBuilder.Entity<Transaccion_Caja>()
                .HasOne(k => k.Caja)
                .WithMany(g => g.Transaccion_Cajas)
                .HasForeignKey(f=>f.Id_Caja);

            modelBuilder.Entity<Transaccion_Caja>()
                .HasOne(y => y.Tipo_Transaccion)
                .WithMany(t => t.Transacciones_Caja)
                .HasForeignKey(y => y.Id_Tipo_Transaccion);

            modelBuilder.Entity<Transaccion_Credito>()
                .HasOne(e => e.Credito)
                .WithMany(l => l.Transacciones_Creditos)
                .HasForeignKey(f => f.Id_Credito);

            modelBuilder.Entity<Transaccion_Credito>()
                .HasOne(s => s.Tipo_Operacion_Credito)
                .WithMany(a => a.Transacciones_Credito)
                .HasForeignKey(t=>t.Id_Tipo_Operecion_Credito);

            /*documento y persona*/
            modelBuilder.Entity<Documento>()
                .HasOne(b => b.Persona)
                .WithMany(q => q.Documentos)
                .HasForeignKey(f => f.Id_Persona);

            modelBuilder.Entity<Documento>()
                .HasOne(w => w.Descripcion)
                .WithMany(d => d.Documentos)
                .HasForeignKey(h=>h.Id_Descripcion_Documento);
            /*Flujo Efectivo*/
            modelBuilder.Entity<Efectivo>()
                .HasOne(w => w.Denominacion)
                .WithMany(d => d.Efectivos)
                .HasForeignKey(t => t.id_denominacion);

            modelBuilder.Entity<Efectivo>()
                .HasOne(w => w.Caja)
                .WithMany(d => d.Efectivos)
                .HasForeignKey(t => t.id_caja);

            modelBuilder.Entity<SaldoDisponible>()
                .HasOne(w => w.Caja)
                .WithMany(d => d.SaldoDisponibles)
                .HasForeignKey(t => t.id_caja);

            modelBuilder.Entity<Efectivo>()
                .HasOne(d => d.SaldoDisponible)
                .WithMany(t => t.Efectivos)
                .HasForeignKey(f=>f.id_saldodisponible);

            // Interes Linea de Credito
            modelBuilder.Entity<Interes_Periodo_Linea_Credito>()
                .HasOne(w => w.LineaCredito)
                .WithMany(d => d.InteresPeriodoLineaCreditos)
                .HasForeignKey(t => t.Id_Linea_Credito);
            modelBuilder.Entity<Interes_Periodo_Linea_Credito>()
                .HasOne(w => w.Periodo)
                .WithMany(d => d.InteresPeriodoLineaCreditos)
                .HasForeignKey(t => t.Id_Periodo);
            /*Menú*/
            modelBuilder.Entity<Menu_Contenido_Vista>()
                .HasOne(w => w.Menu_Categoria_Vista)
                .WithMany(d => d.Menu_Contenido)
                .HasForeignKey(h => h.Id_Menu_Categoria_Vista);

            modelBuilder.Entity<Menu_Rol_Contenido>()
                .HasOne(w => w.MenuContenidoVista)
                .WithMany(d => d.MenuRolContenidos)
                .HasForeignKey(h => h.Id_Menu_Contenido_Vista);

            modelBuilder.Entity<Menu_Rol_Categoria>()
                .HasOne(w => w.MenuCategoriaVista)
                .WithMany(d => d.MenuRolCategorias)
                .HasForeignKey(h => h.Id_Menu_Categoria_Vista);
            /*Configuraciones Buro*/
            modelBuilder.Entity<Configuracion_Calificacion>()
                .HasOne(w => w.Buro)
                .WithMany(d => d.Configuracion_Calificacion)
                .HasForeignKey(t => t.Id_Buro);

            modelBuilder.Entity<Calificacion_Crediticia>()
                .HasOne(w => w.Configuracion_Calificacion)
                .WithMany(d => d.Calificacion_Crediticias)
                .HasForeignKey(t => t.Id_Configuracion_Calificacion);

            modelBuilder.Entity<Calificacion_Crediticia>()
                .HasOne(w => w.Persona)
                .WithMany(d => d.Calificacion_Crediticias)
                .HasForeignKey(t => t.Id_Persona);

            modelBuilder.Entity<Calificacion_Crediticia>()
                .HasOne(w => w.Credito)
                .WithMany(d => d.Calificacion_Crediticias)
                .HasForeignKey(t => t.Id_Credito);

            /*transaccion libreta*/
            modelBuilder.Entity<Libreta_Transaccion>()
                .HasOne(w => w.Ahorro)
                .WithMany(d => d.Libreta)
                .HasForeignKey(t => t.Id_Ahorro);


            /*enlaces de contabilidad*/
            #region enlace_contabilidad

            modelBuilder.Entity<Conf_Partida_Contable>()
                .HasOne(t => t.Agencia)
                .WithMany(c => c.Conf_Partidas_Contables)
                .HasForeignKey(d => d.Id_Agencia);

            modelBuilder.Entity<Conf_Detalle_Partida_Contable>()
                .HasOne(d => d.Conf_Partida_Contable)
                .WithMany(f => f.Conf_Detalles_Partidas_Contables)
                .HasForeignKey(c=>c.Id_Conf_Partida_Contable)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Detalle_Asiento_Contable>()
                .HasOne(t => t.Asiento_Contable)
                .WithMany(r => r.Detalles_Asientos_Contables)
                .HasForeignKey(a=>a.Id_Asiento_Contable);

            #endregion
            /*ternima enlace a contabilidad*/


            /*para relaciones de Persona y evaluacion tecnica*/
            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.Persona)
                .WithMany(e => e.Evaluaciones)
                .HasForeignKey(p=>p.Id_Persona);

            modelBuilder.Entity<Evaluacion>()
                .HasOne(h => h.Credito)
                .WithOne(d => d.Evaluacion)
                .HasForeignKey<Credito>(b => b.Id_Credito);
            modelBuilder.Entity<Gasto>()
                .HasOne(t => t.Tipo_Gasto)
                .WithMany(g => g.Gastos)
                .HasForeignKey(t=>t.Id_Tipo_Gasto);

            modelBuilder.Entity<Tipo_Gasto>()
                .HasOne(g => g.Categoria_Gasto)
                .WithMany(c => c.Tipos_Gastos)
                .HasForeignKey(i => i.Id_Categoria_Gasto);

            modelBuilder.Entity<Otro_Ingreso>()
                .HasOne(i => i.Tipo_Ingreso)
                .WithMany(o => o.Otros_Ingresos)
                .HasForeignKey(t=>t.Id_Tipo_Ingreso);

            modelBuilder.Entity<Activo_Negocio>()
                .HasOne(a => a.Evaluacion)
                .WithMany(a => a.Activos_Negocio)
                .HasForeignKey(i => i.Id_Evaluacion);

            modelBuilder.Entity<Costo_Venta>()
                .HasOne(c => c.Evaluacion)
                .WithMany(c => c.Costos_Ventas)
                .HasForeignKey(i=>i.Id_Evaluacion);

            modelBuilder.Entity<Cuenta_Cobrar>()
                .HasOne(c => c.Evaluacion)
                .WithMany(c => c.Cuentas_Cobrar)
                .HasForeignKey(e=>e.Id_Evaluacion);

            modelBuilder.Entity<Gasto>()
                .HasOne(g => g.Evaluacion)
                .WithMany(g => g.Gastos)
                .HasForeignKey(e => e.Id_Evaluacion);

            modelBuilder.Entity<Inventario_Negocio>()
                .HasOne(i => i.Evaluacion)
                .WithMany(e => e.Inventarios_Negocio)
                .HasForeignKey(e=>e.Id_Evaluacion);

            modelBuilder.Entity<Otro_Ingreso>()
                .HasOne(o => o.Evaluacion)
                .WithMany(e => e.Otros_Ingresos)
                .HasForeignKey(e=>e.Id_Evaluacion);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Evaluacion)
                .WithMany(e => e.Prestamos)
                .HasForeignKey(e => e.Id_Evaluacion);

            /*para la colecta*/
            modelBuilder.Entity<Colecta>()
                .HasOne(c => c.Colecta_Diaria)
                .WithMany(d => d.Colectas)
                .HasForeignKey(h => h.Id_Colecta_Diaria);

            /*enlazando referencias para transaccional*/
            modelBuilder.Entity<Evaluacion_Referencia_Persona>()
                .HasOne(h => h.Evaluacion_Referencia_Catalogo)
                .WithMany(e => e.Evaluaciones_Referencias)
                .HasForeignKey(r => r.Id_Evaluacion_Referencia_Catalogo);

            modelBuilder.Entity<Evaluacion_Referencia_Persona>()
                .HasOne(h => h.Persona)
                .WithMany(e => e.Evaluaciones_Referencias)
                .HasForeignKey(r=>r.Id_Persona);

            modelBuilder.Entity<Evaluacion_Referencia_Persona>()
                .HasOne(h => h.Referencia)
                .WithMany(e => e.Evaluaciones_Referencias)
                .HasForeignKey(r => r.Id_Referencia);

            modelBuilder.Entity<Evaluacion_Referencia_Persona>()
                .HasOne(h => h.Pregunta)
                .WithMany(e => e.Evaluaciones_Referencias)
                .HasForeignKey(r => r.Id_Pregunta);

            modelBuilder.Entity<Evaluacion_Referencia_Persona>()
                .HasOne(h => h.Respuesta)
                .WithMany(e => e.Evaluaciones_Referencias)
                .HasForeignKey(r => r.Id_Respuesta);

            /*enlazando referencia para el caatalogo de pregunta*/
            modelBuilder.Entity<Pregunta>()
                .HasOne(h => h.Evaluacion_Referencia_Catalogo)
                .WithMany(e => e.Preguntas)
                .HasForeignKey(r => r.Id_Referencia_Evaluada_Catalogo);

            modelBuilder.Entity<Respuesta>()
                .HasOne(h => h.Pregunta)
                .WithMany(e => e.Respuestas)
                .HasForeignKey(r => r.Id_Pregunta);

            /*catalogo de pregunta para persona directo*/
            modelBuilder.Entity<Catalogo_Socio>()
                .HasOne(d => d.Persona)
                .WithMany(w => w.Catalogos_Socio)
                .HasForeignKey(j=>j.Id_Persona);

            modelBuilder.Entity<Catalogo_Socio>()
                .HasOne(c => c.Catalogo_Cuestionario)
                .WithMany(s => s.Catalogo_Socios)
                .HasForeignKey(a=>a.Id_Catalogo_Cuestionario);

            modelBuilder.Entity<Catalogo_Socio>()
                .HasOne(t => t.Pregunta_Catalogo)
                .WithMany(w => w.Catalogo_Socios)
                .HasForeignKey(l=>l.Id_Pregunta_Catalogo);

            modelBuilder.Entity<Catalogo_Socio>()
                .HasOne(h => h.Respuesta_Catalogo)
                .WithMany(w => w.Catalogo_Socios)
                .HasForeignKey(q=>q.Id_Respuesta_Catalogo);

            /*enlace de las preguntas y respuestas*/
            modelBuilder.Entity<Pregunta_Catalogo>()
                .HasOne(t => t.Catalogo_Cuestionario)
                .WithMany(v => v.Preguntas)
                .HasForeignKey(q=>q.Id_Catalogo_Cuestionario);

            modelBuilder.Entity<Respuesta_Catalogo>()
                .HasOne(f => f.Pregunta_Catalogo)
                .WithMany(g => g.Respuestas)
                .HasForeignKey(v=>v.Id_Pregunta_Catalogo);
            
            //Relacion de la llamada con la referencia
            modelBuilder.Entity<Llamada_Telefonica_Referencia>()
                .HasOne(t => t.Referencia)
                .WithMany(l=>l.Llamadas_Telefonicas_Referencia)
                .HasForeignKey(e=>e.Id_Referencia);

            //para las firmas autorizadas
            modelBuilder.Entity<Firma_Autorizada_Ahorro>()
                .HasOne(r => r.Ahorro)
                .WithMany(w => w.Firmas_Autorizada_Ahorro)
                .HasForeignKey(l=>l.Id_Ahorro);

            modelBuilder.Entity<Caja>()
                .HasOne(d => d.Agencia)
                .WithMany(x => x.Cajas)
                .HasForeignKey(d => d.Id_Agencia);

            /*el manejo de islas*/
            modelBuilder.Entity<Ingreso_Cheque>()
                .HasOne(t => t.Agencia)
                .WithMany(f => f.Cheques)
                .HasForeignKey(t=>t.Id_Agencia);

            modelBuilder.Entity<Accion>()
                .HasOne(d => d.Tipo_Accion)
                .WithMany(z => z.Acciones)
                .HasForeignKey(c=>c.Id_Tipo_Accion);

            /*acceso directos*/
            modelBuilder.Entity<Acceso_Directo>()
                .HasOne(f => f.Categoria_Acceso_Directo)
                .WithMany(d => d.Acceso_Directos)
                .HasForeignKey(c=>c.Id_Categoria_Acceso_Directo);

            /*mantenimiento grupales*/
            modelBuilder.Entity<Credito_Grupal>()
                .HasOne(t => t.Agencia)
                .WithMany(d => d.Credito_Grupals)
                .HasForeignKey(c => c.Id_Agencia);

            modelBuilder.Entity<Detalle_Credito_Grupal>()
                .HasAlternateKey(c => new { c.Id_Credito, c.Id_Credito_Grupal });

            modelBuilder.Entity<Detalle_Credito_Grupal>()
                .HasOne(f => f.Credito_Grupal)
                .WithMany(d => d.Detalle_Credito_Grupal)
                .HasForeignKey(c => c.Id_Credito_Grupal);

            modelBuilder.Entity<Detalle_Credito_Grupal>()
                .HasOne(f => f.Credito)
                .WithMany(d => d.Detalle_Credito_Grupal)
                .HasForeignKey(c => c.Id_Credito);

            /*manejo del tablero*/
            modelBuilder.Entity<Rancking>()
                .HasOne(d => d.Cartera)
                .WithMany(d => d.Rankings)
                .HasForeignKey(d => d.Id_Cartera);

            modelBuilder.Entity<Meta>()
                .HasOne(a => a.Cartera)
                .WithMany(a => a.Metas)
                .HasForeignKey(a=>a.Id_Cartera);

            modelBuilder.Entity<Cosecha>()
                .HasOne(c => c.Cartera)
                .WithMany(c => c.Cosechas)
                .HasForeignKey(c=>c.Id_Cartera);

            modelBuilder.Entity<Reporte_Mora>()
            .HasOne(d => d.Cartera)
            .WithMany(d => d.Reporte_Moras)
            .HasForeignKey(d => d.Id_Cartera);

            modelBuilder.Entity<Deposito_Plazo>()
            .HasOne(h => h.Persona)
            .WithMany(w => w.Deposito_Plazo)
            .HasForeignKey(f => f.Id_Persona);

            modelBuilder.Entity<Beneficiarios_Deposito_Plazo>()
           .HasOne(h => h.Deposito_Plazo)
           .WithMany(w => w.Beneficiarios_Deposito_Plazo)
           .HasForeignKey(f => f.Id_Deposito_Plazo);

            modelBuilder.Entity<Beneficiarios_Deposito_Plazo>()
           .HasOne(h => h.Parentesco)
           .WithMany(w => w.Beneficiarios_Deposito_Plazo)
           .HasForeignKey(f => f.Id_Parentesco);

            modelBuilder.Entity<Autorizado>()
           .HasOne(h => h.Deposito_Plazo)
           .WithMany(w => w.Autorizado)
           .HasForeignKey(f => f.Id_Deposito_Plazo);

            modelBuilder.Entity<Deposito_Plazo>()
              .HasOne(h => h.Forma_Pago)
              .WithMany(w => w.Deposito_Plazo)
              .HasForeignKey(f => f.Id_Forma_Pago);

            modelBuilder.Entity<Deposito_Plazo>()
              .HasOne(h => h.Linea_Deposito_Plazo)
              .WithMany(w => w.Depositos_Plazo)
              .HasForeignKey(f => f.Id_Linea_Deposito_Plazo);

            modelBuilder.Entity<Transaccion_Deposito_Plazo>()
           .HasOne(o => o.Deposito_Plazo)
           .WithMany(t => t.Transacciones_Depositos)
           .HasForeignKey(k => k.Id_Deposito_Plazo);

            modelBuilder.Entity<Transaccion_Deposito_Plazo>()
                .HasOne(l => l.Tipo_Operacion_Ahorro)
                .WithMany(e => e.Transaccion_Deposito_Plazo)
                .HasForeignKey(j => j.Id_Tipo_Operacion_Ahorro);

            modelBuilder.Entity<Credito_Con_Ahorro>()
                .HasOne(b => b.Porcentaje_Ahorro_Credito)
                .WithMany(f => f.Credito_Con_Ahorro)
                .HasForeignKey(j => j.Id_Porcentaje_Ahorro_Credito);

            modelBuilder.Entity<Fotografia_Comprobante>()
            .HasOne(b => b.Colecta)
            .WithMany(q => q.Fotografia_Comprobantes)
            .HasForeignKey(f => f.Id_Colecta);

            modelBuilder.Entity<Fotografia_Atm>()
            .HasOne(b => b.Banco)
            .WithMany(q => q.Fotografia_Atms)
            .HasForeignKey(f => f.Id_Banco);
            modelBuilder.Entity<Comite_Ampliado>()
           .HasOne(b => b.Credito)
           .WithMany(q => q.Comite_Ampliados)
           .HasForeignKey(f => f.Id_Credito);
            modelBuilder.Entity<Fotografia_Comite>()
           .HasOne(b => b.Comite_Ampliado)
           .WithMany(q => q.Fotografia_Comites)
           .HasForeignKey(f => f.Id_Comite_Ampliado);

            modelBuilder.Entity<CreditoComiteAmpliado>()
            .HasOne(b => b.Comite_Ampliado)
            .WithMany(q => q.CreditoComiteAmpliado)
            .HasForeignKey(f => f.Id_Comite_Ampliado);

            modelBuilder.Entity<Linea_Credito>()
                .HasOne(d => d.TipoGarantia)
                .WithMany(s => s.Linea_Creditos)
                .HasForeignKey(f => f.Tipo_Garantia);

            modelBuilder.Entity<Linea_Credito>()
                .HasOne(d => d.TipoDestinoCredito)
                .WithMany(s => s.Linea_Creditos)
                .HasForeignKey(f => f.Destino_Credito);
        }

    }
}