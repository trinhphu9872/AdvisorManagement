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
using System.Data.Entity.Migrations;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    [Authorize]
    public class RoleMenusController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private string routePermission = "Admin/RoleMenus";
        // GET: Admin/RoleMenus
        public ActionResult Index()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                ViewBag.Message = "Role Menu";
                var roleMenu = serviceMenu.getRoleMenu();
                ViewBag.roleMenu = roleMenu;
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(roleMenu);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }

        // GET: Admin/RoleMenus/Details/5
        public ActionResult Details(int? id)
        {

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RoleMenu roleMenu = db.RoleMenu.Find(id);
                if (roleMenu == null)
                {
                    return HttpNotFound();
                }

                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(roleMenu);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }

        }

        // GET: Admin/RoleMenus/Create
        public ActionResult Create()
        {

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                ViewBag.id_menu = new SelectList(db.Menu, "id", "menu_name");
                ViewBag.id_role = new SelectList(db.Role, "id", "role_name");
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }

        // POST: Admin/RoleMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create([Bind(Include = "id,id_role,id_menu")] RoleMenu roleMenu)
        {

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                if (ModelState.IsValid)
                {
                    db.RoleMenu.Add(roleMenu);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.id_Menu = new SelectList(db.Menu, "id", "nameMenu", roleMenu.id_menu);
                ViewBag.id_Role = new SelectList(db.Role, "id", "roleName", roleMenu.id_role);

                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(roleMenu);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
       
        }

        // GET: Admin/RoleMenus/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RoleMenu roleMenu = db.RoleMenu.Find(id);
                if (roleMenu == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id_menu = new SelectList(db.Menu, "id", "menu_name", roleMenu.id_menu);
                ViewBag.id_role = new SelectList(db.Role, "id", "role_name", roleMenu.id_role);
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(roleMenu);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }

        // POST: Admin/RoleMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit([Bind(Include = "id,id_role,id_menu")] RoleMenu roleMenu)
        {

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

                if (ModelState.IsValid)
                {
                    db.Entry(roleMenu).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.id_menu = new SelectList(db.Menu, "id", "menu_name", roleMenu.id_menu);
                ViewBag.id_role = new SelectList(db.Role, "id", "role_name", roleMenu.id_role);

                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(roleMenu);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }

        }

        // GET: Admin/RoleMenus/Delete/5
        public ActionResult Delete(int? id)
        {

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RoleMenu roleMenu = db.RoleMenu.Find(id);
                if (roleMenu == null)
                {
                    return HttpNotFound();
                }

                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(roleMenu);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        


        }

        // POST: Admin/RoleMenus/Delete/5
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                RoleMenu roleMenu = db.RoleMenu.Find(id);
                db.RoleMenu.Remove(roleMenu);
                db.SaveChanges();
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
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

        public ActionResult SortMenu()
        {

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                ViewBag.ListMenu = serviceMenu.MenuItem();
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult UpdateMenu(string itemIDs)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
                int count = 1;
                List<int> listIDlist = new List<int>();
                listIDlist = itemIDs.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                foreach (var itemID in listIDlist)
                {
                    try
                    {
                        Menu menu = db.Menu.Where(x => x.id == itemID).FirstOrDefault();
                        menu.order_id = count;
                        db.Menu.AddOrUpdate(menu);
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    count++;
                }
                return Json(true, JsonRequestBehavior.AllowGet);
                }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }
    }
}
