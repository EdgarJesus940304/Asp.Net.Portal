$(function () {

    //#region Clases
    function UserModel() {
        this.UserName = null;
        this.Password = null;
    }
    //#endregion

    var model = new UserModel();

    //#region Acciones

    $("#login").on("click", function () {
        model.UserName = $("#userkey").val();
        model.Password = $("#userPassword").val();
        if (!Validate())
            return;
        $.ajax('/Login/Login',
            {
                type: 'POST',
                data: { userModel: model },
                beforeSend: function () {
                    Swal.fire({
                        title: 'Validando creedenciales...',
                        html: 'Espere un momento',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        didOpen: () => {
                            Swal.showLoading();
                        }
                    });
                },
                success: function (response) {
                    if (response.Number != 200) {
                        Swal.close();
                        Swal.fire({
                            title: '¡Error!',
                            text: response.Message,
                            icon: 'warning',
                            confirmButtonText: 'OK'
                        });
                    } else {
                        window.location.href = "/Users";
                    }
                },
                error: function (error) {   
                    Swal.close();
                    var errorMessage = error.responseJSON.Message || 'Ocurrió un error inesperado.';
                    Swal.fire({
                        title: '¡Error!',
                        text: errorMessage,
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            });

    });
    //#endregion

    //#region Validaciones
    function Validate() {
        if (IsNullOrUndefined(model.UserName)) {
            return false;
        }
        if (IsNullOrUndefined(model.Password)) {
            return false;
        }

        return true;
    }
    //#endregion

});