using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvisorManagement.Models;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    public class VLClassController : Controller
    {
        CP25Team09Entities db = new CP25Team09Entities();
        // GET: Admin/ManagementListClass
        public ActionResult Index()
        {
            var listClass = db.VLClass.ToList();
            ViewBag.nameUser = db.AccountUser.ToList();
            return View(listClass);
        }
        public ActionResult EditClass(string classCode) {

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
        [HttpPost]
        public ActionResult EditClass(VLClass cdeatilclass)
        {
            db.Entry(cdeatilclass).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}