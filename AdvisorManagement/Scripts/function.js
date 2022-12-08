jQuery.fn.showLoading = function (msg) { msg = msg || ''; jQuery(this).html('<img src="/Content/images/loader2.gif"> ' + msg).show(); }
jQuery.fn.hideLoading = function () { jQuery(this).hide(); }

function redirectTo(location) {
    window.location = location;
}
//Show Alert
function ShowAlert(title, messages) {
    $("#myAlert").modal();
    $("#h4Alert").html(title);
    $("#divContentAlert").html(messages);
}
//Xu ly ky tu ""
function string(html) {
    html = html.replace('"', '');
    html = html.replace('"', '');
    return html;
}
//Hiển thị tin nhắn 
function Show(id) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Home/Messages/" + id);
    
}

//function Show(id) {
//    $("#myModal").modal();
//    $("#MessagesContent").load("/Home/Messages/" + id);
//}
//Hien thi foem cap nhat thong tin ca nhan
function ShowFormStudentInfo() {
    $("#myModal").modal({ backdrop: 'static', keyboard: false });
    $("#MessagesContent").load("/Home/UpdateStudentInfo/");
}

// Cập nhật sinh viên cựu sinh viên
function CapNhatThongTinCuuSinhVien() {
    $("#myModal").modal();
    $("#MessagesContent").load("/Home/CapNhatCuuSinhVien/");
}

//function ShowFormStudentInfo() {
//    var adr = '/Home/UpdateStudentInfo'+ '&t=' + Math.random();
//    $.ajax({
//        type: 'GET',
//        url: adr,
//        async: true,
//        dataType: 'html',
//        success: function (html) {
//            //$("#divThoiKhoiBieu").html(html);
//        },
//    })
//.fail(
//    function (jqXHR, textStatus, err) {
//        ShowAlert("Lỗi kết nối", err);
//    });
//}

//Cap nhật thông tin cá nhân
function UpdateStudentInfo() {
    //check valid value
    if ($("#txtNoiSinh").val().trim() == "0" || $("#txtNoiSinh").val().trim() == "" || $("#txtNoiSinh").val().trim() == undefined) {
        Swal.fire("Vui lòng nhập dữ liệu", "Nơi sinh chưa được chọn!", "warning")
        return;
    }
    if ($("#ddlDanToc").val() == "" || $("#ddlDanToc").val() == undefined) {
        Swal.fire("Vui lòng nhập dữ liệu", "Dân tộc chưa được chọn!", "warning")
        return;
    }
    if ($("#txtDiDong").val() == "" || $("#txtDiDong").val() == undefined) {
        Swal.fire("Vui lòng nhập dữ liệu", "Di động chưa được nhập!", "warning")
        return;
    }
    if ($("#txtCMND").val() == "" || $("#txtCMND").val() == undefined) {
        Swal.fire("Vui lòng nhập dữ liệu", "Chứng minh nhân dân chưa được nhập!", "warning")
        return;
    }
    //--
    if ($("#txtHoTenNguoiLH").val().trim() == "" || $("#txtHoTenNguoiLH").val() == undefined) {
        Swal.fire("Vui lòng nhập dữ liệu", "Họ tên người liên hệ chưa được nhập!", "warning")
        return;
    }
    if ($("#txtDienThoaiNguoiLH").val().trim() == "" || $("#txtDienThoaiNguoiLH").val() == undefined) {
        Swal.fire("Vui lòng nhập dữ liệu", "Điện thoại người liên hệ chưa được nhập!", "warning")
        return;
    }
    if ($("#txtDiaChiNguoiLH").val().trim() == "" || $("#txtDiaChiNguoiLH").val() == undefined) {
        Swal.fire("Vui lòng nhập dữ liệu", "Địa chỉ người liên hệ chưa được nhập!", "warning")
        return;
    }
    //
    var para = new Object();
    para.HoLot = $('#txtHoLot').val();
    para.Ten = $('#txtTen').val();
    para.NgaySinh = $('#txtNgaySinh').val();
    //
    para.NoiSinh = $('#txtNoiSinh option:selected').text();
    para.NoiSinhID = $('#txtNoiSinh').val();

    para.DanToc = $('#ddlDanToc').val();
    para.TonGiao = $('#ddlTonGiao').val();
    para.Email = $('#txtMail').val();
    para.OtherEmail = $('#txtEmailSV').val();
    para.DienThoaiBan = $('#txtDienThoai').val();
    para.DiDong = $('#txtDiDong').val();
    para.CMND = $('#txtCMND').val();
    //para.NoiCapCMND = $('#txtNoiCap').val();
    para.NgayCapCMND = $('#txtNgayCap').val();
    para.NoiCapCMND = $('#txtNoiCap option:selected').text();
    para.NoiCapCMNDID = $('#txtNoiCap').val();

    para.QuocGiaThuongTru = $('#ddlQuocGiaThuongTru').val();
    para.TinhThanhThuongTru = $('#ddlTinhThanhThuongTru').val();
    para.QuanHuyenThuongTru = $('#ddlQuanHuyenThuongTru').val();
    //para.PhuongXaThuongTru = $('#txtPhuongXaThuongTru').val();
    para.PhuongXaThuongTru = $('#ddlPhuongXaThuongTru option:selected').text();
    para.PhuongXaThuongTruID = $('#ddlPhuongXaThuongTru').val();
    para.SoNhaThuongTru = $('#txtSoNhaThuongTru').val();
    para.DiaChiLienLac = $('#txtDiaChiLienLac').val();

    para.MatCha = $("#chkMatCha").is(":checked");
    para.HoTenCha = $('#txtHoTenCha').val();
    para.NgaySinhCha = $('#txtNgaySinhCha').val();
    para.NgheNghiepCha = $('#txtNgheNghiepCha').val();
    para.DiDongCha = $('#txtDiDongCha').val();
    //para.TonGiaoCha = $('#ddlTonGiaoCha').val();
    //para.DanTocCha = $('#ddlDanTocCha').val();
    //para.QuocGiaCha = $('#ddlQuocGiaCha').val();
    para.EmailCha = $('#txtEmailCha').val();
    para.HoKhauThuongTruCha = $('#txtHoKhauThuongTruCha').val();

    para.MatMe = $("#chkMatMe").is(":checked");
    para.HoTenMe = $('#txtHoTenMe').val();
    para.NgaySinhMe = $('#txtNgaySinhMe').val();
    para.NgheNghiepMe = $('#txtNgheNghiepMe').val();
    para.DiDongMe = $('#txtDiDongMe').val();
    //para.TonGiaoMe = $('#ddlTonGiaoMe').val();
    //para.DanTocMe = $('#ddlDanTocMe').val();
    //para.QuocGiaMe = $('#ddlQuocGiaMe').val();
    para.EmailMe = $('#txtEmailMe').val();
    para.HoKhauThuongTruMe = $('#txtHoKhauThuongTruMe').val();

    para.HoTenNguoiGiamHo = $('#txtHoTenNguoiGiamHo').val();
    para.NamSinhNguoiGiamHo = $('#txtNamSinhNguoiGiamHo').val();
    para.NgheNghiepNguoiGiamHo = $('#txtNgheNghiepNguoiGiamHo').val();

    para.HoTenNguoiLH = $('#txtHoTenNguoiLH').val();
    para.DienThoaiNguoiLH = $('#txtDienThoaiNguoiLH').val();
    para.DiaChiNguoiLH = $('#txtDiaChiNguoiLH').val();

    $("#btnLuuThongTin").showLoading("Loading ...");
    $.ajax({
        type: 'POST',
        url: '/API/StudentApi/SaveStudentInfo' + "?t=" + Math.random(),
        async: true,
        data: para,
        dataType: 'html',
        success: function (html) {
            html = html.replace('"', '');
            html = html.replace('"', '');
            var kq = html.indexOf("...");
            if (kq > -1) {
                //lu thanh cong dong va reload
                $("#myModal").modal("hide");
                location.reload();
            }
            else {
                //$("#myModal").modal("hide");
                alert('Lưu thông tin không thành công.');
            }
            $("#btnLuuThongTin").html("Cập nhật");
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#divAlert").html(err);
        $("#btnLuuThongTin").html("Cập nhật");
    });
}

// UPdate cựu sinh viên
function UpdateCuuSinhVien() {
    alert(1);
    var txtSDT = $('#txtSDT').val();
    var txtFaceBook = $('#txtFaceBook').val();
    var txtNoiCongTac = $('#txtNoiCongTac').val();
    var txtDiaChi = $('#txtDiaChi').val();

    $.ajax({
        type: 'POST',
        url: '/Home/UpdateCuuSinhVien' + '?SDT=' + txtSDT + '&FaceBook=' + txtFaceBook + '&NoiCongTac=' + txtNoiCongTac + '&DiaChi=' + txtDiaChi + '&t=' + Math.random(),
        async: true,
        data: para,
        dataType: 'html',
        success: function (html) {
            html = html.replace('"', '');
            html = html.replace('"', '');
            var kq = html.indexOf("...");
            if (kq > -1) {
                //lu thanh cong dong va reload
                $("#myModal").modal("hide");
                location.reload();
            }
            else {
                $("#myModal").modal("hide");
                ShowAlert("Thông báo", html)
            }
            $("#btnLuuThongTin").html("Cập nhật");
        },
    })  
    .fail(
    function (jqXHR, textStatus, err) {
        $("#divAlert").html(err);
        $("#btnLuuThongTin").html("Cập nhật");
    });
}

//Hien thi form doi mat khau
function ShowFormStudentPw() {
    $("#MessagesContent").html("Loading ....");
    $("#myModal").modal();
    $.ajax({
        type: 'GET',
        url: "/Home/ChangPasswordStudent",
        async: false,
        dataType: 'html',
        success: function (html) {
            $("#MessagesContent").html(html);
        },
    })
   .fail(
   function (jqXHR, textStatus, err) {
       $("#divAlert").html(err);
   });
}
//Doi mat khau
function UpdateStudentPW() {
    var pw = $('#txtPW').val();
    var pw1 = $('#txtPW1').val();
    var pw2 = $('#txtPW2').val();
    if (pw == '' || pw1 == "" || pw2 == "") {
        ShowAlert("Chú ý ", "Vui lòng nhập hết thông tin trước khi lưu");
        return;
    }
    if (pw1 != pw2) {
        ShowAlert("Chú ý ", "Mật khẩu nhập lại không chính xác");
        return;
    }

    var parapw = new Object();
    parapw.ID = "";
    parapw.txtPW = pw;
    parapw.txtPW1 = pw1;
    parapw.txtPW2 = pw2;
    $("#btnUpdate").showLoading("...");
    $.ajax({
        type: 'GET',
        url: '/API/Student/auther' + "?t=" + Math.random(),
        async: true,
        data: { pw: pw, pw1: pw1, pw2: pw2 },
        dataType: 'html',
        success: function (html) {
            html = string(html);
            var kq = html.indexOf("...");
            if (kq > -1)
            { $("#myModal").modal("hide"); }
            ShowAlert("Thông báo", html);
            $("#btnUpdate").html("Cập nhật");
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#btnUpdate").hideLoading();
        $("#btnUpdate").html("Cập nhật");
        ShowAlert("Thông báo", err);
    });
}

//Xem CTDT
function StudentStudyProgram() {
    $("#divStudyProgams").showLoading("Loading ...");
    var ID = $('#ddlStudyProgams').val();
    $.ajax({
        type: 'GET',
        url: '/API/Student/StudyPgrograms/' + ID + "?t=" + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divStudyProgams").html(string(html));
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err);
    });
}
// ve thoi khoa bieu
function DrawingSchedules() {
    var YearStudy = $('#YearStudy').val();
    var TermID = $('#TermID').val();
    var Week = $('#Week').val();
    var typeID = $('#TypeID').val();
    var adr = '/Home/DrawingSchedules?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Week=' + Week + '&t=' + Math.random();
    if (typeID == "1")
        adr = '/Home/DrawingSchedules2?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Week=' + Week + '&t=' + Math.random();
    //$("#divThoiKhoiBieu").html("<b style='color:red'>Đang tải dử liệu .....</b>");
    $("#divThoiKhoiBieu").showLoading('Loading ...');

    $.ajax({
        type: 'GET',
        url: adr,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divThoiKhoiBieu").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Lỗi kết nối", err);
    });
}
// ve thoi khoa bieu theo lop
function DrawingClassStudentSchedules() {
    var YearStudy = $('#YearStudy').val();
    var TermID = $('#TermID').val();
    var Week = $('#Week').val();
    var ClassStudentID = $('#ClassStudentID').val();
    var typeID = $('#TypeID').val();
    var adr = '/Home/DrawingClassStudentSchedules_Mau2?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Week=' + Week + '&ClassStudentID=' + ClassStudentID + '&t=' + Math.random();
    if (typeID == "1")
        adr = '/Home/DrawingClassStudentSchedules_Mau2?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Week=' + Week + '&ClassStudentID=' + ClassStudentID + '&t=' + Math.random();
    //$("#divThoiKhoiBieu").html("<b style='color:red'>Đang tải dử liệu .....</b>");
    $("#divThoiKhoiBieu").showLoading('Loading ...');

    $.ajax({
        type: 'GET',
        url: adr,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divThoiKhoiBieu").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        //ShowAlert("Lỗi kết nối", err);
    });
}
// ve thoi khoa bieu theo giang vien
function DrawingProfessorSchedules() {
    var YearStudy = $('#YearStudy2').val();
    var TermID = $('#TermID2').val();
    var Week = $('#Week2').val();
    var ProfessorID = $('#ProfessorID').val();
    var typeID = $('#TypeID').val();
    var adr = '/Home/DrawingProfessorSchedule?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Week=' + Week + '&ProfessorID=' + ProfessorID + '&t=' + Math.random();
    if (typeID == "1")
        adr = '/Home/DrawingProfessorSchedule?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Week=' + Week + '&ProfessorID=' + ProfessorID + '&t=' + Math.random();

    $.ajax({
        type: 'GET',
        url: adr,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divThoiKhoiBieu").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Lỗi kết nối", err);
    });
}

// ve thoi khoa bieu theo sinh vien
function DrawingStudentSchedules(studentId, yearId, termId, weekId) {
    // Tai' code 17062016
    var adr = '/Home/DrawingStudentSchedule?StudentId=' + studentId + '&YearId=' + yearId + '&TermId=' + termId + '&WeekId=' + weekId + '&t=' + Math.random();
    $.ajax({
        type: 'GET',
        url: adr,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divThoiKhoiBieu").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Lỗi kết nối", err);
    });
}

//Lay lop sinh vien theo hoc ky 
function LoadClassStudentbyTerm() {
    var NamHoc = $('#YearStudy').val();
    var HocKy = $('#TermID').val();
    // Send an AJAX request
    $.getJSON("/Home/GetClassStudentByTerm/" + NamHoc + "$" + HocKy)
        .done(function (data) {
            $("#ClassStudentID").empty();

            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.ClassStudentID + ">" + item.ClassStudentName + " </option>").appendTo($('#ClassStudentID'));
            });
            var CurrentClassStudentID = data[0].StudentClassStudentID;

            $("#ClassStudentID").val(CurrentClassStudentID)
        });
}
//Lay lop sinh vien theo hoc ky 
function GetProfessorByTerm() {
    var NamHoc = $('#YearStudy2').val();
    var HocKy = $('#TermID2').val();
    // Send an AJAX request
    $.getJSON("/Home/GetProfessorByTerm/" + NamHoc + "$" + HocKy)
        .done(function (data) {
            $("#ProfessorID").empty();

            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.ProfessorID + ">" + item.ProfessorName + " </option>").appendTo($('#ProfessorID'));
            });
        });
}
//Lay tuan 
function LoadWeek() {
    var NamHoc = $('#YearStudy').val();
    var HocKy = $('#TermID').val();
    // Send an AJAX request
    $.getJSON("/Home/GetWeek/" + NamHoc + "$" + HocKy)
        .done(function (data) {
            $("#Week").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.Week + ">" + item.DisPlayWeek + " </option>").appendTo($('#Week'));
            });
            $("#Week").val(data[0].WeekOfYear);

            var cbTweekDisplay = document.getElementById('cbTweek').style.display;
            if (cbTweekDisplay == 'block' || cbTweekDisplay == '') {
                DrawingSchedules();
            }
            else {
                DrawingSchedules2();
            }            
        });
}


//Lay tuan tkb lop
function LoadWeekClassStudent() {
    var NamHoc = $('#YearStudy').val();
    var HocKy = $('#TermID').val();
    // Send an AJAX request
    $.getJSON("/Home/GetWeek/" + NamHoc + "$" + HocKy)
        .done(function (data) {
            $("#Week").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.Week + ">" + item.DisPlayWeek + " </option>").appendTo($('#Week'));
            });
            $("#Week").val(data[0].WeekOfYear);

            DrawingClassStudentSchedules();
        });
}


//Lay tuan tkb gv
function LoadWeekProfessors() {
    var NamHoc = $('#YearStudy2').val();
    var HocKy = $('#TermID2').val();
    // Send an AJAX request
    $.getJSON("/Home/GetWeek/" + NamHoc + "$" + HocKy)
        .done(function (data) {
            $("#Week2").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.Week + ">" + item.DisPlayWeek + " </option>").appendTo($('#Week2'));
            });
            $("#Week2").val(data[0].WeekOfYear);

            DrawingProfessorSchedules();
        });
}

//Diem 
function LoadMarkOption() {
    var isCheck = $("input:radio[name=Marks]:checked").val();
    if (isCheck == 0) {
        ShowMarks();
    }
    else {
        XemDiemTheoChuongTrinhDaoTao();
    }
}

//xem diem
function ShowMarks() {
    var StudyProgram = $('#ddlStudyProgram').val();
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();
    var HeDiem = $('#ddlHeDiem').val();
    //$("#divHienthiKQHT").html("<b>Đang tải dữ liệu .....</b>");
    $("#divHienthiKQHT").showLoading('Loading ...');
    $.ajax({
        type: 'GET',
        url: '/Home/ShowMark?StudyProgram=' + StudyProgram + '&YearStudy=' + YearStudy + '&TermID=' + TermID + '&HeDiem=' + HeDiem + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHienthiKQHT").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}


//Xem diem theo CTDT
function XemDiemTheoChuongTrinhDaoTao() {
    $("#divHienthiKQHT").showLoading("Loading ...");
    var StudyProgram = $('#ddlStudyProgram').val();

    $.ajax({
        type: 'GET',
        url: '/Home/XemDiemTheoChuongTrinhDaoTao?StudyProgram=' + StudyProgram + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHienthiKQHT").html(html);
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err);
    });
}

//Show mark detail
function ShowMarkDetail(curid, Stid) {
    var url = "";
    if (Stid == "") {
        url = curid + '?t=' + Math.random();
    }
    else {
        url = curid + "?StudentID=" + Stid + '&t=' + Math.random()
    }
    $.ajax({
        type: 'GET',
        url: '/Home/ShowMarkDetail/' + url,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}
//Show mark detail
function ShowMarkDetailLoai(curid, Stid) {
    var url = "";
    if (Stid == "") {
        url = curid + '?t=' + Math.random();
    }
    else {
        url = curid + "?StudentID=" + Stid + '&t=' + Math.random()
    }
    $.ajax({
        type: 'GET',
        url: '/Home/ShowMarkDetailToType/' + url,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
        .fail(
            function (jqXHR, textStatus, err) {
                ShowAlert("Thông báo", err)
            });
}
//Show mon hoc thao luan
function ShowCurriculumComment() {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();
    $.ajax({
        type: 'GET',
        url: '/Home/ShowCurriculums' + '?YearStudy=' + YearStudy + '&TermID=' + TermID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHienthiykien").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

//Show thao luan
function ShowComment(SchID) {
    $.ajax({
        type: 'GET',
        url: '/Home/ShowComment?SchID=' + SchID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

//Dang thao luan
function InsertComment(SchID) {
    var txtNoiDung = $("#txtnoidung").val();

    $.post("/API/Student/InsertComment", { SCHID: SchID, Con: txtNoiDung }, function (result) {
        var html = result.replace('"', '');
        html = result.replace('"', '');
        var kq = result.indexOf("...");
        if (kq > -1) {
            ShowComment(SchID);
        }
        else {
            ShowAlert("Thông báo", html);
        }
    });
}

//Show hoc phi
function HienThiPhiHocPhan() {
    //var YearStudy = $('#ddlYearStudy').val();
    //var TermID = $('#ddlTermID').val();
    $("#divHocPhiHocPhan").showLoading("...");
    $.ajax({
        type: 'GET',
        url: '/Home/HienThiPhiHocPhan',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHocPhiHocPhan").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

function HienThiPhiKyTucXa() {
    $("#divHocPhiKyTucXa").showLoading("...");
    $.ajax({
        type: 'GET',
        url: '/Home/HienThiPhiKyTucXa',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHocPhiKyTucXa").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

//Show lich thi
function ShowExam() {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();
    $("#divLichThi").showLoading("Loading ...");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowExam' + '?YearStudy=' + YearStudy + '&TermID=' + TermID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divLichThi").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}
//Update professor ìno
function UpdateProfessorInfo() {
    var para = new Object();
    para.txtDienThoai = $('#txtDienThoai').val();
    para.txtDiDong = $('#txtDiDong').val();
    para.txtEmail = $('#txtEmail').val();
    para.txtDiaChiLienLac = $('#txtDiaChiLienLac').val();

    $.ajax({
        type: 'POST',
        url: '/API/ProfessorApi/UpdateProfessorInfo' + "?t=" + Math.random(),
        async: true,
        data: para,
        dataType: 'html',
        success: function (html) {
            html = html.replace('"', '');
            html = html.replace('"', '');
            var kq = html.indexOf("...");
            if (kq > -1)
            { $("#myModal").modal("hide"); }
            location.reload();
            //ShowAlert("Thông báo", html)
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#divAlert").html(err);
    });
}
//Hien thi foem cap nhat thong tin ca nhan
function ShowFormProfessorInfo() {
    $("#myModal").modal();
    $("#MessagesContent").load("/Professor/UpdateInfo/");
}
//Doi mat khau
function UpdateProfessorPW() {
    var pw = $('#txtPW').val();
    var pw1 = $('#txtPW1').val();
    var pw2 = $('#txtPW2').val();
    if (pw == '' || pw1 == "" || pw2 == "") {
        ShowAlert("Chú ý ", "Vui lòng nhập hết thông tin trước khi lưu");
        return;
    }
    if (pw1 != pw2) {
        ShowAlert("Chú ý ", "Mật khẩu nhập lại không chính xác");
        return;
    }

    var parapw = new Object();
    parapw.ID = "";
    parapw.txtPW = pw;
    parapw.txtPW1 = pw1;
    parapw.txtPW2 = pw2;

    $("#btnUpdate").showLoading("");
    $.ajax({
        type: 'GET',
        url: '/API/Professor/auther' + "?t=" + Math.random(),
        async: true,
        data: { pw: pw, pw1: pw1, pw2: pw2 },
        dataType: 'html',
        success: function (html) {
            html = string(html);
            var kq = html.indexOf("...");
            if (kq > -1)
            { $("#myModal").modal("hide"); }
            ShowAlert("Thông báo", html);
            $("#btnUpdate").html("Cập nhật");
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#btnUpdate").html("Cập nhật");
        ShowAlert("Thông báo", err);
    });
}
//Hien thi thoi khoa bieu tong quat giang vien
function ShowTBKChung() {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();
    var Type = $('#ddlType').val();
    var wk = $('#ddlWeek').val();
    //if (Type == "0")
    //{ $('#ddlWeek').attr("disabled", "disabled"); }
    //else
    //{ $('#ddlWeek').removeAttr("disabled"); }
    $("#divHienthi").showLoading("Loading ...");
    $.ajax({
        type: 'GET',
        url: '/Professor/ShowSchedules?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Type=' + Type + '&Week=' + wk + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHienthi").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", textStatus)
    });
}
// Thoi khoa bieu giang vien theo tuan
function ShowTKBTuanGiangVien() {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();
    var wk = $('#ddlWeek').val();
    $("#divHienthi").showLoading("Loading ...");
    $.ajax({
        type: 'GET',
        url: '/Professor/DrawingSchedulesByTerms?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Week=' + wk + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHienthi").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", textStatus)
    });
}
//Load tuan 
function LoadWeek_2() {
    var NamHoc = $('#ddlYearStudy').val();
    var HocKy = $('#ddlTermID').val();
    var i = 0;

    // Send an AJAX request
    $.getJSON("/Professor/GetTuanTheoNamHocHocKy/" + NamHoc + "$" + HocKy)
        .done(function (data) {
            $("#ddlWeek").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.Week + ">" + item.DisPlayWeek + " </option>").appendTo($('#ddlWeek'));
            });
            $("#ddlWeek").val(data[0].WeekOfYear);
            ShowTBKChung();
        });
}
//Danh sach dang ky giang vien
function ShowRegists() {
    var YearStudy = $('#YearStudy').val();
    var TermID = $('#TermID').val();
    //$("#divHienthi").html("<img src='/content/images/ajax-loader.gif' /><b>Đang tải dữ liệu .....</b>");
    $.ajax({
        type: 'GET',
        url: '/Professor/ShowRegists' + '?YearStudy=' + YearStudy + '&TermID=' + TermID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHienthi").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

//Load NQ
function LoadNCN() {
    var id = $("#ddlNhomQuyen").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowChucNangTheoNhomQuyen/' + id,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html("Lỗi kết nối");
        ShowAlert("Thông báo", err)
    });
}

//Load CN
function LoadCN() {
    var id = $("#ddlChonNhom").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowChucNang/' + id,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html("Lỗi kết nối");
        ShowAlert("Thông báo", err)
    });
}
//
function SaveAuthor() {
    $("#idSave").html("Đang lưu ...");
    $.ajax({
        url: '/Staff/SaveAuthor',
        dataType: 'html',
        type: 'POST',
        data: $('form').serialize() + '&ajax=1',
        success: function (data) {
            $("#idSave").html("Cập Nhật");
            ShowAlert("Thông báo", data);
        }
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#idSave").html("Cập Nhật");
        ShowAlert("Thông báo", err)
    });

}
//
function showValues() {
    var str = $("form").serialize();
    $("#results").text(str);
}
//
function ShowStudentSearch() {
    var id = $("#txtKeyWord").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'POST',
        url: '/Staff/ShowSearchStudent',
        data: { 'StudentID': id },
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html("Lỗi kết nối");
        ShowAlert("Thông báo", err)
    });
}
//
function ShowMarks_2(id) {
    var StudyProgram = $('#ddlStudyProgram').val();
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();
    var HeDiem = $('#ddlHeDiem').val();
    $("#divHienthiKQHT").showLoading('Loading ...');
    $.ajax({
        type: 'GET',
        url: '/Home/ShowMark?StudyProgram=' + StudyProgram + '&StudentID=' + id + '&YearStudy=' + YearStudy + '&TermID=' + TermID + '&HeDiem=' + HeDiem + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHienthiKQHT").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}
//
function LoadCourse() {
    var G = $('#ddlGradualevel').val();
    var S = $('#ddlStudyTypes').val();
    // Send an AJAX request
    $.getJSON("/Staff/GetCourse/" + G + "$" + S)
        .done(function (data) {
            $("#ddlCourse").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.CourseID + ">" + item.CourseName + " </option>").appendTo($('#ddlCourse'));
                LoadClassStudent();
            });
        });
}
//
function LoadClassStudent() {
    var D = $('#ddlDepartment').val();
    var C = $('#ddlCourse').val();
    // Send an AJAX request
    $.getJSON("/Staff/GetClassStudent/" + C + "$" + D)
        .done(function (data) {
            $("#ddlClassStudent").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.ClassStudentID + ">" + item.ClassStudentName + " </option>").appendTo($('#ddlClassStudent'));
            });
        });
}
//
function ShowMarkSChedules() {
    var id = $("#ddlClassStudent").val();
    var stid = $("#txtStudentID").val().trim();
    if (stid != '')
        id = stid;
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowMarkSChedules/' + id,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html("Lỗi kết nối");
        ShowAlert("Thông báo", err)
    });
}
//
function SetLink1(stid) {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();

    var link = "/Public/StudentSchedules?SID=" + stid + '&Y=' + YearStudy + '&T=' + TermID;
    $("a.linkschedule_" + stid).attr("href", link);
}
//
function SetLink2(stid) {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();

    var link = '/Staff/ShowStudentExam?SID=' + stid + '&Y=' + YearStudy + '&T=' + TermID;
    $("a.linkexam_" + stid).attr("href", link);
}
//

function ShowProfessor(type) {
    var id = $("#ddlDepartment").val();
    var pid = $("#txtProfessorID").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowProfessors/?DPID=' + id + '&PID=' + pid + '&TID=' + type,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html("Lỗi kết nối");
        ShowAlert("Thông báo", err)
    });
}

// show professer cấu hình password
function ShowProfessor(type) {
    var id = $("#ddlDepartment").val();
    var pid = $("#txtProfessorID").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowProfessors?DPID=' + id + '&PID=' + pid + '&TID=' + type,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
        .fail(
            function (jqXHR, textStatus, err) {
                $("#divKetQua").html("Lỗi kết nối");
                ShowAlert("Thông báo", err)
            });
}
////Lay Co so 
function LoadBuilding() {
    var id = $('#ddlCampus').val();
    // Send an AJAX request
    $.getJSON("/Public/GetBuilding/" + id)
        .done(function (data) {
            $("#ddlBuilding").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.BuildingID + ">" + item.BuildingName + " </option>").appendTo($('#ddlBuilding'));
            });

        });
}
//Show phong trong

function ShowRoomsAvailable() {
    var campusid = $("#ddlCampus").val();
    var Buildingid = $("#ddlBuilding").val().trim();
    var date = $("#datetimepicker1").val();
    var PeriodID = $("#txtTietBD").val();
    var NumberOfPeriods = $("#txtSoTiet").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowRoomsAvailable?campusid=' + campusid + '&Buildingid=' + Buildingid + '&date=' + date + '&PeriodID=' + PeriodID + '&NumberOfPeriods=' + NumberOfPeriods + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}

//lich phong

function ShowRoomSchedules() {
    var RoomID = $("#ddlRooms").val();
    var date1 = $("#datetimepicker1").val();
    var date2 = $("#datetimepicker2").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowRoomSchedules?RoomID=' + RoomID + '&Date1=' + date1 + '&Date2=' + date2 + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//
function ShowProfessorTotalPeriods() {
    var yearstudy = $("#ddlYearStudy").val();
    var termid = $("#ddlTermID").val();
    var Department = $("#ddlDepartment").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowProfessorTotalPeriods?YearStudy=' + yearstudy + '&TermID=' + termid + '&DepartmentID=' + Department + '&Order=' + '1' + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
// set cookie
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function Processcookie() {
    var idkey = getCookie("menukey");
    $("." + idkey).removeClass("collapse");
    $("#" + idkey).addClass("in");
    $("#" + idkey).attr("style", "height: auto")
}
//Load danh sach sinh vien co van

function ShowStudentClassStudent() {
    var ClassStudentID = $("#ddlClassStudent").val();
    var ddlYearStudy = $("#ddlYearStudy").val();
    var ddlTermID = $("#ddlTermID").val();


    $("#divDanhSachSinhVien").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/AjaxStudentClassStudent?ClassStudentID=' + ClassStudentID + '&YearStudy=' + ddlYearStudy + '&TermID=' + ddlTermID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divDanhSachSinhVien").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divDanhSachSinhVien").html(err);
        ShowAlert("Thông báo", err)
    });
}
//show lich trung sinh vien
function ShowLichTrungSinhVien() {
    var ClassStudentID = $("#ddlClassStudent").val();
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    $("#divLichTrungSinhVien").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/LichTrungSinhVien?classStudent=' + ClassStudentID + '&TermID=' + TermID + '&YearStudy=' + YearStudy + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divLichTrungSinhVien").html(string(html));
        },
    })
        .fail(
            function (jqXHR, textStatus, err) {
                $("#divLichTrungSinhVien").html(err);
                ShowAlert("Thông báo", err)
            });
}
function LoadDanhSachLichSV() {
    var Week = $("#dllTuan").val(); 
    var DayofWeek = $("#dllThu").val();
    $("#divLichTrungSV").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/ShowLichTrungTheoTuan?Week=' + Week + '&DayofWeek=' + DayofWeek +'&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divLichTrungSV").html(string(html));
        },
    })
        .fail(
            function (jqXHR, textStatus, err) {
                $("#divLichTrungSV").html(err);
                ShowAlert("Thông báo", err)
            });
}

//
function ShowStudentClassStudentMarks() {
    var ClassStudentID = $("#ddlClassStudent").val();

    var ddlYearStudy = $("#ddlYearStudy").val();
    var ddlTermID = $("#ddlTermID").val();

    $("#divKetQuaHocTap").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/AjaxStudentClassStudentMarks?ClassStudentID=' + ClassStudentID + '&TermID=' + ddlTermID + '&YearStudy=' + ddlYearStudy + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQuaHocTap").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQuaHocTap").html(err);
        ShowAlert("Thông báo", err)
    });
}
//thong ke diem cvht
function ShowStudentClassStudentMarkStatisc() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    var ClassStudentID = $("#ddlClassStudent").val();

    $("#divThongKeKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/AjaxStudentClassStudentMarkStatisc?TermID=' + TermID + '&YearStudy=' + YearStudy + '&ClassStudentID=' + ClassStudentID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divThongKeKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divThongKeKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//
////Lay lop theo CVHT
function LoadClassStudentByProfessor() {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $("#ddlTermID").val();
    // Send an AJAX request
    $.getJSON("/Professor/AjaxClassStudent?YearStudy=" + YearStudy + '&TermID=' + TermID)
        .done(function (data) {
            $("#ddlClassStudent").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.ClassStudentID + ">" + item.ClassStudentName + " </option>").appendTo($('#ddlClassStudent'));
            });

            LoadFunciton();
        });
}

////Lay lop theo NV 
function LoadClassStudentByStaff() {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $("#ddlTermID").val();
    // Send an AJAX request
    $.getJSON("/Staff/AjaxClassStudent?YearStudy=" + YearStudy + '&TermID=' + TermID)
        .done(function (data) {
            $("#ddlClassStudent").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.ClassStudentID + ">" + item.ClassStudentName + " </option>").appendTo($('#ddlClassStudent'));
            });

        });
}

//CapNhat nhom tin
function ShowCapNhatNhomTin(id) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Staff/CapNhatNhomTin/" + id);
}
//
function CapNhatNhomTin() {
    var MaNhomTin = $('#txtMaNhomTin').val();
    var TenNhomTin = $('#txtTenNhom').val();
    var ThuTu = $('#txtThuTuHienThi').val();
    var Flag = $('#txtFlag').val();
    var HienThi = $("#chkHienThi").is(":checked") ? 1 : 0;

    $.ajax({
        type: 'GET',
        url: '/Staff/LuuNhomTin?MaNhomTin=' + MaNhomTin + '&TenNhomTin=' + TenNhomTin + '&ThuTu=' + ThuTu + '&HienThi=' + HienThi + '&Flag=' + Flag + "&t=" + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            ShowAlert("Thông báo", html)
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#alertid").html(err);
    });
}
// xoa nhom tin 
function XoaNhomTin(MaNhomTin) {
    if (!confirm("Xác nhận xóa ?")) {
        return false;
    }
    $.ajax({
        type: 'GET',
        url: '/Staff/LuuNhomTin?MaNhomTin=' + MaNhomTin + '&TenNhomTin=' + 0 + '&ThuTu=' + 0 + '&HienThi=' + 0 + '&Flag=' + 2 + "&t=" + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            ShowAlert("Thông báo", html)
            window.location.reload();
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#alertid").html(err);
    });
}
//SHowTinTuc
function ShowTinTuc(page) {
    var NhomTin = $("#ddlNhomTin").val();
    var MaTin = 0;
    //var page = 1;

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowTinTuc?MaNhom=' + NhomTin + '&MaTin=' + MaTin + '&page=' + page + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}

//xoa tin tuc
function XoaTin(MaTin) {
    if (!confirm("Xác nhận xóa ?")) {
        return false;
    }
    $.ajax({
        type: 'GET',
        url: '/Staff/XoaTin?MaTin=' + MaTin + "&t=" + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            ShowAlert("Thông báo", html)
            ShowTinTuc();
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err);
    });
}

//Cap nhat menu
function ShowCapNhatMenu(id) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Staff/CapNhatMenus/" + id);
}
//luu cap nhat menu
function CapNhatMenus() {
    var MaMenu = $('#txtMaMenu').val();
    var TenMenu = $('#txtTenMenu').val();
    var Url = $('#txtUrl').val();
    var ThuTu = $('#txtThuTuHienThi').val();
    var HienThi = $("#chkHienThi").is(":checked") ? 1 : 0;// $('#chkHienThi').val();
    $.ajax({
        type: 'GET',
        url: '/Staff/LuuMenus?MaMenu=' + MaMenu + '&TenMenu=' + TenMenu + '&ThuTu=' + ThuTu + '&HienThi=' + HienThi + '&Url=' + Url + "&t=" + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            ShowAlert("Thông báo", html)
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#alertid").html(err);
    });
}

//xóa menu
function XoaMenu(Mamenu) {
    if (!confirm("Xác nhận xóa ?")) {
        return false;
    }
    $.ajax({
        type: 'GET',
        url: '/Staff/XoaMenus?MaMenu=' + Mamenu + "&t=" + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            ShowAlert("Thông báo", html)
            window.location.reload();
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

//Hien viet
function StudentStatisticByOlogy() {
    var id = $("#ddlGradualevel").val();
    var stid = $("#ddlStudyTypes").val().trim();
    var sttt = $("#ddlTinhTrang").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStudentStaticByOlogy/' + id + '-' + stid + '?std=' + sttt,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
function ShowProfessorStatisticByDepartment() {
    $("#divSoLuong").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowProfessorStatisticByDepartment',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divSoLuong").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divSoLuong").html(err);
        ShowAlert("Thông báo", err)
    });
}
function ShowProfessorStatisticAcademicDegrees() {
    $("#divHocVi").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowProfessorStatisticAcademicDegrees',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHocVi").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divHocVi").html(err);
        ShowAlert("Thông báo", err)
    });
}
function ShowStatisticAcademicTitles() {
    $("#divHocHam").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStatisticAcademicTitles',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHocHam").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divHocHam").html(err);
        ShowAlert("Thông báo", err)
    });
}
function DepartmentsManagingSelCurriculum() {
    var id = $("#ddlGradualevel").val();
    var stid = $("#ddlDepartment").val().trim();
    var sttt = $("#ddlCurriculumGroup").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowDepartmentsManagingSelCurriculum?G=' + id + '&D=' + stid + '&C=' + sttt,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
function SearchCurriculums() {
    var id = $("#txttim").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowSearchCurriculums?str=' + id,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
function SearchStudyProgram() {
    var id = $("#txtTim").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowSearchStudyProgram?strFiter=' + id,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
function CourseSel() {
    var id = $("#ddlCourse").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowCourseSel?CID=' + id,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}

function ShowDetailCurriculums(id) {

    $.ajax({
        type: 'GET',
        url: '/Staff/ShowDetailCurriculums?S=' + id,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}
function LoadStudyProgram() {
    var id = $('#ddlCourse').val();
    // Send an AJAX request
    $.getJSON("/Staff/GetStudyProgram/" + id)
        .done(function (data) {
            $("#ddlStudyProgram").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.StudyProgramID + ">" + item.StudyProgramName + " </option>").appendTo($('#ddlStudyProgram'));
            });

        });
}
function StudyProgramSel() {
    var st = $("#ddlStudyProgram").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStudyProgramSel?StudyProgramID=' + st,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}

function LoadBuildingByCourseID() {
    var id = $('#ddlCampus').val();
    // Send an AJAX request
    $.getJSON("/Staff/GetBuilding/" + id)
        .done(function (data) {
            $("#ddlBuilding").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.BuildingID + ">" + item.BuildingName + " </option>").appendTo($('#ddlBuilding'));
            });

        });
}
function StudentBehaviorScoreTotal() {
    var Year = $("#ddlYearStudy").val();
    var Term = $("#ddlTermID").val().trim();
    var GraduateLevel = $("#ddlGraduateLevel").val();
    var StudyType = $("#ddlStudyType").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStudentBehaviorScoreTotal?Y=' + Year + '&T=' + Term + '&G=' + GraduateLevel + '&S=' + StudyType,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
function ShowGetCampus() {
    var cid = $('#ddlCampus').val();
    var bid = $('#ddlBuilding').val();

    $("#divKetQua").showLoading('Loading ...');
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowGetCampus?c=' + cid + '&b=' + bid,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}
function StudentAverageScoresByTerms() {
    var Year = $("#ddlYearStudy").val();
    var Term = $("#ddlTermID").val().trim();
    var GraduateLevel = $("#ddlGraduateLevel").val();
    var StudyType = $("#ddlStudyType").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStudentAverageScoresByTerms?Y=' + Year + '&T=' + Term + '&G=' + GraduateLevel + '&S=' + StudyType,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
function RoomsTotalPeriods() {
    var Year = $("#ddlYearStudy").val();
    var Term = $("#ddlTermID").val().trim();
    var Campus = $("#ddlCampus").val();
    var Building = $("#ddlBuilding").val();
    var Order = $("#ddlOrder").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowRoomsTotalPeriods?Y=' + Year + '&T=' + Term + '&C=' + Campus + '&B=' + Building + '&O=' + Order,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Thong ke nhap hoc
function StudentAttendedEnrollsbyNganh() {
    var Course = $("#ddlCourse").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStudentAttendedEnrollsbyNganh?C=' + Course,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Thống kê nhập học theo khối
function StudentAttendedEnrollsbyKhoi() {
    var Course = $("#ddlCourse").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStudentAttendedEnrollsbyKhoi?K=' + Course,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Thống kê nhập học theo khối ngành
function StudentAttendedEnrollsbyKhoiNganh() {
    var Course = $("#ddlCourse").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStudentAttendedEnrollsbyKhoiNganh?KN=' + Course,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Thống kê tốt nghiệp
function GraduatesStatistics() {
    var Year = $("#ddlYearStudy").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowGraduatesStatistics?Y=' + Year,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Thống kê biến động
function Studentmissingschool() {
    var Year = $("#ddlYearStudy").val();
    var Course = $("#ddlCourse").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStudentmissingschool?Y=' + Year + '&C=' + Course,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Danh sách kỷ luật
function ListStudentDiscipline() {
    var Year = $("#ddlYearStudy").val();
    var Course = $("#ddlCourse").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowListStudentDiscipline?Y=' + Year + '&C=' + Course,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Thống kê diện chính sách
function StatisticsPolicyArea() {

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStatisticsPolicyArea/',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Thống kê sinh viên nước ngoài
function StatisticsForeignStudents() {
    var Year = $("#ddlYearStudy").val();
    var Course = $("#ddlCourse").val();
    var Term = $("ddlTermID").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowStatisticsForeignStudents?C=' + Course + '&Y=' + Year + '&T' + Term,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Phân quyền nhân viên
function PhanQuyenNhanVien() {
    var Staff = $("#ddlStaff").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/ShowPhanQuyenNhanVien?S=' + Staff,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Luu Quyền nhân viên
function LuuQuyenNhanVien() {
    var Staff = $("#ddlStaff").val();
    $("#idSave").html("Đang lưu ...");
    $.ajax({
        url: '/Staff/SaveNhanVien',
        dataType: 'html',
        type: 'POST',
        data: $('form').serialize() + '&ajax=1',
        success: function (data) {
            $("#idSave").html("Cập Nhật");
            ShowAlert("Thông báo", data);
        }
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#idSave").html("Cập Nhật");
        ShowAlert("Thông báo", err)
    });
}
//Nhap diem qua trinh giang vien
function ShowScheduleStudyUnitInputMarks() {
    var Y = $("#YearStudy").val();
    var T = $("#TermID").val();
    //$("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/ShowScheduleStudyUnitInputMarks?Y=' + Y + '&T=' + T,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Nhap diem thi theo lhp
function ShowInputMarkByStudyUnit() {
    var Y = $("#YearStudy").val();
    var T = $("#TermID").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/ShowInputMarkByStudyUnit?Y=' + Y + '&T=' + T,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Nhap diem thi theo lop ti
function ShowInputMarkByExamination() {
    var Y = $("#YearStudy").val();
    var T = $("#TermID").val();
    //$("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/ShowInputMarkByExamination?Y=' + Y + '&T=' + T,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Dang ky in bang diem
function ShowStudentRegistConfirm() {
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowStudentRegistConfirm',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}
//Luu Dang ky in bang diem
function ShowStudentRegistConfirm_Upd() {
    //var CID = $("#ddlConfirm").val();
    //var Num = $("#txtSoLuong").val();
    //var note = $("#txtGhiChu").val();
    //if (Num > 2) {
    //    ShowAlert("Thông báo", "Đăng ký số lượng không quá 2 trên 1 lần đăng ký");
    //    return false;
    //}
    //if (Num <= 0) {
    //    ShowAlert("Thông báo", "Đăng ký số lượng không hợp lệ!");
    //    return false;
    //}

    var Confirm = $("#ddlConfirm").val();
    var Lydo = $("#ddlLydo").val();
    var Noidung = "";

    if (Confirm == 1 && Lydo == 5) {
        Noidung = $("#txtContent").val();
    }

    $.ajax({
        type: 'POST',
        url: '/Home/ShowStudentRegistConfirm_upd',
        async: true,
        dataType: 'text',
        data: { Confirm: Confirm, Lydo: Lydo, Noidung: Noidung },
        success: function (html) {
            if (html == "0") {
                ShowStudentRegistConfirm();
                ShowAlert("Thông báo", "Đăng ký thành công")
            }
            else if (html == "1") {
                ShowAlert("Thông báo", "Đăng ký thất bại")
            }
            else {
                ShowAlert("Thông báo", html);
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}
//Print div
function printContent(el) {
    var restorepage = document.body.innerHTML;
    var printcontent = document.getElementById(el).innerHTML;
    document.body.innerHTML = printcontent;
    window.print();
    document.body.innerHTML = restorepage;
}

//Load popup sv || Thành 04042017
function LoadHtmlToPopup(url) {
    $("#myModal").modal();
    $("#MessagesContent").load(url);
}

//Lay danh sach sinh vien || Thành 04042017
function GetStudentInClassStudents() {
    
    var CourseID = $("#ddlCourse").val();
    var DepartmentID = $("#ddlDepartment").val();
    var StudyTypesID = $("#ddlStudyTypes").val();
    var GradualevelID = $("#ddlGradualevel").val();
    var ClassStudentID = $("#ddlClassStudent").val();
    var stid = $("#txtStudentID").val().trim();
    if (stid != '')
        id = stid;
    $.ajax({
        type: 'POST',
        url: '/Staff/GetStudentInClassStudents/',
        data: {
            GradualevelID: GradualevelID
            , StudyTypesID: StudyTypesID
            , DepartmentID: DepartmentID
            , CourseID: CourseID
            , ClassStudentID: ClassStudentID
        },
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divDanhSachSinhVien").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divDanhSachSinhVien").html("Lỗi " + err);
    });
}

function LoadBoMon() {
    var Department = $('#ddlDepartment').val();
    // Send an AJAX request
    $.getJSON("/Public/GetSubDepartments/" + Department)
        .done(function (data) {
            $("#ddlSubDepartment").empty();
            // On success, 'data' contains a list of products.
            $("<option value='-1'>" + "Tất cả" + " </option>").appendTo($('#ddlSubDepartment'));
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.OlogyID + ">" + item.OlogyName + " </option>").appendTo($('#ddlSubDepartment'));
            });
            ShowMonHocTheoKhoa();
        });
}

function ShowMonHocTheoKhoa() {
    var d = $("#ddlDepartment").val();
    var sd = $("#ddlSubDepartment").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Public/ShowBoMon?DepartmentID=' + d + '&SubDepartmentID=' + sd,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
    });
}

//Thong ke || An hien control || Thanh 14052017
function ShowHideControll() {
    var LoaiPhieu = $("#dr_BieuMau :selected").val();
    if (LoaiPhieu == "Mau01") {
        $('#bt_LayKhoaHoc').attr("disabled", "disabled");
        $('#txt_KhoaHoc').attr("disabled", "disabled");
    }
    else {
        $('#bt_LayKhoaHoc').removeAttr("disabled");
        $('#txt_KhoaHoc').removeAttr("disabled");
    }
}

//Thong ke || Mo Report || Thanh 14052017
function ShowReport() {
    $("#divHienthi").showLoading("Loading ....");
    var LoaiPhieu = $("#dr_BieuMau :selected").val();
    var DonViBaoCao = $('#txt_DonViBaoCao').val();
    var NgayBaoCao = $('#txt_NgayBaoCao').val();
    var NamTuyenSinh = $('#txt_NamTuyenSinh').val();
    var NgayIn = $('#txt_NgayIn').val();
    var NamHoc = $('#txt_NamHoc').val();
    var HocKy = $('#txt_HocKy').val();

    if (LoaiPhieu != 'Demo') {
        $.ajax({
            type: 'Get',
            url: '/Public/ThongKe/' + "?t=" + Math.random(),
            async: true,
            data: { donViBaoCao: DonViBaoCao, ngayBaoCao: NgayBaoCao, namTuyenSinh: NamTuyenSinh, loaiPhieu: LoaiPhieu, namHoc: NamHoc, hocKy: HocKy },
            dataType: 'html',
            success: function (html) {
                $("#divHienthi").html(string(html));
            },
        })
        .fail(
        function (jqXHR, textStatus, err) {
            $("#divHienthi").html(err);
            ShowAlert("Thông báo", err)
        });
    }
    else {
        alert(1);
        $.ajax({
            type: 'Get',
            url: '/Public/ThongKeCrystal/' + "?t=" + Math.random(),
            async: true,
            //data: { donViBaoCao: DonViBaoCao, ngayBaoCao: NgayBaoCao, namTuyenSinh: NamTuyenSinh, loaiPhieu: LoaiPhieu, namHoc: NamHoc, hocKy: HocKy },
            dataType: 'html',
            success: function (html) {
                $("#divHienthi").html(string(html));
            },
        })
       .fail(
       function (jqXHR, textStatus, err) {
           $("#divHienthi").html(err);
           ShowAlert("Thông báo", err)
       });
    }
}

//Thong ke || Lay khoa hoc || Thanh 14052017
function LoadCourseThongKe() {
    var G = $('#ddlGradualevel').val();
    var S = $('#ddlStudyTypes').val();
    var id = G + "$" + S;
    $.ajax({
        type: 'POST',
        url: '/Staff/LoadCourseThongKe/',
        data: { id: id },
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divDanhSachKhoaHoc").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divDanhSachKhoaHoc").html("Lỗi " + err);
    });
}

//Thong ke || Hien thi popup || Thanh 14052017
function HienThiFormChonKhoa() {
    LoadHtmlToPopup("/Staff/GetDanhSachKhoaHoc");
}

//Hoat dong sinh vien || chon tat ca su kien || Thanh 19062017
function CheckAllItems() {
    $("#CheckAll").change(function () {
        $("input:checkbox").prop('checked', $(this).prop("checked"));
    });
}

//Hoat dong sinh vien || load theo nam hoc hoc ky || Thanh 19062017
function LoadActivity() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    var isCheck = $("input:radio[name=CheckActivities]:checked").val();
    if (isCheck == 0) {
        ShowRegisterActivity();
    }
    else {
        ShowEventActivity();
    }
}

//Hoat dong sinh vien || dang ky hoat dong || Thanh 19062017
function ShowRegisterActivity() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/RegisterActivity' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
    });
}

//Hoat dong sinh vien || hoat dong da tham gia || Thanh 19062017
function ShowEventActivity() {
    //var YearStudy  = $("#ddlYearStudy").val();
    //var TermID = $("#ddlTermID").val();
    //var TermID = $("#ddlTermID").val();

    $("#divKetQua").showLoading("Loading ....");

    $.ajax({
        type: 'GET',
        url: '/Home/EventActivity' + "?t=" + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
    });
}

//kiem tra lỗi nhap điểm
function Onblur(cuobj, max, obj) {
    var valueee = document.getElementById(cuobj).value;

    if (parseInt(max) > 0) {
        if (valueee == '' || isNaN(valueee) == true || parseInt(valueee) > parseInt(max) || parseInt(valueee) < 0 || Number.isInteger(parseFloat(valueee)) == false) {
            document.getElementById(cuobj).value = 0;

            document.getElementById(cuobj).style.border = "thin solid red";
            document.getElementById("btnSave").setAttribute("disabled", true);
            ShowAlert("Thông báo", "Điểm không hợp lệ. Hệ thống sẽ trả điểm đó về 0.")
            return false;
        }
        else {
            document.getElementById(cuobj).style.border = "thin solid blue";
            $("#btnSave").removeAttr("disabled");
            return false;
        }
    }

    else if (parseInt(max) < 0) {
        if (valueee == '' || isNaN(valueee) == true || parseInt(valueee) < 0 || Number.isInteger(parseFloat(valueee)) == false) {
            document.getElementById(cuobj).value = 0;

            document.getElementById(cuobj).style.border = "thin solid red";
            document.getElementById("btnSave").setAttribute("disabled", true);
            ShowAlert("Thông báo", "Điểm không hợp lệ. Hệ thống sẽ trả điểm đó về 0.")
            return false;
        }
        else {
            document.getElementById(cuobj).style.border = "thin solid blue";
            $("#btnSave").removeAttr("disabled");
            return false;
        }
    }
}

//Hoat dong sinh vien || Luu hoat dong || Thanh 19062017
function SaveActivity() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    var strData = "";
    $('input[name=CheckSub]:checked').each(function () {
        strData += $(this).val() + ';';
    });

    //if (strData == "") {
    //    ShowAlert("Thông báo", "Vui lòng chọn sự kiện để đăng ký !");
    //    return;
    //}

    $.ajax({
        type: 'GET',
        url: '/Home/SaveActivity' + "?t=" + Math.random(),
        async: true,
        data: { dataSelect: strData, yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            ShowAlert("Thông báo", html)
        },
    })
   .fail(
   function (jqXHR, textStatus, err) {
       ShowAlert("Thông báo", err);
   });
}

//Noi ngoai tru
function LoadNoiNgoaiTru() {
    var isCheck = $("input:radio[name=NoiNgoaiTru]:checked").val();
    if (isCheck == 0) {
        ShowKTX();
    }
    else {
        ShowNgoaiTru();
    }
}

//KTX
function ShowKTX() {
    document.getElementById('btnDangKy').style.display = 'block';
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowKTX',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}

//Gia hạn KTX
function ShowGiaHan() {
    document.getElementById('btnDangKy').style.display = 'none';
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowGiaHan',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divKetQua").html(err);
            ShowAlert("Thông báo", err)
        });
}

// Show form đăng ký gia hạn KTX
function DangKyGiaHanKTXForm(IdSvKt, NgayRa) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Home/DangKyGiaHanKTXForm?IdSvKt=" + IdSvKt + "&NgayRa=" + NgayRa);
}

// Luu dang ky gia han KTX
function DangKyGiaHanKTX() {
    var IdSvKt = $("#txtIdSvKt").val();
    var LoaiGiaHan = $("#ddlLoaiGiaHan").val();
    var NgayRa = $("#txtNgayRa").val();

    if (LoaiGiaHan == "0") {
        if ($("#txtNgayGiaHan").val() == "" || $("#txtNgayGiaHan").val() == undefined) {
            ShowAlert("Thông báo", "Chưa nhập ngày gia hạn!")
            return;
        }
        else {
            var NgayGiaHan = $("#txtNgayGiaHan").val();
        }
    }
    else {
        var NgayGiaHan = "";
    }

    $.ajax({
        type: 'POST',
        url: '/Home/DangKyGiaHanKTX',
        async: true,
        dataType: 'text',
        data: { LoaiGiaHan: LoaiGiaHan, NgayGiaHan: NgayGiaHan, IdSvKt: IdSvKt, NgayRa: NgayRa },
        success: function (html) {
            if (html == "0") {
                $("#myModal").modal('hide');
                ShowAlert("Thông báo", "Đăng ký thành công")
                $("#divKetQua").load("/Home/ShowGiaHan/")
            }           
            else if (html == "1") {
                ShowAlert("Thông báo", "Sinh viên đã kết thúc giao dịch học kỳ không thể đăng ký gia hạn");
            }
            else if (html == "2") {
                ShowAlert("Thông báo", "Sinh viên đã đăng ký gia hạn trước đó");
            }
            else if (html == "3") {
                ShowAlert("Thông báo", "Đăng ký thất bại")
            }
            else {
                ShowAlert("Thông báo", html);
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}

// Show form cập nhật gia hạn KTX
function CapNhatGiaHanKTXForm(ID, NgayRa) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Home/CapNhatGiaHanKTXForm?ID=" + ID + "&NgayRa=" + NgayRa);
}

// Luu cap nhat gia han KTX
function CapNhatGiaHanKTX() {
    var ID = $("#txtID").val();
    var NgayRa = $("#txtNgayRa").val();
    var NgayGiaHan = $("#txtNgayGiaHan").val();

    if (NgayGiaHan == "" || NgayGiaHan == undefined) {
        ShowAlert("Thông báo", "Chưa nhập ngày gia hạn!")
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/Home/CapNhatGiaHanKTX',
        async: true,
        dataType: 'text',
        data: { NgayGiaHan: NgayGiaHan, ID: ID, NgayRa: NgayRa },
        success: function (html) {
            if (html == "0") {
                $("#myModal").modal('hide');
                ShowAlert("Thông báo", "Cập nhật thành công")
                $("#divKetQua").load("/Home/ShowGiaHan/")
            }
            else if (html == "1") {
                ShowAlert("Thông báo", "Đã duyệt không thể cập nhật");
            }
            else {
                ShowAlert("Thông báo", html);
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}

// Xoa gia han KTX
function XoaGiaHanKTX(ID) {
    if (confirm('Bạn có chắc chắn muốn xóa gia hạn này?')) {
        $.ajax({
            type: 'POST',
            url: '/Home/XoaGiaHanKTX',
            async: true,
            dataType: 'text',
            data: { ID: ID },
            success: function (html) {
                if (html == "0") {
                    $("#myModal").modal('hide');
                    ShowAlert("Thông báo", "Xóa thành công")
                    $("#divKetQua").load("/Home/ShowGiaHan/")
                }
                else if (html == "1") {
                    ShowAlert("Thông báo", "Đã duyệt không thể xóa");
                }
                else {
                    ShowAlert("Thông báo", html);
                }
            },
        })
        .fail(
            function (jqXHR, textStatus, err) {
                ShowAlert("Thông báo", err)
            });
    }  
}

//Xin ra KTX
function ShowXinRa() {
    document.getElementById('btnDangKy').style.display = 'none';
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowXinRa',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divKetQua").html(err);
            ShowAlert("Thông báo", err)
        });
}

// Show form đăng ký xin ra KTX
function DangKyXinRaKTXForm(IdSvKt, NgayRa) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Home/DangKyXinRaKTXForm?IdSvKt=" + IdSvKt + "&NgayRa=" + NgayRa);
}

// Luu dang ky xin ra KTX
function DangKyXinRaKTX() {
    var IdSvKt = $("#txtIdSvKt").val();
    var NgayRa = $("#txtNgayRa").val();

    if ($("#txtNgayXinRa").val() == "" || $("#txtNgayXinRa").val() == undefined) {
        ShowAlert("Thông báo", "Chưa nhập ngày xin ra!")
        return;
    }
    else {
        var NgayXinRa = $("#txtNgayXinRa").val();
    }

    $.ajax({
        type: 'POST',
        url: '/Home/DangKyXinRaKTX',
        async: true,
        dataType: 'text',
        data: { NgayXinRa: NgayXinRa, IdSvKt: IdSvKt, NgayRa: NgayRa },
        success: function (html) {
            if (html == "0") {
                $("#myModal").modal('hide');
                ShowAlert("Thông báo", "Đăng ký thành công")
                $("#divKetQua").load("/Home/ShowXinRa/")
            }
            else if (html == "1") {
                ShowAlert("Thông báo", "Sinh viên đã kết thúc giao dịch học kỳ không thể đăng ký gia hạn");
            }
            else if (html == "2") {
                ShowAlert("Thông báo", "Sinh viên đã đăng ký gia hạn trước đó");
            }
            else {
                ShowAlert("Thông báo", html);
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}

// Show form cập nhật xin ra KTX
function CapNhatXinRaKTXForm(ID, NgayRa) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Home/CapNhatXinRaKTXForm?ID=" + ID + "&NgayRa=" + NgayRa);
}

// Luu cap nhat xin ra KTX
function CapNhatXinRaKTX() {
    var ID = $("#txtID").val();
    var NgayRa = $("#txtNgayRa").val();
    var NgayXinRa = $("#txtNgayXinRa").val();

    if (NgayXinRa == "" || NgayXinRa == undefined) {
        ShowAlert("Thông báo", "Chưa nhập ngày xin ra!")
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/Home/CapNhatXinRaKTX',
        async: true,
        dataType: 'text',
        data: { NgayXinRa: NgayXinRa, ID: ID, NgayRa: NgayRa },
        success: function (html) {
            if (html == "0") {
                $("#myModal").modal('hide');
                ShowAlert("Thông báo", "Cập nhật thành công")
                $("#divKetQua").load("/Home/ShowXinRa/")
            }
            else if (html == "1") {
                ShowAlert("Thông báo", "Đã duyệt không thể cập nhật");
            }
            else {
                ShowAlert("Thông báo", html);
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}

// Xoa xin ra KTX
function XoaXinRaKTX(ID) {
    if (confirm('Bạn có chắc chắn muốn xóa đơn xin ra này?')) {
        $.ajax({
            type: 'POST',
            url: '/Home/XoaXinRaKTX',
            async: true,
            dataType: 'text',
            data: { ID: ID },
            success: function (html) {
                if (html == "0") {
                    $("#myModal").modal('hide');
                    ShowAlert("Thông báo", "Xóa thành công")
                    $("#divKetQua").load("/Home/ShowXinRa/")
                }
                else if (html == "1") {
                    ShowAlert("Thông báo", "Đã duyệt không thể xóa");
                }
                else {
                    ShowAlert("Thông báo", html);
                }
            },
        })
        .fail(
            function (jqXHR, textStatus, err) {
                ShowAlert("Thông báo", err)
            });
    }
}

//Ngoai tru
function ShowNgoaiTru() {
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowNgoaiTru',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}

// Show form đăng ký nội ngoại trú
function ShowDangKy() {
    var isCheck = $("input:radio[name=NoiNgoaiTru]:checked").val();
    if (isCheck == 0) {
        $("#myModal").modal();
        $("#MessagesContent").load("/Home/ShowInsertKTXForm/");
    }
    else {
        $("#myModal").modal();
        $("#MessagesContent").load("/Home/ShowInsertNgoaiTruForm/");
    }
}

// Show form cập nhật KTX
function ShowUpdateKTXForm(IDlog) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Home/ShowUpdateKTXForm?IDlog=" + IDlog);
}

// Show form cập nhật ngoại trú
function ShowUpdateNgoaiTruForm(IDlog) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Home/ShowUpdateNgoaiTruForm?IDlog=" + IDlog);
}

// Luu KTX
function StudentKTX_Ins() {
    //var Phong = $("#ddlRoom").val();
    var Phong = "";
    var DoiTuong = $("#ddlDoiTuong").val();
    //var Lau = $("#txtLau").val();
    var NgayVao = $("#txtNgayVao").val();
    var NgayRa = $("#txtNgayRa").val();
    //var YearStudy = $("#ddlYearStudy").val();
    //var TermID = $("#ddlTermID").val();


    if (NgayVao == "" || NgayVao == undefined) {
        ShowAlert("Thông báo", "Chưa nhập ngày vào!")
        return;
    }
    else if (NgayRa == "" || NgayRa == undefined) {
        ShowAlert("Thông báo", "Chưa nhập ngày ra!")
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/Home/StudentKTX_Ins',
        async: true,
        dataType: 'text',
        data: { Phong: Phong, DoiTuong: DoiTuong, NgayVao: NgayVao, NgayRa: NgayRa }, //, YearStudy: YearStudy, TermID: TermID },
        success: function (html) {
            if (html == "0") {
                $("#myModal").modal('hide');
                ShowAlert("Thông báo", "Đăng ký thành công")
                $("#divKetQua").load("/Home/ShowKTX/")
            }
            else if (html == "1") {
                ShowAlert("Thông báo", "Đăng ký thất bại")
            }
            else {
                ShowAlert("Thông báo", html);
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}

// Luu ngoai tru
function StudentNgoaiTru_Ins() {
    var SoNha = $("#txtSoNhaThuongTru").val();
    var PhuongXa = $("#ddlPhuongXaThuongTru").val();
    var QuanHuyen = $("#ddlQuanHuyenThuongTru").val();
    var TinhThanh = $("#ddlTinhThanhThuongTru").val();
    var TenChuTro = $("#txtTenChuTro").val();
    var PhoneChuTro = $("#txtPhoneChuTro").val();
    //var YearStudy = $("#ddlYearStudy").val();
    //var TermID = $("#ddlTermID").val();
    $.ajax({
        type: 'POST',
        url: '/Home/StudentNgoaiTru_Ins',
        async: true,
        dataType: 'text',
        data: { SoNha: SoNha, PhuongXa: PhuongXa, QuanHuyen: QuanHuyen, TinhThanh: TinhThanh, TenChuTro: TenChuTro, PhoneChuTro: PhoneChuTro }, //, YearStudy: YearStudy, TermID: TermID },
        success: function (html) {
            if (html == "0") {
                $("#myModal").modal('hide');
                ShowAlert("Thông báo", "Đăng ký thành công")
                $("#divKetQua").load("/Home/ShowNgoaiTru/")
            }
            else if (html == "1") {
                ShowAlert("Thông báo", "Đăng ký thất bại")
            }
            else {
                ShowAlert("Thông báo", html);
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}

// Cap nhat KTX
function StudentKTX_Upd() {
    //var Phong = $("#ddlRoom").val();
    var Phong = "";
    var DoiTuong = $("#ddlDoiTuong").val();
    //var Lau = $("#txtLau").val();
    var NgayVao = $("#txtNgayVao").val();
    var NgayRa = $("#txtNgayRa").val();
    //var YearStudy = $("#ddlYearStudy").val();
    //var TermID = $("#ddlTermID").val();
    var IDlog = $("#txtIDlog").val();

    if (NgayVao == "" || NgayVao == undefined) {
        ShowAlert("Thông báo", "Chưa nhập ngày vào!")
        return;
    }
    else if (NgayRa == "" || NgayRa == undefined) {
        ShowAlert("Thông báo", "Chưa nhập ngày ra!")
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/Home/StudentKTX_Upd',
        async: true,
        dataType: 'text',
        data: { Phong: Phong, DoiTuong: DoiTuong, NgayVao: NgayVao, NgayRa: NgayRa, IDlog: IDlog }, //, YearStudy: YearStudy, TermID: TermID, IDlog: IDlog },
        success: function (html) {
            if (html == "0") {
                $("#myModal").modal('hide');
                ShowAlert("Thông báo", "Cập nhật thành công")
                $("#divKetQua").load("/Home/ShowKTX/")
            }
            else if (html == "1") {
                ShowAlert("Thông báo", "Cập nhật thất bại")
            }
            else {
                ShowAlert("Thông báo", html);
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}

// Cap nhat ngoai tru
function StudentNgoaiTru_Upd() {
    var SoNha = $("#txtSoNhaThuongTru").val();
    var PhuongXa = $("#ddlPhuongXaThuongTru").val();
    var QuanHuyen = $("#ddlQuanHuyenThuongTru").val();
    var TinhThanh = $("#ddlTinhThanhThuongTru").val();
    var TenChuTro = $("#txtTenChuTro").val();
    var PhoneChuTro = $("#txtPhoneChuTro").val();
    //var YearStudy = $("#ddlYearStudy").val();
    //var TermID = $("#ddlTermID").val();
    var IDlog = $("#txtIDlog").val();
    $.ajax({
        type: 'POST',
        url: '/Home/StudentNgoaiTru_Upd',
        async: true,
        dataType: 'text',
        data: { SoNha: SoNha, PhuongXa: PhuongXa, QuanHuyen: QuanHuyen, TinhThanh: TinhThanh, TenChuTro: TenChuTro, PhoneChuTro: PhoneChuTro, IDlog: IDlog }, //, YearStudy: YearStudy, TermID: TermID, IDlog: IDlog },
        success: function (html) {
            if (html == "0") {
                $("#myModal").modal('hide');
                ShowAlert("Thông báo", "Cập nhật thành công")
                $("#divKetQua").load("/Home/ShowNgoaiTru/")
            }
            else if (html == "1") {
                ShowAlert("Thông báo", "Cập nhật thất bại")
            }
            else {
                ShowAlert("Thông báo", html);
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}

//Diem ren luyen || Load diem ren luyen theo nam hoc hoc ky|| Thanh 20062017
function LoadBehavior() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    var isCheck = $("input:radio[name=CheckBehaviorRole]:checked").val();
    if (isCheck == 0) {
        ShowBehaviorByStudent();
    }
    else {
        ShowBehaviorByClass();
    }
}

//Diem ren luyen || Diem ren luyen sinh vien|| Thanh 20062017
function ShowBehaviorByStudent() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/BehaviorByStudent' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
    });
}

//Diem ren luyen || Lưu điểm rèn luyện bởi sinh viên|| Thanh 22062017
function SaveBehaviorByStudent() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    var StrData = "";
    $('#ContentContainsScore input').each(function () {

        var name = this.name;
        if (name != "txtDiemCuoi") {
            if (this.type == 'checkbox' || this.type == 'radio') {
                if (this.checked == true) {
                    StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';
                }
                else {
                    StrData += $(this).attr("data-BehaviorDetail") + ';' + 0 + '!';
                }
            }
            else {
                StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';
            }

            // alert(StrData);
            //StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';

            //if (this.type == 'checkbox') {
            //    if (this.checked == true) {
            //        StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';
            //    }
            //    else {
            //        StrData += $(this).attr("data-BehaviorDetail") + ';' + 0 + '!';
            //    }
            //}
            //else {
            //    StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';
            //}
        }
    });

    $.ajax({
        type: 'POST',
        url: '/Home/SaveBehaviorByStudent' + "?t=" + Math.random(),
        async: true,
        data: { strData: StrData, yearStudy: YearStudy, termID: TermID, IsSaveBehavior: true },
        dataType: 'JSON',
        success: function (result) {
            if (result) {
                ShowBehaviorByStudent();
                ShowAlert("Thông báo", "Lưu Thành công !");
            }
            else {
                ShowAlert("Thông báo", "Lưu thất bại !");
            }
        },
    })
   .fail(
   function (jqXHR, textStatus, err) {
       ShowAlert("Thông báo", err);
   });
}


function OnchangeScoreLTGV() {
    onLoadTotalLTGV();
}
function onLoadTotalLTGV() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    var StrData = "";
    $('#ContentContainsScore input').each(function () {
        var name = this.name;
        if (name != "txtDiemCuoi") {
            if (this.type == 'checkbox') {
                if (this.checked == true) {
                    StrData += $(this).val() + ';';

                }
                else {
                    StrData += 0 + ';';
                }
            }
            else {
                StrData += $(this).val() + ';';
            }
        }
    });
    var entry = 0;
    var myArray = StrData.split(';;');
    for (var i = 0; i < myArray.length - 1; i++) {
        entry = entry + parseInt(myArray[i]);

    }
    document.getElementById("lblTongDiemDuKien").innerHTML = entry;
}

//Diem ren luyen || Sinh viên thêm hoạt động|| Thanh 21062017
function ShowAddActivity() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    $.ajax({
        type: 'GET',
        url: '/Home/AddActivities' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
.fail(
   function (jqXHR, textStatus, err) {
       ShowAlert("Thông báo", err);
   });
}

//Diem ren luyen ||Lưu sinh viên thêm hoạt động|| Thanh 21062017
function SaveAddActivity(YearStudy, TermID) {
    var BehaviorGroupID = $("#BehaviorGroupID").val();
    var txt_Content = $("#txt_Content").val();
    if (txt_Content == "") {
        ShowAlert("Thông báo", "Vui lòng nhập nội dung !");
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/Home/SaveAddActivities' + "?t=" + Math.random(),
        async: true,
        data: { bhvGroupID: BehaviorGroupID, bhvGroupDetailName: txt_Content, yearStudy: YearStudy, termID: TermID },
        dataType: 'JSON',
        success: function (result) {
            if (result == '0')
                ShowAddActivity();
            else
                ShowAlert("Thông báo", "Lưu không thành công vui lòng kiểm tra lại !");
        },
    })
.fail(
  function (jqXHR, textStatus, err) {
      ShowAlert("Thông báo", err);
  });
}

//Diem ren luyen ||Xóa sinh viên thêm hoạt động|| Thanh 21062017
function DeleteAddActivity(BhvGroupDetailID, BhvGroupID, YearStudy, TermID) {

    $.ajax({
        type: 'POST',
        url: '/Home/DeleteAddActivities' + "?t=" + Math.random(),
        async: true,
        data: { bhvGroupDetailID: BhvGroupDetailID, bhvGroupID: BhvGroupID, yearStudy: YearStudy, termID: TermID },
        dataType: 'JSON',
        success: function (result) {
            if (result == '0')
                ShowAddActivity();
            else
                ShowAlert("Thông báo", "Lưu không thành công vui lòng kiểm tra lại !");
        },
    })
.fail(
  function (jqXHR, textStatus, err) {
      ShowAlert("Thông báo", err);
  });

}

//Diem ren luyen || Sinh viên xem hoat động|| Thanh 21062017
function ShowViewActivityAdd() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $.ajax({
        type: 'GET',
        url: '/Home/ViewActivitiesAdd' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
.fail(
   function (jqXHR, textStatus, err) {
       ShowAlert("Thông báo", err);
   });
}

//Diem ren luyen || Diem ren luyen lop|| Thanh 20062017
function ShowBehaviorByClass() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/BehaviorByClass' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
    });
}

//Diem ren luyen ||Chuyen diem ca nhan sang diem lop|| Thanh 21062017
function MoveBehaviorCore() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $.ajax({
        type: 'POST',
        url: '/Home/MoveBehaviorByObject' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'JSON',
        success: function (result) {
            if (result == true) {
                ShowBehaviorByClass();
                ShowAlert("Thông báo", "Chuyển điểm thành công ");
            }
            else
                ShowAlert("Thông báo", "Chuyển điểm không thành công vui lòng kiểm tra lại !");
        },
    })
.fail(
  function (jqXHR, textStatus, err) {
      ShowAlert("Thông báo", err);
  });

}

//Diem ren luyen || Lưu điểm rèn luyện bởi sinh viên|| Thanh 22062017
function SaveBehaviorByObject(StudentID, Type, YearStudy, TermID) {
    var StrData = "";
    $('#ContentContainsScore input').each(function () {
        var value = "";
        var name = this.name;
        if (name != "txtDiemCuoi") { //&& name != "txtGhiChu1_" + $(this).attr("data-BehaviorDetail")) {
            if (this.type == 'checkbox' || this.type == 'radio') {
                if (this.checked == true) {
                    //value = $("[name='txtGhiChu1_" + $(this).attr("data-BehaviorDetail") + "']").val();
                    StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + ';' + '' + '!';

                    //if ($(this).attr("data-BehaviorDetail")) {
                    //    var score = $("[id='" + $(this).attr("data-BehaviorDetail") + "']").val();
                    //    StrData += $(this).attr("data-BehaviorDetail") + ';' + score + ';' + value + '!';
                    //}
                }
                else {
                    StrData += $(this).attr("data-BehaviorDetail") + ';' + 0 + ';' + '' + '!';
                }
            }
            else {
                //value = $("[name='txtGhiChu1_" + $(this).attr("data-BehaviorDetail") + "']").val();
                StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + ';' + '' + '!';

                //if ($(this).attr("data-BehaviorDetail")) {
                //    var score = $("[id='" + $(this).attr("data-BehaviorDetail") + "']").val();
                //    StrData += $(this).attr("data-BehaviorDetail") + ';' + score + ';' + value + '!';
                //}
            }

            //StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';

            //if (this.type == 'checkbox') {
            //    if (this.checked == true) {
            //        StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';
            //    }
            //    else {
            //        StrData += $(this).attr("data-BehaviorDetail") + ';' + 0 + '!';
            //    }
            //}
            //else {
            //    StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';
            //}
        }
    });

    $.ajax({
        type: 'POST',
        url: '/Home/SaveMarkBehaviorByObject' + "?t=" + Math.random(),
        async: true,
        data: { strData: StrData, studentID: StudentID, type: Type, yearStudy: YearStudy, termID: TermID },
        dataType: 'JSON',
        success: function (result) {
            if (result) {
                ShowAlert("Thông báo", "Lưu Thành công !");
            }
            else {
                ShowAlert("Thông báo", "Lưu thất bại !");
            }
        },
    })
   .fail(
   function (jqXHR, textStatus, err) {
       ShowAlert("Thông báo", err);
   });
}

//Diem ren luyen ||Cham diem hoat dong sinh vien them|| Thanh 21062017
function MarkBehaviorStudentAdd(StudentID, YearStudy, TermID) {
    $.ajax({
        type: 'GET',
        url: '/Home/MarkBehaviorStudentAdd' + "?t=" + Math.random(),
        async: true,
        data: { studentID: StudentID, yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
.fail(
 function (jqXHR, textStatus, err) {
     ShowAlert("Thông báo", err);
 });
}

//Diem ren luyen ||Cham diem sinh vien nhap them|| Thanh 21062017
function SaveMarkBehaviorStudentAdd(StudentID, YearStudy, TermID) {
    var StrData = "";
    $('input[name=txt_Score]').each(function () {
        StrData += $(this).val() + ';' + $(this).data("id") + '!';
    });

    $.ajax({
        type: 'GET',
        url: '/Home/SaveMarkBehaviorStudentAdd' + "?t=" + Math.random(),
        async: true,
        data: { data: StrData, studentID: StudentID, yearStudy: YearStudy, termID: TermID },
        dataType: 'JSON',
        success: function (result) {
            $("#myModal").modal("hide");
            ShowAlert("Thông báo", result);
        },
    })
.fail(
function (jqXHR, textStatus, err) {
    ShowAlert("Thông báo", err);
});

}

//close from 
function CloseFrom() {
    location.reload();
    $("#myAlert").modal("hide");

}

//--------------------------- Diem ren luyen CVHT -------------------------//
//Diem ren luyen ||Trang ren luyen co van hoc tap|| Thanh 21062017
function BehaviorScoreProfessor() {
    var ClassStudentID = $("#ddlClassStudent").val();
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    $("#divDiemRenLuyen").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/ProfessorBehaviorScore' + '?t=' + Math.random(),
        async: true,
        data: { classStudent: ClassStudentID, yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divDiemRenLuyen").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divDiemRenLuyen").html(err);
        ShowAlert("Thông báo", err)
    });
}


//Diem ren luyen ||Chuyen diem lop sang diem CVHT|| Thanh 21062017
function MoveBehaviorCoreByProfessor() {
    var ClassStudentID = $("#ddlClassStudent").val();
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
   
    $.ajax({
        type: 'POST',
        url: '/Professor/MoveBehaviorByObject' + "?t=" + Math.random(),
        async: true,
        data: { classStudent: ClassStudentID, yearStudy: YearStudy, termID: TermID },
        dataType: 'JSON',
        success: function (result) {
            if (result == true) {
                BehaviorScoreProfessor();
                ShowAlert("Thông báo", "Chuyển điểm thành công ");
            }
            else
                ShowAlert("Thông báo", "Chuyển điểm không thành công vui lòng kiểm tra lại !");
        },
    })
.fail(
  function (jqXHR, textStatus, err) {
      ShowAlert("Thông báo", err);
  });

}

// Chuyển điể sinh viên qua cố vấn
function MoveBehaviorCoreByStudent() {
    if (confirm('Giảng viên có chắc chắn muốn chuyển điểm?')) {
        var ClassStudentID = $("#ddlClassStudent").val();
        var YearStudy = $("#ddlYearStudy").val();
        var TermID = $("#ddlTermID").val();

        $.ajax({
            type: 'POST',
            url: '/Professor/MoveBehaviorCoreByStudent' + "?t=" + Math.random(),
            async: true,
            data: { classStudent: ClassStudentID, yearStudy: YearStudy, termID: TermID },
            dataType: 'JSON',
            success: function (result) {
                if (result == true) {
                    BehaviorScoreProfessor();
                    ShowAlert("Thông báo", "Chuyển điểm thành công ");
                }
                else
                    ShowAlert("Thông báo", "Chuyển điểm không thành công vui lòng kiểm tra lại !");
            },
        })
        .fail(
            function (jqXHR, textStatus, err) {
                ShowAlert("Thông báo", err);
            });
    }   
}

//Diem ren luyen ||Load diem ren luyen trang quan ly|| Thanh 21062017
function LoadBehaviorCoreManager() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    var ClassStudentGroupID = $("#ddlClassStudentGroupID").val();

    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/LoadBehaviorScoreManager' + "?t=" + Math.random(),
        async: true,
        data: { classStudentGroupID: ClassStudentGroupID, yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
    });
}

//Diem ren luyen ||Xuat excel detail|| Thanh 21062017
function ExportBehaviorCoreDetailByClass() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    var ClassStudentGroupID = $("#ddlClassStudentGroupID").val();

    $.ajax({
        type: 'GET',
        url: '/ReportHelper/BangDiemRenLuyenChiTiet' + "?t=" + Math.random(),
        async: true,
        data: { classStudentID: ClassStudentGroupID, yearStudy: YearStudy, termID: TermID },
        dataType: 'JSON',
        success: function (result) {
            ShowAlert("Thông báo", "Thành công !");
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err);
    });
}

//Cố vấn học tập gửi tin nhắn cho sinh viên
function GetGuiTinNhanSV() {
    $.ajax({
        type: 'GET',
        url: '/Professor/CVHTGuiTinNhanSV',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divGuiTinNhanSV").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divGuiTinNhanSV").html(err);
        ShowAlert("Thông báo", err)
    });
}

//lay danh sach lop
function LoadHtmlToPopupCVHT(url) {
    var ClassStudentID = $("#ddlClassStudent").val();
    var YearStudy = $("#ddlYearStudy").val();
    var TermId = $("#ddlTermID").val();

    $("#myModal").modal();
    $("#MessagesContent").load(url + "?ClassStudentID=" + ClassStudentID + "&YearStudy=" + YearStudy + "&TermID=" + TermId);
}

//luu tin nhan
function LuuTinNhan() {

    var txtNoiDung = tinymce.get('txtNoiDung').getContent();
    var txtMaSinhVien = $("#txtMaSinhVien").val();
    var txtTieuDe = $("#txtTieuDe").val();

    if (txtTieuDe.length > 250) {
        alert("Tiêu đề tin nhắn không được vượt quá 250 ký tự");
        return;
    }

    if (txtNoiDung.length > 2000) {
        alert("Nội dung tin nhắn không được vượt quá 2000 ký tự");
        return;
    }

    if (txtMaSinhVien == "") {
        ShowAlert("Thông báo", "Chưa nhập mã sinh viên");
        return;
    }
    if (txtTieuDe == "") {
        ShowAlert("Thông báo", "Chưa nhập tiêu đề");
        return;
    } else if (txtNoiDung == "") {
        ShowAlert("Thông báo", "Chưa nhập nội dung tin nhắn");
        return;
    } else {
        $.ajax({
            type: 'Post',
            url: '/Professor/LuuTinNhan?txtMaSinhVien=' + txtMaSinhVien + '&txtTieuDe=' + txtTieuDe + '&txtNoiDung=' + escape(txtNoiDung) + '&t=' + Math.random(),
            async: true,
            dataType: 'Json',
            success: function (result) {
                ShowAlert("Thông báo", result)
            },
        })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", "Gửi tin nhắn không thành công")
    });
    }
}
//Lưu email
function LuuEmail() {

    var txtNoiDung = tinymce.get('txtNoiDung').getContent();
    var txtMaSinhVien = $("#txtMaSinhVien").val();
    var txtTieuDe = $("#txtTieuDe").val();

    if (txtTieuDe.length > 250) {
        alert("Tiêu đề tin nhắn không được vượt quá 250 ký tự");
        return;
    }

    if (txtNoiDung.length > 2000) {
        alert("Nội dung tin nhắn không được vượt quá 2000 ký tự");
        return;
    }

    if (txtMaSinhVien == "") {
        ShowAlert("Thông báo", "Chưa nhập mã sinh viên");
        return;
    }
    if (txtTieuDe == "") {
        ShowAlert("Thông báo", "Chưa nhập tiêu đề");
        return;
    } else if (txtNoiDung == "") {
        ShowAlert("Thông báo", "Chưa nhập nội dung email");
        return;
    } else {
        $.ajax({
            type: 'Post',
            url: '/Professor/LuuEmail?txtMaSinhVien=' + txtMaSinhVien + '&txtTieuDe=' + txtTieuDe + '&txtNoiDung=' + escape(txtNoiDung) + '&t=' + Math.random(),
            async: true,
            dataType: 'Json',
            success: function (result) {
                ShowAlert("Thông báo", result)
            },
        })
            .fail(
                function (jqXHR, textStatus, err) {
                    ShowAlert("Thông báo", "Gửi email không thành công")
                });
    }
}

//------------------------------ Nhap hoc online -------------------------------
//Show form
function InsertAndUpdateOnlineAdmission(id) {
    $.ajax({
        type: 'GET',
        url: '/Staff/InsertAndUpdateOnlineAdmission' + "?t=" + Math.random(),
        async: true,
        data: { ID: id },
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
.fail(
   function (jqXHR, textStatus, err) {
       ShowAlert("Thông báo", err);
   });
}

//Save
function SaveOnlineAdmission() {
    var MaDot = $("#txtMaDot").val();
    var MaKhoa = $("#ddlCourse").val();
    var NgayBatDau = $("#txtNgayBatDau").val();
    var DoiTuong = $("#ddlDoiTuong").val();
    var NgayKetThuc = $("#txtNgayKetThuc").val();

    $.ajax({
        type: 'POST',
        url: '/Staff/SaveOnlineAdmission' + "?t=" + Math.random(),
        async: true,
        data: { maDot: MaDot, maKhoa: MaKhoa, doiTuong: DoiTuong, ngayBatDau: NgayBatDau, ngayKetThuc: NgayKetThuc },
        dataType: 'JSON',
        success: function (result) {
            if (result) {
                $("#myModal").modal("hide");
                location.reload();
            }
            else {
                ShowAlert("Thông báo", "Lưu thất bại !");
            }
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err);
    });
}

//delete
function DeleteOnlineAdmission(id) {
    $.ajax({
        type: 'POST',
        url: '/Staff/DeleteOnlineAdmission' + "?t=" + Math.random(),
        async: true,
        data: { maDot: id },
        dataType: 'JSON',
        success: function (result) {
            if (result) {
                location.reload();
                ShowAlert("Thông báo", "Xóa thành công !");
            }
            else {
                ShowAlert("Thông báo", "Xóa thất bại !");
            }
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err);
    });
}

// Lịch coi thi giảng viên || KHOA || 12.09.2017
function GetLichCoiThiGiangVien() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    $("#divLichCoiThiSinhVien").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/GetLichCoiThiGiangVien' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divLichCoiThiSinhVien").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divLichCoiThiSinhVien").html(err);
    });
}

// Xem lịch thi toàn bộ của lớp học phần || 2022
function XemLichThiGiangVien() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    $("#divXemLichThiGV").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/GetLichThiGiangVien' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divXemLichThiGV").html(string(html));
        },
    })
        .fail(
            function (jqXHR, textStatus, err) {
                $("#divXemLichThiGV").html(err);
            });
}

// Diem danh sinh vien || KHOA || 21.09.2017
function GetDiemDanhSinhVien() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $("#divGetDiemDanhSinhVien").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/GetDiemDanhSinhVien' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divGetDiemDanhSinhVien").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divGetDiemDanhSinhVien").html(err);
    });
}

//
function GetTinhThanhThuongTru() {
    $('#ddlQuanHuyenThuongTru').html('');
    $('#ddlPhuongXaThuongTru').html('');
    var QuocGiaThuongTru = $('#ddlQuocGiaThuongTru').val();
    $.getJSON('/OnlineAdmission/GetTinhThanhThuongTru?QuocGiaThuongTru=' + QuocGiaThuongTru)
        .done(function (data) {
            $("#ddlTinhThanhThuongTru").empty();
            $("<option selected>Chọn</option>").appendTo($('#ddlTinhThanhThuongTru'));
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.Value + ">" + item.Text + " </option>").appendTo($('#ddlTinhThanhThuongTru'));
            });
        });
}

function GetQuanHuyenThuongTru() {
    var TinhThuongTru = $('#ddlTinhThanhThuongTru').val();

    $.getJSON('/OnlineAdmission/GetQuanHuyenThuongTru?TinhThanhThuongTru=' + TinhThuongTru)
        .done(function (data) {
            $("#ddlQuanHuyenThuongTru").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.Value + ">" + item.Text + " </option>").appendTo($('#ddlQuanHuyenThuongTru'));
            });
        });
}

function GetTinhThanhTamTru() {
    var QuocGiaTamTru = $('#ddlQuocGiaTamTru').val();
    $.getJSON('/OnlineAdmission/GetTinhThanhTamTru?QuocGiaTamTru=' + QuocGiaTamTru)
        .done(function (data) {
            $("#ddlTinhThanhTamTru").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.Value + ">" + item.Text + " </option>").appendTo($('#ddlTinhThanhTamTru'));
            });
        });
}

function GetQuanHuyenTamTru() {
    var TinhThanhTamTru = $('#ddlTinhThanhTamTru').val();
    $.getJSON('/OnlineAdmission/GetQuanHuyenTamTru?TinhThanhTamTru=' + TinhThanhTamTru)
        .done(function (data) {
            $("#ddlQuanTamTru").empty();
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $("<option value=" + item.Value + ">" + item.Text + " </option>").appendTo($('#ddlQuanTamTru'));
            });
        });
}

//Show thao luan hiang vien
function ShowThaoLuan() {
    var YearStudy = $('#YearStudy').val();
    var TermID = $('#TermID').val();
    $("#divHienthi").html("<img src='/content/images/ajax-loader.gif' /><b>Đang tải dữ liệu .....</b>");
    $.ajax({
        type: 'GET',
        url: '/Professor/ShowThaoLuan' + '?YearStudy=' + YearStudy + '&TermID=' + TermID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divHienthi").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

// Load tin nhắn đã gửi
function GetLichSuGuiTinNhan() {
    var YearStudy = $('#YearStudy').val();
    var TermID = $('#TermID').val();
    $("#divLichSuGuiTinNhan").html("<img src='/content/images/ajax-loader.gif' /><b>Đang tải dữ liệu .....</b>");
    $.ajax({
        type: 'GET',
        url: '/Professor/LichSuGuiTinNhan',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divLichSuGuiTinNhan").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

// Load lịch sử gui email
function ShowLichSuGuiMail() {
    $("#divLichSuGuiMail").html("<img src='/content/images/ajax-loader.gif' /><b>Đang tải dữ liệu .....</b>");
    $.ajax({
        type: 'GET',
        url: '/Professor/DSLichSuGuiEmail',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divLichSuGuiMail").html(html);
        },
    })
        .fail(
            function (jqXHR, textStatus, err) {
                ShowAlert("Thông báo", err)
            });
}


// Xóa tin nhắn đã gửi || Khoa: 01.02.2018
function XoaTinNhan(MessageID) {
    $.ajax({
        type: 'Post',
        url: '/Professor/DelMessages?MessageID=' + MessageID,
        async: true,
        dataType: 'JSON',
        success: function (data) {
            ShowAlert("Thông báo", data);
            GetLichSuGuiTinNhan();
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)

        });
}

//--- insert thiết bị hư hỏng
function InUpBaoHongThietBi() {
    var ddlTenThietBi = $('#ddlTenThietBi').val();
    var ddlPhong = $('#ddlPhong').val();
    var txtSoLuong = $('#txtSoLuong').val();

    if (txtSoLuong == null || txtSoLuong == "") {
        ShowAlert("Thông báo", "Nhập số lượng")
        return;
    }

    // alert(ddlTenThietBi + ', ' + ddlPhong + ', ' + $('#txtSoLuong').val() + ', ' + $('#txtGhiChu').val());
    $.ajax({
        type: 'Post',
        url: '/Home/InUpBaoHongThietBi?ddlTenThietBi=' + ddlTenThietBi + '&ddlPhong=' + ddlPhong
            + '&txtSoLuong=' + txtSoLuong + '&txtGhiChu=' + $('#txtGhiChu').val(),
        async: true,
        dataType: 'JSON',
        success: function (data) {
            ShowAlert("Thông báo", data);
            GetDanhSachThietBiDaBaoHong();
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

function GetDanhSachThietBiDaBaoHong() {

    //$("#divGetThietBiBaoHong").html("<img src='/content/images/ajax-loader.gif' /><b>Đang tải dữ liệu .....</b>");
    $.ajax({
        type: 'Get',
        url: '/Home/GetDanhSachThietBiDaBaoHong',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divGetThietBiBaoHong").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

function XoaThietBiDaBao(ID) {
    if (!confirm("Xác nhận xóa ?")) {
        return false;
    }
    $.ajax({
        type: 'GET',
        url: '/Home/XoaThietBiDaBao?ID=' + ID + "&t=" + Math.random(),
        async: true,
        dataType: 'JSON',
        success: function (data) {
            ShowAlert("Thông báo", data);
            GetDanhSachThietBiDaBaoHong();
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#alertid").html(err);
    });
}

function InUpChuyenPhong() {

    var ddlLoaiYeuCau = $('#ddlLoaiYeuCau').val();
    var ddlPhongCu = $('#ddlPhongCu').val();
    var ddlPhongMoi = $('#ddlPhongMoi').val();
    var txtNgayChuyen = $('#datetimepicker2').val();
    // alert(ddlTenThietBi + ', ' + ddlPhong + ', ' + $('#txtSoLuong').val() + ', ' + $('#txtGhiChu').val());
    $.ajax({
        type: 'Post',
        url: '/Home/InUpChuyenPhong?ddlLoaiYeuCau=' + ddlLoaiYeuCau + '&ddlPhongCu=' + ddlPhongCu + '&ddlPhongMoi=' + ddlPhongMoi
            + '&txtNgayChuyen=' + txtNgayChuyen + '&txtGhiChu=' + $('#txtGhiChu').val(),
        async: true,
        dataType: 'JSON',
        success: function (data) {
            ShowAlert("Thông báo", data);
            GetDangKyChuyenPhongRoiKTC();
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

function DisRoiKyTucPhong() {
    var ddlLoaiYeuCau = $('#ddlLoaiYeuCau').val();
    if (ddlLoaiYeuCau == 2) {
        $('#phongmuonchuyen :input').attr('disabled', false);
    } else {
        $('#phongmuonchuyen :input').attr('disabled', true);
    }
}

function GetDangKyChuyenPhongRoiKTC() {

    //$("#divGetThietBiBaoHong").html("<img src='/content/images/ajax-loader.gif' /><b>Đang tải dữ liệu .....</b>");
    $.ajax({
        type: 'Get',
        url: '/Home/GetChuyenPhong',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divGetDangKy").html(html);
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

function XoaDangKyChuyenPhong(ID) {
    if (!confirm("Xác nhận xóa ?")) {
        return false;
    }
    $.ajax({
        type: 'GET',
        url: '/Home/XoaDangKyChuyenPhong?ID=' + ID + "&t=" + Math.random(),
        async: true,
        dataType: 'JSON',
        success: function (data) {
            ShowAlert("Thông báo", data);
            GetDangKyChuyenPhongRoiKTC();
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#alertid").html(err);
    });
}

// In bảng điểm rèn luyện của cố vấn học tập | khoa | 05.08.2018
function InDanhSachSinhVien() {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();
    var ClassStudentID = $("#ddlClassStudent").val();
    window.open("/Professor/InDanhSachSinhVien/" + YearStudy + "$" + TermID + "$" + ClassStudentID);
}

function CheckIsReadTrue(MessageID) {
    $.ajax({
        type: 'POST',
        url: '/Home/CheckIsReadTrue?MessageID=' + MessageID + "&t=" + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            location.reload();
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#alertid").html(err);
    });
}

function ReloadPage() {
    location.reload();
}

//Xem ket qua khao sat
function XemKQKS() {
    var isCheck = $("input:radio[name=KhaoSat]:checked").val();
    if (isCheck == 0) {
        DanhSachKetQuaKhaoSatHP();
    }
    else {
        DanhSachKetQuaKhaoSatThuong();
    }
}

function DanhSachKetQuaKhaoSatHP() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $("#divKetQuaKhaoSat").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/DanhSachKetQuaKhaoSatHP?YearStudy=' + YearStudy + '&TermID=' + TermID,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQuaKhaoSat").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divKetQuaKhaoSat").html(err);
        ShowAlert("Thông báo", err)
    });
}

function DanhSachKetQuaKhaoSatThuong() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $("#divKetQuaKhaoSat").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/DanhSachKetQuaKhaoSatThuong?YearStudy=' + YearStudy + '&TermID=' + TermID,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQuaKhaoSat").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divKetQuaKhaoSat").html(err);
        ShowAlert("Thông báo", err)
    });
}

function XemKetQuaDangKyHP() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $("#divKetQuaDangKyHP").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/XemKetQuaDangKyHP?YearStudy=' + YearStudy + '&TermID=' + TermID,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQuaDangKyHP").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divKetQuaDangKyHP").html(err);
            ShowAlert("Thông báo", err)
        });
}

function XuatPhieuDangKy() {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();

    window.open("/Home/XuatPhieuDangKy/" + YearStudy + "$" + TermID);
}

//Mien giam, tro cap
function LoadMienGiamTroCap() {
    var isCheck = $("input:radio[name=MienGiamTroCap]:checked").val();
    if (isCheck == 0) {
        ShowMienGiam();
    }
    else {
        ShowTroCap();
    }
}

//Mien giam
function ShowMienGiam() {
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowMienGiam',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}

//Ngoai tru
function ShowTroCap() {
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowTroCap',
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        $("#divKetQua").html(err);
        ShowAlert("Thông báo", err)
    });
}

function ShowBehaviorDiscussion(BehaviorDetailID, BehaviorDetailName, StudentID, Type, YearStudy, TermID) {
    $.ajax({
        type: 'GET',
        url: '/Home/ShowBehaviorDiscussion?BehaviorDetailID=' + BehaviorDetailID + '&StudentID=' + StudentID + '&BehaviorDetailName='
            + BehaviorDetailName + "&Type=" + Type + "&YearStudy=" + YearStudy + "&TermID=" + TermID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
    });
}

function InsertBehaviorDiscussion(BehaviorDetailID, BehaviorDetailName, StudentID, Type, YearStudy, TermID) {
    var txtNoiDung = $("#txtnoidung").val();

    $.post("/Home/InsertBehaviorDiscussion", { BehaviorDetailID: BehaviorDetailID, StudentID: StudentID, NoiDung: txtNoiDung, YearStudy: YearStudy, TermID: TermID }, function (result) {
        var html = result.replace('"', '');
        html = result.replace('"', '');
        var kq = result.indexOf("...");
        if (kq > -1) {
            $("#myModal").modal("hide");
            alert("Bình luận thành công");

            //if (Type == "1") {
            //    SaveBehaviorByStudent_Temp('');
            //    ShowBehaviorByStudent();
            //}
            //else {
            //    SaveBehaviorByObject_Temp(StudentID, Type, YearStudy, TermID, '');
            //    location.reload();
            //}            
        }
        else {
            ShowAlert("Thông báo", html);
        }
    });
}

function ShowBehaviorDetail(BehaviorDetailID, StudentID, Type, YearStudy, TermID) {
    $.ajax({
        type: 'GET',
        url: '/Home/ShowBehaviorDetail?BehaviorDetailID=' + BehaviorDetailID + '&StudentID=' + StudentID
            + "&Type=" + Type + "&YearStudy=" + YearStudy + "&TermID=" + TermID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
.fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

function InsertBehaviorDetail() {
    var Type = $("#txtType").val();
    var YearStudy = $("#txtYearStudy").val();
    var TermID = $("#txtTermID").val();
    var BehaviorDetailID = $("#txtBehaviorDetailID").val();
    var StudentID = $("#txtStudentID").val();
    var TenChiTieu = $("#txtTenChiTieu").val();

    $.post("/Home/InsertBehaviorDetail", { BehaviorDetailID: BehaviorDetailID, StudentID: StudentID, YearStudy: YearStudy, TermID: TermID, TenChiTieu: TenChiTieu }, function (result) {
        var html = result.replace('"', '');
        html = result.replace('"', '');
        var kq = result.indexOf("...");
        if (kq > -1) {
            $("#myModal").modal("hide");
            alert("Thêm hoạt động thành công");

            //if (Type == "1") {
            //    SaveBehaviorByStudent_Temp('');
            //    ShowBehaviorByStudent();
            //}
            //else {
            //    SaveBehaviorByObject_Temp(StudentID, Type, YearStudy, TermID, '');
            //    location.reload();
            //}         
        }
        else {
            ShowAlert("Thông báo", html);
        }
    });
}

function UploadFile(url) {
    if (window.FormData !== undefined) {
        var StudentID = $("#txtStudentID").val();
        var BehaviorDetailID = $("#txtBehaviorDetailID").val();
        var YearStudy = $("#txtYearStudy").val();
        var TermID = $("#txtTermID").val();

        var fileUpload = $("#HinhMinhChung").get(0);
        var files = fileUpload.files;
        var fileData = new FormData();

        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        fileData.append('username', 'Manas');

        $.ajax({
            url: '/Home/UploadFiles?StudentID=' + StudentID + '&BehaviorDetailID=' + BehaviorDetailID + '&YearStudy=' + YearStudy + '&TermID=' + TermID,
            type: "POST",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (data) {
                if (data.Result == true) {
                    return;
                }
                else {
                    alert(data.Url);
                }
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
    else {
        alert("FormData is not supported.");
    }
}

function SaveBehaviorByStudent_Temp(BehaviorDetailID) {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    var StrData = "";
    $('#ContentContainsScore input').each(function () {

        var name = this.name;
        if (name != "txtDiemCuoi") {
            if (this.type == 'checkbox' || this.type == 'radio') {
                if (this.checked == true) {
                    if (BehaviorDetailID == '' || BehaviorDetailID != $(this).attr("data-BehaviorDetail")) {
                        StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';
                    }
                }
                else {
                    if (BehaviorDetailID == '' || BehaviorDetailID != $(this).attr("data-BehaviorDetail")) {
                        StrData += $(this).attr("data-BehaviorDetail") + ';' + 0 + '!';
                    }
                }
            }
            else {
                if (BehaviorDetailID == '' || BehaviorDetailID != $(this).attr("data-BehaviorDetail")) {
                    StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + '!';
                }
            }
        }
    });

    $.ajax({
        type: 'POST',
        url: '/Home/SaveBehaviorByStudent' + "?t=" + Math.random(),
        async: true,
        data: { strData: StrData, yearStudy: YearStudy, termID: TermID, IsSaveBehavior: false },
        dataType: 'JSON',
        success: function (result) { },
    })
}

function SaveBehaviorByObject_Temp(StudentID, Type, YearStudy, TermID, BehaviorDetailID) {
    var StrData = "";
    $('#ContentContainsScore input').each(function () {
        var value = "";
        var name = this.name;
        if (name != "txtDiemCuoi") {
            if (this.type == 'checkbox' || this.type == 'radio') {
                if (this.checked == true) {
                    if (BehaviorDetailID == '' || BehaviorDetailID != $(this).attr("data-BehaviorDetail")) {
                        StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + ';' + '' + '!';
                    }
                }
                else {
                    if (BehaviorDetailID == '' || BehaviorDetailID != $(this).attr("data-BehaviorDetail")) {
                        StrData += $(this).attr("data-BehaviorDetail") + ';' + 0 + ';' + '' + '!';
                    }
                }
            }
            else {
                if (BehaviorDetailID == '' || BehaviorDetailID != $(this).attr("data-BehaviorDetail")) {
                    StrData += $(this).attr("data-BehaviorDetail") + ';' + $(this).val() + ';' + '' + '!';
                }
            }
        }
    });

    $.ajax({
        type: 'POST',
        url: '/Home/SaveMarkBehaviorByObject' + "?t=" + Math.random(),
        async: true,
        data: { strData: StrData, studentID: StudentID, type: Type, yearStudy: YearStudy, termID: TermID },
        dataType: 'JSON',
        success: function (result) { },
    })
}

function XemHinhMinhChung(BehaviorDetailID, StudentID, YearStudy, TermID) {
    $("#myModal").modal();
    $("#MessagesContent").load("/Home/XemHinhMinhChung?BehaviorDetailID=" + BehaviorDetailID + "&StudentID=" + StudentID
        + "&YearStudy=" + YearStudy + "&TermID=" + TermID);
}

function HienThiSuKienCTXH() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    $("#divSuKienCTXH").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/HienThiSuKienCTXH' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divSuKienCTXH").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divSuKienCTXH").html(err);
    });
}

function HienFormDangKyCTXH(ExcutionActivityID) {
    $.ajax({
        type: 'GET',
        url: '/Home/HienFormDangKyCTXH?ExcutionActivityID=' + ExcutionActivityID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
    });
}

function DangKyCTXH(ExcutionActivityID) {
    var Roles = $("#ddlRoles").val();

    $.post("/Home/DangKyCTXH", { ExcutionActivityID: ExcutionActivityID, Roles: Roles }, function (result) {
        var html = result.replace('"', '');
        html = result.replace('"', '');
        var kq = result.indexOf("...");
        if (kq > -1) {
            $("#myModal").modal("hide");
            alert("Đăng ký thành công");
            HienThiSuKienCTXH();
        }
        else {
            ShowAlert("Thông báo", html);
        }
    });
}

function HuyDangKyCTXH(ExcutionActivityID) {
    if (confirm("Bạn có chắc chắn muốn hủy đăng ký?")) {
        $.post("/Home/HuyDangKyCTXH", { ExcutionActivityID: ExcutionActivityID }, function (result) {
            var html = result.replace('"', '');
            html = result.replace('"', '');
            var kq = result.indexOf("...");
            if (kq > -1) {
                $("#myModal").modal("hide");
                alert("Hủy đăng ký thành công");
                HienThiSuKienCTXH();
            }
            else {
                ShowAlert("Thông báo", html);
            }
        });
    }   
}

function DeleteBehaviorDetail(BehaviorDetailID, StudentID, YearStudy, TermID, Type) {
    if (confirm("Bạn có chắc chắn muốn xóa hoạt động này?")) {
        $.post("/Home/DeleteBehaviorDetail", { BehaviorDetailID: BehaviorDetailID, StudentID: StudentID, YearStudy: YearStudy, TermID: TermID }, function (result) {
            var html = result.replace('"', '');
            html = result.replace('"', '');
            var kq = result.indexOf("...");
            if (kq > -1) {
                $("#myModal").modal("hide");
                alert("Xóa hoạt động thành công");

                if (Type == "1") {
                    SaveBehaviorByStudent_Temp(BehaviorDetailID);
                    ShowBehaviorByStudent();
                }
                else {
                    SaveBehaviorByObject_Temp(StudentID, Type, YearStudy, TermID, BehaviorDetailID);
                    location.reload();
                }
            }
            else {
                ShowAlert("Thông báo", html);
            }
        });
    }
}

function ShowEditBehaviorDetailForm(BehaviorDetailID, StudentID, YearStudy, TermID, Type) {
    $.ajax({
        type: 'GET',
        url: '/Home/ShowEditBehaviorDetailForm?BehaviorDetailID=' + BehaviorDetailID + '&StudentID=' + StudentID
            + "&YearStudy=" + YearStudy + "&TermID=" + TermID + "&Type=" + Type + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        ShowAlert("Thông báo", err)
    });
}

function UpdateBehaviorDetail() {
    var Type = $("#txtType").val();
    var YearStudy = $("#txtYearStudy").val();
    var TermID = $("#txtTermID").val();
    var BehaviorDetailID = $("#txtBehaviorDetailID").val();
    var StudentID = $("#txtStudentID").val();
    var TenChiTieu = $("#txtTenChiTieu").val();

    $.post("/Home/UpdateBehaviorDetail", { BehaviorDetailID: BehaviorDetailID, StudentID: StudentID, YearStudy: YearStudy, TermID: TermID, TenChiTieu: TenChiTieu }, function (result) {
        var html = result.replace('"', '');
        html = result.replace('"', '');
        var kq = result.indexOf("...");
        if (kq > -1) {
            $("#myModal").modal("hide");
            alert("Cập nhật hoạt động thành công");

            //if (Type == "1") {
            //    SaveBehaviorByStudent_Temp('');
            //    ShowBehaviorByStudent();
            //}
            //else {
            //    SaveBehaviorByObject_Temp(StudentID, Type, YearStudy, TermID, '');
            //    location.reload();
            //}
        }
        else {
            ShowAlert("Thông báo", html);
        }
    });
}

function HienThiSuKienCTXH_NV() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();

    $("#divSuKienCTXH").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Staff/HienThiSuKienCTXH' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divSuKienCTXH").html(string(html));
        },
    })
    .fail(
    function (jqXHR, textStatus, err) {
        $("#divSuKienCTXH").html(err);
    });
}

function DiemDanhCTXH() {
    debugger;
    var StudentID = $("#txtMASV").val();
    var ExcutionActivityID = $("#txtExcutionActivityID").val();

    $.post("/Staff/DiemDanhCTXH", { ExcutionActivityID: ExcutionActivityID, StudentID: StudentID }, function (result) {
        var html = result.replace('"', '');
        html = result.replace('"', '');
        var kq = result.indexOf("...");
        if (kq > -1) {
            alert("Điểm danh thành công");
            document.getElementById("txtMASV").value = "";
            document.getElementById("txtMASV").focus();
        }
        else {
            alert(html);
            document.getElementById("txtMASV").value = "";
            document.getElementById("txtMASV").focus();
        }
    });
}

function DanhSachHoaDon() {
    var YearStudy = $('#ddlYearStudy').val();
    var TermID = $('#ddlTermID').val();
    $("#divDanhSachHoaDon").showLoading("Loading ...");

    $.ajax({
        type: 'GET',
        url: '/Home/DanhSachHoaDon' + '?YearStudy=' + YearStudy + '&TermID=' + TermID + '&t=' + Math.random(),
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divDanhSachHoaDon").html(html);
        },
    })
        .fail(
            function (jqXHR, textStatus, err) {
                ShowAlert("Thông báo", err)
            });
}

//Hien thi danh sach LHP de phan cong
function HienThiLHP_PhanCong() {
    var Y = $("#YearStudy").val();
    var T = $("#TermID").val();
    $("#divKetQua").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/HienThiLHPPhanCong?Y=' + Y + '&T=' + T,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divKetQua").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divKetQua").html(err);
            ShowAlert("Thông báo", err)
        });
}

//Hien thi form phan cong GV
function HienThiFormPhanCong(malhp, mamh, tenmh, tenhp, sotietdunglop, sotiethethong) {
    if (malhp === '') {
        Swal.fire('Thông báo!', 'Không tìm thấy được dữ liệu. <b style ="color: red">Vui lòng nhấn F5 để tải lại.</b>', 'warning');
        return;
    }

    $.ajax({
        type: 'GET',
        url: '/Professor/PhanCongGiangVien?malhp=' + malhp,
        async: true,
        dataType: 'html',
        success: function (html) {
            $('#ModalPhanCongGiangDayContent').html(html);

            $('#mamhModal').text(mamh);
            $('#tenmhModal').text(tenmh);
            $('#tenhpModal').text(tenhp);
            $('#sotietdunglopModal').text(sotietdunglop);
            $('#sotiethethongModal').text(sotiethethong);
            document.getElementById("malhpModal").value = malhp;

            HienThiDSGV_PhanCong(malhp);
            HienThiDSGV_DaPhanCong(malhp);

            $('#ModalPhanCongGiangDay').modal('show');
        },
        error: function (jqXHR, textStatus, err) {
            Swal.fire("Lỗi", "<b style='color:red'>Vui lòng nhấn Ctrl + F5 để tải lại. </b>", "error");
        }
    });
}

//Hien thi danh sach GV
function HienThiDSGV_PhanCong(malhp) {
    var sort = $("#ddlLocGVTheo").val();

    $("#divDSGV").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/HienThiGVPhanCong?malhp=' + malhp + '&sort=' + sort,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divDSGV").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divDSGV").html(err);
            ShowAlert("Thông báo", err)
        });
}

//Hien thi danh sach GV da phan cong
function HienThiDSGV_DaPhanCong(malhp) {
    $("#divDSGVPC").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Professor/HienThiGVDaPhanCong?malhp=' + malhp,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divDSGVPC").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divDSGVPC").html(err);
            ShowAlert("Thông báo", err)
        });
}

// Luu phan cong
function LuuPhanCongGiangDay(magv) {
    var malhp = $('#malhpModal').val();

    $.ajax({
        type: 'GET',
        url: '/Professor/LuuPhanCongGiangDay',
        async: true,
        data: {
            malhp: malhp,
            magv: magv,
        },
        dataType: 'json',
        success: function (res) {
            if (res.status) {
                document.getElementById("txtSearch").value = "";
                HienThiDSGV_PhanCong(malhp);
                HienThiDSGV_DaPhanCong_TinhSoTiet(malhp);
            }
            else {
                Swal.fire('Thông báo', 'Phân công không thành công!', 'info');
                return;
            }
        },
        error: function (jqXHR, textStatus, err) {
            Swal.fire("Lỗi", "<b style='color:red'>Vui lòng nhấn Ctrl + F5 để tải lại. </b>", "error");
        }
    });
}

function HienThiDSGV_DaPhanCong_TinhSoTiet(malhp) {
    $("#divDSGVPC").showLoading("Loading ....");

    $.ajax({
        type: 'GET',
        url: '/Professor/HienThiGVDaPhanCong?malhp=' + malhp,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#divDSGVPC").html(string(html));

            var sotietdunglop = parseInt($('#sotietdunglopModal').text());
            var sotiethethong = parseInt($('#sotiethethongModal').text());
            var gv = $('#txtDSGV').val().split(",");
            var count = gv.length - 1;

            if (count > 0) {
                var a = sotietdunglop / count;
                var round = Math.ceil(a);
                var b = sotietdunglop;

                for (var i = 0; i < count; i++) {
                    if (gv[i] != "") {
                        if (i == 0) {
                            b = b - round;
                            document.getElementById("txtSoTietDungLop_" + gv[i]).value = round;
                            document.getElementById("txtSoTietHeThong_" + gv[i]).value = sotiethethong;
                        }
                        else if (i == count - 1) {
                            b = Math.floor(b);
                            document.getElementById("txtSoTietDungLop_" + gv[i]).value = b;
                            document.getElementById("txtSoTietHeThong_" + gv[i]).value = "0";
                        }
                        else {
                            b = b - round;
                            document.getElementById("txtSoTietDungLop_" + gv[i]).value = round;
                            document.getElementById("txtSoTietHeThong_" + gv[i]).value = "0";
                        }
                    }
                }
            }
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#divDSGVPC").html(err);
            ShowAlert("Thông báo", err)
        });
}

// Cap nhat phan cong
function CapNhatPhanCongGiangDay() {
    var gv = $('#txtDSGV').val().split(",");
    var malhp = $('#malhpModal').val();
    var sotietdunglop = $('#sotietdunglopModal').text();
    var sotiethethong = $('#sotiethethongModal').text();

    var data = "";
    for (var i = 0; i < gv.length; i++) {
        if (gv[i] != "") {
            var SoTietDungLop = document.getElementById("txtSoTietDungLop_" + gv[i]).value;
            var SoTietHeThong = document.getElementById("txtSoTietHeThong_" + gv[i]).value;

            data += gv[i] + '_' + SoTietDungLop + '_' + SoTietHeThong + ";";
        }
    }

    $.ajax({
        type: 'GET',
        url: '/Professor/CapNhatPhanCongGiangDay',
        async: true,
        data: {
            malhp: malhp,
            dsgv: data,
            sotietdunglop: sotietdunglop,
            sotiethethong: sotiethethong
        },
        dataType: 'json',
        success: function (res) {
            if (res.status) {
                $('#ModalPhanCongGiangDay').modal('hide');
                HienThiLHP_PhanCong();
            }
            else {
                Swal.fire('Thông báo', 'Cập nhật phân công không thành công!', 'info');
                return;
            }
        },
        error: function (jqXHR, textStatus, err) {
            Swal.fire("Lỗi", "<b style='color:red'>Vui lòng nhấn Ctrl + F5 để tải lại. </b>", "error");
        }
    });
}

// Xoa phan cong
function XoaPhanCongGiangDay(magv) {
    var malhp = $('#malhpModal').val();

    $.ajax({
        type: 'GET',
        url: '/Professor/XoaPhanCongGiangDay',
        async: true,
        data: {
            malhp: malhp,
            magv: magv,
        },
        dataType: 'json',
        success: function (res) {
            if (res.status) {
                document.getElementById("txtSearch").value = "";
                HienThiDSGV_PhanCong(malhp);
                HienThiDSGV_DaPhanCong_TinhSoTiet(malhp);
            }
            else {
                Swal.fire('Thông báo', 'Xóa phân công không thành công!', 'info');
                return;
            }
        },
        error: function (jqXHR, textStatus, err) {
            Swal.fire("Lỗi", "<b style='color:red'>Vui lòng nhấn Ctrl + F5 để tải lại. </b>", "error");
        }
    });
}

//Doi mat khau phu huynh
function UpdateParentPW() {
    var pw = $('#txtPW').val();
    var pw1 = $('#txtPW1').val();
    var pw2 = $('#txtPW2').val();
    if (pw == '' || pw1 == "" || pw2 == "") {
        ShowAlert("Chú ý ", "Vui lòng nhập hết thông tin trước khi lưu");
        return;
    }
    if (pw1 != pw2) {
        ShowAlert("Chú ý ", "Mật khẩu nhập lại không chính xác");
        return;
    }

    var parapw = new Object();
    parapw.ID = "";
    parapw.txtPW = pw;
    parapw.txtPW1 = pw1;
    parapw.txtPW2 = pw2;
    $("#btnUpdate").showLoading("...");
    $.ajax({
        type: 'GET',
        url: '/API/Student/parent' + "?t=" + Math.random(),
        async: true,
        data: { pw: pw, pw1: pw1, pw2: pw2 },
        dataType: 'html',
        success: function (html) {
            html = string(html);
            var kq = html.indexOf("...");
            if (kq > -1) { $("#myModal").modal("hide"); }
            ShowAlert("Thông báo", html);
            $("#btnUpdate").html("Cập nhật");
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            $("#btnUpdate").hideLoading();
            $("#btnUpdate").html("Cập nhật");
            ShowAlert("Thông báo", err);
        });
}

function HienFormKhaoSat(ID) {
    $.ajax({
        type: 'GET',
        url: '/Home/HienFormKhaoSat?ID=' + ID,
        async: true,
        dataType: 'html',
        success: function (html) {
            $("#myModal").modal();
            $("#MessagesContent").html(string(html));
        },
    })
    .fail(
        function (jqXHR, textStatus, err) {
            ShowAlert("Thông báo", err)
        });
}

//Show kết quả đăng ký kế hoạch tùng làm 12-4-2022
function ShowKetQuaDangKyKeHoach() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $("#divKetQuaDKKH").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowKetQuaDangKyKeHoach' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#divKetQuaDKKH").html(string(html));
        },
    })
        .fail(
            function (jqXHR, textStatus, err) {
                $("#divKetQuaDKKH").html(err);
            });
}


//Đăng ký ngành phụ 12-7-2022

//Show kết quả đăng ký kế hoạch tùng làm 12-4-2022
function ShowKetQuaDangKyNganhPhu() {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $("#KetQuaDangKyNganhPhu").showLoading("Loading ....");
    $.ajax({
        type: 'GET',
        url: '/Home/ShowKetQuaDangKyNganhPhu' + "?t=" + Math.random(),
        async: true,
        data: { yearStudy: YearStudy, termID: TermID },
        dataType: 'html',
        success: function (html) {
            $("#KetQuaDangKyNganhPhu").html(string(html));
        },
    })
        .fail(
            function (jqXHR, textStatus, err) {
                $("#KetQuaDangKyNganhPhu").html(err);
    });
}

function OnRegisterNganhPhu(StudyProgramID) {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $.post("/Home/OnRegisterNganhPhu", { StudyProgramID: StudyProgramID, YearStudy: YearStudy, TermID: TermID}, function (result) {
        var html = result.replace('"', '');
        var kq = result.indexOf("...");
        if (kq > -1) {
            alert("Cập nhật hoạt động thành công");
        }
        else {
            ShowAlert("Thông báo", html);
            ShowKetQuaDangKyNganhPhu();
        }
    });
}
function OnCancelNganhPhu(StudyProgramID) {
    var YearStudy = $("#ddlYearStudy").val();
    var TermID = $("#ddlTermID").val();
    $.post("/Home/OnCancelNganhPhu", { StudyProgramID: StudyProgramID, YearStudy: YearStudy, TermID: TermID }, function (result) {
        var html = result.replace('"', '');
        var kq = result.indexOf("...");
        if (kq > -1) {
            alert("Cập nhật hoạt động thành công");
        }
        else {
            ShowAlert("Thông báo", html);
            ShowKetQuaDangKyNganhPhu();
        }
    });
}
