/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class AreaController : ControllerBase
    {
        private AreaApp areaApp = new AreaApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = areaApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (AreaEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = areaApp.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (AreaEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = true;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                treeList = treeList.TreeWhere(t => t.text.Contains(keyword), "id", "parentId");
            }
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = areaApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(AreaEntity areaEntity, string keyValue)
        {
            areaApp.SubmitForm(areaEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            areaApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }


        //根据编号取得省份
       [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetProvinceDataJson(string keyValue)
        {
            var data = new
            {
                ProGrup = this.GetProGrupList(keyValue)
            };
            return Content(data.ToJson());
        }

        private object GetProGrupList(string keyValu)
        {
            var data = areaApp.GetForm(keyValu);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("FullName", data.F_FullName);
            return dictionary;
        }
        //根据parentid取省份
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetnewProvinceDataJson(string keyValue)
        {
            var data = new
            {
                ProGrup = this.GetnewProGrupList(keyValue)
            };
            return Content(data.ToJson());
        }

        private object GetnewProGrupList(string keyValue)
        {
            
            var data = areaApp.GetForm(keyValue);
            var result = DAL.Sys_AreaDAL.Instance.FindEntity(t => t.F_Id == data.F_ParentId);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("FullName", result.F_FullName);
            return dictionary;
        }


        //根据编号取省份
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetProvinceJson()
        {
            var data = areaApp.GetList();
            var treelist = new List<TreeSelectModel>();
            foreach (AreaEntity item in data)
            {
                if (item.F_Layers ==1)
                {
                    TreeSelectModel treeModel = new TreeSelectModel();
                    treeModel.id = item.F_Id;
                    treeModel.text = item.F_FullName;
                    treeModel.parentId = "0";
                    treeModel.data = item;
                    treelist.Add(treeModel);
                }
                else continue;
            }
            return Content(treelist.TreeSelectJson());
        }

        [HttpGet]
        public string GetProvinceName()
        {
            string[] name=new string [34] ;
            string[] id = new string [34];
            var data = areaApp.GetList();
            var treelit = new List<TreeSelectModel>();
            int i = 0;
            foreach (AreaEntity item in data)
            {
                if (item.F_Layers == 1)
                {
                    name[i] = item.F_FullName;
                    id[i] = item.F_Id;
                    i++;
                }
            }
            string Name=null;
            string Id = null;
            for (i = 0; i < name.Length; i++)
            {
                Name += name[i] + ",";
                Id += id[i] + ',';
            }
            return Name+Id;
        }

    }
}
