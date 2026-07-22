insert into aycnucleo_alpha.transaccion_ahorro(
	Dias,
	Fecha,
	Id_Ahorro,
	Id_Tipo_Operacion_Ahorro,
	Interes_Acumulado,
	Interes_Aplicado,
	Saldo,
	Valor,
	Creado_En,
	Creado_Por,
    Borrado_Logico
)
SELECT
	m.DIAS,
	m.FECHA,
    a.Id_Ahorro,
    case
		when m.TIPO=1 then 1
        when m.TIPO=2 then 3
        when m.COMPROBANT='999999' then 2
	end as Id_Tipo_Operacion_Ahoro,/*hacer el calculo de las operaciones*/
	m.INT_ACUM,
    m.INT_APLIC,
    m.SALDO,
    m.VALOR,
    curdate(),
    'Migra',
    0
FROM administracion.movaho m
left join aycnucleo_alpha.ahorro a on a.No_Ahorro=m.CODIGOAHO
where COALESCE(a.Id_Ahorro) is not null;