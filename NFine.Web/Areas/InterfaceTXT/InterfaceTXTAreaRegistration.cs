using System.Web.Mvc;

namespace NFine.Web.Areas.InterfaceTXT
{
    public class InterfaceTXTAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "InterfaceTXT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "InterfaceTXT_default",
                "InterfaceTXT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
