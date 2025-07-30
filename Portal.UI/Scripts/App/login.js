$(function () {
    function UserModel() {
        this.UserKey = null;
        this.Password = null;
    }

    $("#login").on("click", function () {
        var model = new UserModel();
        model.UserKey = $("#userkey").val();
        model.UserPassword = $("#userPassword").val();
        $("#login").hide();
        $("#loading").show();
        $.ajax('/Login/Login',
            {
                type: 'POST',
                data: { userModel: model },
                success: function (response) {
                    if (response.Number != 200) {
                        $("#login").show();
                        $("#loading").hide();
                    } else {
                        window.location.href = "/Users/Index";
                    }
                },
                error: function (response) {
                    $("#login").show();
                    $("#loading").hide();
                    console.log(response);
                }
            });

    });

});