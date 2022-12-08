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

namespace AdvisorManagement.Areas.Admin.Controllers
{
    public class RoleMenusController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        // GET: Admin/RoleMenus
        public ActionResult Index()
        {
            ViewBag.Message = "Role Menu";
            var roleMenu = serviceMenu.getRoleMenu();

            ViewBag.roleMenu = roleMenu;
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View(roleMenu);
        }

        // GET: Admin/RoleMenus/Details/5
        public ActionResult Details(int? id)
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

            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View(roleMenu);
        }

        // GET: Admin/RoleMenus/Create
        public ActionResult Create()
        {
            ViewBag.id_Menu = new SelectList(db.Menu, "id", "nameMenu");
            ViewBag.id_Role = new SelectList(db.Role, "id", "roleName");
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            return View();
        }

        // POST: Admin/RoleMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create([Bind(Include = "id,id_Role,id_Menu")] RoleMenu roleMenu)
        {
            if (ModelState.IsValid)
            {
                db.RoleMenu.Add(roleMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_Menu = new SelectList(db.Menu, "id", "nameMenu", roleMenu.id_Menu);
            ViewBag.id_Role = new SelectList(db.Role, "id", "roleName", roleMenu.id_Role);

            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View(roleMenu);
        }

        // GET: Admin/RoleMenus/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.id_Menu = new SelectList(db.Menu, "id", "nameMenu", roleMenu.id_Menu);
            ViewBag.id_Role = new SelectList(db.Role, "id", "roleName", roleMenu.id_Role);

            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View(roleMenu);
        }

        // POST: Admin/RoleMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit([Bind(Include = "id,id_Role,id_Menu")] RoleMenu roleMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roleMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_Menu = new SelectList(db.Menu, "id", "nameMenu", roleMenu.id_Menu);
            ViewBag.id_Role = new SelectList(db.Role, "id", "roleName", roleMenu.id_Role);

            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View(roleMenu);
        }

        // GET: Admin/RoleMenus/Delete/5
        public ActionResult Delete(int? id)
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

            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View(roleMenu);
        }

        // POST: Admin/RoleMenus/Delete/5
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {
            RoleMenu roleMenu = db.RoleMenu.Find(id);
            db.RoleMenu.Remove(roleMenu);
            db.SaveChanges();

            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return RedirectToAction("Index");
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
