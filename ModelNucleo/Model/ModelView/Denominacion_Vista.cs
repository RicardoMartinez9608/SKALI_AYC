using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
   public class Denominacion_Vista
    {
        public Denominacion_efectivo Denominacion_dinero { get; set; }
    }
    public class Reporte_Denominacion
    {
        public int Id_denominacion { get; set; }
        public decimal denominacion { get; set; }
        public string tipo { get; set; }
        public decimal valor { get; set; }
        public List<Denominacion_Vista> detalle { get; set; }
    }
}
