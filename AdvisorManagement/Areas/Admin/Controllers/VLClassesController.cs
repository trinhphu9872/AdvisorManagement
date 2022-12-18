using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdvisorManagement.Models;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    public class VLClassesController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();

        // GET: Admin/VLClasses
        public async Task<ActionResult> Index()
        {
            var vLClass = db.VLClass.Include(v => v.Advisor);
            return View(await vLClass.ToListAsync());
        }

        // GET: Admin/VLClasses/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VLClass vLClass = await db.VLClass.FindAsync(id);
            if (vLClass == null)
            {
                return HttpNotFound();
            }
            return View(vLClass);
        }

        // GET: Admin/VLClasses/Create
        public ActionResult Create()
        {
            ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code");
            return View();
        }

        // POST: Admin/VLClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,advisor_code,create_time,update_time")] VLClass vLClass)
        {
            if (ModelState.IsValid)
            {
                vLClass.create_time= DateTime.Now;
                db.VLClass.Add(vLClass);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code", vLClass.advisor_code);
            return View(vLClass);
        }

        // GET: Admin/VLClasses/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VLClass vLClass = await db.VLClass.FindAsync(id);
            if (vLClass == null)
            {
                return HttpNotFound();
            }
            ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code", vLClass.advisor_code);
            return View(vLClass);
        }

        // POST: Admin/VLClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,advisor_code,create_time,update_time")] VLClass vLClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vLClass).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.advisor_code = new SelectList(db.Advisor, "advisor_code", "advisor_code", vLClass.advisor_code);
            return View(vLClass);
        }

        // GET: Admin/VLClasses/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VLClass vLClass = await db.VLClass.FindAsync(id);
            if (vLClass == null)
            {
                return HttpNotFound();
            }
            return View(vLClass);
        }

        // POST: Admin/VLClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            VLClass vLClass = await db.VLClass.FindAsync(id);
            db.VLClass.Remove(vLClass);
            await db.SaveChangesAsync();
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
