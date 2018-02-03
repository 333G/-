using NFine.Application.InterfaceTXT;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.InterfaceTXT;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.InterfaceTXT.Controllers
{
    public class IFDocSyncaddressController : ControllerBase
    {

        public ActionResult APIDocumentIndex()
        {
            return View();
        }


        // GET: /InterfaceTXT/IFDocSyncaddress/
        private IFDocSyncaddressApp ifDocSyncaddressApp = new IFDocSyncaddressApp();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = ifDocSyncaddressApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

    }
}
