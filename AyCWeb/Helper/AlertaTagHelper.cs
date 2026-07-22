using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AyCWeb.Helper
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("ayc-alerta")]
    public class AlertaTagHelper : TagHelper
    {
        [HtmlAttributeName("respuesta")]
        public Mensaje_Respuesta Respuesta { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            string tipo = "";
            string mensaje = "";
            string nota = "";
            if (Respuesta != null)
            {
                if (Respuesta.Satisfactorio)
                {
                    nota = "Correcto";
                    tipo = Tipo_Mensaje.Correcto;
                    mensaje = Respuesta.Descripcion_Correcto;
                    if (Respuesta.Count == 0 && string.IsNullOrEmpty(Respuesta.Descripcion_Correcto) && string.IsNullOrEmpty(Respuesta.Descripcion_Error))
                    {
                        nota = "Precaución";
                        tipo = Tipo_Mensaje.Precuacion;
                        mensaje = "No se han encontrado datos";
                    }
                }
                else
                {
                    nota = "Error";
                    tipo = Tipo_Mensaje.Error;
                    mensaje = Respuesta.Descripcion_Error;
                }

                if (Respuesta.Count > 0)
                    output.Content.SetHtmlContent("");
                else
                    output.Content.SetHtmlContent($@"<div class='alert alert-{tipo} alert-dismissible' role='alert'>
                <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times;</span></button>
                <strong>{nota}: </strong>{mensaje}</div>");

                output.TagMode = TagMode.StartTagAndEndTag;
            }
        }
    }
}
