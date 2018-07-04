$('#view-profile').click(function (e) {
    e.preventDefault();
    let id = $(this).data("id");
    $.ajax({
        url: '/Account/GetUserInfo',
        type: "Post",
        typeName: 'json',
        data: { id: id },
        success: function (res) {
            if (res.success) {
                $('#profile-id').val(res.data.Id);
                $('#profile-email').val(res.data.Email);
                $('#profile-fullname').val(res.data.FullName);
                $("#profile-avatar").val(res.data.Avartar);
                $("#profile-avatar-box").html('<img src="' + res.data.Avartar + '" class="img-reponsive" />');
                $('#modal-profile').modal('show');
            }
            else {
                $("#error").val(res.message);
                showToastr();
            }
        },
        error: function () {
            $("#error").val("Lỗi hệ thống!");
            showToastr();
        }
    });
});

$('#profile-file-avatar').change(function () {
    // Checking whether FormData is available in browser
    if (window.FormData !== undefined) {

        var fileUpload = $("#profile-file-avatar").get(0);
        var files = fileUpload.files;

        // Create FormData object
        var fileData = new FormData();

        // Looping over all files and add it to FormData object
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        };

        $.ajax({
            url: '/Upload/UploadImage',
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: fileData,
            success: function (res) {
                $("#profile-avatar").val(res.data);
                $("#profile-avatar-box").html('<img src="' + res.data + '" class="img-reponsive" />');
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    } else {
        alert("FormData is not supported.");
    }
});

$('#profile-save').click(function () {
    let id = $("#profile-id").val();
    let fullname = $("#profile-fullname").val();
    let avatar = $("#profile-avatar").val();
    $.ajax({
        url: '/Account/EditInfo',
        type: "POST",
        typeName: 'json',
        data: {
            id: id,
            fullname: fullname,
            avatar: avatar
        },
        success: function (res) {
            if (res.success) {
                $('#profile-mail').val(res.data.Email);
                $('#profile-fullname').val(res.data.FullName);
                $("#profile-avatar").val(res.data.Avartar);
                $("#profile-avatar-box").html('<img src="' + res.data.Avartar + '" class="img-reponsive" />');
                $("#success").val(res.message);
                showToastr();
                window.location.href = "/";
            }
            else {
                $("#error").val(res.message);
                showToastr();
            }
        },
        error: function () {
            $("#error").val("Lỗi hệ thống!");
            showToastr();
        }
    });
});

$('#view-password').click(function (e) {
    e.preventDefault();
    let id = $(this).data("id");
    $.ajax({
        url: '/Account/GetInfoPassword',
        type: "Post",
        typeName: 'json',
        data: { id: id },
        success: function (res) {
            if (res.success) {
                $('#change-password-id').val(id);
                $('#change-password-email').val(res.email);
                $('#modal-password').modal('show');
            }
            else {
                $("#error").val(res.message);
                showToastr();
            }
        },
        error: function () {
            $("#error").val("Lỗi hệ thống!");
            showToastr();
        }
    });
});

$('#change-password-save').click(function (e) {
    e.preventDefault();
    let isValid = true;
    let id = $('#change-password-id').val();
    let password = $('#change-password').val();
    let confirm = $('#change-password-confirm').val();

    $('#error-change-password').hide();
    $('#error-change-password-confirm').hide();

    if (password.length <= 0) {
        $('#error-change-password').show();
        isValid = false;
    }
    if (confirm.length <= 0) {
        $('#error-change-password-confirm').show();
        isValid = false;
    }
    if (isValid) {
        $.ajax({
            url: '/Account/ChangePassword',
            type: "Post",
            typeName: 'json',
            data: {
                id: id,
                password: password
            },
            success: function (res) {
                if (res.success) {
                    $('#modal-password').modal('hide');
                    $('#log-off').submit();
                }
                else {
                    $("#error").val(res.message);
                    showToastr();
                }
            },
            error: function () {
                $("#error").val("Lỗi hệ thống!");
                showToastr();
            }
        });
    }
});

$('#config-save').click(function () {
    let isValid = true;
    let password = $("#config-password").val();
    let confirm = $('#config-confirm').val();

    $('#error-config-password').addClass('err-hide');
    $('#error-config-confirm').addClass('err-hide');

    if (password.length <= 0) {
        isValid = false;
        $('#error-config-password').removeClass('err-hide');
    }

    if (confirm.length <= 0 || password != confirm) {
        isValid = false;
        $('#error-config-confirm').removeClass('err-hide');
    }
    if (isValid == true) {
        $.ajax({
            url: '/Account/UpdateCommonPassword',
            type: "Post",
            typeName: 'json',
            data: { password: password },
            success: function (res) {
                if (res.success) {
                    $("#success").val(res.message);
                    showToastr();
                    $('#modal-config').modal('hide');
                }
                else {
                    $("#error").val(res.message);
                    showToastr();
                }
            },
            error: function () {
                $("#error").val("Lỗi hệ thống!");
                showToastr();
            }
        });
    }
});