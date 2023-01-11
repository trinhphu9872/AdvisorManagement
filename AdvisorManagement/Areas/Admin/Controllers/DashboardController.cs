using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace AdvisorManagement.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();

        private string routePermission = "Admin/Dashboard";
        CP25Team09Entities db = new CP25Team09Entities();
        string connect = @"Data Source=tuleap.vanlanguni.edu.vn,18082;Initial Catalog=CP25Team09;Persist Security Info=True;User ID=CP25Team09;Password='CP25Team09'";
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.avatar = serviceAccount.getAvatar(User.Identity.Name);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return View();
        }

        /*Return Json DashboardSum*/
        public JsonResult DashboardSum()
        {
            try
            {

                SqlConnection conStr = new SqlConnection(connect);
                conStr.Open();
                // cmd Student
                SqlCommand cmdStudent = new SqlCommand("SELECT Count(student_code) as student from Student;", conStr);
                DataTable dt = new DataTable();
                SqlDataAdapter commandStudent = new SqlDataAdapter(cmdStudent);
                commandStudent.Fill(dt);
                // cmd Advisor
                SqlCommand cmdAdvisor = new SqlCommand("SELECT Count(advisor_code) as advisor from Advisor;", conStr);
                DataTable dt1 = new DataTable();
                SqlDataAdapter commandAdvisor = new SqlDataAdapter(cmdAdvisor);
                commandAdvisor.Fill(dt1);

                string[] name = { "Student", "Advisor" };
                string[] value = new string[2];
                value[0] = dt.Rows[0]["student"].ToString();
                value[1] = dt1.Rows[0]["advisor"].ToString();
                conStr.Close();
                return Json(new { name, value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //DashboardFileAdvisor
        /*Return Json DashboardFileAdvisor*/
        public JsonResult DashboardFileAdvisor()
        {
            try
            {
                
                SqlConnection conStr = new SqlConnection(connect);
                conStr.Open();
       
                // cmd Advisor
                SqlCommand cmdAdvisor = new SqlCommand("SELECT A.user_name, count(B.id) as file_up from AccountUser A,ProofPlan B Where A.id_role = 2 and B.id_creator = A.id Group by A.user_name;", conStr);
                DataTable dt = new DataTable();
                SqlDataAdapter commandAdvisor = new SqlDataAdapter(cmdAdvisor);
                commandAdvisor.Fill(dt);

                string[] nameAd = new string[dt.Rows.Count];
                string[] valueAd = new string[dt.Rows.Count];
                for (int i = 0; i < nameAd.Length; i++)
                {
                   nameAd[i] = dt.Rows[i]["user_name"].ToString();
                   valueAd[i] = dt.Rows[i]["file_up"].ToString();
                }
                conStr.Close();
                return Json(new { nameAd, valueAd }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //DashboardClass
        /*Return Json Class*/
        public JsonResult DashboardClass()
        {
            try
            {

                SqlConnection conStr = new SqlConnection(connect);
                conStr.Open();

                // cmd Advisor
                SqlCommand cmdAdvisor = new SqlCommand("SELECT A.user_name, count(B.id) as countClass from AccountUser A,VLClass B Where A.id_role = 2 and B.advisor_code = A.user_code Group by A.user_name;", conStr);
                DataTable dt = new DataTable();
                SqlDataAdapter commandAdvisor = new SqlDataAdapter(cmdAdvisor);
                commandAdvisor.Fill(dt);

                string[] nameCl = new string[dt.Rows.Count];
                string[] valueCl = new string[dt.Rows.Count];
                for (int i = 0; i < nameCl.Length; i++)
                {
                    nameCl[i] = dt.Rows[i]["user_name"].ToString();
                    valueCl[i] = dt.Rows[i]["countClass"].ToString();
                }
                conStr.Close();
                return Json(new { nameCl, valueCl }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}