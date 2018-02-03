/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    [HandlerLogin]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload()
        {
            var files = Request.Files;
            if (files.Count <= 0)
                return Content("error|文件不能为空");

            HttpPostedFileBase file = files[0];
            //允许上传的文件格式类型
            bool checkExtensionResult = NFine.Code.FileHelper.CheckExtension(file.FileName, new string[] { ".jpg", ".png",".gif",".bmp" });
            if (!checkExtensionResult)
                return Content("error|文件格式不正确，只能上传jpg,png,gif,bmp格式的图片");

            string filePath = "/Resource/editor/" + DateTime.Now.ToString("yyyy/MM/dd");
            string strNewFileName;
            NFine.Code.UploadFileHelper uploadFile = new UploadFileHelper(file, filePath);
            uploadFile.Upload(out strNewFileName);

            return Content(filePath+"/"+strNewFileName);
        }
    }
}
