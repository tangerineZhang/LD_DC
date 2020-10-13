using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using LD.Common;

using BlueThink.Comm;
using DotNet.Utilities;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;

namespace LD.Common
{
    /// <summary>
    /// 数据访问类:LotInfo
    /// </summary>
    public partial class Statistics
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;

        //获取菜品和图片信息
        //public DataSet GetMenuInfoList(string strWhere, string filedOrder)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select a.guid,a.rGuid,a.vtID,a.vtName,a.name,a.describe,a.creatorguid,a.creator,a.createDate,a.isValid, ");
        //    strSql.Append("b.guid,b.vGuid,b.imgPath,b.format,b.imgName,b.size,b.isCoverphoto,b.createDate,b.isValid,b.sortNo  ");
        //    strSql.Append("FROM MenuInfo a  ");
        //    strSql.Append("left join DishesImage b on a.guid = b.vGuid  ");
        //    if (strWhere.Trim() != "")
        //    {
        //        strSql.Append(" where " + strWhere);
        //    }
        //    strSql.Append(" order by " + filedOrder);
        //    return DbHelperSQL.Query(strSql.ToString());
        //}

        //获取一段时间的预定和实际情况
        public DataSet GetThisMonth(string date1, string date2, string rGuid)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("select t1.yj,t2.sj from ( ");
            strSql.Append("select isnull(sum(rAmount),0) yj from [LDDC].[dbo].[OrderInfo] where convert(varchar,orderDate,21) >='" + date1 + "'  and convert(varchar,orderDate,21)  <= '" + date2 + "' and rGuid='" + rGuid + "' ) t1, ");
            strSql.Append("(select isnull(sum(rAmount),0) sj from [LDDC].[dbo].[OrderInfo] where  convert(varchar,orderDate,21) >='" + date1 + "'  and convert(varchar,orderDate,21)  <= '" + date2 + "' and rGuid='" + rGuid + "' and eDate is not NULL  ");
            strSql.Append(") t2 ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        //获取当月预计、实际人数
        public DataSet GetThisMonth(string date, string rGuid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t1.yj,t2.sj from ( ");
            strSql.Append("select isnull(sum(rAmount),0) yj from [LDDC].[dbo].[OrderInfo] where  left(convert(varchar,orderDate,21),7)  = '" + date + "' and rGuid='" + rGuid + "' ) t1, ");
            strSql.Append("(select isnull(sum(rAmount),0) sj from [LDDC].[dbo].[OrderInfo] where  left(convert(varchar,orderDate,21),7)  = '" + date + "' and rGuid='" + rGuid + "' and eDate is not NULL ");
            strSql.Append(") t2 ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        //获取当天午餐预定、实际情况
        public DataSet GetTodayLunch(string date, string rGuid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t1.yj,t2.sj from ( ");
            strSql.Append("select isnull(sum(rAmount),0) yj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and eID = 2 ) t1, ");
            strSql.Append("(select isnull(sum(rAmount),0) sj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and eID = 2 and eDate is not NULL ");
            strSql.Append(") t2 ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        //获取当天晚餐预定、实际情况
        public DataSet GetTodayDinner(string date, string rGuid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t1.yj,t2.sj from ( ");
            strSql.Append("select isnull(sum(rAmount),0) yj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and eID = 3 ) t1, ");
            strSql.Append("(select isnull(sum(rAmount),0) sj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and eID = 3 and eDate is not NULL ");
            strSql.Append(") t2 ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        //获取当天晚餐预定、实际情况
        public DataSet GetDateMeetingLunch(string date, string rGuid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t1.yj,t2.sj from ( ");
            strSql.Append("select isnull(sum(rAmount),0) yj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and eID = 4 ) t1, ");
            strSql.Append("(select isnull(sum(rAmount),0) sj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and eID = 4 and eDate is not NULL ");
            strSql.Append(") t2 ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        //获取当月预计、实际人数
        public DataSet GetThisMonthByUser(string date, string rGuid, string userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t1.yj,t2.sj from ( ");
            strSql.Append("select isnull(sum(rAmount),0) yj from [LDDC].[dbo].[OrderInfo] where  left(convert(varchar,orderDate,21),7)  = '" + date + "' and rGuid='" + rGuid + "' and rUserID='" + userID + "' ) t1, ");
            strSql.Append("(select isnull(sum(rAmount),0) sj from [LDDC].[dbo].[OrderInfo] where  left(convert(varchar,orderDate,21),7)  = '" + date + "' and rGuid='" + rGuid + "' and rUserID='" + userID + "' and eDate is not NULL ");
            strSql.Append(") t2 ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        //获取当天午餐预定、实际情况
        public DataSet GetTodayLunchByUser(string date, string rGuid, string userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t1.yj,t2.sj from ( ");
            strSql.Append("select isnull(sum(rAmount),0) yj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and rUserID='" + userID + "' and eID = 2 ) t1, ");
            strSql.Append("(select isnull(sum(rAmount),0) sj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and rUserID='" + userID + "' and eID = 2 and eDate is not NULL ");
            strSql.Append(") t2 ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        //获取当天午餐预定、实际情况
        public DataSet GetMeetingLunchByUser(string date, string rGuid, string userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t1.yj,t2.sj from ( ");
            strSql.Append("select isnull(sum(rAmount),0) yj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and rUserID='" + userID + "' and eID = 4 ) t1, ");
            strSql.Append("(select isnull(sum(rAmount),0) sj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and rUserID='" + userID + "' and eID = 4 and eDate is not NULL ");
            strSql.Append(") t2 ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        //获取当天晚餐预定、实际情况
        public DataSet GetTodayDinnerByUser(string date, string rGuid, string userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select t1.yj,t2.sj from ( ");
            strSql.Append("select isnull(sum(rAmount),0) yj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and rUserID='" + userID + "' and eID = 3 ) t1, ");
            strSql.Append("(select isnull(sum(rAmount),0) sj from [LDDC].[dbo].[OrderInfo] where orderDate = '" + date + "' and rGuid='" + rGuid + "' and rUserID='" + userID + "' and eID = 3 and eDate is not NULL ");
            strSql.Append(") t2 ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetOrderInfo(string orderDate, string rGuid, string eID, int osGuid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [guid],[rGuid],[orderDate],[eID],[eType],[rDate],[eDate],[rUserID],[rUser],[isTakeout],[tDate],[tUserID],[tUser],[osGuid],[osState],[rAmount],[creatorID],[creator],[createDate],[isValid]");
            strSql.Append("FROM [LDDC].[dbo].[OrderInfo] ");
            strSql.Append("WHERE [orderDate] between '" + orderDate + "' and '" + orderDate + "' and rGuid = '" + rGuid + "' and eID in (" + eID + ") and osGuid = " + osGuid +" ");
            strSql.Append("order by eID ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetTopInfo(string begDate, string endDate, string rGuid, string eID, int osGuid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [eType],[rUser],count([guid]) 次数 ");
            strSql.Append("FROM [LDDC].[dbo].[OrderInfo] ");
            strSql.Append("WHERE [orderDate] between '" + begDate + "' and '" + endDate + "' and rGuid = '" + rGuid + "' and eID in (" + eID + ") and osGuid = 2 ");
            strSql.Append("GROUP BY [eType],[rUser]");
            strSql.Append("ORDER BY count([guid]) desc");
            return DbHelperSQL.Query(strSql.ToString());
        }
    }


}




