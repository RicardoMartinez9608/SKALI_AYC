use aycnucleo_alpha;
/*reiniciar las tablas*/
SET FOREIGN_KEY_CHECKS = 0;
truncate table conf_partida_contable;
alter table conf_partida_contable auto_increment=1;

truncate table conf_detalle_partida_contable;
alter table conf_detalle_partida_contable auto_increment=1;
SET FOREIGN_KEY_CHECKS = 1;

/*Pago de créditos*/
/*Administrativa---------------------------------------------------*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(1,"PAGO_CREDITO","Pago de crédito Administrativa, Empleados 05,00",1,0,8);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(1,1,"INTERES","Interés Administrativa Empleados 05,00",0,1,"5110101005");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(2,1,"CAPITAL","Capital Administrativa Empleados 05,00",0,1,"1120101005");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(3,1,"RECARGO","Recargo Administrativa Empleados 05,00",0,1,"5110301005");

/*Carpeta Azul*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(2,"PAGO_CREDITO","Pago de crédito Administrativa, Carpeta Azul 10,00",1,0,9);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(4,2,"INTERES","Interés Administrativa Carpeta Azul 10,00",0,1,"5110101001");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(5,2,"COMISION","Interés Administrativa Carpeta Azul 10,00",0,1,"5110401001");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(6,2,"CAPITAL","Capital Administrativa Carpeta Azul 10,00",0,1,"1120101001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(7,2,"RECARGO","Recargo Administrativa Carpeta Azul 10,00",0,1,"5110301001");

/*Estacionarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(3,"PAGO_CREDITO","Pago de crédito Administrativa, Estacionarios 03,00",1,0,2);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(8,3,"INTERES","Interés Administrativa Estacionarios 03,00",0,1,"5110101003");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(9,3,"COMISION","Interés Administrativa Estacionarios 03,00",0,1,"5110401003");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(10,3,"CAPITAL","Capital Administrativa Estacionarios 03,00",0,1,"1120101003");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(11,3,"RECARGO","Recargo Administrativa Estacionarios 03,00",0,1,"5110301003");

/*Prendarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(4,"PAGO_CREDITO","Pago de crédito Administrativa, Prendarios 04,00",1,0,6);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(12,4,"INTERES","Interés Administrativa Prendarios 04,00",0,1,"5110101004");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(13,4,"COMISION","Interés Administrativa Prendarios 04,00",0,1,"5110401004");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(14,4,"CAPITAL","Capital Administrativa Prendarios 04,00",0,1,"1120101004");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(15,4,"RECARGO","Recargo Administrativa Prendarios 04,00",0,1,"5110301004");

/*Re-estructurado*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(5,"PAGO_CREDITO","Pago de crédito Administrativa, Re-estructurado 06,00",1,0,7);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(16,5,"INTERES","Interés Administrativa Re-estructurado 06,00",0,1,"5110101007");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(17,5,"COMISION","Interés Administrativa Re-estructurado 06,00",0,1,"5110401007");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(18,5,"CAPITAL","Capital Administrativa Re-estructurado 06,00",0,1,"1120101007");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(19,5,"RECARGO","Recargo Administrativa Re-estructurado 06,00",0,1,"5110301007");

/*Arrenda-Financiero*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(6,"PAGO_CREDITO","Pago de crédito Administrativa, Arrenda-Financiero 09,00",1,0,3);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(20,6,"INTERES","Interés Administrativa Arrenda-Financiero 09,00",0,1,"5110101011");
		
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(21,6,"CAPITAL","Capital Administrativa Arrenda-Financiero 09,00",0,1,"1120101010");

/*Micro-crédito*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(7,"PAGO_CREDITO","Pago de crédito Administrativa, Micro-crédito 21,00",1,0,10);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(22,7,"INTERES","Interés Administrativa Micro-crédito 21,00",0,1,"5110101012");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(23,7,"COMISION","Interés Administrativa Micro-crédito 21,00",0,1,"5110401011");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(24,7,"CAPITAL","Capital Administrativa Micro-crédito 21,00",0,1,"1120101011");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(25,7,"RECARGO","Recargo Administrativa Micro-crédito 21,00",0,1,"5110301011");

/*Micro-empresa*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(8,"PAGO_CREDITO","Pago de crédito Administrativa, Micro-empresa 22,00",1,0,11);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(26,8,"INTERES","Interés Administrativa Micro-empresa 22,00",0,1,"5110101013");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(27,8,"COMISION","Interés Administrativa Micro-empresa 22,00",0,1,"5110401012");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(28,8,"CAPITAL","Capital Administrativa Micro-empresa 22,00",0,1,"1120101012");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(29,8,"RECARGO","Recargo Administrativa Micro-empresa 22,00",0,1,"5110301012");

/*Grupo Solidario*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(9,"PAGO_CREDITO","Pago de crédito Administrativa, Grupo Solidario 23,00",1,0,12);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(30,9,"INTERES","Interés Administrativa Grupo Solidario 23,00",0,1,"5110101014");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(31,9,"COMISION","Interés Administrativa Grupo Solidario 23,00",0,1,"5110401013");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(32,9,"CAPITAL","Capital Administrativa Grupo Solidario 23,00",0,1,"1120101013");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(33,9,"RECARGO","Recargo Administrativa Grupo Solidario 23,00",0,1,"5110301013");

/*Crédito Ágil*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(10,"PAGO_CREDITO","Pago de crédito Administrativa, Crédito Ágil 24,00",1,0,14);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(34,10,"INTERES","Interés Administrativa Crédito Ágil 24,00",0,1,"5110101015");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(35,10,"COMISION","Interés Administrativa Crédito Ágil 24,00",0,1,"5110401014");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(36,10,"CAPITAL","Capital Administrativa Crédito Ágil 24,00",0,1,"1120101014");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(37,10,"RECARGO","Recargo Administrativa Crédito Ágil 24,00",0,1,"5110301014");

/*Estacionario GS*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(11,"PAGO_CREDITO","Pago de crédito Administrativa, Estacionario GS 25,00",1,0,13);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(38,11,"INTERES","Interés Administrativa Estacionario GS 25,00",0,1,"5110101016");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(39,11,"COMISION","Interés Administrativa Estacionario GS 25,00",0,1,"5110401015");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(40,11,"CAPITAL","Capital Administrativa Estacionario GS 25,00",0,1,"1120101015");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(41,11,"RECARGO","Recargo Administrativa Estacionario GS 25,00",0,1,"5110301015");

/*Micropersonal*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(12,"PAGO_CREDITO","Pago de crédito Administrativa, Micropersonal 07,00",1,0,1);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(42,12,"INTERES","Interés Administrativa Estacionario GS 07,00",0,1,"5110101010");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(43,12,"COMISION","Interés Administrativa Estacionario GS 07,00",0,1,"5110401010");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(44,12,"CAPITAL","Capital Administrativa Estacionario GS 07,00",0,1,"1120301001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(45,12,"RECARGO","Recargo Administrativa Estacionario GS 07,00",0,1,"5110301010");

/*Jurídico-Vencidos*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(13,"PAGO_CREDITO","Pago de crédito Administrativa, Jurídico-Vencidos 18,00",1,0,16);

		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(48,13,"CAPITAL","Capital Administrativa Jurídico-Vencidos 18,00",0,1,"1129702001");
		

/*Ahuachapán--------------------------------------------------------------------*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(14,"PAGO_CREDITO","Pago de crédito Ahuachapán, Empleados 05,01",2,0,8);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(49,14,"INTERES","Interés Ahuachapán Empleados 05,01",0,1,"5110102005");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(50,14,"CAPITAL","Capital Ahuachapán Empleados 05,01",0,1,"1120102005");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(51,14,"RECARGO","Recargo Ahuachapán Empleados 05,01",0,1,"5110302005");

/*Carpeta Azul*/ 
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(15,"PAGO_CREDITO","Pago de crédito Ahuachapán, Carpeta Azul 10,01",2,0,9);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(52,15,"INTERES","Interés Ahuachapán Carpeta Azul 10,01",0,1,"5110102001");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(53,15,"COMISION","Interés Ahuachapán Carpeta Azul 10,01",0,1,"5110402001");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(54,15,"CAPITAL","Capital Ahuachapán Carpeta Azul 10,01",0,1,"1120102001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(55,15,"RECARGO","Recargo Ahuachapán Carpeta Azul 10,01",0,1,"5110302001");

/*Estacionarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(16,"PAGO_CREDITO","Pago de crédito Ahuachapán, Estacionarios 03,01",2,0,2);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(56,16,"INTERES","Interés Ahuachapán Estacionarios 03,01",0,1,"5110102003");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(57,16,"COMISION","Interés Ahuachapán Estacionarios 03,01",0,1,"5110402003");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(58,16,"CAPITAL","Capital Ahuachapán Estacionarios 03,01",0,1,"1120102003");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(59,16,"RECARGO","Recargo Ahuachapán Estacionarios 03,01",0,1,"5110302003");

/*Prendarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(17,"PAGO_CREDITO","Pago de crédito Ahuachapán, Prendarios 04,01",2,0,6);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(60,17,"INTERES","Interés Ahuachapán Prendarios 04,01",0,1,"5110102004");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(61,17,"COMISION","Interés Ahuachapán Prendarios 04,01",0,1,"5110402004");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(62,17,"CAPITAL","Capital Ahuachapán Prendarios 04,01",0,1,"1120102004");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(63,17,"RECARGO","Recargo Ahuachapán Prendarios 04,01",0,1,"5110302004");

/*Re-estructurado*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(18,"PAGO_CREDITO","Pago de crédito Ahuachapán, Re-estructurado 06,01",2,0,7);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(64,18,"INTERES","Interés Ahuachapán Re-estructurado 06,01",0,1,"5110102007");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(65,18,"COMISION","Interés Ahuachapán Re-estructurado 06,01",0,1,"5110402007");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(66,18,"CAPITAL","Capital Ahuachapán Re-estructurado 06,01",0,1,"1120102007");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(67,18,"RECARGO","Recargo Ahuachapán Re-estructurado 06,01",0,1,"5110302007");

/*Arrenda-Financiero*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(19,"PAGO_CREDITO","Pago de crédito Ahuachapán, Arrenda-Financiero 09,01",1,0,3);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(68,19,"INTERES","Interés Ahuachapán Arrenda-Financiero 09,01",0,1,"5110102011");
		
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(69,19,"CAPITAL","Capital Ahuachapán Arrenda-Financiero 09,01",0,1,"1120102010");

/*Micro-crédito*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(20,"PAGO_CREDITO","Pago de crédito Ahuachapán, Micro-crédito 21,01",2,0,10);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(70,20,"INTERES","Interés Ahuachapán Micro-crédito 21,01",0,1,"5110102012");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(71,20,"COMISION","Interés Ahuachapán Micro-crédito 21,01",0,1,"5110402011");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(72,20,"CAPITAL","Capital Ahuachapán Micro-crédito 21,01",0,1,"1120102011");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(73,20,"RECARGO","Recargo Ahuachapán Micro-crédito 21,01",0,1,"5110302011");

/*Micro-empresa*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(21,"PAGO_CREDITO","Pago de crédito Ahuachapán, Micro-empresa 22,01",2,0,11);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(74,21,"INTERES","Interés Ahuachapán Micro-empresa 22,01",0,1,"5110102013");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(75,21,"COMISION","Interés Ahuachapán Micro-empresa 22,01",0,1,"5110402012");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(76,21,"CAPITAL","Capital Ahuachapán Micro-empresa 22,01",0,1,"1120102012");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(78,21,"RECARGO","Recargo Ahuachapán Micro-empresa 22,01",0,1,"5110302012");

/*Grupo Solidario*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(22,"PAGO_CREDITO","Pago de crédito Ahuachapán, Grupo Solidario 23,01",2,0,12);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(79,22,"INTERES","Interés Ahuachapán Grupo Solidario 23,01",0,1,"5110102014");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(80,22,"COMISION","Interés Ahuachapán Grupo Solidario 23,01",0,1,"5110402013");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(81,22,"CAPITAL","Capital Ahuachapán Grupo Solidario 23,01",0,1,"1120102013");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(82,22,"RECARGO","Recargo Ahuachapán Grupo Solidario 23,01",0,1,"5110302013");

/*Crédito Ágil*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(23,"PAGO_CREDITO","Pago de crédito Ahuachapán, Crédito Ágil 24,01",2,0,14);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(83,23,"INTERES","Interés Ahuachapán Crédito Ágil 24,01",0,1,"5110102015");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(84,23,"COMISION","Interés Ahuachapán Crédito Ágil 24,01",0,1,"5110402014");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(85,23,"CAPITAL","Capital Ahuachapán Crédito Ágil 24,01",0,1,"1120102014");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(86,23,"RECARGO","Recargo Ahuachapán Crédito Ágil 24,01",0,1,"5110302014");

/*Estacionario GS*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(24,"PAGO_CREDITO","Pago de crédito Ahuachapán, Estacionario GS 25,01",2,0,13);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(87,24,"INTERES","Interés Ahuachapán Estacionario GS 25,01",0,1,"5110102016");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(88,24,"COMISION","Interés Ahuachapán Estacionario GS 25,01",0,1,"5110402015");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(89,24,"CAPITAL","Capital Ahuachapán Estacionario GS 25,01",0,1,"1120102015");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(90,24,"RECARGO","Recargo Ahuachapán Estacionario GS 25,01",0,1,"5110302015");

/*Micropersonal*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(25,"PAGO_CREDITO","Pago de crédito Ahuachapán, Micropersonal 07,01",2,0,1);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(91,25,"INTERES","Interés Ahuachapán Estacionario GS 07,01",0,1,"5110102010");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(92,25,"COMISION","Interés Ahuachapán Estacionario GS 07,01",0,1,"5110402010");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(93,25,"CAPITAL","Capital Ahuachapán Estacionario GS 07,01",0,1,"1120302001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(94,25,"RECARGO","Recargo Ahuachapán Estacionario GS 07,01",0,1,"5110302010");

/*Jurídico-Vencidos*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(26,"PAGO_CREDITO","Pago de crédito Ahuachapán, Jurídico-Vencidos 18,01",2,0,16);

		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(95,26,"CAPITAL","Capital Ahuachapán Jurídico-Vencidos 18,01",0,1,"1129702001");


/*Chalchuapa--------------------------------------------------------------------*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(27,"PAGO_CREDITO","Pago de crédito Chalchuapa, Empleados 05,02",3,0,8);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(96,27,"INTERES","Interés Chalchuapa Empleados 05,02",0,1,"5110103005");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(97,27,"CAPITAL","Capital Chalchuapa Empleados 05,02",0,1,"1120103005");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(98,27,"RECARGO","Recargo Chalchuapa Empleados 05,02",0,1,"5110303005");

/*Carpeta Azul*/ 
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(28,"PAGO_CREDITO","Pago de crédito Chalchuapa, Carpeta Azul 10,02",3,0,9);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(99,28,"INTERES","Interés Chalchuapa Carpeta Azul 10,02",0,1,"5110103001");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(100,28,"COMISION","Interés Chalchuapa Carpeta Azul 10,02",0,1,"5110403001");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(101,28,"CAPITAL","Capital Chalchuapa Carpeta Azul 10,02",0,1,"1120103001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(102,28,"RECARGO","Recargo Chalchuapa Carpeta Azul 10,02",0,1,"5110303001");

/*Estacionarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(29,"PAGO_CREDITO","Pago de crédito Chalchuapa, Estacionarios 03,02",3,0,2);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(103,29,"INTERES","Interés Chalchuapa Estacionarios 03,02",0,1,"5110103003");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(104,29,"COMISION","Interés Chalchuapa Estacionarios 03,02",0,1,"5110403003");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(105,29,"CAPITAL","Capital Chalchuapa Estacionarios 03,02",0,1,"1120103003");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(106,29,"RECARGO","Recargo Chalchuapa Estacionarios 03,02",0,1,"5110303003");

/*Prendarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(30,"PAGO_CREDITO","Pago de crédito Chalchuapa, Prendarios 04,02",3,0,6);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(107,30,"INTERES","Interés Chalchuapa Prendarios 04,02",0,1,"5110103004");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(108,30,"COMISION","Interés Chalchuapa Prendarios 04,02",0,1,"5110403004");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(109,30,"CAPITAL","Capital Chalchuapa Prendarios 04,02",0,1,"1120103004");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(110,30,"RECARGO","Recargo Chalchuapa Prendarios 04,02",0,1,"5110303004");

/*Re-estructurado*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(31,"PAGO_CREDITO","Pago de crédito Chalchuapa, Re-estructurado 06,02",3,0,7);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(111,31,"INTERES","Interés Chalchuapa Re-estructurado 06,02",0,1,"5110103007");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(112,31,"COMISION","Interés Chalchuapa Re-estructurado 06,02",0,1,"5110403007");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(113,31,"CAPITAL","Capital Chalchuapa Re-estructurado 06,02",0,1,"1120103007");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(114,31,"RECARGO","Recargo Chalchuapa Re-estructurado 06,02",0,1,"5110303007");

/*Arrenda-Financiero*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(32,"PAGO_CREDITO","Pago de crédito Chalchuapa, Arrenda-Financiero 09,02",3,0,3);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(115,32,"INTERES","Interés Chalchuapa Arrenda-Financiero 09,02",0,1,"5110103011");
		
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(116,32,"CAPITAL","Capital Chalchuapa Arrenda-Financiero 09,02",0,1,"1120103010");

/*Micro-crédito*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(33,"PAGO_CREDITO","Pago de crédito Chalchuapa, Micro-crédito 21,02",3,0,10);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(117,33,"INTERES","Interés Chalchuapa Micro-crédito 21,02",0,1,"5110103012");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(118,33,"COMISION","Interés Chalchuapa Micro-crédito 21,02",0,1,"5110403011");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(119,33,"CAPITAL","Capital Chalchuapa Micro-crédito 21,02",0,1,"1120103011");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(120,33,"RECARGO","Recargo Chalchuapa Micro-crédito 21,02",0,1,"5110303011");

/*Micro-empresa*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(34,"PAGO_CREDITO","Pago de crédito Chalchuapa, Micro-empresa 22,02",3,0,11);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(121,34,"INTERES","Interés Chalchuapa Micro-empresa 22,02",0,1,"5110103013");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(122,34,"COMISION","Interés Chalchuapa Micro-empresa 22,02",0,1,"5110403012");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(123,34,"CAPITAL","Capital Chalchuapa Micro-empresa 22,02",0,1,"1120103012");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(124,34,"RECARGO","Recargo Chalchuapa Micro-empresa 22,02",0,1,"5110303012");

/*Grupo Solidario*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(35,"PAGO_CREDITO","Pago de crédito Chalchuapa, Grupo Solidario 23,02",3,0,12);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(125,35,"INTERES","Interés Chalchuapa Grupo Solidario 23,02",0,1,"5110103014");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(126,35,"COMISION","Interés Chalchuapa Grupo Solidario 23,02",0,1,"5110403013");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(127,35,"CAPITAL","Capital Chalchuapa Grupo Solidario 23,02",0,1,"1120103013");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(128,35,"RECARGO","Recargo Chalchuapa Grupo Solidario 23,02",0,1,"5110303013");

/*Crédito Ágil*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(36,"PAGO_CREDITO","Pago de crédito Chalchuapa, Crédito Ágil 24,02",3,0,14);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(129,36,"INTERES","Interés Chalchuapa Crédito Ágil 24,02",0,1,"5110103015");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(130,36,"COMISION","Interés Chalchuapa Crédito Ágil 24,02",0,1,"5110403014");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(131,36,"CAPITAL","Capital Chalchuapa Crédito Ágil 24,02",0,1,"1120103014");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(132,36,"RECARGO","Recargo Chalchuapa Crédito Ágil 24,02",0,1,"5110303014");

/*Estacionario GS*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(37,"PAGO_CREDITO","Pago de crédito Chalchuapa, Estacionario GS 25,02",3,0,13);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(133,37,"INTERES","Interés Chalchuapa Estacionario GS 25,02",0,1,"5110103016");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(134,37,"COMISION","Interés Chalchuapa Estacionario GS 25,02",0,1,"5110403015");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(135,37,"CAPITAL","Capital Chalchuapa Estacionario GS 25,02",0,1,"1120103015");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(136,37,"RECARGO","Recargo Chalchuapa Estacionario GS 25,03",0,1,"5110303015");

/*Micropersonal*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(38,"PAGO_CREDITO","Pago de crédito Chalchuapa, Micropersonal 07,02",3,0,1);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(137,38,"INTERES","Interés Chalchuapa Estacionario GS 07,02",0,1,"5110103010");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(138,38,"COMISION","Interés Chalchuapa Estacionario GS 07,02",0,1,"5110403010");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(139,38,"CAPITAL","Capital Chalchuapa Estacionario GS 07,02",0,1,"1120303001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(140,38,"RECARGO","Recargo Chalchuapa Estacionario GS 07,02",0,1,"5110303010");

/*Jurídico-Vencidos*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(39,"PAGO_CREDITO","Pago de crédito Chalchuapa, Jurídico-Vencidos 18,02",3,0,16);

		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(141,39,"CAPITAL","Capital Chalchuapa Jurídico-Vencidos 18,02",0,1,"1129703001");


/*Santa Ana--------------------------------------------------------------------*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(40,"PAGO_CREDITO","Pago de crédito Santa Ana, Empleados 05,03",4,0,8);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(142,40,"INTERES","Interés Santa Ana Empleados 05,03",0,1,"5110104005");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(143,40,"CAPITAL","Capital Santa Ana Empleados 05,03",0,1,"1120104005");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(144,40,"RECARGO","Recargo Santa Ana Empleados 05,03",0,1,"5110304005");

/*Carpeta Azul*/ 
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(41,"PAGO_CREDITO","Pago de crédito Santa Ana, Carpeta Azul 10,03",4,0,9);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(145,41,"INTERES","Interés Santa Ana Carpeta Azul 10,03",0,1,"5110104001");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(146,41,"COMISION","Interés Santa Ana Carpeta Azul 10,03",0,1,"5110404001");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(147,41,"CAPITAL","Capital Santa Ana Carpeta Azul 10,03",0,1,"1120104001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(148,41,"RECARGO","Recargo Santa Ana Carpeta Azul 10,03",0,1,"5110304001");

/*Estacionarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(42,"PAGO_CREDITO","Pago de crédito Santa Ana, Estacionarios 03,03",4,0,2);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(149,42,"INTERES","Interés Santa Ana Estacionarios 03,03",0,1,"5110104003");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(150,42,"COMISION","Interés Santa Ana Estacionarios 03,03",0,1,"5110404003");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(151,42,"CAPITAL","Capital Santa Ana Estacionarios 03,03",0,1,"1120104003");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(152,42,"RECARGO","Recargo Santa Ana Estacionarios 03,03",0,1,"5110304003");

/*Prendarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(43,"PAGO_CREDITO","Pago de crédito Santa Ana, Prendarios 04,03",4,0,6);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(153,43,"INTERES","Interés Santa Ana Prendarios 04,03",0,1,"5110104004");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(154,43,"COMISION","Interés Santa Ana Prendarios 04,03",0,1,"5110404004");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(155,43,"CAPITAL","Capital Santa Ana Prendarios 04,03",0,1,"1120104004");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(156,43,"RECARGO","Recargo Santa Ana Prendarios 04,03",0,1,"5110304004");

/*Re-estructurado*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(44,"PAGO_CREDITO","Pago de crédito Santa Ana, Re-estructurado 06,03",4,0,7);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(157,44,"INTERES","Interés Santa Ana Re-estructurado 06,03",0,1,"5110104007");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(158,44,"COMISION","Interés Santa Ana Re-estructurado 06,03",0,1,"5110404007");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(159,44,"CAPITAL","Capital Santa Ana Re-estructurado 06,03",0,1,"1120104007");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(160,44,"RECARGO","Recargo Santa Ana Re-estructurado 06,03",0,1,"5110304007");

/*Arrenda-Financiero*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(45,"PAGO_CREDITO","Pago de crédito Santa Ana, Arrenda-Financiero 09,03",4,0,3);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(161,45,"INTERES","Interés Santa Ana Arrenda-Financiero 09,03",0,1,"5110104011");
		
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(162,45,"CAPITAL","Capital Santa Ana Arrenda-Financiero 09,03",0,1,"1120104010");

/*Micro-crédito*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(46,"PAGO_CREDITO","Pago de crédito Santa Ana, Micro-crédito 21,03",4,0,10);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(163,46,"INTERES","Interés Santa Ana Micro-crédito 21,03",0,1,"5110104012");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(164,46,"COMISION","Interés Santa Ana Micro-crédito 21,03",0,1,"5110404011");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(165,46,"CAPITAL","Capital Santa Ana Micro-crédito 21,03",0,1,"1120104011");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(166,46,"RECARGO","Recargo Santa Ana Micro-crédito 21,03",0,1,"5110304011");

/*Micro-empresa*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(47,"PAGO_CREDITO","Pago de crédito Santa Ana, Micro-empresa 22,03",4,0,11);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(167,47,"INTERES","Interés Santa Ana Micro-empresa 22,03",0,1,"5110104013");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(168,47,"COMISION","Interés Santa Ana Micro-empresa 22,03",0,1,"5110404012");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(169,47,"CAPITAL","Capital Santa Ana Micro-empresa 22,03",0,1,"1120104012");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(170,47,"RECARGO","Recargo Santa Ana Micro-empresa 22,03",0,1,"5110304012");

/*Grupo Solidario*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(48,"PAGO_CREDITO","Pago de crédito Santa Ana, Grupo Solidario 23,03",4,0,12);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(171,48,"INTERES","Interés Santa Ana Grupo Solidario 23,03",0,1,"5110104014");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(172,48,"COMISION","Interés Santa Ana Grupo Solidario 23,03",0,1,"5110404013");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(173,48,"CAPITAL","Capital Santa Ana Grupo Solidario 23,03",0,1,"1120104013");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(174,48,"RECARGO","Recargo Santa Ana Grupo Solidario 23,03",0,1,"5110304013");

/*Crédito Ágil*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(49,"PAGO_CREDITO","Pago de crédito Santa Ana, Crédito Ágil 24,03",4,0,14);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(175,49,"INTERES","Interés Santa Ana Crédito Ágil 24,03",0,1,"5110104015");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(176,49,"COMISION","Interés Santa Ana Crédito Ágil 24,03",0,1,"5110404014");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(178,49,"CAPITAL","Capital Santa Ana Crédito Ágil 24,03",0,1,"1120104014");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(179,49,"RECARGO","Recargo Santa Ana Crédito Ágil 24,03",0,1,"5110304014");

/*Estacionario GS*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(50,"PAGO_CREDITO","Pago de crédito Santa Ana, Estacionario GS 25,03",4,0,13);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(180,50,"INTERES","Interés Santa Ana Estacionario GS 25,03",0,1,"5110104016");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(182,50,"COMISION","Interés Santa Ana Estacionario GS 25,03",0,1,"5110404015");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(183,50,"CAPITAL","Capital Santa Ana Estacionario GS 25,03",0,1,"1120104015");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(184,50,"RECARGO","Recargo Santa Ana Estacionario GS 25,03",0,1,"5110304015");

/*Micropersonal*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(51,"PAGO_CREDITO","Pago de crédito Santa Ana, Micropersonal 07,03",4,0,1);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(185,51,"INTERES","Interés Santa Ana Estacionario GS 07,03",0,1,"5110104010");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(186,51,"COMISION","Interés Santa Ana Estacionario GS 07,03",0,1,"5110404010");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(187,51,"CAPITAL","Capital Santa Ana Estacionario GS 07,03",0,1,"1120304001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(188,51,"RECARGO","Recargo Santa Ana Estacionario GS 07,03",0,1,"5110304010");

/*Jurídico-Vencidos*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(52,"PAGO_CREDITO","Pago de crédito Santa Ana, Jurídico-Vencidos 18,03",4,0,16);

		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(189,52,"CAPITAL","Capital Santa Ana Jurídico-Vencidos 18,03",0,1,"1129704001");


/*Sonsonate--------------------------------------------------------------------*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(53,"PAGO_CREDITO","Pago de crédito Sonsonate, Empleados 05,04",5,0,8);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(190,53,"INTERES","Interés Sonsonate Empleados 05,04",0,1,"5110105005");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(191,53,"CAPITAL","Capital Sonsonate Empleados 05,04",0,1,"1120105005");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(192,53,"RECARGO","Recargo Sonsonate Empleados 05,04",0,1,"5110305005");

/*Carpeta Azul*/ 
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(54,"PAGO_CREDITO","Pago de crédito Sonsonate, Carpeta Azul 10,04",5,0,9);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(193,54,"INTERES","Interés Sonsonate Carpeta Azul 10,04",0,1,"5110105001");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(194,54,"COMISION","Interés Sonsonate Carpeta Azul 10,04",0,1,"5110405001");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(195,54,"CAPITAL","Capital Sonsonate Carpeta Azul 10,04",0,1,"1120105001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(196,54,"RECARGO","Recargo Sonsonate Carpeta Azul 10,04",0,1,"5110305001");

/*Estacionarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(55,"PAGO_CREDITO","Pago de crédito Sonsonate, Estacionarios 03,04",5,0,2);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(197,55,"INTERES","Interés Sonsonate Estacionarios 03,04",0,1,"5110105003");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(198,55,"COMISION","Interés Sonsonate Estacionarios 03,04",0,1,"5110405003");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(199,55,"CAPITAL","Capital Sonsonate Estacionarios 03,04",0,1,"1120105003");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(200,55,"RECARGO","Recargo Sonsonate Estacionarios 03,04",0,1,"5110305003");

/*Prendarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(56,"PAGO_CREDITO","Pago de crédito Sonsonate, Prendarios 04,04",5,0,6);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(201,56,"INTERES","Interés Sonsonate Prendarios 04,04",0,1,"5110105004");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(202,56,"COMISION","Interés Sonsonate Prendarios 04,04",0,1,"5110405004");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(203,56,"CAPITAL","Capital Sonsonate Prendarios 04,04",0,1,"1120105004");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(204,56,"RECARGO","Recargo Sonsonate Prendarios 04,04",0,1,"5110305004");

/*Re-estructurado*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(57,"PAGO_CREDITO","Pago de crédito Sonsonate, Re-estructurado 06,04",5,0,7);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(205,57,"INTERES","Interés Sonsonate Re-estructurado 06,04",0,1,"5110105007");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(206,57,"COMISION","Interés Sonsonate Re-estructurado 06,04",0,1,"5110405007");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(207,57,"CAPITAL","Capital Sonsonate Re-estructurado 06,04",0,1,"1120105007");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(208,57,"RECARGO","Recargo Sonsonate Re-estructurado 06,04",0,1,"5110305007");

/*Arrenda-Financiero*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(58,"PAGO_CREDITO","Pago de crédito Sonsonate, Arrenda-Financiero 09,04",5,0,3);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(209,58,"INTERES","Interés Sonsonate Arrenda-Financiero 09,04",0,1,"5110105011");
		
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(210,58,"CAPITAL","Capital Sonsonate Arrenda-Financiero 09,04",0,1,"1120105010");

/*Micro-crédito*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(59,"PAGO_CREDITO","Pago de crédito Sonsonate, Micro-crédito 21,04",5,0,10);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(211,59,"INTERES","Interés Sonsonate Micro-crédito 21,04",0,1,"5110105012");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(212,59,"COMISION","Interés Sonsonate Micro-crédito 21,04",0,1,"5110405011");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(213,59,"CAPITAL","Capital Sonsonate Micro-crédito 21,04",0,1,"1120105011");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(214,59,"RECARGO","Recargo Sonsonate Micro-crédito 21,04",0,1,"5110305011");

/*Micro-empresa*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(60,"PAGO_CREDITO","Pago de crédito Sonsonate, Micro-empresa 22,04",5,0,11);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(215,60,"INTERES","Interés Sonsonate Micro-empresa 22,04",0,1,"5110105013");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(216,60,"COMISION","Interés Sonsonate Micro-empresa 22,04",0,1,"5110405012");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(217,60,"CAPITAL","Capital Sonsonate Micro-empresa 22,04",0,1,"1120105012");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(218,60,"RECARGO","Recargo Sonsonate Micro-empresa 22,04",0,1,"5110305012");

/*Grupo Solidario*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(61,"PAGO_CREDITO","Pago de crédito Sonsonate, Grupo Solidario 23,04",5,0,12);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(219,61,"INTERES","Interés Sonsonate Grupo Solidario 23,04",0,1,"5110105014");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(220,61,"COMISION","Interés Sonsonate Grupo Solidario 23,04",0,1,"5110405013");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(221,61,"CAPITAL","Capital Sonsonate Grupo Solidario 23,04",0,1,"1120105013");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(222,61,"RECARGO","Recargo Sonsonate Grupo Solidario 23,04",0,1,"5110305013");

/*Crédito Ágil*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(62,"PAGO_CREDITO","Pago de crédito Sonsonate, Crédito Ágil 24,04",5,0,14);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(223,62,"INTERES","Interés Sonsonate Crédito Ágil 24,04",0,1,"5110105015");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(224,62,"COMISION","Interés Sonsonate Crédito Ágil 24,04",0,1,"5110405014");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(225,62,"CAPITAL","Capital Sonsonate Crédito Ágil 24,04",0,1,"1120105014");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(226,62,"RECARGO","Recargo Sonsonate Crédito Ágil 24,04",0,1,"5110305014");

/*Estacionario GS*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(63,"PAGO_CREDITO","Pago de crédito Sonsonate, Estacionario GS 25,04",5,0,13);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(227,63,"INTERES","Interés Sonsonate Estacionario GS 25,04",0,1,"5110105016");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(228,63,"COMISION","Interés Sonsonate Estacionario GS 25,04",0,1,"5110405015");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(229,63,"CAPITAL","Capital Sonsonate Estacionario GS 25,04",0,1,"1120105015");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(230,63,"RECARGO","Recargo Sonsonate Estacionario GS 25,04",0,1,"5110305015");

/*Micropersonal*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(64,"PAGO_CREDITO","Pago de crédito Sonsonate, Micropersonal 07,04",5,0,1);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(231,64,"INTERES","Interés Sonsonate Estacionario GS 07,04",0,1,"5110105010");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(232,64,"COMISION","Interés Sonsonate Estacionario GS 07,04",0,1,"5110405010");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(233,64,"CAPITAL","Capital Sonsonate Estacionario GS 07,04",0,1,"1120305001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(234,64,"RECARGO","Recargo Sonsonate Estacionario GS 07,04",0,1,"5110305010");

/*Jurídico-Vencidos*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(65,"PAGO_CREDITO","Pago de crédito Sonsonate, Jurídico-Vencidos 18,04",5,0,16);

		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(235,65,"CAPITAL","Capital Sonsonate Jurídico-Vencidos 18,04",0,1,"1129705001");


/*San Juan Opico--------------------------------------------------------------------*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(66,"PAGO_CREDITO","Pago de crédito San Juan Opico, Empleados 05,05",6,0,8);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(236,66,"INTERES","Interés San Juan Opico Empleados 05,05",0,1,"5110106005");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(237,66,"CAPITAL","Capital San Juan Opico Empleados 05,05",0,1,"1120106005");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(238,66,"RECARGO","Recargo San Juan Opico Empleados 05,05",0,1,"5110306005");

/*Carpeta Azul*/ 
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(67,"PAGO_CREDITO","Pago de crédito San Juan Opico, Carpeta Azul 10,05",6,0,9);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(239,67,"INTERES","Interés San Juan Opico Carpeta Azul 10,05",0,1,"5110106001");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(240,67,"COMISION","Interés San Juan Opico Carpeta Azul 10,05",0,1,"5110406001");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(241,67,"CAPITAL","Capital San Juan Opico Carpeta Azul 10,05",0,1,"1120106001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(242,67,"RECARGO","Recargo San Juan Opico Carpeta Azul 10,05",0,1,"5110306001");

/*Estacionarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(68,"PAGO_CREDITO","Pago de crédito San Juan Opico, Estacionarios 03,05",6,0,2);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(243,68,"INTERES","Interés San Juan Opico Estacionarios 03,05",0,1,"5110106003");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(244,68,"COMISION","Interés San Juan Opico Estacionarios 03,05",0,1,"5110406003");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(245,68,"CAPITAL","Capital San Juan Opico Estacionarios 03,05",0,1,"1120106003");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(246,68,"RECARGO","Recargo San Juan Opico Estacionarios 03,05",0,1,"5110306003");

/*Prendarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(69,"PAGO_CREDITO","Pago de crédito San Juan Opico, Prendarios 04,05",6,0,6);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(247,69,"INTERES","Interés San Juan Opico Prendarios 04,05",0,1,"5110106004");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(248,69,"COMISION","Interés San Juan Opico Prendarios 04,05",0,1,"5110406004");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(249,69,"CAPITAL","Capital San Juan Opico Prendarios 04,05",0,1,"1120106004");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(250,69,"RECARGO","Recargo San Juan Opico Prendarios 04,05",0,1,"5110306004");

/*Re-estructurado*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(70,"PAGO_CREDITO","Pago de crédito San Juan Opico, Re-estructurado 06,05",6,0,7);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(251,70,"INTERES","Interés San Juan Opico Re-estructurado 06,05",0,1,"5110106007");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(252,70,"COMISION","Interés San Juan Opico Re-estructurado 06,05",0,1,"5110406007");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(253,70,"CAPITAL","Capital San Juan Opico Re-estructurado 06,05",0,1,"1120106007");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(254,70,"RECARGO","Recargo San Juan Opico Re-estructurado 06,05",0,1,"5110306007");

/*Arrenda-Financiero*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(71,"PAGO_CREDITO","Pago de crédito San Juan Opico, Arrenda-Financiero 09,05",6,0,3);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(255,71,"INTERES","Interés San Juan Opico Arrenda-Financiero 09,05",0,1,"5110106011");
		
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(256,71,"CAPITAL","Capital San Juan Opico Arrenda-Financiero 09,05",0,1,"1120106010");

/*Micro-crédito*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(72,"PAGO_CREDITO","Pago de crédito San Juan Opico, Micro-crédito 21,05",6,0,10);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(257,72,"INTERES","Interés San Juan Opico Micro-crédito 21,05",0,1,"5110106012");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(258,72,"COMISION","Interés San Juan Opico Micro-crédito 21,05",0,1,"5110406011");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(259,72,"CAPITAL","Capital San Juan Opico Micro-crédito 21,05",0,1,"1120106011");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(260,72,"RECARGO","Recargo San Juan Opico Micro-crédito 21,05",0,1,"5110306011");

/*Micro-empresa*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(73,"PAGO_CREDITO","Pago de crédito San Juan Opico, Micro-empresa 22,05",6,0,11);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(261,73,"INTERES","Interés San Juan Opico Micro-empresa 22,05",0,1,"5110106013");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(262,73,"COMISION","Interés San Juan Opico Micro-empresa 22,05",0,1,"5110406012");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(263,73,"CAPITAL","Capital San Juan Opico Micro-empresa 22,05",0,1,"1120106012");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(264,73,"RECARGO","Recargo San Juan Opico Micro-empresa 22,05",0,1,"5110306012");

/*Grupo Solidario*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(74,"PAGO_CREDITO","Pago de crédito San Juan Opico, Grupo Solidario 23,05",6,0,12);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(265,74,"INTERES","Interés San Juan Opico Grupo Solidario 23,05",0,1,"5110106014");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(266,74,"COMISION","Interés San Juan Opico Grupo Solidario 23,05",0,1,"5110406013");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(267,74,"CAPITAL","Capital San Juan Opico Grupo Solidario 23,05",0,1,"1120106013");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(268,74,"RECARGO","Recargo San Juan Opico Grupo Solidario 23,05",0,1,"5110306013");

/*Crédito Ágil*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(75,"PAGO_CREDITO","Pago de crédito San Juan Opico, Crédito Ágil 24,05",6,0,14);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(269,75,"INTERES","Interés San Juan Opico Crédito Ágil 24,05",0,1,"5110106015");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(270,75,"COMISION","Interés San Juan Opico Crédito Ágil 24,05",0,1,"5110406014");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(271,75,"CAPITAL","Capital San Juan Opico Crédito Ágil 24,05",0,1,"1120106014");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(272,75,"RECARGO","Recargo San Juan Opico Crédito Ágil 24,05",0,1,"5110306014");

/*Estacionario GS*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(76,"PAGO_CREDITO","Pago de crédito San Juan Opico, Estacionario GS 25,05",6,0,13);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(273,76,"INTERES","Interés San Juan Opico Estacionario GS 25,05",0,1,"5110106016");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(274,76,"COMISION","Interés San Juan Opico Estacionario GS 25,05",0,1,"5110406015");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(275,76,"CAPITAL","Capital San Juan Opico Estacionario GS 25,05",0,1,"1120106015");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(276,76,"RECARGO","Recargo San Juan Opico Estacionario GS 25,05",0,1,"5110306015");

/*Micropersonal*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(77,"PAGO_CREDITO","Pago de crédito San Juan Opico, Micropersonal 07,05",6,0,1);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(277,77,"INTERES","Interés San Juan Opico Estacionario GS 07,05",0,1,"5110106010");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(278,77,"COMISION","Interés San Juan Opico Estacionario GS 07,05",0,1,"5110406010");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(279,77,"CAPITAL","Capital San Juan Opico Estacionario GS 07,05",0,1,"1120306001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(280,77,"RECARGO","Recargo San Juan Opico Estacionario GS 07,05",0,1,"5110306010");

/*Jurídico-Vencidos*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(78,"PAGO_CREDITO","Pago de crédito San Juan Opico, Jurídico-Vencidos 18,05",6,0,16);

		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(281,78,"CAPITAL","Capital San Juan Opico Jurídico-Vencidos 18,05",0,1,"1129706001");

/*Santa Tecla--------------------------------------------------------------------*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(79,"PAGO_CREDITO","Pago de crédito Santa Tecla, Empleados 05,06",7,0,8);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(282,79,"INTERES","Interés Santa Tecla Empleados 05,06",0,1,"5110107005");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(283,79,"CAPITAL","Capital Santa Tecla Empleados 05,06",0,1,"1120107005");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(284,79,"RECARGO","Recargo Santa Tecla Empleados 05,06",0,1,"5110307005");

/*Carpeta Azul*/ 
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(80,"PAGO_CREDITO","Pago de crédito Santa Tecla, Carpeta Azul 10,06",7,0,9);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(285,80,"INTERES","Interés Santa Tecla Carpeta Azul 10,06",0,1,"5110107001");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(286,80,"COMISION","Interés Santa Tecla Carpeta Azul 10,06",0,1,"5110407001");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(287,80,"CAPITAL","Capital Santa Tecla Carpeta Azul 10,06",0,1,"1120107001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(288,80,"RECARGO","Recargo Santa Tecla Carpeta Azul 10,06",0,1,"5110307001");

/*Estacionarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(81,"PAGO_CREDITO","Pago de crédito Santa Tecla, Estacionarios 03,06",7,0,2);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(289,81,"INTERES","Interés Santa Tecla Estacionarios 03,06",0,1,"5110107003");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(290,81,"COMISION","Interés Santa Tecla Estacionarios 03,06",0,1,"5110407003");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(291,81,"CAPITAL","Capital Santa Tecla Estacionarios 03,06",0,1,"1120107003");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(292,81,"RECARGO","Recargo Santa Tecla Estacionarios 03,06",0,1,"5110307003");

/*Prendarios*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(82,"PAGO_CREDITO","Pago de crédito Santa Tecla, Prendarios 04,06",7,0,6);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(293,82,"INTERES","Interés Santa Tecla Prendarios 04,06",0,1,"5110107004");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(294,82,"COMISION","Interés Santa Tecla Prendarios 04,06",0,1,"5110407004");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(295,82,"CAPITAL","Capital Santa Tecla Prendarios 04,06",0,1,"1120107004");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(296,82,"RECARGO","Recargo Santa Tecla Prendarios 04,06",0,1,"5110307004");

/*Re-estructurado*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(83,"PAGO_CREDITO","Pago de crédito Santa Tecla, Re-estructurado 06,06",7,0,7);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(297,83,"INTERES","Interés Santa Tecla Re-estructurado 06,06",0,1,"5110107007");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(298,83,"COMISION","Interés Santa Tecla Re-estructurado 06,06",0,1,"5110407007");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(299,83,"CAPITAL","Capital Santa Tecla Re-estructurado 06,06",0,1,"11201067007");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(300,83,"RECARGO","Recargo Santa Tecla Re-estructurado 06,06",0,1,"5110307007");

/*Arrenda-Financiero*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(84,"PAGO_CREDITO","Pago de crédito Santa Tecla, Arrenda-Financiero 09,06",7,0,3);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(301,84,"INTERES","Interés Santa Tecla Arrenda-Financiero 09,06",0,1,"5110107011");
		
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(302,84,"CAPITAL","Capital Santa Tecla Arrenda-Financiero 09,06",0,1,"1120107010");

/*Micro-crédito*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(85,"PAGO_CREDITO","Pago de crédito Santa Tecla, Micro-crédito 21,06",7,0,10);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(303,85,"INTERES","Interés Santa Tecla Micro-crédito 21,06",0,1,"5110107012");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(304,85,"COMISION","Interés Santa Tecla Micro-crédito 21,06",0,1,"5110407011");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(305,85,"CAPITAL","Capital Santa Tecla Micro-crédito 21,06",0,1,"1120107011");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(306,85,"RECARGO","Recargo Santa Tecla Micro-crédito 21,06",0,1,"5110307011");

/*Micro-empresa*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(86,"PAGO_CREDITO","Pago de crédito Santa Tecla, Micro-empresa 22,06",7,0,11);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(307,86,"INTERES","Interés Santa Tecla Micro-empresa 22,06",0,1,"5110107013");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(308,86,"COMISION","Interés Santa Tecla Micro-empresa 22,06",0,1,"5110407012");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(309,86,"CAPITAL","Capital Santa Tecla Micro-empresa 22,06",0,1,"1120107012");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(310,86,"RECARGO","Recargo Santa Tecla Micro-empresa 22,06",0,1,"5110307012");

/*Grupo Solidario*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(87,"PAGO_CREDITO","Pago de crédito Santa Tecla, Grupo Solidario 23,06",7,0,12);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(311,87,"INTERES","Interés Santa Tecla Grupo Solidario 23,06",0,1,"5110107014");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(312,87,"COMISION","Interés Santa Tecla Grupo Solidario 23,06",0,1,"5110407013");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(313,87,"CAPITAL","Capital Santa Tecla Grupo Solidario 23,06",0,1,"1120107013");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(314,87,"RECARGO","Recargo Santa Tecla Grupo Solidario 23,06",0,1,"5110307013");

/*Crédito Ágil*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(88,"PAGO_CREDITO","Pago de crédito Santa Tecla, Crédito Ágil 24,06",7,0,14);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(315,88,"INTERES","Interés Santa Tecla Crédito Ágil 24,06",0,1,"5110107015");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(316,88,"COMISION","Interés Santa Tecla Crédito Ágil 24,06",0,1,"5110407014");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(317,88,"CAPITAL","Capital Santa Tecla Crédito Ágil 24,06",0,1,"1120107014");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(318,88,"RECARGO","Recargo Santa Tecla Crédito Ágil 24,06",0,1,"5110307014");

/*Estacionario GS*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(89,"PAGO_CREDITO","Pago de crédito Santa Tecla, Estacionario GS 25,06",7,0,13);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(319,89,"INTERES","Interés Santa Tecla Estacionario GS 25,06",0,1,"5110107016");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(320,89,"COMISION","Interés Santa Tecla Estacionario GS 25,06",0,1,"5110407015");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(321,89,"CAPITAL","Capital Santa Tecla Estacionario GS 25,06",0,1,"1120107015");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(322,89,"RECARGO","Recargo Santa Tecla Estacionario GS 25,06",0,1,"5110307015");

/*Micropersonal*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(90,"PAGO_CREDITO","Pago de crédito Santa Tecla, Micropersonal 07,06",7,0,1);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(323,90,"INTERES","Interés Santa Tecla Estacionario GS 07,06",0,1,"5110107010");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(324,90,"COMISION","Interés Santa Tecla Estacionario GS 07,06",0,1,"5110407010");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(325,90,"CAPITAL","Capital Santa Tecla Estacionario GS 07,06",0,1,"1120307001");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(326,90,"RECARGO","Recargo Santa Tecla Estacionario GS 07,06",0,1,"5110307010");

/*Jurídico-Vencidos*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(91,"PAGO_CREDITO","Pago de crédito Santa Tecla, Jurídico-Vencidos 18,06",7,0,16);

		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(327,91,"CAPITAL","Capital Santa Tecla Jurídico-Vencidos 18,06",0,1,"1129707001");


/*Detalle de los créditos*/

/*Faltaba pronto pago*/

/*Administrativa*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(325,"PAGO_CREDITO","Pago de crédito Adminsitrativa, ProntoPago 17,00",1,0,5);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(636,325,"INTERES","Interés Adminsitrativa ProntoPago 17,00",0,1,"5110101002");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(637,325,"COMISION","Comisión Adminsitrativa ProntoPago 17,00",0,1,"5110401002");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(638,325,"CAPITAL","Capital Adminsitrativa ProntoPago 17,00",0,1,"1120101002");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(639,325,"RECARGO","Recargo Adminsitrativa ProntoPago 17,00",0,1,"5110301002");

/*Ahuachapán*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(326,"PAGO_CREDITO","Pago de crédito Ahuachapán, ProntoPago 17,01",2,0,5);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(640,326,"INTERES","Interés Ahuachapán ProntoPago 17,01",0,1,"5110102002");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(641,326,"COMISION","Comisión Ahuachapán ProntoPago 17,01",0,1,"5110402002");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(642,326,"CAPITAL","Capital Ahuachapán ProntoPago 17,01",0,1,"1120102002");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(643,326,"RECARGO","Recargo Ahuachapán ProntoPago 17,01",0,1,"5110302002");
        
/*Chalchuapa*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(327,"PAGO_CREDITO","Pago de crédito Chalchuapa, ProntoPago 17,02",3,0,5);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(644,327,"INTERES","Interés Chalchuapa ProntoPago 17,02",0,1,"5110103002");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(645,327,"COMISION","Comisión Chalchuapa ProntoPago 17,02",0,1,"5110403002");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(646,327,"CAPITAL","Capital Chalchuapa ProntoPago 17,02",0,1,"1120103002");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(647,327,"RECARGO","Recargo Chalchuapa ProntoPago 17,02",0,1,"5110303002");
        
/*Santa Ana*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(328,"PAGO_CREDITO","Pago de crédito Santa Ana, ProntoPago 17,03",4,0,5);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(648,328,"INTERES","Interés Santa Ana ProntoPago 17,03",0,1,"5110104002");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(649,328,"COMISION","Comisión Santa Ana ProntoPago 17,03",0,1,"5110404002");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(650,328,"CAPITAL","Capital Santa Ana ProntoPago 17,03",0,1,"1120104002");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(651,328,"RECARGO","Recargo Santa Ana ProntoPago 17,03",0,1,"5110304002");
        
/*Sonsonate*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(329,"PAGO_CREDITO","Pago de crédito Sonsonate, ProntoPago 17,04",5,0,5);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(652,329,"INTERES","Interés Sonsonate ProntoPago 17,04",0,1,"5110105002");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(653,329,"COMISION","Comisión Sonsonate ProntoPago 17,04",0,1,"5110405002");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(654,329,"CAPITAL","Capital Sonsonate ProntoPago 17,04",0,1,"1120105002");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(655,329,"RECARGO","Recargo Sonsonate ProntoPago 17,04",0,1,"5110305002");
        
/*San Juan Opico*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(330,"PAGO_CREDITO","Pago de crédito San Juan Opico, ProntoPago 17,05",6,0,5);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(656,330,"INTERES","Interés San Juan Opico ProntoPago 17,05",0,1,"5110106002");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(657,330,"COMISION","Comisión San Juan Opico ProntoPago 17,05",0,1,"5110406002");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(658,330,"CAPITAL","Capital San Juan Opico ProntoPago 17,05",0,1,"1120106002");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(659,330,"RECARGO","Recargo San Juan Opico ProntoPago 17,05",0,1,"5110306002");
        
/*Santa Tecla*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(331,"PAGO_CREDITO","Pago de crédito Santa Tecla, ProntoPago 17,06",7,0,5);

		/*Interés*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(660,331,"INTERES","Interés Santa Tecla ProntoPago 17,06",0,1,"5110107002");
		/*Comisión*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(661,331,"COMISION","Comisión Santa Tecla ProntoPago 17,06",0,1,"5110407002");
		/*Capital*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(662,331,"CAPITAL","Capital Santa Tecla ProntoPago 17,06",0,1,"1120107002");
		/*Recargo*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(663,331,"RECARGO","Recargo Santa Tecla ProntoPago 17,06",0,1,"5110307002");