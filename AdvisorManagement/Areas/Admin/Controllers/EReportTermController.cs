using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    public class EReportTermController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private StudentsMiddleware serviceStd = new StudentsMiddleware();
        private PlanMiddleware servicePlan = new PlanMiddleware();
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
            var listClass = serviceStd.getClassAdmin(year);
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
        public ActionResult GetReportClass(int id_class)
        {
            var reportClass = db.PlanClass.Where(x => x.id_class == id_class).ToList().OrderBy(x => x.number_title);
            return Json(new { data = reportClass, success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}