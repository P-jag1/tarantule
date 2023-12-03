using System.Web.Mvc;

namespace Tarantule.Areas.Logged
{
    public class LoggedAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Logged";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Logged_default",
                "Logged/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new []{ "Tarantule.Areas.Logged.Controllers" }
            );
        }
    }
}