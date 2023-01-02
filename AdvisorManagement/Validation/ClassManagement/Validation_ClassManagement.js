$(document).ready(function () {
    $("#class_code").keyup(function () {
        let usernameValue = $("#class_code").val();
        if (usernameValue.length == "") {
            user_nameError = false;
            //Nếu dùng thẻ <p> để làm câu thông báo bên dưới thì:
            $('#valid_VLuclass').text("Mã lớp không được bỏ trống").show(); //.show() khi nó đang ẩn thì dùng

        }
        else {
            $('#valid_VLuclass').hide();
            user_nameError = true;
        }
    });
    $("#semester_name").keyup(function () {
        let usernameValue = $("#semester_name").val();
        if (usernameValue.length == "") {
            user_nameError = false;
            //Nếu dùng thẻ <p> để làm câu thông báo bên dưới thì:
            $('#valid_semester').text("Học kỳ không được bỏ trống").show(); //.show() khi nó đang ẩn thì dùng

        }
        else {
            $('#valid_semester').hide();
            user_nameError = true;
        }
    });


});