using AdvisorManagement.Models;
using AdvisorManagement.Models.ViewModel;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Spire.Pdf.OPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace AdvisorManagement.Middleware
{
    public class AccountMiddleware 
    {

        private CP25Team09Entities db = new CP25Team09Entities();
        private PlanMiddleware servicePlan = new PlanMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();



        // Get Role
        public string getRoles(string roles)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.user_code == roles);
            if (sql != null)
            {
                return db.Role.SingleOrDefault(x => x.id == sql.id).role_name;
            }
            return db.Role.SingleOrDefault(x => x.id == 4).role_name;
        }
        // Check User
        public string UserProfileCheck(string mail, AccountUser account)
        {
            try
            {
                string Message = "";
                if (this.adMailValid(mail) || this.stuMailValid(mail))
                {
                    switch (account.id_role)
                    {
                        case 1:
                            Message += this.AdminProfile(mail ,account);
                            break;
                        case 2:
                            Message += this.AdvisorProfile(mail, account);
                            break;
                        default:
                            Message += this.StudentProfile(mail, account);
                            break;
                    }
                }
                return Message;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        // Register Admin Profile 
        public string AdminProfile(string mail, AccountUser account)
        {
            try
            {
                AccountInitModel itemAccount = new AccountInitModel();
                itemAccount.role_id = 1;
                itemAccount.user_name = account.user_name;
                itemAccount.img_profile = account.img_profile;
                itemAccount.phone = account.phone;


                if (this.adMailValid(mail))
                {
                    itemAccount.user_code = mail.Split('@')[0] + "_cntt";
                }
                else
                {
                    string code = mail.Split('@')[0];
                    itemAccount.user_code = code.Split('.').Length == 2 ? code.Split('.')[1] : code ;
                }
                AccountUser accountUser = new AccountUser();
                accountUser.id = getID() + 1;
                accountUser.email = mail;
                accountUser.id_role = itemAccount.role_id;
                accountUser.user_code = itemAccount.user_code;
                accountUser.img_profile = itemAccount.img_profile;
                accountUser.phone = itemAccount.phone;
                accountUser.user_name = itemAccount.user_name;
                accountUser.create_time = DateTime.Now;
                accountUser.update_time = DateTime.Now;
                db.AccountUser.Add(accountUser);
                db.SaveChanges();

                return "Đăng kí thành công admin vào trong hệ thống";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        // Register Student Profile 
        public string StudentProfile(string mail, AccountUser account)
        {
            try
            {
                AccountInitModel itemAccount = new AccountInitModel();
                itemAccount.role_id = 3;
                itemAccount.user_name = account.user_name;
                itemAccount.img_profile = account.img_profile;
                itemAccount.phone = account.phone;

                if (this.stuMailValid(mail))
                {
                    string code = mail.Split('@')[0];
                    itemAccount.user_code = code.Split('.').Length == 2 ? code.Split('.')[1] : code;
                }
                else
                {
                    string code = mail.Split('@')[0];
                    itemAccount.user_code = code.Split('.').Length == 2 ? code.Split('.')[1] : code;
                }
                AccountUser accountUser = new AccountUser();
                accountUser.id = getID() + 1;
                accountUser.email = mail;
                accountUser.id_role = itemAccount.role_id;
                accountUser.user_code = itemAccount.user_code;
                accountUser.img_profile = itemAccount.img_profile;
                accountUser.phone = itemAccount.phone;
                accountUser.user_name = itemAccount.user_name;
                accountUser.create_time = DateTime.Now;
                accountUser.update_time = DateTime.Now;
                db.AccountUser.Add(accountUser);
                db.SaveChanges();
                Student student = new Student();
                student.student_code = itemAccount.user_code;
                student.account_id = getID();
                student.status_id = 1;
                db.Student.Add(student);
                db.SaveChanges();
                return "Đăng kí thành công học sinh vào trong hệ thống";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Register Advisor Profile
        public string AdvisorProfile(string mail, AccountUser account)
        {
            try
            {
                AccountInitModel itemAccount = new AccountInitModel();
                itemAccount.role_id = 2;
                itemAccount.user_name = account.user_name;
                itemAccount.img_profile = account.img_profile;
                itemAccount.phone = account.phone;
                if (this.adMailValid(mail))
                {
                    itemAccount.user_code = mail.Split('@')[0] + "_cntt";
                }
                else
                {
                    string code = mail.Split('@')[0];
                    itemAccount.user_code = code.Split('.').Length == 2 ? code.Split('.')[1] : code;
                }
                AccountUser accountUser = new AccountUser();
                accountUser.id = getID() + 1;
                accountUser.email = mail;
                accountUser.id_role = itemAccount.role_id;
                accountUser.user_code = itemAccount.user_code;
                accountUser.img_profile = itemAccount.img_profile;
                accountUser.phone = itemAccount.phone;
                accountUser.user_name = itemAccount.user_name;
                accountUser.create_time = DateTime.Now;
                accountUser.update_time = DateTime.Now;
                db.AccountUser.Add(accountUser);
                db.SaveChanges();
                Advisor advisor = new Advisor();
                advisor.advisor_code = itemAccount.user_code;
                advisor.account_id = getID();
                advisor.status_id = 1;
                db.Advisor.Add(advisor);
                db.SaveChanges();
                return "Đăng kí thành công cố vấn học tập vào trong hệ thống";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Register Profile
        public void UserProfile(ClaimsIdentity logData)
        {
            var objectTest = logData;
            string[] data = objectTest.Claims.Where(x => x.Type == "name").First().Value.Split('-');
            AccountInitModel itemAccount = new AccountInitModel();
            itemAccount.img_profile = string.Empty;

            string[] mailGet = logData.Name.Trim().Split('@');

            if (mailGet[mailGet.Length - 1] == "vlu.edu.vn")
            {
                itemAccount.role_id = 2;
                itemAccount.user_name = data[0].Trim();
                itemAccount.user_code = mailGet[0] + "_cntt";
                writeRecordUser(itemAccount, logData.Name);
                Advisor advisor = new Advisor();
                advisor.advisor_code = itemAccount.user_code;
                advisor.account_id = getID();
                advisor.status_id = 1;
                db.Advisor.Add(advisor);
            }
            else
            {
                itemAccount.role_id = 3;
                itemAccount.user_code = data[0].Trim();
                itemAccount.user_name = data[1].Trim();
                writeRecordUser(itemAccount, logData.Name);
                Student student = new Student();
                student.student_code = itemAccount.user_code;
                student.account_id = getID();
                student.status_id = 1;
                db.Student.Add(student);
            }
            db.SaveChanges();
        }

        // write init user
        public void writeRecordUser(AccountInitModel data,string mail)
        {
            AccountUser accountUser = new AccountUser();
            accountUser.id = getID()+1;
            accountUser.email = mail;
            accountUser.id_role = data.role_id;
            accountUser.user_code = data.user_code;
            accountUser.img_profile = data.img_profile;
            accountUser.phone = data.phone;
            accountUser.user_name = data.user_name;
            accountUser.create_time = DateTime.Now;
            accountUser.update_time = DateTime.Now;
            db.AccountUser.Add(accountUser);
            db.SaveChanges();
        }
        // get new id 
        public int getID()
        {
            var lastRecordUser =  db.AccountUser.
               OrderByDescending(s => s.id).
               Select(user => new
               {
                   Id = user.id,
               }).FirstOrDefault();
            return (lastRecordUser != null) ? (int)lastRecordUser.Id : 0;
        }
        // get Roles for Prodile
        public bool getPermission(string userMail,string routeAccess)
        {
            var seeMenu = (from pq in db.AccountUser
                           join mnrole in db.RoleMenu on pq.id_role equals mnrole.id_role
                           join mn in db.Menu on mnrole.id_menu equals mn.id
                           where pq.email == userMail && pq.id_role == mnrole.id_role && mn.action_link == routeAccess
                           select new Models.ViewModel.MenuItem
                           {      
                               actionLink = mn.action_link
                           });
            return (seeMenu.FirstOrDefault(x => x.actionLink == routeAccess) != null ) ? true : false;

        }

        public bool WriteDataFromExcelClass(string email, string name_advisor, string id_class,string hk_year)
        {
            var flag = false;
            AccountInitModel itemAccount = new AccountInitModel();
            string[] mailGet = email.Trim().Split('@');
            var user = db.AccountUser.Where(x => x.email == email).Count();            
            if (user == 0)
            {
                itemAccount.role_id = 2;
                itemAccount.user_name = name_advisor;
                itemAccount.user_code = mailGet[0] + "_cntt";
                writeRecordUser(itemAccount, email);
                var checkAdv = db.Advisor.Where(x => x.advisor_code == itemAccount.user_code).Count();
                if(checkAdv == 0)
                {
                    Advisor advisor = new Advisor();
                    advisor.advisor_code = itemAccount.user_code;
                    advisor.account_id = getID();
                    advisor.status_id = 1;
                    db.Advisor.Add(advisor);
                    db.SaveChanges();
                }                
                VLClass vlclass = new VLClass();
                var checkClas = checkClass(id_class,hk_year);
                if (checkClas)
                {
                    vlclass.course = getCours(id_class);
                    vlclass.class_code = id_class;
                    vlclass.advisor_code = itemAccount.user_code;
                    vlclass.semester_name = hk_year;
                    vlclass.create_time = DateTime.Now;
                    db.VLClass.Add(vlclass);
                    db.SaveChanges();
                    servicePlan.AssignmentTemplate(vlclass.id, int.Parse(hk_year));
                    servicePlan.PlanStatus(vlclass.id);
                    flag = true;
                }               
            }
            else
            {
                var user_code = db.AccountUser.FirstOrDefault(x => x.email == email).user_code;
                var checkAdv = db.Advisor.Where(x => x.advisor_code == user_code).Count();
                if (checkAdv == 0)
                {
                    var advisor_code = db.AccountUser.FirstOrDefault(x => x.email == email).user_code;
                    Advisor advisor = new Advisor();
                    advisor.advisor_code = advisor_code;
                    advisor.account_id = getID();
                    advisor.status_id = 1;
                    db.Advisor.Add(advisor);
                    db.SaveChanges();
                }
                VLClass vlclass = new VLClass();
                var checkClas = checkClass(id_class, hk_year);
                if (checkClas)
                {
                    var advisor_code = db.AccountUser.FirstOrDefault(x => x.email == email).user_code;
                    vlclass.course = getCours(id_class);
                    vlclass.class_code = id_class;
                    vlclass.advisor_code = advisor_code;
                    vlclass.semester_name = hk_year;
                    vlclass.create_time = DateTime.Now;
                    db.VLClass.Add(vlclass);
                    db.SaveChanges();
                    servicePlan.AssignmentTemplate(vlclass.id, int.Parse(hk_year));
                    servicePlan.PlanStatus(vlclass.id);
                    flag = true;
                }                
            }           
            return flag;
        }

        public bool checkClass(object id_class, string year)
        {
            var flag = false;
            var isClass = db.VLClass.Where(x => x.class_code == id_class && x.semester_name == year).Count();
            if (isClass == 0)
            {
                return true;
            }
            return flag;
        }

        public string getAvatar(string user_mail)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.email == user_mail);
            if(sql.img_profile != null && sql.img_profile != "")
            {
                return sql.img_profile.Replace("~/Images/imageProfile/", "");
            }
            else
            {
                sql.img_profile = "avata.png";
                return sql.img_profile;

            }
            
        }
        public string getTextName(string user_mail)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.email == user_mail);
            string text = sql.user_name;
            return text;
        }
        public string getCode(string user_mail)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.email == user_mail);
            string text = sql.user_code;
            return text;
        }
        public string getRoleTextName(string user_mail)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.email == user_mail);

            return sql.id_role + "";
        }

        public ExcelPackage getExcelPackage(ExcelPackage pck, int year, List<VLClass> listStudent)
        {
            var ws = pck.Workbook.Worksheets.Add("CVHT " + (year - 1) + "-" + year);
            ws.Cells["A:AZ"].Style.Font.Name = "Times New Roman";
            ws.Cells["A:AZ"].Style.Font.Size = 13;
            ws.Cells["A:AZ"].Style.WrapText = true;
            ws.Cells["A:AZ"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Column(1).Width = 6.56;
            ws.Column(2).Width = 15.56;
            ws.Column(3).Width = 28.78;
            ws.Column(4).Width = 26.22;
            ws.Column(5).Width = 25.78;
            ws.Column(6).Width = 19.11;
            ws.Column(7).Width = 15.67;
            ws.Row(3).Height = 22.80;
            ws.Row(4).Height = 22.80;
            ws.Row(5).Height = 22.80;
            ws.Cells[1, 1, 1, 3].Merge = true;
            ws.Cells[1, 1, 1, 3].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
            ws.Cells[1, 1, 1, 3].Style.Font.Size = 11;
            ws.Cells[1, 1, 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            ws.Cells[2, 1, 2, 3].Merge = true;
            ws.Cells[2, 1, 2, 3].Value = "KHOA: CÔNG NGHỆ THÔNG TIN";
            ws.Cells[2, 1, 2, 3].Style.Font.Size = 11;
            ws.Cells[2, 1, 2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            ws.Cells[2, 1, 2, 3].Style.Font.Bold = true;
            ws.Cells[3, 1, 3, 7].Merge = true;
            ws.Cells[3, 1, 3, 7].Value = "DANH SÁCH CỐ VẤN HỌC TẬP";
            ws.Cells[3, 1, 3, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[3, 1, 3, 7].Style.Font.Size = 18;
            ws.Cells[3, 1, 3, 7].Style.Font.Bold = true;
            ws.Cells[4, 1, 4, 7].Merge = true;
            ws.Cells[4, 1, 4, 7].Value = "NĂM HỌC " + (year - 1) + " - " + year;
            ws.Cells[4, 1, 4, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[4, 1, 4, 7].Style.Font.Size = 18;
            ws.Cells[4, 1, 4, 7].Style.Font.Bold = true;

            ws.Cells[5, 1].Value = "STT";
            ws.Cells[5, 1].Style.Font.Bold = true;
            ws.Cells[5, 2].Value = "MGV";
            ws.Cells[5, 2].Style.Font.Bold = true;
            ws.Cells[5, 3].Value = "CVHT";
            ws.Cells[5, 3].Style.Font.Bold = true;
            ws.Cells[5, 4].Value = "EMAIL";
            ws.Cells[5, 4].Style.Font.Bold = true;
            ws.Cells[5, 5].Value = "MÃ LỚP";
            ws.Cells[5, 5].Style.Font.Bold = true;
            ws.Cells[5, 6].Value = "SLSV";
            ws.Cells[5, 6].Style.Font.Bold = true;
            ws.Cells[5, 7].Value = "GHI CHÚ";
            ws.Cells[5, 7].Style.Font.Bold = true;
            var i = 1;
            int rowStart = 6;
            var advisor_code = listStudent.DistinctBy(x => x.advisor_code).ToList();
            foreach (var item in advisor_code)
            {
                var MGV = item.advisor_code;
                var advisor_name = db.AccountUser.FirstOrDefault(x => x.user_code == item.advisor_code).user_name;
                var advisor_email = db.AccountUser.FirstOrDefault(x => x.user_code == item.advisor_code).email;
                var class_code = getClassCodeExportListClass(listStudent, item.advisor_code);
                var count_student = getCountStudent(listStudent, item.advisor_code);
                ws.Cells[string.Format("A{0}", rowStart)].Value = i;
                ws.Cells[string.Format("B{0}", rowStart)].Value = MGV;
                ws.Cells[string.Format("C{0}", rowStart)].Value = advisor_name;
                ws.Cells[string.Format("D{0}", rowStart)].Value = advisor_email;
                ws.Cells[string.Format("E{0}", rowStart)].Value = class_code;
                ws.Cells[string.Format("F{0}", rowStart)].Value = count_student;
                ws.Cells[string.Format("G{0}", rowStart)].Value = "";

                rowStart++;
                i++;
            }
            ws.Cells[5, 1, rowStart - 1, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[5, 1, rowStart - 1, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Row(rowStart).Height = 25.80;
            ws.Cells[rowStart, 5, rowStart, 7].Merge = true;
            ws.Cells[rowStart, 5, rowStart, 7].Value = "TP.HCM, ngày   tháng   năm " + year;
            ws.Cells[rowStart, 5, rowStart, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[rowStart, 5, rowStart, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            ws.Cells[rowStart + 1, 2, rowStart + 1, 3].Merge = true;
            ws.Cells[rowStart + 1, 2, rowStart + 1, 3].Value = "Ban chủ nhiệm khoa";
            ws.Cells[rowStart + 1, 2, rowStart + 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[rowStart + 1, 5, rowStart + 1, 7].Merge = true;
            ws.Cells[rowStart + 1, 5, rowStart + 1, 7].Value = "Trợ lý CTSV";
            ws.Cells[rowStart + 1, 5, rowStart + 1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            pck.Save();
            return pck;
        }        
        
        public string getClassCodeExportListClass(List<VLClass> listStudent, string advisor_code)
        {
            string class_code = "";
            string data5 = "";
            string data7 = "";
            foreach (var item in listStudent)
            {
                if (item.advisor_code == advisor_code)
                {
                    if (data5 == "")
                    {
                        data5 = data5 + item.class_code;
                    }
                    else
                    {
                        var check = class_code.Contains(item.class_code.Remove(item.class_code.Length - 2));
                        if (check == true)
                        {
                            var split = class_code.Split('\n');
                            if (split.Count() > 1)
                            {
                                for (var i = 0; i < split.Length; i++)
                                {
                                    var checkk = split[i].Contains(item.class_code.Remove(item.class_code.Length - 2));
                                    if (checkk == true)
                                    {
                                        var number = item.class_code.Substring(item.class_code.Length - 2);
                                        foreach (Char c in number)
                                        {
                                            if (!Char.IsDigit(c))
                                            {
                                                data5 = class_code.Replace(split[i], split[i] + "," + item.class_code.Substring(item.class_code.Length - 1));
                                                class_code = "";
                                                data7 = "";
                                                break;
                                            }
                                            else
                                            {
                                                data5 = class_code.Replace(split[i], split[i] + "," + item.class_code.Substring(item.class_code.Length - 2));
                                                class_code = "";
                                                data7 = "";
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var number = item.class_code.Substring(item.class_code.Length - 2);
                                foreach (Char c in number)
                                {
                                    if (!Char.IsDigit(c))
                                    {
                                        data5 = data5 + "," + item.class_code.Substring(item.class_code.Length - 1);
                                        break;
                                    }
                                    else
                                    {
                                        data5 = data5 + "," + item.class_code.Substring(item.class_code.Length - 2);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            data7 = data7 + item.class_code + "\n";
                        }
                    }
                    if (data7 != "")
                    {
                        class_code = data7 + data5;                       
                    }
                    else
                    {
                        class_code = data5;
                    }
                }
            }
            return class_code;
        }

        public int getCountStudent(List<VLClass> listStudent, string advisor_code)
        {
            int count = 0;
            foreach (var item in listStudent)
            {
                if (item.advisor_code == advisor_code)
                {
                    count += db.ListStudents.Where(x => x.id_class == item.id).Count();
                }
            }

            return count;
        }

        public int getCours(string mssv)
        {
            var index = mssv.IndexOf("K");
            var cours = mssv.Substring(index + 1, 2);
            return int.Parse(cours);
        }

        public  bool IsPhoneNumberValid(string phoneNumber)
        {
            string phonePattern = @"^(84|\+84|0)(1\d{9}|3\d{8}|5\d{8}|7\d{8}|8\d{8}|9\d{8})$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }
        public bool IsValidEmail(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                return false;

            string advisorMailPattems = @"^[^@\s]+@vlu\.edu\.vn$";
            string studnetPattems = @"^[^@\s]+@(vanlanguni\.vn|vlu\.edu\.vn)$";
            bool Check = Regex.IsMatch(email, advisorMailPattems) || Regex.IsMatch(email, studnetPattems) ? true : false;
            return Check;
        }

        public bool adMailValid(string mail)
        {
            string advisorMailPattems = @"^[^@\s]+@vlu\.edu\.vn$";
            return Regex.IsMatch(mail, advisorMailPattems) ? true : false;
        }

        public bool stuMailValid(string mail)
        {
            string studnetPattems = @"^[^@\s]+@(vanlanguni\.vn|vlu\.edu\.vn)$";
            return Regex.IsMatch(mail, studnetPattems) ? true : false;
        }

        public bool IsValidVietnameseName(string name)
        {
            string pattern = @"^([A-ZĐ][a-zđ]*)(\s[A-ZĐ][a-zđ]*)+$";
            return Regex.IsMatch(name, pattern);
        }

    }
}