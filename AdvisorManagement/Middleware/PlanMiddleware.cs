using AdvisorManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IdentityModel.Protocols.WSTrust;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using Microsoft.Ajax.Utilities;
using Spire.Pdf.Fields;
using AdvisorManagement.Models.ViewModel;

namespace AdvisorManagement.Middleware
{
    public class PlanMiddleware
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        public object getListProof(string user_email, int id)
        {
            var id_account = db.AccountUser.FirstOrDefault(x=>x.email== user_email).id;
            if (id_account != null)
            {
                var listProof = (from pq in db.AccountUser
                                 join pr in db.ProofPlan on pq.id equals pr.id_creator
                                 where pr.id_titleplan == id && id_account == pr.id_creator
                                 select new Models.ViewModel.ListProofPlan
                                 {
                                     id = pr.id,
                                     content = pr.content,
                                     proof = pr.file_proof,
                                     create_time = pr.create_time,
                                     semester = pr.semester,
                                     status = pr.status,
                                     creator = pq.user_name
                                 }).OrderByDescending(x => x.create_time).ToList();
                return listProof;
            }            
            return null;
        }

        public object getListProofAdmin(string semester, int id_class)
        {
  
            if (semester == "0")
                {
                    var listProof = (from pq in db.AccountUser
                                     join pr in db.ProofPlan on pq.id equals pr.id_creator
                                     join pl in db.PlanClass on pr.id_titleplan equals pl.id
                                     where pr.id_titleplan == pl.id && pl.id_class == id_class
                                     select new Models.ViewModel.ListProofPlan
                                     {
                                         id = pr.id,
                                         content = pr.content,
                                         title = pl.content,
                                         proof = pr.file_proof,
                                         create_time = pr.create_time,
                                         semester = pr.semester,
                                         status = pr.status,
                                         creator = pq.user_name
                                     }).OrderByDescending(x => x.create_time).ToList();
                    return listProof;
            }
            else
            {
                var listProof = (from pq in db.AccountUser
                                 join pr in db.ProofPlan on pq.id equals pr.id_creator
                                 join pl in db.PlanClass on pr.id_titleplan equals pl.id
                                 where pr.id_titleplan == pl.id && pl.id_class == id_class && pr.semester == semester
                                 select new Models.ViewModel.ListProofPlan
                                 {
                                     id = pr.id,
                                     content = pr.content,
                                     title = pl.content,
                                     proof = pr.file_proof,
                                     create_time = pr.create_time,
                                     semester = pr.semester,
                                     status = pr.status,
                                     creator = pq.user_name
                                 }).OrderByDescending(x => x.create_time).ToList();
                return listProof;
            }
               

            return null;
        }

        public object getListPlanSatus(string year)
        {
            if (year == "0")
            {
                var listPlan = (from pq in db.AccountUser
                                 join vl in db.VLClass on pq.user_code equals vl.advisor_code
                                 join ps in db.PlanStatus on vl.id equals ps.id_class
                                 join sp in db.StatusPlan on ps.id_status equals sp.id
                                 where vl.id == ps.id_class && sp.status_name != "Đang làm"
                                 select new Models.ViewModel.ListPlanSubmited
                                 {
                                     id = ps.id,
                                     id_class = vl.id,
                                     user_code = pq.user_code,
                                     user_name = pq.user_name,
                                     class_code = vl.class_code,
                                     semester = vl.semester_name,
                                     status = sp.status_name
                                 }).OrderByDescending(x=>x.status).ToList();
                return listPlan;
            }
            else
            {
                var listPlan = (from pq in db.AccountUser
                                join vl in db.VLClass on pq.user_code equals vl.advisor_code
                                join ps in db.PlanStatus on vl.id equals ps.id_class
                                join sp in db.StatusPlan on ps.id_status equals sp.id
                                where vl.id == ps.id_class && vl.semester_name == year && sp.status_name != "Đang làm"
                                select new Models.ViewModel.ListPlanSubmited
                                {
                                    id = ps.id,
                                    user_code = pq.user_code,
                                    id_class = vl.id,
                                    user_name = pq.user_name,
                                    class_code = vl.class_code,
                                    semester = vl.semester_name,
                                    status = sp.status_name
                                }).OrderByDescending(x => x.status).ToList();
                return listPlan;
            }
        }

        public object getListAdvisorUnfinished()
        {
            var listPlan = (from pq in db.AccountUser
                            join vl in db.VLClass on pq.user_code equals vl.advisor_code
                            join ps in db.PlanStatus on vl.id equals ps.id_class
                            join sp in db.StatusPlan on ps.id_status equals sp.id
                            where vl.id == ps.id_class && sp.status_name == "Đang làm"
                            select new Models.ViewModel.ListAdvisorUnfinished
                            {
                                id = ps.id,
                                id_class = vl.id,
                                user_code = pq.user_code,
                                user_name = pq.user_name,
                                class_code = vl.class_code,
                                email = pq.email,
                            }).ToList();

            List<ListAdvisorUnfinished> advisor1 = new List<ListAdvisorUnfinished>();         
                var user = "";
                var class_code = "";
                foreach (var item in listPlan)
                {                
                    if (user == item.user_code)
                    {
                        class_code += ", "+ item.class_code;                        
                        var b = advisor1.FirstOrDefault(x=>x.user_code == item.user_code);
                        advisor1.Remove(b);
                        item.class_code = class_code;
                     }                   
                    else
                    {
                        item.class_code = item.class_code;
                    }
                    advisor1.Add(item);
                    class_code = item.class_code;
                    user = item.user_code;
                }
            return advisor1;
        }

        
        public int getYear()
        {
            int year;
            DateTime date = DateTime.Now.Date;
            if (date.Month < 9)
            {
                year = date.Year;
            }
            else
            {
                year = date.Year + 1;
            }
            return year;
        }

        public string getDecribe(string decribe)
        {
            string chuoi = decribe;
            var split = chuoi.Split('-');
            var result = "";
            if (split.Length > 1)
            {
                for (var i = 1; i < split.Length; i++)
                {
                    result += "\n" + "- " + split[i];
                    result.Trim();
                }
                return "'" + result.Trim();
            }
            else
            {
                result = decribe;
                return result;
            }

        }

        static Regex ConvertToUnsign_rg = null;
        public string ConvertToUnsign(string strInput)
        {
            if (ReferenceEquals(ConvertToUnsign_rg, null))
            {
                ConvertToUnsign_rg = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            }
            var temp = strInput.Normalize(NormalizationForm.FormD);
            return ConvertToUnsign_rg.Replace(temp, string.Empty).Replace("đ", "d").Replace("Đ", "D").Replace(" ","");
        }

        public void AssignmentTemplate(int id_class, int year)
        {
            var templateClass = db.PlanClass.Where(x => x.year == year && x.id_class == id_class).ToList();
            foreach (var item in templateClass)
            {
                var temp = db.PlanClass.Find(item.id);
                db.PlanClass.Remove(temp);
                db.SaveChanges();
            }

            var template = db.PlanAdvisor.Where(x => x.year == year).OrderBy(x => x.number_title).ToList();
            PlanClass planClass = new PlanClass();
            foreach (var item in template)
            {
                planClass.year = item.year;
                planClass.number_title = item.number_title;
                planClass.content = item.content;
                planClass.describe = item.describe;
                planClass.source = item.source;
                planClass.note = item.note;
                planClass.id_class = id_class;
                db.PlanClass.Add(planClass);
                db.SaveChanges();
            }
        }

        public void PlanStatus(int id_class)
        {
            if (db.PlanStatus.Where(x => x.id_class == id_class).Count() == 0)
            {
                PlanStatus planStatus = new PlanStatus();
                planStatus.id_class = id_class;
                planStatus.id_status = 1;
                db.PlanStatus.Add(planStatus);
                db.SaveChanges();
            }

        }


        public ExcelPackage ExportTemplateAdmin(ExcelPackage pck, List<PlanAdvisor> template,int? year)
        {
            pck.Workbook.Properties.Title = year.ToString();
            var ws = pck.Workbook.Worksheets.Add(year.ToString());

            ws.Cells["A:AZ"].Style.Font.Name = "Times New Roman";
            ws.Cells["A:AZ"].Style.Font.Size = 13;
            ws.Column(1).Width = 8;
            ws.Column(2).Width = 45;
            ws.Column(3).Width = 13.67;
            ws.Column(4).Width = 13.67;
            ws.Column(5).Width = 13.67;
            ws.Column(6).Width = 38;
            ws.Column(7).Width = 35;
            ws.Column(8).Width = 10;
            ws.Row(5).Height = 22.80;
            ws.Row(6).Height = 22.80;
            ws.Column(2).Style.WrapText = true;
            ws.Column(3).Style.WrapText = true;
            ws.Column(4).Style.WrapText = true;
            ws.Column(5).Style.WrapText = true;
            ws.Column(6).Style.WrapText = true;
            ws.Column(7).Style.WrapText = true;
            ws.Column(8).Style.WrapText = true;
            ws.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(8).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Row(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[1, 1, 1, 2].Merge = true;
            ws.Cells[1, 1, 1, 2].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
            ws.Cells[1, 1, 1, 2].Style.Font.Size = 12;
            ws.Cells[2, 1, 2, 2].Merge = true;
            ws.Cells[2, 1, 2, 2].Value = "KHOA: CÔNG NGHỆ THÔNG TIN";
            ws.Cells[2, 1, 2, 2].Style.Font.Size = 12;
            ws.Cells[2, 1, 2, 2].Style.Font.Bold = true;
            ws.Cells[3, 1, 3, 8].Merge = true;
            ws.Cells[3, 1, 3, 8].Value = "KẾ HOẠCH HOẠT ĐỘNG CỐ VẤN HỌC TẬP";
            ws.Cells[3, 1, 3, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[3, 1, 3, 8].Style.Font.Size = 14;
            ws.Cells[3, 1, 3, 8].Style.Font.Bold = true;
            ws.Cells[4, 1, 4, 8].Merge = true;
            ws.Cells[4, 1, 4, 8].Value = "NĂM HỌC " + (year - 1) + " - " + year;
            ws.Cells[4, 1, 4, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[4, 1, 4, 8].Style.Font.Size = 14;
            ws.Cells[4, 1, 4, 8].Style.Font.Bold = true;
            ws.Cells[5, 1, 6, 1].Merge = true;
            ws.Cells[5, 1, 6, 1].Value = "STT";
            ws.Cells[5, 1, 6, 1].Style.Font.Bold = true;
            ws.Cells[5, 2, 6, 2].Merge = true;
            ws.Cells[5, 2, 6, 2].Value = "NỘI DUNG";
            ws.Cells[5, 2, 6, 2].Style.Font.Bold = true;
            ws.Cells[5, 3, 5, 5].Merge = true;
            ws.Cells[5, 3, 5, 5].Value = "THỜI GIAN DỰ KIẾN";
            ws.Cells[5, 3, 5, 5].Style.Font.Bold = true;
            ws.Cells["C6"].Value = "Học kỳ 1";
            ws.Cells["C6"].Style.Font.Bold = true;
            ws.Cells["D6"].Value = "Học kỳ 2";
            ws.Cells["D6"].Style.Font.Bold = true;
            ws.Cells["E6"].Value = "Học kỳ 3";
            ws.Cells["E6"].Style.Font.Bold = true;
            ws.Cells[5, 6, 6, 6].Merge = true;
            ws.Cells[5, 6, 6, 6].Value = "MÔ TẢ CÔNG VIỆC";
            ws.Cells[5, 6, 6, 6].Style.Font.Bold = true;
            ws.Cells[5, 7, 6, 7].Merge = true;
            ws.Cells[5, 7, 6, 7].Value = "TÀI NGUYÊN CHUẨN BỊ";
            ws.Cells[5, 7, 6, 7].Style.Font.Bold = true;
            ws.Cells[5, 8, 6, 8].Merge = true;
            ws.Cells[5, 8, 6, 8].Value = "GHI CHÚ";
            ws.Cells[5, 8, 6, 8].Style.Font.Bold = true;
            int rowStart = 7;
            bool check = false;
            int stt = 0;
            string content = "";
            string source = "";
            int countSTT = 1;
            int countContent = 1;
            int countSource = 1;
            foreach (var item in template)
            {
                if (item.number_title == stt)
                {
                    if (item.content == content)
                    {
                        if (item.source == source)
                        {
                            ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
                            countSTT++;
                            countContent++;
                            countSource++;
                        }
                        else
                        {
                            ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
                            ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
                            ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
                            countSTT++;
                            countContent++;
                        }
                    }
                    else
                    {
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
                        ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
                        ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
                        ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
                        countSTT++;
                    }
                }
                else
                {
                    if (item.number_title == 3)
                    {
                        ws.InsertRow(rowStart - countSTT, 1);
                        ws.Row(rowStart - countSTT).Height = 31.20;
                        ws.Cells[rowStart - countSTT, 2].Value = "Tổ chức họp lớp SV theo định kỳ/đột xuất.";
                        ws.Cells[rowStart - countSTT, 3, rowStart - countSTT, 8].Merge = true;

                        ws.InsertRow(rowStart + countSTT - 1, 1);
                        ws.Row(rowStart + countSTT - 1).Height = 31.20;
                        ws.Cells[rowStart + countSTT - 1, 2].Value = "...";
                        ws.Cells[rowStart + countSTT - 1, 6].Value = "...";
                        rowStart = rowStart + 2;
                    }
                    if (countSTT > 1)
                    {
                        if (item.number_title == 3)
                        {
                            ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Merge = true;
                            ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Value = ws.Cells[rowStart - countSTT - 1, 1].Value;
                            ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            countSTT = 1;
                        }
                        else
                        {
                            ws.Cells[rowStart - countSTT, 1, rowStart - 1, 1].Merge = true;
                            countSTT = 1;
                        }
                    }
                    if (countContent > 1)
                    {
                        ws.Cells[rowStart - countContent, 2, rowStart - 1, 2].Merge = true;
                        countContent = 1;
                    }
                    if (countSource > 1)
                    {
                        ws.Cells[rowStart - countSource, 7, rowStart - 1, 7].Merge = true;
                        countSource = 1;
                    }
                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.number_title;
                    ws.Cells[string.Format("A{0}", rowStart)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
                    ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
                    ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
                    ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
                }
                rowStart++;
                stt = (int)item.number_title;
                content = item.content;
                source = item.source;

            }

            ws.Column(10).Style.Font.Size = 13;
            /* ws.Cells["A:AZ"].AutoFitColumns();*/
            ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
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
            ws.Row(rowStart + 8).Height = 31.80;
            pck.Save();
            return pck;
        }

        public ExcelPackage ExportTemplateAdvisor(ExcelPackage pck, List<PlanClass> template, int year, string class_code)
        {
            pck.Workbook.Properties.Title = class_code;
            var ws = pck.Workbook.Worksheets.Add(class_code);

            ws.Cells["A:AZ"].Style.Font.Name = "Times New Roman";
            ws.Cells["A:AZ"].Style.Font.Size = 13;
            ws.Column(1).Width = 8;
            ws.Column(2).Width = 45;
            ws.Column(3).Width = 13.67;
            ws.Column(4).Width = 13.67;
            ws.Column(5).Width = 13.67;
            ws.Column(6).Width = 38;
            ws.Column(7).Width = 35;
            ws.Column(8).Width = 10;
            ws.Row(5).Height = 22.80;
            ws.Row(6).Height = 22.80;
            ws.Column(2).Style.WrapText = true;
            ws.Column(3).Style.WrapText = true;
            ws.Column(4).Style.WrapText = true;
            ws.Column(5).Style.WrapText = true;
            ws.Column(6).Style.WrapText = true;
            ws.Column(7).Style.WrapText = true;
            ws.Column(8).Style.WrapText = true;
            ws.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(8).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Row(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[1, 1, 1, 2].Merge = true;
            ws.Cells[1, 1, 1, 2].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
            ws.Cells[1, 1, 1, 2].Style.Font.Size = 12;
            ws.Cells[2, 1, 2, 2].Merge = true;
            ws.Cells[2, 1, 2, 2].Value = "KHOA: CÔNG NGHỆ THÔNG TIN";
            ws.Cells[2, 1, 2, 2].Style.Font.Size = 12;
            ws.Cells[2, 1, 2, 2].Style.Font.Bold = true;
            ws.Cells[3, 1, 3, 8].Merge = true;
            ws.Cells[3, 1, 3, 8].Value = "KẾ HOẠCH HOẠT ĐỘNG CỐ VẤN HỌC TẬP";
            ws.Cells[3, 1, 3, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[3, 1, 3, 8].Style.Font.Size = 14;
            ws.Cells[3, 1, 3, 8].Style.Font.Bold = true;
            ws.Cells[4, 1, 4, 8].Merge = true;
            ws.Cells[4, 1, 4, 8].Value = "NĂM HỌC " + (year - 1) + " - " + year;
            ws.Cells[4, 1, 4, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[4, 1, 4, 8].Style.Font.Size = 14;
            ws.Cells[4, 1, 4, 8].Style.Font.Bold = true;
            ws.Cells[5, 1, 6, 1].Merge = true;
            ws.Cells[5, 1, 6, 1].Value = "STT";
            ws.Cells[5, 1, 6, 1].Style.Font.Bold = true;
            ws.Cells[5, 2, 6, 2].Merge = true;
            ws.Cells[5, 2, 6, 2].Value = "NỘI DUNG";
            ws.Cells[5, 2, 6, 2].Style.Font.Bold = true;
            ws.Cells[5, 3, 5, 5].Merge = true;
            ws.Cells[5, 3, 5, 5].Value = "THỜI GIAN DỰ KIẾN";
            ws.Cells[5, 3, 5, 5].Style.Font.Bold = true;
            ws.Cells["C6"].Value = "Học kỳ 1";
            ws.Cells["C6"].Style.Font.Bold = true;
            ws.Cells["D6"].Value = "Học kỳ 2";
            ws.Cells["D6"].Style.Font.Bold = true;
            ws.Cells["E6"].Value = "Học kỳ 3";
            ws.Cells["E6"].Style.Font.Bold = true;
            ws.Cells[5, 6, 6, 6].Merge = true;
            ws.Cells[5, 6, 6, 6].Value = "MÔ TẢ CÔNG VIỆC";
            ws.Cells[5, 6, 6, 6].Style.Font.Bold = true;
            ws.Cells[5, 7, 6, 7].Merge = true;
            ws.Cells[5, 7, 6, 7].Value = "TÀI NGUYÊN CHUẨN BỊ";
            ws.Cells[5, 7, 6, 7].Style.Font.Bold = true;
            ws.Cells[5, 8, 6, 8].Merge = true;
            ws.Cells[5, 8, 6, 8].Value = "GHI CHÚ";
            ws.Cells[5, 8, 6, 8].Style.Font.Bold = true;
            int rowStart = 7;
            bool check = false;
            int stt = 0;
            string content = "";
            string source = "";
            int countSTT = 1;
            int countContent = 1;
            int countSource = 1;
            foreach (var item in template)
            {
                if (item.number_title == stt)
                {
                    if (item.content == content)
                    {
                        if (item.source == source)
                        {
                            ws.Cells[string.Format("C{0}", rowStart)].Value = item.hk1;
                            ws.Cells[string.Format("D{0}", rowStart)].Value = item.hk2;
                            ws.Cells[string.Format("E{0}", rowStart)].Value = item.hk3;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
                            countSTT++;
                            countContent++;
                            countSource++;
                        }
                        else
                        {
                            ws.Cells[string.Format("C{0}", rowStart)].Value = item.hk1;
                            ws.Cells[string.Format("D{0}", rowStart)].Value = item.hk2;
                            ws.Cells[string.Format("E{0}", rowStart)].Value = item.hk3;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
                            ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
                            ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
                            countSTT++;
                            countContent++;
                        }
                    }
                    else
                    {
                        ws.Cells[string.Format("C{0}", rowStart)].Value = item.hk1;
                        ws.Cells[string.Format("D{0}", rowStart)].Value = item.hk2;
                        ws.Cells[string.Format("E{0}", rowStart)].Value = item.hk3;
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
                        ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
                        ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
                        ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
                        countSTT++;
                    }
                }
                else
                {
                    if (item.number_title == 3)
                    {
                        ws.InsertRow(rowStart - countSTT, 1);
                        ws.Row(rowStart - countSTT).Height = 31.20;
                        ws.Cells[rowStart - countSTT, 2].Value = "Tổ chức họp lớp SV theo định kỳ/đột xuất.";
                        ws.Cells[rowStart - countSTT, 3, rowStart - countSTT, 8].Merge = true;

                        ws.InsertRow(rowStart + countSTT - 1, 1);
                        ws.Row(rowStart + countSTT - 1).Height = 31.20;
                        ws.Cells[rowStart + countSTT - 1, 2].Value = "...";
                        ws.Cells[rowStart + countSTT - 1, 6].Value = "...";
                        rowStart = rowStart + 2;
                    }
                    if (countSTT > 1)
                    {
                        if (item.number_title == 3)
                        {
                            ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Merge = true;
                            ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Value = ws.Cells[rowStart - countSTT - 1, 1].Value;
                            ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            countSTT = 1;
                        }
                        else
                        {
                            ws.Cells[rowStart - countSTT, 1, rowStart - 1, 1].Merge = true;
                            countSTT = 1;
                        }
                    }
                    if (countContent > 1)
                    {
                        ws.Cells[rowStart - countContent, 2, rowStart - 1, 2].Merge = true;
                        countContent = 1;
                    }
                    if (countSource > 1)
                    {
                        ws.Cells[rowStart - countSource, 7, rowStart - 1, 7].Merge = true;
                        countSource = 1;
                    }
                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.number_title;
                    ws.Cells[string.Format("A{0}", rowStart)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.hk1;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.hk2;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.hk3;
                    ws.Cells[string.Format("F{0}", rowStart)].Value = getDecribe(item.describe);
                    ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
                    ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
                }
                rowStart++;
                stt = (int)item.number_title;
                content = item.content;
                source = item.source;

            }


            ws.Column(10).Style.Font.Size = 13;
            /* ws.Cells["A:AZ"].AutoFitColumns();*/
            ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
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



            ws.Row(rowStart + 8).Height = 31.80;
            pck.Save();
            return pck;
        }

        public string getClassCode(int id)
        {
            var id_class = db.PlanClass.Find(id).id_class;
            var class_code = db.VLClass.Find(id_class).class_code;
            return class_code;
        }
    }
}