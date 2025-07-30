$(function () {

    GetUsers();

    //#region Variables
    var usersTable = null;
    //#endregion

    //#region Clases
    function UserModel() {
        this.Id = null;
        this.Name = null;
        this.Password = null;
        this.UserName = null;
    }
    //#endregion

    //#region Consultar
    function GetUsers() {
        usersTable = $('#usersTable').DataTable({
            destroy: true,
            "processing": true,
            "serverSide": true,
            "language": {
                url: '/Scripts/DataTables/Spanish.json'
            },
            "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable
            buttons: [],
            "order": [[1, "asc"]],
            "lengthMenu": [
                [10, 20, 100, 200, 500, 1000, - 1],
                [10, 20, 100, 200, 500, 1000, "Todos"]
            ],
            "ajax": {
                url: "Users/ListUsers",
                type: 'POST',
                contentType: 'application/json',
                data: function (d) {
                    return JSON.stringify({
                        SearchBy: d.search.value,
                        Take: d.length,
                        Skip: d.start,
                        SortBy: d.columns[d.order[0].column].data,
                        SortDir: d.order[0].dir === 'asc'
                    });
                }
            },
            columns: [
                {
                    "data": "Name",
                    "searchable": true,
                    "className": "dt-left",

                },
                {
                    "data": "CreationDate",
                    "searchable": true,
                    "className": "dt-center",
                    render: function (data) {
                        if (!data) return '';

                        const match = /\/Date\((\d+)\)\//.exec(data);
                        if (!match) return data;

                        const timestamp = parseInt(match[1], 10);
                        const date = new Date(timestamp);

                        const day = String(date.getDate()).padStart(2, '0');
                        const month = String(date.getMonth() + 1).padStart(2, '0'); // Enero = 0
                        const year = date.getFullYear();

                        const hours = String(date.getHours()).padStart(2, '0');
                        const minutes = String(date.getMinutes()).padStart(2, '0');
                        const seconds = String(date.getSeconds()).padStart(2, '0');

                        return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
                    }
                },
                {
                    "data": "UserName",
                    "searchable": true,
                    "className": "dt-left",
                },
                {
                    "data": "StatusName",
                    "searchable": true,
                    "className": "dt-center",

                },
                {
                    "data": null,
                    "className": "dt-rigth",
                    "orderable": false,
                    "defaultContent": "",
                    "render": function (data, type, row) {
                        return '<button title="Seleccionar"><i class="fa fa-check"></i></button>'

                    }
                }
            ]

        });
    }
    //#endregion

    //#region Funciones alta, eliminar, editar

    $("#newUser").on("click", function () {
        $("#titleModal").text("Nuevo usuario");
        $("#userModal").modal("show");
    });

    $("#saveButton").on("click", function () {
        var user = new UserModel();
        user.Name = $("#name").val();
        user.UserName = $("#userName").val();
        user.Password = $("#name").val();

        $.ajax({
            url: "/Users/SaveUser",
            type: "POST",
            data: { user: user },
            success: function (response) {
                GetUsers();

            },
            error: function (response) {

            }

        });
    })

    //#endregion

});