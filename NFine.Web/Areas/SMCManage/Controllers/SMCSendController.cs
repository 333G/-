using NFine.Application.SMCManage;
using NFine.Application.TXLManage;
using NFine.Code;
using NFine.Domain.Entity.SMCManage;
using NFine.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ToolGood.Words;


namespace NFine.Web.Areas.SMCManage.Controllers
{
    public class SMCSendController : ControllerBase
    {
        decimal? transition;//全局变量，用于将普通用户发送短信计算出的费用赋值给smcSendEntity.F_Price
        int resultzifuNum;//全局变量，用来接收短信内容字符数，超过500不能发送
        int FLG;

        private PheGrupApp pheGrupApp = new PheGrupApp();
        private PheInfoApp pheInfoApp = new PheInfoApp();
        //
        // GET: /SMCManage/SMCSend/
        private SMCSendApp smcSendApp = new SMCSendApp();

        public ActionResult SendForm()
        {
            return View();
        }
        public ActionResult SendEditForm()
        {
            return View();
        }

        public ActionResult TXTSendForm()
        {
            return View();
        }

        public ActionResult TXLSendForm()
        {
            return View();
        }

        public ActionResult XLSSendForm()
        {
            return View();
        }
        public ActionResult MobileListForm()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = smcSendApp.GetListSugar(pagination, queryJson),//换用sqlsugar 的方式获取数据
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            //for (int x = 0; x < data.rows.Count(); x++)////后台把号码给截断100位,只是临时方法，最好是在sql中做（sql中已处理）
            //{
            //    if (data.rows[x].F_MobileList.Length > 100)
             //   {
              //      data.rows[x].F_MobileList = data.rows[x].F_MobileList.Substring(0,100);
             //   }
            //}
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SMCSendEntity smcSendEntity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                var LoginInfo = OperatorProvider.Provider.GetCurrent();
                List<string> DirtyWord = CheckDirtyWord(smcSendEntity.F_SmsContent, smcSendEntity.F_GroupChannelId);//发现敏感词
                if (DirtyWord.Count != 0)//有敏感词处理
                {
                    return Error("有敏感词:" + DirtyWord + "，请处理！");
                }
                //去掉重复号码并更新F_MobileList
                string newF_MobileList = MobileRemoval(smcSendEntity.F_MobileList);
                smcSendEntity.F_MobileList = newF_MobileList;

                smcSendEntity.F_SmsContent.TrimEnd(); //去除字符串末尾空格；
                smcSendEntity.F_MobileCount = smcSendEntity.F_MobileList.Split(',').Length;//手机号码条数           
                if (LoginInfo.UserCode != "admin")
                {
                    bool result = CheckBalance(smcSendEntity.F_SmsContent, smcSendEntity.F_GroupChannelId, smcSendEntity.F_MobileCount.ToInt()); //余额检验 
                    DateTime now = DateTime.Now;
                    if (!smcSendEntity.F_IsTimer)
                        smcSendEntity.F_SendTime = now;
                    if (result)
                    {
                        //待审核时，费用应为 0.00 ，此时还未拆号计费。待拆号完成后，更新计费数据.
                        //smcSendEntity.F_Price = transition; //transition为普通用户发送短信的费用  
                        int newzifuNum = resultzifuNum;
                        if (newzifuNum > 500)
                            return Error("短信字数最多500字，请检查！");
                        else
                        {
                            smcSendEntity.F_Price = 0.00M;
                            smcSendApp.SubmitForm(smcSendEntity, keyValue);
                        }
                    }
                    else
                        return Error("余额不足，请检查！");
                }
                else
                {
                    DateTime now = DateTime.Now;
                    if (!smcSendEntity.F_IsTimer)
                        smcSendEntity.F_SendTime = now;
                    smcSendApp.SubmitForm(smcSendEntity, keyValue);
                }
                return Success("操作成功。");
            }
            else{
                try
                {
                    smcSendEntity.Modify(keyValue);
                    smcSendApp.UpdateForm(smcSendEntity);
                    return Success("操作成功。");
                }
                catch(Exception ex)
                { return Error("出现错误，请联系管理员：" + ex); }
            }
        }

        //敏感词校验
        private List<string> CheckDirtyWord(string F_SmsContent ,string GroupChannelId)
        {
            List<string> flag = new List<string>();
            List<string> SplitWords = SearchMethod(F_SmsContent);//分词，每个分词来查询
            foreach (var item in SplitWords)
            {
                if ((DAL.SMS_SensitiveWordsDAL.Instance.FindEntity(t => t.F_IsChannelKeyWord == false && t.F_SensitiveWords == item))!=null)
                {
                    flag.Add(item);
                }
            }
            string ChannelSensitive = GetChannelSensitive(GroupChannelId);//获取通道关键字
            if (ChannelSensitive != null)
            {
                StringSearch keyword = new StringSearch();
                keyword.SetKeywords(ChannelSensitive.Split('|'));
                flag .AddRange( keyword.FindAll(F_SmsContent));//判断有无敏感词,并返回所有
            }
            return flag;
        }

        //分词
        public List<string> SearchMethod(string content)
        {
            NFine.Search.LuceneHelper helper = new Search.LuceneHelper();
            List<string> list = new List<string>();
            list = helper.Search(content);
            return list;
        }

        //XLS中敏感词替换
        private string RemoveDirtyWord(string F_SmsContent,string GroupChannelId)
        {
            string Content = F_SmsContent;
            string SensitiveStr = null;        
            List<string> SplitWords = SearchMethod(F_SmsContent);//分词
            foreach (var item in SplitWords)
            {
                if ((DAL.SMS_SensitiveWordsDAL.Instance.FindEntity(t => t.F_IsChannelKeyWord == false && t.F_SensitiveWords == item)) != null)
                {
                    SensitiveStr = SensitiveStr + item + "|";//敏感词库中有这个分词，加入敏感词列
                }
            }
            if (SensitiveStr != null)
            {
                StringSearch keyword = new StringSearch();
                keyword.SetKeywords(SensitiveStr.Split('|'));
                Content = keyword.Replace(F_SmsContent, '*');//系统关键字屏蔽    
            }
            string ChannelSensitive = GetChannelSensitive(GroupChannelId);//获取通道关键字
            if (ChannelSensitive != null)
            {
                StringSearch keyword = new StringSearch();
                keyword.SetKeywords(ChannelSensitive.Split('|'));
                Content = keyword.Replace(Content, '*');//通道关键字屏蔽
            }
            return Content;
        }

        //获取通道敏感词
        private string GetChannelSensitive(string GroupChannelId)
        {
            List<SMS_SensitiveWords> Sensitivelist = new List<SMS_SensitiveWords>();
            try
            {
                OC_GroupChannel GroupChannelModel = DAL.OC_GroupChannelDAL.Instance.FindEntity(t => t.F_ID == GroupChannelId);
                int MobileChannelId = GroupChannelModel.F_MobileChannel.ToInt();
                int UnicomChannelId = GroupChannelModel.F_UnicomChannel.ToInt();
                int TelecomChannelId = GroupChannelModel.F_TelecomChannel.ToInt();
                Sensitivelist = DAL.SMS_SensitiveWordsDAL.Instance.FindList(t => t.F_IsChannelKeyWord == true && 
                (t.F_ChannelId == MobileChannelId || t.F_ChannelId == UnicomChannelId || t.F_ChannelId == TelecomChannelId));
                string SensitiveStr = null;
                if (Sensitivelist.Count() > 0)
                {
                    foreach (var item in Sensitivelist)
                    {
                        SensitiveStr += item.F_SensitiveWords + "|";
                    }
                }
                return SensitiveStr;

            }
            catch
            {
                return null;
            }
        }



        //校验余额是否充足
        public bool CheckBalance(string zifu, string GroupChannelId, int phoneNum)
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            int zifuNum = zifu.Length; //获取输入的字符数
            resultzifuNum = zifuNum;//传给全局变量，提交时用来判断是否超过500字符
            var sys_user = DAL.Sys_UserDAL.Instance.FindEntity(t => t.F_Id == LoginInfo.UserId);
            string zfsid = sys_user.F_Signature;
            int zfsidNum = zfsid.Length + 2;//获取占短信内容字符数
            var groupchannel = DAL.OC_GroupChannelDAL.Instance.FindEntity(t => t.F_ID == GroupChannelId);
            decimal? danjia = groupchannel.F_ChannelPrice;
            int num = Math.Ceiling((double)(zifuNum + zfsidNum) / 66).ToInt() ;//可以将短信分为几条
            decimal? sum = num * danjia * phoneNum;//总价格=（（输入的字符+占短信内容字符）/66）*通道单价*手机号码数
            var oc_userInfo = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_Id == LoginInfo.UserId);
            decimal? money = oc_userInfo.F_Balance * 100;//“元”转换成“分”
            transition = sum;//普通用户发送短信的费用赋值给全局变量
            if (sum <= money)
            {
                ////发送成功后更新当前用户的剩余金额
                //double shengyu = ((double)money - (double)sum) / 100;//“分”转换成“元”
                //oc_userInfo.F_Balance = (decimal)shengyu;
                //DAL.OC_UserInfoDAL.Instance.Update(oc_userInfo);
                return true;
            }
            else
            {
                return false;
            }
        }

        //提交手机号去重
        private string MobileRemoval(string mobile)
        {
            List<string> list = new List<string>(mobile.Split(','));
            list = list.Distinct().ToList();
            string newF_MobileList = string.Join(",", list.ToArray());
            return newF_MobileList;
        }

        //取所有分组号码
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetTreeGroupPhone(string GropIds)
        {
            //获取前台的分组信息
            string[] array = GropIds.Split(':');
            ArrayList phoneArray = new ArrayList();
            //获取所有手机信息
            var phoneList = pheInfoApp.GetList();
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < phoneList.Count; j++)
                {
                    if (array[i] == phoneList[j].GroupId)
                    {
                        phoneArray.Add(phoneList[j].Mobile);
                    }
                }
            }

            string json = string.Join(",", (string[])phoneArray.ToArray(typeof(string)));

            return json;
            //return Content(json);
        }

        //按条件更改通道
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult ChangeChannel(string queryJson, int keyvalue)//ID or RootId
        {
            List<SMC_SendSms> listdata = new List<SMC_SendSms>();
            var queryParam = queryJson.ToJObject();
            try
            {
                listdata = DAL.SMC_SendSmsDAL.Instance.GetList(queryJson);
                if (listdata.Count == 0)
                    return Error("没有找到可以合并的数据，请重新输入条件。");
            }
            catch (Exception ex)
            {
                return Error("Ops，出现了错误：" + ex);
            }
            foreach (var item in listdata)
            {
                item.F_GroupChannelId = queryParam["F_GroupChannelId"].ToString();//重新赋值通道
            }
            if (queryParam["F_GroupChannelId"] != null)
                DAL.SMC_SendSmsDAL.Instance.UpdateRange(listdata);//批量更新
            return Success("修改通道成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            string sql = "select F_Id from OC_UserInfo ";
            List<OC_UserInfo> resultList = DAL.OC_UserInfoDAL.Instance.FindList(sql);
            int length = resultList.Count;
            Dictionary<string, string> map = new Dictionary<string, string>();
            for (int i = 0; i < length; i++)
            {
                OC_UserInfo oc_UserInfo = resultList[i];
                map.Add(oc_UserInfo.F_Id, oc_UserInfo.F_Id);
            }
            if (map.ContainsKey(LoginInfo.UserId)) //判断是否为普通用户
            {
                var data = DAL.SMC_SendSmsDAL.Instance.FindEntity(t => t.F_Id == keyValue);
                if (data.F_DealState == 0 & data.F_OperateState == 0)
                {
                    smcSendApp.DeleteForm(keyValue);
                    return Success("删除成功！");
                }
                else
                    return Error("该信息已审核，无法删除！");
            }
            else
            {
                smcSendApp.DeleteForm(keyValue);
                return Success("删除成功！");
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult TXTSendDo()
        {
            String utf = UploadTXTFile();
            if (utf != "true")
            {
                return Success(utf);
            }
            string data2 = Request.Form["data2"];//取得传送序列
            return Success("短信发送成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]

        //获取F_Id字段为keyValue值的数据并返回JSon类型 客户
        public ActionResult GetFormJson(string keyValue)
        {
            var data = smcSendApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult TXTSendSubmitForm(SMCSendEntity smcSendEntity, string keyValue)
        {
            var oc_UserInfo = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_Id == OperatorProvider.Provider.GetCurrent().UserId);//获取当前登陆者实体信息，以便取得ROOTID
            smcSendEntity.F_SmsContent = Request.Form["F_Content"];//内容
            smcSendEntity.F_SmsContent.TrimEnd();//去除字符串末尾空格
            List<string> DirtyWord = CheckDirtyWord(smcSendEntity.F_SmsContent,smcSendEntity.F_GroupChannelId);//发现敏感词
            if (DirtyWord.Count != 0)//有敏感词处理
            {
                return Error("有敏感词:" + DirtyWord + "，请处理！");
            }

            string utf = UploadTXTFile();
            // string data2 = Request.Form["data2"];//取得传送序列
            //smcSendEntity.F_SmsContent = Request.Form["F_Content"];//内容
            string Is_Timer = Request.Form["F_IsTimer"];//定时开关
            if (Is_Timer == "on")
            {
                string str = Request.Form["F_SendTime"].ToString();//发送时间  
                smcSendEntity.F_SendTime = DateTime.ParseExact(str, "yyyy-MM-dd HH:mm:ss", null);
                smcSendEntity.F_IsTimer = true;
                smcSendEntity.F_SendState = 1;
            }
            else
            {
                smcSendEntity.F_SendTime = DateTime.Now;
                smcSendEntity.F_IsTimer = false;
                smcSendEntity.F_SendState = 0;
            }

            // smcSendEntity.F_SendChannl = Request.Form["F_SendChannl"];//取得传送序列

            smcSendEntity.F_SmbSign = "txt";
            smcSendEntity.F_SendSign = "文本";
            smcSendEntity.F_MobileList = utf;//存的是号码

            bool data = TXTMobileChar(smcSendEntity.F_MobileList);//检测手机号是否有非法字符
            if(data)
            {
                //去掉重复号码并更新F_MobileList
                string newF_MobileList = MobileRemoval(smcSendEntity.F_MobileList);
                smcSendEntity.F_MobileList = newF_MobileList;
            }
            else
                return Error("手机号有非法字符，请检查！");
            smcSendEntity.F_MobileCount = utf.Split(',').Length; ;//存的是号码数量
            smcSendEntity.F_RootId = oc_UserInfo.F_RootId;
            if (OperatorProvider.Provider.GetCurrent().UserCode != "admin")
            {
                bool result = CheckBalance(smcSendEntity.F_SmsContent, smcSendEntity.F_GroupChannelId, smcSendEntity.F_MobileCount.ToInt());//余额检验
                if (result)
                {
                    smcSendEntity.F_Price = 0.00M;
                    //smcSendEntity.F_Price = transition;
                    smcSendApp.SubmitForm(smcSendEntity, keyValue);
                }
                else
                    return Error("余额不足，请充值！");
            }
            else
                smcSendApp.SubmitForm(smcSendEntity, keyValue);
            return Success("短信发送成功。");
        }

        private string UploadTXTFile()
        {
            HttpPostedFileBase postedFile = Request.Files["F_SelTXT"];//获取上传信息对象
            string filePath = postedFile.FileName;//获取上传的文件路径
            string path;   //上传文件服务器上路径

            string fileExt = filePath.Substring(filePath.LastIndexOf("."));
            if (fileExt == ".txt" || fileExt == ".TXT")//必须是文本文件
            {
                try
                {
                    //服务器上传文件夹路径
                    string upfilePath = Server.MapPath("~/FileUpLoad/upload/");
                    if (!Directory.Exists(upfilePath))
                    {
                        Directory.CreateDirectory(upfilePath);
                    }
                    //文件在服务器上的路径
                    path = System.IO.Path.Combine(Server.MapPath("~/FileUpLoad/upload/"), System.IO.Path.GetFileName(DateTime.Now.ToString("yyyymmddhhMMss") + fileExt));
                    postedFile.SaveAs(path);//保存
                    //读取服务器上的txt文件
                    StreamReader sr = new StreamReader(path);
                    string phoneList = sr.ReadToEnd();

                    sr.Close();
                    //sr.Dispose();
                    string[] moblieArray = phoneList.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    string phones = string.Join(",", moblieArray);//字符串拼接，用,号分隔
                    int phoneCount = moblieArray.Length;  //获取电话号码的个数

                    //删除服务器上的上传的文件
                    //if (System.IO.File.Exists(path))
                    //{
                    //    System.IO.File.Delete(path);
                    //}
                    return phones;
                }
                catch
                {
                    return "文件上传失败！";
                }
            }
            else
            {
                return "文件格式不正确！";
            }
        }



        //判断手机号有无非法字符
        private bool TXTMobileChar(string mobile)
        {
            char[] str = mobile.ToCharArray();
            int flg = 0;     
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' & str[i] <= '9') || str[i] == ',')
                    flg++;
                else
                {
                    FLG = -1;
                    break;
                }                  
            }
            if (FLG == -1)
                return false;
            else
                return true;
        }


        //上传Excel文件并发送
        [HttpPost]
        //public ActionResult UploadFile(SMC_SendSms model, HttpPostedFileBase file)
        //{
        public ActionResult UploadFile(SMCSendEntity model, HttpPostedFileBase file)
        {
            // HttpPostedFileBase file = Request.Files["file"];

            #region 上传文件

            if (file == null)
                return Error("没有文件！");
            //允许上传的文件格式类型
            bool checkExtensionResult = NFine.Code.FileHelper.CheckExtension(file.FileName, new string[] { ".xls", ".xlsx", ".csv" });
            if (!checkExtensionResult)
                return Error("上传文件格式有误，请重新选择");

            string filePath = "/Resource/SMC_SendSms/" + DateTime.Now.ToString("yyyy/MM/dd");
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
            List<SMC_SendSms> list = TableToList(dt);

            var arr = list.Select(x => x.F_MobileList).ToArray();
            bool data = TXTMobileChar(arr.ToString());//判断手机号是否有非法字符
            if (data)//如果没有，检查手机号是否有重复
                list = Removallist(list);//去除excel里向同一个手机号发相同的内容
            else
                return Error("手机号有非法字符，请检查！");
            int MobileNumCount = list.Count;
            int Priority = DAL.OC_GroupChannelDAL.Instance.FindEntity(t => t.F_ID == model.F_GroupChannelId).F_Priority.ToInt() - 5;//获取通道优先级,XLS上传的数据优先级自动-5       
            if (MobileNumCount / 1000 > 0)
                Priority = Priority -  5;//如果单次插入号码>1000，优先级降低五个
            int ssum = 0;

            for (int i = 0; i < MobileNumCount; i++)
            {
                if (OperatorProvider.Provider.GetCurrent().UserCode != "admin")
                {
                    int tongji = list[i].F_SmsContent.Length;
                    list[i].F_SmsContent = RemoveDirtyWord(list[i].F_SmsContent,list[i].F_GroupChannelId);//替换敏感词
                    ssum = ssum + tongji;
                    list[i].F_GroupChannelId = model.F_GroupChannelId;
                    list[i].F_MobileCount = 1;
                    list[i].F_Priority = Priority;//批量赋值优先级
                    //以下是将excel表的每条数据计算价格并插入到SMC_SendSms的F_Price字段
                    var newgroupchannel = DAL.OC_GroupChannelDAL.Instance.FindEntity(t => t.F_ID == model.F_GroupChannelId);
                    decimal? newdanjia = newgroupchannel.F_ChannelPrice;
                    //decimal? listnum = (tongji / 66 + 1) * newdanjia * 1;//计算excel里每条短信的价格
                    decimal? listnum = 0.00M;
                    list[i].F_Price = listnum;
                }

            }
            bool newresult = CheckBalance(ssum.ToString(), model.F_GroupChannelId, MobileNumCount);//余额检验
            if (newresult)
            {
                var result = BLL.SMC_SendSmsManager.Instance.Add(list);
                return Success(string.Format("上传成功！成功导入{0}条数据", result.Count));
            }
            else
            {
                return Error("余额不足，请充值！");
            }
            //else
            //{
            //    var result = BLL.SMC_SendSmsManager.Instance.Add(list);
            //    return Success(string.Format("上传成功！成功导入{0}条数据", result.Count));
            //}
        }

        /// <summary>
        /// table数据转成对象
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SMC_SendSms> TableToList(DataTable dt)
        {

            List<Entity.SMC_SendSms> list = new List<SMC_SendSms>();

            var userModel = NFine.Code.OperatorProvider.Provider.GetCurrent();
            var oc_UserInfo = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_Id == OperatorProvider.Provider.GetCurrent().UserId);//获取当前登陆者实体信息，以便取得ROOTID
            string Is_Timer = Request["F_IsTimer"]; //定时开关
            foreach (DataRow dr in dt.Rows)
            {
                SMC_SendSms model = new SMC_SendSms();
                model.F_CreatorTime = DateTime.Now;
                //model.F_CreatorUserId = userModel.Id.ToString();
                model.F_CreatorUserId = userModel.UserId;
                model.F_RootId = oc_UserInfo.F_RootId;
                model.F_OperateState = 0; //默认待处理
                model.F_DeleteMark = false;
                model.F_EnabledMark = true;

                //定时开关
                if (Is_Timer == "on")
                {
                    string str = Request["F_SendTime"].ToString();//发送时间  
                    model.F_SendTime = DateTime.ParseExact(str, "yyyy-MM-dd HH:mm:ss", null);
                    model.F_IsTimer = true;
                }
                else
                {
                    model.F_SendTime = DateTime.Now;
                    model.F_IsTimer = false;
                }

                model.F_SendSign = "手机号";
                model.F_SmbSign = "excel";
                model.F_SendState = 0;
                model.F_DealState = 0;
                model.F_MobileList = dr[0].ToString();    //手机号List
                model.F_SmsContent = dr[1].ToString();   //发送内容
                model.F_Id = Guid.NewGuid().ToString();  //not null

                list.Add(model);
            }
            return list;
        }

        //excel群发时去除重复的号码与内容（即：去除向同一个手机号发相同的内容）
        private List<SMC_SendSms> Removallist(List<SMC_SendSms> list)
        {  
            int length = list.Count;
            Dictionary<string, SMC_SendSms> map = new Dictionary<string, SMC_SendSms>();
            for (int i = 0; i < length; i++)
            {
                SMC_SendSms model = new SMC_SendSms();
                string mobillist = list[i].F_MobileList;
                string smscontent = list[i].F_SmsContent;
                string str = mobillist + smscontent;
                if (!map.ContainsKey(str))
                {
                    model.F_CreatorTime = list[i].F_CreatorTime;
                    model.F_CreatorUserId = list[i].F_CreatorUserId;
                    model.F_DealState = list[i].F_DealState;
                    model.F_DeleteMark = list[i].F_DeleteMark;
                    model.F_DeleteTime = list[i].F_DeleteTime;
                    model.F_DeleteUserId = list[i].F_DeleteUserId;
                    model.F_EnabledMark = list[i].F_EnabledMark;
                    model.F_GroupChannelId = list[i].F_GroupChannelId;
                    model.F_Id = list[i].F_Id;
                    model.F_IsTimer = list[i].F_IsTimer;
                    model.F_IsVerificationCode = list[i].F_IsVerificationCode;
                    model.F_LastModifyTime = list[i].F_LastModifyTime;
                    model.F_LastModifyUserId = list[i].F_LastModifyUserId;
                    model.F_MobileCount = list[i].F_MobileCount;
                    model.F_MobileList = list[i].F_MobileList;
                    model.F_OperateState = list[i].F_OperateState;
                    model.F_Price = list[i].F_Price;
                    model.F_Priority = list[i].F_Priority;
                    model.F_RootId = list[i].F_RootId;
                    model.F_SendSign = list[i].F_SendSign;
                    model.F_SendState = list[i].F_SendState;
                    model.F_SendTime = list[i].F_SendTime;
                    model.F_Signature = list[i].F_Signature;
                    model.F_SmbSign = list[i].F_SmbSign;
                    model.F_SmsContent = list[i].F_SmsContent;
                    map.Add(str, model);
                }
            }
            List<SMC_SendSms> DictionaryToList = map.Values.ToList();
            return DictionaryToList;
        }



        //选中批量审核
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Review(string keyValue)
        {
            string[] keys = keyValue.Split(',');
            int length = keyValue.Split(',').Length - 1;
            if (length == 0)
            {
                return Error("未选择要处理的数据，请检查！");
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    var result = smcSendApp.GetForm(keys[i]);
                    if (result.F_DealState == 9)
                        return Error("已审核，无需修改！");

                    else
                    {
                        result.F_DealState = 9;
                        smcSendApp.UpdateForm(result);
                    }
                }
                return Success("状态已改为审核！");
            }
        }

        //按条件审核
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult NewReview(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            var keyF_UserId = queryParam["F_UserId"];
            var keyF_RootId = queryParam["F_RootId"];
            var keyF_SmsContent = queryParam["F_SmsContent"];

            if (keyF_UserId != null & keyF_RootId.ToString() == "" & keyF_SmsContent.ToString() == "")
            {
                var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
                string fid = result.F_Id;
                string sql = "select * from SMC_SendSms where F_CreatorUserId = '" + fid + "'";
                updaterange(sql);

            }
            else if (keyF_RootId != null & keyF_UserId.ToString() == "" & keyF_SmsContent.ToString() == "")
            {
                string sql = "select * from SMC_SendSms where F_RootId = '" + keyF_RootId + "'";
                updaterange(sql);
            }
            else if (keyF_SmsContent != null & keyF_UserId.ToString() == "" & keyF_RootId.ToString() == "")
            {
                string sql = "select * from SMC_SendSms where F_SmsContent like '%" + keyF_SmsContent + "%'";
                updaterange(sql);
            }
            else if (keyF_UserId != null & keyF_RootId != null & keyF_SmsContent.ToString() == "")
            {
                var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
                string fid = result.F_Id;
                string sql = "select * from SMC_SendSms where F_RootId = '" + keyF_RootId + "' and F_CreatorUserId = '" + fid + "'";
                updaterange(sql);
            }
            else if (keyF_UserId != null & keyF_RootId.ToString() == "" & keyF_SmsContent != null)
            {
                var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
                string fid = result.F_Id;
                string sql = "select * from SMC_SendSms where F_CreatorUserId = '" + fid + "' and F_SmsContent like '%" + keyF_SmsContent + "%'";
                updaterange(sql);
            }
            else if (keyF_UserId.ToString() == "" & keyF_RootId != null & keyF_SmsContent != null)
            {
                string sql = "select * from SMC_SendSms where F_RootId = '" + keyF_RootId + "' and F_SmsContent like '%" + keyF_SmsContent + "%'";
                updaterange(sql);
            }
            else if (keyF_UserId != null & keyF_RootId != null & keyF_SmsContent != null)
            {
                var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
                string fid = result.F_Id;
                string sql = "select * from SMC_SendSms where F_CreatorUserId = '" + fid + "' and F_RootId = '" + keyF_RootId + "' and F_SmsContent like '%" + keyF_SmsContent + "%'";
                updaterange(sql);
            }
            return Success("状态已改为审核！");
        }

        public List<SMC_SendSms> updaterange(string sql)
        {
            List<SMC_SendSms> resultList = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
            int length = resultList.Count;
            for (int i = 0; i < length; i++)
            {
                if (resultList[i].F_DealState == 9)
                    Console.WriteLine("状态已为审核！");
                else
                    resultList[i].F_DealState = 9;
                DAL.SMC_SendSmsDAL.Instance.Update(resultList[i]);
            }
            return resultList;
        }


        //选中批量处理
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(string keyValue)
        {
            string[] keys = keyValue.Split(',');
            int length = keyValue.Split(',').Length - 1;
            if (length == 0)
            {
                return Error("未选择要处理的数据，请检查！");
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    var result = smcSendApp.GetForm(keys[i]);
                    if (result.F_OperateState == 9)
                        return Error("已处理，无需修改！");

                    else
                    {
                        result.F_OperateState = 9;
                        result.F_SendState = -2;//不发送
                        smcSendApp.UpdateForm(result);
                    }
                }
                return Success("状态已改为处理！");
            }
        }

        ////按条件已处理
        //[HttpGet]
        //[HandlerAjaxOnly]

        //public ActionResult NewManage(Pagination pagination, string queryJson)
        //{
        //    var queryParam = queryJson.ToJObject();
        //    var keyF_UserId = queryParam["F_UserId"];
        //    var keyF_RootId = queryParam["F_RootId"];
        //    var keyF_SmsContent = queryParam["F_SmsContent"];
        //    try
        //    {
        //        if (keyF_UserId != null & keyF_RootId.ToString() == "" & keyF_SmsContent.ToString() == "")
        //        {
        //            var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
        //            string fid = result.F_Id;
        //            string sql = "select * from SMC_SendSms where F_CreatorUserId = '" + fid + "'";
        //            List<SMC_SendSms> resultList = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
        //            int length = resultList.Count;
        //            for (int i = 0; i < length; i++)
        //            {
        //                if (resultList[i].F_OperateState == 9)
        //                    return Success("选择的记录已是“已处理”状态！");
        //                else
        //                    resultList[i].F_OperateState = 9;
        //                DAL.SMC_SendSmsDAL.Instance.Update(resultList[i]);
        //            }

        //        }
        //        else if (keyF_RootId != null & keyF_UserId.ToString() == "" & keyF_SmsContent.ToString() == "")
        //        {
        //            string sql = "select * from SMC_SendSms where F_RootId = '" + keyF_RootId + "'";
        //            List<SMC_SendSms> resultList = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
        //            int length = resultList.Count;
        //            for (int i = 0; i < length; i++)
        //            {
        //                if (resultList[i].F_OperateState == 9)
        //                    return Success("选择的记录已是“已处理”状态！");
        //                else
        //                    resultList[i].F_OperateState = 9;
        //                DAL.SMC_SendSmsDAL.Instance.Update(resultList[i]);
        //            }
        //        }
        //        else if (keyF_SmsContent != null & keyF_UserId.ToString() == "" & keyF_RootId.ToString() == "")
        //        {
        //            string sql = "select * from SMC_SendSms where F_SmsContent like '%" + keyF_SmsContent + "%'";
        //            List<SMC_SendSms> resultList = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
        //            int length = resultList.Count;
        //            for (int i = 0; i < length; i++)
        //            {
        //                if (resultList[i].F_OperateState == 9)
        //                    return Success("选择的记录已是“已处理”状态！");
        //                else
        //                    resultList[i].F_OperateState = 9;
        //                DAL.SMC_SendSmsDAL.Instance.Update(resultList[i]);
        //            }
        //        }
        //        else if (keyF_UserId != null & keyF_RootId != null & keyF_SmsContent.ToString() == "")
        //        {
        //            var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
        //            string fid = result.F_Id;
        //            string sql = "select * from SMC_SendSms where F_RootId = '" + keyF_RootId + "' and F_CreatorUserId = '" + fid + "'";
        //            List<SMC_SendSms> resultList = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
        //            int length = resultList.Count;
        //            for (int i = 0; i < length; i++)
        //            {
        //                if (resultList[i].F_OperateState == 9)
        //                    return Success("选择的记录已是“已处理”状态！");
        //                else
        //                    resultList[i].F_OperateState = 9;
        //                DAL.SMC_SendSmsDAL.Instance.Update(resultList[i]);
        //            }
        //        }
        //        else if (keyF_UserId != null & keyF_RootId.ToString() == "" & keyF_SmsContent != null)
        //        {
        //            var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
        //            string fid = result.F_Id;
        //            string sql = "select * from SMC_SendSms where F_CreatorUserId = '" + fid + "' and F_SmsContent like '%" + keyF_SmsContent + "%'";
        //            List<SMC_SendSms> resultList = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
        //            int length = resultList.Count;
        //            for (int i = 0; i < length; i++)
        //            {
        //                if (resultList[i].F_OperateState == 9)
        //                    return Success("选择的记录已是“已处理”状态！");
        //                else
        //                    resultList[i].F_OperateState = 9;
        //                DAL.SMC_SendSmsDAL.Instance.Update(resultList[i]);
        //            }
        //        }
        //        else if (keyF_UserId.ToString() == "" & keyF_RootId != null & keyF_SmsContent != null)
        //        {
        //            string sql = "select * from SMC_SendSms where F_RootId = '" + keyF_RootId + "' and F_SmsContent like '%" + keyF_SmsContent + "%'";
        //            List<SMC_SendSms> resultList = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
        //            int length = resultList.Count;
        //            for (int i = 0; i < length; i++)
        //            {
        //                if (resultList[i].F_OperateState == 9)
        //                    return Success("选择的记录已是“已处理”状态！");
        //                else
        //                    resultList[i].F_OperateState = 9;
        //                DAL.SMC_SendSmsDAL.Instance.Update(resultList[i]);
        //            }
        //        }
        //        else if (keyF_UserId != null & keyF_RootId != null & keyF_SmsContent != null)
        //        {
        //            var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
        //            string fid = result.F_Id;
        //            string sql = "select * from SMC_SendSms where F_CreatorUserId = '" + fid + "' and F_RootId = '" + keyF_RootId + "' and F_SmsContent like '%" + keyF_SmsContent + "%'";
        //            List<SMC_SendSms> resultList = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
        //            int length = resultList.Count;
        //            for (int i = 0; i < length; i++)
        //            {
        //                if (resultList[i].F_OperateState == 9)
        //                    return Success("选择的记录已是“已处理”状态！");
        //                else
        //                    resultList[i].F_OperateState = 9;
        //                DAL.SMC_SendSmsDAL.Instance.Update(resultList[i]);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Error("选择的记录已是“已处理”状态：" + e);
        //    }
        //    return Success("状态已改为处理！");
        //}

        //按条件已处理
        [HttpGet]
        [HandlerAjaxOnly]

        public ActionResult NewManage(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            var keyF_UserId = queryParam["F_UserId"];
            var keyF_RootId = queryParam["F_RootId"];
            var keyF_SmsContent = queryParam["F_SmsContent"];

            if (keyF_UserId != null & keyF_RootId.ToString() == "" & keyF_SmsContent.ToString() == "")
            {
                var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
                string fid = result.F_Id;
                string sql = "select * from SMC_SendSms where F_CreatorUserId = '" + fid + "'";
                updateManage(sql);
            }
            else if (keyF_RootId != null & keyF_UserId.ToString() == "" & keyF_SmsContent.ToString() == "")
            {
                string sql = "select * from SMC_SendSms where F_RootId = '" + keyF_RootId + "'";
                updateManage(sql);
            }
            else if (keyF_SmsContent != null & keyF_UserId.ToString() == "" & keyF_RootId.ToString() == "")
            {
                string sql = "select * from SMC_SendSms where F_SmsContent like '%" + keyF_SmsContent + "%'";
                updateManage(sql);
            }
            else if (keyF_UserId != null & keyF_RootId != null & keyF_SmsContent.ToString() == "")
            {
                var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
                string fid = result.F_Id;
                string sql = "select * from SMC_SendSms where F_RootId = '" + keyF_RootId + "' and F_CreatorUserId = '" + fid + "'";
                updateManage(sql);
            }
            else if (keyF_UserId != null & keyF_RootId.ToString() == "" & keyF_SmsContent != null)
            {
                var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
                string fid = result.F_Id;
                string sql = "select * from SMC_SendSms where F_CreatorUserId = '" + fid + "' and F_SmsContent like '%" + keyF_SmsContent + "%'";
                updateManage(sql);
            }
            else if (keyF_UserId.ToString() == "" & keyF_RootId != null & keyF_SmsContent != null)
            {
                string sql = "select * from SMC_SendSms where F_RootId = '" + keyF_RootId + "' and F_SmsContent like '%" + keyF_SmsContent + "%'";
                updateManage(sql);
            }
            else if (keyF_UserId != null & keyF_RootId != null & keyF_SmsContent != null)
            {
                var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyF_UserId.ToInt());
                string fid = result.F_Id;
                string sql = "select * from SMC_SendSms where F_CreatorUserId = '" + fid + "' and F_RootId = '" + keyF_RootId + "' and F_SmsContent like '%" + keyF_SmsContent + "%'";
                updateManage(sql);
            }
            return Success("状态已改为处理！");
        }
        public ActionResult updateManage(string sql)
        {
            List<SMC_SendSms> resultList = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
            int length = resultList.Count;
            for (int i = 0; i < length; i++)
            {
                if (resultList[i].F_OperateState == 9)
                    return Success("状态已为处理！");
                else
                    resultList[i].F_OperateState = 9;
                DAL.SMC_SendSmsDAL.Instance.Update(resultList[i]);
            }         
            return Success("状态已改为处理！");
        }
    
        //根据F_CreatorUserId获取F_UserId
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetF_UserId(string CreatorUserId)
        {
            var data = BLL.OC_UserInfoManager.Instance.GetModel(CreatorUserId);
            return data.F_UserId.ToString();
        }

        //按条件合并
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult mergeMore(Pagination pagination , string queryJson)
        {
            List<SMC_SendSms> listdata = new List<SMC_SendSms>();
            try
            {
                listdata = DAL.SMC_SendSmsDAL.Instance.GetList(queryJson);
                if (listdata.Count < 2)
                    return Error("没有找到可以合并的数据，请重新输入条件。");
            }
            catch(Exception ex)
            {
                return Error("Ops，出现了错误：" + ex);
            }
            //不知道该如何合并，不清楚算法上如何实现，试着写了一个，通过基础验证
            Dictionary<string, SMC_SendSms> list = new Dictionary<string, SMC_SendSms>();
            for (int i = 0; i < listdata.Count; i++)
            {
                if (list.ContainsKey(listdata[i].F_SmsContent))
                {
                    list[listdata[i].F_SmsContent].F_MobileList += listdata[i].F_MobileList;
                    DAL.SMC_SendSmsDAL.Instance.Delete(t=>t.F_Id==listdata[i].F_Id);//删除原有的项
                }
                else
                {
                    DAL.SMC_SendSmsDAL.Instance.Delete(t => t.F_Id == listdata[i].F_Id);//删除原有的项
                    list.Add(listdata[i].F_SmsContent, listdata[i]);//合并重复信息的项
                }
            }
            foreach (var item in list.Values)
            {
                DAL.SMC_SendSmsDAL.Instance.Add(item);//添加新的项
            }
            return Success("按条件合并成功！");
        }

    }
}





    
