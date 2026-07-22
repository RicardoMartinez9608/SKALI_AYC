SET character_set_client = utf8;
insert into aycnucleo_alpha.descripcion_documento (Id_Descripcion_Documento,Descripcion)
values
	(1,"DUI"),
	(2,"NIT"),
    (3,"InfoRed"),
    (5,"Recibo Agua"),
    (6,"Recibo Luz"),
    (7,"Garantía Especial");
    


insert into 
	aycnucleo_alpha.linea_credito (
								Codigo,
                                Descripcion_Linea,
                                Dia_Gracia,
                                Interes,
                                Recargo,
                                Interes_Comision,
                                Tasa_Mora,
                                Tipo_Garantia,
                                Cancelacion_anticipada
							) 
                            select 
									LINEA,
                                    DESCRIP,
                                    GRACIA,
									
									CASE
										WHEN COBRO_COMISION >0.00 /*Nuevo*/
											THEN COBRO_COMISION
										WHEN COBRO_COMISION = null /*Recurrente*/
											THEN 0.00
									END as Comision
									
									,
                                    0.00,
                                    TASA_INTERES,
                                    INTERES_MORA,
                                    1 ,
                                    5.00
										from administracion.linea;

/*insertar manualmente la l'inea juridica*/
insert into aycnucleo_alpha.linea_credito(
	Codigo,
	Descripcion_Linea,
	Dia_Gracia,
	Interes,
	Recargo,
	Interes_Comision,
	Tasa_Mora,
	Tipo_Garantia
)
value(
	18,
    "Jurídico",
    0,
    2,
    0.00,
    0.00,
    0.00,
    1
);