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
using Spire.Pdf.OPC;

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
                if (db.VLClass.Where(x => x.class_code.Trim().ToUpper() == class_code.Trim().ToUpper() && x.semester_name == stringYear).Count() == 0)
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
        public ActionResult UpdateClass(int id, string class_code, string advisor_code, int year)
        {
            if(class_code.Trim() != "")
            {
                VLClass vLClass = db.VLClass.Find(id);
                var stringYear = year.ToString();
                if (vLClass.class_code.Trim().ToUpper() != class_code.Trim().ToUpper())
                {
                    if (db.VLClass.Where(x => x.class_code.Trim().ToUpper() == class_code.Trim().ToUpper() && x.semester_name == stringYear).Count() == 0)
                    {
                        vLClass.class_code = class_code;
                        vLClass.advisor_code = advisor_code;
                        vLClass.course = serviceAccount.getCours(class_code);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Mã lớp học đã tồn tại" });
                    }
                }
                else
                {
                    vLClass.class_code = class_code;
                    vLClass.advisor_code = advisor_code;
                    vLClass.course = serviceAccount.getCours(class_code);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
                }                             
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
                        int countEmpty = 0;
                        int countExist = 0;
                        int checkAdv_code = 0;
                        int checkEmail = 0;
                        string msgError = "";
                        string msg = "";
                        ImportData(out count, filePath, out msgError, out countEmpty, out countExist, out checkAdv_code, out checkEmail);
                        if (count > 0 || countEmpty > 0 || countExist > 0 || checkAdv_code > 0 || checkEmail > 0) {
                            if(count > 0)
                            {
                                msg += "Thêm thành công " + count + " lớp" + "\n";
                            }
                            if(checkAdv_code> 0)
                            {
                                msg += "Có " + checkAdv_code + " cố vấn có mã giảng viên không hợp lệ" + "\n";
                            }
                            if (checkEmail > 0)
                            {
                                msg += "Có " + checkEmail + " cố vấn có email không hợp lệ" + "\n";
                            }
                            if (countEmpty> 0)
                            {
                                msg += "Có " + countEmpty + " cố vấn chứa dữ liệu trống, không thực hiện thay đổi" + "\n";
                            }
                            if(countExist > 0)
                            {
                                msg += "Có " + countExist + " lớp đã tồn tại, không thực hiện thay đổi";
                            }
                            return Json(new { success = true, message = msg});
                        }
                        else if(msgError != "")
                        {
                            return Json(new { success = false, message = msgError });
                        }else if (count == 0)
                        {
                            return Json(new { success = true, message = "Không có sự thay đổi nào" });
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
        private bool ImportData(out int count, string filePath, out string msgError, out int countEmpty, out int countExist, out int checkAdv_code, out int checkEmail)
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            var result = false;
            count = 0;
            countEmpty = 0;
            countExist = 0;
            msgError = "";
            checkAdv_code = 0;
            checkEmail = 0;
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
                    Object advisor_code = worksheet.Cells[startRow, startColumn + 1].Value;
                    Object name_advisor = worksheet.Cells[startRow, startColumn + 2].Value;
                    Object email = worksheet.Cells[startRow, startColumn + 3].Value;
                    Object id_class = worksheet.Cells[startRow, startColumn + 4].Value;
                    Object duthua = worksheet.Cells[startRow, startColumn + 5].Value;
                    Object thieu = worksheet.Cells[5, startColumn + 4].Value;
                    var year = servicePlan.getYear();
                    var isSuccess = false;  
                    if (data == null && advisor_code == null && name_advisor == null && email == null && id_class == null)
                    {                      
                        break;
                    }else if(thieu == null && duthua == null)
                    {
                        msgError = "File import thiếu cột dữ liệu, import thất bại";
                        break;
                    }else if(duthua != null)
                    {
                        msgError = "File import chứa cột dữ liệu thừa, import thất bại";
                        break;
                    }else if(data == null || advisor_code == null || name_advisor == null || email == null || id_class == null ||
                        data.ToString().Trim() == "" || advisor_code == null || name_advisor.ToString().Trim() == "" || email.ToString().Trim() == "" || id_class.ToString().Trim() == "")
                    {
                        countEmpty++;                      
                    }
                    else
                    {
                        string[] mailGet = email.ToString().Trim().Split('@');                        
                        if (mailGet[mailGet.Length - 1] == "vlu.edu.vn")
                        {
                            string advisorCode = mailGet[0] + "_cntt";
                            if (advisorCode.ToString() == advisor_code.ToString())
                            {
                                var idClass = id_class.ToString().Split('\n');
                                foreach (var item in idClass)
                                {
                                    var child = item.Trim().Split(',');
                                    for (var i = 0; i < child.Length; i++)
                                    {
                                        var stringClass = "";
                                        if (child[i].Length <= 3)
                                        {
                                            var number = child[0].Substring(child[0].Length - 2);
                                            foreach (Char c in number)
                                            {
                                                if (!Char.IsDigit(c))
                                                {
                                                    stringClass = child[0].Substring(0, child[0].Length - 1) + child[i];
                                                    break;
                                                }
                                                else
                                                {
                                                    stringClass = child[0].Substring(0, child[0].Length - 2) + child[i];
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            stringClass = child[i].ToString();
                                        }
                                        var check = serviceAccount.WriteDataFromExcelClass(email.ToString(), name_advisor.ToString(), stringClass.ToString().Trim(), year.ToString());
                                        if (check)
                                        {
                                            count++;
                                        }
                                        else
                                        {
                                            countExist++;
                                        }
                                    }                                   
                                }
                            }
                            else
                            {
                                checkAdv_code++;
                            }
                        }
                        else
                        {
                             checkEmail++;
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

        [HttpPost]
        public JsonResult ExcelExport(int year)
        {
           /* var year = servicePlan.getYear();*/
            var stringYear = year.ToString();
            var listClass = db.VLClass.Where(x=>x.semester_name == stringYear).OrderByDescending(x => x.create_time).ToList();
            var nameUser = Session["nameUser"];
            IEnumerable<AccountUser> name = nameUser as IEnumerable<AccountUser>;
            var hocky = Session["hocky"];
            List<VLClass> listStudent = (List<VLClass>)listClass;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    var package = serviceAccount.getExcelPackage(pck, year, listStudent);
                    var fileOject = File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CVHT_Phancong_" + (year - 1) + "-" + year + ".xlsx");
                    return Json(fileOject, JsonRequestBehavior.AllowGet);
                  /*  Response.Clear();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + "CVHT_Phancong_"+ (year-1) +"-" + year +".xlsx");
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.End();*/
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /* [HttpPost]
         public JsonResult ExportTemplateAdvisor(int id_class)
         {
             var listPlan = db.PlanClass.Where(y => y.id_class == id_class).OrderBy(x => x.number_title).ThenBy(x => x.content).ToList();
             var year = (int)db.PlanClass.FirstOrDefault(x => x.id_class == id_class).year;
             var class_code = db.VLClass.Find(id_class).class_code;
             var advisor_code = db.VLClass.Find(id_class).advisor_code;
             var user_name = db.AccountUser.FirstOrDefault(x => x.user_code == advisor_code).user_name;
             var name_advisor = servicePlan.ConvertToUnsign(user_name);
             List<PlanClass> template = (List<PlanClass>)listPlan;
             try
             {
                 using (ExcelPackage pck = new ExcelPackage())
                 {
                     var package = servicePlan.ExportTemplateAdvisor(pck, template, year, class_code);
                     var fileOject = File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KehoachCVHT_" + (year - 1) + "-" + year + "_" + name_advisor + "_" + class_code + ".xlsx");
                     return Json(fileOject, JsonRequestBehavior.AllowGet);
                 }
             }
             catch (Exception ex)
             {
                 throw;
             }
         }*/

        public FileResult Download()
        {            
            var filePath = Server.MapPath("~/FileSource/CVHT_Phancong_2022-2023.xlsx");
            return File(filePath, "application/force- download", Path.GetFileName(filePath));
        }
    }
}