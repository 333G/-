using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using NFine.Domain.Entity.OCManage;
using NFine.Application.OCManage;
using System.Linq;
using SqlSugar;
using System.Data.SqlClient;
using NFine.DAL;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class SensitiveWordsController : ControllerBase
    {
        //全局变量，用来获取导入敏感词时的类型和通道
        bool IsChannelKeyWord;
        int? ChannelId;
        public ActionResult SensitiveWordsIndex()
        {
            return View();
        }
        public ActionResult InsertForm()
        {
            return View();
        }
        public ActionResult ImportForm()
        {
            return View();
        }
        private SensitiveWordsApp SensitiveWordsApp = new SensitiveWordsApp();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = BLL.SMS_SensitiveWordsManager.Instance.GetList(pagination, queryJson),
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
            var data = BLL.SMS_SensitiveWordsManager.Instance.Model(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SensitiveWordsEntity SensitiveWordsEntity, string keyValue)
        {
            SensitiveWordsApp.SubmitForm(SensitiveWordsEntity, keyValue);
            return Success("操作成功。");
        }

        //多词输入
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult InsertForm(SensitiveWordsEntity SensitiveWordsEntity, string keyValue)
        {
            var str = SensitiveWordsEntity.F_SensitiveWords.EndsWith(",");//判断最后一位是否以“,”结尾，不满足会有空值插入
            if (str == false)
            {
                return Error("请以“,”结尾");
            }
            else
            {
                string[] Words = SensitiveWordsEntity.F_SensitiveWords.Split(',');
                List<SMS_SensitiveWords> list = new List<SMS_SensitiveWords>();
                var userModel = OperatorProvider.Provider.GetCurrent();
                try
                {
                    for (int x = 0; x < Words.Length - 1; x++)
                    {
                        SMS_SensitiveWords model = new SMS_SensitiveWords();
                        if (SensitiveWordsEntity.F_IsChannelKeyWord == true)//通道敏感词
                        {
                            if (SensitiveWordsEntity.F_ChannelId == null)//添加通道敏感词时，判断是否漏选通道
                                return Error("请选择通道!");
                            else
                            {
                                model.F_ChannelId = SensitiveWordsEntity.F_ChannelId;
                                model.F_IsChannelKeyWord = SensitiveWordsEntity.F_IsChannelKeyWord;
                            }                          
                        }
                        else
                        {
                            model.F_ChannelId = 0;
                            model.F_IsChannelKeyWord = SensitiveWordsEntity.F_IsChannelKeyWord;
                        }
                        model.F_SensitiveWords = Words[x];
                        model.F_CreatorTime = DateTime.Now;
                        model.F_CreatorUserId = userModel.Id.ToString();
                        model.F_LastModifyTime = DateTime.Now;
                        model.F_LastModifyUserId = userModel.UserId;
                        model.F_Id = Guid.NewGuid().ToString();  //not null
                        list.Add(model);
                    }
                    list = EqualityComparer(list);//对比去重
                    SMS_SensitiveWordsDAL.Instance.Add(list);
                }
                catch (Exception ex)
                {
                    return Error("发生错误：" + ex + "请联系管理员!");
                }
                return Success("操作成功。");
            }          
        }

        //导入敏感词
        //上传Excel文件并导入
        [HttpPost]
        public ActionResult UploadFile(SMS_SensitiveWords model, HttpPostedFileBase file)
        {
            //if (model.F_IsChannelKeyWord == 0)

             IsChannelKeyWord = model.F_IsChannelKeyWord;
             ChannelId = model.F_ChannelId;
            // HttpPostedFileBase file = Request.Files["file"];

            #region 上传文件

            if (file == null)
                return Error("没有文件！");
            //允许上传的文件格式类型
            bool checkExtensionResult = NFine.Code.FileHelper.CheckExtension(file.FileName, new string[] { ".xls", ".xlsx", ".csv" });
            if (!checkExtensionResult)
                return Error("上传文件格式有误，请重新选择");

            string filePath = "/Resource/SMS_SensitiveWord/" + DateTime.Now.ToString("yyyy/MM/dd");
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

            List<SMS_SensitiveWords> list = TableToList(dt);
            list= EqualityComparer(list);
            var result = DAL.SMS_SensitiveWordsDAL.Instance.Add(list);
            return Success(string.Format("上传成功！成功导入{0}条数据", result.Count));
        }

        //对比重复，去重
        private List<SMS_SensitiveWords> EqualityComparer(List<SMS_SensitiveWords> baselist)
        {
            List<SMS_SensitiveWords> Alllist = DAL.SMS_SensitiveWordsDAL.Instance.FindList("select F_Id,F_SensitiveWords from SMS_SensitiveWords");
            baselist = comparelist(baselist, Alllist);//对比去重
            return baselist;
        }

        /// <summary>
        /// table数据转成对象
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SMS_SensitiveWords> TableToList(DataTable dt)
        {
            List<SMS_SensitiveWords> list = new List<SMS_SensitiveWords>();
            var userModel = NFine.Code.OperatorProvider.Provider.GetCurrent();

            foreach (DataRow dr in dt.Rows)
            {
                SMS_SensitiveWords model = new SMS_SensitiveWords();
                model.F_CreatorTime = DateTime.Now;
                model.F_CreatorUserId = userModel.Id.ToString();
                model.F_SensitiveWords = dr[0].ToString();
                model.F_Id = Guid.NewGuid().ToString();  //not null
                model.F_IsChannelKeyWord = IsChannelKeyWord;
                model.F_ChannelId = ChannelId;
                list.Add(model);
            }
            return list;
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            SensitiveWordsApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        private List<SMS_SensitiveWords> comparelist(List<SMS_SensitiveWords> List, List<SMS_SensitiveWords> AllList)
        {
            List<SMS_SensitiveWords> NewList = new List<SMS_SensitiveWords>();
            List.ForEach(t => NewList.Add(t));//复制list

            try
            {
                for (int i = 0; i < List.Count; i++)
                {
                    for (int j = 0; j < AllList.Count; j++)
                    {
                        if (List[i].F_SensitiveWords == AllList[j].F_SensitiveWords)
                        {
                            if (NewList.Contains(List[i]))
                                NewList.Remove(List[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NewList = new List<SMS_SensitiveWords>();
                return NewList;
            }
            return NewList;
        }

    }
}
       