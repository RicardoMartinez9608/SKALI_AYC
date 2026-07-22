using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelNucleo.Model.Contabilidad
{
    public class Conf_Partida_Contable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Conf_Partida_Contable { get; set; }
        public string Identificador { get; set; }
        public string Descripcion { get; set; }
        public int Id_Agencia { get; set; }
        public Agencia Agencia { get; set; }

        public int Id_Linea_Credito { get; set; }
        public int Id_Linea_Ahorro { get; set; }

        public int Id_Factura_Servicio { get; set; }
        public int Id_Otras_Operaciones { get; set; }
        [NotMapped]
        public ICollection<Conf_Detalle_Partida_Contable> Conf_Detalles_Partidas_Contables { get; set; }
    }
}
