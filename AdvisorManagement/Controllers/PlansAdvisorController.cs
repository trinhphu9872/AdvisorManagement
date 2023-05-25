using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdvisorManagement.Models;
using AdvisorManagement.Middleware;
using Spire.Xls;
using System.IO;
using System.IO.Compression;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using System.Web.UI.WebControls;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text;
using System.Web.Services.Description;
using AdvisorManagement.Models.ViewModel;
using System.Web.UI;
using Ionic.Zip;
using OfficeOpenXml.DataValidation;
using MimeKit;
using AdvisorManagement.Hubs;
using MailKit;
using System.Security.Principal;

namespace AdvisorManagement.Controllers
{
    [LoginFilter]
    public class PlansAdvisorController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private StudentsMiddleware serviceStd = new StudentsMiddleware();
        private PlanMiddleware servicePlan = new PlanMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private MailServicesMiddleware mailService = new MailServicesMiddleware();
        public void init()
        {
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
        }
        private MailServicesMiddleware servicesMail = new MailServicesMiddleware();
        // GET: PlansAdvisor
        public ActionResult Index()
        {
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            this.init();
            var role = serviceStd.getRoles(User.Identity.Name);
            ViewBag.role = role;
            Session["role"] = role;
            var id_account = serviceStd.getID(User.Identity.Name);
            ViewBag.listProof = db.ProofPlan.Where(x => x.id_creator == id_account).ToList();
            var year = servicePlan.getYear();
            ViewBag.listYear = db.PlanAdvisor.Where(x => x.year < year).DistinctBy(x=>x.year).ToList();
            var user_code = db.AccountUser.Find(id_account).user_code;
            ViewBag.listYearAdvisor = db.VLClass.Where(x => x.semester_name != year.ToString()).Where(y=>y.advisor_code == user_code).DistinctBy(x => x.semester_name).OrderByDescending(x=>x.semester_name).ToList();
            Session["yearNow"] = year;            
            return View();
        }       

        // GET: PlansAdvisor/Create
        public ActionResult Create()
        {
            this.init();
            return View();
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var title = db.PlanAdvisor.SingleOrDefault(x => x.id == id);
                db.PlanAdvisor.Remove(title);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase postedfile, string mota, string semester)
        {

            try
            {
                string filePath = string.Empty;

                if (postedfile == null || postedfile.ContentLength == 0)
                {
                    ViewBag.Error = "Vui lòng chọn file";
                    return Json(new { success = false, message = "Vui lòng chọn file" });
                } else if (mota == "")
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" });
                }
                else
                {
                    if (postedfile.FileName.EndsWith(".xls") || postedfile.FileName.EndsWith(".xlsx") || postedfile.FileName.EndsWith(".docx") || postedfile.FileName.EndsWith(".doc") || 
                        postedfile.FileName.EndsWith(".pdf") || postedfile.FileName.EndsWith(".png") || postedfile.FileName.EndsWith(".jpg"))
                    {
                        var id = Session["id"];
                        var class_code = servicePlan.getClassCode((int)id);
                        var year = db.PlanClass.Find(id).year;
                        string path = Server.MapPath("~/Proof/"+ year + "/" + class_code + "/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        filePath = path + Path.GetFileName(postedfile.FileName);
                        string extension = Path.GetExtension(postedfile.FileName);
                        /*FileInfo fi = new FileInfo(filePath)*/
                        if (!System.IO.File.Exists(filePath))
                        {
                            postedfile.SaveAs(filePath);
                        }
                        else
                        {
                            return Json(new { success = false, message = "Tên file đã tồn tại" });
                        }
                        
                        if (Path.GetExtension(postedfile.FileName) == ".xls")
                        {
                            Workbook workbook = new Workbook();
                            workbook.LoadFromFile(filePath);
                            workbook.SaveToFile(filePath, ExcelVersion.Version2013);
                        }                    
                        var id_account = db.AccountUser.FirstOrDefault(x=>x.email == User.Identity.Name).id;
                        ProofPlan proof = new ProofPlan();
                        proof.content = mota;
                        proof.semester = semester;
                        proof.file_proof = postedfile.FileName;
                        proof.create_time = DateTime.Now;
                        proof.id_creator = id_account;
                        proof.id_titleplan = (int)id;
                        db.ProofPlan.Add(proof);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Upload thành công" });

                    }
                    else
                    {
                        return Json(new { success = false, message = "Vui lòng chọn file hợp lệ" });
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Vui lòng chọn file excel hoặc word" });
            }
        }

        public FileResult Download(int? id)
        {
            var prooffile = db.ProofPlan.Find(id);
            var nameProof = prooffile.file_proof;
            var id_title = prooffile.id_titleplan;
            var year = db.PlanClass.Find(id_title).year;
            var class_code = servicePlan.getClassCode((int)id_title);
            var filePath = Server.MapPath("~/Proof/" + year.ToString() + "/" + class_code + "/") +nameProof;
            return File(filePath, "application/force- download", Path.GetFileName(filePath));
        }

        [HttpPost]
        public ActionResult CreateTitle(int idtitle, string content, /*string HK1, string HK2, string HK3,*/ string describe, string source, string note, string evaluate)
        {

            try
            {
                if(content != "" && describe != "" && source !="" && evaluate != "") { 
                var year = servicePlan.getYear();
                PlanAdvisor plan = new PlanAdvisor();
                if (idtitle < 0)
                {
                    return Json(new { success = false, message = "Vui lòng điển số thứ tự lớn hơn 0 " });
                }
                bool isBoolean = bool.TryParse(int.Parse(evaluate.Trim()).ToString(), out _);
                if (isBoolean)
                {
                    return Json(new { success = false, message = "Vui lòng điền chữ số " });

                }
                if (int.Parse(evaluate.Trim()) < 0)
                {
                    return Json(new { success = false, message = "Vui lòng tiêu chí lớn hơn 0 " });
                }
                plan.number_title = idtitle;
                plan.content = content;
                plan.year = year;
                plan.describe= describe;
                plan.source= source;
                plan.note = note;
                plan.evaluate = evaluate;
                db.PlanAdvisor.Add(plan);
                db.SaveChanges();
                return Json(new { success = true, message = "Thêm thành công" });
                } else
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ trường dữ liệu" });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Vui lòng chọn file excel hoặc word" });
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            try
            {
                var title = db.PlanAdvisor.SingleOrDefault(x => x.id == id);
                return Json(new { success = true, T = title, message = "Lấy thông tin thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lấy thông tin thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateTittle(int id, int numTitle, string content, string describe, string source, string note, string evaluate)
        {
            try
            {
                if (numTitle != null && content != "" && describe != "" && source!= "" && evaluate !="")
                {
                    if (numTitle < 0)
                    {
                        return Json(new { success = false, message = "Vui lòng điển số thứ tự lớn hơn 0 " });
                    }
                    bool isBoolean = bool.TryParse(int.Parse(evaluate.Trim()).ToString(), out _);
                    if (isBoolean)
                    {
                        return Json(new { success = false, message = "Vui lòng điền chữ số " });

                    }
                    if (int.Parse(evaluate.Trim()) < 0)
                    {
                        return Json(new { success = false, message = "Vui lòng tiêu chí lớn hơn 0 " });
                    }
                    var title = db.PlanAdvisor.SingleOrDefault(x => x.id == id);
                    title.number_title = numTitle;
                    title.content = content;
                    title.describe = describe;
                    title.source = source;
                    title.note = note;
                    title.evaluate = evaluate;
          
                    db.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ các trường thông tin" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteProof(int id)
        {
            try
            {
                var prooffile = db.ProofPlan.Find(id);
                var nameProof = prooffile.file_proof;
                var id_title = prooffile.id_titleplan;
                var year = db.PlanClass.Find(id_title).year;
                var class_code = servicePlan.getClassCode((int)id_title);
                var filePath = Server.MapPath("~/Proof/" + year.ToString() + "/" + class_code + "/") + nameProof;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                var proof = db.ProofPlan.SingleOrDefault(x => x.id == id);
                db.ProofPlan.Remove(prooffile);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Copy(int id)
        {
            try
            {
                var year = servicePlan.getYear();
                var temp = db.PlanAdvisor.Where(x => x.year == id).Select(y => y.describe).ToList();
                var listPlan = db.PlanAdvisor.Where(x=>x.year == year).Select(y=>y.describe).ToList();
               
                if(id != year)
                {
                        var count = 0;
                        var checkExist = temp.Except(listPlan);
                        if(checkExist.Count() != 0)
                        {
                            foreach(var i in checkExist)
                            {
                                if(db.PlanAdvisor.Where(x => x.year == year).Where(y => y.describe == i).ToList().Count() ==0)
                                {
                                var newPlan = db.PlanAdvisor.Where(x => x.year == id).Where(y => y.describe == i).ToList();
                                foreach (var n in newPlan)
                                    {
                                        PlanAdvisor plan = new PlanAdvisor();
                                        plan.year = year;
                                        plan.number_title = n.number_title;
                                        plan.content = n.content;
                                        plan.describe = n.describe;
                                        plan.source = n.source;
                                        db.PlanAdvisor.Add(plan);
                                        db.SaveChanges();
                                        count++;
                                    }
                                }                                              
                            }
                        return Json(new { success = true, message = "Copy thành công, có thêm " + count +" đề mục mới" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, message = "Không có thêm sự thay đổi" }, JsonRequestBehavior.AllowGet);
                    }            
                }
                return Json(new { success = true, message = "Không có thêm sự thay đổi" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Copy thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetData(int year)
        {
            var datos = db.PlanAdvisor.Where(x=>x.year == year).ToList().OrderBy(x => x.number_title);

            return Json(new {data = datos }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExportTemplate(int? year)
        {
            var listPlan = db.PlanAdvisor.Where(x => x.year == year).OrderBy(x=>x.number_title).ThenBy(x=>x.content).ToList();
            List<PlanAdvisor> template = (List<PlanAdvisor>)listPlan;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    var package = servicePlan.ExportTemplateAdmin(pck, template, year);
                    var fileOject = File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KehoachCVHT_" + year + ".xlsx");
                    return Json(fileOject, JsonRequestBehavior.AllowGet);
                    /*  Response.Clear();
                      Response.Clear();
                      Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                      Response.AddHeader("content-disposition", "attachment; filename="+"KehoachCVHT_" + year + ".xlsx");
                      Response.BinaryWrite(pck.GetAsByteArray());
                      Response.End();*/
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: PlansAdvisor/Details/5
        public ActionResult DetailClass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            this.init();
            var role = serviceStd.getRoles(User.Identity.Name);
            ViewBag.role = role;
            Session["role"] = role;
            var id_account = serviceStd.getID(User.Identity.Name);
            ViewBag.listProof = db.ProofPlan.Where(x => x.id_creator == id_account).ToList();
            var year = servicePlan.getYear();
            var id_status = db.PlanStatus.FirstOrDefault(x => x.id_class == id).id_status;
            var nameStatus = db.StatusPlan.FirstOrDefault(x => x.id == id_status).status_name;
            ViewBag.yearClass = db.VLClass.Find(id).semester_name.ToString();
            ViewBag.yearNow = servicePlan.getYear().ToString();
            Session["yearNow"] = year;
            Session["id_class"] = id;
            Session["name_status"] = nameStatus;                  
            return View(db.PlanClass.Where(x => x.id_class == id).Where(y => y.year == year).ToList().OrderBy(x => x.number_title));
        }

        [HttpGet]
        public ActionResult GetDataAdvisor(int year)
        {
            var listClassAdvisor = serviceStd.getClass(User.Identity.Name, year);
            return Json(new { data = listClassAdvisor, success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateAdvisor(int id)
        {
            try
            {
                var title = db.PlanClass.Find(id);
                return Json(new { success = true, T = title, message = "Lấy thông tin thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lấy thông tin thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateTittleAdvisor(int id, int numTitle, string content, string hk1, string hk2, string hk3, string describe, string source, string note)
        {
            try
            {
                if (numTitle != null && content != "" && describe != "" && source != "" )
                {
                    var title = db.PlanClass.Find(id);
                    title.number_title = numTitle;
                    title.content = content;
                    title.hk1 = hk1;
                    title.hk2 = hk2;
                    title.hk3 = hk3;
                    title.describe = describe;
                    title.source = source;
                    title.note = note;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ các trường thông tin" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult CreateTitleAdvisor(int idtitle, string content, /*string HK1, string HK2, string HK3,*/ string describe, string source, string note, int year, int id_class)
        {

            try
            {
                if (content != "" && content != "" && describe != "" && source != "")
                {
                    PlanClass plan = new PlanClass();
                    plan.number_title = idtitle;
                    plan.content = content;
                    plan.year = year;
                    plan.describe = describe;
                    plan.source = source;
                    plan.note = note;
                    plan.id_class = id_class;
                    db.PlanClass.Add(plan);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Thêm thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ trường dữ liệu" });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Dữ liệu nhập quá độ dài" });
            }
        }

        [HttpPost]
        public ActionResult DeleteTitleAdvisor(int id)
        {
            try
            {
                var title = db.PlanClass.Find(id);
                db.PlanClass.Remove(title);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ExportTemplateAdvisor(int id_class)
        {
            var listPlan = db.PlanClass.Where(y=>y.id_class == id_class).OrderBy(x => x.number_title).ThenBy(x => x.content).ToList();
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
                    var fileOject = File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KehoachCVHT_"+(year - 1)+"-"+year+"_"+name_advisor+"_"+class_code+".xlsx");
                    return Json(fileOject, JsonRequestBehavior.AllowGet);                  
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public FileResult ExportAllTemplateAdvisor()
        {                 
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (ZipFile zip = new ZipFile())
                    {         
                        var id_status = db.StatusPlan.FirstOrDefault(x => x.status_name == "Hoàn thành").id;
                        var listClass = db.PlanStatus.Where(x=>x.id_status == id_status).DistinctBy(x=>x.id_class).Select(x=>x.id_class).ToList();
                        foreach (int id_class in listClass)
                        {
                            var listPlan = db.PlanClass.Where(y => y.id_class == id_class).OrderBy(x => x.number_title).ThenBy(x => x.content).ToList();
                            var year = (int)db.PlanClass.FirstOrDefault(x => x.id_class == id_class).year;
                            var class_code = db.VLClass.Find(id_class).class_code;
                            var advisor_code = db.VLClass.Find(id_class).advisor_code;
                            var user_name = db.AccountUser.FirstOrDefault(x => x.user_code == advisor_code).user_name;
                            var name_advisor = servicePlan.ConvertToUnsign(user_name);
                            List<PlanClass> template = (List<PlanClass>)listPlan;
                            using (ExcelPackage pck = new ExcelPackage())
                            {
                                var package = servicePlan.ExportTemplateAdvisor(pck, template, year, class_code);
                                var fileOject = File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KehoachCVHT_" + (year - 1) + "-" + year + "_" + name_advisor + "_" + class_code + ".xlsx");
                                zip.AddEntry(fileOject.FileDownloadName, fileOject.FileContents);
                            }
                        }                        
                        zip.Save(stream);
                        return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Zip, "KehoachCVHT.zip");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }           
        }


        // GET: PlansAdvisor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            PlanClass titlePlan = db.PlanClass.Find(id);
            if (titlePlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            this.init();
            ViewBag.role = Session["role"];
            var id_class = db.PlanClass.FirstOrDefault(x => x.id == id).id_class;
            ViewBag.id_class = id_class;
            Session["id"] = id;
            Session["class_code"] = db.VLClass.Find(id_class).class_code;
            ViewBag.hocky = db.Semester.DistinctBy(x => x.semester_name).ToList();
            return View(titlePlan);
        }

        [HttpGet]
        public ActionResult GetDataDetails()
        {
            var id = (int)Session["id"];
            var listProof = servicePlan.getListProof(User.Identity.Name, (int)id);
            return Json(new { data = listProof, success = false }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ProofPlan()
        {
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            this.init();
            var role = serviceStd.getRoles(User.Identity.Name);
            ViewBag.role = role;
            Session["role"] = role;
            var id_account = serviceStd.getID(User.Identity.Name);
            ViewBag.listProof = db.ProofPlan.Where(x => x.id_creator == id_account).ToList();
            var year = servicePlan.getYear();
            ViewBag.listYear = db.PlanAdvisor.Where(x => x.year < year).DistinctBy(x => x.year).ToList();
            var user_code = db.AccountUser.Find(id_account).user_code;
            ViewBag.listYearAdvisor = db.VLClass.DistinctBy(x => x.semester_name).OrderByDescending(x => x.semester_name).ToList();
            var listClass = db.VLClass.OrderBy(x => x.semester_name).ToList();
            ViewBag.listClass = listClass;
            Session["yearNow"] = year;

            return View();
        }

        [HttpGet]
        public ActionResult GetListClassPlan(int year)
        {
            var listClass = serviceStd.getClassAdmin(year);
            return Json(new { data = listClass, success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Proof(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VLClass vlclass = db.VLClass.Find(id);
            if (vlclass == null)
            {
                return HttpNotFound();
            }
            this.init();
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            ViewBag.role = Session["role"];           
            Session["id"] = id;
            Session["class_code"] = db.VLClass.Find(id).class_code;
            ViewBag.hocky = db.Semester.DistinctBy(x => x.semester_name).ToList();

            return View();
        }


        [HttpGet]
        public ActionResult GetListProof(string semester)
        {
            var id_class = (int)Session["id"];
            var listProof = servicePlan.getListProofAdmin(semester, id_class);
            return Json(new { data = listProof, success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportAllProof()
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        var id_class = (int)Session["id"];
                        var year = db.VLClass.Find(id_class).semester_name;
                        var class_code = db.VLClass.Find(id_class).class_code;                                                                          
                        var filePath = Server.MapPath("~/Proof/" + year.ToString() + "/" + class_code + "/");
                        if (Directory.Exists(filePath))
                        {
                            var files = Directory.GetFiles(filePath);
                            if(files.Count() > 0)
                            {
                                zip.AddFiles(files, false, "");
                                zip.Save(stream);
                                var fileOject = File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Zip, "Minhchung_kehoachCVHT_" + (int.Parse(year) - 1) + "-" + year + "_" + class_code + ".zip");
                                return Json(new { success = true, file = fileOject }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { success = false, message = "Kế hoạch chưa có minh chứng nào" }, JsonRequestBehavior.AllowGet);
                            }
                            
                        }
                        else
                        {
                            return Json(new { success = false, message = "Kế hoạch chưa có minh chứng nào" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public ActionResult SubmitPlan(int id_class)
        {
            var id_status = db.StatusPlan.FirstOrDefault(x => x.status_name == "Chờ duyệt").id;
            PlanStatus planStatus = db.PlanStatus.SingleOrDefault(x=>x.id_class == id_class);
            planStatus.id_status = id_status;
            planStatus.message = null;
            planStatus.modify_time = DateTime.Now;
            db.SaveChanges();

            return Json(new {  success = true, message = "Nộp kế hoạch thành công" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UndoPlan(int id_class)
        {
            var id_status = db.StatusPlan.FirstOrDefault(x => x.status_name == "Đang làm").id;
            PlanStatus planStatus = db.PlanStatus.SingleOrDefault(x => x.id_class == id_class);
            planStatus.id_status = id_status;
            db.SaveChanges();
            return Json(new { success = true, message = "Hoàn tác thành công" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReturnPlan(int id_class, string message, string subject)
        {
            if(message == "" || subject == "")
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ trường dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
            var checkStatus = db.PlanStatus.FirstOrDefault(x => x.id_class == id_class).id_status;
            var id_status = db.StatusPlan.FirstOrDefault(x => x.status_name == "Đang làm").id;
            var email_advisor = servicePlan.getEmailAdvisor(id_class);
            PlanStatus planStatus = db.PlanStatus.SingleOrDefault(x => x.id_class == id_class);
            planStatus.id_status = id_status;
            planStatus.message = message;
            planStatus.modify_time = DateTime.Now;
            db.SaveChanges();
            if (!mailService.IsValidEmail(email_advisor))
            {

                return Json(new { success = false, message = "Mail không hợp lệ " }, JsonRequestBehavior.AllowGet);
            }

            if (db.AccountUser.Where(x => x.email == email_advisor).ToList().Count() > 0)
            {
                var id_account = db.AccountUser.FirstOrDefault(x => x.email == email_advisor).id;
                NotificationHub objNotifHub = new NotificationHub();
                Annoucement objNotif = new Annoucement();
                objNotif.title = subject;
                objNotif.message = message;
                db.Annoucement.Add(objNotif);
                db.SaveChanges();
                Notification notif = new Notification();
                notif.id_notification = objNotif.id;
                notif.send_to = email_advisor;
                notif.create_time = DateTime.Now;
                notif.is_read = false;
                notif.id_user = serviceAccount.getIdMail(User.Identity.Name);

                //MailRequest mail = new MailRequest();
                //mail.To = email_advisor;
                //mail.Message = message;
                //mail.Subject = subject;
                //mailService.MailSendRequest(mail, this.LoadHtmlTemplate());
                db.Configuration.ProxyCreationEnabled = false;
                db.Notification.Add(notif);
                db.SaveChanges();

                objNotifHub.SendNotification(email_advisor);

                return Json(new { success = true, message = "Trả về thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Gửi không thành công" }, JsonRequestBehavior.AllowGet);

            }            
        }

        public ActionResult BrowsePlan(int id_class, string message, string subject)
        {
            if (message == "" || subject == "")
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ trường dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
            var email_advisor = servicePlan.getEmailAdvisor(id_class);
            var id_status = db.StatusPlan.FirstOrDefault(x => x.status_name == "Hoàn thành").id;
            PlanStatus planStatus = db.PlanStatus.SingleOrDefault(x => x.id_class == id_class);
            planStatus.id_status = id_status;
            planStatus.modify_time = DateTime.Now;
            db.SaveChanges();
            if (!mailService.IsValidEmail(email_advisor))
            {

                return Json(new { success = false, message = "Mail không hợp lệ " }, JsonRequestBehavior.AllowGet);
            }

            if (db.AccountUser.Where(x => x.email == email_advisor).ToList().Count() > 0)
            {
                var id_account = db.AccountUser.FirstOrDefault(x => x.email == email_advisor).id;
                NotificationHub objNotifHub = new NotificationHub();
                Annoucement objNotif = new Annoucement();
                objNotif.title = subject;
                objNotif.message = message;
                db.Annoucement.Add(objNotif);
                db.SaveChanges();
                Notification notif = new Notification();
                notif.id_notification = objNotif.id;
                notif.send_to = email_advisor;
                notif.create_time = DateTime.Now;
                notif.is_read = false;
                notif.id_user = serviceAccount.getIdMail(User.Identity.Name);

                //MailRequest mail = new MailRequest();
                //mail.To = email_advisor;
                //mail.Message = message;
                //mail.Subject = subject;
                //mailService.MailSendRequest(mail, this.LoadHtmlTemplate());
                db.Configuration.ProxyCreationEnabled = false;
                db.Notification.Add(notif);
                db.SaveChanges();

                objNotifHub.SendNotification(email_advisor);

                return Json(new { success = true, message = "Duyệt kế hoạch thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Duyệt không thành công" }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult PlanSubmited()
        {
            var year = servicePlan.getYear();
            Session["yearNow"] = year;
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            this.init();
            var role = serviceStd.getRoles(User.Identity.Name);
            ViewBag.role = role;
            Session["role"] = role;           
            ViewBag.listYearAdvisor = db.VLClass.DistinctBy(x => x.semester_name).OrderByDescending(x => x.semester_name).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult GetListPlanSubmited(string year)
        {
            var listPlanSubmited = servicePlan.getListPlanSatus(year);
            return Json(new { data = listPlanSubmited, success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListPlanSubmitedClass(int id_class)
        {
            var listPlanSubmited = db.PlanClass.Where(x => x.id_class == id_class).ToList().OrderBy(x => x.number_title);
            var id_status = db.StatusPlan.FirstOrDefault(x => x.status_name == "Hoàn thành").id;
            if(db.PlanStatus.FirstOrDefault(x=>x.id_class == id_class).id_status == id_status)
            {
                return Json(new { data = listPlanSubmited, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = listPlanSubmited, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetListAdvisor()
        {                        
            var listAdvisor = servicePlan.getListAdvisorUnfinished();
            return Json(new { data = listAdvisor, success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRemind()
        {

            string emailUser = User.Identity.Name;
            var listAdvisor = servicePlan.getListAdvisorUnfinished();
            List<ListAdvisorUnfinished> advisor = (List<ListAdvisorUnfinished>)listAdvisor;
            string email = "";
            foreach (var item in advisor)
            {
                if(email != "")
                {
                    email += ", " + item.email;
                }
                else
                {
                    email += item.email;
                }

            }
            return Json(new { data = email, emailUser = emailUser, success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendMailRemind(string to, string subject, string message)
        {
            string [] arrEmail = to.Trim().Split(',');
            foreach (var item in arrEmail) {
                var id_account = db.AccountUser.FirstOrDefault(x => x.email == item.Trim()).id;
                NotificationHub objNotifHub = new NotificationHub();
                Annoucement objNotif = new Annoucement();
                objNotif.title = subject;
                objNotif.message = message;
                db.Annoucement.Add(objNotif);
                db.SaveChanges();
                Notification notif = new Notification();
                notif.id_notification = objNotif.id;
                notif.send_to = item.Trim();
                notif.create_time = DateTime.Now;
                notif.is_read = false;
                notif.id_user = serviceAccount.getIdMail(User.Identity.Name);

                db.Configuration.ProxyCreationEnabled = false;
                db.Notification.Add(notif);
                db.SaveChanges();
                objNotifHub.SendNotification(item.Trim());
            }
            return Json(new { message = "Nhắc nhở thành công", success = true }, JsonRequestBehavior.AllowGet);
        }
        //public string LoadHtmlTemplate()
        //{
        //    // Load the HTML template from a file, database or other source
        //    // Here's an example of loading from a file:

        //    var templateFilePath = Server.MapPath("~/Views/Shared/MailTemplate.html");

        //    string htmlTemplate = "";

        //    using (StreamReader reader = new StreamReader(templateFilePath))
        //    {
        //        htmlTemplate = reader.ReadToEnd();
        //    }

        //    return htmlTemplate;


        //    //return "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body bgcolor=\"#f5f5f5\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\" offset=\"0\" style=\"padding:70px 0 70px 0;\">\r\n    <table width=\"600\" height=\"auto\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color:#fdfdfd; border:1px solid #dcdcdc; border-radius:3px !important;\">\r\n        <tr>\r\n            <td width=\"600\" height=\"auto\" bgcolor=\"#000\" border=\"0\" style=\"padding:36px 48px; display:block; margin: 0px auto;\">\r\n                <h1 style=\"color:#ffffff; font-family:&quot; Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif; font-size:30px; line-height:150%; font-weight:300; margin:0; text-align:left;\">[[Subject]]</h1>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td width=\"600\" bgcolor=\"#fdfdfd\" border=\"0\" style=\"color:#737373; font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif; font-size:14px; line-height:150%; text-align:left; padding:48px;\">\r\n                <p style=\"margin:0 0 16px;\">Xin chào giảng viên <b>[[Name]]</b></p>\r\n                <p style=\"margin:0 0 16px;\">[[Message]]</p>\r\n                <p style=\"margin:0 0 16px;\">Trân trọng,</p>\r\n\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td width=\"600\" border=\"0\" style=\"padding:0 48px 48px 48px; color:#707070; font-family:Arial; font-size:12px; line-height:125%; text-align:center;\">\r\n                <b>KHOA CÔNG NGHỆ THÔNG TIN - TRƯỜNG ĐẠI HỌC VĂN LANG</p>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";
        //}
    }
}
