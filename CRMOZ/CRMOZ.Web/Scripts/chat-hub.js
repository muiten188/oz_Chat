var hub = $.connection.chatHub;

// Load danh sách user
hub.client.allUser = function (users) {
    let html = '';
    $.map(users, function (item) {
        if (item.Connected == true) {
            html += '<a href="javascript:void(0)" style="clear:both;" class="item-user" data-user="' + item.ID + '" data-fullname="' + item.FullName + '" data-name="' + item.UserName + '" data-id="' + item.ConnectionId + '" data-avatar="' + item.Avatar + '"><div class="contact-item">';
            html += '<div class="img-user"><img src="' + item.Avatar + '" class="img-circle" /><i class="fa fa-check-circle"></i></div>';
            html += '<div class="name-user"><span>' + item.FullName + '</span><span>' + item.Email + '</span></div>';
            html += '</div></a>';
        }
        else {
            html += '<a href="javascript:void(0)" style="clear:both;" class="item-user" data-user="' + item.ID + '" data-fullname="' + item.FullName + '" data-name="' + item.UserName + '" data-id="' + item.ConnectionId + '" data-avatar="' + item.Avatar + '"><div class="contact-item">';
            html += '<div class="img-user"><img src="' + item.Avatar + '" class="img-circle" /><i class="fa fa-times-circle"></i></div>';
            html += '<div class="name-user"><span>' + item.FullName + '</span><span>' + item.Email + '</span></div>';
            html += '</div></a>';
        }
    });
    $("#contact").html(html);
}

// Load danh sách user đã từng chat với bạn
hub.client.allMessageUser = function (users, count) {
    if (count > 0) {
        notification("Bạn có " + count + " tin nhắn mới chưa đọc!");
    }
    let html = '';
    $.map(users, function (item) {
        if (item.Connected == true) {
            html += '<a href="javascript:void(0)" style="clear:both;" class="item-user" data-user="' + item.ID + '" data-fullname="' + item.FullName + '" data-name="' + item.UserName + '" data-id="' + item.ConnectionId + '" data-avatar="' + item.Avatar + '"><div class="message-item">';
            html += '<div class="img-user"><img src="' + item.Avatar + '" class="img-circle" /><i class="fa fa-check-circle"></i></div>';
            html += '<div class="name-user"><span>' + item.FullName + '</span><span>' + item.Email + '</span></div>';
            if (item.CountNew > 0) {
                html += '<div class="count-new message-valid" id="count-new-' + item.ID + '">' + item.CountNew + '</div>';
            }
            else {
                html += '<div class="count-new" id="count-new-' + item.ID + '">' + item.CountNew + '</div>';
            }
            html += '</div></a>';
        }
        else {
            html += '<a href="javascript:void(0)" style="clear:both;" class="item-user" data-user="' + item.ID + '" data-fullname="' + item.FullName + '" data-name="' + item.UserName + '" data-id="' + item.ConnectionId + '" data-avatar="' + item.Avatar + '"><div class="message-item">';
            html += '<div class="img-user"><img src="' + item.Avatar + '" class="img-circle" /><i class="fa fa-times-circle"></i></div>';
            html += '<div class="name-user"><span>' + item.FullName + '</span><span>' + item.Email + '</span></div>';
            if (item.CountNew > 0) {
                html += '<div class="count-new message-valid" id="count-new-' + item.ID + '">' + item.CountNew + '</div>';
            }
            else {
                html += '<div class="count-new" id="count-new-' + item.ID + '">' + item.CountNew + '</div>';
            }
            html += '</div></a>';
        }
    });
    $("#message").html(html);
    $('#top-new-count span').html(count);
}

// Hàm xử lý khi có user connect
hub.client.connect = function (id, username, fullname) {
    notification(fullname + " đang online!");

    $('#message a').each(function () {
        let name = $(this).data('name');
        if (name == username) {
            $(this).attr('data-id', id);
            $(this).find('i').removeClass('fa-times-circle').addClass('fa-check-circle');
        }
    });

    $('#contact a').each(function () {
        let name = $(this).data('name');
        if (name == username) {
            $(this).attr('data-id', id);
            $(this).find('i').removeClass('fa-times-circle').addClass('fa-check-circle');
        }
    });
}

// Hàm xử lý khi có user disconnect
hub.client.disConnect = function (username, fullname) {
    $('#message a').each(function () {
        let name = $(this).data('name');
        if (name == username) {
            $(this).attr('data-id', '');
            $(this).find('i').removeClass('fa-check-circle').addClass('fa-times-circle');
        }
    });

    $('#contact a').each(function () {
        let name = $(this).data('name');
        if (name == username) {
            $(this).attr('data-id', '');
            $(this).find('i').removeClass('fa-check-circle').addClass('fa-times-circle');
        }
    });
}

// Hiển thị 1 tin nhắn mới
hub.client.messagePrivate = function (message, isMe) {
    let html = '';
    if (isMe) {
        if (message.IsFile == true) {
            html += '<div class="chat-message-item chat-message-img-right message-right clearfix">';
            html += '<div class="message-item-box row">';
            html += '<div class="message-item-text col-xs-11 lightgallery">';
            let arr = getArrayImage(message.Content);
            $.map(arr, function (value) {
                html += '<a href="' + value + '" class="item-image"><img src=' + value + ' width="100"/>';
                html += '</a>';
            });
            html += '</div>';
            let date = formatDate(message.CreatedDate);
            html += '<div class="message-item-img col-xs-1">';
            html += '<img src="' + message.FromAvatar + '" class="img-circle" width="40" height="40" />';
            html += '<div class="message-item-time" data-time="' + message.StrDateTime + '">' + date + '</div></div>';
            html += '</div></div>';
            $("#list-chat-message").append(html);
            $('.lightgallery').lightGallery({
                thumbnail: true,
                animateThumb: false,
                showThumbByDefault: false
            });
        }
        else {
            let item = $('.message-right:last-child');
            let strDate = item.find('.message-item-time').data('time');
            
            if (false&&message.StrDateTime == strDate) {
                let htmlChild = '<span>' + message.Content + '</span>';
                item.find('.item-text').append(htmlChild);
            }
            else {
                html += '<div class="chat-message-item message-right clearfix">';
                html += '<div class="message-item-box row">';
                html += '<div class="message-item-name">' + message.FromFullName + '</div>';
                html += '<div class="message-item-text col-xs-11">';
                html += '<div class="item-text"><span>' + message.Content + '</span></div></div>';
                let date = formatDate(message.CreatedDate);
                html += '<div class="message-item-img col-xs-1">';
                html += '<img src="' + message.FromAvatar + '" class="img-circle" width="40" height="40" />';
                html += '<div class="message-item-time" data-time="' + message.StrDateTime + '">' + date + '</div></div>';
                html += '</div></div>';
                $("#list-chat-message").append(html);
            }
        }
    }
    else {
        if (message.IsFile == true) {
            html += '<div class="chat-message-item chat-message-img-left message-left clearfix">';
            html += '<div class="message-item-box row">';
            html += '<div class="message-item-img col-xs-1">';
            html += '<img src="' + message.FromAvatar + '" class="img-circle" width="40" height="40" />';
            let date = formatDate(message.CreatedDate);
            html += '<div class="message-item-time">' + date + '</div></div>';
            html += '<div class="message-item-text col-xs-11 lightgallery">';
            let arr = getArrayImage(message.Content);
            $.map(arr, function (value) {
                html += '<a href="' + value + '" class="item-image"><img src=' + value + ' width="100"/>';
                html += '</a>';
            });
            html += '</div></div></div>';
            $("#list-chat-message").append(html);
            $('.lightgallery').lightGallery({
                thumbnail: true,
                animateThumb: false,
                showThumbByDefault: false
            });
        }
        else {
            let item = $('.message-left:last-child');
            let strDate = item.find('.message-item-time').data('time');
            if (false&&message.StrDateTime == strDate) {
                let htmlChild = '<span>' + message.Content + '</span>';
                item.find('.item-text').append(htmlChild);
            }
            else {
                html += '<div class="chat-message-item message-left clearfix">';
                html += '<div class="message-item-box row">';
                html += '<div class="message-item-name">' + message.FromFullName + '</div>';
                html += '<div class="message-item-img col-xs-1">';
                html += '<img src="' + message.FromAvatar + '" class="img-circle" width="40" height="40" />';
                let date = formatDate(message.CreatedDate);
                html += '<div class="message-item-time" data-time="' + message.StrDateTime + '">' + date + '</div></div>';
                html += '<div class="item-text"><span>' + message.Content + '</span></div>';
                html += '</div></div></div>';
                $("#list-chat-message").append(html);
            }
        }
    }
    $("#list-chat-message").scrollTop($("#list-chat-message")[0].scrollHeight);
}

// Hiển thị tất cả tin nhắn theo userid
hub.client.messagePrivates = function (messages, userId) {
    let strDateLeft = '';
    let strDateRight = '';
    $.map(messages, function (item) {
        let html = '';
        if (item.FromID == userId) {
            if (item.IsFile == true) {
                html += '<div class="chat-message-item chat-message-img-right message-right clearfix">';
                html += '<div class="message-item-box row">';
                html += '<div class="message-item-text col-xs-11 lightgallery">';
                let arr = getArrayImage(item.Content);
                $.map(arr, function (value) {
                    html += '<a href="' + value + '" class="item-image"><img src=' + value + ' width="100"/>';
                    html += '</a>';
                });
                html += '</div>';
                let date = formatDate(item.CreatedDate);
                html += '<div class="message-item-img col-xs-1">';
                html += '<img src="' + item.FromAvatar + '" class="img-circle" width="40" height="40" />';
                html += '<div class="message-item-time" data-time="' + item.StrDateTime + '">' + date + '</div></div>';
                html += '</div></div>';
                $("#list-chat-message").append(html);
                $('.lightgallery').lightGallery({
                    thumbnail: true,
                    animateThumb: false,
                    showThumbByDefault: false
                }); 
            }
            else {
                let htmlChild = '';
                if (item.StrDateTime == strDateRight) {
                    htmlChild = '<span>' + item.Content + '</span>';
                    $('.message-right:last-child').find('.item-text').append(htmlChild);
                }
                else {
                    html += '<div class="chat-message-item message-right clearfix">';
                    html += '<div class="message-item-box row">';
                    html += '<div class="message-item-name">' + item.FromFullName + '</div>';
                    html += '<div class="message-item-text col-xs-11" id="lightgallery">';
                    html += '<div class="item-text"><span>' + item.Content + '</span></div></div>';
                    let date = formatDate(item.CreatedDate);
                    html += '<div class="message-item-img col-xs-1">';
                    html += '<img src="' + item.FromAvatar + '" class="img-circle" width="40" height="40" />';
                    html += '<div class="message-item-time">' + date + '</div></div>';
                    html += '</div></div>';
                    $("#list-chat-message").append(html);
                }
            }

            strDateRight = item.StrDateTime;
        }
        else {
            if (item.IsFile == true) {
                html += '<div class="chat-message-item chat-message-img-left message-left clearfix">';
                html += '<div class="message-item-box row">';
                html += '<div class="message-item-img col-xs-1">';
                html += '<img src="' + item.FromAvatar + '" class="img-circle" width="40" height="40" />';
                let date = formatDate(item.CreatedDate);
                html += '<div class="message-item-time">' + date + '</div></div>';
                html += '<div class="message-item-text col-xs-11 lightgallery">';
                let arr = getArrayImage(item.Content);
                $.map(arr, function (value) {
                    html += '<a href="' + value + '" class="item-image"><img src=' + value + ' width="100"/>';
                    html += '</a>';
                });
                html += '</div></div></div>';
                $("#list-chat-message").append(html);
                $('.lightgallery').lightGallery({
                    thumbnail: true,
                    animateThumb: false,
                    showThumbByDefault: false
                });
            }
            else {
                let htmlChild = '';
                if (item.StrDateTime == strDateLeft) {
                    htmlChild = '<span>' + item.Content + '</span>';
                    $('.message-left:last-child').find('.item-text').append(htmlChild);
                }
                else {
                    html += '<div class="chat-message-item message-left clearfix">';
                    html += '<div class="message-item-box row">';
                    html += '<div class="message-item-name">' + item.FromFullName + '</div>';
                    html += '<div class="message-item-img col-xs-1">';
                    html += '<img src="' + item.FromAvatar + '" class="img-circle" width="40" height="40" />';
                    let date = formatDate(item.CreatedDate);
                    html += '<div class="message-item-time">' + date + '</div></div>';
                    html += '<div class="message-item-text col-xs-11">';
                    html += '<div class="item-text"><span>' + item.Content + '</span></div>';
                    html += '</div></div></div>';
                    $("#list-chat-message").append(html);
                }
            }
            strDateLeft = item.StrDateTime;
        }
    });
    $("#list-chat-message").scrollTop($("#list-chat-message")[0].scrollHeight);
}

// Thông báo tin nhắn bằng Notification
hub.client.notificationMessage = function (fullname, message) {
    if (window.Notification) {
        Notification.requestPermission(function (permission) {
            if (permission === "granted") {
                var notification = new Notification("ChatOnline", {
                    body: fullname + ": " + message,
                    icon: "/Content/images/Group-icon.png"
                });

                setTimeout(notification.close.bind(notification), 3000);
            }
        });
    }
}

// Hàm hiển thị tổng số tin nhắn
hub.client.countNewMessage = function (count) {
    if (parseInt(count) > 0) {
        $("#top-new-count").html('<i class="fa fa-envelope-o"></i><span data-total="' + count + '" class="label label-info" >' + count + '</span>');
    }
    else {
        $("#top-new-count").html('<i class="fa fa-envelope-o"></i><span data-total="0" class="label label-info" >0</span>');
    }
}

//Hàm đặt lại số tin nhắn chưa đọc
hub.client.upCountMessage = function (userId, count, total) {
    $('#count-new-' + userId).html(count);
    $('#count-new-' + userId).attr("data-count", count);
    $('#top-new-count').html('<i class="fa fa-envelope-o"></i><span data-total="' + total + '" class="label label-info" >' + total + '</span>');
}

// Bật thông báo 2 user đang tương tác
hub.client.userInteractive = function (userId, isAdd) {
    if (isAdd) {
        $('#Interactive').val(userId);
    }
    else {
        $('#Interactive').val('');
    }
}

// Thêm tổng in nhắn cho group
hub.client.addCountMessagePrivate = function (userId) {
    // Đặt lại tổng tin nhắn trên Header
    let total = $('#top-new-count span').html();
    total = parseInt(total) + 1;
    $('#top-new-count span').html(total);

    // Đặt lại số tin nhắn trên cho user
    let count = parseInt($('#count-new-' + userId).html());
    count = count + 1;
    $('#count-new-' + userId).html(count)
    $('#count-new-' + userId).addClass('message-valid');
}

// Trừ tổng số tin nhắn chưa đọc cho user khi user đọc tin nhắn
hub.client.devCountMessagePrivate = function (userId, count) {
    // Đặt lại tổng tin nhắn trên Header
    let total = $('#top-new-count span').html();
    total = parseInt(total) - count;
    $('#top-new-count span').html(total);

    // Đặt lại số tin nhắn trên cho user
    $('#count-new-' + userId).html('0');
    $('#count-new-' + userId).removeClass('message-valid');
}


// -------- GROUP CHAT ----------

// Thêm mới nhóm thành công
hub.client.createGroupSuccess = function (groupName, groupId) {
    let html = '';
    html += '<div class="group-item" id="group-item-' + groupId + '" data-name="' + groupName + '" data-id="' + groupId + '">';
    html += '<img src="/Content/images/Group-icon.png" width="50" height="50" class="img-circle" />';
    html += '<div class="name-group-box"><span>' + groupName + '</span>';
    html += '<div class="group-dots"><span></span><span></span><span></span></div>';
    html += '<div class="right-click">';
    html += '<a href="#" data-id="' + groupId + '" data-name="' + groupName + '" class="right-click-item view-member">Thành viên</a>';
    html += '<a href="#" data-id="' + groupId + '" data-name="' + groupName + '" class="right-click-item out-group">Rời khỏi nhóm</a>';
    html += '<a href="#" data-id="' + groupId + '" data-name="' + groupName + '" class="right-click-item remove-group">Xóa nhóm</a>';
    html += '<a href="#" class="right-close"><i class="fa fa-close"></i></a>';
    html += '</div></div></div>';
    $('#list-group').append(html);
}

// Hiển thị toàn bộ nhóm mà user là thành viên
hub.client.allGroup = function (groups, total) {
    if (total > 0) {
        notification("Bạn có " + total + " thông báo mới chưa đọc!");
    }
    // Đặt lại tổng số tin nhắn (Header)
    $('#header-message-group span').html(total);

    let html = '';
    if (groups.length > 0) {
        $.map(groups, function (item) {
            html += '<div class="group-item" id="group-item-' + item.ID + '" data-name="' + item.Name + '" data-id="' + item.ID + '">';
            html += '<img src="/Content/images/Group-icon.png" width="50" height="50" class="img-circle" />';
            html += '<div class="name-group-box"><span>' + item.Name + '</span>';
            html += '<div class="group-dots"><span></span><span></span><span></span></div>';
            html += '<div class="right-click">';
            html += '<a href="#" data-id="' + item.ID + '" data-name="' + item.Name + '" class="right-click-item view-member">Thành viên</a>';
            html += '<a href="#" data-id="' + item.ID + '" data-name="' + item.Name + '" class="right-click-item out-group">Rời khỏi nhóm</a>';
            html += '<a href="#" data-id="' + item.ID + '" data-name="' + item.Name + '" class="right-click-item remove-group">Xóa nhóm</a>';
            html += '<a href="#" class="right-close"><i class="fa fa-close"></i></a>';
            html += '</div></div>';
            if (item.Count == 0) {
                html += '<div class="count-message">0</div></div>';
            }
            else {
                html += '<div class="count-message count-message-valid">' + item.Count + '</div></div>';
            }
        });
    }
    else {
        html += '<div class="group-item"><div class="alert alert-warning">Bạn chưa tham gia bất cứ nhóm nào</div></div>';
    }
    $('#list-group').html(html);
}

// Hiển thị tin nhắn của 1 nhóm
hub.client.allGroupMessage = function (messages, userId) {
    let strDateLeft = '';
    let strDateRight = '';
    $.map(messages, function (item) {
        let html = '';
        let childMessage = '';
        if (item.FromID != userId) {
            if (item.IsFile == true) {
                html += '<div class="chat-message-item chat-message-img-left message-left clearfix" data-id="' + item.FromID + '">';
                html += '<div class="message-item-box row">';
                html += '<div class="message-item-img col-xs-1">';
                html += '<img src="' + item.FromAvatar + '" class="img-circle" width="40" height="40" />';
                let date = formatDate(item.CreatedDate);
                html += '<div class="message-item-time">' + date + '</div></div>';
                html += '<div class="message-item-text col-xs-11 lightgallery">';
                let arr = getArrayImage(item.Content);
                $.map(arr, function (value) {
                    html += '<a href="' + value + '" class="item-image"><img src=' + value + ' width="100"/>';
                    html += '</a>';
                });
                html += '</div></div></div>';
                $("#group-chat-message").append(html);
                $('.lightgallery').lightGallery({
                    thumbnail: true,
                    animateThumb: false,
                    showThumbByDefault: false
                });
            }
            else {
                let lastLeft = $('.message-left:last-child');
                let leftId = lastLeft.data('id');
                if (item.StrDateTime == strDateLeft && item.FromID == leftId) {
                    childMessage = '<span>' + item.Content + '</span>';
                    lastLeft.find('.item-text').append(childMessage);
                }
                else {
                    html += '<div class="chat-message-item message-left clearfix" data-id="' + item.FromID + '">';
                    html += '<div class="message-item-box row">';
                    html += '<div class="message-item-name">' + item.FromFullName + '</div>';
                    html += '<div class="message-item-img col-xs-1">';
                    html += '<img src="' + item.FromAvatar + '" class="img-circle" width="40" height="40" />';
                    let date = formatDate(item.CreatedDate);
                    html += '<div class="message-item-time">' + date + '</div></div>';
                    html += '<div class="message-item-text col-xs-11">';
                    html += '<div class="item-text"><span>' + item.Content + '</span></div>';
                    html += '</div></div></div>';
                    $("#group-chat-message").append(html);
                }
            }
            strDateLeft = item.StrDateTime;
        }
        else {
            if (item.IsFile == true) {
                html += '<div class="chat-message-item chat-message-img-right message-right clearfix" data-id="' + item.FromID + '">';
                html += '<div class="message-item-box row">';
                html += '<div class="message-item-text col-xs-11 lightgallery">';
                let arr = getArrayImage(item.Content);
                $.map(arr, function (value) {
                    html += '<a href="' + value + '" class="item-image"><img src=' + value + ' width="100"/>';
                    html += '</a>';
                });
                html += '</div>';
                let date = formatDate(item.CreatedDate);
                html += '<div class="message-item-img col-xs-1">';
                html += '<img src="' + item.FromAvatar + '" class="img-circle" width="40" height="40" />';
                html += '<div class="message-item-time">' + date + '</div></div>';
                html += '</div></div>';
                $("#group-chat-message").append(html);
                $('.lightgallery').lightGallery({
                    thumbnail: true,
                    animateThumb: false,
                    showThumbByDefault: false
                });
            }
            else {
                let lastRight = $('.message-right:last-child');
                let rightId = lastRight.data('id');

                if (item.StrDateTime == strDateRight && item.FromID == rightId) {
                    childMessage = childMessage = '<span>' + item.Content + '</span>';
                    lastRight.find('.item-text').append(childMessage);
                }
                else {
                    html += '<div class="chat-message-item message-right clearfix" data-id="' + item.FromID + '">';
                    html += '<div class="message-item-box row">';
                    html += '<div class="message-item-name">' + item.FromFullName + '</div>';
                    html += '<div class="message-item-text col-xs-11">';
                    html += '<div class="item-text"><span>' + item.Content + '</span></div></div>';
                    let date = formatDate(item.CreatedDate);
                    html += '<div class="message-item-img col-xs-1">';
                    html += '<img src="' + item.FromAvatar + '" class="img-circle" width="40" height="40" />';
                    html += '<div class="message-item-time">' + date + '</div></div>';
                    html += '</div></div>';
                    $("#group-chat-message").append(html);
                }
            }
            
            strDateRight = item.StrDateTime;
        }
        
    });
    $("#group-chat-message").scrollTop($("#group-chat-message")[0].scrollHeight);
}

// Thêm mới tin nhắn vào group thành công
hub.client.groupMessage = function (message, isMe) {
    let html = '';
    if (isMe != true) {
        if (message.IsFile == true) {
            html += '<div class="chat-message-item chat-message-img-left message-left clearfix" data-id="' + message.FromID + '">';
            html += '<div class="message-item-box row">';
            html += '<div class="message-item-img col-xs-1">';
            html += '<img src="' + message.FromAvatar + '" class="img-circle" width="40" height="40" />';
            let date = formatDate(message.CreatedDate);
            html += '<div class="message-item-time">' + date + '</div></div>';
            html += '<div class="message-item-text col-xs-11 lightgallery">';
            let arr = getArrayImage(message.Content);
            $.map(arr, function (value) {
                html += '<a href="' + value + '" class="item-image"><img src=' + value + ' width="100"/>';
                html += '</a>';
            });
            html += '</div></div></div>';
            $("#group-chat-message").append(html);
            $('.lightgallery').lightGallery({
                thumbnail: true,
                animateThumb: false,
                showThumbByDefault: false
            });
        }
        else {
            let item = $('.message-left:last-child');
            let strDate = item.find('.message-item-time').data('time');
            let leftId = item.data('id');
            if (message.StrDateTime == strDate && message.FromID == leftId) {
                let htmlChild = '<span>' + message.Content + '</span>';
                item.find('.item-text').append(htmlChild);
            }
            else {
                html += '<div class="chat-message-item message-left clearfix" data-id="' + message.FromID + '">';
                html += '<div class="message-item-box row">';
                html += '<div class="message-item-name">' + message.FromFullName + '</div>';
                html += '<div class="message-item-img col-xs-1">';
                html += '<img src="' + message.FromAvatar + '" class="img-circle" width="40" height="40" />';
                let date = formatDate(message.CreatedDate);
                html += '<div class="message-item-time" data-time="' + message.StrDateTime + '">' + date + '</div></div>';
                html += '<div class="message-item-text col-xs-11">';
                html += '<div class="item-text"><span>' + message.Content + '</span></div>';
                html += '</div></div></div>';
                $("#group-chat-message").append(html);
            }
        }
    }
    else {
        if (message.IsFile == true) {
            html += '<div class="chat-message-item chat-message-img-right message-right clearfix" data-id="' + message.FromID + '">';
            html += '<div class="message-item-box row">';
            html += '<div class="message-item-text col-xs-11 lightgallery">';
            let arr = getArrayImage(message.Content);
            $.map(arr, function (value) {
                html += '<a href="' + value + '" class="item-image"><img src=' + value + ' width="100"/>';
                html += '</a>';
            });
            html += '</div>';
            let date = formatDate(message.CreatedDate);
            html += '<div class="message-item-img col-xs-1">';
            html += '<img src="' + message.FromAvatar + '" class="img-circle" width="40" height="40" />';
            html += '<div class="message-item-time" data-time="' + message.StrDateTime + '">' + date + '</div></div>';
            html += '</div></div>';
            $("#group-chat-message").append(html);
            $('.lightgallery').lightGallery({
                thumbnail: true,
                animateThumb: false,
                showThumbByDefault: false
            });
        }
        else {
            let item = $('.message-right:last-child');
            let strDate = item.find('.message-item-time').data('time');
            let rightId = item.data('id');

            if (message.StrDateTime == strDate && message.FromID == rightId) {
                let htmlChild = '<span>' + message.Content + '</span>';
                item.find('.item-text').append(htmlChild);
            }
            else {
                html += '<div class="chat-message-item message-right clearfix" data-id="' + message.FromID + '">';
                html += '<div class="message-item-box row">';
                html += '<div class="message-item-name">' + message.FromFullName + '</div>';
                html += '<div class="message-item-text col-xs-11">';
                html += '<div class="item-text"><span>' + message.Content + '</span></div></div>';
                let date = formatDate(message.CreatedDate);
                html += '<div class="message-item-img col-xs-1">';
                html += '<img src="' + message.FromAvatar + '" class="img-circle" width="40" height="40" />';
                html += '<div class="message-item-time" data-time="' + message.StrDateTime + '">' + date + '</div></div>';
                html += '</div></div>';
                $("#group-chat-message").append(html);
            }
        }
    }
    $("#group-chat-message").scrollTop($("#group-chat-message")[0].scrollHeight);
}

// Hiển thị danh sách tat ca user de them tao nhom moi
hub.client.getAllUserForGroup = function (users) {
    let html = '';
    if (users.length <= 0) {
        html += '<div style="padding:5px 18px;background:#f2f2f2;border-radius: 15px;"><span>Tất cả thành viên đã được thêm vào nhóm!</span></div>';
    }
    else {
        $.map(users, function (item, index) {
            html += '<div class="user-check-item"><label><span class="stt">' + (index + 1) + '.  </span>';
            html += '<input type="checkbox" data-id="' + item.UserID + '" data-name="' + item.UserName + '" class="minimal"><span class="fullname">' + item.FullName + '</span></label>';
            html += '<p>' + item.UserName + '</p></div>';
        });
    }
    $('#update-group-user').hide();
    $('#save-group-user').show();
    $('#modal-group .name-group').show();
    $('#modal-group .modal-title').html('Tạo nhóm');
    $('#modal-group .list-user-group').html(html);
    $('#modal-group .list-user-group input').iCheck({
        checkboxClass: 'icheckbox_square-blue'
    });
    $('#modal-group').modal('show');
}

// Hiển thị danh sách user của nhóm
hub.client.listUserInGroup = function (users) {
    let html = '';
    $.map(users, function (item, index) {
        if (item.IsChecked == true) {
            html += '<div class="user-check-item"><label><span class="stt">' + (index + 1) + '.  </span>';
            html += '<input type="checkbox" class="check-box" checked data-id="' + item.UserID + '" class="minimal"><span class="fullname">' + item.FullName + '</span></label>';
            html += '<p>' + item.UserName + '</p>';
            html += '<a href="#" class="remove-user-group" id="user-group-' + item.UserID +'" data-id="' + item.UserID + '"><i class="fa fa-trash"></i></a></div>';
        }
        else {
            html += '<div class="user-check-item"><label><span class="stt">' + (index + 1) + '.  </span>';
            html += '<input type="checkbox" data-id="' + item.UserID + '" data-name="' + item.UserName + '" class="minimal"><span class="fullname">' + item.FullName + '</span></label>';
            html += '<p>' + item.UserName + '</p></div>';
        }
    });

    $('#update-group-user').show();
    $('#save-group-user').hide();
    $('#modal-group .list-user-group').html(html);
    $('#modal-group .list-user-group input').iCheck({
        checkboxClass: 'icheckbox_square-blue'
    });
    $('#modal-group').modal('show');
}

//Xóa nhóm thành công/ Rời khỏi nhóm thành công
hub.client.successOutGroup = function (groupId, message) {
    $('#list-group .group-item').each(function () {
        let thisItem = $(this);
        let id = thisItem.data("id");
        if (id == groupId) {
            thisItem.remove();
        }
    });
    $("#success").val(message);
    showToastr();
}

// Thêm tổng in nhắn cho group
hub.client.addCountMessageGroup = function (groupId) {
    // Đặt lại tổng tin nhắn trên Header
    let total = $('#header-message-group span').html();
    total = parseInt(total) + 1;
    $('#header-message-group span').html(total);

    // Đặt lại số tin nhắn trên cho nhóm
    let item = $('#group-item-' + groupId).find('.count-message');
    let count = parseInt(item.html());
    count = count + 1;
    item.html(count);
    item.addClass('count-message-valid');
}

// Trừ tổng số tin nhắn chưa đọc cho group khi user đọc tin nhắn
hub.client.devCountMessageGroup = function (groupId, count) {
    // Đặt lại tổng tin nhắn trên Header
    let total = $('#header-message-group span').html();
    total = parseInt(total) - count;
    $('#header-message-group span').html(total);

    // Đặt lại số tin nhắn trên cho nhóm
    let item = $('#group-item-' + groupId).find('.count-message');
    item.html('0');
    item.removeClass('count-message-valid');
}


// Hiển thị thông báo lỗi
hub.client.alertMessage = function (message, type) {
    if (type) {
        $("#success").val(message);
    }
    else {
        $("#error").val(message);
    }
    showToastr();
}


//---------- XỬ LÝ SỰ KIỆN CLIEND --------------------

$.connection.hub.start().done(function () {

    $("#input-chat").on("click", "#btn-send", function () {
        let chatId = $('#chat-id').val();
        if (chatId.length <= 0) {
            $("#error").val('Vui lọng chọn đối tượng bạn muốn trò chuyện!');
            return showToastr();
        }
        let message = $('#input-send').val();
        let groupName = $('#chat-group').val();
        if (groupName.length > 0) {
            let groupId = $('#chat-id').val();
            if (groupId.length > 0 && message.length > 0) {
                hub.server.addMessageToGroup(groupId, groupName, message);
            }
        }
        else {
            let userId = $('#chat-id').val();
            let total = $('#top-new-count span').data('total');
            let count = $('#count-new-' + userId).data('count');

            if (!count) count = 0;
            if (userId.length > 0 && message.length > 0) {
                hub.server.addMessagePrivate(userId, message);
            }
        }
        $("#input-send").val('').focus();
    });

    $('#input-chat').keypress(function (e) {
        let chatId = $('#chat-id').val();
        if (chatId.length <= 0) {
            $("#error").val('Vui lọng chọn đối tượng bạn muốn trò chuyện!');
            return showToastr();
        }
        if (e.which == 13) {
            let message = $('#input-send').val();
            let groupName = $('#chat-group').val();
            if (groupName.length > 0) {
                let groupId = $('#chat-id').val();
                if (groupId.length > 0 && message.length > 0) {
                    hub.server.addMessageToGroup(groupId, groupName, message);
                }
            }
            else {
                let userId = $('#chat-id').val();
                let total = $('#top-new-count span').data('total');
                let count = $('#count-new-' + userId).data('count');

                if (!count) count = 0;
                if (userId.length > 0 && message.length > 0) {
                    hub.server.addMessagePrivate(userId, message);
                }
            }
            $("#input-send").val('').focus();
        }
    });

    //--------- COMMON ----------------

    // Khi người dùng click vào tab tin  nhắn
    $('#tab-message').on('click', function () {
        hub.server.loadAllMessage();
    });

    // Khi người dùng click vào icon thông báo tin nhắn mới
    $('#top-new-count').on('click', function () {
        $('#tab-message').tab('show');
        $('#main-sidebar').addClass("main-sidebar-show");
        hub.server.loadAllMessage();
    });

    // Khi người dùng click vào tab danh bạ
    $('#tab-contact').on('click', function () {
        hub.server.loadAllContact();
    });

    // Khi người dùng click vào tab nhóm
    $('#tab-group').on('click', function () {
        hub.server.loadAllGroup();
    });

    // Khi người dùng click vào icon thông báo tin nhắn mới từ group
    $('#header-message-group').on('click', function () {
        $('.contact-item').removeClass('active');
        $('.message-item').removeClass('active');
        $('.group-item').removeClass('active');
        $('#tab-group').tab('show');
        $('#main-sidebar').addClass("main-sidebar-show");
        hub.server.loadAllGroup();
    });

    //--------- PRIVATE CHAT ----------------

    $('#contact').on('click', 'a', function () {
        $('#main-sidebar').removeClass("main-sidebar-show");
        $('#list-chat-message').removeClass('list-hide');
        $('#group-chat-message').addClass('list-hide');

        let userId = $(this).data('user');

        $('#chat-group').val('');
        $('#chat-id').val(userId);

        $("#list-chat-message").html('');
        hub.server.getAllMessagePrivate(userId);

        let fullname = $(this).data('fullname');
        let avatar = $(this).data('avatar');
        $("#display-user").html(fullname);
        $('#display-img').attr('src', avatar);
    });

    $('#message').on('click', 'a', function () {
        $('#main-sidebar').removeClass("main-sidebar-show");
        $('#list-chat-message').removeClass('list-hide');
        $('#group-chat-message').addClass('list-hide');
        $('#main-right-bar').removeClass('active');

        let userId = $(this).data('user');

        $('#chat-group').val('');
        $('#chat-id').val(userId);

        $("#list-chat-message").html('');
        hub.server.getAllMessagePrivate(userId);

        let fullname = $(this).data('fullname');
        let avatar = $(this).data('avatar');
        $("#display-user").html(fullname);
        $('#display-img img').attr('src', avatar);
    });

    //--------- GROUP CHAT ----------------
    //$(".btn-create-group").on("click", function () {
    //    let groupName = $('.txt-name-group').val();
    //    if (groupName.length > 0) {
    //        hub.server.createGroup(groupName);
    //        $('.txt-name-group').val('').focus();
    //    }
    //});

    // Lấy tin nhắn của 1 nhóm
    $("#list-group").on("click", ".group-item", function () {
        $('.message-item').removeClass('active');
        $('.contact-item').removeClass('active');
        $('.group-item').removeClass('active');
        $('#list-chat-message').addClass('list-hide');
        $('#group-chat-message').removeClass('list-hide');
        $(this).addClass('active');
        $('#main-sidebar').removeClass("main-sidebar-show");

        let id = $(this).data("id");
        let name = $(this).data("name");

        $("#chat-id").val(id);
        $("#chat-group").val(name);
        $("#display-user").html(name);
        $("#group-chat-message").html('');

        hub.server.getAllGroupMessage(id);
    });

    // Mở modal thêm nhóm mới
    $('#add-group').on('click', '.btn-create-group', function () {
        let id = $('#chat-id').val();
        hub.server.getUserForGroup();
    });

    // Lấy ra tất cả các user là thành viên của nhóm
    $('#list-group').on('click', '.view-member', function () {
        let groupId = $(this).data('id');
        let groupName = $(this).data('name');
        hub.server.getListUserInGroup(groupId);
        $('#modal-group .name-group').hide();
        $('#modal-group .modal-title').html('<span data-name="' + groupName + '" data-id="' + groupId + '">' + groupName + '</span>');
    });

    // Tạo nhóm mới
    $('#save-group-user').click(function () {
        let groupName = $('#modal-group .name-group input').val();
        let listChecked = getChecked();
        hub.server.addUserGroup(listChecked, groupName);
    });

    // Update thành viên nhóm
    $('#update-group-user').click(function () {
        let groupName = $('#modal-group .modal-title span').data('name');
        let listChecked = getChecked();
        hub.server.updateUserGroup(listChecked, groupName,[]);
    });

    //Xóa thành viên khỏi nhóm
    $('.list-user-group').on('click', '.remove-user-group', function (e) {
        e.preventDefault();
        let groupId = $("#modal-group .modal-title span").data('id');
        let groupName = $("#modal-group .modal-title span").data('name');
        let userId = $(this).data("id");
        hub.server.removeUserFromGroup(userId, groupId, groupName);
    });

    //Rời khỏi nhóm
    $('#list-group').on('click', '.out-group', function () {
        let groupId = $(this).data('id');
        let groupName = $(this).data('name');
        hub.server.outFromGroup(groupId, groupName);
    });

    // Xóa nhóm
    $('#list-group').on('click', '.remove-group', function () {
        let groupId = $(this).data('id');
        let groupName = $(this).data('name');
        hub.server.removeGroup(groupId, groupName);
    });
}); 

function getChecked() {
    let userChecked = [];
    $('#modal-group .user-check-item .minimal').each(function () {
        if ($(this).is(':checked')) {
            let name = $(this).data('name');
            userChecked.push(name);
        }
    });
    return userChecked;
}

function formatDate(input) {
    var date = new Date(input);
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    //return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + "  " + strTime;
    return strTime;
}

// Thông báo Desktop
function notification(message) {
    if (window.Notification) {
        Notification.requestPermission(function (permission) {
            if (permission === "granted") {
                var notification = new Notification("Chat Online", {
                    body: message,
                    icon: "/Content/images/Group-icon.png"
                });

                setTimeout(notification.close.bind(notification), 5000);
            }
        });
    }
}

function getArrayImage(strImage) {
    let arr = [];
    arr = strImage.split(',');
    return arr;
}