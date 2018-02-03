using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class SendSmsController : ControllerBase
    {
        //
        // GET: /OCManage/SendSms/

        public ActionResult SendSmsIndex()
        {
            return View();
        }
        public ActionResult ChangechannelForm()
        {
            return View();
        }

    }
}
