using NFine.Application.TXLManage;
using NFine.Code;
using NFine.Domain.Entity.TXLManage;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFine.Application.SystemManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.TXLManage;

namespace NFine.Web.Areas.TXLManage.Controllers
{
    public class PheGrupController : ControllerBase
    {
        private PheInfoApp pheInfoApp = new PheInfoApp();
        private PheGrupApp pheGrupApp = new PheGrupApp();
        //private IPheGrupRepository service = new PheGrupRepository();

        public ActionResult MoveMemberForm()
        {
            return View();
        }
        public ActionResult AddMore()
        {
            return View();
        }
        public ActionResult SendSMS()
        {
            return View();
        }
       
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson(string groupId,bool t)
        {
            var data = pheGrupApp.GetList();
            var treeList = new List<TreeSelectModel>();
            if (groupId == "")//全部群组
            {
                foreach (PheGrupEntity item in data)
                {
                    TreeSelectModel treeModel = new TreeSelectModel();
                    treeModel.id = item.F_Id;
                    treeModel.text = item.GroupName;
                    treeModel.parentId = "0";
                    treeModel.data = item;
                    treeList.Add(treeModel);
                }
            }
            else
            {
                if (t==true)//群组groupId
                {
                    var g = pheGrupApp.GetForm(groupId);
                    TreeSelectModel treeModel = new TreeSelectModel();
                    treeModel.id = g.F_Id;
                    treeModel.text = g.GroupName;
                    treeModel.parentId = "0";
                    treeModel.data = g;
                    treeList.Add(treeModel);
                }
                else//除groupId外其他群组
                {
                    foreach (PheGrupEntity item in data)
                    {
                        TreeSelectModel treeModel = new TreeSelectModel();
                        if (item.F_Id != groupId)
                        {
                            treeModel.id = item.F_Id;
                            treeModel.text = item.GroupName;
                            treeModel.parentId = "0";
                            treeModel.data = item;
                            treeList.Add(treeModel);
                        }
                    }
                }
            }
                return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
           // List<PheGrupEntity>a=pheGrupApp.GetList(pagination, keyword);
            
            var data = new
            {
                rows =  BLL.Self.PheGrupBLL.Instance.GetList(keyword),
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
            var data = pheGrupApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
 
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(PheGrupEntity pheGrupEntity, string keyValue)
        {
            pheGrupApp.SubmitForm(pheGrupEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            string[] FIDList = keyValue.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            for (int x = 0; x < FIDList.Count(); x++)
            {
                pheGrupApp.DeleteForm(FIDList[x]);
            }
                return Success("删除成功。");
        }

        //根据组编号取得组名
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPheGrupDataJson(string keyValue)
        {
            var data = new
            {
                PheGrup = this.GetPheGrupList(keyValue)
            };
            return Content(data.ToJson());
        }

        private object GetPheGrupList(string keyValu)
        {
            var data = pheGrupApp.GetForm(keyValu);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("GroupName", data.GroupName);
            return dictionary;
        }

        //取指定用户下的所有组
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGroupJson(string userId)
        {
            if (userId == null ) {
                userId = OperatorProvider.Provider.GetCurrent().UserId;
            }
            var data = pheGrupApp.GetList(userId);
            var treeList = new List<TreeSelectModel>();
            foreach (PheGrupEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.GroupName;
                treeModel.parentId = "0";
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        //取所有分组号码
        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetTreeGroupPhone(string GropIds)
        //{
        //    string[] array = GropIds.Split(':');
        //    //if (GropIds == null)
        //    //{
        //    //    GropIds = OperatorProvider.Provider.GetCurrent().GropIds;
        //    //}
        //    //string phone = HttpContext.Request["json"].ToString();
        //    string phone = GropIds;
        //    var data = pheGrupApp.GetList(phone);
        //    var treeList = new List<TreeSelectModel>();
        //    foreach (PheGrupEntity item in data)
        //    {
        //        TreeSelectModel treeModel = new TreeSelectModel();
        //        treeModel.id = item.F_Id;
        //        treeModel.text = item.GroupName;
        //        treeModel.parentId = "0";
        //        treeModel.data = item;
        //        treeList.Add(treeModel);
        //    }
        //    return Content(treeList.TreeSelectJson());
        //}


        //上传Excel文件并导入
        [HttpPost]
        public ActionResult UploadFile(TXL_PhoneInfo model, HttpPostedFileBase file)
        {
            // HttpPostedFileBase file = Request.Files["file"];
            #region 上传文件

            if (AID == null)
                return Error("请先选择群组！");

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
            List<TXL_PhoneInfo> list = TableToList(dt,AID);
            int AllCount = list.Count();
            list = EqualityComparer(list);//如果是批量导入黑白名单，则对比去重
            var result = BLL.TXL_PhoneInfoManager.Instance.Add(list);
            return Success(string.Format("上传成功！成功导入{0}条数据", result.Count, ",失败了{1}条数据", AllCount - result.Count()));
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
        private List<TXL_PhoneInfo> TableToList(DataTable dt,string id)
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
                model.GroupId =id;                  //群组id,直接取用前端数据
                model.Name = dr[2].ToString();      //姓名
                model.Sex = dr[3].ToString();       //性别
                string Provincename = dr[4].ToString();  //省份名称
                model.Province = getProvince(Provincename);//根据名称获取省份ID，存入数据库
                model.City = dr[5].ToString();       // 城市
                model.Operator = dr[6].ToString();   //运营商
                model.State = dr[7].ToInt();          //状态
                model.F_Description = dr[8].ToString();  //备注

                model.F_Id = Guid.NewGuid().ToString();  //not null
                list.Add(model);
            }
            return list;
             AID = null;//清除全局变量AID值
        }
        public string getProvince(string value)
        {
            AreaApp area = new AreaApp();
            var data = area.GetList();
            var treelist = new List<TreeSelectModel>();
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
        public static string AID = null;
        [HttpGet]
        [HandlerAjaxOnly]
        public string getGroupId(string keyValue)  //获取前端选中的导入组的GroupId
        {
            AID = keyValue;//修改全局变量AID
            return AID;
        }


        public string getPhoneNum(string keyValue)
        {
            string value = null;
            string[] GroupIdArry = keyValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < GroupIdArry.Count(); i++)
            {
                value += export(GroupIdArry[i]);
            }
                return value;
        }//根据传入的groupid，导出组内成员电话号码

        private string export(string groupid)
        {
            string queryJson = "{\"GroupId\":\"" + groupid+ "\"}";
            List<TXL_PhoneInfo> list = NFine.BLL.TXL_PhoneInfoManager.Instance.GetList(queryJson);
            DataTable dt = ListToTable(list);
            string keyvalue=null;
            foreach (DataRow dr in dt.Rows)
            {
               if(groupid==dr[1].ToString())
                    keyvalue = keyvalue+dr[0].ToString() + ",";
            }
            return keyvalue;
        }
        private DataTable ListToTable(List<TXL_PhoneInfo> list)
        //    private DataTable ListToTable(List<PheInfoEntity> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("手机号");
            dt.Columns.Add("组名");
            

            foreach (var model in list)
            {
                PheGrupApp pheGrupApp = new PheGrupApp();      
                DataRow dr = dt.NewRow();
                dr[0] = model.Mobile;
                dr[1] = model.GroupId;              
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }

}
