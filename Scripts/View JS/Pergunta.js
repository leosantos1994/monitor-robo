
loadTable()

function loadTable() {
    $.ajax({
        url: "./Pergunta/GetAll",
        type: "GET",
        success: (data) => {
            $('table tbody').detach();
            $('table').append($('<tbody>'));
            $.each(data, (i, obj) => {
                var markup =
                    "<tr>" +
                    `<td>${obj.Pergunta}</td>` +
                    `<td> </td>` +
                    `<td> </td>` +
                    `<td> </td>` +
                    `<td> </td>` +
                    `<td> </td>` +
                    `<td>${getCampoResposta(obj.CampoRespostaPergunta)}</td>` +
                    `<td> </td>` +
                    `<td> </td>` +


                    `<td class='hide-on-init'><button data-toggle="tooltip" title="Edit" class="pd-setting-ed" data-regID="${obj.ID}" onclick="editPergunta(this)">` +
                    "<i class='fa fa-pencil-square-o' aria-hidden='true'></i></button>" +
                    `<button data-toggle="tooltip" title="Trash" class="pd-setting-ed" data-regID="${obj.ID}"` +
                    `onclick="deletePergunta(${obj.ID})">` +
                    "<i class='fa fa-trash-o' aria-hidden='true'></i></button></td>" +
                    "</tr > ";

                $("table tbody").append(markup);
            });
        }
    });
}

function editPergunta(a) {
    $.ajax({
        url: `./Pergunta/GetById?id=${a.getAttribute("data-regId")}`,
        type: "get",
        success: (data) => {
            $('#id').val(data.ID)
            $('#txtPergunta').val(data.Pergunta)
            $('#slPergunta').val(data.CampoRespostaPergunta)
            loadTable()
        }
    });
}

function deletePergunta(id) {
    $.ajax({
        url: `./Pergunta/Delete?id=${id}`,
        type: 'post',
        success: (data) => {
            loadTable();
            alert(data.msg)
        }
    });
}
function getCampoResposta(campo) {
    switch (campo) {
        case 1:
            return "Data de Nascimento"
        case 2:
            return "CPF"
        case 3:
            return "Nome da Mãe"
        default:
            return null;
    }
}