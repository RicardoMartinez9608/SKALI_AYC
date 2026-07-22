using ModelNucleo.Model;
using ModelNucleo.Model.Evaluacion_Referencia;
using System.Collections.Generic;
using System.Linq;

namespace ModelNucleo.CalculosYProceos
{
    public class CalculoPonderacionEvaluacion
    {
        public decimal Ponderar(List<Evaluacion_Referencia_Persona> preguntas,decimal total_max_preguntas)
        {
            decimal Total_preguntas_respondidas = preguntas.Sum(s=>s.Ponderacion);
            decimal Porcentaje = Total_preguntas_respondidas > 0? (Total_preguntas_respondidas / total_max_preguntas)*100 :0.00M;
            return Porcentaje;
        }

        public Valoracion_Referencia Definir_Matriz(decimal ponderacion)
        {
            if (ponderacion >= 50)
                return Valoracion_Referencia.Buena_Referencia;
            else
                return Valoracion_Referencia.Mala_Referencia;
        }
    }
}
