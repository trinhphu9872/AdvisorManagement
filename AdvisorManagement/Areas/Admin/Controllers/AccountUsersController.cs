using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdvisorManagement.Middleware;
using AdvisorManagement.Models;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountUsersController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private string routePermission = "Admin/AccountUsers";

        //GET: Admin/AccountUsers
        public ActionResult Index()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                var accountUser = db.AccountUser.Include(a => a.Role);
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(accountUser.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // GET: Admin/AccountUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AccountUser accountUser = db.AccountUser.Find(id);
                if (accountUser == null)
                {
                    return HttpNotFound();
                }
                return View(accountUser);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Admin/AccountUsers/Create
        public ActionResult Create()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                ViewBag.id_Role = new SelectList(db.Role, "id", "role_name");
                AccountUser user = new AccountUser();
                return View(user);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: Admin/AccountUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(AccountUser accountUser, HttpPostedFileBase ImageUpload)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                if (ModelState.IsValid)
                {
                    //accountUser.ID = Guid.NewGuid();
                    if (accountUser.ImageUpload != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(accountUser.ImageUpload.FileName).ToString();
                        string extension = Path.GetExtension(accountUser.ImageUpload.FileName);
                        filename = filename + extension;
                        accountUser.img_profile = "~/Images/imageProfile/" + filename;
                        accountUser.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/imageProfile/"), filename));
                    }

                    accountUser.create_time = DateTime.Now;
                    accountUser.update_time = DateTime.Now;
                    db.AccountUser.Add(accountUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.id_Role = new SelectList(db.Role, "id", "role_name", accountUser.id_role);
                return View(accountUser);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // GET: Admin/AccountUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AccountUser accountUser = db.AccountUser.Find(id);
                if (accountUser == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id_Role = new SelectList(db.Role, "id", "role_name", accountUser.id_role);
                return View(accountUser);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // POST: Admin/AccountUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(AccountUser accountUser)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                if (ModelState.IsValid)
                {
                    if (accountUser.ImageUpload != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(accountUser.ImageUpload.FileName).ToString();
                        string extension = Path.GetExtension(accountUser.ImageUpload.FileName);
                        filename = filename + extension;
                        accountUser.img_profile = "~/Images/imageProfile/" + filename;
                        accountUser.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/imageProfile/"), filename));
                    }

                    accountUser.update_time = DateTime.Now;
                    db.Entry(accountUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.id_Role = new SelectList(db.Role, "id", "role_name", accountUser.id_role);
                return View(accountUser);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }   
        }

        // GET: Admin/AccountUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AccountUser accountUser = db.AccountUser.Find(id);
                if (accountUser == null)
                {
                    return HttpNotFound();
                }
                return View(accountUser);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
        }

        // POST: Admin/AccountUsers/Delete/5
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                AccountUser accountUser = db.AccountUser.Find(id);
                db.AccountUser.Remove(accountUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
    }
}
