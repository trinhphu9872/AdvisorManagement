using System.Web.Mvc;

namespace AdvisorManagement.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
              "user_manegement",
              "admin-quan-li/nguoi-dung",
              new { controller = "AccountUsers", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "AdvisorManagement.Areas.Admin.Controllers" }
          );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}