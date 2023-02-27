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
using OfficeOpenXml;
using System.Web.UI.WebControls;
using OfficeOpenXml.Style;
using System.Drawing;

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

        [HttpPost]
        public JsonResult ExportTemplate(int? year)
        {
            var listPlan = db.PlanAdvisor.Where(x => x.year == year).OrderBy(x=>x.number_title).ThenBy(x=>x.content).ToList();
            List<PlanAdvisor> template = (List<PlanAdvisor>)listPlan;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    pck.Workbook.Properties.Title = year.ToString();
                    var ws = pck.Workbook.Worksheets.Add(year.ToString());

                    ws.Cells["A:AZ"].Style.Font.Name = "Times New Roman";
                    ws.Cells["A:AZ"].Style.Font.Size = 13;
                    ws.Column(1).Width = 8;
                    ws.Column(2).Width = 45;
                    ws.Column(3).Width = 13.67;
                    ws.Column(4).Width = 13.67;
                    ws.Column(5).Width = 13.67;
                    ws.Column(6).Width = 38;
                    ws.Column(7).Width = 35;
                    ws.Column(8).Width = 10;
                    ws.Row(5).Height = 22.80;
                    ws.Row(6).Height = 22.80;
                    ws.Column(2).Style.WrapText = true;
                    ws.Column(3).Style.WrapText = true;
                    ws.Column(4).Style.WrapText = true;
                    ws.Column(5).Style.WrapText = true;
                    ws.Column(6).Style.WrapText = true;
                    ws.Column(7).Style.WrapText = true;
                    ws.Column(8).Style.WrapText = true;
                    ws.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Column(8).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Row(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[1, 1, 1, 2].Merge = true;
                    ws.Cells[1, 1, 1, 2].Value = "TRƯỜNG ĐẠI HỌC VĂN LANG";
                    ws.Cells[1, 1, 1, 2].Style.Font.Size = 12;
                    ws.Cells[2, 1, 2, 2].Merge = true;
                    ws.Cells[2, 1, 2, 2].Value = "KHOA: CÔNG NGHỆ THÔNG TIN";
                    ws.Cells[2, 1, 2, 2].Style.Font.Size = 12;
                    ws.Cells[2, 1, 2, 2].Style.Font.Bold = true;
                    ws.Cells[3, 1, 3, 8].Merge = true;
                    ws.Cells[3, 1, 3, 8].Value = "KẾ HOẠCH HOẠT ĐỘNG CỐ VẤN HỌC TẬP";
                    ws.Cells[3, 1, 3, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[3, 1, 3, 8].Style.Font.Size = 14;
                    ws.Cells[3, 1, 3, 8].Style.Font.Bold = true;
                    ws.Cells[4, 1, 4, 8].Merge = true;
                    ws.Cells[4, 1, 4, 8].Value = "NĂM HỌC "+ (year - 1) + " - "+year;
                    ws.Cells[4, 1, 4, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[4, 1, 4, 8].Style.Font.Size = 14;
                    ws.Cells[4, 1, 4, 8].Style.Font.Bold = true;
                    ws.Cells[5, 1, 6, 1].Merge = true;
                    ws.Cells[5, 1, 6, 1].Value = "STT";
                    ws.Cells[5, 1, 6, 1].Style.Font.Bold = true;
                    ws.Cells[5, 2, 6, 2].Merge = true;
                    ws.Cells[5, 2, 6, 2].Value = "NỘI DUNG";
                    ws.Cells[5, 2, 6, 2].Style.Font.Bold = true;
                    ws.Cells[5, 3, 5, 5].Merge = true;
                    ws.Cells[5, 3, 5, 5].Value = "THỜI GIAN DỰ KIẾN";
                    ws.Cells[5, 3, 5, 5].Style.Font.Bold = true;
                    ws.Cells["C6"].Value = "Học kỳ 1";
                    ws.Cells["C6"].Style.Font.Bold = true;
                    ws.Cells["D6"].Value = "Học kỳ 2";
                    ws.Cells["D6"].Style.Font.Bold = true;
                    ws.Cells["E6"].Value = "Học kỳ 3";
                    ws.Cells["E6"].Style.Font.Bold = true;
                    ws.Cells[5, 6, 6, 6].Merge = true;
                    ws.Cells[5, 6, 6, 6].Value = "MÔ TẢ CÔNG VIỆC";
                    ws.Cells[5, 6, 6, 6].Style.Font.Bold = true;
                    ws.Cells[5, 7, 6, 7].Merge = true;
                    ws.Cells[5, 7, 6, 7].Value = "TÀI NGUYÊN CHUẨN BỊ";
                    ws.Cells[5, 7, 6, 7].Style.Font.Bold = true;
                    ws.Cells[5, 8, 6, 8].Merge = true;
                    ws.Cells[5, 8, 6, 8].Value = "GHI CHÚ";
                    ws.Cells[5, 8, 6, 8].Style.Font.Bold = true;
                    int rowStart = 7;
                    bool check = false;
                    int stt =0;
                    string content = "";
                    string source = "";
                    int countSTT = 1;
                    int countContent = 1;
                    int countSource = 1;
                    foreach (var item in template)
                    {                                               
                        if (item.number_title == stt)
                        {   
                            if(item.content == content) {
                                if (item.source == source)
                                {
                                    ws.Cells[string.Format("F{0}", rowStart)].Value = servicePlan.getDecribe(item.describe);                           
                                    countSTT++;
                                    countContent++;
                                    countSource++;
                                }
                                else
                                {
                                    ws.Cells[string.Format("F{0}", rowStart)].Value = servicePlan.getDecribe(item.describe);
                                    ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
                                    ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
                                    countSTT++;
                                    countContent++;
                                }                                                           
                            }
                            else
                            {
                                ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
                                ws.Cells[string.Format("F{0}", rowStart)].Value = servicePlan.getDecribe(item.describe);
                                ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
                                ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0,191,143,0);
                                countSTT++;
                            }                                                
                        } else
                        {
                            if (item.number_title == 3)
                            {
                                ws.InsertRow(rowStart - countSTT , 1);
                                ws.Row(rowStart - countSTT).Height = 31.20;
                                ws.Cells[rowStart - countSTT, 2].Value = "Tổ chức họp lớp SV theo định kỳ/đột xuất.";
                                ws.Cells[rowStart - countSTT, 3, rowStart - countSTT, 8].Merge = true;

                                ws.InsertRow(rowStart + countSTT - 1, 1);
                                ws.Row(rowStart + countSTT - 1).Height = 31.20;
                                ws.Cells[rowStart + countSTT - 1, 2].Value = "...";
                                ws.Cells[rowStart + countSTT - 1, 6].Value = "...";
                                rowStart = rowStart + 2;
                            }
                            if (countSTT > 1)
                            {
                                if(item.number_title == 3)
                                {
                                    ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Merge = true;
                                    ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Value = ws.Cells[rowStart - countSTT - 1, 1].Value;
                                    ws.Cells[rowStart - countSTT - 2, 1, rowStart - 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    countSTT = 1;
                                }
                                else
                                {
                                    ws.Cells[rowStart - countSTT, 1, rowStart - 1, 1].Merge = true;
                                    countSTT = 1;
                                }                               
                            }
                            if(countContent > 1)
                            {
                                ws.Cells[rowStart - countContent, 2, rowStart - 1, 2].Merge = true;
                                countContent = 1;
                            }
                            if(countSource> 1)
                            {
                                ws.Cells[rowStart - countSource, 7, rowStart - 1, 7].Merge = true;
                                countSource = 1;
                            }                            
                            ws.Cells[string.Format("A{0}", rowStart)].Value = item.number_title;
                            ws.Cells[string.Format("A{0}", rowStart)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("B{0}", rowStart)].Value = item.content;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = servicePlan.getDecribe(item.describe);                               
                            ws.Cells[string.Format("G{0}", rowStart)].Value = item.source;
                            ws.Cells[string.Format("G{0}", rowStart)].Style.Font.Color.SetColor(0, 191, 143, 0);
                        }                      
                        rowStart++;
                        stt = (int)item.number_title;
                        content = item.content;
                        source = item.source;

                    }


                    ws.Column(10).Style.Font.Size = 13;
                    /* ws.Cells["A:AZ"].AutoFitColumns();*/
                    ws.Cells[5, 1, rowStart-1, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[5, 1, rowStart-1, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[5, 1, rowStart-1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[5, 1, rowStart-1, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;                    
                    ws.Cells[rowStart + 2, 2].Value = "Trưởng khoa";
                    ws.Cells[rowStart + 2, 2].Style.Font.Bold = true;

                    ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Merge = true;
                    ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Value = "TP. Hồ Chí Minh, ngày   tháng   năm " + (year -1);
                    ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Style.Font.Italic = true;
                    ws.Cells[rowStart + 1, 6, rowStart + 1, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Merge = true;
                    ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Value = "Cố vấn học tập";
                    ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Style.Font.Bold = true;
                    ws.Cells[rowStart + 2, 6, rowStart + 2, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Merge = true;
                    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Value = "Lưu ý: CVHT cụ thể hóa các công việc trong phần mô tả theo đặc thù đơn vị và kế hoạch cụ thể của cá nhân, làm căn cứ thực hiện công tác này trong NH " + (year-1)+ " - " + year + ".";
                    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.Font.Bold = true;
                    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.Font.Italic = true;
                    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells[rowStart + 8, 1, rowStart + 8, 8].Style.WrapText = true;

                   
                
                    ws.Row(rowStart+8).Height = 31.80;
                    pck.Save();
                    var fileOject = File(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KehoachCVHT_" + year + ".xlsx");
                    return Json(fileOject, JsonRequestBehavior.AllowGet);
                    /*  Response.Clear();
                      Response.Clear();
                      Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                      Response.AddHeader("content-disposition", "attachment; filename="+"KehoachCVHT_" + year + ".xlsx");
                      Response.BinaryWrite(pck.GetAsByteArray());
                      Response.End();*/
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
