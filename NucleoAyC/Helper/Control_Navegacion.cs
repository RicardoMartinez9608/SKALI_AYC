using System.Collections.Generic;
using System.Linq;

namespace NucleoAyC.Helper
{
    public class Control_Navegacion
    {
        public Control_Navegacion()
        {
        
        }

        public T Paginar<T>(IEnumerable<T> list, T current,bool paginar=false,bool siguiente=true)
        {
            T objeto_retornar = default(T);
            if (paginar)
            {
                if (siguiente)
                {
                    objeto_retornar = Siguiente(list, current);
                    if (objeto_retornar == null)
                        objeto_retornar = current;
                }
                else
                {
                    objeto_retornar = Anterior(list, current);
                    if (objeto_retornar == null)
                        objeto_retornar = current;
                }
            }
            else
                objeto_retornar= current;

            return objeto_retornar;
        }
        public T Siguiente<T>(IEnumerable<T> list, T current)
        {
            try
            {
                return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
            }
            catch
            {
                return default(T);
            }
        }

        public T Anterior<T>(IEnumerable<T> list, T current)
        {
            try
            {
                return list.TakeWhile(x => !x.Equals(current)).Last();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
