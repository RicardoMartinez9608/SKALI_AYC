var raiz = "";

function constructor(url) {
    raiz = url;
}

function Eventos_Inicial() {
    Enfocar();
    Calcular_Cuota();
    GuardarCredito();
    EnviarPreComite();
}

function EnviarPreComite() {
    $("#Enviar_Comite").click(function () {
        var id_credito = $("#Id_Credito").val();
        $.get(raiz + "/Comite/CargarPreComite",
            { id_credito: id_credito },
            function (result) {
                alertify.success("El crédito se ha sometido a comité");
            }
        ).fail(function () {
            alertify.error("el crédito no pudo colocarse en comité");
        });
    });
}

function Enfocar() {
    $("#Id_Linea").focus();
}

function Calcular_Cuota() {
    $("#Calcular_Cuota").click(function () {
        ProcesarCalculo();
    });
}

function ProcesarCalculo() {
    /*obtener los datos de linea de crédito*/
    var lineaCredito = $("#Id_Linea").val();

    $.get(raiz + "/Linea_Credito/LineaDetalle",
        { id_linea: lineaCredito },
        function (lineadetalle) {
            $("#Interes_Comision").val(lineadetalle.interes_Comision);
            var id_linea = lineadetalle.id_Linea;
            var monto = $("#Monto_pago").val();
            var id_periodo = $("#Id_Periodo").val();
            var plazo = $("#Plazo").val();
            var id_credito = $("#form_credito #Id_Credito").val();

            $.get(raiz + "/Credito/CuotaPrimaria",
                { 
                    id_linea: id_linea, 
                    monto: monto, 
                    id_periodo: id_periodo, 
                    plazo: plazo,
                    id_credito: 0
                },
                function (resultado) {
                    $("#Valor_Cuota").val(resultado.cuotaPrimaria.toFixed(2));
                    $("#PlanAyC").val(resultado.cuotaAyC);
                    $("#Valor_IVA").val(resultado.ivaCalculado);
                    $("#Cuota_Total").val(resultado.cuotaTotal);
                    $("#Numero_cuotas").val(resultado.no_Cuotas);
                });
        });
}

function GuardarCredito() {
    $("#Guardar_Credito").click(function () {
        var datos_credito = $("#form_credito").serializeAnything();
        var id_credito = $("#Id_Credito").val();
        if (id_credito > 0) {
            $.post(raiz + "/Credito/Edit", datos_credito, function (resultado) {
                alertify.success("El crédito se ha guardado correctamente");
                Detectar_Grid_Credito();
            }).fail(function (error) {
                alertify.error(error.responseText);
            });
        } else {
            $.post(raiz + "/Credito/Create", datos_credito, function (resultado) {
                alertify.success("El crédito se ha guardado correctamente");
                Detectar_Grid_Credito();
            }).fail(function (error) {
                alertify.error(error.responseText);
            });
        }
    });

    function Detectar_Grid_Credito() {
        $("#Mantenimiento").modal("hide");
        if ($("#ListaCreditos").length > 0) {
            $("#ListaCreditos").dxDataGrid("instance").refresh();
        } else {
            Cargar_Cuentas_Credito();
        }
    }
}