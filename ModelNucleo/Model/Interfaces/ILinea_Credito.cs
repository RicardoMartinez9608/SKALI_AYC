namespace ModelNucleo.Model.Interfaces
{
    public interface ILinea_Credito
    {
        string Codigo { get; set; }
        string Descripcion_linea { get; set; }
        int Tipo_Garantia { get; set; }
        decimal Interes_Comision { get; set; }
        decimal Tasa_Mora { get; set; }
        decimal Recargo { get; set; }
        int Dia_Gracia { get; set; }

        /*agregado para manejar como dinero los cargos*/
        bool PlanAyC_Cobros_Periodico_Dinero { get; set; }

        bool Activar_Deducciones { get; set; }

        decimal Interes { get; set; }/*el % para el dato, no el calculado*/

        /*nuevos campos que hacen falta de acuerdo a reuniones*/
        /*rango de los plazos que se permitirán*/
        int Plazo_Rango_Inicial { get; set; }
        int Plazo_Rango_Final { get; set; }

        //decimal Estructuracion { get; set; }/*es un cobro al momento de la liquidación*/

        /*rango de los montos permitidos para esta línea de creditos*/
        decimal Monto_Rango_Inicial { get; set; }
        decimal Monto_Rango_Final { get; set; }

        int Destino_Credito { get; set; }
    }
}
