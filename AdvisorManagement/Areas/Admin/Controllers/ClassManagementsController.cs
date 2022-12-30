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
        private MenuMiddleware serviceMenu = new MenuMiddleware();

        private string routePermission = "Admin/ClassManagements";
        CP25Team09Entities db = new CP25Team09Entities();
        // GET: Admin/ClassManagements
        public ActionResult Index()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                 var listClass = db.VLClass.ToList();
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
        public ActionResult EditClass(int id)
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
    }
}