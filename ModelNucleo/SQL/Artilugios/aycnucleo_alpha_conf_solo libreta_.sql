use aycnucleo_alpha;

/*Administrativa*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(456,"PAGO_LIBRETA","PAGO POR LIBRETA",1,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(784,456,"PAGO_LIBRETA","PAGO LIBRETA",0,1,"5210201002");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(457,"PAGO_LIBRETA","PAGO POR LIBRETA",1,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(785,457,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(458,"PAGO_LIBRETA","PAGO POR LIBRETA",1,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(786,458,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(459,"PAGO_LIBRETA","PAGO POR LIBRETA",1,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(787,459,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(460,"PAGO_LIBRETA","PAGO POR LIBRETA",1,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(788,460,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(461,"PAGO_LIBRETA","PAGO POR LIBRETA",1,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(789,461,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(462,"PAGO_LIBRETA","PAGO POR LIBRETA",1,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(790,462,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(463,"PAGO_LIBRETA","PAGO POR LIBRETA",1,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(791,463,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(464,"PAGO_LIBRETA","PAGO POR LIBRETA",1,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(792,464,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(465,"PAGO_LIBRETA","PAGO POR LIBRETA",1,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(793,465,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(466,"PAGO_LIBRETA","PAGO POR LIBRETA",1,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(794,466,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210201002");
        

/*AHUCHAPÁN*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(467,"PAGO_LIBRETA","PAGO POR LIBRETA",2,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(795,467,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(468,"PAGO_LIBRETA","PAGO POR LIBRETA",2,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(796,468,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(469,"PAGO_LIBRETA","PAGO POR LIBRETA",2,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(797,469,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(470,"PAGO_LIBRETA","PAGO POR LIBRETA",2,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(798,470,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(471,"PAGO_LIBRETA","PAGO POR LIBRETA",2,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(799,471,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(472,"PAGO_LIBRETA","PAGO POR LIBRETA",2,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(800,472,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(473,"PAGO_LIBRETA","PAGO POR LIBRETA",2,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(801,473,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(474,"PAGO_LIBRETA","PAGO POR LIBRETA",2,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(802,474,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(475,"PAGO_LIBRETA","PAGO POR LIBRETA",2,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(803,475,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(476,"PAGO_LIBRETA","PAGO POR LIBRETA",2,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(804,476,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(477,"PAGO_LIBRETA","PAGO POR LIBRETA",2,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(805,477,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210202002");
        
        
/*CHALCHUAPA*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(478,"PAGO_LIBRETA","PAGO POR LIBRETA",3,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(806,478,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(479,"PAGO_LIBRETA","PAGO POR LIBRETA",3,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(807,479,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(480,"PAGO_LIBRETA","PAGO POR LIBRETA",3,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(808,480,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(481,"PAGO_LIBRETA","PAGO POR LIBRETA",3,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(809,481,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(482,"PAGO_LIBRETA","PAGO POR LIBRETA",3,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(810,482,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(483,"PAGO_LIBRETA","PAGO POR LIBRETA",3,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(811,483,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(484,"PAGO_LIBRETA","PAGO POR LIBRETA",3,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(812,484,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(485,"PAGO_LIBRETA","PAGO POR LIBRETA",3,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(813,485,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(486,"PAGO_LIBRETA","PAGO POR LIBRETA",3,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(814,486,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(487,"PAGO_LIBRETA","PAGO POR LIBRETA",3,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(815,487,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(488,"PAGO_LIBRETA","PAGO POR LIBRETA",3,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(816,488,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210203002");



/*SANTA ANA*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(489,"PAGO_LIBRETA","PAGO POR LIBRETA",4,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(817,489,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(490,"PAGO_LIBRETA","PAGO POR LIBRETA",4,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(818,490,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(491,"PAGO_LIBRETA","PAGO POR LIBRETA",4,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(819,491,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(492,"PAGO_LIBRETA","PAGO POR LIBRETA",4,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(820,492,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(493,"PAGO_LIBRETA","PAGO POR LIBRETA",4,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(821,493,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(494,"PAGO_LIBRETA","PAGO POR LIBRETA",4,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(822,494,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(495,"PAGO_LIBRETA","PAGO POR LIBRETA",4,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(823,495,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(496,"PAGO_LIBRETA","PAGO POR LIBRETA",4,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(824,496,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(497,"PAGO_LIBRETA","PAGO POR LIBRETA",4,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(825,497,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(498,"PAGO_LIBRETA","PAGO POR LIBRETA",4,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(826,498,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(499,"PAGO_LIBRETA","PAGO POR LIBRETA",4,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(827,499,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210204002");
        
        
/*SONSONATE*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(500,"PAGO_LIBRETA","PAGO POR LIBRETA",5,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(828,500,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(501,"PAGO_LIBRETA","PAGO POR LIBRETA",5,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(829,501,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(502,"PAGO_LIBRETA","PAGO POR LIBRETA",5,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(830,502,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(503,"PAGO_LIBRETA","PAGO POR LIBRETA",5,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(831,503,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(504,"PAGO_LIBRETA","PAGO POR LIBRETA",5,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(832,504,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(505,"PAGO_LIBRETA","PAGO POR LIBRETA",5,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(833,505,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(506,"PAGO_LIBRETA","PAGO POR LIBRETA",5,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(834,506,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(507,"PAGO_LIBRETA","PAGO POR LIBRETA",5,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(835,507,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(508,"PAGO_LIBRETA","PAGO POR LIBRETA",5,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(836,508,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(509,"PAGO_LIBRETA","PAGO POR LIBRETA",5,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(837,509,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(510,"PAGO_LIBRETA","PAGO POR LIBRETA",5,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(838,510,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210205002");
        
        
/*SAN JUAN OPICO*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(511,"PAGO_LIBRETA","PAGO POR LIBRETA",6,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(839,511,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(512,"PAGO_LIBRETA","PAGO POR LIBRETA",6,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(840,512,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(513,"PAGO_LIBRETA","PAGO POR LIBRETA",6,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(841,513,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(514,"PAGO_LIBRETA","PAGO POR LIBRETA",6,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(842,514,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(515,"PAGO_LIBRETA","PAGO POR LIBRETA",6,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(843,515,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(516,"PAGO_LIBRETA","PAGO POR LIBRETA",6,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(844,516,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(517,"PAGO_LIBRETA","PAGO POR LIBRETA",6,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(845,517,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(518,"PAGO_LIBRETA","PAGO POR LIBRETA",6,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(846,518,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(519,"PAGO_LIBRETA","PAGO POR LIBRETA",6,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(847,519,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(520,"PAGO_LIBRETA","PAGO POR LIBRETA",6,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(848,520,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(521,"PAGO_LIBRETA","PAGO POR LIBRETA",6,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(849,521,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210206002");
        
        
        
/*SANTA TECLA*/
/*Depósitos*/
/*Empleados*/
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(522,"PAGO_LIBRETA","PAGO POR LIBRETA",7,16,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(850,522,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");

/*Corriente Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(523,"PAGO_LIBRETA","PAGO POR LIBRETA",7,3,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(851,523,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");
        
/*Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(524,"PAGO_LIBRETA","PAGO POR LIBRETA",7,4,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(852,524,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(525,"PAGO_LIBRETA","PAGO POR LIBRETA",7,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(853,525,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");
        
/*Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(526,"PAGO_LIBRETA","PAGO POR LIBRETA",7,1,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(854,526,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");
        
/*Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(527,"PAGO_LIBRETA","PAGO POR LIBRETA",7,2,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(855,527,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");
        
/*Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(528,"PAGO_LIBRETA","PAGO POR LIBRETA",7,6,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(856,528,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");
        
        
/*Infantil Corto Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(529,"PAGO_LIBRETA","PAGO POR LIBRETA",7,7,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(857,529,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");
        
/*Infantil Largo Plazo Interno*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(530,"PAGO_LIBRETA","PAGO POR LIBRETA",7,8,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(858,530,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");
        
/*Infantil Corto Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(531,"PAGO_LIBRETA","PAGO POR LIBRETA",7,9,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(859,531,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");
        
/*Infantil Largo Plazo Externo*/        
insert into conf_partida_contable(Id_Conf_Partida_Contable,Identificador,Descripcion,Id_Agencia,Id_Linea_Ahorro,Id_Linea_Credito) values
(532,"PAGO_LIBRETA","PAGO POR LIBRETA",7,10,0);

		/*Depósito*/
		insert into conf_detalle_partida_contable 
		(Id_Conf_Detalle_Partida_Contable, Id_Conf_Partida_Contable,Identificador,Concepto,Cargo,Abono,Cuenta_Contable)
		values(860,532,"PAGO_LIBRETA","PAGO POR LIBRETA",0,1,"5210207002");