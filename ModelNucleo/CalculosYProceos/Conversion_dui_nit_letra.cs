using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.CalculosYProceos
{
   public class Conversion_dui_nit_letra
    {
        #region DUI EN LETRAS.
        public string Enletras(string num)
        {
            string res = "", res1 = "", res2 = "", res3 = "", res4 = "", res5 = "", res6 = "", res7 = "", dec = "";
            string nro, nro1, nro3, nro4;

            if (num.Length == 10)
            {
                string cadena = num;
                // separar dui por guiones
                string[] valores = cadena.Split("-");
                //primera dimencion
                string celda1 = valores[0].Trim();
                //segunda dimencion
                string celda2 = valores[1].Trim();

                //contador
                int startIndex = 0;

                try
                {
                    for (int i = 0; i != celda1.Length; i++)
                    {
                        //obtener cantidad de ceros del documento antes de un entero
                        bool escero = celda1[i] == '0';
                        if (escero)
                        {

                            if (i < 3)
                            {
                                startIndex = i + 1;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    celda1 = celda1.TrimStart(new char[] { '0' });
                    nro = celda1;
                    nro1 = celda2;
                }
                catch
                {
                    return "";
                }
                int suma_valores = celda1.Length + startIndex;
                //colocar ceros a varianle contador
                if (startIndex == 1)
                {
                    res2 = toText(double.Parse("0")) + dec;
                    res = toText(double.Parse(nro)) + dec;
                    res1 = toText(double.Parse(nro1)) + dec;

                }
                else if (startIndex == 2)
                {
                    res2 = toText(double.Parse("0")) + dec;
                    res3 = toText(double.Parse("0")) + dec;
                    res = toText(double.Parse(nro)) + dec;
                    res1 = toText(double.Parse(nro1)) + dec;
                }
                else if (startIndex == 3)
                {
                    res2 = toText(double.Parse("0")) + dec;
                    res3 = toText(double.Parse("0")) + dec;
                    res4 = toText(double.Parse("0")) + dec;
                    res = toText(double.Parse(nro)) + dec;
                    res1 = toText(double.Parse(nro1)) + dec;
                }


                return res4 + res3 + res2 + res + " GUION " + res1;
            }
            //// -----------------------------------------------------------------variables NIT--------------------------------------------------
            string cadena1 = num;
            // separar nit por guiones
            string[] valores_nit = cadena1.Split("-");
            //primera dimencion
            string celda1_nit = valores_nit[0].Trim();
            //segunda dimencion
            string celda2_nit = valores_nit[1].Trim();
            //tercera dimencion
            string celda3_nit = valores_nit[2].Trim();
            //cuarta dimencion
            string celda4_nit = valores_nit[3].Trim();
            //contador
            int start_nit = 0;
            int start_nit1 = 0;
            int start_nit_2 = 0;
            int start_nit2 = 0;
            int total = 0;
            int total2 = 0;
            int start_nit_3 = 0;
            int start_nit3 = 0;
            int total3 = 0;
            try
            {
                start_nit = celda1_nit.Length;
                celda1_nit = celda1_nit.TrimStart(new char[] { '0' });
                start_nit1 = Convert.ToInt32(celda1_nit.Length);
                total = start_nit - start_nit1;

                start_nit_2 = celda2_nit.Length;
                celda2_nit = celda2_nit.TrimStart(new char[] { '0' });
                start_nit2 = Convert.ToInt32(celda2_nit.Length);
                total2 = start_nit_2 - start_nit2;

                start_nit_3 = celda3_nit.Length;
                celda3_nit = celda3_nit.TrimStart(new char[] { '0' });
                start_nit3 = Convert.ToInt32(celda3_nit.Length);
                total3 = start_nit_3 - start_nit3;

                nro = celda1_nit;
                nro1 = celda2_nit;
                nro3 = celda3_nit;
                nro4 = celda4_nit;
                if (total == 1)
                {
                    res2 = toText(double.Parse("0")) + dec;
                    res = toText(double.Parse(nro)) + dec;
                    res1 = toText(double.Parse(nro1)) + dec;
                    res3 = toText(double.Parse(nro3)) + dec;
                    res4 = toText(double.Parse(nro4)) + dec;
                }
                if (total2 == 1)
                {
                    res5 = toText(double.Parse("0")) + dec;
                    res = toText(double.Parse(nro)) + dec;
                    res1 = toText(double.Parse(nro1)) + dec;
                    res3 = toText(double.Parse(nro3)) + dec;
                    res4 = toText(double.Parse(nro4)) + dec;
                }
                if (total3 == 1)
                {
                    res6 = toText(double.Parse("0")) + dec;
                    res = toText(double.Parse(nro)) + dec;
                    res1 = toText(double.Parse(nro1)) + dec;
                    res3 = toText(double.Parse(nro3)) + dec;
                    res4 = toText(double.Parse(nro4)) + dec;
                }
                if (total3 == 2)
                {
                    res6 = toText(double.Parse("0")) + dec;
                    res7 = toText(double.Parse("0")) + dec;
                    res = toText(double.Parse(nro)) + dec;
                    res1 = toText(double.Parse(nro1)) + dec;
                    res3 = toText(double.Parse(nro3)) + dec;
                    res4 = toText(double.Parse(nro4)) + dec;
                }
            }
            catch
            {
                return "";
            }
            return res2 + res + " GUION " + res5 + res1 + " GUION " + res6 + res7 + res3 + " GUION " + res4;
        } 
        #endregion
        private string toText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO ";
            else if (value == 00) Num2Text = "CERO CERO ";
            else if (value == 000) Num2Text = "CERO CERO CERO ";
            else if (value == 0000) Num2Text = "CERO CERO CERO CERO ";
            else if (value == 00000) Num2Text = "CERO CERO CERO CERO CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }
            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }
            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;
        }
       
        public string extraer_ceros(string datos)
        {
            Int32.Parse(datos);
            return "";

        }
    }
}
