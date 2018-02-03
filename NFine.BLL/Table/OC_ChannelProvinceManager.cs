using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL.Table
{
   public partial class OC_ChannelProvinceManager
    {
        ///<summary>
        ///条件查询获取列表
        ///</summary>
        /// <param name="F_Id">F_Id</param>
        //public List<OC_ChannelProvince> GetList(string F_Id, string queryJson)
        //{
        //    using (var db = DBHelper.GetReadInstance())
        //    {
        //        var queryable = db.Queryable<OC_ChannelProvince>().Where(t => t.F_Id == F_Id);
        //        var queryParam = queryJson.ToJObject();
        //        if (!queryParam["F_ProvinceName"].IsEmpty())
        //        {
        //            string F_ProvinceName = queryParam["F_ProvinceName"].ToString();
        //            queryable.Where(a => a.F_ProvinceName == F_ProvinceName);
        //        }
        //        if (!queryParam["F_ProvinceId"].IsEmpty())
        //        {
        //            int F_ProvinceId = queryParam["F_ProvinceId"].ToInt();
        //            queryable.Where(a => a.F_ProvinceId == F_ProvinceId);
        //        }
        //        if (!queryParam["F_ChannelId"].IsEmpty())
        //        {
        //            int F_ChannelId = queryParam["F_ChannelId"].ToInt();
        //            queryable.Where(t => t.F_ChannelId == F_ChannelId);
        //        }
        //        var tempData = queryable;
        //        return tempData.ToList();
        //    }
        //}

    }
}
