use aycnucleo_alpha;

/*Administrativa*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(92,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente interno Administrativa, Empleados 01,00",1,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(328,92,"DEPOSITO","Depósito Administrativa Empleados 01,00",0,1,"2120401001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(93,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente externo Administrativa, Empleados 01,00",1,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(329,93,"DEPOSITO","Depósito Administrativa corriente externo 01,00",0,1,"2120501002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(94,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Interno Administrativa, Empleados 01,00",1,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(330,94,"DEPOSITO","Depósito Administrativa Corto Plazo Interno 01,00",0,1,"2120501003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(95,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Administrativa, Empleados 01,00",1,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(331,95,"DEPOSITO","Depósito Administrativa Corto Plazo Externo 01,00",0,1,"2120501003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(96,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Administrativa, Empleados 01,00",1,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(332,96,"DEPOSITO","Depósito Administrativa Corto Plazo Externo 01,00",0,1,"2120501004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(97,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Interno Administrativa, Empleados 01,00",1,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(333,97,"DEPOSITO","Depósito Administrativa Largo Plazo Interno 01,00",0,1,"2120501001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(98,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Externo Administrativa, Empleados 01,00",1,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(334,98,"DEPOSITO","Depósito Administrativa Largo Plazo Externo 01,00",0,1,"2120501002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(99,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Interno Administrativa, Empleados 01,00",1,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(335,99,"DEPOSITO","Depósito Administrativa Infantil Largo Plazo Externo 01,00",0,1,"2120501005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(101,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Interno Administrativa, Empleados 01,00",1,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(336,101,"DEPOSITO","Depósito Administrativa Infantil Largo Plazo Externo 01,00",0,1,"2120501006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(102,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Externo Administrativa, Empleados 01,00",1,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(337,102,"DEPOSITO","Depósito Administrativa Infantil Corto Plazo Externo 01,00",0,1,"2120501007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(103,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Externo Administrativa, Empleados 01,00",1,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(338,103,"DEPOSITO","Depósito Administrativa Infantil Largo Plazo Externo 01,00",0,1,"2120501008 ");
        


/*Retiros*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(104,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente interno Administrativa, Empleados 01,00",1,16,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(339,104,"RETIRO","Retiro Administrativa Empleados 01,00",1,0,"2120401001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(105,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente externo Administrativa, Empleados 01,00",1,3,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(340,105,"RETIRO","Retiro Administrativa corriente externo 01,00",1,0,"2120501002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(106,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Interno Administrativa, Empleados 01,00",1,4,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(341,106,"RETIRO","Retiro Administrativa Corto Plazo Interno 01,00",1,0,"2120501003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(107,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Administrativa, Empleados 01,00",1,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(342,107,"RETIRO","Retiro Administrativa Corto Plazo Externo 01,00",1,0,"2120501003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(108,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Administrativa, Empleados 01,00",1,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(343,108,"RETIRO","Retiro Administrativa Corto Plazo Externo 01,00",1,0,"2120501004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(109,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Interno Administrativa, Empleados 01,00",1,2,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(344,109,"RETIRO","Retiro Administrativa Largo Plazo Interno 01,00",1,0,"2120501001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(110,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Externo Administrativa, Empleados 01,00",1,6,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(345,110,"RETIRO","Retiro Administrativa Largo Plazo Externo 01,00",1,0,"2120501002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(111,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Interno Administrativa, Empleados 01,00",1,7,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(346,111,"RETIRO","Retiro Administrativa Infantil Largo Plazo Externo 01,00",1,0,"2120501005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(112,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Interno Administrativa, Empleados 01,00",1,8,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(347,112,"RETIRO","Retiro Administrativa Infantil Largo Plazo Externo 01,00",1,0,"2120501006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(113,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Externo Administrativa, Empleados 01,00",1,9,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(348,113,"RETIRO","Depósito Administrativa Infantil Corto Plazo Externo 01,00",1,0,"2120501007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(114,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Externo Administrativa, Empleados 01,00",1,10,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(349,114,"RETIRO","Retiro Administrativa Infantil Largo Plazo Externo 01,00",1,0,"2120501008 ");
        
        

/*Depósito Interés*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(115,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente interno Administrativa, Empleados 01,00",1,16,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(350,115,"DEPOSITO_INTERES","Depósito Interés Administrativa Empleados 01,00",1,0,"2120401001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(351,115,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(116,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente externo Administrativa, Empleados 01,00",1,3,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(352,116,"DEPOSITO_INTERES","Depósito Interés Administrativa corriente externo 01,00",1,0,"2120501002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(353,116,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(117,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Interno Administrativa, Empleados 01,00",1,4,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(354,117,"DEPOSITO_INTERES","Depósito Interés Administrativa Corto Plazo Interno 01,00",1,0,"2120501003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(355,117,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(118,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Administrativa, Empleados 01,00",1,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(356,118,"DEPOSITO_INTERES","Depósito Interés Administrativa Corto Plazo Externo 01,00",1,0,"2120501003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(357,118,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(119,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Administrativa, Empleados 01,00",1,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(358,119,"DEPOSITO_INTERES","Depósito Interés Administrativa Corto Plazo Externo 01,00",1,0,"2120501004");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(359,119,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(120,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Interno Administrativa, Empleados 01,00",1,2,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(360,120,"DEPOSITO_INTERES","Depósito Interés Administrativa Largo Plazo Interno 01,00",1,0,"2120501001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(361,120,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(121,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Externo Administrativa, Empleados 01,00",1,6,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(362,121,"DEPOSITO_INTERES","Depósito Interés Administrativa Largo Plazo Externo 01,00",1,0,"2120501002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(363,121,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(122,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Interno Administrativa, Empleados 01,00",1,7,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(364,122,"DEPOSITO_INTERES","Depósito Interés Administrativa Infantil Largo Plazo Externo 01,00",1,0,"2120501005");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(365,122,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(123,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Interno Administrativa, Empleados 01,00",1,8,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(366,123,"DEPOSITO_INTERES","Retiro Administrativa Infantil Largo Plazo Externo 01,00",1,0,"2120501006");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(367,123,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(124,"DEPOSITO_INTERES_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Externo Administrativa, Empleados 01,00",1,9,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(368,124,"DEPOSITO_INTERES","Depósito Administrativa Infantil Corto Plazo Externo 01,00",1,0,"2120501007");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(369,124,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(125,"DEPOSITO_INTERES_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Externo Administrativa, Empleados 01,00",1,10,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(370,125,"DEPOSITO_INTERES","Retiro Administrativa Infantil Largo Plazo Externo 01,00",1,0,"2120501008 ");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(371,125,"GASTO_INTERES","Gasto por Depósito Interés Administrativa Empleados 01,00",0,1,"4110101001");
        
        


/*Ahuachapán-------------------------------------------------------------------------*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(126,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente interno Ahuachapán, Empleados 02,01",2,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(372,126,"DEPOSITO","Depósito Ahuachapán Empleados 02,01",0,1,"2120402001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(127,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente externo Ahuachapán, Empleados 02,01",2,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(373,127,"DEPOSITO","Depósito Ahuachapán corriente externo 02,01",0,1,"2120502002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(128,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Interno Ahuachapán, Empleados 02,01",2,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(374,128,"DEPOSITO","Depósito Ahuachapán Corto Plazo Interno 02,01",0,1,"2120502003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(129,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Ahuachapán, Empleados 02,01",2,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(375,129,"DEPOSITO","Depósito Ahuachapán Corto Plazo Externo 02,01",0,1,"2120502003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(130,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Ahuachapán, Empleados 02,01",2,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(376,130,"DEPOSITO","Depósito Ahuachapán Corto Plazo Externo 02,01",0,1,"2120502004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(131,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Interno Ahuachapán, Empleados 02,01",2,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(377,131,"DEPOSITO","Depósito Ahuachapán Largo Plazo Interno 02,01",0,1,"2120502001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(132,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Externo Ahuachapán, Empleados 02,01",2,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(378,132,"DEPOSITO","Depósito Ahuachapán Largo Plazo Externo 02,01",0,1,"2120502002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(133,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Interno Ahuachapán, Empleados 02,01",2,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(379,133,"DEPOSITO","Depósito Ahuachapán Infantil Largo Plazo Externo 02,01",0,1,"2120502005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(134,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Interno Ahuachapán, Empleados 02,01",2,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(380,134,"DEPOSITO","Depósito Ahuachapán Infantil Largo Plazo Externo 02,01",0,1,"2120502006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(135,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Externo Ahuachapán, Empleados 02,01",2,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(381,135,"DEPOSITO","Depósito Ahuachapán Infantil Corto Plazo Externo 02,01",0,1,"2120502007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(136,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Externo Ahuachapán, Empleados 02,01",2,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(382,136,"DEPOSITO","Depósito Ahuachapán Infantil Largo Plazo Externo 02,01",0,1,"2120502008 ");
        


/*Retiros*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(137,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente interno Ahuachapán, Empleados 02,01",2,16,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(383,137,"RETIRO","Retiro Ahuachapán Empleados 02,01",1,0,"2120402001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(138,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente externo Ahuachapán, Empleados 02,01",2,3,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(384,138,"RETIRO","Retiro Ahuachapán corriente externo 02,01",1,0,"2120502002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(139,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Interno Ahuachapán, Empleados 02,01",2,4,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(385,139,"RETIRO","Retiro Ahuachapán Corto Plazo Interno 02,01",1,0,"2120502003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(140,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Ahuachapán, Empleados 02,01",2,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(386,140,"RETIRO","Retiro Ahuachapán Corto Plazo Externo 02,01",1,0,"2120502003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(141,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Ahuachapán, Empleados 02,01",2,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(387,141,"RETIRO","Retiro Ahuachapán Corto Plazo Externo 02,01",1,0,"2120502004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(142,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Interno Ahuachapán, Empleados 02,01",2,2,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(388,142,"RETIRO","Retiro Ahuachapán Largo Plazo Interno 02,01",1,0,"2120502001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(143,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Externo Ahuachapán, Empleados 02,01",2,6,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(389,143,"RETIRO","Retiro Ahuachapán Largo Plazo Externo 02,01",1,0,"2120502002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(144,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Interno Ahuachapán, Empleados 02,01",2,7,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(390,144,"RETIRO","Retiro Ahuachapán Infantil Largo Plazo Externo 02,01",1,0,"2120502005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(145,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Interno Ahuachapán, Empleados 02,01",2,8,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(391,145,"RETIRO","Retiro Ahuachapán Infantil Largo Plazo Externo 02,01",1,0,"2120502006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(146,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Externo Ahuachapán, Empleados 02,01",2,9,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(392,146,"RETIRO","Depósito Ahuachapán Infantil Corto Plazo Externo 02,01",1,0,"2120502007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(147,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Externo Ahuachapán, Empleados 02,01",2,10,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(393,147,"RETIRO","Retiro Ahuachapán Infantil Largo Plazo Externo 02,01",1,0,"2120502008 ");
        
        

/*Depósito Interés*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(148,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente interno Ahuachapán, Empleados 02,01",2,16,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(394,148,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Empleados 02,01",1,0,"2120402001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(395,148,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(149,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente externo Ahuachapán, Empleados 02,01",2,3,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(396,149,"DEPOSITO_INTERES","Depósito Interés Ahuachapán corriente externo 02,01",1,0,"2120502002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(397,149,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(150,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Interno Ahuachapán, Empleados 02,01",2,4,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(398,150,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Corto Plazo Interno 02,01",1,0,"2120502003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(399,150,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(151,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Ahuachapán, Empleados 02,01",2,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(400,151,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Corto Plazo Externo 02,01",1,0,"2120502003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(401,151,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(152,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Ahuachapán, Empleados 02,01",2,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(402,152,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Corto Plazo Externo 02,01",1,0,"2120502004");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(403,152,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(153,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Interno Ahuachapán, Empleados 02,01",2,2,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(404,153,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Largo Plazo Interno 02,01",1,0,"2120502001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(405,153,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(154,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Externo Ahuachapán, Empleados 02,01",2,6,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(406,154,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Largo Plazo Externo 02,01",1,0,"2120502002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(407,154,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(155,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Interno Ahuachapán, Empleados 02,01",2,7,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(408,155,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Infantil Largo Plazo Externo 02,01",1,0,"2120502005");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(409,155,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(156,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Interno Ahuachapán, Empleados 02,01",2,8,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(410,156,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Infantil Largo Plazo Externo 02,01",1,0,"2120502006");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(411,156,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(157,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Externo Ahuachapán, Empleados 02,01",2,9,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(412,157,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Infantil Corto Plazo Externo 02,01",1,0,"2120502007");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(413,157,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(158,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Externo Ahuachapán, Empleados 02,01",2,10,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(414,158,"DEPOSITO_INTERES","Depósito Interés Ahuachapán Infantil Largo Plazo Externo 02,01",1,0,"2120502008 ");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(415,158,"GASTO_INTERES","Gasto por Depósito Interés Ahuachapán Empleados 02,01",0,1,"4110102001");
        

/*Chalchuapa-------------------------------------------------------------------------*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(159,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente interno Chalchuapa, Empleados 03,02",3,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(416,159,"DEPOSITO","Depósito Chalchuapa Empleados 03,02",0,1,"2120403001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(160,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente externo Chalchuapa, Empleados 03,02",3,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(417,160,"DEPOSITO","Depósito Chalchuapa corriente externo 03,02",0,1,"2120503002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(161,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Interno Chalchuapa, Empleados 03,02",3,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(418,161,"DEPOSITO","Depósito Chalchuapa Corto Plazo Interno 03,02",0,1,"2120503003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(162,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Chalchuapa, Empleados 03,02",3,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(419,162,"DEPOSITO","Depósito Chalchuapa Corto Plazo Externo 03,02",0,1,"2120503003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(163,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Chalchuapa, Empleados 03,02",3,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(420,163,"DEPOSITO","Depósito Chalchuapa Corto Plazo Externo 03,02",0,1,"2120503004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(164,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Interno Chalchuapa, Empleados 03,02",3,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(421,164,"DEPOSITO","Depósito Chalchuapa Largo Plazo Interno 03,02",0,1,"2120503001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(165,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Externo Chalchuapa, Empleados 03,02",3,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(422,165,"DEPOSITO","Depósito Chalchuapa Largo Plazo Externo 03,02",0,1,"2120503002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(166,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Interno Chalchuapa, Empleados 03,02",3,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(423,166,"DEPOSITO","Depósito Chalchuapa Infantil Largo Plazo Externo 03,02",0,1,"2120503005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(167,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Interno Chalchuapa, Empleados 03,02",3,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(424,167,"DEPOSITO","Depósito Chalchuapa Infantil Largo Plazo Externo 03,02",0,1,"2120503006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(168,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Externo Chalchuapa, Empleados 03,02",3,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(425,168,"DEPOSITO","Depósito Chalchuapa Infantil Corto Plazo Externo 03,02",0,1,"2120503007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(169,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Externo Chalchuapa, Empleados 03,02",3,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(426,169,"DEPOSITO","Depósito Chalchuapa Infantil Largo Plazo Externo 03,02",0,1,"2120503008 ");
        


/*Retiros*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(170,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente interno Chalchuapa, Empleados 03,02",3,16,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(427,170,"RETIRO","Retiro Chalchuapa Empleados 03,02",1,0,"2120403001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(171,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente externo Chalchuapa, Empleados 03,02",3,3,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(428,171,"RETIRO","Retiro Chalchuapa corriente externo 03,02",1,0,"2120503002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(172,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Interno Chalchuapa, Empleados 03,02",3,4,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(429,172,"RETIRO","Retiro Chalchuapa Corto Plazo Interno 03,02",1,0,"2120503003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(173,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Chalchuapa, Empleados 03,02",3,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(430,173,"RETIRO","Retiro Chalchuapa Corto Plazo Externo 03,02",1,0,"2120503003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(174,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Chalchuapa, Empleados 03,02",3,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(431,174,"RETIRO","Retiro Chalchuapa Corto Plazo Externo 03,02",1,0,"2120503004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(175,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Interno Chalchuapa, Empleados 03,02",3,2,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(432,175,"RETIRO","Retiro Chalchuapa Largo Plazo Interno 03,02",1,0,"2120503001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(176,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Externo Chalchuapa, Empleados 03,02",3,6,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(433,176,"RETIRO","Retiro Chalchuapa Largo Plazo Externo 03,02",1,0,"2120503002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(177,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Interno Chalchuapa, Empleados 03,02",3,7,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(434,177,"RETIRO","Retiro Chalchuapa Infantil Largo Plazo Externo 03,02",1,0,"2120503005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(178,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Interno Chalchuapa, Empleados 03,02",3,8,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(435,178,"RETIRO","Retiro Chalchuapa Infantil Largo Plazo Externo 03,02",1,0,"2120503006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(179,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Externo Chalchuapa, Empleados 03,02",3,9,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(436,179,"RETIRO","Depósito Chalchuapa Infantil Corto Plazo Externo 03,02",1,0,"2120503007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(180,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Externo Chalchuapa, Empleados 03,02",3,10,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(437,180,"RETIRO","Retiro Chalchuapa Infantil Largo Plazo Externo 03,02",1,0,"2120503008 ");
        
        

/*Depósito Interés*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(181,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente interno Chalchuapa, Empleados 03,02",3,16,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(438,181,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Empleados 03,02",1,0,"2120403001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(439,181,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(182,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente externo Chalchuapa, Empleados 03,02",3,3,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(440,182,"DEPOSITO_INTERES","Depósito Interés Chalchuapa corriente externo 03,02",1,0,"2120503002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(441,182,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(183,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Interno Chalchuapa, Empleados 03,02",3,4,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(442,183,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Corto Plazo Interno 03,02",1,0,"2120503003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(443,183,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(184,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Chalchuapa, Empleados 03,02",3,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(444,184,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Corto Plazo Externo 03,02",1,0,"2120503003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(445,184,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(185,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Chalchuapa, Empleados 03,02",3,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(446,185,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Corto Plazo Externo 03,02",1,0,"2120503004");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(447,185,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(186,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Interno Chalchuapa, Empleados 03,02",3,2,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(448,186,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Largo Plazo Interno 03,02",1,0,"2120503001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(449,186,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(187,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Externo Chalchuapa, Empleados 03,02",3,6,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(450,187,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Largo Plazo Externo 03,02",1,0,"2120503002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(451,187,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(188,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Interno Chalchuapa, Empleados 03,02",3,7,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(452,188,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Infantil Largo Plazo Externo 03,02",1,0,"2120503005");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(453,188,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(189,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Interno Chalchuapa, Empleados 03,02",3,8,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(454,189,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Infantil Largo Plazo Externo 03,02",1,0,"2120503006");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(455,189,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(190,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Externo Chalchuapa, Empleados 03,02",3,9,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(456,190,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Infantil Corto Plazo Externo 03,02",1,0,"2120503007");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(457,190,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(191,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Externo Ahuachapán, Empleados 03,02",3,10,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(458,191,"DEPOSITO_INTERES","Depósito Interés Chalchuapa Infantil Largo Plazo Externo 03,02",1,0,"2120503008 ");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(459,191,"GASTO_INTERES","Gasto por Depósito Interés Chalchuapa Empleados 03,02",0,1,"4110103001");
        
        
/*Santa Ana-------------------------------------------------------------------------*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(192,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente interno Santa Ana, Empleados 04,03",4,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(460,192,"DEPOSITO","Depósito Santa Ana Empleados 04,03",0,1,"2120404001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(193,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente externo Santa Ana, Empleados 04,03",4,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(461,193,"DEPOSITO","Depósito Santa Ana corriente externo 04,03",0,1,"2120504002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(194,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Interno Santa Ana, Empleados 04,03",4,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(462,194,"DEPOSITO","Depósito Santa Ana Corto Plazo Interno 04,03",0,1,"2120504003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(195,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Santa Ana, Empleados 04,03",4,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(463,195,"DEPOSITO","Depósito Santa Ana Corto Plazo Externo 04,03",0,1,"2120504003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(196,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Santa Ana, Empleados 04,03",4,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(464,196,"DEPOSITO","Depósito Santa Ana Corto Plazo Externo 04,03",0,1,"2120504004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(197,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Interno Santa Ana, Empleados 04,03",4,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(465,197,"DEPOSITO","Depósito Santa Ana Largo Plazo Interno 04,03",0,1,"2120504001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(198,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Externo Santa Ana, Empleados 04,03",4,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(466,198,"DEPOSITO","Depósito Santa Ana Largo Plazo Externo 04,03",0,1,"2120504002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(199,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Interno Santa Ana, Empleados 04,03",4,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(467,199,"DEPOSITO","Depósito Santa Ana Infantil Largo Plazo Externo 04,03",0,1,"2120504005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(200,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Interno Santa Ana, Empleados 04,03",4,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(468,200,"DEPOSITO","Depósito Santa Ana Infantil Largo Plazo Externo 04,03",0,1,"2120504006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(201,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Externo Santa Ana, Empleados 04,03",4,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(469,201,"DEPOSITO","Depósito Santa Ana Infantil Corto Plazo Externo 04,03",0,1,"2120504007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(202,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Externo Santa Ana, Empleados 04,03",4,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(470,202,"DEPOSITO","Depósito Santa Ana Infantil Largo Plazo Externo 04,03",0,1,"2120504008 ");
        


/*Retiros*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(203,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente interno Santa Ana, Empleados 04,03",4,16,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(471,203,"RETIRO","Retiro Santa Ana Empleados 04,03",1,0,"2120404001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(204,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente externo Santa Ana, Empleados 04,03",4,3,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(472,204,"RETIRO","Retiro Santa Ana corriente externo 04,03",1,0,"2120504002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(205,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Interno Santa Ana, Empleados 04,03",4,4,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(473,205,"RETIRO","Retiro Santa Ana Corto Plazo Interno 04,03",1,0,"2120504003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(206,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Santa Ana, Empleados 04,03",4,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(474,206,"RETIRO","Retiro Santa Ana Corto Plazo Externo 04,03",1,0,"2120504003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(207,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Santa Ana, Empleados 04,03",4,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(475,207,"RETIRO","Retiro Santa Ana Corto Plazo Externo 04,03",1,0,"2120504004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(208,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Interno Santa Ana, Empleados 04,03",4,2,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(476,208,"RETIRO","Retiro Santa Ana Largo Plazo Interno 04,03",1,0,"2120504001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(209,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Externo Santa Ana, Empleados 04,03",4,6,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(477,209,"RETIRO","Retiro Santa Ana Largo Plazo Externo 04,03",1,0,"2120504002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(210,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Interno Santa Ana, Empleados 04,03",4,7,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(478,210,"RETIRO","Retiro Santa Ana Infantil Largo Plazo Externo 04,03",1,0,"2120504005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(211,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Interno Santa Ana, Empleados 04,03",4,8,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(479,211,"RETIRO","Retiro Santa Ana Infantil Largo Plazo Externo 04,03",1,0,"2120504006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(212,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Externo Santa Ana, Empleados 04,03",4,9,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(480,212,"RETIRO","Depósito Santa Ana Infantil Corto Plazo Externo 04,03",1,0,"2120504007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(213,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Externo Santa Ana, Empleados 04,03",4,10,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(481,213,"RETIRO","Retiro Santa Ana Infantil Largo Plazo Externo 04,03",1,0,"2120504008 ");
        
        

/*Depósito Interés*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(214,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente interno Santa Ana, Empleados 04,03",4,16,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(482,214,"DEPOSITO_INTERES","Depósito Interés Santa Ana Empleados 04,03",1,0,"2120404001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(483,214,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(215,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente externo Santa Ana, Empleados 04,03",4,3,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(484,215,"DEPOSITO_INTERES","Depósito Interés Santa Ana corriente externo 04,03",1,0,"2120504002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(485,215,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(216,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Interno Santa Ana, Empleados 04,03",4,4,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(486,216,"DEPOSITO_INTERES","Depósito Interés Santa Ana Corto Plazo Interno 04,03",1,0,"2120504003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(487,216,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(217,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Santa Ana, Empleados 04,03",4,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(488,217,"DEPOSITO_INTERES","Depósito Interés Santa Ana Corto Plazo Externo 04,03",1,0,"2120504003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(489,217,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(218,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Santa Ana, Empleados 04,03",4,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(490,218,"DEPOSITO_INTERES","Depósito Interés Santa Ana Corto Plazo Externo 04,03",1,0,"2120504004");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(491,218,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(219,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Interno Santa Ana, Empleados 04,03",4,2,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(492,219,"DEPOSITO_INTERES","Depósito Interés Santa Ana Largo Plazo Interno 04,03",1,0,"2120504001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(493,219,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(220,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Externo Santa Ana, Empleados 04,03",4,6,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(494,220,"DEPOSITO_INTERES","Depósito Interés Santa Ana Largo Plazo Externo 04,03",1,0,"2120504002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(495,220,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(221,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Interno Santa Ana, Empleados 04,03",4,7,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(496,221,"DEPOSITO_INTERES","Depósito Interés Santa Ana Infantil Largo Plazo Externo 04,03",1,0,"2120504005");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(497,221,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(222,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Interno Santa Ana, Empleados 04,03",4,8,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(498,222,"DEPOSITO_INTERES","Depósito Interés Santa Ana Infantil Largo Plazo Externo 04,03",1,0,"2120504006");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(499,222,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(223,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Externo Santa Ana, Empleados 04,03",4,9,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(500,223,"DEPOSITO_INTERES","Depósito Interés Santa Ana Infantil Corto Plazo Externo 04,03",1,0,"2120504007");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(501,223,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(224,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Externo Santa Ana, Empleados 04,03",4,10,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(502,224,"DEPOSITO_INTERES","Depósito Interés Santa Ana Infantil Largo Plazo Externo 04,03",1,0,"2120504008 ");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(503,224,"GASTO_INTERES","Gasto por Depósito Interés Santa Ana Empleados 04,03",0,1,"4110104001");
        
        

/*Sonsonate-------------------------------------------------------------------------*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(225,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente interno Sonsonate, Empleados 05,04",5,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(504,225,"DEPOSITO","Depósito Sonsonate Empleados 05,04",0,1,"2120405001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(226,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente externo Sonsonate, Empleados 05,04",5,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(505,226,"DEPOSITO","Depósito Sonsonate corriente externo 05,04",0,1,"2120505002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(227,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Interno Sonsonate, Empleados 05,04",5,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(506,227,"DEPOSITO","Depósito Sonsonate Corto Plazo Interno 05,04",0,1,"2120505003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(228,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Sonsonate, Empleados 05,04",5,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(507,228,"DEPOSITO","Depósito Sonsonate Corto Plazo Externo 05,04",0,1,"2120505003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(229,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Sonsonate, Empleados 05,04",5,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(508,229,"DEPOSITO","Depósito Sonsonate Corto Plazo Externo 05,04",0,1,"2120505004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(230,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Interno Sonsonate, Empleados 05,04",5,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(509,230,"DEPOSITO","Depósito Sonsonate Largo Plazo Interno 05,04",0,1,"2120505001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(231,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Externo Sonsonate, Empleados 05,04",5,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(510,231,"DEPOSITO","Depósito Sonsonate Largo Plazo Externo 05,04",0,1,"2120505002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(232,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Interno Sonsonate, Empleados 05,04",5,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(511,232,"DEPOSITO","Depósito Sonsonate Infantil Largo Plazo Externo 05,04",0,1,"2120505005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(233,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Interno Sonsonate, Empleados 05,04",5,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(512,233,"DEPOSITO","Depósito Sonsonate Infantil Largo Plazo Externo 05,04",0,1,"2120505006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(234,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Externo Sonsonate, Empleados 05,04",5,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(513,234,"DEPOSITO","Depósito Sonsonate Infantil Corto Plazo Externo 05,04",0,1,"2120505007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(235,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Externo Sonsonate, Empleados 05,04",5,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(514,235,"DEPOSITO","Depósito Sonsonate Infantil Largo Plazo Externo 05,04",0,1,"2120505008 ");
        


/*Retiros*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(236,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente interno Sonsonate, Empleados 05,04",5,16,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(515,236,"RETIRO","Retiro Sonsonate Empleados 05,04",1,0,"2120405001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(237,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente externo Sonsonate, Empleados 05,04",5,3,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(516,237,"RETIRO","Retiro Sonsonate corriente externo 05,04",1,0,"2120505002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(238,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Interno Sonsonate, Empleados 05,04",5,4,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(517,238,"RETIRO","Retiro Sonsonate Corto Plazo Interno 05,04",1,0,"2120505003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(239,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Sonsonate, Empleados 05,04",5,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(518,239,"RETIRO","Retiro Sonsonate Corto Plazo Externo 05,04",1,0,"2120505003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(240,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Sonsonate, Empleados 05,04",5,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(519,240,"RETIRO","Retiro Sonsonate Corto Plazo Externo 05,04",1,0,"2120505004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(241,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Interno Sonsonate, Empleados 05,04",5,2,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(520,241,"RETIRO","Retiro Sonsonate Largo Plazo Interno 05,04",1,0,"2120505001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(242,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Externo Sonsonate, Empleados 05,04",5,6,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(521,242,"RETIRO","Retiro Sonsonate Largo Plazo Externo 05,04",1,0,"2120505002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(243,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Interno Sonsonate, Empleados 05,04",5,7,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(522,243,"RETIRO","Retiro Sonsonate Infantil Largo Plazo Externo 05,04",1,0,"2120505005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(244,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Interno Sonsonate, Empleados 05,03",5,8,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(523,244,"RETIRO","Retiro Sonsonate Infantil Largo Plazo Externo 05,04",1,0,"2120505006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(245,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Externo Sonsonate, Empleados 05,04",5,9,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(524,245,"RETIRO","Depósito Sonsonate Infantil Corto Plazo Externo 05,04",1,0,"2120505007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(246,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Externo Sonsonate, Empleados 05,04",5,10,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(525,246,"RETIRO","Retiro Sonsonate Infantil Largo Plazo Externo 05,04",1,0,"2120505008 ");
        
        

/*Depósito Interés*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(247,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente interno Sonsonate, Empleados 05,04",5,16,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(526,247,"DEPOSITO_INTERES","Depósito Interés Sonsonate Empleados 05,04",1,0,"2120405001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(527,247,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(248,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente externo Sonsonate, Empleados 05,04",5,3,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(528,248,"DEPOSITO_INTERES","Depósito Interés Sonsonate corriente externo 05,04",1,0,"2120505002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(529,248,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(249,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Interno Sonsonate, Empleados 05,04",5,4,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(530,249,"DEPOSITO_INTERES","Depósito Interés Sonsonate Corto Plazo Interno 05,04",1,0,"2120505003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(531,249,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(250,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Sonsonate, Empleados 05,04",5,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(532,250,"DEPOSITO_INTERES","Depósito Interés Sonsonate Corto Plazo Externo 05,04",1,0,"2120505003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(533,250,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(251,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Sonsonate, Empleados 05,04",5,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(534,251,"DEPOSITO_INTERES","Depósito Interés Sonsonate Corto Plazo Externo 05,04",1,0,"2120505004");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(535,251,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(252,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Interno Sonsonate, Empleados 05,04",5,2,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(536,252,"DEPOSITO_INTERES","Depósito Interés Sonsonate Largo Plazo Interno 05,04",1,0,"2120505001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(537,252,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(253,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Externo Sonsonate, Empleados 05,04",5,6,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(538,253,"DEPOSITO_INTERES","Depósito Interés Sonsonate Largo Plazo Externo 05,04",1,0,"2120505002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(539,253,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(254,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Interno Sonsonate, Empleados 05,04",5,7,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(540,254,"DEPOSITO_INTERES","Depósito Interés Sonsonate Infantil Largo Plazo Externo 05,04",1,0,"2120505005");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(541,254,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(255,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Interno Sonsonate, Empleados 05,04",5,8,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(542,255,"DEPOSITO_INTERES","Depósito Interés Sonsonate Infantil Largo Plazo Externo 05,04",1,0,"2120505006");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(543,255,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(257,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Externo Sonsonate, Empleados 05,04",5,9,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(544,257,"DEPOSITO_INTERES","Depósito Interés Sonsonate Infantil Corto Plazo Externo 05,04",1,0,"2120505007");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(545,257,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(258,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Externo Sonsonate, Empleados 05,04",5,10,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(546,258,"DEPOSITO_INTERES","Depósito Interés Sonsonate Infantil Largo Plazo Externo 05,04",1,0,"2120505008 ");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(547,258,"GASTO_INTERES","Gasto por Depósito Interés Sonsonate Empleados 05,04",0,1,"4110105001");
        
        
        

/*San Juan Opico-------------------------------------------------------------------------*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(259,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente interno San Juan Opico, Empleados 06,05",6,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(548,259,"DEPOSITO","Depósito San Juan Opico Empleados 06,05",0,1,"2120406001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(260,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente externo San Juan Opico, Empleados 06,05",6,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(549,260,"DEPOSITO","Depósito San Juan Opico corriente externo 06,05",0,1,"2120506002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(261,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Interno San Juan Opico, Empleados 06,05",6,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(550,261,"DEPOSITO","Depósito San Juan Opico Corto Plazo Interno 06,05",0,1,"2120506003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(262,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo San Juan Opico, Empleados 06,05",6,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(551,262,"DEPOSITO","Depósito San Juan Opico Corto Plazo Externo 06,05",0,1,"2120506003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(263,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo San Juan Opico, Empleados 06,05",6,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(552,263,"DEPOSITO","Depósito San Juan Opico Corto Plazo Externo 06,05",0,1,"2120506004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(264,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Interno San Juan Opico, Empleados 06,05",6,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(553,264,"DEPOSITO","Depósito San Juan Opico Largo Plazo Interno 06,05",0,1,"2120506001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(265,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Externo San Juan Opico, Empleados 06,05",6,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(554,265,"DEPOSITO","Depósito San Juan Opico Largo Plazo Externo 06,05",0,1,"2120506002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(266,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Interno San Juan Opico, Empleados 06,05",6,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(555,266,"DEPOSITO","Depósito San Juan Opico Infantil Largo Plazo Externo 06,05",0,1,"2120506005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(267,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Interno San Juan Opico, Empleados 06,05",6,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(556,267,"DEPOSITO","Depósito San Juan Opico Infantil Largo Plazo Externo 06,05",0,1,"2120506006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(268,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Externo San Juan Opico, Empleados 06,05",6,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(557,268,"DEPOSITO","Depósito San Juan Opico Infantil Corto Plazo Externo 06,05",0,1,"2120506007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(269,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Externo San Juan Opico, Empleados 06,05",6,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(558,269,"DEPOSITO","Depósito San Juan Opico Infantil Largo Plazo Externo 06,05",0,1,"2120506008 ");
        


/*Retiros*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(270,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente interno San Juan Opico, Empleados 06,05",6,16,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(559,270,"RETIRO","Retiro San Juan Opico Empleados 06,05",1,0,"2120406001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(271,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente externo San Juan Opico, Empleados 06,05",6,3,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(560,271,"RETIRO","Retiro San Juan Opico corriente externo 06,05",1,0,"2120506002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(272,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Interno San Juan Opico, Empleados 06,05",6,4,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(561,272,"RETIRO","Retiro San Juan Opico Corto Plazo Interno 06,05",1,0,"2120506003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(273,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo San Juan Opico, Empleados 06,05",6,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(562,273,"RETIRO","Retiro San Juan Opico Corto Plazo Externo 06,05",1,0,"2120506003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(274,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo San Juan Opico, Empleados 06,05",6,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(563,274,"RETIRO","Retiro San Juan Opico Corto Plazo Externo 06,05",1,0,"2120506004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(275,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Interno San Juan Opico, Empleados 06,05",6,2,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(564,275,"RETIRO","Retiro San Juan Opico Largo Plazo Interno 06,05",1,0,"2120506001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(276,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Externo San Juan Opico, Empleados 06,05",6,6,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(565,276,"RETIRO","Retiro San Juan Opico Largo Plazo Externo 06,05",1,0,"2120506002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(277,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Interno San Juan Opico, Empleados 06,05",6,7,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(566,277,"RETIRO","Retiro San Juan Opico Infantil Largo Plazo Externo 06,05",1,0,"2120506005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(278,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Interno San Juan Opico, Empleados 06,05",6,8,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(567,278,"RETIRO","Retiro San Juan Opico Infantil Largo Plazo Externo 06,05",1,0,"2120506006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(279,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Externo San Juan Opico, Empleados 06,05",6,9,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(568,279,"RETIRO","Depósito San Juan Opico Infantil Corto Plazo Externo 06,05",1,0,"2120506007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(280,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Externo San Juan Opico, Empleados 06,05",6,10,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(569,280,"RETIRO","Retiro San Juan Opico Infantil Largo Plazo Externo 06,05",1,0,"2120506008 ");
        
        

/*Depósito Interés*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(281,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente interno San Juan Opico, Empleados 06,05",6,16,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(570,281,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Empleados 06,05",1,0,"2120406001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(571,281,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(282,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente externo San Juan Opico, Empleados 06,05",6,3,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(572,282,"DEPOSITO_INTERES","Depósito Interés San Juan Opico corriente externo 06,05",1,0,"2120506002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(573,282,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(283,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Interno San Juan Opico, Empleados 06,05",6,4,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(574,283,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Corto Plazo Interno 06,05",1,0,"2120506003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(575,283,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(284,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo San Juan Opico, Empleados 06,05",6,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(576,284,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Corto Plazo Externo 06,05",1,0,"2120506003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(577,284,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(285,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo San Juan Opico, Empleados 06,05",6,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(578,285,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Corto Plazo Externo 06,05",1,0,"2120506004");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(579,285,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(286,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Interno San Juan Opico, Empleados 06,05",6,2,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(580,286,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Largo Plazo Interno 06,05",1,0,"2120506001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(581,286,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(287,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Externo San Juan Opico, Empleados 06,05",6,6,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(582,287,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Largo Plazo Externo 06,05",1,0,"2120506002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(583,287,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(288,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Interno San Juan Opico, Empleados 06,05",6,7,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(584,288,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Infantil Largo Plazo Externo 06,05",1,0,"2120506005");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(585,288,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(289,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Interno San Juan Opico, Empleados 06,05",6,8,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(586,289,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Infantil Largo Plazo Externo 06,05",1,0,"2120506006");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(587,289,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(290,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Externo San Juan Opico, Empleados 06,05",6,9,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(588,290,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Infantil Corto Plazo Externo 06,05",1,0,"2120506007");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(589,290,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(291,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Externo San Juan Opico, Empleados 06,05",6,10,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(590,291,"DEPOSITO_INTERES","Depósito Interés San Juan Opico Infantil Largo Plazo Externo 06,05",1,0,"2120506008 ");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(591,291,"GASTO_INTERES","Gasto por Depósito Interés San Juan Opico Empleados 06,05",0,1,"4110106001");
        
        

/*Santa Tecla-------------------------------------------------------------------------*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(292,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente interno Santa Tecla, Empleados 07,06",7,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(592,292,"DEPOSITO","Depósito Santa Tecla Empleados 07,06",0,1,"2120407001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(293,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro corriente externo Santa Tecla, Empleados 07,06",7,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(593,293,"DEPOSITO","Depósito Santa Tecla corriente externo 07,06",0,1,"2120507002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(294,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Interno Santa Tecla, Empleados 07,06",6,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(594,294,"DEPOSITO","Depósito Santa Tecla Corto Plazo Interno 07,06",0,1,"2120507003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(295,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Santa Tecla, Empleados 07,06",7,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(595,295,"DEPOSITO","Depósito Santa Tecla Corto Plazo Externo 07,06",0,1,"2120507003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(296,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Corto Plazo Externo Santa Tecla, Empleados 07,06",7,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(596,296,"DEPOSITO","Depósito Santa Tecla Corto Plazo Externo 07,06",0,1,"2120507004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(297,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Interno Santa Tecla, Empleados 07,06",7,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(597,297,"DEPOSITO","Depósito Santa Tecla Largo Plazo Interno 07,06",0,1,"2120507001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(298,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Largo Plazo Externo Santa Tecla, Empleados 07,06",7,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(598,298,"DEPOSITO","Depósito Santa Tecla Largo Plazo Externo 07,06",0,1,"2120507002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(299,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Interno Santa Tecla, Empleados 07,06",7,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(599,299,"DEPOSITO","Depósito Santa Tecla Infantil Largo Plazo Externo 07,06",0,1,"2120507005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(300,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Interno Santa Tecla, Empleados 07,06",7,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(600,300,"DEPOSITO","Depósito Santa Tecla Infantil Largo Plazo Externo 07,06",0,1,"2120507006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(301,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Corto Plazo Externo Santa Tecla, Empleados 07,06",7,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(601,301,"DEPOSITO","Depósito Santa Tecla Infantil Corto Plazo Externo 07,06",0,1,"2120507007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(302,"DEPOSITO_AHORRO","Depósito de cuenta de ahorro Infantil Largo Plazo Externo Santa Tecla, Empleados 07,06",7,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(602,302,"DEPOSITO","Depósito Santa Tecla Infantil Largo Plazo Externo 07,06",0,1,"2120507008 ");
        


/*Retiros*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(303,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente interno Santa Tecla, Empleados 07,06",7,16,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(603,303,"RETIRO","Retiro Santa Tecla Empleados 07,06",1,0,"2120407001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(304,"RETIRO_AHORRO","Retiro de cuenta de ahorro corriente externo Santa Tecla, Empleados 07,06",7,3,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(604,304,"RETIRO","Retiro Santa Tecla corriente externo 07,06",1,0,"2120507002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(305,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Interno Santa Tecla, Empleados 07,06",7,4,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(605,305,"RETIRO","Retiro Santa Tecla Corto Plazo Interno 07,06",1,0,"2120507003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(306,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Santa Tecla, Empleados 07,06",7,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(606,306,"RETIRO","Retiro Santa Tecla Corto Plazo Externo 07,06",1,0,"2120507003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(307,"RETIRO_AHORRO","Retiro de cuenta de ahorro Corto Plazo Externo Santa Tecla, Empleados 07,06",7,1,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(607,307,"RETIRO","Retiro Santa Tecla Corto Plazo Externo 07,06",1,0,"2120507004");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(308,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Interno Santa Tecla, Empleados 07,06",7,2,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(608,308,"RETIRO","Retiro Santa Tecla Largo Plazo Interno 07,06",1,0,"2120507001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(309,"RETIRO_AHORRO","Retiro de cuenta de ahorro Largo Plazo Externo Santa Tecla, Empleados 07,06",7,6,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(609,309,"RETIRO","Retiro Santa Tecla Largo Plazo Externo 07,06",1,0,"2120507002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(310,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Interno Santa Tecla, Empleados 07,06",7,7,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(610,310,"RETIRO","Retiro Santa Tecla Infantil Largo Plazo Externo 07,06",1,0,"2120507005");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(311,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Interno Santa Tecla, Empleados 07,06",7,8,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(611,311,"RETIRO","Retiro Santa Tecla Infantil Largo Plazo Externo 07,06",1,0,"2120507006");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(312,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Corto Plazo Externo Santa Tecla, Empleados 07,06",7,9,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(612,312,"RETIRO","Depósito Santa Tecla Infantil Corto Plazo Externo 07,06",1,0,"2120507007");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(313,"RETIRO_AHORRO","Retiro de cuenta de ahorro Infantil Largo Plazo Externo Santa Tecla, Empleados 07,06",7,10,0);

		/*Retiros*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(613,313,"RETIRO","Retiro Santa Tecla Infantil Largo Plazo Externo 07,06",1,0,"2120507008 ");
        
        

/*Depósito Interés*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(314,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente interno Santa Tecla, Empleados 07,06",7,16,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(614,314,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Empleados 07,06",1,0,"2120407001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(615,314,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(315,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro corriente externo Santa Tecla, Empleados 07,06",7,3,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(616,315,"DEPOSITO_INTERES","Depósito Interés Santa Tecla corriente externo 07,06",1,0,"2120507002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(617,315,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(316,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Interno Santa Tecla, Empleados 07,06",7,4,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(618,316,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Corto Plazo Interno 07,06",1,0,"2120507003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(619,316,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(317,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Santa Tecla, Empleados 07,06",7,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(620,317,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Corto Plazo Externo 07,06",1,0,"2120507003");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(621,317,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(318,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Corto Plazo Externo Santa Tecla, Empleados 07,06",7,1,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(622,318,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Corto Plazo Externo 07,06",1,0,"2120507004");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(623,318,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(319,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Interno Santa Tecla, Empleados 07,06",7,2,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(624,319,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Largo Plazo Interno 07,06",1,0,"2120507001");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(625,319,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(320,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Largo Plazo Externo Santa Tecla, Empleados 07,06",7,6,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(626,320,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Largo Plazo Externo 07,06",1,0,"2120507002");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(627,320,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(321,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Interno Santa Tecla, Empleados 07,06",7,7,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(628,321,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Infantil Largo Plazo Externo 07,06",1,0,"2120507005");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(629,321,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(322,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Interno Santa Tecla, Empleados 07,06",7,8,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(630,322,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Infantil Largo Plazo Externo 07,06",1,0,"2120507006");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(631,322,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(323,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Corto Plazo Externo Santa Tecla, Empleados 07,06",7,9,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(632,323,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Infantil Corto Plazo Externo 07,06",1,0,"2120507007");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(633,323,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(324,"DEPOSITO_INTERES_AHORRO","Depósito Interés de cuenta de ahorro Infantil Largo Plazo Externo Santa Tecla, Empleados 07,06",7,10,0);

		/*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(634,324,"DEPOSITO_INTERES","Depósito Interés Santa Tecla Infantil Largo Plazo Externo 07,06",1,0,"2120507008 ");
        
        /*Depósito Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(635,324,"GASTO_INTERES","Gasto por Depósito Interés Santa Tecla Empleados 07,06",0,1,"4110107001");