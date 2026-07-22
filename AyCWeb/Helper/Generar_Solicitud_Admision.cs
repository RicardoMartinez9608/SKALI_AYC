using AyCWeb.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ModelNucleo.Model;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AyCWeb.Helper
{
    public class Generar_Solicitud_Admision
    {
        public MemoryStream Generar_Solicitud_Socio(Ubicacion_Politica_Etiqueta_Persona ubicacion_etiqueta_persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "solicitud_socio_admision.docx");
            string documento_destino = Completa_Nombre_Aleatorio();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos(ubicacion_etiqueta_persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);
            //documentStream.Close();
            doc.Close();
            return documentStream;
        }

        private string Completa_Nombre_Aleatorio()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos(Ubicacion_Politica_Etiqueta_Persona ubicacion_etiqueta_persona, string xml)
        {
            Persona persona = ubicacion_etiqueta_persona.Persona;
            Etiqueta_Municipio_Departamento etiquetas = ubicacion_etiqueta_persona.Etiquetas;

            /*La persona como tal*/
            xml = Reemplazar(xml, "NOMBRE_COMPLETO", persona.NombreCompleto);
            xml = Reemplazar(xml, "PROFESION_OFICIO", persona.Profesion);
            
            /*obtener los nombres de los municipio y departamento*/
            xml = Reemplazar(xml, "MUNICIPIO",etiquetas.Municipio_Domicilio);
            xml = Reemplazar(xml, "DEPARTAMENTO", etiquetas.Departamento_Domicilio);

            xml = Reemplazar(xml, "DUI", persona.Dui);
            xml = Reemplazar(xml, "NIT", persona.Nit);

            var fecha_guardado = DateTime.Now;
            StringBuilder string_fecha = new StringBuilder();
            string_fecha.AppendFormat("{0:dd,MMMM,yyyy}", fecha_guardado);
            string[] partes_fecha = string_fecha.ToString().Split(',');

            xml = Reemplazar(xml, "DIAS", partes_fecha[0]);
            xml = Reemplazar(xml, "MES_LETRAS", partes_fecha[1]);
            xml = Reemplazar(xml, "YEAR", partes_fecha[2]);

            return xml;
        }

        private string Reemplazar(string xml, string fuente, string valor)
        {
            valor = valor ?? "No definido";
            Regex regexText = new Regex(fuente);
            xml = regexText.Replace(xml, valor);
            return xml;
        }

    }
}
