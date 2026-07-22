using ModelNucleo.Model;
using ModelNucleo.Model.Evaluacion_Tecnica;
using ModelNucleo.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelNucleo.CalculosYProceos
{
    public class Calificacion_Evaluacion
    {
        private decimal _solvencia;
        private decimal _capicidad_pago;

        public Calificacion_Evaluacion()
        {

        }

        public Calificacion_Vista Calificar(Evaluacion_Tecnica_Vista evaluacion)
        {
            _solvencia = evaluacion.Activo_Total > 0 ? Math.Round(evaluacion.Total_Pasivo_Patrimonio / evaluacion.Activo_Total,2) : 0;
            _capicidad_pago = evaluacion.Evaluacion.Monto > 0 ? Math.Round(evaluacion.Activo_Total / evaluacion.Evaluacion.Monto,2) : 0;

            Calificacion_Vista calificacion = new Calificacion_Vista();
            calificacion.Caracter = new Ponderacion_Datos_Calificacion { Valor= Calificar_Caracter(evaluacion),Ponderacion=Ponderacion.Caracter };
            calificacion.Capacidad =new Ponderacion_Datos_Calificacion { Valor= Calificar_Capacidad(evaluacion), Ponderacion = Ponderacion.Capacidad };
            calificacion.Capital = new Ponderacion_Datos_Calificacion { Valor = Calificar_Capital(evaluacion), Ponderacion = Ponderacion.Capital };
            calificacion.Condiciones =new Ponderacion_Datos_Calificacion { Valor= Calificar_Condiciones(evaluacion), Ponderacion = Ponderacion.Condiciones };
            calificacion.Colateral =new Ponderacion_Datos_Calificacion { Valor= Calificar_Colateral(evaluacion), Ponderacion = Ponderacion.Colateral };

            calificacion.Monto_Propuesto = evaluacion.Evaluacion.Monto;
            calificacion.Plazo_Propuesto = evaluacion.Evaluacion.Plazo;
            calificacion.Cuota_Propuesta = evaluacion.Evaluacion.Cuota;

            calificacion.Total_Garantias = evaluacion.Activo_Total;
            calificacion.Solvencia = _solvencia;
            calificacion.Capacidad_pago = _capicidad_pago;

            /*indicadores toma*/
            decimal suma_margen_venta = 0.00M;
            if(evaluacion.Evaluacion.Costos_Ventas.Any())
                suma_margen_venta = evaluacion.Evaluacion.Costos_Ventas.Sum(t => t.Margen_Bruto_Porcentaje);
            decimal suma_disponible_negocio_otros_ingresos = evaluacion.Disponibilidad_Negocio + evaluacion.Otros_Ingresos;

            calificacion.Razon_Circulante = _solvencia;
            calificacion.Capital_Trabajo = evaluacion.Activo_Circulante - evaluacion.Pasivo_Circulante;
            calificacion.Rotacion_Inventario =evaluacion.Inventario>0?Math.Round(evaluacion.Costo_Venta / evaluacion.Inventario,2):0;
            calificacion.Rotacion_Cuenta_Cobrar =evaluacion.Cuentas_Cobrar>0? Math.Round(evaluacion.Recuperacion_Cuentas_Cobrar / evaluacion.Cuentas_Cobrar,2):0;
            calificacion.Endeudamiento = _solvencia;
            calificacion.Margen_Bruto =evaluacion.Venta_Mensual>0? Math.Round(evaluacion.Costo_Venta / evaluacion.Venta_Mensual,2):0;
            calificacion.Margen_Operativo =evaluacion.Venta_Mensual>0? Math.Round(evaluacion.Disponibilidad_Negocio / evaluacion.Venta_Mensual,2):0;
            calificacion.Punto_Equilibrio = suma_margen_venta > 0 ? Math.Round(evaluacion.Gastos_Familiares + evaluacion.Gastos_Negocio + evaluacion.Evaluacion.Cuota / suma_margen_venta,2) : 0.00M;
            calificacion.Relacion_Utilidad_Negocio = suma_disponible_negocio_otros_ingresos>0? Math.Round(evaluacion.Disponibilidad_Negocio / suma_disponible_negocio_otros_ingresos,2):0.00M;
            calificacion.Cobertura_Cuota = evaluacion.Evaluacion.Cuota>0? Math.Round(evaluacion.Total_Disponible / evaluacion.Evaluacion.Cuota,2):0.00M;

            return calificacion;
        }

        private decimal Calificar_Caracter(Evaluacion_Tecnica_Vista evaluacion)
        {
            decimal caracter = 0.00M;
            int edad = evaluacion.Evaluacion.Persona.Edad;
            int personas_acargo = evaluacion.Evaluacion.Persona.Personas_Cargo;
            int antiguedad_comerciante = evaluacion.Evaluacion.Antiguedad_Negocio;
            bool reside_zona_peligrosa = evaluacion.Evaluacion.Reside_Zona_Peligrosa;
            Estado_Familiar estado_familiar = evaluacion.Evaluacion.Persona.EstadoFamiliar;
            bool comprobar_referncia = evaluacion.Evaluacion.Referencia_Comprobada;
            Residencia condicion_residencia = evaluacion.Evaluacion.Persona.Residencia;

            /*ponderacion por edad*/
            if (edad >= 18 && edad <= 21)
                caracter += -50;
            if (edad >= 22 && edad <= 29)
                caracter += 0.80M;
            if (edad >= 46 && edad <= 60)
                caracter += 2.00M;
            if (edad > 60)
                caracter += 1.60M;

            /*ver personas a cargo*/
            if (personas_acargo == 0)
                caracter += 2.00M;
            if (personas_acargo == 2)
                caracter += 2.00M;
            if (personas_acargo == 3)
                caracter += 2.00M;
            if (personas_acargo == 4)
                caracter += 1.60M;
            if (personas_acargo == 5)
                caracter += 0.40M;

            /*falta la antiguedad de ser comerciante*/
            if (antiguedad_comerciante < 1)
                caracter += 0.00M;
            if (antiguedad_comerciante == 1)
                caracter += 2.50M;
            if (antiguedad_comerciante == 2)
                caracter += 3.00M;
            if (antiguedad_comerciante == 3)
                caracter += 3.50M;
            if (antiguedad_comerciante == 4)
                caracter += 4.00M;
            if (antiguedad_comerciante == 5)
                caracter += 4.50M;
            if (antiguedad_comerciante > 5)
                caracter += 5.00M;

            /*falta la recidencia*/
            if (!reside_zona_peligrosa)
                caracter += 5.00M;
            if (reside_zona_peligrosa)
                caracter += -50.00M;

            /*estado familiar*/
            if (estado_familiar == Estado_Familiar.Casado)
                caracter += 2.00M;
            if (estado_familiar == Estado_Familiar.Union_Libre)
                caracter += 1.50M;
            if (estado_familiar == Estado_Familiar.Viudo)
                caracter += 1.00M;
            if (estado_familiar == Estado_Familiar.Divorciado)
                caracter += 0.95M;
            if (estado_familiar == Estado_Familiar.Soltero)
                caracter += 0.75M;

            /*falta la comprobación de referencia crediticia*/
            if (comprobar_referncia)
                caracter += 5.00M;
            if (!comprobar_referncia)
                caracter += -50.00M;

            if (condicion_residencia == Residencia.Propio)
                caracter += 3.00M;
            if (condicion_residencia == Residencia.Alquilada)
                caracter += 1.00M;
            if (condicion_residencia == Residencia.Familiar)
                caracter += 2.00M;

            return caracter;
        }

        private decimal Calificar_Capacidad(Evaluacion_Tecnica_Vista evaluacion)
        {
            decimal capacidad = 0.00M;
            decimal solvencia = _solvencia;
            decimal capacidad_pago = _capicidad_pago;

            /*capacidad de pago*/
            if (capacidad_pago < 1.40M)
                capacidad += 12.50M;
            if (capacidad_pago >= 1.40M && capacidad_pago <= 1.70M)
                capacidad += 25.00M;
            if (capacidad_pago > 1.70M)
                capacidad += 17.50M;

            /*solvencia*/
            if (solvencia > 1 && solvencia < 2)
                capacidad += 5.50M;
            if (solvencia < 1)
                capacidad += 0.00M;

            return capacidad;
        }

        private decimal Calificar_Capital(Evaluacion_Tecnica_Vista evaluacion)
        {
            decimal capital = 0.00M;

            decimal reciprocidad = evaluacion.Activo_Total > 0 ? evaluacion.Patrimonio / evaluacion.Activo_Total : 0;
            decimal monto_maximo = evaluacion.Evaluacion.Monto_Credito_Mayor_Historial;
            decimal endeudamiento = _solvencia;

            /*ponderar reciprocidad*/
            if (reciprocidad > 50.00M)
                capital += 10.00M;
            if (reciprocidad < 50.00M)
                capital += 0.00M;

            /*monto con el que ha trabajado*/
            if (monto_maximo >= 100.00M)
                capital += 5.00M;
            if (monto_maximo >= 101.00M && monto_maximo <= 400.00M)
                capital += 4.00M;
            if (monto_maximo >= 401.00M && monto_maximo <= 800.00M)
                capital += 3.00M;
            if (monto_maximo >= 801.00M && monto_maximo <= 1200.00M)
                capital += 2.00M;
            if (monto_maximo >= 12001.00M && monto_maximo <= 2700.00M)
                capital += 1.00M;

            /*endeudamiento*/
            if (endeudamiento == 1)
                capital += 5.00M;
            if (endeudamiento == 2)
                capital += 3.60M;
            if (endeudamiento == 3)
                capital += 3.20M;
            if (endeudamiento == 4)
                capital += 2.00M;
            if (endeudamiento == 5)
                capital += 0.20M;

            return capital;
        }

        private decimal Calificar_Condiciones(Evaluacion_Tecnica_Vista evaluacion)
        {
            decimal condicion = 0.00M;

            int mora = evaluacion.Evaluacion.Mora;
            Sector_Economico sector_economico = Sector_Economico.Comercio;
            int riesgo = evaluacion.Evaluacion.Riesgo_Info_Red;
            int plazo_credito = evaluacion.Evaluacion.Plazo;

            /*ponderación de la mora*/
            if (mora >= 0 && mora <= 30)
                condicion += 5.00M;
            if (mora >= 31 && mora <= 45)
                condicion += 3.33M;
            if (mora >= 46 && mora <= 89)
                condicion += 1.67M;
            if (mora >= 90 && mora > 90)
                condicion += 0.00M;

            /*ponderación del sector económico*/
            if (sector_economico == Sector_Economico.Comercio)
                condicion += 2.00M;
            if (sector_economico == Sector_Economico.Servicios)
                condicion += 1.40M;
            if (sector_economico == Sector_Economico.Consumo)
                condicion += 2.00M;
            if (sector_economico == Sector_Economico.Industria)
                condicion += 0.40M;

            /*ponderación del riesgo*/
            if (riesgo <= 25)
                condicion += 6.00M;
            if (riesgo >= 26 && riesgo <= 50)
                condicion += 4.50M;
            if (riesgo >= 51 && riesgo <= 75)
                condicion += 3.00M;
            if (riesgo >= 76)
                condicion += 0.00M;

            /*ponderación del plazo de crédito*/
            if (plazo_credito == 1)
                condicion += 5.00M;
            if (plazo_credito == 2)
                condicion += 4.58M;
            if (plazo_credito == 3)
                condicion += 4.17M;
            if (plazo_credito == 4)
                condicion += 3.74M;
            if (plazo_credito == 5)
                condicion += 3.32M;
            if (plazo_credito == 6)
                condicion += 2.90M;
            if (plazo_credito == 7)
                condicion += 2.48M;
            if (plazo_credito == 8)
                condicion += 2.06M;
            if (plazo_credito == 9)
                condicion += 1.64M;
            if (plazo_credito == 10)
                condicion += 1.22M;
            if (plazo_credito == 11)
                condicion += 0.80M;
            if (plazo_credito == 12)
                condicion += 0.42M;


            return condicion;
        }

        private decimal Calificar_Colateral(Evaluacion_Tecnica_Vista evaluacion)
        {
            decimal colateral = 0.00M;
            decimal monto_garantia_real = evaluacion.Evaluacion.Monto;
            decimal monto_credito = evaluacion.Evaluacion.Monto;

            /*única ponderación con las garantías*/
            if (monto_garantia_real >= monto_credito)
                colateral += 5.00M;

            return colateral;
        }
    }
}
