$(function () {

    $("#name").focus();

    //#region Clases
    function UserModel() {
        this.Id = null;
        this.Name = null;
        this.Password = null;
        this.UserName = null;
    }
    //#endregion

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

            },
            error: function (response) {

            }

        });
    })

    //var UriBase = "http://localhost:49868/";


    //var usersTable  = $('#usersTable').DataTable({
    //    destroy: true,
    //    "processing": true,
    //    "serverSide": true,
    //    "language": {
    //        url: '/Scripts/Plugins/Spanish.json'
    //    },
    //    "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable
    //    buttons: [],
    //    "order": [[0, "desc"]],
    //    "lengthMenu": [
    //        [10, 20, 100, 200, 500, 1000, - 1],
    //        [10, 20, 100, 200, 500, 1000, "Todos"]
    //    ],
    //    "ajax": {
    //        url: UriBase + "/api/users/show",
    //        type: 'POST',
    //        contentType: 'application/json',
    //        data: function (d) {
    //            return JSON.stringify({
    //                SearchBy: d.search.value,
    //                Take: d.length,
    //                Skip: d.start,
    //                SortBy: d.columns[d.order[0].column].data,
    //                SortDir: d.order[0].dir === 'asc'
    //            });
    //        }
    //    },
    //    columns: [
    //        {
    //            "data": "Nombre",
    //            "searchable": true,
    //            "name": "Nombre",

    //        },
    //        {
    //            "data": "FechaCreacion",
    //            "searchable": true,
    //            "name": "FechaCreacion",
    //        },
    //        {
    //            "data": "Nombre",
    //            "searchable": true,
    //            "name": "Nombre",

    //        },
    //        {
    //            "data": "Nombre",
    //            "searchable": true,
    //            "name": "Nombre",

    //        },
    //        {
    //            "data": null,
    //            "className": "dt-rigth",
    //            "orderable": false,
    //            "defaultContent": "",
    //            "render": function (data, type, row) {
    //                return '<button onclick="angular.element(this).scope().SetWorkshop(this)" title="Seleccionar"><i style="color:#d9534f;" class="fa fa-check"></i></button>'

    //            }
    //        }
    //    ], "drawCallback": function (settings) {
    //        $("#talleresModal").modal("show");
    //    }

    //});


});