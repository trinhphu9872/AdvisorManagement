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

  
        //public ExcelPackage ExportTemplateAdmin(ExcelPackage pck, List<PlanAdvisor> template, int? year)
        //{
        //    pck.Workbook.Properties.Title = year.ToString();
        //    var ws = pck.Workbook.Worksheets.Add(year.ToString());

        //    ws.Cells["A:AZ"].Style.Font.Name = "Times New Roman";
        //    ws.Cells["A:AZ"].Style.Font.Size = 13;
        //    ws.Column(1).Width = 8;
        //    ws.Column(2).Width = 45;
        //    ws.Column(3).Width = 13.67;
        //    ws.Column(4).Width = 13.67;
        //    ws.Column(5).Width = 13.67;
        //    ws.Column(6).Width = 38;
        //    ws.Column(7).Width = 35;
        //    ws.Column(8).Width = 10;
        //    ws.Row(5).Height = 22.80;
        //    ws.Row(6).Height = 22.80;
        //    ws.Column(2).Style.WrapText = true;
        //    ws.Column(3).Style.WrapText = true;
        //    ws.Column(4).Style.WrapText = true;
        //    ws.Column(5).Style.WrapText = true;
        //    ws.Column(6).Style.WrapText = true;
        //    ws.Column(7).Style.WrapText = true;
        //    ws.Column(8).Style.WrapText = true;
        //    ws.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //    ws.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //    ws.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //    ws.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //    ws.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //    ws.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //    ws.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //    ws.Column(8).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //    ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Row(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[1, 1, 1, 2].Merge = true;
        //    ws.Cells[1, 1, 1, 2].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
        //    ws.Cells[1, 1, 1, 2].Style.Font.Size = 12;
        //    ws.Cells[2, 1, 2, 2].Merge = true;
        //    ws.Cells[2, 1, 2, 2].Value = "KHOA: CÔNG NGHỆ THÔNG TIN";
        //    ws.Cells[2, 1, 2, 2].Style.Font.Size = 12;
        //    ws.Cells[2, 1, 2, 2].Style.Font.Bold = true;
        //    ws.Cells[3, 1, 3, 8].Merge = true;
        //    ws.Cells[3, 1, 3, 8].Value = "BÁO CÁO KẾ HOẠCH HOẠT ĐỘNG CỐ VẤN HỌC TẬP";
        //    ws.Cells[3, 1, 3, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[3, 1, 3, 8].Style.Font.Size = 14;
        //    ws.Cells[3, 1, 3, 8].Style.Font.Bold = true;
        //    ws.Cells[4, 1, 4, 8].Merge = true;
        //    ws.Cells[4, 1, 4, 8].Value = "NĂM HỌC " + (year - 1) + " - " + year;
        //    ws.Cells[4, 1, 4, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[4, 1, 4, 8].Style.Font.Size = 14;
        //    ws.Cells[4, 1, 4, 8].Style.Font.Bold = true;
        //    ws.Cells[5, 1, 6, 1].Merge = true;
        //    ws.Cells[5, 1, 6, 1].Value = "STT";
        //    ws.Cells[5, 1, 6, 1].Style.Font.Bold = true;
        //    ws.Cells[5, 2, 6, 2].Merge = true;
        //    ws.Cells[5, 2, 6, 2].Value = "NỘI DUNG";
        //    ws.Cells[5, 2, 6, 2].Style.Font.Bold = true;
        //    ws.Cells[5, 3, 5, 5].Merge = true;
        //    ws.Cells[5, 3, 5, 5].Value = "THỜI GIAN DỰ KIẾN";
        //    ws.Cells[5, 3, 5, 5].Style.Font.Bold = true;
        //    ws.Cells["C6"].Value = "Học kỳ 1";
        //    ws.Cells["C6"].Style.Font.Bold = true;
        //    ws.Cells["D6"].Value = "Học kỳ 2";
        //    ws.Cells["D6"].Style.Font.Bold = true;
        //    ws.Cells["E6"].Value = "Học kỳ 3";
        //    ws.Cells["E6"].Style.Font.Bold = true;
        //    ws.Cells[5, 6, 6, 6].Merge = true;
        //    ws.Cells[5, 6, 6, 6].Value = "MÔ TẢ CÔNG VIỆC";
        //    ws.Cells[5, 6, 6, 6].Style.Font.Bold = true;
        //    ws.Cells[5, 7, 6, 7].Merge = true;
        //    ws.Cells[5, 7, 6, 7].Value = "TÀI NGUYÊN CHUẨN BỊ";
        //    ws.Cells[5, 7, 6, 7].Style.Font.Bold = true;
        //    ws.Cells[5, 8, 6, 8].Merge = true;
        //    ws.Cells[5, 8, 6, 8].Value = "GHI CHÚ";
        //    ws.Cells[5, 8, 6, 8].Style.Font.Bold = true;
        //    int rowStart = 7;
        //    bool check = false;
        //    int stt = 0;
        //    string content = "";
        //    string source = "";
        //    int countSTT = 1;
        //    int countContent = 1;
        //    int countSource = 1;
        //    foreach (var item in template)
        //    {
        //        if (item.number_title == stt)
        //        {
        //            if (item.content == content)
        //            {
        //                if (item.source == source)
        //                {
        //                    ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
        //                    countSTT++;
        //                    countContent++;
        //                    countSource++;
        //                }
        //                else
        //                {
        //                    ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
        //                    ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
        //                    ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
        //                    countSTT++;
        //                    countContent++;
        //                }
        //            }
        //            else
        //            {
        //                ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
        //                ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
        //                ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
        //                ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
        //                countSTT++;
        //            }
        //        }
        //        else
        //        {
        //            if (item.number_title == 3)
        //            {
        //                ws.InsertRow(rowStart - countSTT, 1);
        //                ws.Row(rowStart - countSTT).Height = 31.20;
        //                ws.Cells[rowStart - countSTT, 2].Value = "Tổ chức họp lớp SV theo định kỳ/đột xuất.";
        //                ws.Cells[rowStart - countSTT, 3, rowStart - countSTT, 8].Merge = true;

        //                ws.InsertRow(rowStart + countSTT - 1, 1);
        //                ws.Row(rowStart + countSTT - 1).Height = 31.20;
        //                ws.Cells[rowStart + countSTT - 1, 2].Value = "...";
        //                ws.Cells[rowStart + countSTT - 1, 6].Value = "...";
        //                rowStart = rowStart + 2;
        //            }
        //            if (countSTT > 1)
        //            {
        //                if (item.number_title == 3)
        //                {
        //                    ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Merge = true;
        //                    ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Value = ws.Cells[rowStart - countSTT - 1, 1].Value;
        //                    ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                    countSTT = 1;
        //                }
        //                else
        //                {
        //                    ws.Cells[rowStart - countSTT, 1, rowStart - 1, 1].Merge = true;
        //                    countSTT = 1;
        //                }
        //            }
        //            if (countContent > 1)
        //            {
        //                ws.Cells[rowStart - countContent, 2, rowStart - 1, 2].Merge = true;
        //                countContent = 1;
        //            }
        //            if (countSource > 1)
        //            {
        //                ws.Cells[rowStart - countSource, 7, rowStart - 1, 7].Merge = true;
        //                countSource = 1;
        //            }
        //            ws.Cells[string.Format("A{0}", rowStart)].Value = item.number_title;
        //            ws.Cells[string.Format("A{0}", rowStart)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
        //            ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
        //            ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
        //            ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
        //        }
        //        rowStart++;
        //        stt = (int)item.number_title;
        //        content = item.content;
        //        source = item.source;

        //    }

        //    ws.Column(10).Style.Font.Size = 13;
        //    /* ws.Cells["A:AZ"].AutoFitColumns();*/
        //    ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //    ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //    ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //    ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //    ws.Cells[rowStart + 2, 2].Value = "Trưởng khoa";
        //    ws.Cells[rowStart + 2, 2].Style.Font.Bold = true;

        //    ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Merge = true;
        //    ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Value = "TP. Hồ Chí Minh, ngày   tháng   năm " + (year - 1);
        //    ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Style.Font.Italic = true;
        //    ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //    ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Merge = true;
        //    ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Value = "Cố vấn học tập";
        //    ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Style.Font.Bold = true;
        //    ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Merge = true;
        //    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Value = "Lưu ý: CVHT cụ thể hóa các công việc trong phần mô tả theo đặc thù đơn vị và kế hoạch cụ thể của cá nhân, làm căn cứ thực hiện công tác này trong NH " + (year - 1) + " - " + year + ".";
        //    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.Font.Bold = true;
        //    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.Font.Italic = true;
        //    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.WrapText = true;
        //    ws.Row(rowStart + 8).Height = 31.80;
        //    pck.Save();
        //    return pck;
        //}

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