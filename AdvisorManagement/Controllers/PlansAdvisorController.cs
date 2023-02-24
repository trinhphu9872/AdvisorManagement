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
using Microsoft.Ajax.Utilities;

namespace AdvisorManagement.Controllers
{
    public class PlansAdvisorController : Controller
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private StudentsMiddleware serviceStd = new StudentsMiddleware();
        private PlanMiddleware servicePlan = new PlanMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        // GET: PlansAdvisor
        public ActionResult Index()
        {
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            var role = serviceStd.getRoles(User.Identity.Name);
            ViewBag.role = role;
            Session["role"] = role;
            var id_account = serviceStd.getID(User.Identity.Name);
            ViewBag.listProof = db.ProofPlan.Where(x => x.id_creator == id_account).ToList();
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            var year = servicePlan.getYear();
            ViewBag.listYear = db.PlanAdvisor.Where(x => x.year < year).DistinctBy(x=>x.year).ToList();
            Session["yearNow"] = year;
            return View(db.PlanAdvisor.Where(x=>x.year == year).ToList().OrderBy(x=>x.number_title));
        }

        // GET: PlansAdvisor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanAdvisor titlePlan = db.PlanAdvisor.Find(id);
            if (titlePlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
            ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.role = Session["role"];
            Session["id"] = id;
            var listProof = servicePlan.getListProof(User.Identity.Name, (int)id);
            ViewBag.plan = listProof;
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.hocky = db.Semester.DistinctBy(x=>x.semester_name).ToList();
            return View(titlePlan);
        }

        // GET: PlansAdvisor/Create
        public ActionResult Create()
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View();
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var title = db.PlanAdvisor.SingleOrDefault(x => x.id == id);
                db.PlanAdvisor.Remove(title);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
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
                    if (postedfile.FileName.EndsWith(".xls") || postedfile.FileName.EndsWith(".xlsx") || postedfile.FileName.EndsWith(".docx") || postedfile.FileName.EndsWith(".doc") || 
                        postedfile.FileName.EndsWith(".pdf") || postedfile.FileName.EndsWith(".png") || postedfile.FileName.EndsWith(".jpg"))
                    {
                        string path = Server.MapPath("~/Proof/"+ Session["yearNow"].ToString() + "/");
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
                        return Json(new { success = false, message = "Vui lòng chọn file hợp lệ" });
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
            var id = Session["id"];
            var year = db.PlanAdvisor.Find(id).year;
            var filePath = Server.MapPath("~/Proof/" + year.ToString() + "/") +nameProof;
            return File(filePath, "application/force- download", Path.GetFileName(filePath));
        }

        [HttpPost]
        public ActionResult CreateTitle(int idtitle, string content, /*string HK1, string HK2, string HK3,*/ string describe, string source, string note )
        {

            try
            {
                if(content != "" && content != "" && describe != "" && source !="" && note != "") { 
                var year = servicePlan.getYear();
                PlanAdvisor plan = new PlanAdvisor();
                plan.number_title = idtitle;
                plan.content = content;
                plan.year = year;
                plan.describe= describe;
                plan.source= source;
                plan.note = note;
                var i = describe.Length;
                db.PlanAdvisor.Add(plan);
                db.SaveChanges();
                return Json(new { success = true, message = "Thêm thành công" });
                } else
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ trường dữ liệu" });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Vui lòng chọn file excel hoặc word" });
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            try
            {
                var title = db.PlanAdvisor.SingleOrDefault(x => x.id == id);
                return Json(new { success = true, T = title, message = "Lấy thông tin thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lấy thông tin thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateTittle(int id, int numTitle, string content, string describe, string source, string note)
        {
            try
            {
                if (numTitle != null && content != "" && describe != "" && source!= "" && note !="")
                {
                    var title = db.PlanAdvisor.SingleOrDefault(x => x.id == id);
                    title.number_title = numTitle;
                    title.content = content;
                    title.describe = describe;
                    title.source = source;
                    title.note = note;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ các trường thông tin" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteProof(int id)
        {
            try
            {
                var proof = db.ProofPlan.SingleOrDefault(x => x.id == id);
                db.ProofPlan.Remove(proof);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Copy(int id)
        {
            try
            {
                var year = servicePlan.getYear();
                var temp = db.PlanAdvisor.Where(x => x.year == id).Select(y => y.describe).ToList();
                var listPlan = db.PlanAdvisor.Where(x=>x.year == year).Select(y=>y.describe).ToList();
               
                if(id != year)
                {
                        var count = 0;
                        var checkExist = temp.Except(listPlan);
                        if(checkExist.Count() != 0)
                        {
                            foreach(var i in checkExist)
                            {
                                if(db.PlanAdvisor.Where(x => x.year == year).Where(y => y.describe == i).ToList().Count() ==0)
                                {
                                var newPlan = db.PlanAdvisor.Where(x => x.year == id).Where(y => y.describe == i).ToList();
                                foreach (var n in newPlan)
                                    {
                                        PlanAdvisor plan = new PlanAdvisor();
                                        plan.year = year;
                                        plan.number_title = n.number_title;
                                        plan.content = n.content;
                                        plan.describe = n.describe;
                                        plan.source = n.source;
                                        db.PlanAdvisor.Add(plan);
                                        db.SaveChanges();
                                        count++;
                                    }
                                }                                              
                            }
                        return Json(new { success = true, message = "Copy thành công, có thêm " + count +" đề mục mới" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, message = "Không có thêm sự thay đổi" }, JsonRequestBehavior.AllowGet);
                    }            
                }
                return Json(new { success = true, message = "Không có thêm sự thay đổi" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Copy thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetData(int year)
        {
            var datos = db.PlanAdvisor.Where(x=>x.year == year).ToList().OrderBy(x => x.number_title);

            return Json(new {data = datos }, JsonRequestBehavior.AllowGet);

        }
    }
}
