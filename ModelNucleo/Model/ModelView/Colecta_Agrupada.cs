using System.Collections.Generic;
using System.Linq;

namespace ModelNucleo.Model.ModelView
{
    public class Colecta_Agrupada
    {
        public Colecta_Diaria Colecta_Diaria { get; set; }

        public Colecta_Agrupada(Colecta_Diaria cd)
        {
            Colecta_Diaria = cd;
        }

        public Colecta_Agrupada()
        {

        }

        public IEnumerable<Colecta> Colecta_Ahorro
        {
            get
            {
                return Colecta_Diaria.Colectas.Where(q => q.Id_Ahorro > 0);
            }
            
        }

        public IEnumerable<Colecta> Colecta_Credito
        {
            get
            {
                return Colecta_Diaria.Colectas.Where(q => q.Id_Credito > 0);
            }
        }
    }
}
