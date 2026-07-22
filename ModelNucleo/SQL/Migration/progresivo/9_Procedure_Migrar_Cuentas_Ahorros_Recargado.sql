/*cuentas de ahorros*/
insert into aycnucleo_alpha.ahorro(
	Id_Linea_Ahorro,
	Id_Persona,
	No_Ahorro,
	Saldo_Actual,
	Ultima_Fecha_Traslado,
	Fecha_Apertura,
	Frecuencia_Deposito,
	Mes_Capitalizable,
	Monto_Minimo_Ahorro,
	Plazo,
	Tasa_Interes,
    Fecha_Retiro
)
SELECT 		
	a.Id_Linea_Ahorro,/*id de la linea de ahorro*/
	p.Id_Persona,/*id de la la persona ahorrante*/
	s.CODIGOAHO,
	s.SALDO,
	s.ult_capitaliz,
	s.APERTURA,
	0,
	3,
    0,
	COALESCE(s.PLAZO,0),
	s.INTERES,
    '0000-00-00'
FROM administracion.salaho s
inner join aycnucleo_alpha.persona p on p.Codigo=substring(s.CODIGOAHO,1,7)
inner join aycnucleo_alpha.linea_ahorro a on a.Codigo=substring(s.CODIGOAHO,8,2)
group by s.CODIGOAHO,p.Codigo,s.APERTURA HAVING s.APERTURA=(select date_sub(curdate(),interval 1 day));
/*insertar los beneficiarios*/