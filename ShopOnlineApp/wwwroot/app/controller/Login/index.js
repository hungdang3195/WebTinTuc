var loginController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {
        $('#frmLogin').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtUserName: {
                    required: true
                },
                txtPassword: {
                    required: true
                }
            },
            messages: {
                txtUserName:{
                    required: "Bạn phải nhâp tên"
                },
                txtPassword: {
                    required: "Bạn phải nhập mật khẩu"
                }
            }
        });
        $('#btnLogin').on('click', function (e) {
            if ($('#frmLogin').valid()) {
                e.preventDefault();
                var user = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(user, password);
            }
        });
    }

    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                UserName: user,
                Password: pass
            },
            dateType: 'json',
            url: '/Admin/Login/authen',
            success: function (res) {
                if (res.success===true) {
                    window.location.href = "/Admin/Home/Index";
                    shoponline.notify('Đăng nhập thành công', 'success');
                }
                else {
                    shoponline.notify('Đăng nhập thất bại', 'error');
                }
            }
        })
    }
}