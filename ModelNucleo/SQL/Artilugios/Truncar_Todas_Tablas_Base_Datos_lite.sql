use aycnucleo_alpha;
SET FOREIGN_KEY_CHECKS = 0;

truncate table accion;
truncate table acta;
/*truncate table activo_negocio;*/
truncate table ahorro;
truncate table banco;
truncate table cartera;
truncate table cobro_periodico;
truncate table cobro_periodico_historial;
/*truncate table colecta;*/
/*truncate table colecta_diaria;*/
truncate table comite;
truncate table costo_venta;
truncate table credito;
truncate table cuenta_cobrar;
/*truncate table cuenta_contable;*/
/*truncate table deduccion_desembolso;*/
/*truncate table deduccion_historial;*/
/*truncate table detalle_partida;*/
/*truncate table descripcion_documento;*/
/*truncate table documento;*/
/*truncate table evaluacion;*/
/*truncate table gastos;*/
/*truncate table grupo;*/
/*truncate table inventario_negocio;*/
/*truncate table linea_ahorro;*/
/*truncate table linea_credito;*/
truncate table linea_credito_historial;
/*truncate table naturaleza;*/
/*truncate table otro_ingreso;*/
/*truncate table partida_contable;*/
/*truncate table departamento_pais;*/
/*truncate table municipio_departamento;*/
truncate table persona;
truncate table referencia;
truncate table persona_referencia;
truncate table persona_relaciones;
truncate table persona_tipo_persona;
/*truncate table prestamo;*/
/*truncate table referencia;*/
/*truncate table referencia_bancaria;*/
truncate table transaccion_ahorro;
truncate table transaccion_caja;
truncate table transaccion_credito;
truncate table transaccion_simple;
/*truncate table evaluacion_referencia_persona;*/

truncate table asiento_contable;
truncate table detalle_asiento_contable;
truncate table saldodisponible;
truncate table efectivo;


SET FOREIGN_KEY_CHECKS = 1;