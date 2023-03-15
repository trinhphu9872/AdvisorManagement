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
    // auth
    [Authorize]
    public class AccountUsersController : Controller
    {
        // check
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private string routePermission = "Admin/AccountUsers";
        private string picture;
        //GET: Admin/AccountUsers
        public ActionResult Index()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                 var accountUser = db.AccountUser.Include(a => a.Role).Where(x => x.id_role != 1).OrderBy(y => y.id_role);
                ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
                ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                 ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                return View(accountUser.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }
        // GET: Admin/AccountUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
                ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }

        // GET: Admin/AccountUsers/Create
        public ActionResult Create()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                ViewBag.id_Role = new SelectList(db.Role, "id", "role_name");
                AccountUser user = new AccountUser();
                return View(user);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
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
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

                if (ModelState.IsValid)
                {
                    if (accountUser.id_role == 2) {
                        
                        
                        if (accountUser.ImageUpload != null)
                        {
                            string filename = Path.GetFileNameWithoutExtension(accountUser.ImageUpload.FileName).ToString();
                            string extension = Path.GetExtension(accountUser.ImageUpload.FileName);
                            filename = filename + extension;
                            accountUser.img_profile = "~/Images/imageProfile/" + filename;
                            accountUser.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/imageProfile/"), filename));
                        }
                        accountUser.id = db.AccountUser.ToList().Count() + 1;
                        accountUser.create_time = DateTime.Now;

                        db.AccountUser.Add(accountUser);
                        var addAdvisor = new Advisor();

                        addAdvisor.advisor_code = accountUser.user_code;
                        addAdvisor.account_id = accountUser.id;
                        db.Advisor.Add(addAdvisor);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }else if (accountUser.id_role == 3)
                    {

                    if (accountUser.ImageUpload != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(accountUser.ImageUpload.FileName).ToString();
                        string extension = Path.GetExtension(accountUser.ImageUpload.FileName);
                        filename = filename + extension;
                        accountUser.img_profile = "~/Images/imageProfile/" + filename;
                        accountUser.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/imageProfile/"), filename));
                    }
                    accountUser.id = db.AccountUser.ToList().Count() + 1;
                    accountUser.create_time = DateTime.Now;

                    db.AccountUser.Add(accountUser);
                    var addstudent = new Student ();

                    addstudent.student_code = accountUser.user_code;
                    addstudent.account_id = accountUser.id;
                    db.Student.Add(addstudent);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    }
                    else { 
                    //accountUser.ID = Guid.NewGuid();
                        if (accountUser.ImageUpload != null)
                        {
                            string filename = Path.GetFileNameWithoutExtension(accountUser.ImageUpload.FileName).ToString();
                            string extension = Path.GetExtension(accountUser.ImageUpload.FileName);
                            filename = filename + extension;
                            accountUser.img_profile = "~/Images/imageProfile/" + filename;
                            accountUser.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/imageProfile/"), filename));
                        }
                        accountUser.id= db.AccountUser.ToList().Count() + 1;
                        accountUser.create_time = DateTime.Now;
                    
                        db.AccountUser.Add(accountUser);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

                ViewBag.id_Role = new SelectList(db.Role, "id", "role_name", accountUser.id_role);
                return View(accountUser);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }
        // GET: Admin/AccountUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
                ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                AccountUser accountUser = db.AccountUser.Find(id);
                if (accountUser == null)
                {
                    return HttpNotFound();
                }
                picture = accountUser.img_profile;
                ViewBag.id_Role = db.Role.ToList();
                return View(accountUser);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }

        // POST: Admin/AccountUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(AccountUser accountUser)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission)) {

                ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
                ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                if (ModelState.IsValid)
                {
                    var edituser = db.AccountUser.Find(accountUser.id);
                    if (accountUser.ImageUpload != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(accountUser.ImageUpload.FileName).ToString();
                        string extension = Path.GetExtension(accountUser.ImageUpload.FileName);
                        filename = filename + extension;
                        edituser.img_profile = "~/Images/imageProfile/" + filename;
                        accountUser.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/imageProfile/"), filename));
                    }
                    edituser.address = accountUser.address;
                    edituser.user_name = accountUser.user_name;
                    edituser.user_code = edituser.user_code;
                    //edituser.dateofbirth = accountUser.dateofbirth;
                    edituser.phone = accountUser.phone;
                    edituser.gender = accountUser.gender;
                    edituser.id_role = accountUser.id_role;

                    db.Entry(edituser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.id_Role = new SelectList(db.Role, "id", "roleName", accountUser.id_role);
                 return View(accountUser);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }

        // GET: Admin/AccountUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }

        // POST: Admin/AccountUsers/Delete/5
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                var accountUser = db.AccountUser.Find(id);
                var advisor= db.Advisor.Find(accountUser.user_code);
                var student = db.Student.Find(accountUser.user_code);
                if (advisor != null)
                {
                    db.Advisor.Remove(advisor);
                }
                if (student != null)
                {
                    db.Student.Remove(student);
                }
                db.AccountUser.Remove(accountUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
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
