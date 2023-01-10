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
using Spire.Xls;
using System.IO;

namespace AdvisorManagement.Controllers
{
    public class PlansAdvisorController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private StudentsMiddleware serviceStd = new StudentsMiddleware();
        private ProofMiddleware serviceProof = new ProofMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        // GET: PlansAdvisor
        public ActionResult Index()
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            var role = serviceStd.getRoles(User.Identity.Name);
            ViewBag.role = role;
            Session["role"] = role;
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View(db.TitlePlan.ToList());
        }

        // GET: PlansAdvisor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TitlePlan titlePlan = db.TitlePlan.Find(id);
            if (titlePlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.role = Session["role"];
            Session["id"] = id;
            var listProof = serviceProof.getListProof(User.Identity.Name, (int)id);
            ViewBag.plan = listProof;
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.hocky = db.Semester.ToList().OrderBy(x => x.scholastic);
            return View(titlePlan);
        }

        // GET: PlansAdvisor/Create
        public ActionResult Create()
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View();
        }

        // POST: PlansAdvisor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,title,content")] TitlePlan titlePlan)
        {
            if (ModelState.IsValid)
            {
                db.TitlePlan.Add(titlePlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(titlePlan);
        }

        // GET: PlansAdvisor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TitlePlan titlePlan = db.TitlePlan.Find(id);
            if (titlePlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View(titlePlan);
        }

        // POST: PlansAdvisor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,title,content")] TitlePlan titlePlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(titlePlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(titlePlan);
        }

        // GET: PlansAdvisor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TitlePlan titlePlan = db.TitlePlan.Find(id);
            if (titlePlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View(titlePlan);
        }

        // POST: PlansAdvisor/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TitlePlan titlePlan = db.TitlePlan.Find(id);
            db.TitlePlan.Remove(titlePlan);
            db.SaveChanges();
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

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase postedfile, string mota, string semester)
        {

            try
            {
                string filePath = string.Empty;

                if (postedfile == null || postedfile.ContentLength == 0)
                {
                    ViewBag.Error = "Vui lòng chọn file";
                    return Json(new { success = false, message = "Vui lòng chọn file" });
                } else if (mota == "")
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" });
                }
                else
                {
                    if (postedfile.FileName.EndsWith(".xls") || postedfile.FileName.EndsWith(".xlsx") || postedfile.FileName.EndsWith(".docx"))
                    {
                        string path = Server.MapPath("~/Proof/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        filePath = path + Path.GetFileName(postedfile.FileName);
                        string extension = Path.GetExtension(postedfile.FileName);
                        postedfile.SaveAs(filePath);
                        if (Path.GetExtension(postedfile.FileName) == ".xls")
                        {
                            Workbook workbook = new Workbook();
                            workbook.LoadFromFile(filePath);
                            workbook.SaveToFile(filePath, ExcelVersion.Version2013);
                        }
                        var id = Session["id"];
                        var id_account = db.AccountUser.FirstOrDefault(x=>x.email == User.Identity.Name).id;
                        ProofPlan proof = new ProofPlan();
                        proof.content = mota;
                        proof.semester = semester;
                        proof.file_proof = postedfile.FileName;
                        proof.create_time = DateTime.Now;
                        proof.id_creator = id_account;
                        proof.id_titleplan = (int)id;
                        db.ProofPlan.Add(proof);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Upload thành công" });

                    }
                    else
                    {
                        return Json(new { success = false, message = "Vui lòng chọn file excel hoặc word" });
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Vui lòng chọn file excel hoặc word" });
            }
        }

        public FileResult Download(string nameProof)
        {
            var filePath = Server.MapPath("~/Proof/") + nameProof;
            return File(filePath, "application/force- download", Path.GetFileName(filePath));
        }
    }
}
