using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;

namespace NFine.DAL
{
    /// <summary>
    /// 黑白名单管理
    /// </summary>
    public partial class TXL_PhoneInfoDAL
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<TXL_PhoneInfo> GetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<TXL_PhoneInfo>().Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true);
                var queryParam = queryJson.ToJObject();
                /*
                 * if (!queryParam["F_Mobile"].IsEmpty())
                {
                    string F_Mobile = queryParam["F_Mobile"].ToString();
                    queryable.Where(a => a.F_Mobile == F_Mobile);
                }
                if (!queryParam["F_Sign"].IsEmpty())
                {
                    bool? F_Sign = queryParam["F_Sign"].ToBool();
                    queryable.Where(t => t.F_Sign == F_Sign);
                }
                if (!queryParam["F_Level"].IsEmpty())
                {
                    int F_Level = queryParam["F_Level"].ToInt();
                    queryable.Where(t => t.F_Level == F_Level);
                }
                */
                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }
        public List<TXL_PhoneInfo> GetList(string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<TXL_PhoneInfo>();
                var queryParam = queryJson.ToJObject();
                if (!queryParam["GroupId"].IsEmpty())
                {
                    string GroupId = queryParam["GroupId"].ToString();
                    queryable.Where(a => a.GroupId == GroupId);
                }
                if (!queryParam["Province"].IsEmpty())
                {
                    string Province = queryParam["Province"].ToString();
                    queryable.Where(a => a.Province == Province);
                }
                if (!queryParam["Sex"].IsEmpty())
                {
                    string Sex = queryParam["Sex"].ToString();
                    queryable.Where(a => a.Sex == Sex);
                }
                if (!queryParam["Operator"].IsEmpty())
                {
                    string Operator = queryParam["Operator"].ToString();
                    queryable.Where(a => a.Operator == Operator);
                }
                if (!queryParam["State"].IsEmpty())
                {
                    int State = queryParam["State"].ToInt();
                    queryable.Where(a => a.State == State);
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    string keyword = queryParam["keyword"].ToString();
                    queryable.Where(a => a.Name.Contains(keyword) || a.Mobile.Contains(keyword));
                }
                  
               
                return queryable.ToList();
            }
        }
        /// <summary>
        /// 查询所有可用的通讯录人员数组
        /// </summary>
        /// <param name="F_Id">ID</param>
        /// <param name="Mobile">手机号</param>
        /// <returns></returns>
        public List<TXL_PhoneInfo> GetList(string F_Id, string Mobile)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                //var queryable = db.Queryable<OC_BlackList>().Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true);
                var queryable = db.Queryable<TXL_PhoneInfo>();//.Where(t => t.F_Id == "2a11613f - 3cad - 4b53 - a644 - 7088e954f70c");
                                                              //var queryable = db.Queryable<TXL_PhoneInfo>().Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true);

                if (!F_Id.IsEmpty())
                {
                    queryable.Where(a => a.F_Id == F_Id);
                }
                //if (!Mobile.IsEmpty())????????????????????测试用，待还原
                if (Mobile != "undefined")
                {
                    queryable.Where(a => a.Mobile == Mobile);
                }
                return queryable.ToList();
            }
        }
    }
}

