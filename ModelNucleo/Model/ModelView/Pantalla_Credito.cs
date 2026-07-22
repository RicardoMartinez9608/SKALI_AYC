using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ModelNucleo.CalculosYProceos;

namespace ModelNucleo.Model.ModelView
{
   public class Pantalla_Credito
    {
        public Pantalla_Credito()
        { }
        public Pantalla_Credito(
            Credito credito, 
            string Nombre_Codeudor, 
            string municipio, 
            Linea_Credito_Historial linea_historial)
        {
            Aprobado_Por = "Comité de Crédito";
            Nombre_Socio = credito.Persona.NombreCompleto;
            Codigo_Credito = credito.Codigo_Credito;
            Destino = linea_historial != null ? linea_historial.Descripcion_linea : "";
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Monto_Letras = string.Format("{0} {1}", conversion.Enletras(credito.Monto_pago.ToString()), "Dólares de Estados Unidos de América");
            Monto_Numero = credito.Monto_pago;
            Plazo = credito.Plazo;
            Forma_Pago = credito.Periodo.Descripcion;
            Cuota = credito.Cuota_Total;
            Garantia = linea_historial != null ? linea_historial.Tipo_Garantia.ToString() : "";
            Vencimiento = credito.Vencido_En;
            Codeudor = Nombre_Codeudor;
            Deducciones = linea_historial != null ? linea_historial.Deducciones_Historial : new List<Deduccion_Historial>();

            if (linea_historial != null)
            {
                if (linea_historial.Deducciones_Historial != null)
                    Total_Deduccion = linea_historial.Deducciones_Historial.Sum(s => s.Valor);
            }

            if (credito.Id_Comportamiento_Credito == 5)
            {
                Liquido_Recibir = 0.00M;
            }

            Liquido_Recibir = Monto_Numero - Total_Deduccion;
            Ciudad_Celebrado = municipio;
            Hora_Celebrado = credito.Liquidado_En.Hour.ToString();
            DateTime liquidado = credito.Liquidado_En.Date;
            Fecha_Letra = string.Format("{0} de {1} de {2}", liquidado.Day, Nombre_Mes(liquidado.Month), liquidado.Year);
        }

        #region Encabezado

        public string Aprobado_Por { get; set; }//por lo general COMITE DE PRESTAMOS
        public string Asesor { get; set; }
        public string Colector { get; set; }
        public string dia_ejecucion { get; set; }
        public string hora_ejecucion { get; set; }
        public Agencia agencia { get; set; }
        public string Creador { get; set; }

        #endregion

        #region Datos del prestamo

        public string Nombre_Socio { get; set; }
        public string Codigo_Credito { get; set; }
        public string Destino { get; set; }//poede ser la descripción de la línea de crédito
        public string Monto_Letras { get; set; }
        public decimal Monto_Numero { get; set; }
        public int Plazo { get; set; }
        public string Forma_Pago { get; set; }
        public decimal Cuota { get; set; }
        public string Garantia { get; set; }
        public DateTime Vencimiento { get; set; }
        public string Codeudor { get; set; }

        #endregion

        #region Deducciones

        public ICollection<Deduccion_Historial> Deducciones { get; set; }
        public decimal Total_Deduccion { get; set; }
        public decimal Liquido_Recibir { get; set; }

        #endregion

        #region Lugar y Fecha

        public string Ciudad_Celebrado { get; set; }
        public string Hora_Celebrado { get; set; }
        public string Fecha_Letra { get; set; }

        #endregion

        private string Nombre_Mes(int mes)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-Es", false).DateTimeFormat;
            return dtinfo.GetMonthName(mes);
        }
    }
}
