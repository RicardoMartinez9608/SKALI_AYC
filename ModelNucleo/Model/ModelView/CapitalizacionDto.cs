using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Model.ModelView
{
    public class CapitalizacionDto
    {
        public int IdAhorro { get; set; }
        public decimal Valor { get; set; }
        public DateTime Fecha { get; set; }
        public int IdDepositoPlazo { get; set; }
        public bool EsDepositoPlazo { get; set; }
    }
}
