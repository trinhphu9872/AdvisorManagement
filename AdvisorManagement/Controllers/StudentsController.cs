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

namespace AdvisorManagement.Controllers
{
    public class StudentsController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private StudentsMiddleware serviceStudents = new StudentsMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        // GET: Class
        public ActionResult Index()
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            var listClass = serviceStudents.getClass(User.Identity.Name);
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
                    return RedirectToAction("DetailClass", new { classCode = classCode });
                }
            }
            return View();
        }

        public ActionResult DetailClass(string classCode)
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            if (serviceStudents.getPermission(classCode, User.Identity.Name))
            {
                var detailClass = serviceStudents.getStudent(classCode);
                if (detailClass == null)
                {
                    return HttpNotFound();
                }
                ViewBag.detailClass = serviceStudents.getStudent(classCode);
                ViewBag.classInfo = serviceStudents.getInfoClass(classCode);
                Session["classCode"] = classCode;
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                var role = serviceStudents.getRoles(User.Identity.Name);
                ViewBag.role = role;
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult UpdateStudent(int id)
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            int roleUser = serviceStudents.getRolesUser(id);
            if (roleUser == 2 || roleUser == 1)
            {
                return HttpNotFound();
            }
            AccountUser user = db.AccountUser.Find(id);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);


            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateStudent([Bind(Include = "id,user_name,phone")] AccountUser accountUser)
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            if (ModelState.IsValid)
            {
                var tempUser = db.AccountUser.FirstOrDefault(x => x.id == accountUser.id);
                tempUser.user_name = accountUser.user_name;
                tempUser.phone = accountUser.phone;
                db.Entry(tempUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DetailClass", new { classCode = Session["classCode"] });
            }
            return View(accountUser);
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
                        int count = 0;
                        ImportData(out count, filePath);
                        if (count > 0)
                        {
                            return Json(new { success = true, message = "Thêm thành công " + count.ToString() + " sinh viên mới" });
                        }
                        else
                        {
                            return Json(new { success = true, message = "Import thành công" });
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
        private bool ImportData(out int count, string filePath)
        {

            var result = false;
            count = 0;
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
                    Object id_class = worksheet.Cells[startRow, startColumn + 21].Value;
                    Object status = worksheet.Cells[startRow, startColumn + 10].Value;
                    Object gender = worksheet.Cells[startRow, startColumn + 4].Value;
                    Object address1 = worksheet.Cells[startRow, startColumn + 7].Value;
                    Object address2 = worksheet.Cells[startRow, startColumn + 9].Value;
                    Object address3 = worksheet.Cells[startRow, startColumn + 8].Value;
                    var isSuccessAcc = false;
                    var isSuccessStd = false;
                    var classCode = Session["classCode"];
                    if (data != null && mssv != null && name != null && phone != null
                        && email != null && id_class != null && status != null && gender != null)
                    {
                        if (id_class.ToString() == classCode.ToString())
                        {
                            var address = address1.ToString() + ", " + address2.ToString() + ", " + address3.ToString();
                            //check exist 
                            if (db.AccountUser.Where(t => t.email.Equals(email.ToString())).Count() == 0)
                            {
                                //import db
                                isSuccessAcc = serviceStudents.saveAccount(mssv.ToString(), name.ToString(), phone.ToString(), email.ToString(), address, gender, db);
                            }
                            else
                            {
                                serviceStudents.UpdateAccount(mssv.ToString(), name.ToString(), phone.ToString(), address, gender.ToString(), db);
                            }
                            if (db.Student.Where(t => t.student_code.Equals(mssv.ToString())).Count() == 0)
                            {
                                //import db
                                isSuccessStd = serviceStudents.saveStudent(mssv.ToString(), id_class.ToString(), status.ToString(), db);
                            }
                            else
                            {
                                serviceStudents.UpdateStudentImport(mssv.ToString(), status.ToString(), db);
                            }
                            if (isSuccessAcc && isSuccessStd)
                            {
                                count++;
                            }
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
            var classCode = Session["classCode"].ToString();
            var detailClass = serviceStudents.getStudent(classCode);
            List<AdvisorManagement.Models.ViewModel.ListStudent> listStudent = (List<AdvisorManagement.Models.ViewModel.ListStudent>)detailClass;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    pck.Workbook.Properties.Title = classCode;
                    var ws = pck.Workbook.Worksheets.Add(classCode);
                    ws.Cells["A1"].Value = "MSSV";
                    ws.Cells["B1"].Value = "Họ và tên";
                    ws.Cells["C1"].Value = "Email";
                    ws.Cells["D1"].Value = "Số điện thoại";
                    ws.Cells["E1"].Value = "Khóa";
                    ws.Cells["F1"].Value = "Trạng thái";

                    int rowStart = 2;
                    foreach (var item in listStudent)
                    {
                        ws.Cells[string.Format("A{0}", rowStart)].Value = item.idStudent;
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.name;
                        ws.Cells[string.Format("C{0}", rowStart)].Value = item.email;
                        ws.Cells[string.Format("D{0}", rowStart)].Value = item.phone;
                        ws.Cells[string.Format("E{0}", rowStart)].Value = item.course;
                        ws.Cells[string.Format("F{0}", rowStart)].Value = item.status;
                        rowStart++;
                    }
                    ws.Cells["A:AZ"].AutoFitColumns();
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
    }
}