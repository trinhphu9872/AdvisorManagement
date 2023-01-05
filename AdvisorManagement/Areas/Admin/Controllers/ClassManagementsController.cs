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
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                var listClass = db.VLClass.ToList();
                ViewBag.listClass = listClass;
                ViewBag.nameUser = db.AccountUser.ToList();
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                ViewBag.hocky = db.Semester.ToList();
                return View(listClass);
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
            ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code");
            ViewBag.class_code = new SelectList(db.VLClass, "class_code", "class_code");
            return View();
        }

        // POST: Admin/VLClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create([Bind(Include = "id,class_code,advisor_code,create_time,update_time")] VLClass vLClass)
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
            VLClass vLClass = db.VLClass.Find(id);
            db.VLClass.Remove(vLClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // HTTP GET
        public ActionResult EditClass(int id )
        {
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
                    Object name_advisor = worksheet.Cells[startRow, startColumn + 2].Value;
                    Object email = worksheet.Cells[startRow, startColumn + 3].Value;
                    Object id_class = worksheet.Cells[startRow, startColumn + 4].Value;
                    var isSuccess = false;
                    if (data != null && stt != null && name_advisor != null && email != null && id_class != null)
                    {
                        serviceAccount.WriteDataFromExcelClass(email.ToString(), name_advisor.ToString(), id_class.ToString());
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
    }
}