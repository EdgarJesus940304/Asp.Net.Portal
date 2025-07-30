$(function () {

    GetUsers();

    //#region Variables
    var url = null;
    var user = new UserModel();
    //#endregion

    //#region Clases
    function UserModel() {
        this.Id = null;
        this.Name = null;
        this.Password = null;
        this.UserName = null;
        this.Status = 1;
    }
    //#endregion

    $("#closeButton").on("click", function () {
        Clean();
        $("#userModal").modal("hide");
    })

    //#region Tablero
    function GetUsers() {
        $('#usersTable').DataTable({
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
                    "className": "dt-left",

                },
                {
                    "data": null,
                    "className": "dt-center",
                    "orderable": false,
                    "defaultContent": "",
                    "render": function (data, type, row) {
                        return '<button title="Ver" class="btn btn-sm btn-info btn-search"><i class="fa fa-search"></i></button> <button title="Actualizar" class="btn btn-sm btn-primary btn-update"><i class="fa fa-pencil"></i></button> <button title="Eliminar" class="btn btn-sm btn-danger btn-delete"><i class="fa fa-trash"></i></button>'

                    }
                }
            ]

        });
    }
    //#endregion

    //#region Consultar
    $('#usersTable tbody').on('click', '.btn-search', function () {
        Clean();

        var fila = $(this).closest('tr');
        var data = $('#usersTable').DataTable().row(fila).data();
        $("#titleModal").text("Consultar usuario");
        $('#status').prop('disabled', true);
        $('#name').prop('disabled', true);
        $('#userName').prop('disabled', true);
        $('#password').prop('disabled', true);
        $('#saveButton').hide();
        user.Id = data.Id;
        GetData();
    });
    //#endregion

    //#region Edicion o Alta
    $("#newUser").on("click", function () {
        Clean();
        url = "/Users/SaveUser";
        $("#titleModal").text("Nuevo usuario");
        $("#userModal").modal("show");
    });

    $('#usersTable tbody').on('click', '.btn-update', function () {
        Clean();
        url = "/Users/UpdateUser";
        var fila = $(this).closest('tr');
        var data = $('#usersTable').DataTable().row(fila).data();
        user.Id = data.Id;

        $("#titleModal").text("Editar usuario");
        GetData();
    });

    $("#saveButton").on("click", function () {
        user.Name = $("#name").val();
        user.UserName = $("#userName").val();
        user.Password = $("#password").val();
        user.Status = $('#status').is(':checked') ? 1 : 0;
        SaveOrUpdate();
    })

    //#endregion

    //#region Eliminar
    $('#usersTable tbody').on('click', '.btn-delete', function () {
        Clean();
        var fila = $(this).closest('tr');
        var data = $('#usersTable').DataTable().row(fila).data();

        Swal.fire({
            title: '¡Atención!',
            text: '¿Estás seguro de eliminar al usuario ' + data.Name + '?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Sí, continuar',
            cancelButtonText: 'Cancelar',
            reverseButtons: false
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/Users/DeleteUser",
                    type: "POST",
                    data: { userId: data.Id },
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
                            });
                        }

                    },
                    error: function (response) {
                        Swal.fire({
                            title: '¡Error!',
                            text: response.Message,
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }

                });

            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire('Cancelado', 'La acción fue cancelada.', 'error');
            }
        });
    });
    //#endregion

    //#region Limpiar
    function Clean() {
        user = new UserModel();
        $('#status').prop('checked', true);
        $("#name").val("");
        $("#userName").val("");
        $("#password").val("");
        $('#status').prop('disabled', false);
        $('#name').prop('disabled', false);
        $('#userName').prop('disabled', false);
        $('#password').prop('disabled', false);
        $('#saveButton').show();
        url = null;
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
                    });
                }

            },
            error: function (response) {
                Swal.fire({
                    title: '¡Error!',
                    text: response.Message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }

        });
    }
    function GetData() {
        $.ajax({
            url: "/Users/GetUser",
            type: "GET",
            data: { id: user.Id },
            success: function (response) {
                if (response.Number == 200) {
                    $("#name").val(response.Data.Name);
                    $("#userName").val(response.Data.UserName);
                    $("#password").val(response.Data.Password);
                    $('#status').prop('checked', false);
                    if (response.Data.Status != null || response.Data.Status != undefined) {
                        if (response.Data.Status > 0)
                            $('#status').prop('checked', true);
                    }
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
    }
    //#endregion

});