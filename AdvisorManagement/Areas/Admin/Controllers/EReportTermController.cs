using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using AdvisorManagement.Models.ViewModel;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    [LoginFilter]
    public class EReportTermController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private StudentsMiddleware serviceStd = new StudentsMiddleware();
        private PlanMiddleware servicePlan = new PlanMiddleware();
        private EreportMiddleware serviceEReport = new EreportMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        public void init()
        {
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
        }
        // GET: Admin/EReportTerm
        public ActionResult Index()
        {
            this.init();
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            var role = serviceStd.getRoles(User.Identity.Name);
            ViewBag.role = role;
            Session["role"] = role; 
            var year = servicePlan.getYear();           
            ViewBag.listYear = db.VLClass.DistinctBy(x => x.semester_name).OrderByDescending(x => x.semester_name).ToList();
            Session["yearNow"] = year;
            return View();
        }


        [HttpGet]
        public ActionResult GetListClass(int year)
        {
     
            var listClass = db.AccountUser.FirstOrDefault(x => x.email == User.Identity.Name).id_role == 1 ?  serviceStd.getClassAdmin(year) : serviceStd.getClassAdvisor(year,User.Identity.Name.ToString());
           
            return Json(new { data = listClass, success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportClass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.hocky = db.Semester.DistinctBy(x => x.semester_name).ToList();
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            ViewBag.Evol = this.getDes(id);
            ViewBag.CountE = this.checkIndexReport(this.getData(id));
            this.init();          
            var id_account = serviceStd.getID(User.Identity.Name);           
            var year = servicePlan.getYear();
            Session["yearNow"] = year;
            Session["id_class"] = id;
            var message = db.PlanStatus.FirstOrDefault(x => x.id_class == id).message;
            var modify_time = db.PlanStatus.FirstOrDefault(x => x.id_class == id).modify_time;            
            return View(db.PlanClass.Where(x => x.id_class == id).Where(y => y.year == year).ToList().OrderBy(x => x.number_title));
        }

        [HttpGet]
        public ActionResult GetReportClass(int? id_class)
        {
            return Json(new { data = this.getData(id_class), success = true}, JsonRequestBehavior.AllowGet);
        }

        private List<ReportCustom> getData(int? id_class)
        {
            var reportClass = db.PlanClass.Where(x => x.id_class == id_class).ToList().OrderBy(x => x.number_title);
            List<ReportCustom> lsR = new List<ReportCustom>();
            foreach (var item in reportClass)
            {
                var z = db.ProofPlan.Where(x => x.id_titleplan == item.id).Select(y => y.file_proof).ToList();
                string temp = "";
                foreach (var pro in z)
                {
                    temp += "\n" + pro.ToString() + "\n";
                }
                ReportCustom re = new ReportCustom(item, z.Count, z.Count > 0 ? temp : null, z.Count >= int.Parse(item.evaluate) ? "Hoàn thành" : "Chưa hoàn thành");
                lsR.Add(re);
            }
            return lsR;

        }

        private List<EvaluationAdvisor> getDes(int? id_class)
        {
            List<ReportCustom> DataFind = this.getData(id_class);
            int maxIndex = (int)DataFind.Max(x => x.number_title);
            int stack = this.checkIndexReport(DataFind);
            double checkEval = (stack / maxIndex) * 100;
            return db.EvaluationAdvisor.Where(x => x.rank_count <= checkEval && checkEval <  x.rank_end).ToList();
        }

        private int checkIndexReport(List<ReportCustom> DataFind)
        {
            int stack = 0;
            int maxIndex = (int)DataFind.Max(x => x.number_title);
            for (int i = 1; i <= maxIndex; i++)
            {
                stack += DataFind.Where(x => x.number_title == i && x.status == "Hoàn thành".Trim()).Count() > 0 ? 1 : 0;
            }
            return stack;
        }

        [HttpPost]
        public JsonResult ExportTemplateCode(int? year, int id_class)
        {
            var listPlan = db.PlanAdvisor.Where(x => x.year == year).OrderBy(x => x.number_title).ThenBy(x => x.content).ToList();
            List<PlanAdvisor> template = (List<PlanAdvisor>)listPlan;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    var package = serviceEReport.ExportTemplate(pck,this.getData(id_class),"K25-Test", User.Identity.Name.ToString(),year);
                    var fileOject = File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCaoKehoachCVHT-" + db.VLClass.FirstOrDefault(x => x.id == id_class ).class_code.ToString() +"-" +(year - 1) + "-" + year + ".xlsx");
                    return Json(fileOject, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetEvolTable()
        {
            var lsEvol = db.EvaluationAdvisor.ToList();
            if (lsEvol.Count() > 0)
            {
                return Json(new { data = lsEvol, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = lsEvol, success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        // Detail
        [HttpGet]
        public ActionResult Details(int? id)
        {
            var lsEvol = db.EvaluationAdvisor.FirstOrDefault(x => x.id == id);  
            if (lsEvol != null)
            {
                return Json(new { data = lsEvol, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = lsEvol, success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        // Create 
        public ActionResult CreateApi([Bind(Include = " rank_type,description,rank_des,rank_count,rank_end ")] EvaluationAdvisor avl)
        {
            if (avl.rank_type == null)
            {
                return Json(new { success = false, message = "Vui lòng điền loại tiêu chí" });
            }
            if (avl.description == null)
            {
                return Json(new { success = false, message = "Vui lòng điền mô tả tiêu chí" });
            }
            if (avl.rank_des == null)
            {
                return Json(new { success = false, message = "Vui lòng điền đánh giá tiêu chí" });
            }
            if (avl.rank_count == null)
            {
                return Json(new { success = false, message = "Vui lòng  điền giá trị bắt đầu" });
            }
            if (avl.rank_end == null)
            {
                return Json(new { success = false, message = "Vui lòng điền giá trị kết thúc" });
            }
            var userCheck = db.EvaluationAdvisor.Where(x => x.rank_type == avl.rank_type).ToList().Count();
            if (userCheck > 0)
            {
                return Json(new { success = false, message = "Tiêu chỉ đã tồn tại trong hệ thông" });
            }
            db.EvaluationAdvisor.Add(avl);
            db.SaveChanges();
            return Json(new { success = true, message = "Thêm dữ liêu tiêu chí thành công" });
        }
        // EDIT API 
        [HttpPost]
        public ActionResult EditApi([Bind(Include = "id,rank_type,description,rank_des,rank_count,rank_end ")] EvaluationAdvisor avl)
        {
            if (avl.rank_type == null)
            {
                return Json(new { success = false, message = "Vui lòng điền loại tiêu chí" });
            }
            if (avl.description == null)
            {
                return Json(new { success = false, message = "Vui lòng điền mô tả tiêu chí" });
            }
            if (avl.rank_des == null)
            {
                return Json(new { success = false, message = "Vui lòng điền đánh giá tiêu chí" });
            }
            if (avl.rank_count == null)
            {
                return Json(new { success = false, message = "Vui lòng  điền giá trị bắt đầu" });
            }
            if (avl.rank_end == null)
            {
                return Json(new { success = false, message = "Vui lòng điền giá trị kết thúc" });
            }
            var lsEvol = db.EvaluationAdvisor.FirstOrDefault(x => x.id == avl.id);
            if (lsEvol == null)
            {
                return Json(new { success = false, message = "Tiêu chỉ không tồn tại trong hệ thông" });
            }
            lsEvol.description = avl.description;
            lsEvol.rank_end = avl.rank_end; 
            lsEvol.rank_count   = avl.rank_count;
            lsEvol.rank_des = avl.rank_des;
            db.Entry(lsEvol).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { success = true, message = "Cập dữ liêu tiêu chí thành công" });
        }
        // Delete
        [HttpPost]
        public ActionResult DeleteApi(int id)
        {
            var userCheck = db.EvaluationAdvisor.FirstOrDefault(x => x.id == id);
            if (userCheck == null)
            {
                return Json(new { success = false, message = "Tiêu chỉ không có tồn tại trong hệ thông" });
            }
            db.EvaluationAdvisor.Remove(userCheck);
            db.SaveChanges();
            return Json(new { success = true, message = "Xoá dữ liêu tiêu chí thành công" });
        }
        // upload

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase postedfile, string mota, string semester, int id)
        {

            try
            {
                string filePath = string.Empty;

                if (postedfile == null || postedfile.ContentLength == 0)
                {
                    ViewBag.Error = "Vui lòng chọn file";
                    return Json(new { success = false, message = "Vui lòng chọn file" });
                }
                else if (mota == "")
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" });
                }
                else
                {
                    if (postedfile.FileName.EndsWith(".xls") || postedfile.FileName.EndsWith(".xlsx") || postedfile.FileName.EndsWith(".docx") || postedfile.FileName.EndsWith(".doc") ||
                        postedfile.FileName.EndsWith(".pdf") || postedfile.FileName.EndsWith(".png") || postedfile.FileName.EndsWith(".jpg"))
                    {
                        //var id = Session["id"];
                        var class_code = servicePlan.getClassCode((int)id);
                        var year = db.PlanClass.Find(id).year;
                        string path = Server.MapPath("~/Proof/" + year + "/" + class_code + "/");
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
                        var id_account = db.AccountUser.FirstOrDefault(x => x.email == User.Identity.Name).id;
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
    }
}