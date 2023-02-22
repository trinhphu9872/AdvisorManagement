using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using OfficeOpenXml;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using System.Web.Services.Description;
using System.Data;


using System.Drawing;
using Spire.Xls.Core;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    public class ClassManagementsController : Controller
    {

        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();

        private string routePermission = "Admin/ClassManagements";
        CP25Team09Entities db = new CP25Team09Entities();
        // GET: Admin/ClassManagements
        public ActionResult Index()
        {
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                var listClass = db.VLClass.ToList();
                ViewBag.listClass = listClass;
                Session["listClass"] = listClass;
                ViewBag.nameUser = db.AccountUser.ToList();
                Session["nameUser"] = db.AccountUser.ToList();
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                ViewBag.hocky = db.Semester.ToList().OrderBy(x => x.scholastic);
                Session["hocky"] = db.Semester.ToList().OrderBy(x => x.scholastic);
                return View(listClass);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult Details(int? id)
        {
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                var detailClass = db.VLClass.Find(id);


                //List<string> AV = new List<string>();
                //for(int i =0; i<db.AccountUser.Count();i++)
                //{
                //    var user = db.AccountUser.Find(i);
                //    var advisor = db.Advisor.Find(user.user_code);
                //    if (advisor !=null)
                //    {
                //        AV.Add(user.username);
                //    }

                //}
                //ViewBag.Advisor = new SelectList(AV);
                ViewBag.Advisor = db.Advisor.ToList();
                ViewBag.nameUser = db.AccountUser.ToList();
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                return View(detailClass);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        //// get create class
        //public ActionResult Create()
        //{
        //    ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code");
        //    return View();
        //}

        //// post create class
        //[HttpPost]
        //public ActionResult Create([Bind(Include = "id,class_code,advisor_code")] VLClass vLClass)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        vLClass.create_time = DateTime.Now;
        //        db.VLClass.Add(vLClass);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code", vLClass.advisor_code);
        //    return View(vLClass);




        //}

        // GET: Admin/VLClasses/Create
        public ActionResult Create()
        {
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code");
            ViewBag.class_code = new SelectList(db.VLClass, "class_code", "class_code");
            ViewBag.hocky = db.Semester.ToList().OrderBy(x => x.scholastic);
            Session["hocky"] = db.Semester.ToList().OrderBy(x => x.scholastic);
            return View();
        }

        // POST: Admin/VLClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create([Bind(Include = "id,class_code,advisor_code,create_time,update_time,semester_name")] VLClass vLClass)
        {
           

            if (ModelState.IsValid)
            {
                db.VLClass.Add(vLClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code", vLClass.advisor_code);
            return View(vLClass);
        }

        // GET: Admin/VLClasses/Delete/5
        public ActionResult Delete(int id)
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VLClass vLClass = db.VLClass.Find(id);
            if (vLClass == null)
            {
                return HttpNotFound();
            }
            return View(vLClass);
        }

        // POST: Admin/VLClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            VLClass vLClass = db.VLClass.Find(id);
            db.VLClass.Remove(vLClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // HTTP GET
        public ActionResult EditClass(int id )
        {
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                var detailClass = db.VLClass.Find(id);


                //List<string> AV = new List<string>();
                //for(int i =0; i<db.AccountUser.Count();i++)
                //{
                //    var user = db.AccountUser.Find(i);
                //    var advisor = db.Advisor.Find(user.user_code);
                //    if (advisor !=null)
                //    {
                //        AV.Add(user.username);
                //    }

                //}
                //ViewBag.Advisor = new SelectList(AV);
                ViewBag.Advisor = db.Advisor.ToList();
                ViewBag.nameUser = db.AccountUser.ToList();
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                return View(detailClass);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // http post edit class
        [HttpPost]
        public ActionResult EditClass(VLClass cdeatilclass)
        {
           

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                db.Entry(cdeatilclass).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult ImportClass(HttpPostedFileBase postedfile)
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
                        return Json(new { success = true, message = "Import thành công" });
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
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            var result = false;
            count = 0;
            try
            {
                /* String path = Server.MapPath("/") + "\\import\\class.xlsx";*/

                var package = new ExcelPackage(new System.IO.FileInfo(filePath));
                var startColumn = 1;
                var startRow = 6;
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                object data = null;
                CP25Team09Entities db = new CP25Team09Entities();
                do
                {
                    data = worksheet.Cells[startRow, startColumn].Value;
                    Object stt = worksheet.Cells[startRow, startColumn].Value;
                    Object name_advisor = worksheet.Cells[startRow, startColumn + 1].Value;
                    Object email = worksheet.Cells[startRow, startColumn + 2].Value;
                    Object id_class = worksheet.Cells[startRow, startColumn + 3].Value;
                    Object hk_year = worksheet.Cells[startRow, startColumn + 4].Value;
                    var isSuccess = false;
                    if (data != null && stt != null && name_advisor != null && email != null && id_class != null)
                    {
                        serviceAccount.WriteDataFromExcelClass(email.ToString(), name_advisor.ToString(), id_class.ToString(), hk_year.ToString());
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
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            var listClass = Session["listClass"];
            var nameUser = Session["nameUser"];
            IEnumerable<AccountUser> name = nameUser as IEnumerable<AccountUser>;
            var hocky = Session["hocky"];
            List<VLClass> listStudent = (List<VLClass>)listClass;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    var ws = pck.Workbook.Worksheets.Add("Danh sách lớp");
             
                    ws.Cells["A1"].Value = "STT";
                    ws.Cells["B1"].Value = "Mã lớp";
                    ws.Cells["C1"].Value = "Tên cố vấn";
                    ws.Cells["D1"].Value = "Học kỳ";
 
                    var i = 1;
                    int rowStart = 2;
                    foreach (var item in listStudent)
                    {
                        ws.Cells[string.Format("A{0}", rowStart)].Value = i;
                        ws.Cells[string.Format("B{0}", rowStart)].Value = item.class_code;
                        if(item.advisor_code != null)
                        {
                            foreach (var itemUser in name)
                            {
                                if (item.advisor_code.Equals(itemUser.user_code))
                                {
                                    ws.Cells[string.Format("C{0}", rowStart)].Value = itemUser.user_name;
                                    break;
                                }
                            }
                        }
                        ws.Cells[string.Format("D{0}", rowStart)].Value = item.semester_name;
               
                        rowStart++;
                        i++;
                    }
                    ws.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + "Danhsachlop.xlsx");
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