using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvisorManagement.Models.ViewModel;
using AdvisorManagement.Models;
using AdvisorManagement.Middleware;
using System.Net;
using System.Data.Entity;
using System.IO;
using Spire.Xls;
using OfficeOpenXml;
using Microsoft.Ajax.Utilities;
using System.Data;
using OfficeOpenXml.Table;
using System.Web.Services.Description;
using System.Runtime.CompilerServices;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Web.UI.WebControls;
using OfficeOpenXml.Style;

namespace AdvisorManagement.Controllers
{
    [LoginFilter]
    public class StudentsController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private StudentsMiddleware serviceStudents = new StudentsMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private PlanMiddleware servicePlan = new PlanMiddleware();
        public void init()
        {
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
        }
        // GET: Class

        public ActionResult Index()
        {
            var year = servicePlan.getYear();
            this.init();
            var listClass = serviceStudents.getClass(User.Identity.Name,year);
            var role = serviceStudents.getRoles(User.Identity.Name);
            ViewBag.role = role;
            var classCode = serviceStudents.getClassCode(User.Identity.Name);
            if (role != null)
            {
                if (role == "Advisor")
                {
                    ViewBag.classAdvisor = listClass;
                    return View(listClass);
                }
                else if (role == "Student")
                {
                    return RedirectToAction("DetailClass", new { id = classCode });
                }
            }
            return View();
        }

        public ActionResult DetailClass(int id)
        {
            if (serviceStudents.getPermission(id,User.Identity.Name))
            {
                this.init();
                var detailClass = serviceStudents.getStudentList(id);
                if (detailClass == null)
                {
                    return HttpNotFound();
                }
                ViewBag.detailClass = detailClass;
                ViewBag.classInfo = serviceStudents.getInfoClass(id);
                Session["id_class"] = id;
                var role = serviceStudents.getRoles(User.Identity.Name);
                ViewBag.role = role;
                var listStdStatus = db.StudentStatus.ToList();
                ViewBag.listStdStatus = listStdStatus;
                return View();
            } else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult UpdateStudent(int id)
        {                                
            try
            {
                AccountUser user = db.AccountUser.Find(id);
                Student std = db.Student.Find(user.user_code);     
                return Json(new { success = true, id_std = user.id, mssv = user.user_code, name = user.user_name, email = user.email, phone = user.phone, status = std.status_id, message = "Lấy thông tin thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lấy thông tin thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateStudent(int id, string name, string phone, int status)
        {
            try
            {
                if(name.Trim() != "" && phone.Trim() != "")
                {
                    AccountUser user = db.AccountUser.Find(id);
                    user.user_name = name;
                    user.phone = phone;
                    Student std = db.Student.Find(user.user_code);
                    std.status_id = status;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ImportStudent(HttpPostedFileBase postedfile)
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            try
            {
                string filePath = string.Empty;
                if (postedfile == null || postedfile.ContentLength == 0)
                {
                    ViewBag.Error = "Vui lòng chọn file";
                    return Json(new { success = false, message = "Vui lòng chọn file" });
                }
                else
                {
                    if (postedfile.FileName.EndsWith(".xls") || postedfile.FileName.EndsWith(".xlsx"))
                    {
                        string path = Server.MapPath("~/Upload/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }   
                        filePath = path + Path.GetFileName(postedfile.FileName);
                        string extension = Path.GetExtension(postedfile.FileName);
                        postedfile.SaveAs(filePath);
                        if (Path.GetExtension(postedfile.FileName) == ".xls")
                        {
                            Workbook workbook = new Workbook();
                            workbook.LoadFromFile(filePath);
                            workbook.SaveToFile(filePath, ExcelVersion.Version2013);
                        }
                        int countAdd = 0;
                        int countUpdate = 0;
                        ImportData(out countAdd, out countUpdate, filePath);
                        if (countAdd !=0 && countUpdate!=0)
                        {
                            return Json(new 
                            { 
                                success = true, 
                                message = "Thêm " + countAdd.ToString() + " sinh viên mới" +"\n" +
                                          "Cập nhật thông tin " + countUpdate.ToString() + " sinh viên"});
                        }else if(countAdd != 0)
                        {
                            return Json(new
                            {
                                success = true,
                                message = "Thêm " + countAdd.ToString() + " sinh viên mới"                                                                     
                            });
                        }
                        else if(countUpdate != 0)
                        {
                            return Json(new
                            {
                                success = true,
                                message = "Cập nhật " + countUpdate.ToString() + " sinh viên"
                            });
                        }
                        else
                        {
                            return Json(new { success = true, message = "Không thực hiện thay đổi nào" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Vui lòng chọn file định dạng Excel" });
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Please upload excel file" });
            }
        }
        private bool ImportData(out int countAdd, out int countUpdate, string filePath)
        {

            var result = false;
            countAdd = 0;
            countUpdate = 0;
            try
            {
                /* String path = Server.MapPath("/") + "\\import\\class.xlsx";*/

                var package = new ExcelPackage(new System.IO.FileInfo(filePath));
                var startColumn = 1;
                var startRow = 2;
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                object data = null;
                CP25Team09Entities db = new CP25Team09Entities();
                do
                {
                    data = worksheet.Cells[startRow, startColumn].Value;
                    Object mssv = worksheet.Cells[startRow, startColumn].Value;
                    Object name = worksheet.Cells[startRow, startColumn + 1].Value;
                    Object phone = worksheet.Cells[startRow, startColumn + 11].Value;
                    Object email = worksheet.Cells[startRow, startColumn + 12].Value;
/*                    Object id_class = worksheet.Cells[startRow, startColumn + 21].Value;*/
                    Object status = worksheet.Cells[startRow, startColumn + 10].Value;
                    Object gender = worksheet.Cells[startRow, startColumn + 4].Value;
                    Object address1 = worksheet.Cells[startRow, startColumn + 7].Value;
                    Object address2 = worksheet.Cells[startRow, startColumn + 9].Value;
                    Object address3 = worksheet.Cells[startRow, startColumn + 8].Value;
                    var isSuccessAcc = false;
                    var isSuccessStd = false;
                    var classCode = Session["id_class"];
                    if (data != null && mssv != null && name != null && phone != null
                        && email != null != null && status != null && gender != null)
                    {
                        
                            var address = address1.ToString() + ", " + address2.ToString() + ", " + address3.ToString();
                            //check exist account
                            if (db.AccountUser.Where(t => t.email.Equals(email.ToString())).Count() == 0)
                            {
                                //import db
                                serviceStudents.saveAccount(mssv.ToString(), name.ToString(), phone.ToString(), email.ToString(), address, gender, db);
                            }
                            else
                            {
                                isSuccessAcc = serviceStudents.UpdateAccount(mssv.ToString(), name.ToString(), phone.ToString(), address, gender.ToString(), db);
                            }
                            //check exist student
                            if (db.Student.Where(t => t.student_code.Equals(mssv.ToString())).Count() == 0)
                            {
                                //import db
                                serviceStudents.saveStudent(mssv.ToString(), status.ToString(), db, (int)classCode);
                            }
                            else
                            {
                                isSuccessStd = serviceStudents.UpdateStudentImport(mssv.ToString(), status.ToString(), db, (int)classCode);
                            }
                            var checkExist = db.ListStudents.Where(x=>x.id_class == (int)classCode).Where(y=>y.student_code == mssv).ToList().Count();
                            if (checkExist == 0)
                            {
                                serviceStudents.AddListStudent((int)classCode, mssv.ToString());
                                countAdd++;
                            }
                            if(isSuccessAcc || isSuccessStd)
                            {
                                countUpdate++;
                            }
                        
                    }
                    startRow++;
                }
                while (data != null);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public void ExcelExport()
        {
            var id_class = Session["id_class"];
            var classCode = db.VLClass.Find(id_class).class_code;
            var detailClass = serviceStudents.getStudentList((int)id_class);
            var infoClass = serviceStudents.getInfoClass((int)id_class);
            List<AdvisorManagement.Models.ViewModel.ListStudent> listStudent = (List<AdvisorManagement.Models.ViewModel.ListStudent>)detailClass;
            List<AdvisorManagement.Models.ViewModel.AdvisorClass> info = (List<AdvisorManagement.Models.ViewModel.AdvisorClass>)infoClass;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    pck.Workbook.Properties.Title = classCode;
                    var ws = pck.Workbook.Worksheets.Add(classCode);
                    ws.Cells["A:AZ"].Style.Font.Name = "Times New Roman";
                    ws.Cells["A:AZ"].Style.Font.Size = 13;
                    ws.Cells["A:AZ"].Style.WrapText = true;
                    ws.Cells["A:AZ"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Column(1).Width = 6.56;
                    ws.Column(2).Width = 15.56;
                    ws.Column(3).Width = 28.78;
                    ws.Column(4).Width = 38;
                    ws.Column(5).Width = 25.78;
                    ws.Column(6).Width = 19.11;
                    ws.Column(7).Width = 15.67;
                    ws.Row(4).Height = 22.80;
                    ws.Row(5).Height = 22.80;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    foreach (var item in info)
                    {
                        ws.Cells[1, 1, 1, 3].Merge = true;                       
                        ws.Cells[1, 1, 1, 3].Value = "Lớp: " + item.idClass;
                        ws.Cells[1, 1, 1, 3].Style.Font.Bold = true;
                        ws.Cells[2, 1, 2, 3].Merge = true;                    
                        ws.Cells[2, 1, 2, 3].Value = "Cố vấn: " + item.name;
                        ws.Cells[2, 1, 2, 3].Style.Font.Bold = true;
                        ws.Cells[3, 1, 3, 3].Merge = true; 
                        ws.Cells[3, 1, 3, 3].Value = "Năm học: " + (int.Parse(item.semester)-1) + "-"  +item.semester;
                        ws.Cells[3, 1, 3, 3].Style.Font.Bold = true;                        
                    }
                    ws.Cells[4, 1, 4, 6].Merge = true;
                    ws.Cells[4, 1, 4, 6].Value = "Danh sách sinh viên";
                    ws.Cells[4, 1, 4, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[4, 1, 4, 6].Style.Font.Size = 18;
                    ws.Cells[4, 1, 4, 6].Style.Font.Bold = true;
                    ws.Cells["A5"].Value = "STT";
                    ws.Cells["B5"].Value = "MSSV";
                    ws.Cells["C5"].Value = "Họ và tên";
                    ws.Cells["D5"].Value = "Email";
                    ws.Cells["E5"].Value = "Số điện thoại";
                    ws.Cells["F5"].Value = "Trạng thái";

                    int rowStart = 6;
                    int stt = 1;
                    foreach (var item in listStudent)
                    {
                        ws.Cells[string.Format("A{0}", rowStart)].Value = stt;
                        ws.Cells[string.Format("A{0}", rowStart)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.idStudent;
                        ws.Cells[string.Format("C{0}", rowStart)].Value = item.name;
                        ws.Cells[string.Format("D{0}", rowStart)].Value = item.email;
                        ws.Cells[string.Format("E{0}", rowStart)].Value = item.phone;                      
                        ws.Cells[string.Format("F{0}", rowStart)].Value = item.status;
                        rowStart++;
                        stt++;
                    }
                    ws.Cells[5, 1, rowStart - 1, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[5, 1, rowStart - 1, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[5, 1, rowStart - 1, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[5, 1, rowStart - 1, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    Response.Clear();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + classCode + ".xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var student = db.ListStudents.SingleOrDefault(x => x.id == id);
                db.ListStudents.Remove(student);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}