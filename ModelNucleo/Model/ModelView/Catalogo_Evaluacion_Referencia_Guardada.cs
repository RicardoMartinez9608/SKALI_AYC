using ModelNucleo.Model.Evaluacion_Referencia;
using System.ComponentModel;

namespace ModelNucleo.Model.ModelView
{
    public class Catalogo_Evaluacion_Referencia_Guardada
    {
        public Pregunta Pregunta_catalogo { get; set; }

        [DefaultValue(0)]
        public int Id_Respuesta_Contestada { get; set; }
    }
}
