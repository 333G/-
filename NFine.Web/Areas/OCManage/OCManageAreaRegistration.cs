using System.Web.Mvc;

namespace NFine.Web.Areas.OCManage
{
    public class OCManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OCManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "OCManage_default",
                "OCManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
