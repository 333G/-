using NFine.Application.TXLManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.TXLManage;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using NFine.Entity;
using System;
using System.Web.Script.Serialization;
using System.IO;
using NFine.Application.SystemManage;
using NFine.Domain.Entity.SystemManage;


namespace NFine.Web.Areas.TXLManage.Controllers
{
    public class PheInfoController : ControllerBase
    {
        private PheInfoApp pheInfoApp = new PheInfoApp();

        public ActionResult SelIndex()
        {
            return View();
        }
        public ActionResult SendSMS()
        {
            return View();
        }
        public ActionResult AddMore()
        {
            return View();
        }
        public ActionResult MoveMember()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            
            var data = new
            {
                rows = pheInfoApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = pheInfoApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(PheInfoEntity pheInfoEntity, string keyValue,string groupId)
        {
            pheInfoApp.SubmitForm(pheInfoEntity, keyValue);
            return Success("操作成功。"+ groupId);
        }

        //防伪表单字段不存在问题
        //批量数据提交，直接添加数据
        //pheInfoList:待提交数据 
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken] 
        public ActionResult BatchSubmitForm(List<PheInfoEntity> pheInfoList)
        {
            int n = 0;
            foreach (PheInfoEntity i in pheInfoList)
            {
                pheInfoApp.SubmitForm(i, i.F_Id);
                n++;
            }

            return Success("成功移动" + n + "条成员信息。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            pheInfoApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        public ActionResult ClearForm(Pagination pagination, string keyValue)
        {
            int n = pheInfoApp.ClearMember(pagination,keyValue);  
            if(n==0)
                return Success("该群组尚无成员，不需要清空。");
            return Success("该群组人员已清空，共清除" + n + "条人员信息");
        }

        /// <summary>
        /// 将查询结果导出到本地
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Export(string GroupId, string Sex,string Operator,string State,string keyword)
        {
            //后台封装
            string queryJson = "{\"GroupId\":\"" + GroupId + "\",\"Sex\":\"" + Sex + "\",\"Operator\":\"" + Operator + "\",\"State\":\"" + State + "\",\"keyword\":\"" + keyword + "\"}";
            List<TXL_PhoneInfo> list = NFine.BLL.TXL_PhoneInfoManager.Instance.GetList(queryJson);
                DataTable dt = ListToTable(list);

                NFine.Code.Excel.NPOIExcel helper = new Code.Excel.NPOIExcel();
                string path = "/Resource/TXL_PhoneInfo-Export/" + DateTime.Now.ToString("yyyy/MM/dd");
                string fileName = Guid.NewGuid() + ".xls";
                if (!NFine.Code.FileHelper.IsExistDirectory(Server.MapPath(path)))
                    NFine.Code.FileHelper.CreateDir(path);
                helper.ToExcel(dt, "通讯录人员", "sheet1", Server.MapPath(path + "/" + fileName));
                
            return DownFile(path + "/" + fileName, "通讯录人员.xls");
         

        }

        /// <summary>
        /// List对象转换成Table
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable ListToTable(List<TXL_PhoneInfo> list)
        //    private DataTable ListToTable(List<PheInfoEntity> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("手机号");
            dt.Columns.Add("组名");
            dt.Columns.Add("姓名");
            dt.Columns.Add("性别");
            dt.Columns.Add("省");
            dt.Columns.Add("市");
            dt.Columns.Add("运营商");
            dt.Columns.Add("状态");
            dt.Columns.Add("备注");

            foreach (var model in list)
            { 
                PheGrupApp pheGrupApp = new PheGrupApp();
                string groupName =pheGrupApp.GetForm(model.GroupId).GroupName;
                var data = DAL.Sys_AreaDAL.Instance.FindEntity(t => t.F_Id == model.Province);
                DataRow dr = dt.NewRow();
                dr[0] = model.Mobile;
                dr[1] = groupName; 
                dr[2] = model.Name;
                dr[3] = model.Sex;
                //dr[4] = model.Province;
                dr[4] = data.F_FullName;
                dr[5] = model.City;
                dr[6] = model.Operator;
                dr[7] = model.State==1 ? "正常" : "未知";
                dr[8] = model.F_Description; 
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /*/// <summary>
         /// 导出信息
         /// </summary>
         /// <param name=""></param>
        [HttpPost]
        [HandlerAuthorize]
        public ActionResult ExpToXls(string queryJson)
        {
            DataTable getdatatable = pheInfoApp.GetDataTable(queryJson);
            NPOIExcel npoiexcel = new NPOIExcel();
            npoiexcel.ToExcel(getdatatable, "人员管理", "sheet1", "d:\text.xls");
            return Success("导出成功。");
        }

        /// <summary>
        /// 取手机号码信息
        /// </summary>
        /// <param name="keyValue"></param>
        public Dictionary<string, object> GetPhoneNumbInfo(string keyValue)
        {
            string MobileStr = Common.request_phonenumb("", "phone=" + keyValue);
            Dictionary<string, object> Dt = Code.Json.JsonReader(MobileStr);
            Dictionary<string, object> dt = new Dictionary<string, object>();

            if (Dt["retMsg"].ToString() == "success") //api返回success时的处理
            {
                dt = Code.Json.JsonReader(Dt["retData"].ToString());
            }
            else
            {  //如果不能识别号码，则显示为未知              
                dt["province"] = "未知";
                dt["city"] = "未知";
                dt["supplier"] = "未知";
            }
            return dt;
        }
        */

        //上传Excel文件并导入
        [HttpPost]
        public ActionResult UploadFile(TXL_PhoneInfo model, HttpPostedFileBase file)
        {
            // HttpPostedFileBase file = Request.Files["file"];

            #region 上传文件

            if (file == null)
                return Error("没有文件！");
            //允许上传的文件格式类型
            bool checkExtensionResult = NFine.Code.FileHelper.CheckExtension(file.FileName, new string[] { ".xls", ".xlsx", ".csv" });
            if (!checkExtensionResult)
                return Error("上传文件格式有误，请重新选择");

            string filePath = "/Resource/TXL_PhoneInfo/" + DateTime.Now.ToString("yyyy/MM/dd");
            string strNewFileName;
            NFine.Code.UploadFileHelper uploadFile = new UploadFileHelper(file, filePath);
            uploadFile.Upload(out strNewFileName);

            #endregion 上传文件

            DataTable dt = null;
            //读取csv并转换成table
            if (NFine.Code.FileHelper.CheckExtension(file.FileName, new string[] { ".csv" }))
            {
                CSVParser helper = new CSVParser(Server.MapPath(filePath + "/" + strNewFileName));
                dt = helper.ParseToDataTable();
                //dt = helper.GetCsvData(Server.MapPath(filePath), strNewFileName.ToLower().Substring(0, strNewFileName.IndexOf('.')));
            }
            else
            {
                //读取excel并转换成table
                NFine.Code.Excel.ExcelHelper helper = new Code.Excel.ExcelHelper(Server.MapPath(filePath) + "/" + strNewFileName);
                dt = helper.ExportExcelToDataTable();
            }
            if (dt == null || dt.Rows.Count == 0)
                return Success("上传成功！成功导入0条数据");

            //table转换成list对象，并执行批量添加操作

            List<TXL_PhoneInfo> list = TableToList(dt);
            list = EqualityComparer(list);//如果是批量导入黑白名单，则对比去重
            var result = BLL.TXL_PhoneInfoManager.Instance.Add(list);
            return Success(string.Format("上传成功！成功导入{0}条数据", result.Count));
        }

        //对比重复，去重
        private List<TXL_PhoneInfo> EqualityComparer(List<TXL_PhoneInfo> baselist)
        {
            List<TXL_PhoneInfo> Alllist = DAL.TXL_PhoneInfoDAL.Instance.FindList("select * from TXL_PhoneInfo");
            baselist = comparelist(baselist, Alllist);//对比去重
            return baselist;
        }
        private List<TXL_PhoneInfo> comparelist(List<TXL_PhoneInfo> List, List<TXL_PhoneInfo> AllList)
        {
            List<TXL_PhoneInfo> NewList = new List<TXL_PhoneInfo>();
            List.ForEach(t => NewList.Add(t));//复制list
            try
            {
                for (int i = 0; i < List.Count; i++)
                {
                    for (int j = 0; j < AllList.Count; j++)
                    {
                        if (List[i].Mobile == AllList[j].Mobile & List[i].GroupId == AllList[j].GroupId)
                        {
                            if (NewList.Contains(List[i]))
                                NewList.Remove(List[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NewList = new List<TXL_PhoneInfo>();
                return NewList;
            }
            return NewList;
        }

        /// <summary>
        /// table数据转成对象
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<TXL_PhoneInfo> TableToList(DataTable dt)
        {
            List<Entity.TXL_PhoneInfo> list = new List<TXL_PhoneInfo>();
            var userModel = NFine.Code.OperatorProvider.Provider.GetCurrent();
            
            foreach (DataRow dr in dt.Rows)
            {
                TXL_PhoneInfo model = new TXL_PhoneInfo();
                model.F_CreatorTime = DateTime.Now;
                model.F_CreatorUserId = userModel.Id.ToString();
                model.F_DeleteMark = false;
                model.F_EnabledMark = true;
                
                
                model.Mobile = dr[0].ToString();    //手机号

                string groupname =dr[1].ToString(); //组名
                model.GroupId = getgroupid(groupname);//根据组名转换层id存入数据库

                model.Name = dr[2].ToString();      //姓名
                model.Sex = dr[3].ToString();       //性别
                string Province_Name = dr[4].ToString();  //省份名
                model.Province = getProvince(Province_Name);
                model.City = dr[5].ToString();       // 城市
                model.Operator = dr[6].ToString();   //运营商
                model.State = dr[7].ToInt();          //状态
                model.F_Description = dr[8].ToString();  //备注

                model.F_Id = Guid.NewGuid().ToString();  //not null
                list.Add(model);
            }
            return list;
        }
        
        public string getProvince(string value)
        {
            AreaApp area = new AreaApp();
            var data = area.GetList();
            var treelist= new List<TreeSelectModel>();
            string ID = null;
            foreach (AreaEntity item in data)
            {
                TreeSelectModel treemodel = new TreeSelectModel();
                treemodel.id = item.F_Id;
                treemodel.text = item.F_FullName;
                treemodel.data = item;
                treelist.Add(treemodel);
                if (value == treemodel.text)
                {
                    ID = item.F_Id;
                }
                else continue;
            }
            return ID;
        }
        public string getgroupid(string value)
        {
            PheGrupApp pheGrupApp = new PheGrupApp();
            var data = pheGrupApp.GetList();
            string ID=null;
            var treelist = new List<TreeSelectModel>();
            foreach (PheGrupEntity item in data)
            {
                    TreeSelectModel treeModel = new TreeSelectModel();
                    treeModel.id = item.F_Id;
                    treeModel.text = item.GroupName;
                    treeModel.parentId = "0";
                    treeModel.data = item;
                    treelist.Add(treeModel);
                if (value == treeModel.text)
                {
                     ID = item.F_Id;
                }
                else continue;
            }
            return ID ;
        }


    }
}
