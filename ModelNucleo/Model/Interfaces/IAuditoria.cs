using System;

namespace ModelNucleo.Model
{
    public interface IAuditoria
    {
        DateTime Creado_En { get; set; }
        DateTime Eliminado_En { get; set; }
        DateTime Modificado_En { get; set; }

        string Creado_Por { get; set; }
        string Eliminado_Por { get; set; }
        string Modificado_Por { get; set; }

        bool Modificado { get; set; }
        bool Borrado_Logico { get; set; }
    }
}
