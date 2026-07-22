var raiz = "";

function constructor(url) {
    raiz = url;
}

//buscando el una persona valida
$(function () {
    $("#dui_buscar").mask("00000000-0");
    BuscarPorDui();
    GenerarDTE();

});

function BuscarPorDui() {
    $("#buscarpordui").click(function () {
        if ($("#dui_buscar").val().length > 0) {
            var filtro = $("#dui_buscar").val();
            $.get(raiz + "/Persona/JDetalle_Persona",
                { filtro: filtro },
                function (detalle) {
                    $("span.nombre_completo").text(detalle.nombreCompleto);
                    $("span.dui").text(detalle.dui);
                    ObtenerPersona(detalle.id_Persona);
                }).fail(function (error) {
                    alertify.error(error.responseText);
                });
        } else {
            alertify.error("Ingrese un número de DUI");
        }

    });
}

function Mensaje(respuesta) {
    $("#mensajes").html("");
    $("#mensajes").html(respuesta);
}
function Mensaje_Abono(respuesta) {
    $("#mensaje_abono_ahorro").html("");
    $("#mensaje_abono_ahorro").html(respuesta);
}

function lookup_displayExpr(item) {
    if (!item) {
        return "";
    }
    return item.Nombres + " " + item.Apellidos + " " + item.Dui;
}

function ObtenerPersona(id_persona) {
    $("#id_persona_valor").val(id_persona);
    ObtenerAhorrosCombos(id_persona);
    ObtenerCreditosCombos(id_persona);
    ObtenerDepositosCombos(id_persona);
    /*otroscobros*/
    Estructuracion();
    Cobrar_Acciones();
    Cobrar_Libreta();
    
}

function ObtenerAhorrosCombos(id_persona) {
    $.get(raiz + "/Componentes/AhorrosCombo",
        { id_persona: id_persona },
        function (selectCombo) {
            $(".ahorros").html(selectCombo);
            ObtenerGridAhorro();
        }
    );
}
function ObtenerDepositosCombos(id_persona) {
    $.get(raiz + "/Componentes/ComboDeposito",
        { id_persona: id_persona },
        function (selectCombo) {
            $(".depositos").html(selectCombo);
            ObtenerGridDeposito();
        }
    );
}

//-----------------------------------------------------------------

function Siguiente_Comprobante() {

    $("#siguiente_comprobante").eq(0).off('click').click(function (e) {
        if ($('#numero_comprobante').val() == "") {
            alertify.error("Ingrese número de Comprobante.");
        } else {
            PasarValor();
        }
    });
}

function PasarValor() {
    MOModal_Abono_Credito_Show();
}

//-----------------------------------------------------------------

function ObtenerGridAhorro() {
    $("#id_ahorro_combo").change(function () {
        var id_ahorro = $(this).val();
        CargarGridTransacciones(id_ahorro);
        OperacionesAhorros();
        Libreta();
    });
}
function ObtenerGridDeposito() {
    $("#Id_Deposito").change(function () {
        var id_deposito = $(this).val();
        CargarGridTransaccionesDeposito(id_deposito);
        OperacionesDepositos();
        //Libreta();
    });
}
function CargarGridTransaccionesDeposito(id_deposito) {
    $.get(raiz + "/Depositos_Plazo/Transacciones",
        { id_deposito: id_deposito },
        function (transacciones) {
            $(".transacciones_deposito").html(transacciones);
            Revertir_Transaccion_Deposito();
            Mostrar_Factura_Transaccion_Deposito();
            //SeleccionarTodosAhorro();
        }
    );
}
function CargarGridTransacciones(id_ahorro, page = 1) {

    $(".transacciones").html(
        "<div class='text-center' style='padding:20px; color:#777;'>Cargando transacciones...</div>"
    );

    $.get(raiz + "/Ahorro/Transacciones",
        { id_ahorro: id_ahorro, page: page },
        function (transacciones) {
            $(".transacciones").html(transacciones);
            Revertir_Transaccion();
            Mostrar_Factura_Transaccion();
            SeleccionarTodosAhorro();
        }
    );
}
$(document).on("click", ".page-link", function (e) {
    e.preventDefault();

    var page = $(this).data("page");
    var id_ahorro = $("#id_ahorro_combo").val();

    CargarGridTransacciones(id_ahorro, page);
});
function MOModal_Abono_Ahorro_Hide() {
    $('#Modal_Para_Ahorros').modal('hide');
}
function MOModal_Retiro_Ahorro_Hide() {
    $('#Modal_Para_Retiros').modal('hide');
}
function MOModal_Abono_Depositos_Hide() {
    $('#Modal_Para_Depositos_Plazo').modal('hide');
}
function MOModal_Retiro_Depositos_Hide() {
    $('#Modal_Para_Retiros_Plazo').modal('hide');
}
function Libreta() {

    $("#siguiente_libreta").click(function () {
        if ($('#numero_renglon').val() == "" || $('#nueva_renglon').val() > 60) {
            alertify.error("Ingrese los datos.");
        } else if ($("#id_transaccion:checked").val() == undefined) {
            alertify.error("Seleccione una transacción");
        }
        else {
            var nrenglon = $("#numero_renglon").val();
            var id_ahorro = $("#id_ahorro_combo option:selected").val();
            var id_transaccion = $("#id_transaccion:checked").val();
            window.open(raiz +
                "/Ahorro/Postear_Libreta?id_ahorro=" +
                id_ahorro +
                "&id_transaccion=" +
                id_transaccion +
                "&nrenglon=" +
                nrenglon);

        }
    });
}
function OperacionesDepositos() {
    $("#deposito_plazo").click(function () {
        $("#siguiente_comprobante_deposito").eq(0).off('click').click(function (e) {
            var comprobanteAhorro = $("#numero_comprobante_deposito").val();
            $('#ncomprobante').html(comprobanteAhorro);
            if ($('#numero_comprobante_deposito').val() == "") {
                alertify.error("Ingrese número de Comprobante.");

            } else {
                MOModal_Abono_Depositos_Hide();
                var id_deposito = $("#Id_Deposito").val();
                var valor = $("#valor").val();
                var comprobante = $('#ncomprobante').text();

                var fecha_cambiada = $('#fecha_cambiada').val();
                var cambiar_fecha = $('#cambiar_fecha').is(':checked');

                var action = raiz + "/Depositos_Plazo/RegistrarDeposito";
                OperacionGeneralDeposito(action, id_deposito, valor, comprobante, fecha_cambiada, cambiar_fecha);
                $('.limpiar').val('');
                $('.cdeposito').val('');
            }
        });
    });

    $("#retiro_plazo").click(function () {
        $("#siguiente_comprobante_retiro_deposito").eq(0).off('click').click(function (e) {
            var comprobanteRetiro = $("#numero_comprobante_retiro_deposito").val();
            $('#ncomprobanteretiro').html(comprobanteRetiro);
            if ($('#numero_comprobante_retiro_deposito').val() == "") {
                alertify.error("Ingrese número de Comprobante.");
            } else {
                MOModal_Retiro_Depositos_Hide();
                var id_deposito = $("#Id_Deposito").val();
                var valor = $("#valor").val();
                var comprobante = $('#ncomprobanteretiro').text();

                var fecha_cambiada = $('#fecha_cambiada').val();
                var cambiar_fecha = $('#cambiar_fecha').is(':checked');

                var action = raiz + "/Depositos_Plazo/RegistrarRetiro";
                OperacionGeneralDeposito(action, id_deposito, valor, comprobante, fecha_cambiada, cambiar_fecha);
                $('.limpiar').val('');
                $('.cretiro').val('');
            }
        });
    });
}
function OperacionesAhorros() {
    $("#deposito").click(function () {
        $("#siguiente_comprobante_ahorro").eq(0).off('click').click(function (e) {
            var comprobanteAhorro = $("#numero_comprobante_ahorro").val();
            $('#ncomprobante').html(comprobanteAhorro);
            if ($('#numero_comprobante_ahorro').val() == "") {
                alertify.error("Ingrese número de Comprobante.");

            } else {
                MOModal_Abono_Ahorro_Hide();
                var id_ahorro = $("#id_ahorro_combo").val();
                var valor = $("#valor").val();
                var comprobante = $('#ncomprobante').text();

                var fecha_cambiada = $('#fecha_cambiada').val();
                var cambiar_fecha = $('#cambiar_fecha').is(':checked');

                var action = raiz + "/Ahorro/RegistrarDeposito";
                OperacionGeneral(action, id_ahorro, valor, comprobante, fecha_cambiada, cambiar_fecha);
                $('.limpiar').val('');
                $('.cdeposito').val('');
            }
        });
    });

    $("#retiro").click(function () {
        $("#siguiente_comprobante_retiro").eq(0).off('click').click(function (e) {
            var comprobanteRetiro = $("#numero_comprobante_retiro").val();
            $('#ncomprobanteretiro').html(comprobanteRetiro);
            if ($('#numero_comprobante_retiro').val() == "") {
                alertify.error("Ingrese número de Comprobante.");
            } else {
                MOModal_Retiro_Ahorro_Hide();
                var id_ahorro = $("#id_ahorro_combo").val();
                var valor = $("#valor").val();
                var comprobante = $('#ncomprobanteretiro').text();

                var fecha_cambiada = $('#fecha_cambiada').val();
                var cambiar_fecha = $('#cambiar_fecha').is(':checked');
                
                var action = raiz + "/Ahorro/RegistrarRetiro";
                OperacionGeneral(action, id_ahorro, valor, comprobante, fecha_cambiada, cambiar_fecha);
                $('.limpiar').val('');
                $('.cretiro').val('');
            }
        });
    });
}
function OperacionGeneralDeposito(
    action,
    id_deposito,
    valor,
    comprobante,
    fecha_cambiada,
    cambiar_fecha) {

    console.log(fecha_cambiada + " " + cambiar_fecha);

    $.post(action,
        {
            id_deposito: id_deposito,
            valor: valor,
            comprobante: comprobante,
            fecha_cambiada: fecha_cambiada,
            cambiar_fecha: cambiar_fecha
        },
        function (mensaje) {
            Mensaje(mensaje);
            CargarGridTransaccionesDeposito(id_deposito);
        }
    );
}
function OperacionGeneral(
    action,
    id_ahorro,
    valor,
    comprobante,
    fecha_cambiada,
    cambiar_fecha) {
    $.post(action,
        {
            id_ahorro: id_ahorro,
            valor: valor,
            comprobante: comprobante,
            fecha_cambiada: fecha_cambiada,
            cambiar_fecha: cambiar_fecha
        },
        function (mensaje) {
            Mensaje(mensaje);
            CargarGridTransacciones(id_ahorro);
        }
    );
}
/*manejo de postear, revertir y condonar*/
function SeleccionarTodosAhorro() {
    $("input[name='todas_transacciones_ahorro']").click(function () {
        $("input[name='transaccion_ahorro']").not(this).prop('checked', this.checked);
    });
}

function Revertir_Transaccion() {
    $("a.revertir").click(function () {
        /*reviertir transaccion de ahorro */
        var id_transaccion_ahorro = $("input[name='transaccion_ahorro']:checked").val();
        if (id_transaccion_ahorro.length > 0) {
            Enviar_Transaccion_Revertida(id_transaccion_ahorro, false);
        }

    });
}
function Revertir_Transaccion_Deposito() {
    $("a.revertir").click(function () {
        /*reviertir transaccion de ahorro */
        var id_transaccion_ahorro = $("input.id_transaccion_valor").first().val();
        if (id_transaccion_ahorro.length > 0) {
            Enviar_Transaccion_Revertida_Deposito(id_transaccion_ahorro, false, true);
        }

    });
}
function DatosLinea() {
    var ids_transaccion = [];
    var valores = "";
    var valor1 = "";
    var valor2 = "";
    var valor3 = "";
    var valor4 = "";
    var valor5 = "";
    $.each($("input[name='transaccion_ahorro']:checked"), function (index_ahorro) {
        ids_transaccion.push($(this).val());
        $(this).parents("tr").find("td").each(function () {
            valores += $(this).html();
            valor1 = $(this).parents("tr").find("td").eq(0).text();
            valor2 = $(this).parents("tr").find("td").eq(1).text();
            valor3 = $(this).parents("tr").find("td").eq(2).text();
            valor4 = $(this).parents("tr").find("td").eq(3).text();
            valor5 = $(this).parents("tr").find("td").eq(4).text();
        });
    });
}

function Agregar_Check_Ahorro() {
    var ids_transaccion_ahorro = [];
    $.each($("input[name='transaccion_ahorro']:checked"), function (index_ahorro) {
        ids_transaccion_ahorro.push($(this).val());
    });
    return ids_transaccion_ahorro;
}
function Enviar_Transaccion_Revertida(ids, es_credito) {
    console.log(ids);
    $.ajax({
        type: "POST",
        url: raiz + "/Caja/Revertir_Transaccion",
        data: { ids: ids, es_credito: es_credito }
    }).done(function (respuesta) {
        /*actualizar la lista de acuerdo a lo que sea*/
        Mensaje(respuesta);
        var id_ahorro = $("#id_ahorro_combo").val();
        CargarGridTransacciones(id_ahorro);
    });
}
function Enviar_Transaccion_Revertida_Deposito(ids, es_credito, es_deposito) {
    $.ajax({
        type: "POST",
        url: raiz + "/Caja/Revertir_Transaccion",
        data: { ids: ids, es_credito: es_credito, es_deposito: es_deposito }
    }).done(function (respuesta) {
        /*actualizar la lista de acuerdo a lo que sea*/
        Mensaje(respuesta);
        var id_deposito = $("#Id_Deposito").val();
        CargarGridTransaccionesDeposito(id_deposito);
    });
}
function Enviar_Transaccion_Revertida_Credito(ids, es_credito) {
    $.ajax({
        type: "POST",
        url: raiz + "/Caja/Revertir_Transaccion",
        data: { ids: ids, es_credito: es_credito }
    }).done(function (respuesta) {
        /*actualizar la lista de acuerdo a lo que sea*/
        Mensaje(respuesta);
        MostrarTransaccionesCreditoEfectuadas();
    });
}
function Enviar_Transaccion_Documento(id_transaccion, es_credito) {
    var a = document.createElement('a');
    if (es_credito) {
        a.href = raiz + "/Credito/Mostrar_Factura_Abono?id_transaccion=" + id_transaccion;
    }
    else {
        a.href = raiz + "/Ahorro/Mostrar_Recibo?id_transaccion=" + id_transaccion;
    }
    a.target = '_blank';
    document.body.appendChild(a);
    a.click();
}
function Enviar_Transaccion_Deposito(id_transaccion_deposito) {
    var a = document.createElement('a');
   
    a.href = raiz + "/Depositos_Plazo/Mostrar_Recibo?id_transaccion_deposito=" + id_transaccion_deposito;
    
    a.target = '_blank';
    document.body.appendChild(a);
    a.click();
}
//para mostrar factura de transacciones simples
function Enviar_Transaccion_Simple_Documento(id_transaccion_simple) {
    var a = document.createElement('a');
    a.href = raiz + "/Caja/Mostrar_Comprobante_Factura?id_transaccion_simple=" + id_transaccion_simple;
    a.target = '_blank';
    document.body.appendChild(a);
    a.click();
}

function Mostrar_Factura_Transaccion() {
    $("a.postear").click(function () {
        /*ids_ahorro para sus transacciones */
        var id_ahorro = $("input[name='transaccion_ahorro']:checked").eq(0).val();
        /*ids_comunes para transacciones*/
        var id_transaccion_simple = $("input[name='transaccion_comun']:checked").eq(0).val();

        var id_credito = $("input[name='transaccion_credito']:checked").eq(0).val();

        if (id_credito != null) {
            Enviar_Transaccion_Documento(id_credito, true);
        }

        if (id_ahorro != null) {
            Enviar_Transaccion_Documento(id_ahorro);
        }

        if (id_transaccion_simple != null) {
            Enviar_Transaccion_Simple_Documento(id_transaccion_simple);
        }
    });
}
function GenerarDTE() {
    $(document).on("click", "a.generarDTE", function () {
        var seleccionadas = $("input[name='transaccion_credito']:checked");
        var IDtransaccion = seleccionadas.eq(0).val();

        if (seleccionadas.length === 0) {
            alertify.alert('Advertencia', 'Por favor, selecciona una transacción antes de continuar.');
            return;
        }

        if (seleccionadas.length > 1) {
            alertify.alert('Advertencia', 'Por favor, selecciona solo una transacción.');
            return;
        }

        alertify.confirm(
            'Confirmación',
            '¿Estás seguro de que deseas generar el DTE para la transacción seleccionada?',
            function () {
                // Mostrar "blockUI"
                document.getElementById("customBlockUI").style.display = "block";

                $.ajax({
                    type: "POST",
                    url: raiz + "/Credito/GenerarDTECredito",
                    data: { IDtransaccion: IDtransaccion }
                }).done(function (respuesta) {
                    alertify.success(respuesta);
                }).fail(function () {
                    alertify.error("Ocurrió un error al generar el DTE.");
                }).always(function () {
                    // Ocultar "blockUI"
                    document.getElementById("customBlockUI").style.display = "none";
                });
            },
            function () {
                alertify.error('Operación cancelada.');
            }
        );
    });
}
function Mostrar_Factura_Transaccion_Deposito() {
    $("a.postear").click(function () {
        /*ids_ahorro para sus transacciones */
        var id_transaccion_deposito = $("input[name='transaccion_ahorro']:checked").eq(0).val();
        /*ids_comunes para transacciones*/
        if (id_transaccion_deposito != null) {
            Enviar_Transaccion_Deposito(id_transaccion_deposito);
        }

    });
}

/*functiones para credito*/
function ObtenerCreditosCombos(id_persona) {
    $.get(raiz + "/Componentes/CreditosCombos",
        { id_persona: id_persona },
        function (selectCombo) {
            $(".creditos").html(selectCombo);
            ObtenerConsultaParaPagoActual(id_persona);
        }
    );
}

//------------------------porcesos por consulta y pago----------------------------------------------------------------
function MOModal_Comprobante_Show(con_ahorro, id_persona) {
    //ConsultaPago();
    $('#Modal_Para_Comprobante').modal('show');

    $("#siguiente_comprobante").eq(0).off('click').click(function (e) {
        if ($('#numero_comprobante').val() == "") {
            alertify.error("Ingrese número de Comprobante.");
        } else {
            if (con_ahorro == 'True') {
                MOModal_Abono_Credito_Ahorro_Show(id_persona);
            } else {
                MOModal_Abono_Credito_Show();
            }

        }
    });

}

function IniciarAbono(con_ahorro, id_persona) {
    $(".abono").click(function () {
        MOModal_Comprobante_Show(con_ahorro, id_persona);
    });
}
function MOModal_Calcular_Anticipado() {
   
    $("#calcular").eq(0).off('click').click(function (e) {
        var porcentaje_descuento = 0.0;
        var pago_anticipado = $('input.total_pago_anticipado').val();
        var descuento = $('input.total_descuento').val();
        if ($("#sesenta").is(":checked")) {
            porcentaje_descuento = 0.6;
        }
        else if ($("#cincuenta").is(":checked")) {
            porcentaje_descuento = 0.5;
        }
        else if ($("#cuarenta").is(":checked")) {
            porcentaje_descuento = 0.4;
        }
        else if ($("#cero").is(":checked")) {
            porcentaje_descuento = 0;
        }

        $.post(raiz + "/Credito/Operacion_Pago_Anticipado",
            {
                porcentaje_descuento: porcentaje_descuento,
                pago_anticipado: pago_anticipado,
                descuento: descuento
            },
            function (consulta) {
                $("#t_pago").text(consulta);
            });
    });

}
function MOModal_Calculo_Show() {
    //ConsultaPago();
    $('#Modal_Calculo_Anticipado').modal('show');
  
        var id_credito = $("#id_credito_combo").val();
        $.get(raiz + "/Credito/Detalle_Pago_Anticipado",
            {
                id_credito: id_credito
            },
            function (consulta) {
                $("div.detalle_anticipado").html(consulta);
                MOModal_Calcular_Anticipado();
                Recalcular_Pago_Anticipado();
            });
    
}
function Recalcular_Pago_Anticipado() {

    $("#recalcular").eq(0).off('click').click(function (e) {
        if ($("#penalizacion").is(":checked")) {
            var id_credito = $("#id_credito_combo").val();
            var penalizacion = true;
            $.get(raiz + "/Credito/Detalle_Pago_Anticipado",
                {
                    penalizacion:penalizacion,
                    id_credito: id_credito
                },
                function (consulta) {
                    $("div.detalle_anticipado").html(consulta);
                    MOModal_Calcular_Anticipado();
                });
        }
    });

}
function Calcular_Pago_Anticipado() {
    $(".calcular_pago").click(function () {
        MOModal_Calculo_Show();
    });
  
}
function MOModal_Abono_Credito_Show() {
    ConsultaPago();
    $('#Modal_Para_Abono').modal('show');
}
function MOModal_Abono_Credito_Ahorro_Show(id_persona) {
    $("div.consultaParaPago").html("");
    ConsultaPago_Ahorro(id_persona);
    $('#Modal_Para_Abono_Ahorro').modal('show');
}
function MOModal_Abono_Credito_Hide() {
    $('#Modal_Para_Abono').modal('hide');
}
function MOModal_Abono_Credito_Ahorro_Hide() {
    $('#Modal_Para_Abono_Ahorro').modal('hide');
}
function MOModal_Comprobante_Hide() {
    $('#Modal_Para_Comprobante').modal('hide');
}
function ObtenerConsultaParaPagoActual(id_persona) {
    $("#id_credito_combo").change(function () {
        var cntrol = $(this);
        var con_ahorro = cntrol.find(':selected').data('bool');
       
        IniciarAbono(con_ahorro,id_persona);
        Calcular_Pago_Anticipado();
        MostrarTransaccionesCreditoEfectuadas();
    });
}

function ConsultaPago() {

    var comprobante = $("#numero_comprobante").val();
    $('#NComprobante').html(comprobante);

    var id_credito = $("#id_credito_combo").val();
    var aldia = true;

    ObtenerData(id_credito, aldia);
    ObtenerDataCalculadora();
}

function ConsultaPago_Ahorro(id_persona) {

    var comprobante = $("#numero_comprobante").val();
    $('#NComprobante').html(comprobante);

    var id_credito = $("#id_credito_combo").val();
    

    ObtenerData_Con_Ahorro(id_credito, id_persona);
   
}
function ObtenerData_Con_Ahorro(id_credito, id_persona) {

    $.get(raiz + "/Credito/Vista_Detalle_Credito_Ahorro",
        {
            id_credito: id_credito
        },
        function (consulta) {
            $("div.datos_ahorro").html(consulta);
          
            ObtenerDepositosCreditoCombos(id_persona);
            OperacionesAhorrosCredito(id_credito);
        }).fail(function (error) {
            alertify.error(error.responseText);
            $("div.datos_ahorro").html("");
        });
}
function ObtenerDepositosCreditoCombos(id_persona) {
    $.get(raiz + "/Componentes/AhorrosComboDeposito",
        { id_persona: id_persona },
        function (selectCombo) {
            $(".Datos_Deposito").html(selectCombo);
        }
    );
}
function OperacionesAhorrosCredito(id_credito) {
    $("#guardar_deposito_credito").click(function () {
        var comprobanteAhorro = $("#comprobante_credito_ahorro").val();
        if ($('#comprobante_credito_ahorro').val() == "") {
                alertify.error("Ingrese número de Comprobante.");
            } else {
            var id_ahorro = $("#Id_Ahorro").val();
            var valor = $("#valor_credito_ahorro").val();
            var comprobante = comprobanteAhorro;

                var action = raiz + "/Ahorro/RegistrarDeposito";
            {
                $.post(action,
                    {
                        id_ahorro: id_ahorro,
                        valor: valor,
                        comprobante: comprobante
                    },
                    function (mensaje) {
                        Mensaje_Abono(mensaje);
                        var aldia = true;
                        ObtenerData_Credito_Ahorro(id_credito,aldia);
                    }
                );
            }
            }
       
    });
}
function ObtenerData_Credito_Ahorro(id_credito, aldia, fecha, valorcuota, condonar_recargo) {
    $("div.datos_para_deposito").hide();
    $.get(raiz + "/Credito/ConsultaParaPago",
        {
            id_credito: id_credito,
            fecha: fecha,
            valor_cuota: valorcuota,
            aldia: aldia,
            condonar_recargo: condonar_recargo/*, saldo_capital: saldo_capital*/
        },
        function (consulta) {
            $("div.detalle_abono_ahorro").html("");
            $("div.detalle_abono_ahorro").html(consulta);
            Cambiando_Valor(true);
            GuardarAbonoCreditoAhorro();
            Condonar_recargo();
            Modal_Calculadora();
            Manera_Pago();
          
        }).fail(function (error) {
            alertify.error(error.responseText);
            $("div.datos_para_deposito").html("");
        });
}
function ObtenerData(id_credito, aldia, fecha, valorcuota, condonar_recargo) {

    $.get(raiz + "/Credito/ConsultaParaPago",
        {
            id_credito: id_credito,
            fecha: fecha,
            valor_cuota: valorcuota,
            aldia: aldia,
            condonar_recargo: condonar_recargo/*, saldo_capital: saldo_capital*/
        },
        function (consulta) {
            $("div.consultaParaPago").html("");
            $("div.consultaParaPago").html(consulta);
            Cambiando_Valor(false);
            Condonar_recargo();
            Modal_Calculadora();
            Manera_Pago();
            GuardarAbono();
        }).fail(function (error) {
            alertify.error(error.responseText);
            $("div.consultaParaPago").html("");
        });
}
function ObtenerDataCalculadora() {

    $.post(raiz + "/Calculadora_Caja/Calcular_Vuelto",
        {
            cuota: 0.00,
            efectivo: 0.0
        },
        function (consulta) {
            $("div.consultaCalculadora").html(consulta);

        });
}
function Modal_Calculadora() {
    $("#Calculadora").click(function () {
        $('#Modal_Calculadora').modal('show');
        Calcular();
        Cerrar_Calculadora();
    });

}

function Calcular() {
    $("input.Calcular_Cambio").click(function () {
        if ($("#efectivo").val() == "") {
            alertify.error("Ingresa la cantidad de efectivo");
        } else if ($("#efectivo").val() <= 0) {
            alertify.error("No se permiten cantidades negativas");
        } else {
            var cuota = parseFloat($("input.valor_tomado").val()).toFixed(2);
            var efectivo = $("#efectivo").val();
            $.post(raiz + "/Calculadora_Caja/Operaciones",
                {
                    valor_cuota: cuota,
                    valor_efectivo: efectivo
                },
                function (consulta) {
                    $("#cambio").val(consulta);
                });
        }
    });
}
function Cerrar_Calculadora() {
    $("input.Cerrar_Calculadora").click(function () {
        $('#Modal_Calculadora').modal('hide');
    });
}

function Cambiando_Valor(con_ahorro) {
    $("#ActualizarPago").click(function () {
        if (con_ahorro == true)
        {
            ActualizarForma_Ahorro_Credito();
        }else{
            ActualizarForma();
        }
       
    });
}

function ActualizarForma() {
    var id_credito = $("#Id_Credito").val();
    var fecha = $("#FechaPago").val();
    var condenar_recargo = false;
    var valor_cuota = 0;
    var aldia = $("input.mod_call").val();
    if (aldia == "true")
        valor_cuota = parseFloat($("input.valor_tomado").val()).toFixed(2);
    else
        valor_cuota = parseFloat($("#Pago_para_cancelar").val()).toFixed(2);
    ObtenerData(id_credito, aldia, fecha, valor_cuota, condenar_recargo);
}
function ActualizarForma_Ahorro_Credito() {
    var id_credito = $("#Id_Credito").val();
    var fecha = $("#FechaPago").val();
    var condenar_recargo = false;
    var valor_cuota = 0;
    var aldia = $("input.mod_call").val();
    if (aldia == "true")
        valor_cuota = parseFloat($("input.valor_tomado").val()).toFixed(2);
    else
        valor_cuota = parseFloat($("#Pago_para_cancelar").val()).toFixed(2);
    ObtenerData_Credito_Ahorro(id_credito, aldia, fecha, valor_cuota, condenar_recargo);
}

function Manera_Pago() {
    $("input.mod_call").each(function (index) {
        $("input.mod_call").eq(index).click(function () {
            var id_credito = $("#Id_Credito").val();
            var fecha = $("#FechaPago").val();
            var valor_cuota = 0;
            var aldia = $("input.mod_call").eq(index).val();
            if (aldia == "true")
                valor_cuota = parseFloat(0.00);//parseFloat($("#Valor_Cuota").val()).toFixed(2);
            else
                valor_cuota = parseFloat($("#Pago_para_cancelar").val()).toFixed(2);
            ObtenerData(id_credito, aldia, fecha, valor_cuota);
        });
    });
}
function Condonar_recargo() {
    $("input.quitar_recargo").click(function () {
        var condenar_recargo = true;
        var id_credito = $("#Id_Credito").val();
        var fecha = "";
        //var valor_Cuota = 0;
        var aldia = true;
        var valor_Cuota = parseFloat($("input.valor_tomado").val()).toFixed(2);
        //var recargo = parseFloat($("#Recargo").val()).toFixed(2);
        //var n = v_cuota - recargo;
        ObtenerData(id_credito, aldia, null, valor_Cuota, condenar_recargo);
    });
}
function MostrarTransaccionesCreditoEfectuadas() {
    var id_credito = $("#id_credito_combo").val();
    $.get(raiz + "/Credito/Transacciones_Efectuadas",
        { id_credito: id_credito },
        function (transacciones) {
            $("div.transaccionesCredito").html(transacciones);
            Revertir_Transaccion_Credito();
            Mostrar_Factura_Transaccion();
            SeleccionarTodosCredito();
        });
}
function Revertir_Transaccion_Credito() {
    $("a.revertir").click(function () {

        $(".transaccion_credito:checked").each(function () {
            var id_transaccion_credito = $(this).val();

            if (id_transaccion_credito) {
                Enviar_Transaccion_Revertida_Credito(id_transaccion_credito, true);
            }
        });

    });
}
function SeleccionarTodosCredito() {
    $("input[name='todas_transacciones_credito']").click(function () {
        $("input[name='transaccion_credito']").not(this).prop('checked', this.checked);
    });
}

function GuardarAbono() {
    $("#guardar_pago_credito").eq(0).off('click').click(function (e) {
        
        var recargo = false;
        if ($("input.quitar_recargo").prop('checked')) {
            recargo = true;
        }
        
        var transaccion = $("#ConsultaParaPago").serializeAnything();
        var comprobante = $("#numero_comprobante").val();
        $.post(raiz + "/Credito/GuardarPago?comprobante=" + comprobante + "&recargo=" + recargo, transaccion, function (respuesta) {
            Mensaje(respuesta);
            MostrarTransaccionesCreditoEfectuadas();
            MOModal_Abono_Credito_Hide();
            MOModal_Comprobante_Hide();
            $("div.consultaParaPago").html("");
            $("#numero_comprobante").val("");
            $(this).off('click');
        });
    });
}
function GuardarAbonoCreditoAhorro() {
    $("#guardar_pago_credito_ahorro").eq(0).off('click').click(function (e) {
        
        var recargo = false;
        if ($("input.quitar_recargo").prop('checked')) {
            recargo = true;
        }

        var transaccion = $("#ConsultaParaPago").serializeAnything();
        var comprobante = $("#numero_comprobante").val();
        $.post(raiz + "/Credito/GuardarPago?comprobante=" + comprobante + "&recargo=" + recargo, transaccion, function (respuesta) {
            Mensaje(respuesta);
            MostrarTransaccionesCreditoEfectuadas();
            MOModal_Abono_Credito_Ahorro_Hide();
            MOModal_Comprobante_Hide();
            $("div.detalle_abono_ahorro").html("");
            $("#numero_comprobante").val("");
            $(this).off('click');
        });
    });
}
/*Otros Cobros*/
function Estructuracion() {
    $("#mostrar_estructuracion").click(function () {
        var id_persona = $("#id_persona_valor").val();
        var fecha = $("#fecha_credito").val();
        $.get(raiz + "/Caja/Estructuraciones",
            {
                id_persona: id_persona,
                fecha: fecha
            },
            function (operaciones) {
                $("div.detalle").html(operaciones);
                //Mostrar_Factura_Transaccion();
            });
    });
}

function Cobrar_Acciones() {
    $("#mostrar_accion").click(function () {
        var id_persona = $("#id_persona_valor").val();
        $.get(raiz + "/Accion/Acciones_Cancelar",
            {
                id_persona: id_persona
            },
            function (acciones) {
                $("div.detalle").html(acciones);
                Registrar_Cobro_Accion();
                //Imprimir_Accion();
            });
    });
}

function Cobrar_Libreta() {
    $("#mostrar_libreta").click(function () {
        var id_persona = $("#id_persona_valor").val();
        $.get(raiz + "/Caja/Mostrar_Libreta",
            {
                id_persona: id_persona
            },
            function (acciones) {
                $("div.detalle").html(acciones);
                Cobro_Posteo_Libreta();
            });
    });
}
function Cobro_Posteo_Libreta() {
    $("#Postear_Libreta").click(function () {
        var id_ahorro = $("#Id_Ahorro_Libreta").val();
        var descuento = $("input[name='descuento']:checked").val();

        window.open(raiz +
            "/Caja/Mostrar_Factura_Libreta?id_ahorro=" +
            id_ahorro +
            "&retiro_cuenta=" +
            descuento);
    });
}
function Registrar_Cobro_Accion() {
    $("a.registrar_cobro_accion").each(function (index) {
        $("a.registrar_cobro_accion").eq(index).click(function () {
            var codigo = $("input.codigo").eq(index).val();
            var id_accion = $("input.id_accion").eq(index).val();
            ///llenar los datos del pop
            $("b.codigo_seleccionado").html(codigo);
            EjecutarCobro(id_accion);
        });
    });
}

function EjecutarCobro(id_accion) {
    $("#registrar_cobro").click(function () {
        Enviar_Cobro_Acciones_Factura(id_accion);
    });
}
function Enviar_Cobro_Acciones_Factura(id_accion) {
    console.log(id_accion);
    var a = document.createElement('a');
    a.href = raiz + "/Accion/Registrar_Cobro?id_accion=" + id_accion;
    a.target = '_blank';
    document.body.appendChild(a);
    a.click();
}