$(function () {

    GetUsers();

    //#region Variables
    var table = null;
    var url = null;
    var type = null;
    var user = new UserModel();
    //#endregion

    $("#newUser").on("click", function () {
        Clean();
        url = "/Users/SaveUser";
        type = "POST";
        $("#titleModal").text("Nuevo usuario");
        $("#userModal").modal("show");
    });

    //#region Clases
    function UserModel() {
        this.Id = null;
        this.Name = null;
        this.Password = null;
        this.UserName = null;
    }
    //#endregion

    //#region Tablero
    function GetUsers() {
        table = $('#usersTable').DataTable({
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
                    "className": "dt-center",
                    "orderable": false,
                    "defaultContent": "",
                    "render": function (data, type, row) {
                        return '<button title="Actualizar" class="btn btn-sm btn-primary btn-actualizar"><i class="fa fa-pencil"></i></button> <button title="Seleccionar" class="btn btn-sm btn-danger"><i class="fa fa-trash"></i></button>'

                    }
                }
            ]

        });
    }
    //#endregion

    //#region Consultar

    $('#usersTable tbody').on('click', '.btn-actualizar', function () {
        Clean();
        url = "/Users/UpdateUser";
        type = "PUT";
        var fila = $(this).closest('tr');
        var data = $('#usersTable').DataTable().row(fila).data();
        user.Id = data.Id;
        $.ajax({
            url: "/Users/GetUser",
            type: "GET",
            data: { Id: user.Id },
            success: function (response) {
                if (response.Number == 200) {
                    $("#name").val(response.Data.Name);
                    $("#userName").val(response.Data.UserName);
                    $("#password").val(response.Data.Password);
                    $("#userModal").modal("show");
                } else {
                    Swal.fire({
                        title: '¡Error!',
                        text: response.Message,
                        icon: 'error',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {

                        }
                    });
                }

            },
            error: function (response) {
                Swal.fire({
                    title: '¡Error!',
                    text: response.Message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {

                    }
                });
            }

        });

    });

    //#endregion

    //#region Edicion o Alta
    $("#saveButton").on("click", function () {
        user.Name = $("#name").val();
        user.UserName = $("#userName").val();
        user.Password = $("#password").val();
        SaveOrUpdate();
    })

    //#endregion

    //#region Eliminar

    $("#newUser").on("click", function () {
        $("#titleModal").text("Nuevo usuario");
        $("#userModal").modal("show");
    });

    //#endregion

    //#region Limpiar datos
    function Clean() {
        user = new UserModel();
        $("#name").val("");
        $("#userName").val("");
        $("#password").val("");

        url = null;
        type = null;
    }
    //#endregion

    //#region Funciones
    function SaveOrUpdate() {
        $.ajax({
            url: url,
            type: "POST",
            data: { user: user },
            success: function (response) {
                if (response.Number == 200) {
                    $("#userModal").modal("hide");
                    Swal.fire({
                        title: '¡Correcto!',
                        text: response.Message,
                        icon: 'success',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            GetUsers();
                        }
                    });
                } else {
                    Swal.fire({
                        title: '¡Error!',
                        text: response.Message,
                        icon: 'error',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {

                        }
                    });
                }

            },
            error: function (response) {
                Swal.fire({
                    title: '¡Error!',
                    text: response.Message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {

                    }
                });
            }

        });
    }
    //#endregion
});