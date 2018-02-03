using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Entity;
using NFine.Code;
using SqlSugar;

namespace NFine.BLL
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class Sys_UserManager
    {
        #region 单例模式
        private static Sys_UserManager instance;
        private static object _lock = new object();

        private Sys_UserManager()
        {
        }

        public static Sys_UserManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Sys_UserManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        /// <summary>
        /// 获取指定用户，待修改，目前显示所有User表中用户
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetList()
        {
             return DAL.Sys_UserDAL.Instance.GetList();  
        }
        public List<Sys_User> GetDepthList(string id)
        {
            return DAL.Sys_UserDAL.Instance.GetDepthList(id);
        }
        public List<Sys_User> GetList(string keyword)
        {
            return DAL.Sys_UserDAL.Instance.GetList(keyword);
        }
        public List<Sys_User> GetListByid(string id)
        {
            return DAL.Sys_UserDAL.Instance.GetListByid(id);
        }
        public List<Sys_User> GetList(Pagination pagination, string keyword)
        {
            return DAL.Sys_UserDAL.Instance.GetList(pagination, keyword);
        }

        public List<Sys_User> GetList(Pagination pagination, string userId, string keyword)
        {
            return DAL.Sys_UserDAL.Instance.GetList(pagination, userId, keyword);
        }

        /// <summary>
        /// 主键
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_User GetModel(int id)
        {
            return DAL.Sys_UserDAL.Instance.FindEntity(a=>a.Id== id);
        }

        //根据F_Id查询单个实体
        public Sys_User GetModel(string creatoruserId)
        {
            return DAL.Sys_UserDAL.Instance.FindEntity(a => a.F_Id == creatoruserId);
        }


        public int Add(Sys_User model)
        {
            object obj = DAL.Sys_UserDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }
    }
}
