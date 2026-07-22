
insert into aycnucleo_alpha.transaccion_credito(
		Abono_capital,
		Dias,
		FechaPago,
		Id_Credito,
		Id_Tipo_Operecion_Credito,
		Interes_Comision,
		IvaSobreComision,
		No,
		Interes,
		Saldo_Capital,
		Valor_Cuota,
		Recargo)
        
SELECT 		
		s.P110401,
        s.DIAS,
        s.FECHA,
        c.Id_Credito,
        1,
        COALESCE(s.INT_APLIC,0),
        COALESCE(s.MANEJO-(s.MANEJO/1.13),0),
        s.COMPROBANT,
        COALESCE((s.MANEJO/1.13),0),
        s.SALDO,
        s.VALOR,
        s.COBRANZA
FROM administracion.movpres s
inner join aycnucleo_alpha.credito c on c.Codigo_Credito=s.CODIGOPRE
WHERE s.COMPROBANT!=1;