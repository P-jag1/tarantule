using System.Web.Mvc;

namespace WebApplication1.Areas.Logged
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
                namespaces: new []{"WebApplication1.Areas.Logged.Controllers"}
            );
        }
    }
}