using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class Ponderacion_Datos_Calificacion
    {
        public decimal Valor { get; set; }
        public Ponderacion Ponderacion { get; set; }
        public decimal Resultado
        {
            get
            {
                return Math.Round((Valor / (int)Ponderacion) * 100, 2);
            }
        }
        public string Operacion
        {
            get
            {
                return string.Format("{0} / {1}", Valor, (int)Ponderacion);
            }
        }
    }

    public enum Ponderacion
    {
        Caracter = 24,
        Capacidad = 33,
        Capital = 20,
        Condiciones = 18,
        Colateral = 5,
        Total = 100
    }
}
