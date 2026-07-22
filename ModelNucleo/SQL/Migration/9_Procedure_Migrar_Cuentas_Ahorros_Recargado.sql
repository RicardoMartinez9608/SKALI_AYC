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
group by s.CODIGOAHO,p.Codigo;
/*insertar los beneficiarios*/

		
		
/*beneficiario 1*/
-- insert into aycnucleo_alpha.referencia(
	-- Nombre,
    -- Residencia,
    -- Telefono,
    -- Edad,
    -- Porcentaje,
    -- Beneficiario
-- )
-- select 
	-- s.BENEF1,
    -- s.DIREC1,
    -- '',
    -- s.EDAD1,
    -- 0.00,
    -- 1
-- from administracion.salaho s
-- inner join aycnucleo_alpha.persona p on p.Codigo=substring(s.CODIGOAHO,1,7)
-- where s.BENEF1!='';

-- insert into aycnucleo_alpha.persona_referencia(
	-- Id_Persona,
    -- Id_Referencia,
    -- Parentesco,
    -- Tipo_Referencia
-- )
-- select 
	-- p.Id_Persona,
    -- r.Id_Referencia,
    -- 0,
    -- 0
-- from administracion.salaho s
-- inner join aycnucleo_alpha.persona p on p.Codigo=substring(s.CODIGOAHO,1,7)
-- inner join aycnucleo_alpha.referencia r on
-- (
		-- r.Nombre=s.BENEF1 and
        -- r.Residencia=s.DIREC1 and
        -- r.Edad=s.EDAD1);
        
-- /*beneficiario 2*/
-- insert into aycnucleo_alpha.referencia(
	-- Nombre,
    -- Residencia,
    -- Telefono,
    -- Edad,
    -- Porcentaje,
    -- Beneficiario
-- )
-- select 
	-- s.BENEF2,
    -- s.DIREC2,
    -- '',
    -- s.EDAD2,
    -- 0.00,
    -- 1
-- from administracion.salaho s
-- inner join aycnucleo_alpha.persona p on p.Codigo=substring(s.CODIGOAHO,1,7)
-- where s.BENEF2!='';

-- insert into aycnucleo_alpha.persona_referencia(
	-- Id_Persona,
    -- Id_Referencia,
    -- Parentesco,
    -- Tipo_Referencia
-- )
-- select 
	-- p.Id_Persona,
    -- r.Id_Referencia,
    -- 0,
    -- 0
-- from administracion.salaho s
-- inner join aycnucleo_alpha.persona p on p.Codigo=substring(s.CODIGOAHO,1,7)
-- inner join aycnucleo_alpha.referencia r on
-- (
		-- r.Nombre=s.BENEF2 and
        -- r.Residencia=s.DIREC2 and
        -- r.Edad=s.EDAD2);
        
-- /*beneficiario 3*/
-- insert into aycnucleo_alpha.referencia(
	-- Nombre,
    -- Residencia,
    -- Telefono,
    -- Edad,
    -- Porcentaje,
    -- Beneficiario
-- )
-- select 
	-- s.BENEF3,
    -- s.DIREC3,
    -- '',
    -- s.EDAD3,
    -- 0.00,
    -- 1
-- from administracion.salaho s
-- inner join aycnucleo_alpha.persona p on p.Codigo=substring(s.CODIGOAHO,1,7)
-- where s.BENEF3!='';

-- insert into aycnucleo_alpha.persona_referencia(
	-- Id_Persona,
    -- Id_Referencia,
    -- Parentesco,
    -- Tipo_Referencia
-- )
-- select 
	-- p.Id_Persona,
    -- r.Id_Referencia,
    -- 0,
    -- 0
-- from administracion.salaho s
-- inner join aycnucleo_alpha.persona p on p.Codigo=substring(s.CODIGOAHO,1,7)
-- inner join aycnucleo_alpha.referencia r on
-- (
		-- r.Nombre=s.BENEF3 and
        -- r.Residencia=s.DIREC3 and
        -- r.Edad=s.EDAD3);
        
        
-- /*beneficiario 4*/
-- insert into aycnucleo_alpha.referencia(
	-- Nombre,
    -- Residencia,
    -- Telefono,
    -- Edad,
    -- Porcentaje,
    -- Beneficiario
-- )
-- select 
	-- s.BENEF4,
    -- s.DIREC4,
    -- '',
    -- s.EDAD4,
    -- 0.00,
    -- 1
-- from administracion.salaho s
-- inner join aycnucleo_alpha.persona p on p.Codigo=substring(s.CODIGOAHO,1,7)
-- where s.BENEF4!='';

-- insert into aycnucleo_alpha.persona_referencia(
	-- Id_Persona,
    -- Id_Referencia,
    -- Parentesco,
    -- Tipo_Referencia
-- )
-- select 
	-- p.Id_Persona,
    -- r.Id_Referencia,
    -- 0,
    -- 0
-- from administracion.salaho s
-- inner join aycnucleo_alpha.persona p on p.Codigo=substring(s.CODIGOAHO,1,7)
-- inner join aycnucleo_alpha.referencia r on
-- (
		-- r.Nombre=s.BENEF4 and
        -- r.Residencia=s.DIREC4 and
        -- r.Edad=s.EDAD4);