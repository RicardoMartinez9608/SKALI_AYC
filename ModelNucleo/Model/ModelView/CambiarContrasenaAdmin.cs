using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class CambiarContrasenaAdmin
    {
        public string Id_Usuario { get; set; }

        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
    }
}
