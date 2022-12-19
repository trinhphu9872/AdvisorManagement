using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    public class ClassManagementsController : Controller
    {
      
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private string routePermission = "Admin/ClassManagements";
        CP25Team09Entities db = new CP25Team09Entities();
        // GET: Admin/ClassManagements
        public ActionResult Index()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                 var listClass = db.VLClass.ToList();
                ViewBag.nameUser = db.AccountUser.ToList();
                return View(listClass);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult EditClass(string classCode)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                var detailClass = db.VLClass.Find(classCode);


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
                return View(detailClass);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult EditClass(VLClass cdeatilclass)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                db.Entry(cdeatilclass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}