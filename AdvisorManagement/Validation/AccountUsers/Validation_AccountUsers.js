$(document).ready(function () {

    $('#user_name').on('input', function () {
        var regex = new RegExp("^[aAàÀảẢãÃáÁạẠăĂằẰẳẲẵẴắẮặẶâÂầẦẩẨẫẪấẤậẬbBcCdDđĐeEèÈẻẺẽẼéÉẹẸêÊềỀểỂễỄếẾệỆfFgGhHiIìÌỉỈĩĨíÍịỊjJkKlLmMnNoOòÒỏỎõÕóÓọỌôÔồỒổỔỗỖốỐộỘơƠờỜởỞỡỠớỚợỢpPqQrRsStTuUùÙủỦũŨúÚụỤưƯừỪửỬữỮứỨựỰvVwWxXyYỳỲỷỶỹỸýÝỵỴzZ0-9 ]*$");
        if ($(this).val() == "") {
            $('#valid_username').text("Họ và tên không được bỏ trống").show();
        } else {
            $('#valid_username').hide();
            if (regex.test($(this).val())) {
                $('#valid_username').hide();

            }
            else {
                $('#valid_username').text("Tên có chứa ký tự đặc biệt").show();

            }
        }
    });
    $("#address").keyup(function () {
        var regex = new RegExp("^[aAàÀảẢãÃáÁạẠăĂằẰẳẲẵẴắẮặẶâÂầẦẩẨẫẪấẤậẬbBcCdDđĐeEèÈẻẺẽẼéÉẹẸêÊềỀểỂễỄếẾệỆfFgGhHiIìÌỉỈĩĨíÍịỊjJkKlLmMnNoOòÒỏỎõÕóÓọỌôÔồỒổỔỗỖốỐộỘơƠờỜởỞỡỠớỚợỢpPqQrRsStTuUùÙủỦũŨúÚụỤưƯừỪửỬữỮứỨựỰvVwWxXyYỳỲỷỶỹỸýÝỵỴzZ0-9/. ]*$");
        let addressValue = $("#address").val();
        if (addressValue == "") {
            
            
            $('#valid_address').text("Đia chỉ không được bỏ trống").show(); //.show() khi nó đang ẩn thì dùng

        }
        else {
            $('#valid_address').hide();
            if (regex.test(addressValue)) {
                $('#valid_address').hide();

            }
            else {
                $('#valid_address').text("Địa chỉ có chứa ký tự đặc biệt").show();

            }
        }
    });
    $('#user_code').on('input', function () {
        var regex = new RegExp("^[a-zA-Z0-9 -]*$");
        if ($(this).val() == "") {
            $('#valid_usercode').text("Mã không được bỏ trống").show();
        } else {
            $('#valid_usercode').hide();
            if (regex.test($(this).val())) {
                $('#valid_usercode').hide();

            }
            else {
                $('#valid_usercode').text("Mã có chứa ký tự đặc biệt").show();

            }
        }
    });
    $('#gender').on('input', function () {
        var regex = new RegExp("^[aAàÀảẢãÃáÁạẠăĂằẰẳẲẵẴắẮặẶâÂầẦẩẨẫẪấẤậẬbBcCdDđĐeEèÈẻẺẽẼéÉẹẸêÊềỀểỂễỄếẾệỆfFgGhHiIìÌỉỈĩĨíÍịỊjJkKlLmMnNoOòÒỏỎõÕóÓọỌôÔồỒổỔỗỖốỐộỘơƠờỜởỞỡỠớỚợỢpPqQrRsStTuUùÙủỦũŨúÚụỤưƯừỪửỬữỮứỨựỰvVwWxXyYỳỲỷỶỹỸýÝỵỴzZ]*$");
        if ($(this).val() == "") {
            $('#valid_gender').text("Giới tính không được bỏ trống").show();
        } else {
            $('#valid_gender').hide();
            if (regex.test($(this).val())) {
                $('#valid_gender').hide();

            }
            else {
                $('#valid_gender').text("Vui lòng ghi đúng giới tính").show();

            }
        }
    });







    const email = document.getElementById("email");
    email.addEventListener("blur", () => {
        let regex = /^([_\-\.0-9a-zA-Z]+)@([_\-\.0-9a-zA-Z]+)\.([a-zA-Z]){2,7}$/;
        let s = email.value;
        if (regex.test(s)) {
            email.classList.remove("is-invalid");
            $('#valid_email').hide();
        } else {
            email.classList.add("is-invalid");
            $('#valid_email').text("Email không đúng").show();
        }
    });
});