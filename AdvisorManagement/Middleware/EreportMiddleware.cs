using AdvisorManagement.Models;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvisorManagement.Models.ViewModel;

namespace AdvisorManagement.Middleware
{

    public class EreportMiddleware
    {
        private AccountMiddleware serviceAccount = new AccountMiddleware();


        public ExcelPackage ExportEvalUser(ExcelPackage pck, List<AdminCheckEvol> evolAll, string title, string us_name, int? year)
        {
            pck.Workbook.Properties.Title = title;
            var ws = pck.Workbook.Worksheets.Add(title);
            ws.Cells["A:AZ"].Style.Font.Name = "Times New Roman";
            ws.Cells["A:AZ"].Style.Font.Size = 13;
            ws.Column(1).Width = 8; // stt
            ws.Column(2).Width = 30; // Mã lớp 
            ws.Column(3).Width = 30; //Cố vấn học tập
            ws.Column(4).Width = 30; // Phần mềm đánh giá
            ws.Column(5).Width = 30; // Phần mềm đánh giá
            ws.Column(6).Width = 30; // Khoa đánh giá 
            ws.Column(7).Width = 30; // Ghi chú


            ws.Column(2).Style.WrapText = true;
            ws.Column(3).Style.WrapText = true;
            ws.Column(4).Style.WrapText = true;
            ws.Column(5).Style.WrapText = true;
            ws.Column(6).Style.WrapText = true;
            ws.Column(7).Style.WrapText = true;

            ws.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Row(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //header
            ws.Cells[1, 1, 1, 2].Merge = true;
            ws.Cells[1, 1, 1, 2].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
            ws.Cells[1, 1, 1, 2].Style.Font.Size = 12;
            ws.Cells[1, 1, 1, 2].Style.Font.Bold = true;

            // 
            ws.Cells[2, 1, 2, 2].Merge = true;
            ws.Cells[2, 1, 2, 2].Value = "KHOA: CÔNG NGHỆ THÔNG TIN";
            ws.Cells[2, 1, 2, 2].Style.Font.Size = 12;
            ws.Cells[2, 1, 2, 2].Style.Font.Bold = true;
            //
            //ws.Cells[3, 1, 3, 2].Merge = true;
            //ws.Cells[3, 1, 3, 2].Value = "Họ và tên giảng viên "+ serviceAccount.getTextName(us_name);
            //ws.Cells[3, 1, 3, 2].Style.Font.Size = 12;
            //ws.Cells[3, 1, 3, 2].Style.Font.Bold = true;
            //
            //
            //ws.Cells[4, 1, 4, 7].Merge = true;
            //ws.Cells[4, 1, 4, 7].Value = "BÁO CÁO TỔNG KẾT CÔNG TÁC CỐ VẤN HỌC TẬP";
            //ws.Cells[4, 1, 4, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //ws.Cells[4, 1, 4, 7].Style.Font.Size = 14;
            //ws.Cells[4, 1, 4, 7].Style.Font.Bold = true;
            //
            ws.Cells[5, 1, 5, 7].Merge = true;
            ws.Cells[5, 1, 5, 7].Value = "BÁO CÁO TỔNG KẾT CÔNG TÁC CỐ VẤN HỌC TẬP";
            ws.Cells[5, 1, 5, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[5, 1, 5, 7].Style.Font.Size = 14;
            ws.Cells[5, 1, 5, 7].Style.Font.Bold = true;
            //
            ws.Cells[6, 1, 6, 7].Merge = true;
            ws.Cells[6, 1, 6, 7].Value = "NĂM HỌC " + (year - 1) + " - " + year;
            ws.Cells[6, 1, 6, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[6, 1, 6, 7].Style.Font.Size = 14;
            ws.Cells[6, 1, 6, 7].Style.Font.Bold = true;

            // title
            // stt
            ws.Cells[7, 1, 7, 1].Merge = true;
            ws.Cells[7, 1, 7, 1].Value = "STT";
            ws.Cells[7, 1, 7, 1].Style.Font.Bold = true;
            ws.Cells[7, 2, 7, 2].Merge = true;
            // nd
            ws.Cells[7, 2, 7, 2].Value = "MÃ LỚP";
            ws.Cells[7, 2, 7, 2].Style.Font.Bold = true;
            ws.Cells[7, 3, 7, 3].Merge = true;
            // mô tả
            ws.Cells[7, 3, 7, 3].Value = "HỌ VÀ TÊN CVHT";
            ws.Cells[7, 3, 7, 3].Style.Font.Bold = true;
            ws.Cells[7, 4, 7, 4].Merge = true;
            // tai nguye
            ws.Cells[7, 4, 7, 4].Value = "PHẦN MỀM ĐÁNH GIÁ";
            ws.Cells[7, 4, 7, 4].Style.Font.Bold = true;
            ws.Cells[7, 5, 7, 5].Merge = true;
            // tai nguye
            ws.Cells[7, 5, 7, 5].Value = "CVHT TỰ ĐÁNH GIÁ";
            ws.Cells[7, 5, 7, 5].Style.Font.Bold = true;
            ws.Cells[7, 6, 7, 6].Merge = true;
            // tai nguye
            ws.Cells[7, 6, 7, 6].Value = "KHOA ĐÁNH GIÁ";
            ws.Cells[7, 6, 7, 6].Style.Font.Bold = true;
            ws.Cells[7, 7, 7, 7].Merge = true;

            // tai nguye
            ws.Cells[7, 7, 7, 7].Value = "GHI CHÚ";
            ws.Cells[7, 7, 7, 7].Style.Font.Bold = true;
            ws.Cells[7, 8, 7, 8].Merge = true;


            int rowStart = 8;
            int index = 1;
            foreach (var item in evolAll)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = index;
                ws.Cells[string.Format("A{0}", rowStart)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.class_id;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.name_advisor;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.evol_sys;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.eval_advisor;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.eval_admin;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.note;
                index++;
                rowStart++;

            }


            ws.Column(10).Style.Font.Size = 13;

            ws.Cells[5, 1, rowStart - 1, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;

            ws.Cells[rowStart + 2, 3, rowStart + 2, 4].Merge = true;
            ws.Cells[rowStart + 2, 3, rowStart + 2, 4].Style.Font.Bold = true;
            ws.Cells[rowStart + 2, 3, rowStart + 2, 4].Value = "Ban chủ nhiệm khoa";
            ws.Cells[rowStart + 2, 3, rowStart + 2, 4].Style.Font.Bold = true;
            ws.Cells[rowStart + 2, 3, rowStart + 2, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


            ws.Cells[rowStart + 6, 3, rowStart + 6, 4].Merge = true;
            ws.Cells[rowStart + 6, 3, rowStart + 6, 4].Value = "TS. BÙI MINH PHỤNG";
            ws.Cells[rowStart + 6, 3, rowStart + 6, 4].Style.Font.Italic = false;
            ws.Cells[rowStart + 6, 3, rowStart + 6, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;




            ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Merge = true;
            ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Value = "TP. Hồ Chí Minh, ngày   tháng   năm " + (year );
            ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Style.Font.Italic = true;
            ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Merge = true;
            ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Value = "Người tổng hợp";
            ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Style.Font.Bold = true;
            ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            ws.Cells[rowStart + 6, 6, rowStart + 6, 8].Merge = true;
            ws.Cells[rowStart + 6, 6, rowStart + 6, 8].Value = serviceAccount.getTextName(us_name);
            ws.Cells[rowStart + 6, 6, rowStart + 6, 8].Style.Font.Italic = false;
            ws.Cells[rowStart + 6, 6, rowStart + 6, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;






            //ws.Row(rowStart + 8).Height = 31.80;
            pck.Save();
            return pck;
        }
    


    public ExcelPackage ExportTemplate(ExcelPackage pck,List<ReportCustom> reports , string class_code, string us_name, int? year, string danhGia)
        {
            pck.Workbook.Properties.Title = class_code;
            var ws = pck.Workbook.Worksheets.Add(class_code);
            ws.Cells["A:AZ"].Style.Font.Name = "Times New Roman";
            ws.Cells["A:AZ"].Style.Font.Size = 13;
            ws.Column(1).Width = 8; // stt
            ws.Column(2).Width = 35; // nội dung
            ws.Column(3).Width = 35; //Mô tả công việc
            ws.Column(4).Width = 35; // tài nguyên chuẩn bị
            ws.Column(5).Width = 30; // hồ sơ
            ws.Column(6).Width = 30; // tiêu chí
            //ws.Column(7).Width = 20; // trang thái

            ws.Column(2).Style.WrapText = true;
            ws.Column(3).Style.WrapText = true;
            ws.Column(4).Style.WrapText = true;
            ws.Column(5).Style.WrapText = true;
            ws.Column(6).Style.WrapText = true;
            //ws.Column(7).Style.WrapText = true;

            ws.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            //ws.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;          

            ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Row(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //header
            ws.Cells[1, 1, 1, 2].Merge = true;
            ws.Cells[1, 1, 1, 2].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
            ws.Cells[1, 1, 1, 2].Style.Font.Size = 12;
            // 
            ws.Cells[2, 1, 2, 2].Merge = true;
            ws.Cells[2, 1, 2, 2].Value = "KHOA: CÔNG NGHỆ THÔNG TIN";
            ws.Cells[2, 1, 2, 2].Style.Font.Size = 12;
            ws.Cells[2, 1, 2, 2].Style.Font.Bold = true;
            //
            //ws.Cells[3, 1, 3, 2].Merge = true;
            //ws.Cells[3, 1, 3, 2].Value = "Họ và tên giảng viên "+ serviceAccount.getTextName(us_name);
            //ws.Cells[3, 1, 3, 2].Style.Font.Size = 12;
            //ws.Cells[3, 1, 3, 2].Style.Font.Bold = true;
            //
            //
            ws.Cells[4, 1, 4, 6].Merge = true;
            ws.Cells[4, 1, 4, 6].Value = "BÁO CÁO KẾ HOẠCH HOẠT ĐỘNG CỐ VẤN HỌC TẬP";
            ws.Cells[4, 1, 4, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[4, 1, 4, 6].Style.Font.Size = 14;
            ws.Cells[4, 1, 4, 6].Style.Font.Bold = true;
            //
            ws.Cells[5, 1, 5, 6].Merge = true;
            ws.Cells[5, 1, 5, 6].Value = "NĂM HỌC " + (year - 1) + " - " + year;
            ws.Cells[5, 1, 5, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[5, 1, 5, 6].Style.Font.Size = 14;
            ws.Cells[5, 1, 5, 6].Style.Font.Bold = true;
            //
            ws.Cells[6, 1, 6, 6].Merge = true;
            ws.Cells[6, 1, 6, 6].Value = "Họ và tên giảng viên " + serviceAccount.getTextName(us_name);
            ws.Cells[6, 1, 6, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            ws.Cells[6, 1, 6, 6].Style.Font.Size = 14;
            ws.Cells[6, 1, 6, 6].Style.Font.Bold = false;

        

            // title
            // stt
            ws.Cells[7, 1, 7, 1].Merge = true;
            ws.Cells[7, 1, 7, 1].Value = "STT";
            ws.Cells[7, 1, 7, 1].Style.Font.Bold = true;
            ws.Cells[7, 2, 7, 2].Merge = true;
            // nd
            ws.Cells[7, 2, 7, 2].Value = "NỘI DUNG";
            ws.Cells[7, 2, 7, 2].Style.Font.Bold = true;
            ws.Cells[7, 3, 7, 3].Merge = true;
            // mô tả
            ws.Cells[7, 3, 7, 3].Value = "MÔ TẢ CÔNG VIỆC";
            ws.Cells[7, 3, 7, 3].Style.Font.Bold = true;
            ws.Cells[7, 4, 7, 4].Merge = true;
            // tai nguye
            ws.Cells[7, 4, 7, 4].Value = "TÀI NGUYÊN CHUẨN BỊ";
            ws.Cells[7, 4, 7, 4].Style.Font.Bold = true;
            ws.Cells[7, 5, 7, 5].Merge = true;
            // tai nguye
            ws.Cells[7, 5, 7, 5].Value = "HỒ SƠ MINH CHỨNG";
            ws.Cells[7, 5, 7, 5].Style.Font.Bold = true;
            ws.Cells[7, 6, 7, 6].Merge = true;
            // tai nguye
            ws.Cells[7, 6, 7, 6].Value = "TRẠNG THÁI";
            ws.Cells[7, 6, 7, 6].Style.Font.Bold = true;
            ws.Cells[7, 7, 7, 7].Merge = true;
            //// tai nguye
            //ws.Cells[6, 7, 6, 7].Value = "TRẠNG THÁI";
            //ws.Cells[6, 7, 6, 7].Style.Font.Bold = true;

            int rowStart = 8;
            bool check = false;
            int stt = 0;
            string content = "";
            string source = "";
            int countSTT = 1;
            int countContent = 1;
            int countSource = 1;
            foreach (var item in reports)
            {
                if (item.number_title == stt)
                {
                    if(item.content == content)
                    {
                        if (item.source == source)
                        {
                            ws.Cells[string.Format("C{0}", rowStart)].Value = item.describe;
                            ws.Cells[string.Format("D{0}", rowStart)].Value = item.source;
                            ws.Cells[string.Format("E{0}", rowStart)].Value = item.group_file;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = item.status;
                            //ws.Cells[string.Format("G{0}", rowStart)].Value = item.status;
                            countSTT++;
                            countContent++;
                            countSource++;
                        }
                        else
                        {
                            ws.Cells[string.Format("C{0}", rowStart)].Value = item.describe;
                            ws.Cells[string.Format("D{0}", rowStart)].Value = item.source;
                            ws.Cells[string.Format("E{0}", rowStart)].Value = item.group_file;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = item.status;
                            //ws.Cells[string.Format("G{0}", rowStart)].Value = item.status;
                            countSTT++;
                            countContent++;
                        }
                    }
                    else
                    {
                        if (item.source == source)
                        {
                            ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
                            ws.Cells[string.Format("C{0}", rowStart)].Value = item.describe;
                            ws.Cells[string.Format("D{0}", rowStart)].Value = item.source;
                            ws.Cells[string.Format("E{0}", rowStart)].Value = item.group_file;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = item.status;
                            //ws.Cells[string.Format("G{0}", rowStart)].Value = item.status;
                            countSTT++;
                            countContent++;
                            countSource++;
                        }
                        else
                        {
                            ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
                            ws.Cells[string.Format("C{0}", rowStart)].Value = item.describe;
                            ws.Cells[string.Format("D{0}", rowStart)].Value = item.source;
                            ws.Cells[string.Format("E{0}", rowStart)].Value = item.group_file;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = item.status;
                            //ws.Cells[string.Format("G{0}", rowStart)].Value = item.status;
                            countSTT++;
                            countContent++;
                        }                                               
                    }
                }
                else
                {
                    if(countSTT > 1)
                    {
                        ws.Cells[rowStart - countSTT, 1, rowStart - 1, 1].Merge = true;
                        countSTT = 1;
                    }
                    if (countContent > 1)
                    {
                        ws.Cells[rowStart - countContent, 2, rowStart - 1, 2].Merge = true;
                        countContent = 1;
                    }
                    if (countSource > 1)
                    {
                        ws.Cells[rowStart - countSource, 4, rowStart - 1, 4].Merge = true;
                        countSource = 1;
                    }
                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.number_title;
                    ws.Cells[string.Format("A{0}", rowStart)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.describe;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.source;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.group_file;
                    ws.Cells[string.Format("F{0}", rowStart)].Value = item.status;
                    //ws.Cells[string.Format("G{0}", rowStart)].Value = item.status;
                }              
                rowStart++;
                stt = (int)item.number_title;
                content = item.content;
                source = item.source;
            }


            ws.Column(10).Style.Font.Size = 13;
            /* ws.Cells["A:AZ"].AutoFitColumns();*/
            ws.Cells[5, 1, rowStart - 1, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowStart + 1, 2].Value = "Giảng viên tự xếp loại: "  + danhGia;
            ws.Cells[rowStart + 1, 2].Style.Font.Bold = true;
            ws.Cells[rowStart + 2, 2].Value = "Trưởng khoa";
            ws.Cells[rowStart + 2, 2].Style.Font.Bold = true;

            ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Merge = true;
            ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Value = "TP. Hồ Chí Minh, ngày   tháng   năm " + (year - 1);
            ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Style.Font.Italic = true;
            ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Merge = true;
            ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Value = "Cố vấn học tập";
            ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Style.Font.Bold = true;
            ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Merge = true;
            ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Value = "Lưu ý: CVHT cụ thể hóa các công việc trong phần mô tả theo đặc thù đơn vị và kế hoạch cụ thể của cá nhân, làm căn cứ thực hiện công tác này trong NH " + (year - 1) + " - " + year + ".";
            ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.Font.Bold = true;
            ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.Font.Italic = true;
            ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.WrapText = true;



            //ws.Row(rowStart + 8).Height = 31.80;
            pck.Save();
            return pck;
        }
    }
}