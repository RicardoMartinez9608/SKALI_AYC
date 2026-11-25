using ModelNucleo.Model;
using System;
using System.Collections.Generic;
using System.Text;

public class CertificacionRequest
{
    public DocumentoFactura documento { get; set; }

    public class DocumentoFactura
    {
        public string tipo_dte { get; set; }
        public string establecimiento { get; set; }
        public string numero_control { get; set; }
        public string punto_venta { get; set; }
        public int condicion_pago { get; set; }
        public Receptor receptor { get; set; }
        public List<Item> items { get; set; }
        public bool retener_iva { get; set; }
        public string tipo_contingencia { get; set; }
        public string motivo_contingencia { get; set; }
        public List<Pago> pagos { get; set; }
    }

    public class Receptor
    {
        public string tipo_documento { get; set; }
        public string numero_documento { get; set; }
        public string correo { get; set; }
        public string correos_cc { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string codigo_actividad { get; set; }
        public Direccion direccion { get; set; }
    }

    public class Direccion
    {
        public string departamento { get; set; }
        public string municipio { get; set; }
        public string complemento { get; set; }
        public string distrito { get; set; }
    }

    public class Item
    {
        public int tipo { get; set; }
        public int cantidad { get; set; }
        public int unidad_medida { get; set; }
        public string descripcion { get; set; }
        public double precio_unitario { get; set; }
        public int descuento { get; set; }
        public double monto_bruto { get; set; }
        public string tipo_venta { get; set; }
        public string codigo { get; set; }
    }

    public class Pago
    {
        public string tipo { get; set; }
        public string plazo { get; set; }
        public int periodo { get; set; }
        public double monto { get; set; }
    }


}
