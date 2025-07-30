$(function () {

    GetMedications();

    //#region Variables
    var url = null;
    var medication = new MedicationModel();
    //#endregion

    //#region Clases
    function MedicationModel() {
        this.Id = null;
        this.Name = null;
        this.Concentration = null;
        this.Price = 0;
        this.Stock = 0;
        this.Presentation = null;
        this.Enable = 1;
        this.PharmaceuticalForm = new PharmaceuticalFormModel();
    }
    function PharmaceuticalFormModel() {
        this.Id = null;
        this.Name = null;
    }
    //#endregion

    $("#closeButton").on("click", function () {
        Clean();
        $("#medicationModal").modal("hide");
    })

    $('#price').on('blur', function () {
        var valor = $(this).val().replace(/[^0-9.]/g, '');

        var numero = parseFloat(valor);
        if (!isNaN(numero)) {
            $(this).val(
                new Intl.NumberFormat('es-MX', {
                    style: 'currency',
                    currency: 'MXN',
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                }).format(numero)
            );
        } else {
            $(this).val('');
        }
    });

    //#region Tablero
    function GetMedications() {
        $('#medicationTable').DataTable({
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
                url: "Medications/ListMedications",
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
                    "data": "Concentration",
                    "searchable": true,
                    "className": "dt-left",
                },
                {
                    "data": "PharmaceuticalForm",
                    render: function (data) {
                        return data?.Name || '';
                    },
                    "searchable": true,
                    "className": "dt-left",

                },
                {
                    "data": "Price",
                    "searchable": true,
                    "className": "dt-right",
                    "render": $.fn.dataTable.render.number(',', '.', 2, '$'),
                },
                {
                    "data": "Presentation",
                    "searchable": true,
                    "className": "dt-center",
                },
                {
                    "data": "Stock",
                    "searchable": true,
                    "className": "dt-right",
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
                        return '<button title="Ver" class="btn btn-sm btn-info btn-search"><i class="fa fa-search"></i></button> <button title="Actualizar" class="btn btn-sm btn-primary btn-update"><i class="fa fa-pencil"></i></button> <button title="Eliminar" class="btn btn-sm btn-danger btn-delete"><i class="fa fa-trash"></i></button>'

                    }
                }
            ]

        });
    }
    //#endregion

    //#region Consultar
    $('#medicationTable tbody').on('click', '.btn-search', function () {
        Clean();

        var fila = $(this).closest('tr');
        var data = $('#medicationTable').DataTable().row(fila).data();
        $("#titleModal").text("Consultar medicamento");
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
    $("#newMedication").on("click", function () {
        Clean();
        url = "/Medications/SaveMedication";
        $("#titleModal").text("Nuevo medicamento");
        $("#medicationModal").modal("show");
    });

    $('#usersTable tbody').on('click', '.btn-update', function () {
        Clean();
        url = "/Medications/UpdateMedication";
        var fila = $(this).closest('tr');
        var data = $('#usersTable').DataTable().row(fila).data();
        user.Id = data.Id;

        $("#titleModal").text("Editar medicamento");
        GetData();
    });

    $("#saveButton").on("click", function () {
        var price = $('#price').val().replace(/[^0-9.]/g, '');

        medication.Name = $("#name").val();
        medication.Concentration = $("#concentration").val();
        medication.Presentation = $("#presentation").val();
        medication.Price = price;
        medication.Stock = $("#stock").val();
        medication.Enable = $('#status').is(':checked') ? 1 : 0;
        SaveOrUpdate();
    })

    //#endregion

    //#region Eliminar
    $('#medicationTable tbody').on('click', '.btn-delete', function () {
        Clean();
        var fila = $(this).closest('tr');
        var data = $('#medicationTable').DataTable().row(fila).data();

        Swal.fire({
            title: '¡Atención!',
            text: '¿Estás seguro de eliminar el medicamento ' + data.Name + '?',
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
                            $("#medicationModal").modal("hide");
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
        medication = new MedicationModel();
        $("#name").val("");
        $("#concentration").val("");
        $("#presentation").val("");
        $("#price").val(0);
        $("#stock").val(0);
        $('#status').prop('checked', true);

        $('#name').prop('disabled', false);
        $('#concentration').prop('disabled', false);
        $('#presentation').prop('disabled', false);
        $('#price').prop('disabled', false);
        $('#stock').prop('disabled', false);
        $('#status').prop('disabled', false);
        $('#saveButton').show();
        url = null;
    }
    //#endregion

    //#region Funciones
    function SaveOrUpdate() {
        $.ajax({
            url: url,
            type: "POST",
            data: { medication: medication },
            success: function (response) {
                if (response.Number == 200) {
                    $("#medicationModal").modal("hide");
                    Swal.fire({
                        title: '¡Correcto!',
                        text: response.Message,
                        icon: 'success',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            GetMedications();
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
            url: "/Medications/GetMedications",
            type: "GET",
            data: { Id: medication.Id },
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
                    $("#medicationModal").modal("show");
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