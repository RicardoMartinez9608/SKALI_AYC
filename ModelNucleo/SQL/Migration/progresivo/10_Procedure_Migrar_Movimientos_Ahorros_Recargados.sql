

use aycnucleo_alpha;
DROP TEMPORARY TABLE IF EXISTS ultima_transaccion;
CREATE temporary TABLE ultima_transaccion(
	DIAS int,
    FECHA DATE,
    CODIGOAHO longtext,
    TIPO INT,
    INT_ACUM decimal,
    INT_APLIC DECIMAL,
    SALDO DECIMAL,
    VALOR DECIMAL
);

insert into ultima_transaccion(
	DIAS,
    FECHA,
    CODIGOAHO,
    TIPO,
    INT_ACUM,
    INT_APLIC,
    SALDO,
    VALOR
)

select 
m2.DIAS,
m2.FECHA,
m2.CODIGOAHO,
case
	when m2.TIPO=1 then 1
    when m2.TIPO=2 then 3
end as TIPO,
m2.INT_ACUM,
m2.INT_APLIC,
m2.SALDO,
m2.VALOR
from administracion.movaho m2
,(select
    max(m.INDICE) as INDICE
from administracion.movaho m 
group by m.CODIGOAHO) m1 
where m2.INDICE=m1.INDICE;

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
	
u.DIAS,
u.FECHA,
a.Id_Ahorro,
u.TIPO,
u.INT_ACUM,
u.INT_APLIC,
u.SALDO,
u.VALOR,
curdate(),
'Migra',
0

from aycnucleo_alpha.ultima_transaccion u
inner join aycnucleo_alpha.ahorro a on u.CODIGOAHO=a.No_Ahorro
AND m.FECHA=(select date_sub(curdate(),interval 1 day));

DROP TEMPORARY TABLE IF EXISTS ultima_transaccion;