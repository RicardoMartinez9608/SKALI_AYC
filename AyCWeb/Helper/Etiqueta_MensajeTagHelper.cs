namespace AyCWeb.Helper
{
    public class Mensaje_Respuesta
    {
        public bool Satisfactorio { get; set; }
        public int Count { get; set; }
        public string Descripcion_Error { get; set; }
        public string Descripcion_Correcto { get; set; }

        public Mensaje_Respuesta() { }
        public Mensaje_Respuesta(bool satisfaccion,int count,string error=null,string correcto=null)
        {
            Satisfactorio = satisfaccion;
            Count = count;
            Descripcion_Error = error;
            Descripcion_Correcto = correcto;
        }
    }

    public static class Tipo_Mensaje{
        public const string Correcto = "success";
        public const string Informativo = "info";
        public const string Precuacion = "warning";
        public const string Error = "danger";
    }
}
