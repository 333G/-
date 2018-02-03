using NFine.Code;
using NFine.Entity;
using NFine.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class PhoneNumAreaInfoController : ControllerBase
    {
        //
        // GET: /OCManage/PhoneNumAreaInfo/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PhoneNumAreaInfoIndex()
        {
            return View();
        }
        public ActionResult ImportForm()
        {
            return View();
        }


        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = BLL.Sys_PhoneNumAreaInfoManager.Instance.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        //导入号码归属地
        //上传Excel文件并导入
        [HttpPost]
        public ActionResult UploadFile(Sys_PhoneNumAreaInfo model, HttpPostedFileBase file)
        {
            // HttpPostedFileBase file = Request.Files["file"];

            #region 上传文件

            if (file == null)
                return Error("没有文件！");
            //允许上传的文件格式类型
            bool checkExtensionResult = NFine.Code.FileHelper.CheckExtension(file.FileName, new string[] { ".xls", ".xlsx", ".csv" });
            if (!checkExtensionResult)
                return Error("上传文件格式有误，请重新选择");

            string filePath = "/Resource/Sys_PhoneNumAreaInfo/" + DateTime.Now.ToString("yyyy/MM/dd");
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

            List<Sys_PhoneNumAreaInfo> list = TableToList(dt);
            list = EqualityComparer(list);
            var result = DAL.Sys_PhoneNumAreaInfoDAL.Instance.Add(list);
            return Success(string.Format("上传成功！成功导入{0}条数据", result.Count));
        }

        //对比重复，去重
        private List<Sys_PhoneNumAreaInfo> EqualityComparer(List<Sys_PhoneNumAreaInfo> baselist)
        {
            List<Sys_PhoneNumAreaInfo> Alllist = DAL.Sys_PhoneNumAreaInfoDAL.Instance.FindList("select F_NumSegment,F_Province,F_City,F_Operator from Sys_PhoneNumAreaInfo");
            baselist = comparelist(baselist, Alllist);//对比去重
            return baselist;
        }

        /// <summary>
        /// table数据转成对象
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<Sys_PhoneNumAreaInfo> TableToList(DataTable dt)
        {
            List<Sys_PhoneNumAreaInfo> list = new List<Sys_PhoneNumAreaInfo>();
            var userModel = NFine.Code.OperatorProvider.Provider.GetCurrent();

            foreach (DataRow dr in dt.Rows)
            {
                Sys_PhoneNumAreaInfo model = new Sys_PhoneNumAreaInfo();
                model.F_Id = dr[0].ToString();
                model.F_NumSegment = dr[1].ToString();
                model.F_Province = dr[2].ToString();
                model.F_City = dr[3].ToString();
                model.F_Operator = dr[4].ToString();
                model.F_PostCode = dr[5].ToString();
                model.F_AreaCode = dr[6].ToString();
                model.F_ReMark = null;         
                list.Add(model);
            }
            return list;
        }
        private List<Sys_PhoneNumAreaInfo> comparelist(List<Sys_PhoneNumAreaInfo> List, List<Sys_PhoneNumAreaInfo> AllList)
        {
            List<Sys_PhoneNumAreaInfo> NewList = new List<Sys_PhoneNumAreaInfo>();
            List.ForEach(t => NewList.Add(t));//复制list
            try
            {
                for (int i = 0; i < List.Count; i++)
                {
                    for (int j = 0; j < AllList.Count; j++)
                    {
                        if (List[i].F_NumSegment == AllList[j].F_NumSegment)
                        {
                            if (NewList.Contains(List[i]))
                                NewList.Remove(List[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NewList = new List<Sys_PhoneNumAreaInfo>();
                return NewList;
            }
            return NewList;
        }

    }
}
