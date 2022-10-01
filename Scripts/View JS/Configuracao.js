setInterval(loadCfg, 10000);
function loadTable() {
    $.ajax({
        url: "./Configuracao/GetAllRobo",
        type: "GET",
        success: (data) => {
            $('table tbody').detach();
            $('table').append($('<tbody>'));
            $.each(data, (i, obj) => {
                var markup =
                    "<tr>" +
                    `<td>${obj.ID}</td>` +
                    `<td>${obj.Nome}</td>` +
                    `<td>${obj.Navegadores}</td>` +
                    `<td>${obj.QtdVotosBranco}</td>` +
                    `<td>${obj.QtdVotosNulo}</td>` +
                    `<td>${obj.QtdVotos}</td>` +
                    `<td>${obj.Chapa}</td>` +
                    `<td>${obj.Regional}</td>` +
                    `<td>${obj.UF}</td>` +
                    `<td>${getStatus(obj.Status)}</td>` +
                    `<td>${moment(obj.AtualizadoEm).format('MM/DD/YYYY h: mm: ss a')}</td>` +
                    `<td class='hide-on-init'><button data-toggle="tooltip" title="Edit" class="pd-setting-ed" data-regID="${obj.ID}" onclick="get(this)">` +
                    "<i class='fa fa-pencil-square-o' aria-hidden='true'></i></button>" +
                    `<button data-toggle="tooltip" title="Trash" class="pd-setting-ed" data-regID="${obj.ID}"` +
                    `onclick="deleteRobo(${obj.ID})">` +
                    "<i class='fa fa-trash-o' aria-hidden='true'></i></button></td>" +
                    "</tr > ";

                $("table tbody").append(markup);
            });
        }
    });
}
function getStatus(status) {
    switch (status) {
        case 0:
            return "Não iniciado"
        case 1:
            return "Ativado"
        case 2:
            return "Concluído"
        default:
            return "Sem descrição"
    }
}
function editRobo(a) {
    if (!$('#txtNome').val())
        alert("Campo nome é obrigatório.")
    else {
        $.ajax({
            url: `./Configuracao/SaveEditRobo/${a.getAttribute("data-regId")}`,
            type: "post",
            data: $('#formModal').serialize(),
            success: (data) => {
                clearModal()
                loadTable()
                alert(data.msg)
            }
        });
    }
}

function deleteRobo(id) {
    $.ajax({
        url: `./Configuracao/DeleteRobo?id=${id}`,
        type: 'post',
        data: $('form').serialize(),
        success: (data) => {
            clearModal();
            loadTable();
            alert(data.msg)
        }
    });
}
function addRobo() {
    $.ajax({
        url: './Configuracao/SaveEditRobo/0',
        type: 'post',
        data: $('form').serialize(),
        success: (data) => {
            clearModal();
            loadTable();
            alert(data.msg)
            //alert('Registro salvo com sucesso');
        }
    });
}
function loadCfg() {
    loadTable()
    $.ajax({
        url: "./Configuracao/GetConfig",
        type: "GET",
        success: (data) => {
            $('#cfgQtdNavegador').val(data.Navegadores)
            $('#ckDistribuicaoAutomatica').prop('checked', data.DistribuirAutomaticamente)
            $('#ckDistribuicaoAutomatica').val(data.DistribuirAutomaticamente ? 'true' : 'false')
            $('#cfgSenhaPadrao').val(data.SenhaPadrao)
            $('#cfgSenhaTroca').val(data.SenhaTroca)
            $('#cfgUrl').val(data.URL)
            $('#cfgNumVotacoes').val(data.NumeroDeVotacoes)
            $('#cfgNumChapas').val(data.NumeroDeChapas)
            $('#ultAlteracao').val(moment(data.UltimaAlteracao).format('MM/DD/YYYY h: mm: ss a'))

            if (data.Iniciado) {
                $('#nmOnLine').val(data.VotantesDoRoboOnline)
                hideAfterInit()
            }
        }
    });

    $.ajax({
        url: "./Configuracao/GetCountVotante",
        type: "GET",
        success: (data) => {
            $('#nmVotantes').val(data.Qtd)
        }
    });
}

function clearModal() {
    $('#txtNome').val('')
    $('#nmQtdBranco').val('')
    $('#nmQtdNulo').val('')
    $('#nmQtdVoto').val('')
    $('#nmChapaVotar').val('')
    $('#nmRegional').val('')
    $('#nmQtdNavegadores').val('')
    $('#txtUF').val('')
    $("#PrimaryModalblbgpro").modal('hide');
}

function get(a) {
    if (a.getAttribute("data-regId")) {
        $.ajax({
            url: `./Configuracao/GetRobo/${a.getAttribute("data-regId")}`,
            type: "GET",
            success: (data) => {
                $('#txtNome').val(data.Nome)
                $('#nmQtdBranco').val(data.QtdVotosBranco)
                $('#nmQtdNulo').val(data.QtdVotosNulo)
                $('#nmQtdVoto').val(data.QtdVotos)
                $('#nmChapaVotar').val(data.Chapa)
                $('#nmRegional').val(data.Regional)
                $('#txtUF').val(data.UF)
                $('#nmQtdNavegadores').val(data.Navegadores)
                document.getElementById("btnModal").onclick = function () { editRobo(a); };
                $("#PrimaryModalblbgpro").modal('show');
            }
        });
    }
    else {
        document.getElementById("btnModal").onclick = function () { addServer(); };
    }
}

function iniciar() {
    if (!$('#cfgStartPassword').val()) {
        alert('Informe uma senha, caso não exista senha padrão definida será necessário informar uma nas configurações dos robôs.')
        return;
    }
    Spinner();
    Spinner.show();
    $("html, body").animate({ scrollTop: 0 }, "slow");
    $.ajax({
        url: `./Configuracao/Iniciar`,
        type: "post",
        data: $('#formStart').serialize(),
        success: (data) => {
            if (data.ok) {
                hideAfterInit();
            }
            alert(data.msg)
        },
        complete: () => {
            Spinner.hide();
        },
    });
}

function hideAfterInit() {
    $('#menuStatus').addClass("bk-active")
    $('#menuStatusMobile').addClass("bk-active")
    $('#btnSalvarConfiguracao').remove()
    $('#divIniciar').remove()
    $('#btnAddRobo').remove()
    $('.hide-on-init').remove()
}

function gravarConfiguracao() {
    $.ajax({
        url: `./Configuracao/SaveEditConfig`,
        type: "post",
        data: $('#formConfiguration').serialize(),
        success: (data) => {
            clearModal(); loadTable();
            alert(data.msg)
            loadCfg()
        }
    });
}

function checkInput(a) {
    var value = a.value == 'true' ? 'false' : 'true'
    $(a).val(value)
}