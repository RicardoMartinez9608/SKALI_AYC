insert into aycnucleo.persona(
	Codigo,
	Nombres,
	Apellidos,
	Direccion,
	Sexo,
	FechaNacimiento,
	Ocupacion,
	Telefono,
	Dui,
	Nit,
	Nacionalidad,
	Correo,
	Id_Cartera
)
SELECT 	m.CODIGO,
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
        c.Id_Cartera
FROM administracion.maeasoc m
inner join administracion.salpres s on (s.FIADOR1=m.CODIGO or s.FIADOR2=m.CODIGO)
inner join aycnucleo.cartera c on c.No=m.COLECTOR
where m.ACTIVO='A' and s.SALDO>0 and locate('F',m.CODIGO,1)>0;

/*insertar los tipos de personas para separar los fiadores y beneficiarios de los clientes*/
insert into aycnucleo.persona_tipo_persona(
	Id_Persona,
	Id_Tipo_Persona
)
select
	p.Id_Persona,
    case
		when locate('F',p.CODIGO,1)>0 then 3
        when locate('F',p.CODIGO,1)=0 then 1
	end as Id_Tipo_Cliente_Formado
from aycnucleo.persona p 
inner join administracion.maeasoc m on m.CODIGO=p.CODIGO
where m.ACTIVO='A';

insert into aycnucleo.persona_relaciones(
		Id_Sujeto,
		Id_Predicado,
		Id_Tipo_Relacion,
		Activo,
		Id_Ahorro,
		Id_Credito,
		Porcentaje_Beneficiario)
select 
	sujeto.Id_Persona,
    predicado.Id_Persona,
    5,
    0,
    0,
    c.Id_Credito,
    0
from aycnucleo.persona sujeto
inner join administracion.maeasoc sujeto_viejo on sujeto_viejo.DUI=sujeto.Dui
inner join administracion.salpres s on (s.FIADOR1=sujeto_viejo.CODIGO or s.FIADOR2=sujeto_viejo.CODIGO)
inner join aycnucleo.credito c on c.Codigo_Credito=s.CODIGOPRE
inner join aycnucleo.persona predicado on substring(s.CODIGOPRE,1,7)=predicado.CODIGO
where predicado.Nombres!='';
