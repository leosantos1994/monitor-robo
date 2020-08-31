loadCfg()
function loadTable() {
    $.ajax({
        url: "/Configuracao/GetAllServer",
        type: "GET",
        success: (data) => {
            $('table tbody').detach();
            $('table').append($('<tbody>'));
            $.each(data, (i, obj) => {
                var markup =
                    `<tr><td>${obj.Nome}</td>` +
                    `<td>${obj.URL}</td>` +
                    `<td>${moment(obj.CriadoEm).format('DD/MM/YYYY')}</td>` +
                    `<td><button class="pd-setting"></button></td>` +
                    `<td><button data-toggle="tooltip" title="Edit" class="pd-setting-ed" data-regID="${obj.ID}" onclick="get(this)">` +
                    "<i class='fa fa-pencil-square-o' aria-hidden='true'></i></button>" +
                    `<button data-toggle="tooltip" title="Trash" class="pd-setting-ed" data-regID="${obj.ID}"` +
                    `onclick="$.post('/Configuracao/Delete/${obj.ID}', loadTable())">` +
                    "<i class='fa fa-trash-o' aria-hidden='true'></i></button></td></tr>";

                $("table tbody").append(markup);
            });
        }
    });
}
function addServer() {
    $.ajax({
        url: '/Configuracao/SaveEditServer/0',
        type: 'post',
        data: $('form').serialize(),
        success: () => {
            clearModal();
            loadTable();
            alert('Registro salvo com sucesso');
        }
    });
}
function loadCfg() {
    $.ajax({
        url: "/Configuracao/GetConfig",
        type: "GET",
        success: (data) => {
            $('#cfgQtdNavegador').val(data.Navegadores)
            $('#cfgQtdVotosNavegador').val(data.VotosPorNavegador)
            $('#cfgSenhaPadrao').val(data.SenhaPadrao)
        }
    });
    loadTable()
}

function clearModal() {
    $('#listNome').val('')
    $('#listUrl').val('')
    $("#PrimaryModalblbgpro").modal('hide');
}

function get(a) {
    if (a.getAttribute("data-regId")) {
        $.ajax({
            url: `Configuracao/GetServer/${a.getAttribute("data-regId")}`,
            type: "GET",
            success: (data) => {
                $('#listNome').val(data.Nome)
                $('#listUrl').val(data.URL)
                document.getElementById("btnModal").onclick = function () { edit(a); };
                $("#PrimaryModalblbgpro").modal('show');
            }
        });
    }
    else {
        document.getElementById("btnModal").onclick = function () { addServer(); };
    }
}

function edit(a) {
    if (!$('#listNome').val() || !$('#listUrl').val())
        alert("Os campos são obrigatórios.")
    else {
        $.ajax({
            url: `Configuracao/SaveEditServer/${a.getAttribute("data-regId")}`,
            type: "post",
            data: $('#formModal').serialize(),
            success: (data) => {
                clearModal()
                loadTable()
                alert("Registro alterado com sucesso.")
            }
        });
    }
}