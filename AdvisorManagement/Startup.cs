using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AdvisorManagement.Middleware;
using AdvisorManagement.Models;
using System.Web.Mvc;
using Microsoft.Owin;
using Owin;
using System.Linq;
using Microsoft.AspNetCore.Builder;

[assembly: OwinStartup(typeof(AdvisorManagement.Startup))]

namespace AdvisorManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
     
            ConfigureAuth(app);
            app.UseKentorOwinCookieSaver(PipelineStage.Authenticate);
            app.MapSignalR();



        }
    }
}
