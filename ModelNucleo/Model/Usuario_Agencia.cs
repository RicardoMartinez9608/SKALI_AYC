using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Usuario_Agencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Usuario_Agencia { get; set; }

        public int Id_Agencia { get; set; }
        public Agencia Agencia { get; set; }

        public string Id_Usuario { get; set; }

        public string Nombre_Completo { get; set; }
    }
}
