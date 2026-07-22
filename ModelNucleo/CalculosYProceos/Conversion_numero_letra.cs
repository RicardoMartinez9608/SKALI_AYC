using System;
using System.Collections.Generic;
using System.Text;

namespace ModelNucleo.CalculosYProceos
{
    public class Conversion_numero_letra
    {
        //DH
        public string Enletras(string num)
        {
            string res = "", res1 = "", res2 = "", dec = "",punto ="";
            string nro, nro1;
            try
            {
                string cadena = num;
                // separar valores
                string[] valores = cadena.Split(".");
                //primera dimencion
                string celda1 = valores[0].Trim();
                string celda2 = "";
                //contador
                int startIndex = 0;
                if (valores.Length >1)
                {
                    //segunda dimencion
                    celda2 = valores[1].Trim();
                    for (int i = 0; i != celda2.Length; i++)
                    {
                        //obtener cantidad de ceros del documento antes de un entero
                        if (celda2[i] == '0')
                        {
                            startIndex = i + 1;
                        }
                    }
                }
                try
                {
                        celda1 = celda1.TrimStart(new char[] { '0' });
                        nro = celda1;
                        nro1 = celda2;
                    if (startIndex == 1)
                    {
                        res = toText(double.Parse(nro)) + dec;
                        res2 = toText(double.Parse("0")) + dec;
                        res1 = toText(double.Parse(nro1)) + dec;
                        punto = "PUNTO";
                    } else if (startIndex == 0)
                    {
                        res = toText(double.Parse(nro)) + dec;
                        if (valores.Length > 1)
                        {
                            res1 = toText(double.Parse(nro1)) + dec;
                            punto = "PUNTO";
                        }
                    } else if (startIndex >1) {
                        res = toText(double.Parse(nro)) + dec;
                        if (valores.Length > 1)
                        {
                            res1 = toText(double.Parse(nro1)) + dec;
                            punto = "PUNTO";
                        }
                    }
                        nro = celda1;
                        res = toText(double.Parse(nro)) + dec;
                }
                catch
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
            return res+" "+ punto +" "+ res2+" " + res1;
        }

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
    }
}
