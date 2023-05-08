using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using AdvisorManagement.Models.ViewModel;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    }
}