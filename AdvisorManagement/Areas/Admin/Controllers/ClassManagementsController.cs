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
using System.Web.UI.WebControls;
using OfficeOpenXml.Style;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    [LoginFilter]
    public class ClassManagementsController : Controller
    {

        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private PlanMiddleware servicePlan = new PlanMiddleware();
        private StudentsMiddleware serviceStd = new StudentsMiddleware();

        private string routePermission = "Admin/ClassManagements";
        CP25Team09Entities db = new CP25Team09Entities();
        public void init()
        {
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
        }
        // GET: Admin/ClassManagements
        public ActionResult Index()
        {
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            this.init();
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                var listClass = db.VLClass.OrderByDescending(x=>x.create_time).ToList();
                ViewBag.listClass = listClass;
                Session["listClass"] = listClass;
                ViewBag.Advisor = db.Advisor.ToList();
                ViewBag.nameUser = db.AccountUser.ToList();
                Session["nameUser"] = db.AccountUser.ToList();
                var year = servicePlan.getYear();
                var yearNow = year.ToString();
                var namhoc = db.VLClass.Where(x=>x.semester_name != yearNow).DistinctBy(x=>x.semester_name).OrderByDescending(x=>x.semester_name).ToList();
                ViewBag.hocky = namhoc;
                Session["hocky"] = namhoc;
                Session["yearNow"] = year;
                return View(listClass);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult GetData(int year)
        {
            var listClast = serviceStd.getClassAdmin(year);          
            return Json(new { data = listClast }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            try
            {
                var detailClass = db.VLClass.Find(id);

                //ViewBag.role = roleMenu.id_role;
                //ViewBag.menu = roleMenu.id_menu;
                return Json(new { success = true, R = detailClass.id, R_semester = detailClass.semester_name, R_advisor = detailClass.advisor_code, message = "Lấy thông tin thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lấy thông tin thất bại" }, JsonRequestBehavior.AllowGet);
            }
            //ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            //ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            //ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            //ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            //if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            //{
            //    var detailClass = db.VLClass.Find(id);


            //    //List<string> AV = new List<string>();
            //    //for(int i =0; i<db.AccountUser.Count();i++)
            //    //{
            //    //    var user = db.AccountUser.Find(i);
            //    //    var advisor = db.Advisor.Find(user.user_code);
            //    //    if (advisor !=null)
            //    //    {
            //    //        AV.Add(user.username);
            //    //    }

            //    //}
            //    //ViewBag.Advisor = new SelectList(AV);
            //    ViewBag.Advisor = db.Advisor.ToList();
            //    ViewBag.nameUser = db.AccountUser.ToList();
            //    ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            //    return View(detailClass);
            //}
            //else
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
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
        //public ActionResult Create()
        //{
        //    ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
        //    ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
        //    ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

        //    ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
        //    ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code");
        //    ViewBag.class_code = new SelectList(db.VLClass, "class_code", "class_code");
        //    ViewBag.hocky = db.Semester.ToList().OrderBy(x => x.scholastic);
        //    Session["hocky"] = db.Semester.ToList().OrderBy(x => x.scholastic);
        //    return View();
        //}

        // POST: Admin/VLClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(string class_code, string advisor_code, int year)
        {
            if(class_code.Trim() != "")
            {
                var stringYear = year.ToString();
                if (db.VLClass.Where(x => x.class_code.ToUpper() == class_code.ToUpper() && x.semester_name == stringYear).Count() == 0)
                {
                    VLClass vLClass = new VLClass();
                    vLClass.class_code = class_code;
                    vLClass.advisor_code = advisor_code;
                    vLClass.create_time = DateTime.Now;
                    vLClass.semester_name = year.ToString();
                    vLClass.course = serviceAccount.getCours(class_code);
                    db.VLClass.Add(vLClass);
                    db.SaveChanges();
                    servicePlan.AssignmentTemplate(vLClass.id, year);
                    servicePlan.PlanStatus(vLClass.id);
                    return Json(new { success = true, message = "Thêm thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Mã lớp học đã tồn tại" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Vui lòng nhập mã lớp học" });
            }      

        }

        // GET: Admin/VLClasses/Delete/5
        [HttpPost]
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
           
            var listPlan = db.PlanClass.Where(x => x.id_class == id).ToList();
            foreach(var item in listPlan)
            {
               var listProof = db.ProofPlan.Where(x=>x.id_titleplan == item.id).ToList();
               if(listProof.Count() > 0)
               {
                  foreach(var pr in listProof)
                    {                        
                        var year = item.year;
                        var class_code = servicePlan.getClassCode((int)item.id);
                        var filePath = Server.MapPath("~/Proof/" + year.ToString() + "/" + class_code);
                        if (Directory.Exists(filePath))
                        {
                            Directory.Delete(filePath, true);
                        }
                        db.ProofPlan.Remove(pr);
                    }
               }
               db.PlanClass.Remove(item);
            }
            var listStd = db.ListStudents.Where(x => x.id_class == id).ToList();
            foreach(var item in listStd)
            {
                db.ListStudents.Remove(item);
            }
            var listPlanStatus = db.PlanStatus.Where(x=>x.id_class ==id).ToList();
            foreach(var item in listPlanStatus)
            {
                db.PlanStatus.Remove(item);
            }
            
            db.VLClass.Remove(vLClass);
            db.SaveChanges();
            return Json(new { success = true, message = "Xóa lớp học thành công" });
        }
       
        // HTTP GET
        [HttpGet]
        public ActionResult EditClass(int id)
        {

            try
            {
                var detailClass = db.VLClass.Find(id);

                //ViewBag.role = roleMenu.id_role;
                //ViewBag.menu = roleMenu.id_menu;
                return Json(new { success = true, R = detailClass.id, R_class_code = detailClass.class_code, R_advisor = detailClass.advisor_code, message = "Lấy thông tin thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lấy thông tin thất bại" }, JsonRequestBehavior.AllowGet);
            }

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
            //ViewBag.Advisor = db.Advisor.ToList();
            //ViewBag.nameUser = db.AccountUser.ToList();
            //ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            //}
            //else
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
        }
        // http post edit class
        [HttpPost]
        public ActionResult UpdateClass(int id, string class_code, string advisor_code)
        {
            if(class_code.Trim() != "")
            {
                VLClass vLClass = db.VLClass.Find(id);
                vLClass.class_code = class_code;
                vLClass.advisor_code = advisor_code;
                vLClass.course = serviceAccount.getCours(class_code);
                db.SaveChanges();
                return Json(new { success = true, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Vui lòng nhập mã lớp học" }, JsonRequestBehavior.AllowGet);
            }
           
        }
        //}
        //else
        //{
        //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //}


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
                        string msgError = "";
                        ImportData(out count, filePath, out msgError);
                        if(msgError != "")
                        {
                            return Json(new { success = false, message = msgError });
                        }                       
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
        private bool ImportData(out int count, string filePath, out string msgError)
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            var result = false;
            count = 0;
            msgError = "";
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
                    Object name_advisor = worksheet.Cells[startRow, startColumn + 1].Value;
                    Object email = worksheet.Cells[startRow, startColumn + 2].Value;
                    Object id_class = worksheet.Cells[startRow, startColumn + 3].Value;
                    Object duthua = worksheet.Cells[startRow, startColumn + 4].Value;
                    var year = servicePlan.getYear();
                    var isSuccess = false;                   
                    if (id_class == null)
                    {
                        msgError = "File import thiếu cột dữ liệu, import thất bại";
                        break;
                    }
                    if(duthua != null)
                    {
                        msgError = "File import chứa cột dữ liệu thừa, import thất bại";
                        break;
                    }
                    if(data == null || name_advisor == null || email == null || id_class == null ||
                        data.ToString().Trim() == "" || name_advisor.ToString().Trim() == "" || email.ToString().Trim() == "" || id_class.ToString().Trim() == "")
                    {
                        msgError = "File import có dữ liệu bị trống, import thất bại";
                        break;
                    }                    
                    if (data != null && name_advisor != null && email != null && id_class != null)
                    {
                        serviceAccount.WriteDataFromExcelClass(email.ToString(), name_advisor.ToString(), id_class.ToString(), year.ToString());
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
            var year = servicePlan.getYear();
            var listClass = db.VLClass.OrderByDescending(x => x.create_time).ToList();
            var nameUser = Session["nameUser"];
            IEnumerable<AccountUser> name = nameUser as IEnumerable<AccountUser>;
            var hocky = Session["hocky"];
            List<VLClass> listStudent = (List<VLClass>)listClass;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    var package = serviceAccount.getExcelPackage(pck, year, listStudent);
                    Response.Clear();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + "CVHT_Phancong_"+ (year-1) +"-" + year +".xlsx");
                    Response.BinaryWrite(package.GetAsByteArray());
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