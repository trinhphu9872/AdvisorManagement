using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using System.Web.Mvc;
using Microsoft.Owin;
using Owin;
using System.Linq;

namespace AdvisorManagement
{
    public partial class Startup
    {

   
        public void Configuration(IAppBuilder app)
        {
     
            ConfigureAuth(app);
            app.UseKentorOwinCookieSaver(PipelineStage.Authenticate);
        }
    }
}
