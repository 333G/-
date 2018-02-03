using NFine.Application.CRMManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.CRMManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;



namespace NFine.Web.Areas.CRMManage.Controllers
{
    public class CustomersController : ControllerBase
    {

        private CustomersApp customersApp = new CustomersApp();
        private CustomeRecordsApp customeRecordsApp = new CustomeRecordsApp();
        public ActionResult GetFormJson()
        {
            return View();
        }
        public ActionResult AddCustomerForm()
        {
            return View();
        }
        public ActionResult CustomerForm()
        {
            return View();
        }
        public ActionResult RecordForm()
        {
            return View();
        }
        public ActionResult AllRecordsForm()
        {
            return View();
        }
        public ActionResult NewCustomersForm()
        {
            return View();
        }
        public ActionResult GetAddFormJson()
        {
            return View();
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetAllRecordsGridJson(Pagination pagination, string queryJson)
        {
           
            var data = new
            {
                rows = customeRecordsApp.GetList(queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            List<NFine.Entity.Views.VCustomersMessage> customersMessageList = new List<NFine.Entity.Views.VCustomersMessage>();

            List<CustomersEntity> customersList = customersApp.GetList(pagination, queryJson);
            foreach (CustomersEntity c in customersList)
            {
                NFine.Entity.Views.VCustomersMessage customersMessageTemp = new NFine.Entity.Views.VCustomersMessage();

                customersMessageTemp.customerId = c.F_Id;
                customersMessageTemp.CRMID = c.F_CrmId;
                customersMessageTemp.customerInformation = c.F_CustInfo;
                customersMessageTemp.customerStateId = c.F_StateId;

                string queryJsonTemp = "{ \"F_CustomeId\":\""+c.F_Id+"\"}";//Json格式字符串

                List<CustomeRecordsEntity> customeRecordsList = customeRecordsApp.GetList(queryJsonTemp);


                try
                {
                    customersMessageTemp.Record1 = customeRecordsList[0].F_Record;
                    customersMessageTemp.Record1Id = customeRecordsList[0].F_Id;

                    customersMessageTemp.Record2 = customeRecordsList[1].F_Record;
                    customersMessageTemp.Record2Id = customeRecordsList[1].F_Id;

                    customersMessageTemp.Record3 = customeRecordsList[2].F_Record;
                    customersMessageTemp.Record3Id = customeRecordsList[2].F_Id;

                    customersMessageTemp.Record4 = customeRecordsList[3].F_Record;
                    customersMessageTemp.Record4Id = customeRecordsList[3].F_Id;
                }
                catch { }

                customersMessageList.Add(customersMessageTemp);
            }
            var data = new
            {
                // rows = BLL.Self.CustomersMessageBLL.Instance.GetList(queryJson), 
                //rows = customersApp.GetList(pagination, queryJson),
                rows = customersMessageList,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]

        //获取F_Id字段为keyValue值的数据并返回JSon类型 客户
        public ActionResult GetFormJson(string keyValue)
        {
            var data = customersApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        //获取CRMID
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetCrmIdText(string keyValue)
        {
            string data = DAL.OC_UserInfoDAL.Instance.FindEntity(t=>t.F_Id==keyValue).F_CrmId.ToString();
            return data;
        }

        [HttpGet]
        [HandlerAjaxOnly]

        //获取F_Id字段为keyValue值的数据并返回JSon类型 客户
        public string GetFIDByCRMID(string keyValue)
        {
            var CRMID = keyValue.ToInt();
            string data = DAL.CRM_CustomersDAL.Instance.FindEntity(t => t.F_CrmId ==CRMID).F_Id;
            return data;
        }

        [HttpGet]
        [HandlerAjaxOnly]

        //获取F_Id字段为keyValue值的数据并返回JSon类型 客户记录
        public ActionResult GetRecordFormJson(string keyValue)
        {
            var data = customeRecordsApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        public ActionResult CustomerSubmitForm(CustomersEntity customersEntity, string keyValue)
        {
            customersApp.SubmitForm(customersEntity, keyValue);//客户提交 
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        public ActionResult RecordSubmitForm(CustomeRecordsEntity customeRecordsEntity, string keyValue)
        {
            customeRecordsApp.SubmitForm(customeRecordsEntity, keyValue);//客户记录提交 
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        //单个数据提交
        //customersEntity:待提交数据
        //keyValue:区分提交方式标志，为空：添加数据，否则修改数据
        public ActionResult SubmitForm(Entity.Views.VCustomersMessage customersMessage, string keyValue)
        {
            CustomersEntity customersEntity = new CustomersEntity();
            customersEntity.F_CustInfo = customersMessage.F_CustInfo;
            customersEntity.F_MobilePhone = customersMessage.F_MobilePhone;
            customersEntity.F_StateId = customersMessage.F_StateId;
            customersEntity.F_CallTime = customersMessage.F_CallTime;

            customersApp.SubmitForm(customersEntity, keyValue);//客户提交

            CustomeRecordsEntity customeRecordsEntity = new CustomeRecordsEntity();
            customeRecordsEntity.F_CustomeId = customersEntity.F_Id;
            customeRecordsEntity.F_Record = customersMessage.F_Record;
            customeRecordsApp.SubmitForm(customeRecordsEntity, keyValue);  //客户记录提交

            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        //批量数据提交，直接添加数据
        //customersList:待提交数据 
        public ActionResult BatchSubmitForm(List<Entity.Views.VCustomersMessage> customersList)   
        {
            int n = 0;
            foreach (Entity.Views.VCustomersMessage v in customersList)
            {
                CustomersEntity customersEntity = new CustomersEntity();
                customersEntity.F_CustInfo = v.F_CustInfo;
                customersEntity.F_MobilePhone = v.F_MobilePhone;
                customersEntity.F_StateId = v.F_StateId;
                customersEntity.F_CallTime = v.F_CallTime;

                customersApp.SubmitForm(customersEntity, "");//客户提交

                CustomeRecordsEntity customeRecordsEntity = new CustomeRecordsEntity();
                customeRecordsEntity.F_CustomeId = customersEntity.F_Id;
                customeRecordsEntity.F_Record = v.F_Record;
                customeRecordsApp.SubmitForm(customeRecordsEntity, "");  //客户记录提交
                n++;

            }
            return Success("成功添加" + n + "条客户信息。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        //删除F_Id字段为keyValue值的数据
        public ActionResult DeleteForm(string keyValue)
        {
            customersApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        //删除F_Id字段为keyValue值的数据
        public ActionResult RecordDeleteForm(string keyValue)
        {
            customeRecordsApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

    }
}



