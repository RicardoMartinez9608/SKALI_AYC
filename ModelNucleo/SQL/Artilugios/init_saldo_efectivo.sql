delete from aycnucleo.saldodisponible;
ALTER TABLE aycnucleo.saldodisponible AUTO_INCREMENT = 0;

INSERT INTO `aycnucleo`.`saldodisponible`
(
`fecha`,
`saldo`,
`id_caja`,
`Cantidad_aki`,
`Cantidad_cheque`,
`Efectivo_total`,
`Numero_cheque`,
`Saldo_anterior`)
VALUES
(
'2019-12-02',
1923.75,
15,
0.00,
0.00,
0.00,
0.00,
0.00),
(
'2019-12-02',
3256.82,
14,
0.00,
0.00,
0.00,
0.00,
0.00);
