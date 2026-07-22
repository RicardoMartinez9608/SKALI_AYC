using ModelNucleo.Model.Contabilidad;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelNucleo.Model
{
    public class Agencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Agencia { get; set; }
        public string Codigo_Agencia { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }

        [NotMapped]
        public ICollection<Usuario_Agencia> Usuario_Agencias { get; set; }
        [NotMapped]
        public ICollection<Caja> Cajas { get; set; }

        [NotMapped]
        public ICollection<Conf_Partida_Contable> Conf_Partidas_Contables { get; set; }

        [NotMapped]
        public ICollection<Ingreso_Cheque> Cheques { get; set; }

        [NotMapped]
        public ICollection<Credito_Grupal> Credito_Grupals { get; set; }
    }
}
