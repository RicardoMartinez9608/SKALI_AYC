using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.Dto
{
    public class EstadoCuentaDTO
    {
        public string No_Ahorro { get; set; }
        public string NombrePersona { get; set; }
        public string EmailPersona { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal SaldoFinal { get; set; }
        public string TextoPublicidad { get; set; }
        public string ImagenArriba { get; set; }
        public string ImagenAbajo { get; set; }
        public List<MovimientoDTO> Movimientos { get; set; }
    }

    public class MovimientoDTO
    {
        public DateTime Fecha { get; set; }
        public string TipoOperacion { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
    }
}
