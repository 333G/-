using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class BlackWhiteListController : ControllerBase
    {
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = BLL.OC_BlackListManager.Instance.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(int id)
        {
            var data = BLL.OC_BlackListManager.Instance.Model(id);
            return Content(data.ToJson());
        }


        //根据编号取用户
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetUserInfoJson()
        {
            var data = "select * from OC_UserInfo ";
            List<OC_UserInfo> resultList = DAL.OC_UserInfoDAL.Instance.FindList(data);
            var treelist = new List<TreeSelectModel>();
            int length = resultList.Count;
            for (int j = 0; j < length; j++)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = resultList[j].F_Id;
                treeModel.text = resultList[j].F_Account;
                treeModel.parentId = "0";
                treelist.Add(treeModel);
            }
            return Content(treelist.TreeSelectJson());
        }


        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(OC_BlackList model)
        {
            bool result = false;
            //登录用户信息
            var userModel = Code.OperatorProvider.Provider.GetCurrent();
            if (userModel == null)
                return Error("您的登录超时，请重新登录再进行操作！");
            //bool checkMobile = NFine.Code.Validate.IsValidMobile(model.F_Mobile);
            //if (!checkMobile)
            //    return Error("手机号码格式有误，请检查！");
            if (model.Id > 0)
            {
                //更新
                var newModel = BLL.OC_BlackListManager.Instance.Model(model.Id);

                newModel.F_Mobile = model.F_Mobile;
                newModel.F_Sign = model.F_Sign;
                newModel.F_Level = model.F_Level;
                newModel.F_Description = model.F_Description;
                newModel.F_LastModifyTime = DateTime.Now;
                newModel.F_LastModifyUserId = userModel.Id;
                newModel.F_LastUserAccount = userModel.UserCode;
                newModel.ComeFrom = 1;//管理员添加
                newModel.UserId = 0;//管理员添加时，用户ID为0

                result = BLL.OC_BlackListManager.Instance.Update(newModel);
            }
            else
            {
                //新增
                model.F_CreatorTime = DateTime.Now;
                string fid = Request["UserId"];//用于判断添加的是系统的还是用户的
                string[] newmobile = model.F_Mobile.Split(',');
                int length = newmobile.Length;
                for (int i = 0; i < length; i++)
                {
                    if (fid != "")//说明添加的是用户黑名单/白名单
                    {
                        var newresult = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_Id == fid);
                        model.F_CreatorUserId = newresult.F_UserId;
                        model.Creator = newresult.F_UserFid;
                    }
                    else//说明是系统添加的黑/白名单
                    {
                        model.F_CreatorUserId = userModel.Id;
                        model.Creator = userModel.UserId;
                    }
                    model.ComeFrom = 1;//管理员添加
                    model.UserId = 0;//管理员添加时，用户ID为0
                    model.F_EnabledMark = true;
                    model.F_DeleteMark = false;
                    //bool res = BLL.OC_BlackListManager.Instance.CheckMobile(model.F_Mobile);//检查号码是否已存在黑白名单里
                    bool res = BLL.OC_BlackListManager.Instance.CheckMobile(newmobile[i]);//检查号码是否已存在黑白名单里
                    if (res)
                    {
                        return Error("号码已存在！");
                    }
                    else
                    {
                        model.F_Mobile = newmobile[i];
                        result = BLL.OC_BlackListManager.Instance.Add(model) > 0;
                    }
                        
                }
            }
            if (result)
                return Success("操作成功。");
            return Error("操作失败，请刷新页面重试");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string ids)
        {
            var userModel = NFine.Code.OperatorProvider.Provider.GetCurrent();
            string[] idList = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<bool> result = BLL.OC_BlackListManager.Instance.Delete(idList, userModel.Id);
            if (result.Count > 0)
                return Success("删除成功。");
            return Error("删除失败，请刷新重试");
        }

        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult CheckMobile(string mobile)
        {
            bool result = BLL.OC_BlackListManager.Instance.CheckMobile(mobile);
            if (result)
                return Success("success");
            return Error("fail");
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchImport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            #region 上传文件

            if (file == null)
                return Error("没有文件！");
            //允许上传的文件格式类型
            bool checkExtensionResult = NFine.Code.FileHelper.CheckExtension(file.FileName, new string[] { ".xls", ".xlsx" });
            if (!checkExtensionResult)
                return Error("上传文件格式有误，请重新选择");

            string filePath = "/Resource/BlackWhite/" + DateTime.Now.ToString("yyyy/MM/dd");
            string strNewFileName;
            NFine.Code.UploadFileHelper uploadFile = new UploadFileHelper(file, filePath);
            uploadFile.Upload(out strNewFileName);

            #endregion 上传文件

            //读取excel并转换成table
            NFine.Code.Excel.ExcelHelper helper = new Code.Excel.ExcelHelper(Server.MapPath(filePath) + "/" + strNewFileName);
            DataTable dt = helper.ExportExcelToDataTable();
            if (dt == null || dt.Rows.Count == 0)
                return Success("上传成功！成功导入0条数据");

            //table转换成list对象，并执行批量添加操作
            List<OC_BlackList> list = TableToList(dt);
            list = EqualityComparer(list);//如果是批量导入黑白名单，则对比去重
            var result = BLL.OC_BlackListManager.Instance.Add(list);
            return Success(string.Format("上传成功！成功导入{0}条数据", result.Count));
        }


        //对比重复，去重
        private List<OC_BlackList> EqualityComparer(List<OC_BlackList> baselist)
        {
            List<OC_BlackList> Alllist = DAL.OC_BlackListDAL.Instance.FindList("select * from OC_BlackList");
            baselist = comparelist(baselist, Alllist);//对比去重
            return baselist;
        }
        private List<OC_BlackList> comparelist(List<OC_BlackList> List, List<OC_BlackList> AllList)
        {
            List<OC_BlackList> NewList = new List<OC_BlackList>();
            List.ForEach(t => NewList.Add(t));//复制list
            try
            {
                for (int i = 0; i < List.Count; i++)
                {
                    for (int j = 0; j < AllList.Count; j++)
                    {
                        if (List[i].F_Mobile == AllList[j].F_Mobile & List[i].F_Sign == AllList[j].F_Sign)
                        {
                            if (NewList.Contains(List[i]))
                                NewList.Remove(List[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NewList = new List<OC_BlackList>();
                return NewList;
            }
            return NewList;
        }



        /// <summary>
        /// table数据转成对象
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<OC_BlackList> TableToList(DataTable dt)
        {
            List<Entity.OC_BlackList> list = new List<OC_BlackList>();
            var userModel = NFine.Code.OperatorProvider.Provider.GetCurrent();
            foreach (DataRow dr in dt.Rows)
            {
                OC_BlackList model = new OC_BlackList();
                model.ComeFrom = 1;
                model.Creator = userModel.UserCode;
                model.F_CreatorTime = DateTime.Now;
                model.F_CreatorUserId = userModel.Id;
                model.F_DeleteMark = false;
                model.F_Description = dr[3].ToString();
                model.F_EnabledMark = true;
                if (dr[1].ToString() == "黑名单")
                {
                    model.F_Level = 0;
                }
                else
                {
                    model.F_Level = dr[2].ToInt();
                    if (model.F_Level < 100)
                        model.F_Level = 100;
                    else if (model.F_Level > 180)
                        model.F_Level = 180;
                }
                model.F_Mobile = dr[0].ToString();
                model.F_Sign = dr[1].ToString() == "黑名单";
                model.UserId = userModel.Id;

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Export(string F_Mobile, string F_Sign, string F_Level)
        {
            List<OC_BlackList> list = NFine.BLL.OC_BlackListManager.Instance.GetList(F_Mobile, F_Sign, F_Level);
            DataTable dt = ListToTable(list);
            NFine.Code.Excel.NPOIExcel helper = new Code.Excel.NPOIExcel();
            string path = "/Resource/BlackWhite-Export/" + DateTime.Now.ToString("yyyy/MM/dd");
            string fileName = Guid.NewGuid() + ".xls";
            if (!NFine.Code.FileHelper.IsExistDirectory(Server.MapPath(path)))
                NFine.Code.FileHelper.CreateDir(path);
            helper.ToExcel(dt, "黑白名单", "sheet1", Server.MapPath(path + "/" + fileName));
            return DownFile(path + "/" + fileName, "黑白名单.xls");
        }

        /// <summary>
        /// List对象转换成Table
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable ListToTable(List<OC_BlackList> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("手机号");
            dt.Columns.Add("类型");
            dt.Columns.Add("级别");
            dt.Columns.Add("备注");

            foreach (var model in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = model.F_Mobile;
                dr[1] = model.F_Sign.ToBool() ? "黑名单" : "白名单";
                dr[2] = model.F_Level;
                dr[3] = model.F_Description;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        [HttpGet]
        public ActionResult Test()
        {
            NFine.Search.LuceneHelper helper = new Search.LuceneHelper();
            helper.CreateIndex();
            return Content("创建索引成功！");
        }

        [HttpGet]
        public ActionResult Test2()
        {
            NFine.Search.LuceneHelper helper = new Search.LuceneHelper();
            List<string> list = new List<string>();
            helper.SearchCheck("测试", out list);
            return Content(list.ToJson());
        }
    }
}