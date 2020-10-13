using LD.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LDDC.DAL
{
    public class DCExcelData
    {

        public DataTable Exists(string where, string s)
        {
            string str = string.Format(" select left(convert(varchar,a.orderDate,21),7) Mouth,a.eType as Types,a.rUserID as UserID,a.rUser as UserName," +
            " isnull(sum(rAmount),0) as Readay," +
            " sum(case when eDate is NULL then 1 else 0 end) as NoReaday" +
            " from (" +
            " SELECT [guid],[rGuid],[orderDate],[eID],[eType],[rDate],[eDate],[rUserID],[rUser],[isTakeout],[tDate],[tUserID],[tUser],[osGuid],[osState],[rAmount],[creatorID],[creator],[createDate],[isValid]" +
            " FROM [LDDC].[dbo].[OrderInfo] ) a " +
            " where a.rUserID<>'' {0}" +

            " group by left(convert(varchar,a.orderDate,21),7),eType,a.rUserID,a.rUser " +
            " having sum(case when eDate is NULL then 1 else 0 end) <> 0  {1} " +
            " ORDER BY eType,sum(case when eDate is NULL then 1 else 0 end)  desc", where, s);
            DataTable dt = DbHelperSQL.ExecSqlDateTable(str);
            return dt;
        }

        public DataTable ExcelCT(string where)
        {
            string str = string.Format("select c.rMonth ,c.name ,c.eType ,isnull(sum(c.yj),0) as yd , isnull(sum(c.sj),0) as sj " +
                " from" +
                " (select a.name,a.rUser,a.rUserID,a.eType,a.orderDate,a.company,a.rMonth,isnull(a.rAmount,0) yj ,isnull(a.sj,0) sj" +
                " from " +
                " (select r.name,rUserID,rUser,r.company,eType,isnull(rAmount,0) rAmount,case when eDate is not NULL then 1 else 0 end as sj ,orderDate,left(convert(varchar,orderDate,21),7) rMonth,rGuid " +
                " from [LDDC].[dbo].[OrderInfo] o " +
                " left join [LDDC].[dbo].[RestaurantInfo] r on r.guid = o.rGuid" +
                " ) a " +
                " where  a.name<>'' " +
                " {0} " +
                " ) c" +
                " group by c.rMonth,c.name,c.eType,c.company" +
                " order by c.rMonth,c.name,c.eType;",where);
            DataTable dt = DbHelperSQL.ExecSqlDateTable(str);
            return dt;
        }

    }
}
