using AyCWeb.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ModelNucleo.Model;
using ModelNucleo.Model.ModelView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AyCWeb.Helper
{
    public class Generar_Solicitud
    {
        public MemoryStream Generar_Solicitud_Socio(Ubicacion_Politica_Etiqueta_Persona ubicacion_etiqueta_persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "solicitud_socio.docx");
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
            xml = Reemplazar(xml, "NOMBRES", persona.Nombres);
            xml = Reemplazar(xml, "APELLIDOS", persona.Apellidos);
            xml = Reemplazar(xml, "EDAD_S", persona.Edad.ToString());
            xml = Reemplazar(xml, "ESTADO_CIVIL", persona.EstadoFamiliar.ToString());
            xml = Reemplazar(xml, "GENERO", persona.Sexo.ToString());
            xml = Reemplazar(xml, "PROFESION", persona.Profesion);
            xml = Reemplazar(xml, "CORREO_ELECTRONICO", persona.Correo);
            xml = Reemplazar(xml, "TELEFONO_FIJO", persona.Telefono);

            xml = Reemplazar(xml, "COMO_LLEGAR_DOMICILIO", " ");

            xml = Reemplazar(xml, "DOMICILIO_SIMPLE", string.Format(
                "{0} ,{1}, {2}",
                etiquetas.Departamento_Domicilio,
                etiquetas.Municipio_Domicilio,
                persona.Domicilio));
            xml = Reemplazar(xml, "NOMBRE_TITULAR_VIVIENDA", " ");//No se tiene el campo
            xml = Reemplazar(xml, "T_RESIDIR", " ");//No se tiene el campo

            xml = Reemplazar(xml, "TELEFONO_M", persona.Telefono);
            xml = Reemplazar(xml, "OPERA_TEL_MOV", " ");//no se tienen este campo


            /*selecciona el estado del dueño de la casa*/
            if (persona.Residencia == Residencia.Propio)
            {
                xml = Reemplazar(xml, "PROPIA", "X");
                xml = Reemplazar(xml, "ALQUILADA", " ");
                xml = Reemplazar(xml, "FAMILIAR_V", " ");
            }

            if (persona.Residencia == Residencia.Alquilada)
            {
                xml = Reemplazar(xml, "PROPIA", " ");
                xml = Reemplazar(xml, "ALQUILADA", "X");
                xml = Reemplazar(xml, "FAMILIAR_V", " ");
            }

            if (persona.Residencia == Residencia.Familiar)
            {
                xml = Reemplazar(xml, "PROPIA", " ");
                xml = Reemplazar(xml, "ALQUILADA", " ");
                xml = Reemplazar(xml, "FAMILIAR_V", "X");
            }

            if (persona.Residencia == Residencia.Seleccionar)
            {
                xml = Reemplazar(xml, "PROPIA", " ");
                xml = Reemplazar(xml, "ALQUILADA", " ");
                xml = Reemplazar(xml, "FAMILIAR_V", " ");
            }
            /*termina selecciona el estado del dueño de la casa*/

            xml = Reemplazar(xml, "DUI_V", persona.Dui);
            xml = Reemplazar(xml, "EXPEDICION", persona.ExpedicionDocumentoIdentifcacion);
            xml = Reemplazar(xml, "EXPIRACION", persona.ExpiracionDocumentoIdentificacion);
            xml = Reemplazar(xml, "NIT_V", persona.Nit);
            xml = Reemplazar(xml, "CARNE_RESIDENT"," ");//no esta el campo
            xml = Reemplazar(xml, "PASAPORTE", persona.Pasaporte);
            xml = Reemplazar(xml, "LUGAR_FECHA_NACIMIENTO", persona.FechaNacimiento.Value.Date.ToString());
            xml = Reemplazar(xml, "NACIONALIDAD", persona.Nacionalidad);

            xml = Reemplazar(xml, "ACTIVIDAD_ECONOMICA", persona.Ocupacion);
            xml = Reemplazar(xml, "NOMBRE_LUGAR_TRABAJO", " "); //no se tiene ese campo

            xml = Reemplazar(xml, "DIRECCION_TRABAJO_S", persona.Direccion_Trabajo);

            xml = Reemplazar(xml, "TIEMPO_LABORAR", " "); //no se tiene ese campo

            xml = Reemplazar(xml, "TELEFONO_TRABAJO", persona.Telefono_Trabajo);
            xml = Reemplazar(xml, "INGRESO_MENSUAL", Math.Round(persona.Ingresos_Mensuales,2).ToString());
            xml = Reemplazar(xml, "CARGO_TRABAJO", persona.Cargo_Trabajo);
            xml = Reemplazar(xml, "NOMBRE_JEFE_INMEDIATO", persona.Nombre_Jefe_Inmediato);

            int centinela = 0;
            if (persona.Persona_Referencias.Any(a=>a.Tipo_Referencia=="1"))
                foreach (Persona_Referencia referencia in persona.Persona_Referencias.Where(a => a.Tipo_Referencia == "1"))
                {
                    centinela += 1;
                    if ( !string.IsNullOrEmpty(referencia.Parentesco) && (centinela == 1 || centinela == 2))
                    {
                        xml = Reemplazar(xml,string.Format("NOMBRE_FAMILIAR_{0}",centinela), referencia.Referencia.Nombre);
                        xml = Reemplazar(xml, string.Format("PARENTESCO_FAMILIAR_{0}", centinela), referencia.Parentesco);
                        xml = Reemplazar(xml, string.Format("DIRECCION_FAMILIAR_{0}", centinela), referencia.Referencia.Residencia);
                        xml = Reemplazar(xml, string.Format("TELEFONO_FAMILIAR_{0}", centinela), referencia.Referencia.Telefono);
                    }else
                        break;
                }

            //los que quedaron en blanco
            for (int i = centinela; i <= 2; i++)
            {
                centinela += 1;
                xml = Reemplazar(xml, string.Format("NOMBRE_FAMILIAR_{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("PARENTESCO_FAMILIAR_{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("DIRECCION_FAMILIAR_{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("TELEFONO_FAMILIAR_{0}", centinela), " ");
            }

            centinela = 0;
            if (persona.Persona_Referencias.Any(p=>p.Tipo_Referencia=="2"))
                foreach (Persona_Referencia referencia in persona.Persona_Referencias.Where(p => p.Tipo_Referencia == "2"))
                {
                    centinela += 1;
                    if (/*!string.IsNullOrEmpty(referencia.Parentesco) && */centinela == 1)
                    {
                        xml = Reemplazar(xml, string.Format("NOMBRE_AMISTAD_{0}", centinela), referencia.Referencia.Nombre);
                        xml = Reemplazar(xml, string.Format("DIRECCION_AMISTAD_{0}", centinela), referencia.Referencia.Residencia);
                        xml = Reemplazar(xml, string.Format("TELEFONO_AMISTAD_{0}", centinela), referencia.Referencia.Telefono);
                    }
                    else
                        break;
                }
            //poner en blanco los datos de amistad no disponible
            for (int i = centinela; i <= 2; i++)
            {
                centinela += 1;
                xml = Reemplazar(xml, string.Format("NOMBRE_AMISTAD_{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("DIRECCION_AMISTAD_{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("TELEFONO_AMISTAD_{0}", centinela), " ");
            }

            if (persona.Tiene_Cargo)
            {
                xml = Reemplazar(xml, "SI_DEP", "X");
                xml = Reemplazar(xml, "NO_DEP", " ");
            }
            else {
                xml = Reemplazar(xml, "SI_DEP", " ");
                xml = Reemplazar(xml, "NO_DEP", "X");
            }

            if (persona.Familiar_Cargo)
            {
                xml = Reemplazar(xml, "SI_FAP", "X");
                xml = Reemplazar(xml, "NO_FAP", " ");
            }
            else
            {
                xml = Reemplazar(xml, "SI_FAP", " ");
                xml = Reemplazar(xml, "NO_FAP", "X");
            }


            if (persona.Asociado_Cargo)
            {
                xml = Reemplazar(xml, "SI_ASP", "X");
                xml = Reemplazar(xml, "NO_ASP", " ");
            }
            else
            {
                xml = Reemplazar(xml, "SI_ASP", " ");
                xml = Reemplazar(xml, "NO_ASP", "X");
            }

            /*los datos de instituciones bancarias con cuentas de ahorro*/
            xml = Reemplazar(xml, "INSTITUCIONES_CUENTAS_AHORRO", " "); //no se tiene el campo
            xml = Reemplazar(xml, "INSTITUCIONES_CREDITOS_ACTIVOS", " "); //no se tiene el campo

            /*toda la información para la apertura de cuenta de ahorro*/
            xml = Reemplazar(xml, "LINEA_AHORRO", " "); //no se tiene el campo
            xml = Reemplazar(xml, "FRECUENCIA_DEPOSITO", " "); //no se tiene el campo
            xml = Reemplazar(xml, "SUMA_AHORRO", " "); //no se tiene el campo
            xml = Reemplazar(xml, "FORMA_REINTEGRO", " "); //no se tiene el campo
            xml = Reemplazar(xml, "PLAZO_AHORRO", " "); //no se tiene el campo
            xml = Reemplazar(xml, "CUOTA_AHORRO", " "); //no se tiene el campo
            xml = Reemplazar(xml, "CONTRATO_VENCE", " "); //no se tiene el campo
            xml = Reemplazar(xml, "NO_RETIRO", " "); //no se tiene el campo

            centinela = 0;
            Func<Persona_Referencia, bool> saber_beneficiario = new Func<Persona_Referencia, bool>(p => p.Referencia.Beneficiario);
            int cuentas_beneficiarios = persona.Persona_Referencias.Count(saber_beneficiario);
            if (persona.Persona_Referencias.Any(saber_beneficiario))
                foreach (Persona_Referencia referencia in persona.Persona_Referencias.Where(saber_beneficiario))
                {
                    centinela += 1;
                    if (centinela>=1 && centinela <= 4)
                    {
                        xml = Reemplazar(xml, string.Format("BENEFICIARIO_NOMBRE_COMPLETO_{0}", centinela), referencia.Referencia.Nombre);
                        xml = Reemplazar(xml, string.Format("EDADB{0}", centinela), referencia.Referencia.Edad.ToString());
                        xml = Reemplazar(xml, string.Format("PARENTESCOB{0}", centinela), referencia.Parentesco);
                        xml = Reemplazar(xml, string.Format("PORCENTAJEB{0}", centinela), Math.Round(referencia.Referencia.Porcentaje,2).ToString());
                    }
                    else
                        break;
                }

            /*borrar las variables que no se utilizaron*/
            for (int i = centinela; i <= 4; i++)
            {
                centinela += 1;
                xml = Reemplazar(xml, string.Format("BENEFICIARIO_NOMBRE_COMPLETO_{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("EDADB{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("PARENTESCOB{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("PORCENTAJEB{0}", centinela), " ");
            }

            Credito credito_solicita = persona.Creditos.Any()?persona.Creditos.Last():null;
            if (credito_solicita != null)
            {
                /*datos de un crédito de acuerdo si es deudor o codeudor*/
                xml = Reemplazar(xml, "ES_DEUDOR", " ");//no se tiene el campo
                xml = Reemplazar(xml, "ES_CODEUDOR", " ");//no se tiene el campo
                xml = Reemplazar(xml, "LINEA_CREDITO", credito_solicita != null ? credito_solicita.Linea_Credito.Descripcion_linea : " ");//no se tiene el campo
                xml = Reemplazar(xml, "TIPO_NEGOCIO", " ");//no se tiene el campo
                xml = Reemplazar(xml, "TEL_FIJO", " ");//no se tiene el campo
                xml = Reemplazar(xml, "TEL_MO", " ");//no se tiene el campo
                xml = Reemplazar(xml, "UBICACION_NEGOCIO_RUTA", " ");//no se tiene el campo
                xml = Reemplazar(xml, "TIEMPO_COMER", " ");//no se tiene el campo
                xml = Reemplazar(xml, "TIEMPO_NEGOCIO", " ");//no se tiene el campo
                xml = Reemplazar(xml, "DIAS_TRA_SEMANA", " ");//no se tiene el campo
                xml = Reemplazar(xml, "DIAS_DESCANSO", " ");//no se tiene el campo
                xml = Reemplazar(xml, "MONTO_CREDITO", credito_solicita != null ? credito_solicita.Monto_pago.ToString("C") : " ");//no se tiene el campo
                xml = Reemplazar(xml, "PLAZO_CREDITO", credito_solicita != null ? credito_solicita.Plazo.ToString() : " ");//no se tiene el campo
                xml = Reemplazar(xml, "FORMA_PAGO", credito_solicita != null ? credito_solicita.Periodo.Descripcion : " ");//no se tiene el campo
                xml = Reemplazar(xml, "DESTINO_CREDITO", " ");//no se tiene el campo
                xml = Reemplazar(xml, "SEMANAL_COL", " ");//no se tiene el campo
                xml = Reemplazar(xml, "QUINCENAL_COL", " ");//no se tiene el campo
            }
            /*estado laboral para datos familiares laboral*/
            Persona_Referencia referencia_conyuge = persona.Persona_Referencias.SingleOrDefault(a=>a.Parentesco=="1");
            if (referencia_conyuge != null && referencia_conyuge.Referencia != null)
            {
                xml = Reemplazar(xml, "NOMBRE_CONYUGE", referencia_conyuge.Referencia.Nombre);
                xml = Reemplazar(xml, "TELE_CONYUGE", referencia_conyuge.Referencia.Telefono);
                xml = Reemplazar(xml, "DIRECCION_TRABAJO_CONYUGE", referencia_conyuge.Referencia.Direccion_Empleo);
                xml = Reemplazar(xml, "CARGO_CONYUGE", referencia_conyuge.Referencia.Cargo_Empleo);
                xml = Reemplazar(xml, "SAL_CONYUGE", referencia_conyuge.Referencia.Salario_Empleo.ToString("C"));
                xml = Reemplazar(xml, "JEFE_IN_CONYUGE", referencia_conyuge.Referencia.Nombre_Jefe_Inmediato);
                xml = Reemplazar(xml, "N_T_CONY", referencia_conyuge.Referencia.Telefono_Empleo);
            }
            else {
                xml = Reemplazar(xml, "NOMBRE_CONYUGE", " ");
                xml = Reemplazar(xml, "TELE_CONYUGE", " ");
                xml = Reemplazar(xml, "DIRECCION_TRABAJO_CONYUGE", " ");
                xml = Reemplazar(xml, "CARGO_CONYUGE", " ");
                xml = Reemplazar(xml, "SAL_CONYUGE", " ");
                xml = Reemplazar(xml, "JEFE_IN_CONYUGE", " ");
                xml = Reemplazar(xml, "N_T_CONY", " ");
            }

            centinela = 0;
            if (persona.Persona_Referencias.Any(a => a.Tipo_Referencia == "1"))
                foreach (Persona_Referencia referencia_hijos in persona.Persona_Referencias.Where(a => a.Tipo_Referencia == "1" && (a.Parentesco=="2" || a.Parentesco=="3")))
                {
                    centinela += 1;
                    if (!string.IsNullOrEmpty(referencia_hijos.Parentesco) && (centinela == 1 || centinela == 2))
                    {
                        xml = Reemplazar(xml,string.Format("NOMBRE_HIJO{0}",centinela), referencia_hijos.Referencia.Nombre);
                        xml = Reemplazar(xml,string.Format("EDAD_HIJO{0}",centinela), referencia_hijos.Referencia.Edad.ToString());

                        if (referencia_hijos.Referencia.Esta_Estudiando)
                        {
                            xml = Reemplazar(xml, string.Format("E_H{0}", centinela), "X");
                            xml = Reemplazar(xml, string.Format("L_T_HIJO{0}", centinela), referencia_hijos.Referencia.Direccion_Estudio);
                        }
                        else
                            xml = Reemplazar(xml, string.Format("E_H{0}", centinela), " ");

                        if (referencia_hijos.Referencia.Esta_Trabajando)
                        {
                            xml = Reemplazar(xml, string.Format("T_H{0}", centinela), "X");
                            xml = Reemplazar(xml, string.Format("L_T_HIJO{0}", centinela), referencia_hijos.Referencia.Direccion_Empleo);
                        }
                        else
                            xml = Reemplazar(xml, string.Format("T_H{0}", centinela), " ");
                    }
                    else
                        break;
                }

            /*borrar las variables que no se utilizaron*/
            for (int i = centinela; i <= 4; i++)
            {
                centinela += 1;
                xml = Reemplazar(xml, string.Format("NOMBRE_HIJO{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("EDAD_HIJO{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("E_H{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("T_H{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("L_T_HIJO{0}", centinela), " ");
            }

            /*referencias del conyuge*/
            centinela = 0;
            if (persona.Persona_Referencias != null)
            {
                Func<Persona_Referencia, bool> parentesco = new Func<Persona_Referencia, bool>(a => a.Parentesco == "1");
                Referencia referencia_person = new Referencia();
                if (persona.Persona_Referencias.Any(parentesco))
                    referencia_person =
                      persona.Persona_Referencias
                      .SingleOrDefault(parentesco).Referencia;
                
                if (referencia_person != null)
                {
                    var referencia_adicinales = referencia_person.Referencias_Adicionales;
                    if (referencia_adicinales != null)
                    {
                        foreach (Referencia_Adicional adicional in referencia_adicinales.ToList())
                        {
                            centinela += 1;
                            if (centinela == 1 || centinela == 2)
                            {
                                xml = Reemplazar(xml, string.Format("NOMBRE_FAMILIAR_CONYUGE{0}", centinela), adicional.Nombre);
                                xml = Reemplazar(xml, string.Format("P_F_CONY{0}", centinela), adicional.Parentesco);
                                xml = Reemplazar(xml, string.Format("DIRECCION_FAMILIAR_CONYUGE{0}", centinela), adicional.Residencia);
                                xml = Reemplazar(xml, string.Format("T_F_CONY{0}", centinela), adicional.Telefono);
                            }
                            else
                                break;
                        }
                    }
                }
            }
            /*borrar las variables que no se utilizaron*/
            for (int i = centinela; i <= 2; i++)
            {
                centinela += 1;
                xml = Reemplazar(xml, string.Format("NOMBRE_FAMILIAR_CONYUGE{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("P_F_CONY{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("DIRECCION_FAMILIAR_CONYUGE{0}", centinela), " ");
                xml = Reemplazar(xml, string.Format("T_F_CONY{0}", centinela), " ");
            }

            /*datos no definidos de preguntas y preguntas de la declaración jurada*/
            xml = Reemplazar(xml, "SI_IG", " ");
            xml = Reemplazar(xml, "NO_IG", " ");

            xml = Reemplazar(xml, "DIAS_ASISTE", " ");
            xml = Reemplazar(xml, "HORAS_ASISTE", " ");

            xml = Reemplazar(xml, "SI_EN", " ");
            xml = Reemplazar(xml, "NO_EN", " ");

            xml = Reemplazar(xml, "ENFERMEDAD_CLIENTE", " ");

            xml = Reemplazar(xml, "SAL_O", " ");

            xml = Reemplazar(xml, "NEGOCIO_O", " ");

            xml = Reemplazar(xml, "SP_O", " ");

            xml = Reemplazar(xml, "OTR_O", " ");

            xml = Reemplazar(xml, "ESPECIFICAR_ACTIVIDAD_ECO_OR", " ");

            xml = Reemplazar(xml, "SI_OTRO_O", " ");
            xml = Reemplazar(xml, "NO_OTRO_O", " ");

            xml = Reemplazar(xml, "R_O", " ");

            xml = Reemplazar(xml, "PROV_O", " ");

            xml = Reemplazar(xml, "FRE_MOV_CUE", " ");

            xml = Reemplazar(xml, "USTED_O_TER", " ");

            xml = Reemplazar(xml, "SIFCT", " ");
            xml = Reemplazar(xml, "NOFCT", " ");

            xml = Reemplazar(xml, "SIROA", " ");
            xml = Reemplazar(xml, "NOROA", " ");

            xml = Reemplazar(xml, "SICPT", " ");
            xml = Reemplazar(xml, "NOCPT", " ");

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
