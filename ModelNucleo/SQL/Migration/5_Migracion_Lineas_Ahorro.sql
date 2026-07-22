insert into aycnucleo_alpha.linea_ahorro(
	Codigo,
    Descripcion,
    Tasa_Interes,
    Id_Tipo_Linea_Ahorro,
    Frecuencia_Deposito,
    Mes_Capitalizable,
    Plazo,
    Monto_Minimo_Ahorro
)
SELECT
	g.LINEA,
    g.DESCRIP,
	i.PORCENT,
    1,2,0,1,0
FROM administracion.glcuenta g
INNER JOIN administracion.intcte i ON i.TIPAHO=g.LINEA
where TIPO='3';

insert into aycnucleo_alpha.linea_ahorro(
	Codigo,
    Descripcion,
    Tasa_Interes,
    Id_Tipo_Linea_Ahorro,
    Frecuencia_Deposito,
    Mes_Capitalizable,
    Plazo,
    Monto_Minimo_Ahorro
) values(	'70',	'Corriente',	1,	2,	0,	1,	0,	1);

	/*(	'60',	'Programado',	2,	6,	0,	13,	0,	1),
    (	'70',	'Corriente',	1,	4,	0,	1,	0,	1),
    (	'61',	'Programado Corto Plazo',	2,	2,	0,	1,	0,	1),
    (	'71',	'Transitorio',	1,	4,	0,	1,	0,	1),
    (	'63',	'Corriente Exterior',	1,	4,	0,	1,	0,	1),
    (	'62',	'Programado Largo Plazo Extendido',	2,	4,	0,	1,	0,	1),
    (	'64',	'Programado 2 Extendido',	2,	4,	0,	1,	0,	1),
    (	'65',	'Programado Extendido L6',	2,	4,	0,	1,	0,	1),
    (	'66',	'Infantil C INT CP',	2,	4,	0,	1,	0,	1),
    (	'67',	'Infantil C INT LP',	2,	4,	0,	1,	0,	1),
    (	'68',	'Infantil C EXT. CP',	2,	4,	0,	1,	0,	1),
    (	'69',	'Infantil C LP',	2,	4,	0,	1,	0,	1),
    (	'80',	'Depositos Soc Comunes',	2,	4,	0,	1,	0,	1),
    (	'81',	'Depositos P Menos de Un',	2,	4,	0,	1,	0,	1),
    (	'82',	'Depositos Soc Comunes 2',	2,	4,	0,	1,	0,	1);*/
    