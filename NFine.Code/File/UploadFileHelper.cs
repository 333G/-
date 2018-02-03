/*******************************************************************************
 * Copyright © 2016 NeoLu 版权所有
 * Author: NeoLu
 * Description: 文件上传帮助类
 * Website：http://www.cnblogs.com/woaic
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NFine.Code
{
    public class UploadFileHelper
    {
        private HttpPostedFileBase _file;
        private string _folder;
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="file">上传控件</param>
        /// <param name="folder">上传的文件夹相对路径</param>
        public UploadFileHelper(HttpPostedFileBase file,string folder)
        {
            _file = file;
            _folder = folder;
        }
        public void Upload(out string strNewFileName)
        {
            if (!FileHelper.IsExistDirectory(_folder))
            {
                FileHelper.CreateDir(_folder);
            }
            string strOldFilePath = "", strExtension = "";
            strOldFilePath = _file.FileName;
            //取得上传文件的扩展名
            strExtension = strOldFilePath.Substring(strOldFilePath.LastIndexOf("."));
            //文件上传后的命名 （GUID）
            strNewFileName = Guid.NewGuid().ToString().Replace("-", "") + strExtension;
            _file.SaveAs(HttpContext.Current.Server.MapPath(_folder) + "/" + strNewFileName);
        }
    }
}
