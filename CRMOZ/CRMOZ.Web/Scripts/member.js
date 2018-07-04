$(".member-search input").on("keyup", function () {
    searchMember();
});

function searchMember() {
    let filter;
    filter = $(".member-search input").val().toLowerCase();
    $('#list-member .member-item').each(function () {
        let item = $(this);
        let infoP = item.find(".member-info p").html().toLowerCase();
        let infoSpan = item.find(".member-info span").html().toLowerCase();
        if (infoP.indexOf(filter) > -1 || infoSpan.indexOf(filter) > -1) {
            item.css({ 'display': '' });
        }
        else {
            item.css({ 'display': 'none' });
        }
    });
}

function GetAllUser() {
    $.ajax({
        url: '/Account/GetAllUser',
        type: "POST",
        typeName: 'json',
        success: function (res) {
            let html = '';
            $.map(res.data, function (item) {
                html += '<a href="#" class="member-item clearfix" data-id="' + item.Id + '">';
                html += '<div class="member-img"><img src="' + item.Avartar + '" class="img-circle" /></div>';
                html += '<div class="member-info"><p>' + item.FullName + '</p><span>' + item.UserName + '</span></div>';
                html += '<div class="member-action">';
                html += '<span class="member-remove" data-id="' + item.Id + '"><i class="fa fa-trash"></i></span>';
                html += '</div></a>';
            });

            $('#list-member .list-member-body').html(html);
        },
        error: function () {
            $("#error").val("Lỗi hệ thống!");
            showToastr();
        }
    });
}

$('#list-member').on("click", ".member-item", function (e) {
    e.preventDefault();
    $('#main-sidebar').removeClass("main-sidebar-show");
    $('.member-item').removeClass('active');
    $(this).addClass('active');

    var item = $(this);
    var id = $(this).data('id');
    $.ajax({
        url: '/Account/GetUserInfo',
        type: "Post",
        typeName: 'json',
        data: { id: id },
        success: function (res) {
            if (res.success) {
                $('#modal-profile form').attr('action', '/Account/Edit');
                $('#Id').val(res.data.Id);
                $('#Email').val(res.data.Email);
                $('#FullName').val(res.data.FullName);
                $('#Email').val(res.data.Email);
                $("#Email").attr('readonly','readonly');
                $('#Password').val(res.data.PasswordHash);
                $('#ConfirmPassword').val(res.data.PasswordHash);
                $("#Avatar").val(res.data.Avartar);
                $("#avatar-box").html('<img src="' + res.data.Avartar + '" class="img-reponsive" />');
                $('.box-title').html(res.data.FullName);
                $('.form-password').hide();
                $('.btn-register').css({ 'display': 'inline-block' });
                $('.role-create').removeClass('hidden');
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

$('#list-member').on("click", ".member-remove",function () {
    var item = $(this);
    var id = $(this).data('id');
    bootbox.confirm({
        message: "Bạn có muốn xóa thành viên này!",
        buttons: {
            confirm: {
                label: 'Đồng ý',
                className: 'btn-success'
            },
            cancel: {
                label: 'Hủy',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result == true) {
                $.ajax({
                    url: '/Account/Delete',
                    type: "Post",
                    typeName: 'json',
                    data: { id: id },
                    success: function (res) {
                        if (res.success) {
                            $("#success").val(res.message);
                            showToastr();
                            window.location.href = '/Account/Register';
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
        }
    });
});

$('#file-avatar').change(function () {
    // Checking whether FormData is available in browser
    if (window.FormData !== undefined) {

        var fileUpload = $("#file-avatar").get(0);
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
                $("#Avatar").val(res.data);
                $("#avatar-box").html('<img src="' + res.data + '" class="img-reponsive" />');
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    } else {
        alert("FormData is not supported.");
    }
});

$('.role-create').click(function () {
    let id = $("#Id").val();
    $.ajax({
        url: '/Account/GetRoles',
        type: "Post",
        typeName: 'json',
        data: { id: id },
        success: function (res) {
            $('#role-email').val(res.email);
            $('#role-fullname').val(res.fullname);
            let html = '';
            $.map(res.data, function (item) {
                console.log(item.Checked)
                if (item.Checked == true) {
                    html += '<option selected="selected" value="' + item.ID + '">' + item.Name + '</option>';
                }
                else {
                    html += '<option value="' + item.ID + '">' + item.Name + '</option>';
                }
            });
            $('#select-role').html(html);
            $('#modal-role').modal('show');
        },
        error: function () {
            $("#error").val("Lỗi hệ thống!");
            showToastr();
        }
    });
});

$('#role-save').click(function () {
    let id = $("#Id").val();
    let roleId = $('#select-role').val();
    $.ajax({
        url: '/Account/UpdateRoles',
        type: "Post",
        typeName: 'json',
        data: { id: id, roleId: roleId },
        success: function (res) {
            if (res.success) {
                $("#success").val(res.message);
                showToastr();
                $('#modal-role').modal('hide');
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
