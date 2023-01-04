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
                ViewBag.Message = "Role Menu";
                var roleMenu = serviceMenu.getRoleMenu();
                ViewBag.roleMenu = roleMenu;
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(roleMenu);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Admin/RoleMenus/Details/5
        public ActionResult Details(int? id)
        {
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

                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(roleMenu);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        // GET: Admin/RoleMenus/Create
        public ActionResult Create()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.id_menu = new SelectList(db.Menu, "id", "menu_name");
                ViewBag.id_role = new SelectList(db.Role, "id", "role_name");
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
       
        }

        // GET: Admin/RoleMenus/Edit/5
        public ActionResult Edit(int? id)
        {
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        // GET: Admin/RoleMenus/Delete/5
        public ActionResult Delete(int? id)
        {
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

                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                return View(roleMenu);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
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

        public ActionResult SortMenu()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                ViewBag.ListMenu = serviceMenu.MenuItem();
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var ListMenu = serviceMenu.MenuItem();
         
        }

        public ActionResult EditMenu(int? id)
        {

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Menu menu = db.Menu.Find(id);
                if (menu == null)
                {
                    return HttpNotFound();
                }
                ViewBag.nameMenu = new SelectList(db.Menu, "name_menu", "name_menu");
                ViewBag.orderID = new SelectList(db.Menu, "order_id", "order_id");
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                Session["orderId_truoc"] = menu.order_id;
                Session["maxValueOrder"] = db.Menu.AsQueryable().Count();

                return View(menu);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: RoleMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult EditMenu([Bind(Include = "id,menu_name,order_id")] Menu menu)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                int orderID_truoc = (int)Session["orderId_truoc"];
                var MenuItem = serviceMenu.MenuItem();
                if (ModelState.IsValid)
                {
                    db.Entry(menu).State = EntityState.Modified;
                    db.SaveChanges();
                    if (MenuItem != null)
                    {
                        if (menu.order_id > orderID_truoc)
                        {
                            foreach (var item in MenuItem)
                            {
                                if (item.orderID <= menu.order_id && item.orderID > orderID_truoc)
                                {
                                    Menu menu1 = db.Menu.Find(item.ID);
                                    menu1.order_id -= 1;
                                    db.Entry(menu1).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else if (menu.order_id < orderID_truoc)
                        {
                            foreach (var item in MenuItem)
                            {
                                if (item.orderID >= menu.order_id && item.orderID < orderID_truoc)
                                {
                                    Menu menu1 = db.Menu.Find(item.ID);
                                    menu1.order_id += 1;
                                    db.Entry(menu1).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            db.Entry(menu).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("sortMenu");
                }
                /* Session.Remove("orderId_truoc");*/
                ViewBag.Menu = MenuItem;
                ViewBag.nameMenu = new SelectList(db.Menu, "nameMenu", "nameMenu");
                ViewBag.orderID = new SelectList(db.Menu, "orderid", "orderid");
                return View(menu);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }
    }

}
