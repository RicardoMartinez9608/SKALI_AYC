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
where 
	m.ACTIVO='A' and 
    locate('F',m.CODIGO,1)=0 and
    
    m.CODIGO in("0225801","0006000","0265101","0001200");
