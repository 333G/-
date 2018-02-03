/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/11/9 20:32:13
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFine.Entity;
using NFine.Code;
using System.Data.SqlClient;

namespace NFine.BLL.SMS
{
    public class SMS_TestBLL
    {
        #region 单例模式
        private static SMS_TestBLL instance;
        private static object _lock = new object();

        private SMS_TestBLL()
        {
        }

        public static SMS_TestBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMS_TestBLL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(SMS_Test model)
        {
            //var currentUserModel = NFine.Code.OperatorProvider.Provider.GetCurrent();

            model.CreateTime = DateTime.Now;
            //model.CreateUserId = currentUserModel.UserId;
            //model.Creator = currentUserModel.UserId;
            model.Creator = "测试";
            return DAL.SMS_TestDAL.Instance.Add(model).ToInt();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(SMS_Test model)
        {
            //var currentUserModel = NFine.Code.OperatorProvider.Provider.GetCurrent();

            model.LastEditTime = DateTime.Now;
            //model.LastEditor = currentUserModel.UserId;
            model.LastEditor = "测试";
            return DAL.SMS_TestDAL.Instance.Update(model);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return DAL.SMS_TestDAL.Instance.Delete(a => a.ID == id);
        }
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public SMS_Test FindEntity(int id)
        {
            return DAL.SMS_TestDAL.Instance.FindEntity(a => a.ID == id);
        }
        /// <summary>
        /// 手写sql根据查询条件
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        public List<SMS_Test> FindList(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM dbo.SMS_Test WHERE TestName=@TestName");
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@TestName", name));

            return DAL.SMS_TestDAL.Instance.FindList(strSql.ToString(),parameters);
        }
        /// <summary>
        /// 使用lamada表达式查询
        /// </summary>
        /// <param name="name">测试名称</param>
        /// <param name="creator">测试使用人</param>
        /// <returns></returns>
        public List<SMS_Test> FindList(string name,string creator)
        {
            return DAL.SMS_TestDAL.Instance.FindList(a => a.TestName == name&&a.Creator==creator);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagination">分页数据</param>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        public List<SMS_Test> FindPageList(Pagination pagination,string name)
        {
            return DAL.SMS_TestDAL.Instance.FindPageList(a=>a.TestName.Contains(name),pagination);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagination">分页数据</param>
        /// <returns></returns>
        public List<SMS_Test> FindPageList(Pagination pagination)
        {
            return DAL.SMS_TestDAL.Instance.FindPageList(pagination);
        }
    }
}
