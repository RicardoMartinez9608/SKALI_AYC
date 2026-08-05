

using DocumentFormat.OpenXml.Packaging;
using ModelNucleo.Model;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ModelNucleo.CalculosYProceos;
using ModelNucleo.Model.ModelView;
using System.Collections.Generic;
using System.IO.Compression;
using SQLitePCL;
using AyCWeb.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace AyCWeb.Helper
{
    public class Generar_Documento
    {
        public System.IO.MemoryStream Generar_Mutuo(
        List<Persona> personas,
        Municipio_Departamento municipio = null,
        Departamento_Pais departamento = null,
        bool es_grupal = false,
        string nombreGrupal = "")
        {
            int no_integrantes = personas.Count();
            Persona persona = personas.FirstOrDefault();
            var primerCredito = persona.Creditos.FirstOrDefault();
            string codigo_credito = primerCredito.Codigo_Credito;

            // 1. REVISAMOS SI EL SOCIO TIENE O NO FIADORES REGISTRADOS EN LA BASE DE DATOS
            bool tieneCodeudoresEnBase = persona.Persona_Relaciones != null && persona.Persona_Relaciones.Any(a => a.Predicado != null);

            List<Tipo_Mutuo> mutuos = new List<Tipo_Mutuo>();

            // =========================================================================
            // 2. REGLA ESPECIAL Y PRIORITARIA PARA AGILMULTIDESTINO (Id_Linea == 19)
            // =========================================================================
            if (!es_grupal && primerCredito.Id_Linea == 19)
            {
                var todosLosMutuos = primerCredito.Linea_Credito.Tipo_Mutuos.ToList();

                foreach (var m in todosLosMutuos)
                {
                    if (tieneCodeudoresEnBase)
                    {
                        // Si el cliente TIENE codeudor -> Fuerza a cargar 'mutuoaycCC.docx'
                        m.Nombre_Extencion_Documento = "mutuoaycCC.docx";
                    }
                    else
                    {
                        // Si el cliente NO TIENE codeudor -> Fuerza a cargar 'mutuoayc.docx'
                        m.Nombre_Extencion_Documento = "mutuoayc.docx";
                    }
                }

                mutuos = todosLosMutuos;
            }
            // =========================================================================
            // 3. REGLAS PARA LAS DEMÁS LÍNEAS DE CRÉDITO
            // =========================================================================
            else if (es_grupal)
            {
                mutuos = primerCredito.Linea_Credito.Tipo_Mutuos
                    .Where(t => t.Integrantes_Grupal == no_integrantes).ToList();
            }
            else if (primerCredito.mutuo_dos_codeudores_dos_fechas)
            {
                mutuos = primerCredito.Linea_Credito.Tipo_Mutuos
                   .Where(t => t.Dos_Codeudores_Dos_Fechas == true).ToList();
            }
            else if (primerCredito.mutuo_dos_fechas && primerCredito.Id_Linea != 13
                     && primerCredito.Id_Linea != 12 && primerCredito.Id_Linea != 9 && primerCredito.Id_Linea != 2
                     && primerCredito.Id_Linea != 17 && primerCredito.Id_Linea != 19 && primerCredito.Id_Linea != 20 && primerCredito.Id_Linea != 21)
            {
                mutuos = primerCredito.Linea_Credito.Tipo_Mutuos
                   .Where(t => t.Dos_Fechas == true).ToList();
            }
            else if (primerCredito.mutuo_dos_codeudores)
            {
                mutuos = primerCredito.Linea_Credito.Tipo_Mutuos
                   .Where(t => t.Dos_Codeudores == true).ToList();
            }
            else
            {
                // Créditos individuales genéricos
                var todosLosMutuos = primerCredito.Linea_Credito.Tipo_Mutuos
                    .Where(a => !a.Dos_Codeudores && !a.Dos_Codeudores_Dos_Fechas && !a.Dos_Fechas)
                    .ToList();

                if (!tieneCodeudoresEnBase)
                {
                    foreach (var m in todosLosMutuos)
                    {
                        string nombreMinusculas = m.Nombre_Extencion_Documento.ToLower().Trim();

                        if (nombreMinusculas.Contains("mutuoaycplus"))
                        {
                            m.Nombre_Extencion_Documento = m.Nombre_Extencion_Documento;
                        }
                        else if (nombreMinusculas.Contains("mutuoaycfah"))
                        {
                            m.Nombre_Extencion_Documento = "MutuoaycAH.docx";
                        }
                    }
                }

                mutuos = todosLosMutuos;
            }

            // =========================================================================
            // 4. CONSTRUCCIÓN DEL ARCHIVO ZIP CON LOS DOCUMENTOS GENERADOS
            // =========================================================================
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    if (mutuos == null || !mutuos.Any())
                    {
                        var entryFalsa = zip.CreateEntry("ALERTA_No_Se_Encontraron_Mutuos.txt");
                        using (StreamWriter swFalso = new StreamWriter(entryFalsa.Open()))
                        {
                            swFalso.Write("La lista 'mutuos' llegó vacía al generador debido a los filtros IF.");
                        }
                    }
                    else
                    {
                        foreach (Tipo_Mutuo tipo in mutuos)
                        {
                            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template\mutuo", tipo.Nombre_Extencion_Documento);
                            string documento_destino = Completa_Nombre_Aleatorio(codigo_credito, tipo);

                            using (MemoryStream documentStream = new MemoryStream())
                            {
                                using (FileStream sourceStream = new FileStream(documento_fuente, FileMode.Open, FileAccess.Read))
                                {
                                    sourceStream.CopyTo(documentStream);
                                }
                                documentStream.Seek(0, SeekOrigin.Begin);

                                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documentStream, true))
                                {
                                    string docText;
                                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                                    {
                                        docText = sr.ReadToEnd();
                                    }

                                    docText = LimpiarEtiquetasXml(docText);

                                    if (es_grupal)
                                        docText = Cargar_Datos_Grupal(personas, docText, municipio, departamento, nombreGrupal);
                                    else
                                        docText = Cargar_Datos(persona, docText, municipio, departamento);

                                    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                                    {
                                        sw.Write(docText);
                                        sw.Flush();
                                    }
                                }

                                documentStream.Seek(0, SeekOrigin.Begin);
                                var zipEntry = zip.CreateEntry(documento_destino);

                                using (Stream entryStream = zipEntry.Open())
                                {
                                    documentStream.CopyTo(entryStream);
                                    entryStream.Flush();
                                }
                            }
                        }
                    }
                }

                byte[] bytesFinales = zipStream.ToArray();
                return new MemoryStream(bytesFinales);
            }
        }




        private string Completa_Nombre_Aleatorio(string directory, Tipo_Mutuo tipo)
        {
            string con_huella = tipo.Huella ? "Con_Huella" : "";
            string copia_original = tipo.Copia ? "Copia" : "Original";
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = string.Format("{0}{1}{2}", copia_original, con_huella, DateTime.Now.ToString("MMddyyyyHmmss"));
            return string.Format(@"c:\\Documentos_Mutuos\{0}\{1}.docx", directory, nombre_archivo_unico);
        }

        private string Cargar_Datos(Persona persona, string xml, Municipio_Departamento municipio = null, Departamento_Pais departamento = null)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();
            Credito credito = persona.Creditos.FirstOrDefault();

            /* La persona como tal */
            xml = Reemplazar(xml, "«nombredelsocio»", " " + persona.NombreCompleto + " ");
            xml = Reemplazar(xml, "«Cedad»", " " + string.Format(conversion.Enletras(persona.Edad.ToString())) + " ");
            xml = Reemplazar(xml, "«codigo_credito»", " " + credito.Codigo_Credito + " ");
            xml = Reemplazar(xml, "«profesion_cli»", " " + persona.Profesion + " ");

            if (municipio != null && municipio.Nombre_Municipio != null)
            {
                xml = Reemplazar(xml, "«dir_casa»", " " + municipio.Nombre_Municipio + " ");
                xml = Reemplazar(xml, "«ciudad»", " " + credito.Nombre_Ciudad_Mutuo + " ");
            }

            xml = Reemplazar(xml, "«DuiEnLetras»", " " + string.Format(documentos.Enletras(persona.Dui)) + " ");

            if (departamento != null && departamento.Nombre_Departamento != null)
            {
                xml = Reemplazar(xml, "«mciudad»", " " + string.Format(departamento.Nombre_Departamento) + " ");
            }
            xml = Reemplazar(xml, "«tplazo»", " " + "MENSUAL" + " ");

            /* Fechas del crédito */
            DateTime fecha = credito.Vencido_En;
            DateTime fecha_dia = credito.Liquidado_En;
            DateTime fecha_primera_cuota = credito.Fecha_Primer_Pago;
            DateTime fecha_hoy = DateTime.Now.Date;

            // Primera Cuota
            string nueva_primera_cuota = fecha_primera_cuota.ToLongDateString();
            string[] valores_primera = nueva_primera_cuota.Split(" ");
            string dia_p = string.Format(conversion.Enletras(valores_primera[1]));
            string año_p = string.Format(conversion.Enletras(valores_primera[5]));
            string dia_fecha_primer = valores_primera[0].Replace(",", string.Empty);
            string fecha_nueva_p = dia_fecha_primer + " " + dia_p.ToLower() + " de " + valores_primera[3] + " de " + año_p.ToLower();
            xml = Reemplazar(xml, "«Cpripag»", " " + string.Format(fecha_nueva_p.ToUpper()) + " ");

            // Apertura
            string nueva_F = fecha_dia.ToLongDateString();
            string[] valores = nueva_F.Split(" ");
            string dia = string.Format(conversion.Enletras(valores[1]));
            string año = string.Format(conversion.Enletras(valores[5]));
            string dia_fecha_ = valores[0].Replace(",", string.Empty);
            string fecha_nueva = dia_fecha_ + " " + dia.ToLower() + " de " + valores[3] + " de " + año.ToLower();
            xml = Reemplazar(xml, "«aperturaC»", " " + fecha_nueva.ToUpper() + " ");

            // Vencimiento
            string fecha_vencimiento = fecha.ToLongDateString();
            string[] valores_vencimiento = fecha_vencimiento.Split(" ");
            string dia_vencimiento = string.Format(conversion.Enletras(valores_vencimiento[1]));
            string año_vencimiento = string.Format(conversion.Enletras(valores_vencimiento[5]));
            string dia_fecha = valores_vencimiento[0].Replace(",", string.Empty);
            string fecha_nueva_vencimiento = dia_fecha + " " + dia_vencimiento.ToLower() + " de " + valores_vencimiento[3] + " de " + año_vencimiento.ToLower();
            xml = Reemplazar(xml, "«Cfechavenc»", " " + fecha_nueva_vencimiento.ToUpper() + " ");

            // Fecha Hoy
            string fecha_ahora = fecha_hoy.ToLongDateString();
            string[] valores_ahora = fecha_ahora.Split(" ");
            string dia_ahora = string.Format(conversion.Enletras(valores_ahora[1]));
            string año_ahora = string.Format(conversion.Enletras(valores_ahora[5]));
            string dia_fecha_ahora = valores_ahora[0].Replace(",", string.Empty);
            string fecha_nueva_ahora = dia_fecha_ahora + " " + dia_ahora.ToLower() + " de " + valores_ahora[3] + " de " + año_ahora.ToLower();
            xml = Reemplazar(xml, "«Cfechahoy»", " " + fecha_nueva_ahora.ToUpper() + " ");

            /* Montos y Plazos */
            if (credito.Id_Linea != 9)
            {
                xml = Reemplazar(xml, "«CMontoPrestamo»", "*** " + string.Format(conversion.Enletras(decimal.Round(credito.Monto_pago, 2).ToString())) + " 00/100" + " ***");
            }
            else if (credito.Id_Linea == 9)
            {
                decimal monto_diferente = credito.Monto_pago * 3;
                xml = Reemplazar(xml, "«CMontoRot»", "*** " + string.Format(conversion.Enletras(decimal.Round(monto_diferente, 2).ToString())) + " 00/100" + " ***");
            }

            xml = Reemplazar(xml, "«Cmeses»", " " + string.Format(conversion.Enletras(credito.Plazo.ToString())) + " ");

            // CORRECCIÓN CUOTAS: Se unificó en un condicional estructurado para evitar que se sobreescriba abajo
            if (credito.Id_Linea == 5 || credito.Id_Linea == 19 || credito.Id_Linea == 20 || credito.Id_Linea == 21)
            {
                xml = Reemplazar(xml, "«Ccuotas»", " " + string.Format(conversion.Enletras(credito.Numero_cuotas.ToString())) + " ");
                xml = Reemplazar(xml, "«fpago»", " " + credito.Periodo.Descripcion.ToUpper() + "ES" + " ");
            }
            else
            {
                xml = Reemplazar(xml, "«Ccuotas»", " " + string.Format(conversion.Enletras(credito.Plazo.ToString())) + " ");
            }

            // Validar tipos de mutuo
            bool mutuo_dosfechas_doscodeudores = persona.Creditos.Any(a => a.mutuo_dos_codeudores_dos_fechas);
            bool mutuo_dosfechas = persona.Creditos.Any(a => a.mutuo_dos_fechas);
            bool mutuo_doscodeudores = persona.Creditos.Any(a => a.mutuo_dos_codeudores);

            if (mutuo_dosfechas_doscodeudores || mutuo_dosfechas || mutuo_doscodeudores)
            {
                xml = Reemplazar(xml, "«cuotsc»", " " + string.Format(conversion.Enletras(Math.Round(credito.Cuota_Menos_Plan, 2).ToString())) + " ");
                xml = Reemplazar(xml, "«cuotsc2»", " " + string.Format(conversion.Enletras(Math.Round(credito.Primer_Pago, 2).ToString())) + " ");
                xml = Reemplazar(xml, "«Ccuota2»", " " + string.Format(conversion.Enletras(Math.Round(credito.Primer_Pago, 2).ToString())) + " ");
                xml = Reemplazar(xml, "«Ccuota»", " " + string.Format(conversion.Enletras(Math.Round(credito.Segundo_Pago, 2).ToString())) + " ");
            }
            else
            {
                xml = Reemplazar(xml, "«cuotsc»", " " + string.Format(conversion.Enletras(Math.Round(credito.Cuota_Menos_Plan, 2).ToString())) + " ");
                xml = Reemplazar(xml, "«Ccuota»", " " + string.Format(conversion.Enletras(Math.Round(credito.Cuota_Total, 2).ToString())) + " ");
            }

            xml = Reemplazar(xml, "«qcob»", " " + string.Format(conversion.Enletras(Math.Round(credito.Plan_AyC, 2).ToString())) + " ");
            xml = Reemplazar(xml, "«seguro»", " " + string.Format(conversion.Enletras(Math.Round(credito.Plan_AyC, 2).ToString())) + " ");
            xml = Reemplazar(xml, "«Fecha_P»", "No se posee");

            /* RELACIONES / CODEUDORES (ESTRUCTURADO CON IF / ELSE IF PARA QUE NO SE PISEN LOS FIADORES) */
            if (persona.Persona_Relaciones != null && persona.Persona_Relaciones.Any())
            {
                int totalRelaciones = persona.Persona_Relaciones.Count(a => a.Predicado != null);

                if (totalRelaciones >= 2)
                {
                    // Caso: Dos Fiadores
                    Persona codeudor1 = persona.Persona_Relaciones.ElementAt(0).Predicado;
                    if (codeudor1 != null)
                    {
                        xml = Reemplazar(xml, "«NombredelFiador»", " " + codeudor1.NombreCompleto + " ");
                        xml = Reemplazar(xml, "«edadf»", " " + string.Format(conversion.Enletras(codeudor1.Edad.ToString())) + " ");
                        xml = Reemplazar(xml, "«dirfiador»", " " + (municipio?.Nombre_Municipio ?? "") + " ");
                        xml = Reemplazar(xml, "«fDuiEnLetras»", " " + documentos.Enletras(codeudor1.Dui) + " ");
                        xml = Reemplazar(xml, "«fNitEnLetras»", " " + documentos.Enletras(codeudor1.Nit) + " ");
                        if (municipio?.Nombre_Municipio != null)
                        {
                            xml = Reemplazar(xml, "«ciudad»", " " + string.Format(municipio.Nombre_Municipio) + " ");
                        }
                    }

                    Persona codeudor2 = persona.Persona_Relaciones.ElementAt(1).Predicado;
                    if (codeudor2 != null)
                    {
                        xml = Reemplazar(xml, "«NombredelFiador2»", " " + codeudor2.NombreCompleto + " ");
                        xml = Reemplazar(xml, "«edadf2»", " " + string.Format(conversion.Enletras(codeudor2.Edad.ToString())) + " ");
                        xml = Reemplazar(xml, "«dirfiador2»", " " + (municipio?.Nombre_Municipio ?? "") + " ");
                        xml = Reemplazar(xml, "«fDuiEnLetras2»", " " + documentos.Enletras(codeudor2.Dui) + " ");
                        xml = Reemplazar(xml, "«fNitEnLetras2»", " " + documentos.Enletras(codeudor2.Nit) + " ");
                        if (municipio?.Nombre_Municipio != null)
                        {
                            xml = Reemplazar(xml, "«mciudad2»", " " + string.Format(municipio.Nombre_Municipio) + " ");
                        }
                    }
                }
                else // Caso: Un solo fiador
                {
                    Persona codeudor = persona.Persona_Relaciones.FirstOrDefault()?.Predicado;
                    if (codeudor != null)
                    {
                        xml = Reemplazar(xml, "«NombredelFiador»", " " + codeudor.NombreCompleto + " ");
                        xml = Reemplazar(xml, "«edadf»", " " + string.Format(conversion.Enletras(codeudor.Edad.ToString())) + " ");
                        xml = Reemplazar(xml, "«dirfiador»", " " + (municipio?.Nombre_Municipio ?? "") + " ");
                        xml = Reemplazar(xml, "«fDuiEnLetras»", " " + documentos.Enletras(codeudor.Dui) + " ");
                        xml = Reemplazar(xml, "«fNitEnLetras»", " " + documentos.Enletras(codeudor.Nit) + " ");
                        if (municipio?.Nombre_Municipio != null)
                        {
                            xml = Reemplazar(xml, "«ciudad»", " " + string.Format(credito.Nombre_Ciudad_Mutuo) + " ");
                        }
                    }
                }
            }
            else
            {
                // Valores por defecto si no hay relaciones
                xml = Reemplazar(xml, "Nombre_Completo_Codeudor", "No se posee ");
                xml = Reemplazar(xml, "Edad_Codeudor_Letras", "No se posee ");
                xml = Reemplazar(xml, "Departamento_Domicilio_Codeudor", "No se posee ");
                xml = Reemplazar(xml, "Dui_Codeudor", "No se posee ");
                xml = Reemplazar(xml, "Nit_Codeudor", "No se posee ");
                xml = Reemplazar(xml, "Ciudad_Celebrado_Mutuo", " ");
            }

            return xml;
        }
        private List<ParametroPersonaDocumento> ListarParametrosPersonales()
        {
            /*registro de datos enumerados*/
            List<ParametroPersonaDocumento> parametrosPersonales = new List<ParametroPersonaDocumento>();
            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 1,
                NombreCompleto = "«nombredelsocio»",
                Edad = "«Cedad»",
                DireccionCasa = "«dir_casa»",
                Ciudad = "«ciudad»",
                Departamento = "«mciudad»",
                DUI = "«DuiEnLetras»",
                NIT = "«NitEnLetras»",
                MontoCredito = "«CMontoPrestamo»"
            });

            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 2,
                NombreCompleto = "«nombredelsocio2»",
                Edad = "«Cedad2»",
                DireccionCasa = "«dir_casa2»",
                Ciudad = "«ciudad2»",
                Departamento = "«mciudad2»",
                DUI = "«DuiEnLetras2»",
                NIT = "«NitEnLetras2»",
                MontoCredito = "«CMontoPrestamo2»"
            });

            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 3,
                NombreCompleto = "«nombredelsocio3»",
                Edad = "«Cedad3»",
                DireccionCasa = "«dir_casa3»",
                Ciudad = "«ciudad3»",
                Departamento = "«mciudad3»",
                DUI = "«DuiEnLetras3»",
                NIT = "«NitEnLetras3»",
                MontoCredito = "«CMontoPrestamo3»"
            });

            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 4,
                NombreCompleto = "«nombredelsocio4»",
                Edad = "«Cedad4»",
                DireccionCasa = "«dir_casa4»",
                Ciudad = "«ciudad4»",
                Departamento = "«mciudad4»",
                DUI = "«DuiEnLetras4»",
                NIT = "«NitEnLetras4»",
                MontoCredito = "«CMontoPrestamo4»"
            });

            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 5,
                NombreCompleto = "«nombredelsocio5»",
                Edad = "«Cedad5»",
                DireccionCasa = "«dir_casa5»",
                Ciudad = "«ciudad5»",
                Departamento = "«mciudad5»",
                DUI = "«DuiEnLetras5»",
                NIT = "«NitEnLetras5»",
                MontoCredito = "«CMontoPrestamo5»"
            });

            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 6,
                NombreCompleto = "«nombredelsocio6»",
                Edad = "«Cedad6»",
                DireccionCasa = "«dir_casa6»",
                Ciudad = "«ciudad6»",
                Departamento = "«mciudad6»",
                DUI = "«DuiEnLetras6»",
                NIT = "«NitEnLetras6»",
                MontoCredito = "«CMontoPrestamo6»"
            });

            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 7,
                NombreCompleto = "«nombredelsocio7»",
                Edad = "«Cedad7»",
                DireccionCasa = "«dir_casa7»",
                Ciudad = "«ciudad7»",
                Departamento = "«mciudad7»",
                DUI = "«DuiEnLetras7»",
                NIT = "«NitEnLetras7»",
                MontoCredito = "«CMontoPrestamo7»"
            });

            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 8,
                NombreCompleto = "«nombredelsocio8»",
                Edad = "«Cedad8»",
                DireccionCasa = "«dir_casa8»",
                Ciudad = "«ciudad8»",
                Departamento = "«mciudad8»",
                DUI = "«DuiEnLetras8»",
                NIT = "«NitEnLetras8»",
                MontoCredito = "«CMontoPrestamo8»"
            });

            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 9,
                NombreCompleto = "«nombredelsocio9»",
                Edad = "«Cedad9»",
                DireccionCasa = "«dir_casa9»",
                Ciudad = "«ciudad9»",
                Departamento = "«mciudad9»",
                DUI = "«DuiEnLetras9»",
                NIT = "«NitEnLetras9»",
                MontoCredito = "«CMontoPrestamo9»"
            });
            parametrosPersonales.Add(new ParametroPersonaDocumento
            {
                No = 10,
                NombreCompleto = "«nombredelsocio10»",
                Edad = "«Cedad10»",
                DireccionCasa = "«dir_casa10»",
                Ciudad = "«ciudad10»",
                Departamento = "«mciudad10»",
                DUI = "«DuiEnLetras10»",
                NIT = "«NitEnLetras10»",
                MontoCredito = "«CMontoPrestamo10»"
            });
            return parametrosPersonales;
        }

        private string Cargar_Datos_Grupal(
            List<Persona> personas,
            string xml,
            Municipio_Departamento municipio = null,
            Departamento_Pais departamento = null,
            string nombreGrupal = "")
        {

            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();

            /*La persona como tal*/
            List<OrdenarPersona> ordenarPersonas = new List<OrdenarPersona>();
            int centinela = 0;
            foreach (var persona in personas)
            {
                centinela++;
                ordenarPersonas.Add(new OrdenarPersona
                {
                    No = centinela,
                    Persona = persona
                });
            }
            /*registro de datos enumerados*/
            List<ParametroPersonaDocumento> parametrosPersonales = ListarParametrosPersonales();

            foreach (var ordenar in ordenarPersonas)
            {
                var parametro = parametrosPersonales.Single(d => d.No == ordenar.No);
                xml = Reemplazar(xml, parametro.NombreCompleto, " " + ordenar.Persona.NombreCompleto + " ");
                xml = Reemplazar(xml, parametro.Edad, " " + string.Format(conversion.Enletras(ordenar.Persona.Edad.ToString())) + " ");
                if (municipio.Nombre_Municipio != null)
                {
                    xml = Reemplazar(xml, parametro.DireccionCasa, " " + municipio.Nombre_Municipio + " ");
                    //xml = Reemplazar(xml, parametro.Ciudad, " " + municipio.Nombre_Municipio + " ");
                }

                xml = Reemplazar(xml, parametro.DUI, " " + string.Format(documentos.Enletras(ordenar.Persona.Dui)) + " ");
                xml = Reemplazar(xml, parametro.NIT, " " + string.Format(documentos.Enletras(ordenar.Persona.Nit)) + " ");
                if (departamento.Nombre_Departamento != null)
                {
                    xml = Reemplazar(xml, parametro.Departamento, " " + string.Format(departamento.Nombre_Departamento) + " ");
                }
                xml = Reemplazar(xml, parametro.MontoCredito, "*** " + string.Format(conversion.Enletras(decimal.Round(ordenar.Persona.Creditos.FirstOrDefault().Monto_pago, 2).ToString())) + " 00/100" + " ***");
            }
            Credito credito = personas.FirstOrDefault().Creditos.FirstOrDefault();
            xml = Reemplazar(xml, "«Cmeses»", " " + string.Format(conversion.Enletras(credito.Plazo.ToString())) + " ");
            string fecha_vencimiento = FechaLetrada(credito.Vencido_En);
            DateTime fecha_dia = credito.Liquidado_En;
            string nueva_F = fecha_dia.ToLongDateString();
            string[] valores = nueva_F.Split(" ");
            string dia = string.Format(conversion.Enletras(valores[1]));
            string año = string.Format(conversion.Enletras(valores[5]));
            string dia_fecha_ = valores[0].Replace(",", string.Empty);
            string fecha_nueva = dia_fecha_ + " " + dia.ToLower() + " de " + valores[3] + " de " + año.ToLower();
            xml = Reemplazar(xml, "«Cfechavenc»", " " + fecha_vencimiento.ToUpper() + " ");
            xml = Reemplazar(xml, "«aperturaC»", " " + fecha_nueva.ToUpper() + " "); /*Cambiar la fecha a letras*/
            xml = Reemplazar(xml, "«Ccuotas»", " " + string.Format(conversion.Enletras(credito.Numero_cuotas.ToString())) + " ");
            xml = Reemplazar(xml, "«cuotsc»", " " + string.Format(conversion.Enletras(Math.Round(credito.Cuota_Menos_Plan, 2).ToString())) + " ");
            DateTime fecha_primera_cuota = credito.Fecha_Primer_Pago;
            DateTime fecha_hoy = DateTime.Now.Date;
            string nueva_primera_cuota = FechaLetrada(fecha_primera_cuota);
            xml = Reemplazar(xml, "«Cpripag»", " " + string.Format(nueva_primera_cuota.ToUpper()) + " ");
            xml = Reemplazar(xml, "«fpago»", " " + credito.Periodo.Descripcion.ToUpper() + "ES" + " ");
            string fechaLargaHoy = FechaLetrada(fecha_hoy);
            xml = Reemplazar(xml, "«Cfechahoy»", " " + string.Format(fechaLargaHoy.ToUpper()) + " ");

            if (municipio.Nombre_Municipio != null)
                xml = Reemplazar(xml, "«ciudad»", " " + municipio.Nombre_Municipio + " ");

            decimal TodoMontoCredito = personas.SelectMany(c => c.Creditos).Sum(x => x.Monto_pago);
            xml = Reemplazar(xml, "«CMontoTPrestamos»", "*** " + string.Format(conversion.Enletras(decimal.Round(TodoMontoCredito, 2).ToString())) + " 00/100" + " ***");
            xml = Reemplazar(xml, "«Gsolidario»", " " + nombreGrupal + " ");

            return xml;
        }

        private string FechaLetrada(DateTime fecha)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            string nueva_primera_cuota = fecha.ToLongDateString();
            string[] valores_primera = nueva_primera_cuota.Split(" ");
            string dia_p = string.Format(conversion.Enletras(valores_primera[1]));
            string año_p = string.Format(conversion.Enletras(valores_primera[5]));
            string dia_fecha_primer = valores_primera[0].Replace(",", string.Empty);
            return dia_fecha_primer + " " + dia_p.ToLower() + " de " + valores_primera[3] + " de " + año_p.ToLower();
        }

        //---------------------------------------------------- MUTUO COPIA----------------------------------------------------------------------
        public MemoryStream Generar_Mutuo_Copia(Persona persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "mutuo_copia.docx");
            string documento_destino = Completa_Nombre_Aleatorio_MutuoCopia();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_MutuoCopia(persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_MutuoCopia()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_MutuoCopia(Persona persona, string xml)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();

            /*La persona como tal*/
            xml = Reemplazar(xml, "Nombre_Completo_Cliente", persona.NombreCompleto);
            xml = Reemplazar(xml, "Edad_Cliente_Letras", persona.Edad.ToString());/*Se debe cambiar a Letras*/
            xml = Reemplazar(xml, "Ocupacion_Cliente", persona.Ocupacion);
            xml = Reemplazar(xml, "Domicilio_Cliente", "Desconocido");/*Falta la residencia*/
            xml = Reemplazar(xml, "Departamento_Domicilio_Cliente", "Desconocido");
            xml = Reemplazar(xml, "Dui_Cliente", string.Format(documentos.Enletras(persona.Dui)));
            xml = Reemplazar(xml, "NitCOpia", string.Format(documentos.Enletras(persona.Nit)));

            /*el crédito de la persona*/
            Credito credito = persona.Creditos.FirstOrDefault();
            xml = Reemplazar(xml, "montoPres", string.Format(conversion.Enletras(decimal.Round(credito.Monto_pago, 2).ToString())));/*Debe cambiarse a letras*/
            xml = Reemplazar(xml, "Plazo_Prestamo_Letras", string.Format(conversion.Enletras(credito.Plazo.ToString()))); /*También debe ir en letras*/
            xml = Reemplazar(xml, "DesemFecha", credito.Liquidado_En.ToLongDateString()); /*Cambiar la fecha a letras*/
            xml = Reemplazar(xml, "VencimientoPres", credito.Vencido_En.ToLongDateString()); /*tambien debe estar en letras*/
            xml = Reemplazar(xml, "Frecuencia_Pago_Prestamo_Letras", credito.Periodo.Descripcion);
            xml = Reemplazar(xml, "Valor_Pago_Prestamo_Letras", string.Format(conversion.Enletras(decimal.Round(credito.Valor_Cuota, 2).ToString()))); /*Se debe cambiar a letras*/
            xml = Reemplazar(xml, "PlanaycPago", string.Format(conversion.Enletras(decimal.Round(credito.Interes, 2).ToString())));/*Se debe poner en letras*/
            xml = Reemplazar(xml, "Pago_Mas_Plan_AyC_Letras", string.Format(conversion.Enletras(decimal.Round(credito.Valor_Cuota, 2).ToString()))); /*falta que se ponga en letra*/
            xml = Reemplazar(xml, "FPago", "No se posee");/*debe cambiarse a letras*/
            if (persona.Persona_Relaciones != null)
            {
                /*Todos los datos del codeudor*/
                Persona codeudor = persona.Persona_Relaciones.FirstOrDefault().Predicado;

                xml = Reemplazar(xml, "Nombre_Completo_Codeudor", codeudor.NombreCompleto);
                xml = Reemplazar(xml, "Edad_Codeudor_Letras", codeudor.Edad.ToString());
                xml = Reemplazar(xml, "Departamento_Domicilio_Codeudor", "No se posee");
                xml = Reemplazar(xml, "Dui_Codeudor", codeudor.Dui);
                xml = Reemplazar(xml, "Nit_Codeudor", codeudor.Nit);

                xml = Reemplazar(xml, "Fecha_Celebrado_Mutuo", credito.Liquidado_En.ToString());
                xml = Reemplazar(xml, "Ciudad_Celebrado_Mutuo", "");
            }
            else
            {
                xml = Reemplazar(xml, "Nombre_Completo_Codeudor", "No se posee");
                xml = Reemplazar(xml, "Edad_Codeudor_Letras", "No se posee");
                xml = Reemplazar(xml, "Departamento_Domicilio_Codeudor", "No se posee");
                xml = Reemplazar(xml, "Dui_Codeudor", "No se posee");
                xml = Reemplazar(xml, "Nit_Codeudor", "No se posee");

                xml = Reemplazar(xml, "«Cfechahoy»", "");
                xml = Reemplazar(xml, "Ciudad_Celebrado_Mutuo", "No se posee");
            }


            return xml;
        }



        //---------------------------------------PAGARE-----------------------------------------------------------------------------------------
        public MemoryStream Generar_Pagare(Persona persona, Municipio_Departamento municipio = null, Departamento_Pais departamento = null)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "QPAGARE.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Pagare();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Pagare(persona, docText, municipio, departamento);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }
        //-------------------------------------------------- PAGARE_CODEUDOR -------------------------------------------------------
        public MemoryStream GenerarPagareCodeudor(PagarCodeudor data)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "PAGAREC.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Pagare();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_PagareC(data, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }
        private string Completa_Nombre_Aleatorio_Pagare()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Pagare(Persona persona, string xml, Municipio_Departamento municipio = null, Departamento_Pais departamento = null)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();
            var fecha = DateTime.Today;
            string nueva_F = fecha.ToLongDateString();
            string[] valores = nueva_F.Split(" ");
            string dia = string.Format(conversion.Enletras(valores[1]));
            string año = string.Format(conversion.Enletras(valores[5]));

            /*La persona como tal*/
            xml = Reemplazar(xml, "«dia»", dia.ToLower());
            xml = Reemplazar(xml, "«mes»", valores[3]);
            xml = Reemplazar(xml, "«ano»", año.ToLower());
            xml = Reemplazar(xml, "«nombredelsocio»", persona.NombreCompleto);
            xml = Reemplazar(xml, "«profecion»", persona.Profesion);

            xml = Reemplazar(xml, "«DuiEnLetras»", string.Format(documentos.Enletras(persona.Dui)));
            xml = Reemplazar(xml, "«NitEnLetras»", string.Format(documentos.Enletras(persona.Nit)));
            xml = Reemplazar(xml, "Fecha_Pagare", persona.Comentario);
            /*el crédito de la persona*/
            Credito credito = persona.Creditos.Last();
            xml = Reemplazar(xml, "«CMontoPrestamo»", string.Format(conversion.Enletras(decimal.Round(credito.Monto_pago, 2).ToString())));
            string fecha_ahora = credito.Vencido_En.ToLongDateString();
            string[] valores_ahora = fecha_ahora.Split(" ");
            string dia_ahora = string.Format(conversion.Enletras(valores_ahora[1]));
            string año_ahora = string.Format(conversion.Enletras(valores_ahora[5]));
            string dia_fecha_ahora = valores_ahora[0].Replace(",", string.Empty);
            string fecha_nueva_ahora = dia_fecha_ahora + " " + dia_ahora.ToLower() + " de " + valores_ahora[3] + " de " + año_ahora.ToLower();/*Debe cambiarse a letras*/
            xml = Reemplazar(xml, "«Cfechavenc»", fecha_nueva_ahora.ToUpper()); /*tambien debe estar en letras*/
            xml = Reemplazar(xml, "«tasaa»", string.Format(conversion.Enletras(decimal.Round(credito.Interes_Comision, 2).ToString()))); /*tambien debe estar en letras*/
            xml = Reemplazar(xml, "«scodigo»", credito.Codigo_Credito);
            if (municipio.Nombre_Municipio != null)
            {
                xml = Reemplazar(xml, "«dir_casa»", " " + credito.Nombre_Ciudad_Mutuo + " ");/*Falta la residencia*/
            }
            if (departamento.Nombre_Departamento != null)
            {
                xml = Reemplazar(xml, "«mciudad»", credito.Nombre_Ciudad_Mutuo);
            }

            xml = Reemplazar(xml, "«ciudad»", credito.Nombre_Ciudad_Mutuo);

            xml = Reemplazar(xml, "«dui2»", string.Format(persona.Dui));
            xml = Reemplazar(xml, "«nit2»", string.Format(persona.Nit));

            return xml;
        }
        // calendario de pagos
        public MemoryStream DescargarCalendarioPagos(CalendarioPago data)
        {
            // Ruta del archivo Excel fuente
            string rutaFuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "calendario_de_pagos.xlsx");
            string documentoDestino = Completa_Nombre_Aleatorio_Pagare();
            File.Copy(rutaFuente, documentoDestino, true);

            // Abrir el archivo Excel copiado para modificarlo
            using (SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Open(documentoDestino, true))
            {
                // Seleccionar la primera hoja del libro
                WorkbookPart workbookPart = spreadsheetDoc.WorkbookPart;
                Sheet sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().FirstOrDefault();
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Asignar los valores del objeto CalendarioPago a celdas específicas
                SetCellValue(worksheetPart, "D42", data.codigoCredito);
                SetCellValue(worksheetPart, "L42", data.nombreCliente);
                SetCellValue(worksheetPart, "M43", Math.Round(data.valorCuota, 2).ToString()); // Redondear a dos decimales
                SetCellValue(worksheetPart, "G62", data.asesor);

                // Para las fechas de pago y descripciones, asignar valores a celdas específicas
                int startRow = 7; // Empieza en la fila 7
                foreach (var fechaPago in data.fechas.OrderByDescending(f => f.fechaPago))
                {
                    string fechaCellReference = $"Y{startRow}";
                    string descripcionCellReference = $"Z{startRow}";

                    SetCellValue(worksheetPart, fechaCellReference, fechaPago.fechaPago);
                    SetCellValue(worksheetPart, descripcionCellReference, fechaPago.descripcion);

                    startRow++;
                }

                // Guardar los cambios
                worksheetPart.Worksheet.Save();
                workbookPart.Workbook.Save();
            }

            // Leer el archivo modificado en un MemoryStream
            MemoryStream documentStream = new MemoryStream();
            using (FileStream fileStream = new FileStream(documentoDestino, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(documentStream);
            }
            documentStream.Position = 0; // Reiniciar la posición del stream a cero antes de devolverlo

            return documentStream;
        }

        private void SetCellValue(WorksheetPart worksheetPart, string cellReference, string value)
        {
            var cell = GetCell(worksheetPart.Worksheet, cellReference);

            if (cell == null)
            {
                cell = InsertCellInWorksheet(worksheetPart.Worksheet, cellReference);
            }

            cell.CellValue = new CellValue(value);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            worksheetPart.Worksheet.Save();
        }

        private Cell GetCell(Worksheet worksheet, string cellReference)
        {
            var sheetData = worksheet.GetFirstChild<SheetData>();
            var row = GetRow(sheetData, GetRowIndex(cellReference));

            if (row != null)
            {
                return row.Elements<Cell>().FirstOrDefault(c => string.Compare(c.CellReference.Value, cellReference, true) == 0);
            }

            return null;
        }

        private Row GetRow(SheetData sheetData, uint rowIndex)
        {
            return sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex) ?? CreateRow(sheetData, rowIndex);
        }

        private Row CreateRow(SheetData sheetData, uint rowIndex)
        {
            Row row = new Row() { RowIndex = rowIndex };
            sheetData.Append(row);
            return row;
        }

        private Cell InsertCellInWorksheet(Worksheet worksheet, string cellReference)
        {
            var sheetData = worksheet.GetFirstChild<SheetData>();
            var row = GetRow(sheetData, GetRowIndex(cellReference));
            var refColumn = GetColumnName(cellReference);
            var newCell = new Cell() { CellReference = cellReference };

            Cell refCell = null;
            foreach (var cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            row.InsertBefore(newCell, refCell);

            return newCell;
        }

        private uint GetRowIndex(string cellReference)
        {
            int rowIndexStart = 0;
            for (int i = 0; i < cellReference.Length; i++)
            {
                if (char.IsDigit(cellReference[i]))
                {
                    rowIndexStart = i;
                    break;
                }
            }

            string rowIndexString = cellReference.Substring(rowIndexStart);
            return uint.Parse(rowIndexString);
        }

        private string GetColumnName(string cellReference)
        {
            return new string(cellReference.Where(c => char.IsLetter(c)).ToArray());
        }
        //metodos para orden descuento deudor

        public MemoryStream GenerarOrdenDescuentoDeudor(ordenDescuento data)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "OrdenDescuentoDeudor.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Pagare();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_OrdenD(data, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }
        private string Cargar_Datos_OrdenD(ordenDescuento data, string xml)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();
            var fecha = DateTime.Today;
            string nueva_F = fecha.ToLongDateString();
            string[] valores = nueva_F.Split(" ");
            string dia = string.Format(valores[1]);
            string año = string.Format(conversion.Enletras(valores[5]));
            xml = Reemplazar(xml, "«mes»", valores[3]);


            xml = Reemplazar(xml, "«dia»", dia.ToString());
            xml = Reemplazar(xml, "«nombredelsocio»", data.nombreCliente);
            xml = Reemplazar(xml, "«edad»", data.edad.ToString());
            xml = Reemplazar(xml, "«ciudadom»", data.ciudad.ToString());
            xml = Reemplazar(xml, "«cuotas»", data.numCuotas.ToString());
            xml = Reemplazar(xml, "«Ccuota»", decimal.Round(data.valCuota, 2).ToString());
            xml = Reemplazar(xml, "«cuota»", decimal.Round(data.valCuota, 2).ToString());
            xml = Reemplazar(xml, "«scodigo»", data.codigoCredito.ToString());
            xml = Reemplazar(xml, "«expedido»", data.expedicionDui.ToString());

            xml = Reemplazar(xml, "«ciudad»", data.ciudad.ToString());
            // xml = Reemplazar(xml, "«fDuiEnLetras»", data.dui.ToString());
            xml = Reemplazar(xml, "««Ccuota»", string.Format(conversion.Enletras(decimal.Round(data.valCuota, 2).ToString())));


            xml = Reemplazar(xml, "«dui2»", string.Format(documentos.Enletras(data.dui)));
            string fecha_ahora = DateTime.Now.ToLongDateString();
            string[] valores_ahora = fecha_ahora.Split(" ");
            string dia_ahora = string.Format(conversion.Enletras(valores_ahora[1]));
            string año_ahora = string.Format(conversion.Enletras(valores_ahora[5]));
            string dia_fecha_ahora = valores_ahora[0].Replace(",", string.Empty);
            string fecha_nueva_ahora = dia_fecha_ahora + " " + dia_ahora.ToLower() + " de " + valores_ahora[3] + " de " + año_ahora.ToLower();/*Debe cambiarse a letras*/
            xml = Reemplazar(xml, "«Cfechahoy»", fecha_nueva_ahora); /*tambien debe estar en letras*/

            return xml;
        }

        //orden descuento fiador
        public MemoryStream GenerarOrdenDescuentoFiador(ordenDescuento data)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "orden_descf.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Pagare();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_OrdenDFiador(data, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }
        private string Cargar_Datos_OrdenDFiador(ordenDescuento data, string xml)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();
            var fecha = DateTime.Today;
            string nueva_F = fecha.ToLongDateString();
            string[] valores = nueva_F.Split(" ");
            string dia = string.Format(valores[1]);
            string año = string.Format(conversion.Enletras(valores[5]));
            xml = Reemplazar(xml, "«mes»", valores[3]);
            xml = Reemplazar(xml, "«anio»", valores[5]);


            xml = Reemplazar(xml, "«dia»", dia.ToString());
            xml = Reemplazar(xml, "«NombredelFiador»", data.nombreCliente);
            xml = Reemplazar(xml, "«edad»", data.edad.ToString());
            xml = Reemplazar(xml, "«ciudadomf»", data.ciudad.ToString());
            xml = Reemplazar(xml, "«cuotas»", data.numCuotas.ToString());
            xml = Reemplazar(xml, "«Ccuota»", decimal.Round(data.valCuota, 2).ToString());
            xml = Reemplazar(xml, "«cuota»", decimal.Round(data.valCuota, 2).ToString());
            xml = Reemplazar(xml, "«scodigo»", data.codigoCredito.ToString());
            xml = Reemplazar(xml, "«expedidof»", data.expedicionDui.ToString());

            xml = Reemplazar(xml, "«ciudad»", data.ciudad.ToString());
            // xml = Reemplazar(xml, "«fDuiEnLetras»", data.dui.ToString());
            xml = Reemplazar(xml, "««Ccuota»", string.Format(conversion.Enletras(decimal.Round(data.valCuota, 2).ToString())));


            xml = Reemplazar(xml, "«duif1»", string.Format(conversion.Enletras(data.dui)));
            string fecha_ahora = DateTime.Now.ToLongDateString();
            string[] valores_ahora = fecha_ahora.Split(" ");
            string dia_ahora = string.Format(conversion.Enletras(valores_ahora[1])); /*aqui*/
            string año_ahora = string.Format(conversion.Enletras(valores_ahora[5]));
            string dia_fecha_ahora = valores_ahora[0].Replace(",", string.Empty);
            string fecha_nueva_ahora = dia_fecha_ahora + " " + dia_ahora.ToLower() + " de " + valores_ahora[3] + " de " + año_ahora.ToLower();/*Debe cambiarse a letras*/
            xml = Reemplazar(xml, "«Cfechahoy»", fecha_nueva_ahora); /*tambien debe estar en letras*/

            return xml;
        }
        public MemoryStream GenerarCompartirDatosFiador(ordenDescuento data)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "autoriza_socio.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Pagare();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_CompartirDatosDFiador(data, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }
        private string Cargar_CompartirDatosDFiador(ordenDescuento data, string xml)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();
            var fecha = DateTime.Today;
            string nueva_F = fecha.ToLongDateString();
            string[] valores = nueva_F.Split(" ");


            xml = Reemplazar(xml, "«nombredelsocio»", data.nombreCliente);

            xml = Reemplazar(xml, "«ciudad»", data.ciudad.ToString());




            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "«dia»", dia + " " + "de " + monthName + " de " + " " + año);

            return xml;
        }
        private string Cargar_Datos_PagareC(PagarCodeudor data, string xml)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();
            var fecha = DateTime.Today;
            string nueva_F = fecha.ToLongDateString();
            string[] valores = nueva_F.Split(" ");
            string dia = string.Format(conversion.Enletras(valores[1]));
            string año = string.Format(conversion.Enletras(valores[5]));

            /*La persona como tal*/

            xml = Reemplazar(xml, "«NombredelFiador»", data.nombreFiador);
            xml = Reemplazar(xml, "«edadf»", data.edad.ToString());
            // xml = Reemplazar(xml, "«dirfiador»", data.direccion.ToString());
            xml = Reemplazar(xml, "«mciudad»", data.ciudad.ToString());
            xml = Reemplazar(xml, "«ciudad»", data.ciudad.ToString());
            // xml = Reemplazar(xml, "«fDuiEnLetras»", data.dui.ToString());
            xml = Reemplazar(xml, "«CMontoPrestamo»", string.Format(conversion.Enletras(decimal.Round(data.monto, 2).ToString())));


            xml = Reemplazar(xml, "«duif1»", string.Format(documentos.Enletras(data.Dui)));
            string fecha_ahora = DateTime.Now.ToLongDateString();
            string[] valores_ahora = fecha_ahora.Split(" ");
            string dia_ahora = string.Format(conversion.Enletras(valores_ahora[1]));
            string año_ahora = string.Format(conversion.Enletras(valores_ahora[5]));
            string dia_fecha_ahora = valores_ahora[0].Replace(",", string.Empty);
            string fecha_nueva_ahora = dia_fecha_ahora + " " + dia_ahora.ToLower() + " de " + valores_ahora[3] + " de " + año_ahora.ToLower();/*Debe cambiarse a letras*/
            xml = Reemplazar(xml, "«Cfechahoy»", fecha_nueva_ahora); /*tambien debe estar en letras*/

            return xml;
        }

        //---------------------------------------ORDEN DESCUENTO-----------------------------------------------------------------------------------------
        public MemoryStream Generar_Orden_Descuento(Persona persona, Municipio_Departamento municipio = null, Departamento_Pais departamento = null)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "ORDEN_desc.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Orden();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Orden(persona, docText, municipio, departamento);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_Orden()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Orden(Persona persona, string xml, Municipio_Departamento municipio = null, Departamento_Pais departamento = null
       )
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();
            var fecha = DateTime.Today;
            string nueva_F = fecha.ToLongDateString();
            string[] valores = nueva_F.Split(" ");
            string dia = string.Format(conversion.Enletras(valores[1]));
            string año = string.Format(conversion.Enletras(valores[5]));

            /*La persona como tal*/
            xml = Reemplazar(xml, "«dia»", dia.ToLower());
            xml = Reemplazar(xml, "«mes»", valores[3]);
            xml = Reemplazar(xml, "«ano»", año.ToLower());
            xml = Reemplazar(xml, "«nombredelsocio»", persona.NombreCompleto);
            xml = Reemplazar(xml, "«profecion»", persona.Profesion);

            xml = Reemplazar(xml, "«dui»", string.Format(documentos.Enletras(persona.Dui)));
            xml = Reemplazar(xml, "«nit»", string.Format(documentos.Enletras(persona.Nit)));
            xml = Reemplazar(xml, "«edad»", " " + string.Format(conversion.Enletras(persona.Edad.ToString())) + " ");
            xml = Reemplazar(xml, "Fecha_Pagare", persona.Comentario);
            /*el crédito de la persona*/
            Credito credito = persona.Creditos.Last();
            xml = Reemplazar(xml, "«cuotas»", string.Format(conversion.Enletras(decimal.Round(credito.Numero_cuotas, 2).ToString())));
            xml = Reemplazar(xml, "«Ccuota»", string.Format(conversion.Enletras(decimal.Round(credito.Monto_pago, 2).ToString())));
            xml = Reemplazar(xml, "«cuota»", decimal.Round(credito.Monto_pago, 2).ToString());
            string fecha_ahora = credito.Vencido_En.ToLongDateString();
            string[] valores_ahora = fecha_ahora.Split(" ");
            string dia_ahora = string.Format(conversion.Enletras(valores_ahora[1]));
            string año_ahora = string.Format(conversion.Enletras(valores_ahora[5]));
            string dia_fecha_ahora = valores_ahora[0].Replace(",", string.Empty);
            string fecha_nueva_ahora = dia_fecha_ahora + " " + dia_ahora.ToLower() + " de " + valores_ahora[3] + " de " + año_ahora.ToLower();/*Debe cambiarse a letras*/
            xml = Reemplazar(xml, "«Cfechavenc»", fecha_nueva_ahora.ToUpper()); /*tambien debe estar en letras*/
            xml = Reemplazar(xml, "«tasaa»", string.Format(conversion.Enletras(decimal.Round(credito.Interes, 2).ToString()))); /*tambien debe estar en letras*/
            xml = Reemplazar(xml, "«scodigo»", credito.Codigo_Credito);
            if (municipio.Nombre_Municipio != null)
            {
                xml = Reemplazar(xml, "«dir_casa»", " " + credito.Nombre_Ciudad_Mutuo + " ");/*Falta la residencia*/
            }
            if (departamento.Nombre_Departamento != null)
            {
                xml = Reemplazar(xml, "«mciudad»", credito.Nombre_Ciudad_Mutuo);
            }

            xml = Reemplazar(xml, "«ciudad»", credito.Nombre_Ciudad_Mutuo);


            return xml;
        }


        //------------------------------------------------------------------- Consultar Record ------------------------------------------------------
        public MemoryStream Consultar_Record(Persona persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "consultar_record.docx");
            string documento_destino = Completa_Nombre_Aleatorio_CRedord();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_CRecord(persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_CRedord()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_CRecord(Persona persona, string xml)
        {
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();
            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "ciudad_r", persona.Comentario);
            xml = Reemplazar(xml, "diarecord", dia);
            xml = Reemplazar(xml, "mesrecord", monthName);
            xml = Reemplazar(xml, "añorecord", año);
            xml = Reemplazar(xml, "nombreCliente", persona.NombreCompleto);
            xml = Reemplazar(xml, "duiCliente", string.Format(documentos.Enletras(persona.Dui)));
            xml = Reemplazar(xml, "nitCliente", string.Format(documentos.Enletras(persona.Nit)));
            /*Todos los datos del codeudor*/

            return xml;
        }
        //-------------------------------------------------------------------Compartir Datos -----------------------------------------------------
        public MemoryStream Compartir_Datos(Persona persona, Municipio_Departamento municipio = null, Departamento_Pais departamento = null)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "autoriza_socio.docx");
            string documento_destino = Completa_Nombre_Aleatorio_CDatos();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_CDatos(persona, docText, municipio, departamento);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_CDatos()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_CDatos(Persona persona, string xml, Municipio_Departamento municipio = null, Departamento_Pais departamento = null)
        {

            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "«dia»", dia + " " + "de " + monthName + " de " + " " + año);

            xml = Reemplazar(xml, "«nombredelsocio»", persona.NombreCompleto);
            if (departamento.Nombre_Departamento != null)
            {
                xml = Reemplazar(xml, "«ciudad»", " " + departamento.Nombre_Departamento + " ");
            }
            /*Todos los datos del codeudor*/

            return xml;
        }

        //------------------------------------------------------------------- Condiciones Generales ---------------------------------------------------------------
        public MemoryStream Generar_Condiciones_Generales(Persona persona)
        {
            string documento_fuente = "";
            if (persona.Creditos.Max(x => x.Id_Linea) == 19 || persona.Creditos.Max(x => x.Id_Linea) == 21 || persona.Creditos.Max(x => x.Id_Linea) == 20)

            {
                documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "condicines_generales_n.docx");

            }
            else
            {
                documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "Condiciones_Generales.docx");

            }
            string documento_destino = Completa_Nombre_Aleatorio_Condiciones_Generales();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Condiciones_Generales(persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_Condiciones_Generales()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Condiciones_Generales(Persona persona, string xml)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            /*La persona como tal*/
            xml = Reemplazar(xml, "Nombre_Socio", persona.NombreCompleto);

            Credito credito = persona.Creditos.FirstOrDefault();
            xml = Reemplazar(xml, "Codigo_Socio", credito.Codigo_Credito);
            xml = Reemplazar(xml, "monto_aprobado", decimal.Round(credito.Monto_pago, 2).ToString());
            xml = Reemplazar(xml, "plazo_meses", credito.Plazo.ToString());
            xml = Reemplazar(xml, "n_cuotas", string.Format(conversion.Enletras(decimal.Round(credito.Numero_cuotas, 2).ToString())));
            xml = Reemplazar(xml, "dia_pago", credito.Vencido_En.Date.ToString("dddd", new CultureInfo("es-ES")));
            xml = Reemplazar(xml, "vencimiento_cre", credito.Vencido_En.Date.ToString("dd/MM/yyyy"));

            xml = Reemplazar(xml, "valor_cuota", decimal.Round(credito.Valor_Cuota, 2).ToString());
            if (persona.Persona_Relaciones != null && persona.Persona_Relaciones.FirstOrDefault() != null)
            {
                Persona codeudor = persona.Persona_Relaciones.FirstOrDefault().Predicado;
                xml = Reemplazar(xml, "Nombre_Codeudor", codeudor.NombreCompleto);
                xml = Reemplazar(xml, "Codigo_Codeudor", codeudor.Codigo);
            }
            else
            {
                xml = Reemplazar(xml, "Nombre_Codeudor", "No posee");
                xml = Reemplazar(xml, "Codigo_Codeudor", "No posee");
            }
            /*Todos los datos del codeudor*/

            return xml;
        }

        //-------------------------------------------------------------------------Carpeta Azul-----------------------------------------------------------------------
        public MemoryStream Generar_Carpeta_Azul(Carpeta_Azul persona, int idCredito = 0)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "carpeta_azul.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Carpeta_Azul();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Carpeta_Azul(persona, docText, idCredito);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_Carpeta_Azul()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Carpeta_Azul(Carpeta_Azul persona, string xml, int idCredito = 0)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            Conversion_dui_nit_letra documentos = new Conversion_dui_nit_letra();
            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "Ciudad_r", persona.Persona.Comentario);
            xml = Reemplazar(xml, "dia_carpeta", dia);
            xml = Reemplazar(xml, "mes_carpeta", monthName);
            xml = Reemplazar(xml, "año_carpeta", año);
            xml = Reemplazar(xml, "Nombre_Deudor", persona.Persona.NombreCompleto);
            xml = Reemplazar(xml, "Dui_Codeudor", persona.Persona.Dui);// Dui deudor dice codeudor por problemas con el word
            xml = Reemplazar(xml, "Nit_Socio", persona.Persona.Dui);
            xml = Reemplazar(xml, "forma_pago", persona.Periodo.Descripcion);
            xml = Reemplazar(xml, "Primer_Cuota", persona.Amortizacion.Min(a => a.Valor_Cuota.ToString("F2")));
            xml = Reemplazar(xml, "Primer_Cuota", persona.Amortizacion.Min(a => a.Valor_Cuota.ToString("F2")));
            xml = Reemplazar(xml, "primer_pago", persona.Amortizacion.Min(d => d.FechaPago).ToString("D"));
            xml = Reemplazar(xml, "fecha_vencimiento", persona.Amortizacion.Max(d => d.FechaPago).ToString("D"));


            Credito credito = persona.Persona.Creditos.Single(a => a.Id_Credito == idCredito);
            var plan_ayc = credito.Interes_Comision - credito.Interes;
            xml = Reemplazar(xml, "Codigo_Cr", credito.Codigo_Credito);
            xml = Reemplazar(xml, "monto_desembolso", string.Format(conversion.Enletras(decimal.Round(credito.Monto_pago, 2).ToString())));
            xml = Reemplazar(xml, "n_cuotas", credito.Numero_cuotas.ToString());
            xml = Reemplazar(xml, "valor_cuota", string.Format(conversion.Enletras(decimal.Round(credito.Valor_Cuota, 2).ToString())));
            xml = Reemplazar(xml, "Interes_Credito", string.Format(conversion.Enletras(credito.Interes_Comision.ToString("F2"))));
            xml = Reemplazar(xml, "plan_ayc", string.Format(conversion.Enletras(decimal.Round(plan_ayc).ToString())));
            xml = Reemplazar(xml, "fecha_inicio", credito.Liquidado_En.ToLongDateString());
            /*Todos los datos del codeudor*/
            if (persona.Persona.Persona_Relaciones.FirstOrDefault() != null)
            {
                Persona codeudor = persona.Persona.Persona_Relaciones.FirstOrDefault().Predicado;
                xml = Reemplazar(xml, "Nombre_Codeudor", codeudor.NombreCompleto);
                xml = Reemplazar(xml, "Dui_Fiador", string.Format(documentos.Enletras(codeudor.Dui)));// Dui fiador dice fiador por problemas con el word
            }
            else
            {
                xml = Reemplazar(xml, "Nombre_Codeudor", "no posee");
                xml = Reemplazar(xml, "Dui_Fiador", "no posee");// Dui fiador dice fiador por problemas con el word
            }

            return xml;
        }
        //---------------------------------------------------------Plan AyC--------------------------------------------------------------------------
        public MemoryStream Generar_PlanAyC(Persona persona, Municipio_Departamento municipio = null, Departamento_Pais departamento = null)
        {
            string documento_fuente = "";
            if (persona.Creditos.Max(x => x.Id_Linea) == 19 || persona.Creditos.Max(x => x.Id_Linea) == 21)

            {
                documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "PlanAyCNuevo.docx");

            }
            else
            {
                documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "PLANAYC.docx");

            }
            //documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "PLANAYC.docx");
            string documento_destino = Completa_Nombre_Aleatorio_PlanAyC();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_PlanAyC(persona, docText, municipio, departamento);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_PlanAyC()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_PlanAyC(Persona persona, string xml, Municipio_Departamento municipio = null, Departamento_Pais departamento = null)
        {
            Conversion_numero_letra conversion = new Conversion_numero_letra();
            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "«dia»", dia);
            xml = Reemplazar(xml, "«mes»", monthName);
            xml = Reemplazar(xml, "«ano»", año);
            if (departamento.Nombre_Departamento != null)
            {
                xml = Reemplazar(xml, "«ciudad»", departamento.Nombre_Departamento);
            }
            xml = Reemplazar(xml, "«nombredelsocio»", persona.NombreCompleto);
            Credito credito = persona.Creditos.FirstOrDefault();
            decimal plan_ayc = credito.Primer_Pago;
            xml = Reemplazar(xml, "«scodigo»", credito.Codigo_Credito);
            xml = Reemplazar(xml, "«qcob»", string.Format(conversion.Enletras(decimal.Round(plan_ayc, 2).ToString())));
            xml = Reemplazar(xml, "«Ccuotas»", string.Format(conversion.Enletras(credito.Numero_cuotas.ToString())));
            xml = Reemplazar(xml, "«Cmeses»", string.Format(conversion.Enletras(credito.Plazo.ToString())));


            return xml;
        }
        //-------------------------------------------------------------DOCUMENTOS AHORROS-------------------------------------------------------------------------------
        public MemoryStream Generar_Solicitud_Socio(Persona persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "Solicitud_Socio_Admision.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Solicitud_Socio();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Solicitud_Socio(persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_Solicitud_Socio()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Solicitud_Socio(Persona persona, string xml)
        {
            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "ciudad_celebrado", persona.Comentario);
            xml = Reemplazar(xml, "Dia_Celebrado", dia);
            xml = Reemplazar(xml, "Mes_Celebrado", monthName);
            xml = Reemplazar(xml, "Año_Cele", año);
            xml = Reemplazar(xml, "Nombre_Cliente", persona.NombreCompleto);
            xml = Reemplazar(xml, "Municipio_Cliente", persona.Comentario);
            xml = Reemplazar(xml, "Departamento_Cliente", persona.Comentario);
            xml = Reemplazar(xml, "Dui_Cliente", persona.Dui);
            xml = Reemplazar(xml, "Nit_Cliente", persona.Nit);
            xml = Reemplazar(xml, "Fecha_Pagare", persona.Comentario);
            /*el crédito de la persona*/


            return xml;
        }
        //---------------------------------------------------Cambio Beneficiario-----------------------------------------------------------------------------
        public MemoryStream Generar_Solicitud_Cambio_Beneficiario(Persona persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "Solicitud_Cambio_Beneficiario.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Cambio_Beneficiario();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Cambio_Beneficiario(persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_Cambio_Beneficiario()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Cambio_Beneficiario(Persona persona, string xml)
        {
            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "ciudad_celebrado", persona.Comentario);
            xml = Reemplazar(xml, "Dia_Celebrado", dia);
            xml = Reemplazar(xml, "Mes_Celebrado", monthName);
            xml = Reemplazar(xml, "Año_Cele", año);
            xml = Reemplazar(xml, "Nombre_Cliente", persona.NombreCompleto);
            xml = Reemplazar(xml, "Municipio_Cliente", persona.Comentario);
            xml = Reemplazar(xml, "Departamento_Cliente", persona.Comentario);
            xml = Reemplazar(xml, "DUI_Cliente", persona.Dui);
            xml = Reemplazar(xml, "NIT_Cliente", persona.Nit);

            /*el Ahorro de la persona*/
            Ahorro ahorro = persona.Ahorros.FirstOrDefault();

            xml = Reemplazar(xml, "Apertura_Ahorro", ahorro.Fecha_Apertura.ToString("MM/dd/yyyy"));
            xml = Reemplazar(xml, "Cuenta_Ahorro", ahorro.No_Ahorro);


            return xml;
        }
        public MemoryStream Generar_Perfil_Socio(Persona persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "Perfil_Socio.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Perfil_Socio();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Perfil_Socio(persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_Perfil_Socio()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Perfil_Socio(Persona persona, string xml)
        {
            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "ciudad_celebrado", persona.Comentario);
            xml = Reemplazar(xml, "Dia_Celebrado", dia);
            xml = Reemplazar(xml, "Mes_Celebrado", monthName);
            xml = Reemplazar(xml, "Año_Cele", año);
            xml = Reemplazar(xml, "Nombres_Cliente", persona.Nombres);
            xml = Reemplazar(xml, "Apellidos_Cliente", persona.Apellidos);
            xml = Reemplazar(xml, "Edad_Cl", persona.Edad.ToString());
            if (persona.EstadoFamiliar == (Estado_Familiar)1)
            {
                xml = Reemplazar(xml, "Estado_Cli", "Soltero");
            }
            else if (persona.EstadoFamiliar == (Estado_Familiar)2)
            {
                xml = Reemplazar(xml, "Estado_Cli", "Casado");
            }
            else if (persona.EstadoFamiliar == (Estado_Familiar)4)
            {
                xml = Reemplazar(xml, "Estado_Cli", "Viudo");
            }
            else if (persona.EstadoFamiliar == (Estado_Familiar)4)
            {
                xml = Reemplazar(xml, "Estado_Cli", "Divorciado");
            }
            else if (persona.EstadoFamiliar == (Estado_Familiar)0)
            {
                xml = Reemplazar(xml, "Estado_Cli", "");
            }
            if (persona.Sexo == (Sexo)1)
            {
                xml = Reemplazar(xml, "Sexo_F", "");
                xml = Reemplazar(xml, "Sexo_M", "X");
            }
            else if (persona.Sexo == (Sexo)2)
            {
                xml = Reemplazar(xml, "Sexo_F", "X");
                xml = Reemplazar(xml, "Sexo_M", "");
            }
            xml = Reemplazar(xml, "Direccion_Cliente", persona.Domicilio);
            xml = Reemplazar(xml, "Telefono_Cliente", persona.Telefono);
            xml = Reemplazar(xml, "Profesion_Cliente", persona.Profesion);
            xml = Reemplazar(xml, "Dui_Cliente", persona.Dui);
            xml = Reemplazar(xml, "Nit_Cliente", persona.Nit);
            xml = Reemplazar(xml, "Expedicion_Dui", persona.ExpedicionDocumentoIdentifcacion);
            xml = Reemplazar(xml, "Expiracion_Dui", persona.ExpiracionDocumentoIdentificacion);
            xml = Reemplazar(xml, "Lugar_Nacimiento", persona.FechaNacimiento.ToString());

            xml = Reemplazar(xml, "Fecha_Nacimiento", persona.FechaNacimiento.ToString());
            xml = Reemplazar(xml, "Lugar_trabajo", persona.Lugar_Trabajo);
            xml = Reemplazar(xml, "Teléfono_trabajo", persona.Telefono_Trabajo);
            xml = Reemplazar(xml, "Ingreso_mensual", persona.Ingresos_Mensuales.ToString("#.##"));


            return xml;
        }

        //------------------------------------------------------ Declaracion Jurada-----------------------------------------------
        public MemoryStream Generar_Solicitud_Declaracion_Jurada(Persona persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "Declaracion_Jurada.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Declaracion_Jurada();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Declaracion_Jurada(persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_Declaracion_Jurada()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Declaracion_Jurada(Persona persona, string xml)
        {
            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "ciudad_celebrado", persona.Comentario);
            xml = Reemplazar(xml, "Dia_Celebrado", dia);
            xml = Reemplazar(xml, "Mes_Celebrado", monthName);
            xml = Reemplazar(xml, "Año_Cele", año);
            xml = Reemplazar(xml, "Nombre_Cliente", persona.NombreCompleto);
            xml = Reemplazar(xml, "Dui_Cliente", persona.Dui);


            return xml;
        }

        //---------------------------------------------Contrato Deposito--------------------------------------------------------------------------
        public MemoryStream Generar_Solicitud_Contrato_Deposito(Persona persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "contrato_deposito.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Contrato_Deposito();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Contrato_Deposito(persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_Contrato_Deposito()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Contrato_Deposito(Persona persona, string xml)
        {
            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "ciudad_celebrado", persona.Comentario);
            xml = Reemplazar(xml, "Dia_Celebrado", dia);
            xml = Reemplazar(xml, "Mes_Celebrado", monthName);
            xml = Reemplazar(xml, "Año_Cele", año);
            xml = Reemplazar(xml, "Nombre_Cliente", persona.NombreCompleto);
            xml = Reemplazar(xml, "Dui_Cliente", persona.Dui);
            xml = Reemplazar(xml, "Nit_Cliente", persona.Nit);

            /*el Ahorro de la persona*/
            Ahorro ahorro = persona.Ahorros.FirstOrDefault();

            xml = Reemplazar(xml, "Cuenta_Ahorro", ahorro.No_Ahorro);


            return xml;
        }
        //---------------------------------------------Contrato Deposito COPIA--------------------------------------------------------------------------
        public MemoryStream Generar_Solicitud_Contrato_Deposito_Copia(Persona persona)
        {
            string documento_fuente = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Documento_Template", "contrato_de_deposito_copia.docx");
            string documento_destino = Completa_Nombre_Aleatorio_Contrato_Deposito_Copia();
            File.Copy(documento_fuente, documento_destino);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documento_destino, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    docText = sr.ReadToEnd();

                docText = Cargar_Datos_Contrato_Deposito_Copia(persona, docText);

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    sw.Write(docText);
            }

            FileStream doc = new FileStream(documento_destino, FileMode.Open);
            MemoryStream documentStream = new MemoryStream();

            documentStream.SetLength(doc.Length);
            doc.Read(documentStream.GetBuffer(), 0, (int)documentStream.Length);

            return documentStream;
        }

        private string Completa_Nombre_Aleatorio_Contrato_Deposito_Copia()
        {
            /*ubicaciion de archivos aleatorio*/
            string nombre_archivo_unico = DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");
            return string.Format(@"c:\\Documentos_Mutuos\{0}.docx", nombre_archivo_unico);
        }

        private string Cargar_Datos_Contrato_Deposito_Copia(Persona persona, string xml)
        {
            var fecha = DateTime.Today;
            var dia = Convert.ToString(fecha.Day);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month);
            var año = Convert.ToString(fecha.Year);
            /*La persona como tal*/
            xml = Reemplazar(xml, "ciudad_celebrado", persona.Comentario);
            xml = Reemplazar(xml, "Dia_Celebrado", dia);
            xml = Reemplazar(xml, "Mes_Celebrado", monthName);
            xml = Reemplazar(xml, "Año_Cele", año);
            xml = Reemplazar(xml, "Nombre_Cliente", persona.NombreCompleto);
            xml = Reemplazar(xml, "Dui_Cliente", persona.Dui);
            xml = Reemplazar(xml, "Nit_Cliente", persona.Nit);

            /*el Ahorro de la persona*/
            Ahorro ahorro = persona.Ahorros.FirstOrDefault();

            xml = Reemplazar(xml, "Cuenta_Ahorro", ahorro.No_Ahorro);


            return xml;
        }



        private string Reemplazar(string xml, string fuente, string valor)
        {
            valor = valor ?? "No definido";
            Regex regexText = new Regex(fuente);
            xml = regexText.Replace(xml, valor);
            return xml;
        }


        private string LimpiarEtiquetasXml(string xml)
        {
            // Esta expresión regular busca códigos de formato de Word que se metieron
            // en medio de los símbolos « y » y los elimina para unir la etiqueta de nuevo.
            return System.Text.RegularExpressions.Regex.Replace(xml, @"(?<=«[^»]*)(<[^>]+>)(?=[^«]*»)", "");
        }
    }

}
