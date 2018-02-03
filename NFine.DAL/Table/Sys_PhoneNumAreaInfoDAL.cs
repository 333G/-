using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;
using NFine.Entity.Models;

namespace NFine.DAL
{
    /// <summary>
    /// 敏感词管理
    /// </summary>
    public partial class Sys_PhoneNumAreaInfoDAL
    {

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<Sys_PhoneNumAreaInfo> GetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<Sys_PhoneNumAreaInfo>();
                var queryParam = queryJson.ToJObject();
                if (!queryParam["F_NumSegment"].IsEmpty())
                {
                    string F_NumSegment = queryParam["F_NumSegment"].ToString();
                    queryable.Where(a => a.F_NumSegment.Contains(F_NumSegment));
                }
                if (!queryParam["F_Province"].IsEmpty())
                {
                    string F_Province = queryParam["F_Province"].ToString();
                    queryable.Where(a => a.F_Province.Contains(F_Province));
                }
                if (!queryParam["F_City"].IsEmpty())
                {
                    string F_City = queryParam["F_City"].ToString();
                    queryable.Where(a => a.F_City.Contains(F_City));
                }
                if (!queryParam["F_Operator"].IsEmpty())
                {
                    string F_Operator = queryParam["F_Operator"].ToString();
                    queryable.Where(a => a.F_Operator.Contains(F_Operator));
                }
                if (!queryParam["F_PostCode"].IsEmpty())
                {
                    string F_PostCode = queryParam["F_PostCode"].ToString();
                    queryable.Where(a => a.F_PostCode.Contains(F_PostCode));
                }
                if (!queryParam["F_AreaCode"].IsEmpty())
                {
                    string F_AreaCode = queryParam["F_AreaCode"].ToString();
                    queryable.Where(a => a.F_AreaCode.Contains(F_AreaCode));
                }

                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }
    }
}
