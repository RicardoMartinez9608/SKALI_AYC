/*Insertar todas las personas clientes, fiadores y beneficiarios*/
insert into aycnucleo_alpha.persona(
	Codigo,
	Nombres,
	Apellidos,
	Domicilio,
	Sexo,
	FechaNacimiento,
	Ocupacion,
	Telefono,
	Dui,
	Nit,
	Nacionalidad,
	Correo,
	Id_Cartera,
    Estado_Socio
)
SELECT 	 distinct m.CODIGO,
		m.NOMBRE,
		m.APELLIDO,
		m.DIRECCION,
		
        COALESCE(m.SEXO,0),
		/*m.FECH_ING,
		m.FECH_RETI,*/
		cast(m.FECH_NACIM as datetime(6)) as FECH_NACIM,
		m.OCUPAC,
		m.TELCASA,
		/*m.TELOFIC,*/
		m.DUI,
		m.NIT,
		m.NACIONALIDAD,
		m.EMAIL,
        CASE
				WHEN COALESCE(c.Id_Cartera,0)=0
					then 6
                when COALESCE(c.Id_Cartera,0)>0
					then c.Id_Cartera
                end Id_Cartera,
        3
FROM administracion.maeasoc m
left join aycnucleo_alpha.cartera c on c.Colector=m.COLECTOR
where m.ACTIVO='A' and locate('F',m.CODIGO,1)=0 and m.FECH_ING= (select date_sub(curdate(),interval 1 day));

/*insertar los tipos de personas para separar los fiadores y beneficiarios de los clientes*/
/*insert into aycnucleo_alpha.persona_tipo_persona(
	Id_Persona,
	Id_Tipo_Persona
)
select
	p.Id_Persona,
    case
		when locate('F',p.CODIGO,1)>0 then 5
        when locate('F',p.CODIGO,1)=0 then 1
	end as Id_Tipo_Cliente_Formado
from aycnucleo_alpha.persona p 
inner join administracion.maeasoc m on m.CODIGO=p.CODIGO
where m.ACTIVO='A';*/

/*no funciono*/

/*delete p1 from aycnucleo_alpha.persona p1
    cross join aycnucleo_alpha.persona p2
    on p1.Dui=p2.Dui and p1.Id_Persona>p2.Id_Persona;*/