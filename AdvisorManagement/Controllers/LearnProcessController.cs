using AdvisorManagement.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvisorManagement.Controllers
{
    [LoginFilter]
    public class LearnProcessController : Controller
    {
        // GET: LearnProcess
        public ActionResult Index()
        {
            return View();
        }
    }
}