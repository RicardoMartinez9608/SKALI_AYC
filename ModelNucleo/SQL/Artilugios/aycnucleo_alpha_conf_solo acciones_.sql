use aycnucleo_alpha;

/*Administrativa*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(378,"PAGO_ACCION","PAGO POR ACCION",1,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(705,378,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(379,"PAGO_ACCION","PAGO POR ACCION",1,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(706,379,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(380,"PAGO_ACCION","PAGO POR ACCION",1,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(707,380,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(381,"PAGO_ACCION","PAGO POR ACCION",1,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(708,381,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(382,"PAGO_ACCION","PAGO POR ACCION",1,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(709,382,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(383,"PAGO_ACCION","PAGO POR ACCION",1,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(710,383,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(384,"PAGO_ACCION","PAGO POR ACCION",1,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(711,384,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(385,"PAGO_ACCION","PAGO POR ACCION",1,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(712,385,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(386,"PAGO_ACCION","PAGO POR ACCION",1,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(713,386,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(387,"PAGO_ACCION","PAGO POR ACCION",1,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(714,387,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(388,"PAGO_ACCION","PAGO POR ACCION",1,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(715,388,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110201003 ");
        

/*AHUCHAPÁN*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(389,"PAGO_ACCION","PAGO POR ACCION",2,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(716,389,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(390,"PAGO_ACCION","PAGO POR ACCION",2,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(717,390,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(391,"PAGO_ACCION","PAGO POR ACCION",2,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(718,391,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(392,"PAGO_ACCION","PAGO POR ACCION",2,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(719,392,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(393,"PAGO_ACCION","PAGO POR ACCION",2,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(720,393,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(394,"PAGO_ACCION","PAGO POR ACCION",2,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(721,394,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(395,"PAGO_ACCION","PAGO POR ACCION",2,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(722,395,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(396,"PAGO_ACCION","PAGO POR ACCION",2,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(723,396,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(397,"PAGO_ACCION","PAGO POR ACCION",2,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(724,397,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(398,"PAGO_ACCION","PAGO POR ACCION",2,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(725,398,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(399,"PAGO_ACCION","PAGO POR ACCION",2,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(726,399,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110202003 ");
        
        
/*CHALCHUAPA*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(400,"PAGO_ACCION","PAGO POR ACCION",3,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(727,400,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(401,"PAGO_ACCION","PAGO POR ACCION",3,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(728,401,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(402,"PAGO_ACCION","PAGO POR ACCION",3,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(729,402,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(403,"PAGO_ACCION","PAGO POR ACCION",3,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(730,403,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(404,"PAGO_ACCION","PAGO POR ACCION",3,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(731,404,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(405,"PAGO_ACCION","PAGO POR ACCION",3,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(732,405,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(406,"PAGO_ACCION","PAGO POR ACCION",3,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(733,406,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(407,"PAGO_ACCION","PAGO POR ACCION",3,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(734,407,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(408,"PAGO_ACCION","PAGO POR ACCION",3,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(735,408,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(409,"PAGO_ACCION","PAGO POR ACCION",3,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(736,409,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(410,"PAGO_ACCION","PAGO POR ACCION",3,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(737,410,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110203003 ");



/*SANTA ANA*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(411,"PAGO_ACCION","PAGO POR ACCION",4,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(738,411,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(412,"PAGO_ACCION","PAGO POR ACCION",4,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(739,412,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(413,"PAGO_ACCION","PAGO POR ACCION",4,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(740,413,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(414,"PAGO_ACCION","PAGO POR ACCION",4,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(741,414,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(415,"PAGO_ACCION","PAGO POR ACCION",4,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(742,415,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(416,"PAGO_ACCION","PAGO POR ACCION",4,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(743,416,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(417,"PAGO_ACCION","PAGO POR ACCION",4,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(744,417,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(418,"PAGO_ACCION","PAGO POR ACCION",4,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(745,418,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(420,"PAGO_ACCION","PAGO POR ACCION",4,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(746,420,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(421,"PAGO_ACCION","PAGO POR ACCION",4,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(747,421,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(422,"PAGO_ACCION","PAGO POR ACCION",4,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(748,422,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110204003 ");
        
        
/*SONSONATE*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(423,"PAGO_ACCION","PAGO POR ACCION",5,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(749,423,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(424,"PAGO_ACCION","PAGO POR ACCION",5,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(750,424,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(425,"PAGO_ACCION","PAGO POR ACCION",5,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(751,425,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(426,"PAGO_ACCION","PAGO POR ACCION",5,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(752,426,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(427,"PAGO_ACCION","PAGO POR ACCION",5,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(753,427,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(428,"PAGO_ACCION","PAGO POR ACCION",5,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(754,428,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(429,"PAGO_ACCION","PAGO POR ACCION",5,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(755,429,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(430,"PAGO_ACCION","PAGO POR ACCION",5,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(756,430,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(431,"PAGO_ACCION","PAGO POR ACCION",5,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(757,431,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(432,"PAGO_ACCION","PAGO POR ACCION",5,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(758,432,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(433,"PAGO_ACCION","PAGO POR ACCION",5,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(759,433,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110205003 ");
        
        
/*SAN JUAN OPICO*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(434,"PAGO_ACCION","PAGO POR ACCION",6,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(760,434,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(435,"PAGO_ACCION","PAGO POR ACCION",6,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(761,435,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(436,"PAGO_ACCION","PAGO POR ACCION",6,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(762,436,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(437,"PAGO_ACCION","PAGO POR ACCION",6,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(763,437,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(438,"PAGO_ACCION","PAGO POR ACCION",6,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(764,438,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(439,"PAGO_ACCION","PAGO POR ACCION",6,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(765,439,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(440,"PAGO_ACCION","PAGO POR ACCION",6,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(766,440,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(441,"PAGO_ACCION","PAGO POR ACCION",6,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(767,441,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(442,"PAGO_ACCION","PAGO POR ACCION",6,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(768,442,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(443,"PAGO_ACCION","PAGO POR ACCION",6,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(769,443,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(444,"PAGO_ACCION","PAGO POR ACCION",6,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(770,444,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110206003 ");
        
        
        
/*SANTA TECLA*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(445,"PAGO_ACCION","PAGO POR ACCION",7,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(771,445,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(446,"PAGO_ACCION","PAGO POR ACCION",7,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(772,446,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(447,"PAGO_ACCION","PAGO POR ACCION",7,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(773,447,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(448,"PAGO_ACCION","PAGO POR ACCION",7,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(774,448,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(449,"PAGO_ACCION","PAGO POR ACCION",7,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(775,449,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(450,"PAGO_ACCION","PAGO POR ACCION",7,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(778,450,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(451,"PAGO_ACCION","PAGO POR ACCION",7,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(779,451,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(452,"PAGO_ACCION","PAGO POR ACCION",7,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(780,452,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(453,"PAGO_ACCION","PAGO POR ACCION",7,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(781,453,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(454,"PAGO_ACCION","PAGO POR ACCION",7,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(782,454,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(455,"PAGO_ACCION","PAGO POR ACCION",7,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(783,455,"PAGO_ACCION","PAGO POR ACCION",0,1,"3110207003 ");