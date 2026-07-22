/*SE INSERTA SOLO LOS CREDITOS*/
insert into aycnucleo_alpha.credito(
					Codigo_Credito,
					Monto_pago,
					Valor_Cuota,
					Liquidado_En,
					Plazo,
					Interes_Comision,
					Interes,
					Destino_Credito,
					Numero_cuotas,
					Aprobado,
					Cancelado,
					Tasa_Mora,
					Id_Persona,
					Id_Linea,
                    Aprobado_En,
                    Cuota_Total,
                    Valor_IVA,
                    Id_Periodo,
                    Liquidado,
                    Recargo,
                    Id_Cartera,
					Id_Comportamiento_Credito
				)
SELECT 		
			
			s.CODIGOPRE,
			s.MONTO,
            CASE
				WHEN COALESCE(l.Interes,0)>0 
					THEN COALESCE((select Cuota(s.MONTO,s.NUMCUOTAS,l.Interes,s.PLAZO)),0.00)
				WHEN COALESCE(l.Interes,0)=0
					THEN 0.00
			END as cuota_pequena,/*debe ir la cuota sin plan ayc*/
            s.APERTURA,
            s.PLAZO,
            s.INTERES,      /*COALESCE(((d.VALOR/s.CUOTA)*s.INTERES),0.00) as Interes,/*Es el interes sin plan ayc*/
            /*CASE
				WHEN s.INTERES>COALESCE(l.Interes,0) and COALESCE(l.Interes,0)>0 
					THEN COALESCE(s.INTERES-l.Interes,0.00)
				WHEN s.INTERES<COALESCE(l.Interes,0) and COALESCE(l.Interes,0)>0
					THEN COALESCE(l.Interes-s.INTERES,0.00)
				WHEN COALESCE(l.Interes,0)=0
					THEN 0.00
			END*/
            
            COALESCE(l.Interes,0) as PlanAyC,				/*(d.MANEJO/1.13) as PlanAyC,/*Es el valor en dinero del plan ayc*/
            s.DESTINO,
            s.NUMCUOTAS,
            1,
            0,
            s.INT_MORA,
            p.Id_Persona,/*Consultar Persona para obtener el dato*/
            l.Id_Linea,/*Consultar la linea de credito*/
            s.APERTURA,
            CASE
				WHEN s.INTERES>0 
					THEN COALESCE((select Cuota(s.MONTO,s.NUMCUOTAS,l.Interes,s.PLAZO))+((select Cuota(s.MONTO,s.NUMCUOTAS,s.INTERES,s.PLAZO))-(select Cuota(s.MONTO,s.NUMCUOTAS,l.Interes,s.PLAZO))+((((select Cuota(s.MONTO,s.NUMCUOTAS,s.INTERES,s.PLAZO))-(select Cuota(s.MONTO,s.NUMCUOTAS,l.Interes,s.PLAZO)))*0.13))),0.00)
				WHEN s.INTERES=0
					THEN 0.00
			END as cuota_grande,
			CASE
				WHEN s.INTERES>COALESCE(l.Interes,0) and COALESCE(l.Interes,0)>0
					THEN COALESCE(((select Cuota(s.MONTO,s.NUMCUOTAS,s.INTERES,s.PLAZO))-(select Cuota(s.MONTO,s.NUMCUOTAS,l.Interes,s.PLAZO)))*0.13,0.00)
				WHEN s.INTERES<COALESCE(l.Interes,0) and COALESCE(l.Interes,0)>0
					THEN COALESCE(((select Cuota(s.MONTO,s.NUMCUOTAS,l.Interes,s.PLAZO))-(select Cuota(s.MONTO,s.NUMCUOTAS,s.INTERES,s.PLAZO)))*0.13,0.00)
				WHEN COALESCE(l.Interes,0)=0
					THEN 0.00
				ELSE 0.00
			END as IVAPlanAyC,				/*d.MANEJO-(d.MANEJO/1.13) as IvaPorPlanAyC,/*Calcular el iva del plan ayc*/
            CASE
				WHEN s.TIP_PAGO = 6 THEN 3
                WHEN s.TIP_PAGO < 6 THEN s.TIP_PAGO
			END as Id_Periodo,/*consultar el id_periodo de la tabla periodo*/
            1,
            d.Recargo,
            c.Id_Cartera,
			CASE
				WHEN SUBSTR(s.CODIGOPRE,11,1)='1' /*Nuevo*/
					THEN 1
				WHEN SUBSTR(s.CODIGOPRE,11,1)<>'1' /*Recurrente*/
					THEN 3 
				ELSE 2 /*Recuperado*/
			END as Id_Comportamiento_Credito
FROM 
	administracion.salpres s
INNER JOIN aycnucleo_alpha.cartera c on c.Colector=s.COLECTOR
/*INNER JOIN administracion.maeasoc m ON m.CODIGO=substring(s.CODIGOPRE,1,7)*/
INNER JOIN aycnucleo_alpha.persona p ON /*p.Dui=m.DUI*/ p.Codigo=substring(s.CODIGOPRE,1,7)
INNER JOIN aycnucleo_alpha.linea_credito l ON l.Codigo=s.DESTINTIP
INNER JOIN administracion.debser d ON d.CODIGOPRE=s.CODIGOPRE
WHERE s.SALDO>0 AND d.MES=1 and s.APERTURA=(select date_sub(curdate(),interval 1 day));

/*SE INSERTA LOS DATOS A LA LINEA-CREDITO-HISTORIAL*/
insert into aycnucleo_alpha.linea_credito_historial(
		Activar_Deducciones,
        Codigo,
        Descripcion_linea,
        Destino_Credito,
        Dia_Gracia,
        Id_Credito,
        Monto_Rango_Inicial,
        Monto_Rango_Final,
        Interes,
        PlanAyC_Cobros_Periodico_Dinero,
        Plazo_Rango_Inicial,
        Plazo_Rango_Final,
        Recargo,
        Interes_Comision,
        Tasa_Mora,
        Tipo_Garantia,
        Cancelacion_anticipada
    )

select
	0,
	l.Codigo,
    l.Descripcion_linea,
	l.Destino_Credito,
    l.Dia_Gracia,
    c.Id_Credito,
    0.00,
    0.00,
    l.Interes as PlanAyC,/*es el porcentaje del plan ayc*/
    0,
    0,
    0,
    c.Recargo,
    c.Interes_Comision,
    c.Tasa_Mora,
    1,
    5.00
from
	aycnucleo_alpha.credito c
right join aycnucleo_alpha.linea_credito l on l.Id_Linea=c.Id_Linea
left join administracion.salpres s on s.CODIGOPRE=c.Codigo_Credito;