using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AdvisorManagement.Middleware;
using AdvisorManagement.Models;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    // auth
    [LoginFilter]
    public class AccountUsersController : Controller
    {
        // check
        private CP25Team09Entities db = new CP25Team09Entities();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private MailServicesMiddleware serviceMail = new MailServicesMiddleware();
        private string routePermission = "Admin/AccountUsers";
        private string picture;
        public void init()
        {
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
        } 
        //GET: Admin/AccountUsers
        public ActionResult Index()
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                this.init();
                ViewBag.id_role = db.Role.ToList();
                var accountUser = db.AccountUser.Include(a => a.Role).OrderBy(y => y.id_role);
                ViewBag.Name = serviceAccount.getTextName(User.Identity.Name);
                ViewBag.RoleName = serviceAccount.getRoleTextName(User.Identity.Name);
                return View(accountUser.ToList().OrderByDescending(x=>x.update_time));
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ProxyAuthenticationRequired);
            }
        }
        // API
        // GET: Admin/AccountUsers/Details/5
        [HttpGet]
        public ActionResult Details(int? id)    
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                AccountUser user = db.AccountUser.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                return Json(new { success = true,
                    detail_code = user.user_code,
                    detail_name = user.user_name,
                    detail_street = user.address,
                    detail_mail = user.email,
                    detail_phone = user.phone,
                    detail_id_role = user.id_role,
                    detail_img = user.img_profile != null ?  user.img_profile.ToString().Replace("~","") : "/Images/imageProfile/avata.png",
                    id_detailUser = user.id, message = "Lấy thông tin thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lấy thông tin thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CreateApi([Bind(Include = " email,user_name,phone,ImageUpload,id_role")]AccountUser account)
        {
            
            if (serviceAccount.getPermission(User.Identity.Name, routePermission)  )
            {
                account.address = null;
                if (account.ImageUpload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(account.ImageUpload.FileName).ToString();
                    string extension = Path.GetExtension(account.ImageUpload.FileName);
                    filename = filename + extension;
                    account.img_profile = "~/Images/imageProfile/" + filename;
                    account.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/imageProfile/"), filename));
                }
                if (account.email == null )
                {
                    return Json(new { success = false, message = "Vui lòng điền mail" });
                }
                if (account.user_name == null)
                {
                    return Json(new { success = false, message = "Vui lòng điền tên " });
                }
                if (account.user_name.Trim().Length == 0 && account.user_name.Length > 0 && string.IsNullOrWhiteSpace(account.user_name))
                {
                    return Json(new { success = false, message = "Vui lòng điển tên " });
                }
                if (serviceAccount.IsValidVietnameseName(account.user_name.Trim()))
                {
                    return Json(new { success = false, message = "Vui lòng điền tên không chứa kí tự đặc biệt" });
                }
                if (account.phone == null)
                {
                    return Json(new { success = false, message = "Vui lòng điền số điện thoại" });
                }
                if (db.AccountUser.Where(x => x.phone == account.phone).ToList().Count() > 0)
                {
                    return Json(new { success = false, message = "Số điện thoại tồn tại trong hệ thống" });

                }
                if (!serviceAccount.IsValidEmail(account.email))
                {
                    return Json(new { success = false, message = "Email không đúng định dạng" });
                }
                if (!serviceAccount.IsPhoneNumberValid(account.phone))
                {
                    return Json(new { success = false, message = "Số điện thoại không đúng định dạng" });
                }
                var userCheck = db.AccountUser.Where(x => x.email == account.email).ToList().Count();
                if (userCheck > 0)
                {
                    return Json(new { success = false, message = "Email tồn tại trong hệ thông" });
                }
                string mess =  serviceAccount.UserProfileCheck(account.email, account);
                if (mess == "")
                {
                    return Json(new { success = false, message = "Thêm người dùng không thành công" });

                }
                return Json(new { success = true, message = mess });

            }
            else
            {
                return Json(new { success = false, message = "Sai phân quyền" });
            }
        }

        // DELETE API
        [HttpPost]
        public ActionResult DeleteApi(int id)
        {
            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                var userCheck = db.AccountUser.FirstOrDefault(x => x.id == id);
                if (userCheck.id == null)
                {
                    return Json(new { success = false, message = "Cố vấn không tồn tại trong hệ thông" });
                }
                if (userCheck.id_role == 2)
                {
                    var classCheck = db.VLClass.Where(y => y.advisor_code == userCheck.user_code).ToList().Count();
                    var adCheck = db.Advisor.FirstOrDefault(x => x.advisor_code == userCheck.user_code);
                    if (classCheck > 0)
                    {
                        adCheck.status_id = 0;
                        db.Entry(adCheck).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Tài khoản tạm khoá do giảng viên có chủ nhiệm lớp" });
                    }
                    else
                    {
                        if (adCheck != null)
                        {
                            db.Advisor.Remove(adCheck);
                            db.SaveChanges();
                        }
                        db.AccountUser.Remove(userCheck);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Tài khoản cố vấn đã xoá khỏi hệ thống" });
                    }
                }
                else if (userCheck.id_role == 3)
                {
                    var studentCheck = db.Student.FirstOrDefault(x => x.account_id == id);
                    if (studentCheck != null) {
                        db.Student.Remove(studentCheck);
                        db.SaveChanges();
                    }
                    db.AccountUser.Remove(userCheck);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Tài khoản sinh viên đã xoá khỏi hệ thống" });

                }
                else
                {
                    
                    var advisorClone = db.Advisor.Where(x => x.account_id == id).ToList();
                    if (advisorClone.Count() > 0)
                    {
                        db.Advisor.Remove(db.Advisor.FirstOrDefault(y => y.account_id == id));
                        db.SaveChanges();
                    }
                    var studentClone = db.Advisor.Where(z => z.account_id == id).ToList();
                    if (studentClone.Count() > 0)
                    {
                        db.Student.Remove(db.Student.FirstOrDefault(y => y.account_id == id));
                        db.SaveChanges();
                    }

                    db.AccountUser.Remove(userCheck);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Tài khoản admin đã xoá khỏi hệ thống" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Sai phân quyền" });
            }
        }
        // EDIT API 
        [HttpPost]
        public ActionResult EditApi([Bind(Include = " id,email,user_name,phone,address,ImageUpload ")] AccountUser account)
        {

            if (serviceAccount.getPermission(User.Identity.Name, routePermission))
            {
                account.address = null;
                var edtUser = db.AccountUser.FirstOrDefault(x => x.id == account.id);
                if (account.ImageUpload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(account.ImageUpload.FileName).ToString();
                    string extension = Path.GetExtension(account.ImageUpload.FileName);
                    filename = filename + extension;
                    account.img_profile = "~/Images/imageProfile/" + filename;
                    account.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/imageProfile/"), filename));
                }
                if (account.email == null)
                {
                    return Json(new { success = false, message = "Vui lòng điền mail" });
                }
                if (account.user_name == null)
                {
                    return Json(new { success = false, message = "Vui lòng điền tên " });
                }
                if (account.user_name.Trim().Length == 0 && account.user_name.Length > 0 && string.IsNullOrWhiteSpace(account.user_name))
                {
                    return Json(new { success = false, message = "Vui lòng điển tên " });
                }
                if (serviceAccount.IsValidVietnameseName(account.user_name.Trim()))
                {
                    return Json(new { success = false, message = "Vui lòng điền tên không chứa kí tự đặc biệt" });
                }
                if (account.phone == null)
                {
                    return Json(new { success = false, message = "Vui lòng điền số điện thoại" });
                }
                if (db.AccountUser.Where(x => x.phone == account.phone).ToList().Count() > 0)
                {
                    return Json(new { success = false, message = "Số điện thoại tồn tại trong hệ thống" });
                }
                if (!serviceAccount.IsValidEmail(account.email))
                {
                    return Json(new { success = false, message = "Email không đúng định dạng" });
                }
                if (!serviceAccount.IsPhoneNumberValid(account.phone))
                {
                    return Json(new { success = false, message = "Số điện thoại không đúng định dạng" });
                }

                if (edtUser == null)
                {
                    return Json(new { success = false, message = "Email không tồn tại trong hệ thông" });
                }
                edtUser.img_profile = account.img_profile;
                edtUser.user_name = account.user_name;
                edtUser.phone = account.phone;
                edtUser.address = account.address;
                edtUser.update_time = DateTime.Now;
                db.Entry(edtUser).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Cập nhật tài khoản thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Sai phân quyền" });
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
