use aycnucleo;

alter table aycnucleo.persona modify FechaNacimiento datetime default null;
alter table aycnucleo.persona_relaciones drop foreign key FK_Persona_Relaciones_Ahorro_Id_Ahorro;

/*1 migracion de los usuarios*/
call Migrar_Socios;

/*alter table aycnucleo.persona modify FechaNacimiento datetime(6) NOT NULL;*/

/*2 migracion de los creditos*/
call Migrar_creditos;

/*3 migracion de los movimientos de abonos*/
call Migrar_Movimientos_Abonos;

/*4 migracion de las cuentas de ahorro*/
call Migrar_Cuentas_Ahorros;

/*5 migrar moviminetos de los ahorros*/
call Migrar_Movimiento_Ahorro();