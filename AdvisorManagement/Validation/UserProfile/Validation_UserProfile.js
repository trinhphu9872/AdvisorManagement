$(document).ready(function () {
    $("#user_name").keyup(function () {
        let usernameValue = $("#user_name").val();
        if (usernameValue.length =="") {
            user_nameError = false;
            //Nếu dùng thẻ <p> để làm câu thông báo bên dưới thì:
            $('#valid_name').text("Họ và tên không được bỏ trống").show(); //.show() khi nó đang ẩn thì dùng
            
        }
        else {
            $('#valid_name').hide();
            user_nameError = true;
        }
    });
    $("#user_code").keyup(function () {
        let usernameValue = $("#user_code").val();
        if (usernameValue.length == "") {

            //Nếu dùng thẻ <p> để làm câu thông báo bên dưới thì:
            $('#valid_code').text("Mã số không được bỏ trống").show(); //.show() khi nó đang ẩn thì dùng
            user_codeError = false;
        }
        else {
            $('#valid_code').hide();
            user_codeError = true;
        }
        
    });
    //$("#phone").keyup(function () {
    //    var a = document.getElementById("#phone").val();
    //    let filter = /^[0-9-+]+$/;

    //    if (filter.test(a)) {
    //        $('#valid2_phone').text("Số điện thoại phải là số").show();
    //    } else {

    //        $('#valid2_phone').text("Số điện thoại phải là số").show();
    //    }

    //});
    $("#phone").keyup(function () {
        let phoneValue = $("#phone").val();
        if (phoneValue.length <= 9 || phoneValue.length >10 ) {
            //Nếu dùng thẻ <p> để làm câu thông báo bên dưới thì:
            phoneError = false;
            $('#valid_phone').text("Số điện thoại phải 10 số").show(); //.show() khi nó đang ẩn thì dùng
        }
        else {
            $('#valid_phone').hide();
            phoneError = true;
        }
        let filter = /^[0-9-+]+$/;

        if (filter.test(phoneValue)) {
            $('#valid2_phone').hide();
            phoneError = true;
        } else if (phoneValue == "") {
            $('#valid2_phone').hide();
            phoneError = true;
        }
        else {
            $('#valid2_phone').text("Số điện thoại phải là số").show();
            phoneError = false;
        }
    });
 
    const email = document.getElementById("email");
    email.addEventListener("blur", () => {
        let regex = /^([_\-\.0-9a-zA-Z]+)@([_\-\.0-9a-zA-Z]+)\.([a-zA-Z]){2,7}$/;
        let s = email.value;
        if (regex.test(s)) {
            email.classList.remove("is-invalid");
            emailError = true;
        } else {
            email.classList.add("is-invalid");
            emailError = false;
        }
    });
   
});