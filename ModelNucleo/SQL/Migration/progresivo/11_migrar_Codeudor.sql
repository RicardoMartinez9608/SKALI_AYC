
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
        c.Id_Cartera,
        3
FROM administracion.maeasoc m
inner join aycnucleo_alpha.cartera c on c.Colector=m.COLECTOR
left join aycnucleo_alpha.persona p on m.DUI=p.Dui
where m.ACTIVO='A' and locate('F',m.CODIGO,1)=1 and m.DUI not in (select Dui from aycnucleo_alpha.persona)