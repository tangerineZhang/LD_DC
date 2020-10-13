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
    public partial class PublicDao
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;


        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="list"></param>
        /// <param name="rCount"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public bool TransactionListSql(List<string> list, ref int rCount, ref string errorInfo)
        {
            string sql = String.Empty;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sql = list[i].ToString();
                    comm.CommandText = sql;
                    comm.ExecuteNonQuery();
                    rCount = i;
                }

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = sql;
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }

        }


        public bool TransactionOrder(LDDC.Model.OrderInfo mOrderInfo, LDDC.Model.OrderState mOrderState, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                strSql.Append("insert into OrderInfo(");
                strSql.Append("guid,rGuid,orderDate,eID,eType,rDate,rUserID,rUser,isTakeout,osGuid,osState,rAmount,creatorID,creator,createDate,isValid)");
                strSql.Append(" values (");
                strSql.Append("@guid,@rGuid,@orderDate,@eID,@eType,@rDate,@rUserID,@rUser,@isTakeout,@osGuid,@osState,@rAmount,@creatorID,@creator,@createDate,@isValid)");
                SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@rGuid", SqlDbType.VarChar,60),
                    new SqlParameter("@orderDate", SqlDbType.DateTime),
					new SqlParameter("@eID", SqlDbType.Int,4),
					new SqlParameter("@eType", SqlDbType.VarChar,30),
					new SqlParameter("@rDate", SqlDbType.DateTime),
					new SqlParameter("@rUserID", SqlDbType.VarChar,60),
					new SqlParameter("@rUser", SqlDbType.VarChar,10),
					new SqlParameter("@isTakeout", SqlDbType.Int,4),
					new SqlParameter("@osGuid", SqlDbType.Int,4),
					new SqlParameter("@osState", SqlDbType.VarChar,30),
                    new SqlParameter("@rAmount", SqlDbType.VarChar,30),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4)};
                parameters[0].Value = mOrderInfo.guid;
                parameters[1].Value = mOrderInfo.rGuid;
                parameters[2].Value = mOrderInfo.orderDate;
                parameters[3].Value = mOrderInfo.eID;
                parameters[4].Value = mOrderInfo.eType;
                parameters[5].Value = mOrderInfo.rDate;
                parameters[6].Value = mOrderInfo.rUserID;
                parameters[7].Value = mOrderInfo.rUser;
                parameters[8].Value = mOrderInfo.isTakeout;
                parameters[9].Value = mOrderInfo.osGuid;
                parameters[10].Value = mOrderInfo.osState;
                parameters[11].Value = mOrderInfo.rAmount;
                parameters[12].Value = mOrderInfo.creatorID;
                parameters[13].Value = mOrderInfo.creator;
                parameters[14].Value = mOrderInfo.createDate;
                parameters[15].Value = mOrderInfo.isValid;

                //rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                comm.CommandText = strSql.ToString();
                comm.Parameters.Clear();
                comm.Parameters.AddRange(parameters);
                rCount += comm.ExecuteNonQuery();
                //comm.CommandText = strSql.ToString();
                //comm.ExecuteNonQuery();
                //rCount += 1;

                strSql = new StringBuilder();
                strSql.Append("insert into OrderState(");
                strSql.Append("guid,oGuid,sID,state,creatorID,creator,createDate,isValid)");
                strSql.Append(" values (");
                strSql.Append("@guid,@oGuid,@sID,@state,@creatorID,@creator,@createDate,@isValid)");
                SqlParameter[] parametersOS = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@oGuid", SqlDbType.VarChar,60),
					new SqlParameter("@sID", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.VarChar,30),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4)};
                parametersOS[0].Value = Guid.NewGuid();
                parametersOS[1].Value = mOrderState.oGuid;
                parametersOS[2].Value = mOrderState.sID;
                parametersOS[3].Value = mOrderState.state;
                parametersOS[4].Value = mOrderState.creatorID;
                parametersOS[5].Value = mOrderState.creator;
                parametersOS[6].Value = mOrderState.createDate;
                parametersOS[7].Value = mOrderState.isValid;

                //rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parametersOS);
                comm.CommandText = strSql.ToString();
                comm.Parameters.Clear();
                comm.Parameters.AddRange(parametersOS);
                rCount += comm.ExecuteNonQuery();

                ///strSql = new StringBuilder();
                //strSql.Append("update dbo.LAW_file set isuse = 1 where infoid = '" + pLAW_info.lawid.ToString() + "'");
                //comm.CommandText = strSql.ToString();
                //comm.ExecuteNonQuery();
                //rCount += 1;





                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }

        }

        public bool updateMeetingOrder(string guid, int amount, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                strSql = new StringBuilder();
                strSql.Append("update [LDDC].[dbo].[OrderInfo] set rAmount = " + amount);
                strSql.Append(" where guid = '" + guid + "'");

                comm.CommandText = strSql.ToString();
                comm.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }

        }


        public bool TransactionCancelOrder(string guid, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                strSql.Append("delete from OrderInfo ");
                strSql.Append(" where guid = '" + guid + "'  ");
                //rCount += DbHelperSQL.ExecuteSql(strSql.ToString());
                comm.CommandText = strSql.ToString();
                rCount += comm.ExecuteNonQuery();

                strSql = new StringBuilder();
                strSql.Append("delete from OrderState ");
                strSql.Append(" where oGuid = '" + guid + "'  ");
                //rCount += DbHelperSQL.ExecuteSql(strSql.ToString());
                comm.CommandText = strSql.ToString();
                rCount += comm.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }

        }

        public bool TransactionScanQRCode(LDDC.Model.OrderInfo mOrderInfo, LDDC.Model.OrderState mOrderState, DataTable dsoito, string etype, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                strSql.Append("update OrderInfo set ");
                strSql.Append("eDate=@eDate,");
                strSql.Append("osGuid=@osGuid,");
                strSql.Append("osState=@osState ");
                strSql.Append(" where guid=@guid ");
                SqlParameter[] parameters = {
					new SqlParameter("@eDate", SqlDbType.DateTime),
					new SqlParameter("@osGuid", SqlDbType.Int,4),
					new SqlParameter("@osState", SqlDbType.VarChar,10),
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)};
                parameters[0].Value = mOrderInfo.eDate;
                parameters[1].Value = mOrderInfo.osGuid;
                parameters[2].Value = mOrderInfo.osState;
                parameters[3].Value = mOrderInfo.guid;

                //rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                comm.CommandText = strSql.ToString();
                comm.Parameters.Clear();
                comm.Parameters.AddRange(parameters);
                rCount += comm.ExecuteNonQuery();


                strSql = new StringBuilder();
                strSql.Append("insert into OrderState(");
                strSql.Append("guid,oGuid,sID,state,creatorID,creator,createDate,isValid)");
                strSql.Append(" values (");
                strSql.Append("@guid,@oGuid,@sID,@state,@creatorID,@creator,@createDate,@isValid)");
                SqlParameter[] parametersOS = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@oGuid", SqlDbType.VarChar,60),
					new SqlParameter("@sID", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.VarChar,10),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4)};
                parametersOS[0].Value = Guid.NewGuid();
                parametersOS[1].Value = mOrderState.oGuid;
                parametersOS[2].Value = mOrderState.sID;
                parametersOS[3].Value = mOrderState.state;
                parametersOS[4].Value = mOrderState.creatorID;
                parametersOS[5].Value = mOrderState.creator;
                parametersOS[6].Value = mOrderState.createDate;
                parametersOS[7].Value = mOrderState.isValid;

                //rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parametersOS);
                comm.CommandText = strSql.ToString();
                comm.Parameters.Clear();
                comm.Parameters.AddRange(parametersOS);
                rCount += comm.ExecuteNonQuery();


                if (dsoito.Rows.Count > 0)
                {
                    DataRow[] dr = dsoito.Select("eType = '" + etype + "' ");
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            strSql = new StringBuilder();
                            strSql.Append("update OrderInfo set ");
                            strSql.Append("eDate=@eDate,");
                            strSql.Append("osGuid=@osGuid,");
                            strSql.Append("osState=@osState ");
                            strSql.Append(" where guid=@guid ");
                            SqlParameter[] parametersTO = {
					            new SqlParameter("@eDate", SqlDbType.DateTime),
					            new SqlParameter("@osGuid", SqlDbType.Int,4),
					            new SqlParameter("@osState", SqlDbType.VarChar,10),
					            new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)};
                            parametersTO[0].Value = DateTime.Now;
                            parametersTO[1].Value = 1;
                            parametersTO[2].Value = "已就餐";
                            parametersTO[3].Value = dr[i]["guid"];

                            //rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parametersTO);
                            comm.CommandText = strSql.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.AddRange(parametersTO);
                            rCount += comm.ExecuteNonQuery();


                            strSql = new StringBuilder();
                            strSql.Append("insert into OrderState(");
                            strSql.Append("guid,oGuid,sID,state,creatorID,creator,createDate,isValid)");
                            strSql.Append(" values (");
                            strSql.Append("@guid,@oGuid,@sID,@state,@creatorID,@creator,@createDate,@isValid)");
                            SqlParameter[] parametersOSTO = {
					            new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					            new SqlParameter("@oGuid", SqlDbType.VarChar,60),
					            new SqlParameter("@sID", SqlDbType.Int,4),
					            new SqlParameter("@state", SqlDbType.VarChar,10),
					            new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					            new SqlParameter("@creator", SqlDbType.VarChar,10),
					            new SqlParameter("@createDate", SqlDbType.DateTime),
					            new SqlParameter("@isValid", SqlDbType.Int,4)};
                            parametersOSTO[0].Value = Guid.NewGuid();
                            parametersOSTO[1].Value = dr[i]["guid"].ToString();
                            parametersOSTO[2].Value = 1;
                            parametersOSTO[3].Value = "已就餐";
                            parametersOSTO[4].Value = mOrderState.creatorID;
                            parametersOSTO[5].Value = mOrderState.creator;
                            parametersOSTO[6].Value = mOrderState.createDate;
                            parametersOSTO[7].Value = mOrderState.isValid;

                            comm.CommandText = strSql.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.AddRange(parametersOSTO);
                            rCount += comm.ExecuteNonQuery();
                        }
                    }
                }

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }

        public bool TransactionScanQRCodeTO(LDDC.Model.OrderState mOrderState, DataTable dsoito, string etype, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
               


                if (dsoito.Rows.Count > 0)
                {
                    DataRow[] dr = dsoito.Select("eType = '" + etype + "' ");
                    if (dr.Length > 0)
                    {
                        for (int i = 0; i < dr.Length; i++)
                        {
                            strSql = new StringBuilder();
                            strSql.Append("update OrderInfo set ");
                            strSql.Append("eDate=@eDate,");
                            strSql.Append("osGuid=@osGuid,");
                            strSql.Append("osState=@osState ");
                            strSql.Append(" where guid=@guid ");
                            SqlParameter[] parametersTO = {
                 new SqlParameter("@eDate", SqlDbType.DateTime),
                 new SqlParameter("@osGuid", SqlDbType.Int,4),
                 new SqlParameter("@osState", SqlDbType.VarChar,10),
                 new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)};
                            parametersTO[0].Value = DateTime.Now;
                            parametersTO[1].Value = 1;
                            parametersTO[2].Value = "已就餐";
                            parametersTO[3].Value = dr[i]["guid"];

                            //rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parametersTO);
                            comm.CommandText = strSql.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.AddRange(parametersTO);
                            rCount += comm.ExecuteNonQuery();


                            strSql = new StringBuilder();
                            strSql.Append("insert into OrderState(");
                            strSql.Append("guid,oGuid,sID,state,creatorID,creator,createDate,isValid)");
                            strSql.Append(" values (");
                            strSql.Append("@guid,@oGuid,@sID,@state,@creatorID,@creator,@createDate,@isValid)");
                            SqlParameter[] parametersOSTO = {
                 new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
                 new SqlParameter("@oGuid", SqlDbType.VarChar,60),
                 new SqlParameter("@sID", SqlDbType.Int,4),
                 new SqlParameter("@state", SqlDbType.VarChar,10),
                 new SqlParameter("@creatorID", SqlDbType.VarChar,30),
                 new SqlParameter("@creator", SqlDbType.VarChar,10),
                 new SqlParameter("@createDate", SqlDbType.DateTime),
                 new SqlParameter("@isValid", SqlDbType.Int,4)};
                            parametersOSTO[0].Value = Guid.NewGuid();
                            parametersOSTO[1].Value = dr[i]["guid"].ToString();
                            parametersOSTO[2].Value = 1;
                            parametersOSTO[3].Value = "已就餐";
                            parametersOSTO[4].Value = mOrderState.creatorID;
                            parametersOSTO[5].Value = mOrderState.creator;
                            parametersOSTO[6].Value = mOrderState.createDate;
                            parametersOSTO[7].Value = mOrderState.isValid;

                            comm.CommandText = strSql.ToString();
                            comm.Parameters.Clear();
                            comm.Parameters.AddRange(parametersOSTO);
                            rCount += comm.ExecuteNonQuery();
                        }
                    }
                }

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }


        public bool MealNotReserved(LDDC.Model.OrderInfo mOrderInfo, LDDC.Model.OrderState mOrderState, DataTable dsoito, string etype, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                if (dsoito.Rows.Count == 0)
                {
                    strSql.Append("insert into OrderInfo(");
                    strSql.Append("guid,rGuid,orderDate,eID,eType,rDate,rUserID,rUser,isTakeout,osGuid,osState,rAmount,creatorID,creator,createDate,isValid,eDate)");
                    strSql.Append(" values (");
                    strSql.Append("@guid,@rGuid,@orderDate,@eID,@eType,@rDate,@rUserID,@rUser,@isTakeout,@osGuid,@osState,@rAmount,@creatorID,@creator,@createDate,@isValid,@eDate)");
                    SqlParameter[] parameters = {
                    new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@rGuid", SqlDbType.VarChar,60),
                    new SqlParameter("@orderDate", SqlDbType.DateTime),
                    new SqlParameter("@eID", SqlDbType.Int,4),
                    new SqlParameter("@eType", SqlDbType.VarChar,30),
                    new SqlParameter("@rDate", SqlDbType.DateTime),
                    new SqlParameter("@rUserID", SqlDbType.VarChar,60),
                    new SqlParameter("@rUser", SqlDbType.VarChar,10),
                    new SqlParameter("@isTakeout", SqlDbType.Int,4),
                    new SqlParameter("@osGuid", SqlDbType.Int,4),
                    new SqlParameter("@osState", SqlDbType.VarChar,30),
                    new SqlParameter("@rAmount", SqlDbType.VarChar,30),
                    new SqlParameter("@creatorID", SqlDbType.VarChar,30),
                    new SqlParameter("@creator", SqlDbType.VarChar,10),
                    new SqlParameter("@createDate", SqlDbType.DateTime),
                    new SqlParameter("@isValid", SqlDbType.Int,4),
                    new SqlParameter("@eDate", SqlDbType.DateTime)};
                    parameters[0].Value = mOrderInfo.guid;
                    parameters[1].Value = mOrderInfo.rGuid;
                    parameters[2].Value = mOrderInfo.orderDate;
                    parameters[3].Value = mOrderInfo.eID;
                    parameters[4].Value = mOrderInfo.eType;
                    parameters[5].Value = mOrderInfo.rDate;
                    parameters[6].Value = mOrderInfo.rUserID;
                    parameters[7].Value = mOrderInfo.rUser;
                    parameters[8].Value = mOrderInfo.isTakeout;
                    parameters[9].Value = mOrderInfo.osGuid;
                    parameters[10].Value = mOrderInfo.osState;
                    parameters[11].Value = mOrderInfo.rAmount;
                    parameters[12].Value = mOrderInfo.creatorID;
                    parameters[13].Value = mOrderInfo.creator;
                    parameters[14].Value = mOrderInfo.createDate;
                    parameters[15].Value = mOrderInfo.isValid;
                    parameters[16].Value = mOrderInfo.eDate;

                    //rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                    comm.CommandText = strSql.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.AddRange(parameters);
                    rCount += comm.ExecuteNonQuery();

                    //插入用餐日志
                    strSql = new StringBuilder();
                    strSql.Append("insert into OrderState(");
                    strSql.Append("guid,oGuid,sID,state,creatorID,creator,createDate,isValid)");
                    strSql.Append(" values (");
                    strSql.Append("@guid,@oGuid,@sID,@state,@creatorID,@creator,@createDate,@isValid)");
                    SqlParameter[] parametersOSTO = {
                     new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
                     new SqlParameter("@oGuid", SqlDbType.VarChar,60),
                     new SqlParameter("@sID", SqlDbType.Int,4),
                     new SqlParameter("@state", SqlDbType.VarChar,10),
                     new SqlParameter("@creatorID", SqlDbType.VarChar,30),
                     new SqlParameter("@creator", SqlDbType.VarChar,10),
                     new SqlParameter("@createDate", SqlDbType.DateTime),
                     new SqlParameter("@isValid", SqlDbType.Int,4)};
                    parametersOSTO[0].Value = Guid.NewGuid();
                    parametersOSTO[1].Value = mOrderState.oGuid;
                    parametersOSTO[2].Value = mOrderState.sID;
                    parametersOSTO[3].Value = mOrderState.state;
                    parametersOSTO[4].Value = mOrderState.creatorID;
                    parametersOSTO[5].Value = mOrderState.creator;
                    parametersOSTO[6].Value = mOrderState.createDate;
                    parametersOSTO[7].Value = mOrderState.isValid;

                    comm.CommandText = strSql.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.AddRange(parametersOSTO);
                    rCount += comm.ExecuteNonQuery();
                }


                //if (dsoito.Rows.Count > 0)
                //{
                //    DataRow[] dr = dsoito.Select("eType = '" + etype + "' ");
                //    if (dr.Length > 0)
                //    {
                //        for (int i = 0; i < dr.Length; i++)
                //        {
                //            strSql = new StringBuilder();
                //            strSql.Append("update OrderInfo set ");
                //            strSql.Append("eDate=@eDate,");
                //            strSql.Append("osGuid=@osGuid,");
                //            strSql.Append("osState=@osState ");
                //            strSql.Append(" where guid=@guid ");
                //            SqlParameter[] parametersTO = {
                // new SqlParameter("@eDate", SqlDbType.DateTime),
                // new SqlParameter("@osGuid", SqlDbType.Int,4),
                // new SqlParameter("@osState", SqlDbType.VarChar,10),
                // new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)};
                //            parametersTO[0].Value = DateTime.Now;
                //            parametersTO[1].Value = 1;
                //            parametersTO[2].Value = "已就餐";
                //            parametersTO[3].Value = dr[i]["guid"];

                //            //rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parametersTO);
                //            comm.CommandText = strSql.ToString();
                //            comm.Parameters.Clear();
                //            comm.Parameters.AddRange(parametersTO);
                //            rCount += comm.ExecuteNonQuery();


                //            strSql = new StringBuilder();
                //            strSql.Append("insert into OrderState(");
                //            strSql.Append("guid,oGuid,sID,state,creatorID,creator,createDate,isValid)");
                //            strSql.Append(" values (");
                //            strSql.Append("@guid,@oGuid,@sID,@state,@creatorID,@creator,@createDate,@isValid)");
                //            SqlParameter[] parametersOSTO = {
                // new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
                // new SqlParameter("@oGuid", SqlDbType.VarChar,60),
                // new SqlParameter("@sID", SqlDbType.Int,4),
                // new SqlParameter("@state", SqlDbType.VarChar,10),
                // new SqlParameter("@creatorID", SqlDbType.VarChar,30),
                // new SqlParameter("@creator", SqlDbType.VarChar,10),
                // new SqlParameter("@createDate", SqlDbType.DateTime),
                // new SqlParameter("@isValid", SqlDbType.Int,4)};
                //            parametersOSTO[0].Value = Guid.NewGuid();
                //            parametersOSTO[1].Value = dr[i]["guid"].ToString();
                //            parametersOSTO[2].Value = 1;
                //            parametersOSTO[3].Value = "已就餐";
                //            parametersOSTO[4].Value = mOrderState.creatorID;
                //            parametersOSTO[5].Value = mOrderState.creator;
                //            parametersOSTO[6].Value = mOrderState.createDate;
                //            parametersOSTO[7].Value = mOrderState.isValid;

                //            comm.CommandText = strSql.ToString();
                //            comm.Parameters.Clear();
                //            comm.Parameters.AddRange(parametersOSTO);
                //            rCount += comm.ExecuteNonQuery();
                //        }
                //    }
                //}

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }

        public bool TransactionTakeOut(LDDC.Model.OrderInfo mOrderInfo, LDDC.Model.OrderState mOrderState, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                strSql.Append("update OrderInfo set ");
                strSql.Append("isTakeout=@isTakeout,");
                strSql.Append("tDate=@tDate,");
                strSql.Append("tUserID=@tUserID,");
                strSql.Append("tUser=@tUser,");
                strSql.Append("osGuid=@osGuid,");
                strSql.Append("osState=@osState ");
                strSql.Append(" where guid=@guid ");
                SqlParameter[] parameters = {
					new SqlParameter("@isTakeout", SqlDbType.Int,4),
					new SqlParameter("@tDate", SqlDbType.DateTime),
					new SqlParameter("@tUserID", SqlDbType.VarChar,30),
					new SqlParameter("@tUser", SqlDbType.VarChar,10),
					new SqlParameter("@osGuid", SqlDbType.Int,4),
					new SqlParameter("@osState", SqlDbType.VarChar,10),
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)};
                parameters[0].Value = mOrderInfo.isTakeout;
                parameters[1].Value = mOrderInfo.tDate;
                parameters[2].Value = mOrderInfo.tUserID;
                parameters[3].Value = mOrderInfo.tUser;
                parameters[4].Value = mOrderInfo.osGuid;
                parameters[5].Value = mOrderInfo.osState;
                parameters[6].Value = mOrderInfo.guid;

                comm.CommandText = strSql.ToString();
                comm.Parameters.Clear();
                comm.Parameters.AddRange(parameters);
                rCount += comm.ExecuteNonQuery();


                strSql = new StringBuilder();
                strSql.Append("insert into OrderState(");
                strSql.Append("guid,oGuid,sID,state,creatorID,creator,createDate,isValid)");
                strSql.Append(" values (");
                strSql.Append("@guid,@oGuid,@sID,@state,@creatorID,@creator,@createDate,@isValid)");
                SqlParameter[] parametersOS = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@oGuid", SqlDbType.VarChar,60),
					new SqlParameter("@sID", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.VarChar,10),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4)};
                parametersOS[0].Value = Guid.NewGuid();
                parametersOS[1].Value = mOrderState.oGuid;
                parametersOS[2].Value = mOrderState.sID;
                parametersOS[3].Value = mOrderState.state;
                parametersOS[4].Value = mOrderState.creatorID;
                parametersOS[5].Value = mOrderState.creator;
                parametersOS[6].Value = mOrderState.createDate;
                parametersOS[7].Value = mOrderState.isValid;


                comm.CommandText = strSql.ToString();
                comm.Parameters.Clear();
                comm.Parameters.AddRange(parametersOS);
                rCount += comm.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }

        public bool TransactionAddFood(LDDC.Model.MenuInfo mMenuInfo, LDDC.Model.DishesImage mDishesImage, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                strSql.Append("insert into MenuInfo(");
                strSql.Append("guid,rGuid,vtID,vtName,name,describe,creatorguid,creator,createDate,isValid)");
                strSql.Append(" values (");
                strSql.Append("@guid,@rGuid,@vtID,@vtName,@name,@describe,@creatorguid,@creator,@createDate,@isValid)");
                SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@rGuid", SqlDbType.VarChar,60),
					new SqlParameter("@vtID", SqlDbType.Int,4),
					new SqlParameter("@vtName", SqlDbType.VarChar,30),
					new SqlParameter("@name", SqlDbType.VarChar,60),
					new SqlParameter("@describe", SqlDbType.VarChar,500),
					new SqlParameter("@creatorguid", SqlDbType.VarChar,60),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4)};
                parameters[0].Value = mMenuInfo.guid;
                parameters[1].Value = mMenuInfo.rGuid;
                parameters[2].Value = mMenuInfo.vtID;
                parameters[3].Value = mMenuInfo.vtName;
                parameters[4].Value = mMenuInfo.name;
                parameters[5].Value = mMenuInfo.describe;
                parameters[6].Value = mMenuInfo.creatorguid;
                parameters[7].Value = mMenuInfo.creator;
                parameters[8].Value = mMenuInfo.createDate;
                parameters[9].Value = mMenuInfo.isValid;

                comm.CommandText = strSql.ToString();
                comm.Parameters.Clear();
                comm.Parameters.AddRange(parameters);
                rCount += comm.ExecuteNonQuery();


                strSql = new StringBuilder();
                strSql.Append("insert into DishesImage(");
                strSql.Append("guid,vGuid,imgPath,format,imgName,size,isCoverphoto,createDate,isValid)");
                strSql.Append(" values (");
                strSql.Append("@guid,@vGuid,@imgPath,@format,@imgName,@size,@isCoverphoto,@createDate,@isValid)");
                SqlParameter[] parametersDI = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@vGuid", SqlDbType.VarChar,60),
					new SqlParameter("@imgPath", SqlDbType.VarChar,200),
					new SqlParameter("@format", SqlDbType.VarChar,12),
					new SqlParameter("@imgName", SqlDbType.VarChar,100),
					new SqlParameter("@size", SqlDbType.Decimal,9),
					new SqlParameter("@isCoverphoto", SqlDbType.Int,4),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4)};
                parametersDI[0].Value = Guid.NewGuid();
                parametersDI[1].Value = mDishesImage.vGuid;
                parametersDI[2].Value = mDishesImage.imgPath;
                parametersDI[3].Value = mDishesImage.format;
                parametersDI[4].Value = mDishesImage.imgName;
                parametersDI[5].Value = mDishesImage.size;
                parametersDI[6].Value = mDishesImage.isCoverphoto;
                parametersDI[7].Value = mDishesImage.createDate;
                parametersDI[8].Value = mDishesImage.isValid;

                comm.CommandText = strSql.ToString();
                comm.Parameters.Clear();
                comm.Parameters.AddRange(parametersDI);
                rCount += comm.ExecuteNonQuery();

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }

        public bool TransactionDailyMeal(LDDC.Model.SetMeal mSetMeal, LDDC.Model.DailyMeal mDailyMeal, DateTime begDate, DateTime endDate, string arrGuid, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                for (DateTime dt = begDate; dt <= endDate; dt = dt.AddDays(1))
                {
                    Guid guid = Guid.NewGuid();
                    strSql = new StringBuilder();
                    strSql.Append("insert into SetMeal(");
                    strSql.Append("guid,rGuid,eGuid,eType,begDate,endDate,creatorguid,creator)");
                    strSql.Append(" values (");
                    strSql.Append("@guid,@rGuid,@eGuid,@eType,@begDate,@endDate,@creatorguid,@creator)");
                    SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@rGuid", SqlDbType.VarChar,60),
					new SqlParameter("@eGuid", SqlDbType.VarChar,60),
					new SqlParameter("@eType", SqlDbType.VarChar,10),
					new SqlParameter("@begDate", SqlDbType.DateTime),
					new SqlParameter("@endDate", SqlDbType.DateTime),
					new SqlParameter("@creatorguid", SqlDbType.VarChar,60),
					new SqlParameter("@creator", SqlDbType.VarChar,10)};
                    parameters[0].Value = guid;
                    parameters[1].Value = mSetMeal.rGuid;
                    parameters[2].Value = mSetMeal.eGuid;
                    parameters[3].Value = mSetMeal.eType;
                    parameters[4].Value = dt;
                    parameters[5].Value = dt;
                    parameters[6].Value = mSetMeal.creatorguid;
                    parameters[7].Value = mSetMeal.creator;

                    comm.CommandText = strSql.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.AddRange(parameters);
                    rCount += comm.ExecuteNonQuery();

                    string[] arrTemp = arrGuid.Split(',');

                    for (int i = 0; i < arrTemp.Length; i++)
                    {
                        strSql = new StringBuilder();
                        strSql.Append("insert into DailyMeal(");
                        strSql.Append("guid,sGuid,vGuid,creatorguid,creator)");
                        strSql.Append(" values (");
                        strSql.Append("@guid,@sGuid,@vGuid,@creatorguid,@creator)");
                        SqlParameter[] parametersDM = {
					        new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					        new SqlParameter("@sGuid", SqlDbType.VarChar,60),
					        new SqlParameter("@vGuid", SqlDbType.VarChar,60),
					        new SqlParameter("@creatorguid", SqlDbType.VarChar,60),
					        new SqlParameter("@creator", SqlDbType.VarChar,10)};
                        parametersDM[0].Value = Guid.NewGuid();
                        parametersDM[1].Value = guid.ToString();//主表guid
                        parametersDM[2].Value = arrTemp[i].ToString();
                        parametersDM[3].Value = mSetMeal.creatorguid;
                        parametersDM[4].Value = mSetMeal.creator;

                        comm.CommandText = strSql.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.AddRange(parametersDM);
                        rCount += comm.ExecuteNonQuery();
                    }
                }

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }

        public bool TransactionUpdateDMeal(LDDC.Model.SetMeal mSetMeal, LDDC.Model.DailyMeal mDailyMeal, DateTime begDate, DateTime endDate, string arrGuid, string sGuid, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {
                for (DateTime dt = begDate; dt <= endDate; dt = dt.AddDays(1))
                {
                    strSql.Append("delete from [LDDC].[dbo].[DailyMeal] where sGuid = '" + sGuid + "'");
                    comm.CommandText = strSql.ToString();
                    rCount += comm.ExecuteNonQuery();

                    string[] arrTemp = arrGuid.Split(',');

                    for (int i = 0; i < arrTemp.Length; i++)
                    {
                        strSql = new StringBuilder();
                        strSql.Append("insert into DailyMeal(");
                        strSql.Append("guid,sGuid,vGuid,creatorguid,creator)");
                        strSql.Append(" values (");
                        strSql.Append("@guid,@sGuid,@vGuid,@creatorguid,@creator)");
                        SqlParameter[] parametersDM = {
					        new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					        new SqlParameter("@sGuid", SqlDbType.VarChar,60),
					        new SqlParameter("@vGuid", SqlDbType.VarChar,60),
					        new SqlParameter("@creatorguid", SqlDbType.VarChar,60),
					        new SqlParameter("@creator", SqlDbType.VarChar,10)};
                        parametersDM[0].Value = Guid.NewGuid();
                        parametersDM[1].Value = sGuid;//主表guid
                        parametersDM[2].Value = arrTemp[i].ToString();
                        parametersDM[3].Value = mSetMeal.creatorguid;
                        parametersDM[4].Value = mSetMeal.creator;

                        comm.CommandText = strSql.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.AddRange(parametersDM);
                        rCount += comm.ExecuteNonQuery();
                    }
                }

                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }


        public bool TransactionDeleteDMeal(string sGuid, ref int rCount, ref string errorInfo)
        {
            StringBuilder strSql = new StringBuilder();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction tran;
            tran = conn.BeginTransaction();
            SqlCommand comm = conn.CreateCommand();
            comm.Connection = conn;
            comm.Transaction = tran;

            try
            {

                strSql.Append("delete from [LDDC].[dbo].[DailyMeal] where sGuid = '" + sGuid + "'");
                comm.CommandText = strSql.ToString();
                rCount += comm.ExecuteNonQuery();

                strSql = new StringBuilder();
                strSql.Append("delete from [LDDC].[dbo].[SetMeal] where guid = '" + sGuid + "'");
                comm.CommandText = strSql.ToString();
                rCount += comm.ExecuteNonQuery();


                tran.Commit();
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                errorInfo = strSql.ToString();
                conn.Close();
                return false;
            }
            finally
            {
                //conn.Close();
            }
        }

        //public bool TransactionDailyMeal(LDDC.Model.SetMeal mSetMeal, LDDC.Model.DailyMeal mDailyMeal, string arrGuid, ref int rCount, ref string errorInfo)
        //{
        //    StringBuilder strSql = new StringBuilder();

        //    SqlConnection conn = new SqlConnection(connectionString);
        //    conn.Open();
        //    SqlTransaction tran;
        //    tran = conn.BeginTransaction();
        //    SqlCommand comm = conn.CreateCommand();
        //    comm.Connection = conn;
        //    comm.Transaction = tran;

        //    try
        //    {
        //        strSql.Append("insert into SetMeal(");
        //        strSql.Append("guid,rGuid,eGuid,eType,begDate,endDate,creatorguid,creator,createDate,isValid)");
        //        strSql.Append(" values (");
        //        strSql.Append("@guid,@rGuid,@eGuid,@eType,@begDate,@endDate,@creatorguid,@creator,@createDate,@isValid)");
        //        SqlParameter[] parameters = {
        //            new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
        //            new SqlParameter("@rGuid", SqlDbType.VarChar,60),
        //            new SqlParameter("@eGuid", SqlDbType.VarChar,60),
        //            new SqlParameter("@eType", SqlDbType.VarChar,10),
        //            new SqlParameter("@begDate", SqlDbType.DateTime),
        //            new SqlParameter("@endDate", SqlDbType.DateTime),
        //            new SqlParameter("@creatorguid", SqlDbType.VarChar,60),
        //            new SqlParameter("@creator", SqlDbType.VarChar,10),
        //            new SqlParameter("@createDate", SqlDbType.DateTime),
        //            new SqlParameter("@isValid", SqlDbType.Int,4)};
        //        parameters[0].Value = mSetMeal.guid;
        //        parameters[1].Value = mSetMeal.rGuid;
        //        parameters[2].Value = mSetMeal.eGuid;
        //        parameters[3].Value = mSetMeal.eType;
        //        parameters[4].Value = mSetMeal.begDate;
        //        parameters[5].Value = mSetMeal.endDate;
        //        parameters[6].Value = mSetMeal.creatorguid;
        //        parameters[7].Value = mSetMeal.creator;

        //        rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

        //        string[] arrTemp = arrGuid.Split(',');

        //        for (int i = 0; i < arrTemp.Length; i++)
        //        {
        //            strSql = new StringBuilder();
        //            strSql.Append("insert into DailyMeal(");
        //            strSql.Append("guid,sGuid,vGuid,creatorguid,creator,createDate,isValid)");
        //            strSql.Append(" values (");
        //            strSql.Append("@guid,@sGuid,@vGuid,@creatorguid,@creator,@createDate,@isValid)");
        //            SqlParameter[] parametersDM = {
        //            new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
        //            new SqlParameter("@sGuid", SqlDbType.VarChar,60),
        //            new SqlParameter("@vGuid", SqlDbType.VarChar,60),
        //            new SqlParameter("@creatorguid", SqlDbType.VarChar,60),
        //            new SqlParameter("@creator", SqlDbType.VarChar,10),
        //            new SqlParameter("@createDate", SqlDbType.DateTime),
        //            new SqlParameter("@isValid", SqlDbType.Int,4)};
        //            parametersDM[0].Value = Guid.NewGuid();
        //            parametersDM[1].Value = mSetMeal.guid.ToString();//主表guid
        //            parametersDM[2].Value = arrTemp[i].ToString();
        //            parametersDM[3].Value = mSetMeal.creatorguid;
        //            parametersDM[4].Value = mSetMeal.creator;

        //            rCount += DbHelperSQL.ExecuteSql(strSql.ToString(), parametersDM);
        //        }

        //        tran.Commit();
        //        conn.Close();
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        errorInfo = strSql.ToString();
        //        conn.Close();
        //        return false;
        //    }
        //    finally
        //    {
        //        //conn.Close();
        //    }
        //}

        public DataSet GetDailyMealList(string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.[guid],a.[sGuid],a.[vGuid],b.[vtID],b.[vtName],b.[name],b.[describe],  ");
            strSql.Append("c.[imgPath]   ");
            strSql.Append("FROM [LDDC].[dbo].[DailyMeal] a  ");
            strSql.Append("left join [LDDC].[dbo].[MenuInfo] b on a.vGuid = b.guid  ");
            strSql.Append("left join [LDDC].[dbo].[DishesImage] c on b.guid = c.vGuid   ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        //获取菜品和图片信息
        public DataSet GetMenuInfoList(string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.guid,a.rGuid,a.vtID,a.vtName,a.name,a.describe,a.creatorguid,a.creator,a.createDate,a.isValid, ");
            strSql.Append("b.guid,b.vGuid,b.imgPath,b.format,b.imgName,b.size,b.isCoverphoto,b.createDate,b.isValid,b.sortNo  ");
            strSql.Append("FROM MenuInfo a  ");
            strSql.Append("left join DishesImage b on a.guid = b.vGuid  ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }
        public DataSet GetMenuType(string rGuid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select vtID,vtName  ");
            strSql.Append("from MenuInfo a  ");
            strSql.Append("where rGuid = '" + rGuid + "'  ");
            strSql.Append("group by  vtID,vtName  ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetMaxEndDate(string rGuid)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MAX([endDate]) endDate from [LDDC].[dbo].[SetMeal] where rGuid = '" + rGuid + "'  ");
            return DbHelperSQL.Query(strSql.ToString());
        }
        public DataSet GetNowDate(string sGuid)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select [guid],[rGuid],[eGuid],[eType],[begDate],[endDate],[creatorguid],[creator],[createDate],[isValid] from [LDDC].[dbo].[SetMeal] where guid = '" + sGuid + "'  ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetIsAddDailyMeal(string rGuid, string begDate, string endDate, int vtid)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [LDDC].[dbo].[SetMeal] where [rGuid]='" + rGuid + "' and (([begDate] <= '" + begDate + "' and [endDate] >= '" + begDate + "') or([begDate] <= '" + endDate + "' and [endDate] >= '" + endDate + "')) and [eGuid] = " + vtid + "");
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetFutureMeal(string rGuid, string rUserID, string orderDate)
        {//获取该餐厅配餐结束的最大日期
            StringBuilder strSql = new StringBuilder();

            //strSql.Append("SELECT rUserID,osState,orderDate,DATENAME(DW,orderDate) as week,eType ");
            //strSql.Append("FROM [LDDC].[dbo].[OrderInfo] AS a ");
            //strSql.Append("where rGuid = '" + rGuid + "' and  [orderDate] >= '" + orderDate + "' and rUserID = '" + rUserID + "'");
            //strSql.Append("Order BY orderDate,eID");

            strSql.Append("SELECT a.rUserID,a.osState,b.name,a.orderDate,DATENAME(DW,a.orderDate) as week,a.eType  ");
            strSql.Append("FROM [LDDC].[dbo].[OrderInfo] a  ");
            strSql.Append("left join [LDDC].[dbo].[RestaurantInfo] b on a.rguid = b.guid ");
            strSql.Append("where a.[orderDate] >= '" + orderDate + "' and a.rUserID = '" + rUserID + "' ");
            strSql.Append("Order BY a.orderDate,a.eID ");

            return DbHelperSQL.Query(strSql.ToString());
        }


        public string GetVTJsonByDataset(DataSet ds)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataTable dt in ds.Tables)
            {
                sb.Append(string.Format("["));

                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        if (dr.Table.Columns[i].ColumnName == "id")
                        {
                            sb.AppendFormat("{0}:\'{1}\',", "value", ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                        else if (dr.Table.Columns[i].ColumnName == "type")
                        {
                            sb.AppendFormat("{0}:\'{1}\',", "text", ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                    }
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("},");
                }

                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("]");
                //sb.Remove(sb.ToString().LastIndexOf(','), 1);
                //sb.Append("],");
            }
            //sb.Remove(sb.ToString().LastIndexOf(','), 1);
            //sb.Append("}");
            return sb.ToString();
        }

        public string GetVTJsonByDataset(DataSet ds, string id, string type,string F1,string F2,string F3)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataTable dt in ds.Tables)
            {
                sb.Append(string.Format("["));

                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        if (dr.Table.Columns[i].ColumnName == id)
                        {
                            sb.AppendFormat("{0}:\'{1}\',", "value", ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                        else if (dr.Table.Columns[i].ColumnName == F1)
                        {
                            sb.AppendFormat("{0}:\'{1}\',", "F1", ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                        else if (dr.Table.Columns[i].ColumnName == F2)
                        {
                            sb.AppendFormat("{0}:\'{1}\',", "F2", ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                        else if (dr.Table.Columns[i].ColumnName == F3)
                        {
                            sb.AppendFormat("{0}:\'{1}\',", "F3", ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                        else if (dr.Table.Columns[i].ColumnName == type)
                        {
                            sb.AppendFormat("{0}:\'{1}\',", "text", ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                    }
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("},");
                }

                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("]");
                //sb.Remove(sb.ToString().LastIndexOf(','), 1);
                //sb.Append("],");
            }
            //sb.Remove(sb.ToString().LastIndexOf(','), 1);
            //sb.Append("}");
            return sb.ToString();
        }

        public string GetVTJsonByDataset(DataSet ds, string id, string type)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataTable dt in ds.Tables)
            {
                sb.Append(string.Format("["));

                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        if (dr.Table.Columns[i].ColumnName == id)
                        {
                            sb.AppendFormat("{0}:\'{1}\',", "value", ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                        else if (dr.Table.Columns[i].ColumnName == type)
                        {
                            sb.AppendFormat("{0}:\'{1}\',", "text", ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                    }
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("},");
                }

                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("]");
                //sb.Remove(sb.ToString().LastIndexOf(','), 1);
                //sb.Append("],");
            }
            //sb.Remove(sb.ToString().LastIndexOf(','), 1);
            //sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 把dataset数据转换成json的格式
        /// </summary>
        /// <param name="ds">dataset数据集</param>
        /// <returns>json格式的字符串</returns>
        public string GetJsonByDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                //如果查询到的数据为空则返回标记ok:false
                return "{\"ok\":false}";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"ok\":true,");
            foreach (DataTable dt in ds.Tables)
            {
                sb.Append(string.Format("\"{0}\":[", dt.TableName));

                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                    }
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("},");
                }

                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("],");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 将object转换成为string
        /// </summary>
        /// <param name="ob">obj对象</param>
        /// <returns></returns>
        public static string ObjToStr(object ob)
        {
            if (ob == null)
            {
                return string.Empty;
            }
            else
                return ob.ToString();
        }

        //public bool TransactionListSql(HLFW.Model.LAW_info pLAW_info, ref int rCount, ref string errorInfo)
        //{
        //    //string sql = String.Empty;
        //    StringBuilder strSql = new StringBuilder();

        //    SqlConnection conn = new SqlConnection(connectionString);
        //    conn.Open();
        //    SqlTransaction tran;
        //    tran = conn.BeginTransaction();
        //    SqlCommand comm = conn.CreateCommand();
        //    comm.Connection = conn;
        //    comm.Transaction = tran;

        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds = DbHelperSQL.Query("select MAX(cast(lawnum as int)) as lawnum from dbo.LAW_info where lawnum like '" + System.DateTime.Now.Year + "%';");
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            if (Convert.IsDBNull(ds.Tables[0].Rows[0]["lawnum"]))
        //            {
        //                int lawnum = System.DateTime.Now.Year * 10000 + 1;
        //                pLAW_info.lawnum = lawnum.ToString();
        //            }
        //            else
        //            {
        //                int lawnum = Convert.ToInt32(ds.Tables[0].Rows[0]["lawnum"]) + 1;
        //                pLAW_info.lawnum = lawnum.ToString();
        //            }
        //        }

        //        DataSet dsYZ = new DataSet();
        //        dsYZ = DbHelperSQL.Query("select * from dbo.LAW_info where lawnum = '" + pLAW_info.lawnum + "'");

        //        if (dsYZ.Tables[0].Rows.Count > 0)
        //        {
        //            errorInfo = "重复的“案件编号”，请重新点击“保存”！";
        //            return false;
        //        }

        //        strSql.Append("insert into LAW_info(");
        //        strSql.Append("lawid,lawarea,areaguid,lawnum,lawdate,lawno,lawexcuse,lawparty,lawrequest,lawamount,lawprogress,lawresult,lawending,lawmoney,lawpay,lawyer,remark,lawperson,laworgan,lawexternal,createuserid,createusername,position,positionid,basiccase,trialdate,casepro,caseproid,salvageloss,lawname,lawexcuseid,otherexcuse)");
        //        strSql.Append(" values (");
        //        strSql.Append("'" + pLAW_info.lawid + "','" + pLAW_info.lawarea + "','" + pLAW_info.areaguid + "','" + pLAW_info.lawnum + "','" + pLAW_info.lawdate + "','" + pLAW_info.lawno + "','" + pLAW_info.lawexcuse + "','" + pLAW_info.lawparty + "','" + pLAW_info.lawrequest + "'," + pLAW_info.lawamount + ",'" + pLAW_info.lawprogress + "', '" + pLAW_info.lawresult + "', '" + pLAW_info.lawending + "'," + pLAW_info.lawmoney + "," + pLAW_info.lawpay + "," + pLAW_info.lawyer + ",'" + pLAW_info.remark + "', '" + pLAW_info.lawperson + "', '" + pLAW_info.laworgan + "', '" + pLAW_info.lawexternal + "', '" + pLAW_info.createuserid + "', '" + pLAW_info.createusername + "', '" + pLAW_info.position + "'," + pLAW_info.positionid + ", '" + pLAW_info.basiccase + "', '" + pLAW_info.trialdate + "', '" + pLAW_info.casepro + "'," + pLAW_info.caseproid + "," + pLAW_info.salvageloss + ", '" + pLAW_info.lawname + "', " + pLAW_info.lawexcuseid + ", '" + pLAW_info.otherexcuse + "')");
        //        comm.CommandText = strSql.ToString();
        //        comm.ExecuteNonQuery();
        //        rCount += 1;

        //        strSql = new StringBuilder();
        //        strSql.Append("insert into dbo.LAW_caseprodetaile (inputdate,type,name,userid,username,infoguid) ");
        //        strSql.Append(" values ");
        //        strSql.Append("(getdate()," + pLAW_info.caseproid + ",'" + pLAW_info.casepro + "','" + pLAW_info.createuserid + "', '" + pLAW_info.createusername + "','" + pLAW_info.lawid + "')");
        //        comm.CommandText = strSql.ToString();
        //        comm.ExecuteNonQuery();
        //        rCount += 1;

        //        strSql = new StringBuilder();
        //        strSql.Append("update dbo.LAW_file set isuse = 1 where infoid = '" + pLAW_info.lawid.ToString() + "'");
        //        comm.CommandText = strSql.ToString();
        //        comm.ExecuteNonQuery();
        //        rCount += 1;





        //        tran.Commit();
        //        conn.Close();
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        errorInfo = strSql.ToString();
        //        conn.Close();
        //        return false;
        //    }
        //    finally
        //    {
        //        //conn.Close();
        //    }

        //}

        public string AccessToken()
        {
            string AccessToken = string.Empty;
            string CorpID = ConfigurationManager.AppSettings["appid"];
            string Secret = ConfigurationManager.AppSettings["appsecret"];

            AccessToken = GetAccessToken(CorpID, Secret);

            return AccessToken;
        }

        public string AccessToken(string CorpID, string Secret)
        {
            string AccessToken = string.Empty;

            AccessToken = GetAccessToken(CorpID, Secret);

            return AccessToken;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="corpid"></param>
        /// <param name="corpsecret"></param>
        /// <returns></returns>
        private string GetAccessToken(string corpid, string corpsecret)
        {
            string Gurl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", corpid, corpsecret);
            string html = HttpHelper.HttpGet(Gurl, "");
            string regex = "\"access_token\":\"(?<token>.*?)\"";

            string token = CRegex.GetText(html, regex, "token");
            return token;
        }

        public string GetCacheAccessToken()
        {
            string token = String.Empty;
            if (CacheHelper.GetCache("wxAccessToken") == String.Empty || CacheHelper.GetCache("wxAccessToken") == null)
            {
                token = AccessToken();
                TimeSpan ts = new TimeSpan(1, 30, 0);
                CacheHelper.SetCache("wxAccessToken", token, ts);
            }
            else
            {
                token = CacheHelper.GetCache("wxAccessToken").ToString();
            }

            return token;
        }


        /// <summary>
        /// 从微信服务器获取图片
        /// </summary>
        /// <param name="imgServerId">图片服务器存储id</param>
        /// <returns>图片</returns>
        public Image GetImage(string accesstoken, string imgServerId)
        {
            string url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", accesstoken, imgServerId);
            try
            {
                // 1.创建httpWebRequest对象
                WebRequest webRequest = WebRequest.Create(url);
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;

                // 2.填充httpWebRequest的基本信息
                httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)"; ;
                httpRequest.ContentType = "application/x-www-form-urlencoded"; ;
                httpRequest.Method = "get";

                Stream responseStream = httpRequest.GetResponse().GetResponseStream();
                Image img = Image.FromStream(responseStream);
                responseStream.Close();
                return img;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <param name="corpid"></param>
        /// <param name="corpsecret"></param>
        /// <returns></returns>
        public string GetJsapi_ticket(string accesstoken)
        {
            string Gurl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token={0}", accesstoken);
            string html = HttpHelper.HttpGet(Gurl, "");
            string regex = "\"ticket\":\"(?<token>.*?)\"";

            string token = CRegex.GetText(html, regex, "token");
            return token;
        }

        public string GetCachewxTicket(string appId)
        {
            string token = String.Empty;
            if (CacheHelper.GetCache("wxTicket") == String.Empty || CacheHelper.GetCache("wxTicket") == null)
            {
                token = GetJsapi_ticket(appId);
                //TimeSpan ts = new TimeSpan(0, 1, 30);
                TimeSpan ts = new TimeSpan(1, 30, 0);
                CacheHelper.SetCache("wxTicket", token, ts);
            }
            else
            {
                token = CacheHelper.GetCache("wxTicket").ToString();
            }


            return token;
        }


        //获取jssdk所需签名
        public string Signature(string ticket, string noncestr, string timestamp, string url, ref string jsapi_ticket)
        {
            //string noncestr = "Wm3WZYTPz0wzccnW";
            //int timestamp = 1414587457;
            //string ticket = GetTicket();

            string string1 = "jsapi_ticket=" + ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;
            jsapi_ticket = string1;
            string signature = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1");
            return signature.ToLower();
        }


        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns>返回毫秒数</returns>
        public long GetTime()
        {
            return (DateTime.Now.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
        }

        //根据openid，access token获得用户信息
        public WXUser Get_UserInfo(string REFRESH_TOKEN, string OPENID, ref string getjson)
        {
            //string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID);公众号接口
            string Str = GetJson("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token=" + REFRESH_TOKEN + "&code=" + OPENID);
            getjson = Str;
            //div1.InnerText += " <Get_UserInfo> " + Str + " <> ";

            WXUser OAuthUser_Model = JsonHelper.ParseFromJson<WXUser>(Str);
            return OAuthUser_Model;
        }

        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public UserGetJson GetUser(string accessToken, string userid)
        {
            string urlFormat = "https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={0}&userid={1}";
            var url = string.Format(urlFormat, accessToken, userid);
            string resultMsg = HttpHelper.HttpGet(url, "");
            //反序列化
            UserGetJson result = JsonConvert.DeserializeObject<UserGetJson>(resultMsg);

            return result;
        }

        //访问微信url并返回微信信息
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = System.Text.Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
                return returnText;
            }
            return returnText;
        }

        public class WXUser
        {
            public WXUser()
            { }
            #region 数据库字段
            private string _UserId;
            private string _DeviceId;

            #endregion

            #region 字段属性
            /// <summary>
            /// 用户的唯一标识
            /// </summary>
            public string UserId
            {
                set { _UserId = value; }
                get { return _UserId; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string DeviceId
            {
                set { _DeviceId = value; }
                get { return _DeviceId; }
            }

            #endregion
        }

        public class UserGetJson
        {
            /// <summary>
            /// 返回的错误消息
            /// </summary>
            public string errcode { get; set; }

            /// <summary>
            /// 对返回码的文本描述内容
            /// </summary>
            public string errmsg { get; set; }

            /// <summary>
            /// 成员UserID。对应管理端的帐号
            /// </summary>
            public string userid { get; set; }

            /// <summary>
            /// 成员名称
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// 成员所属部门id列表
            /// </summary>
            public List<int> department { get; set; }

            /// <summary>
            /// 职位信息
            /// </summary>
            public string position { get; set; }

            /// <summary>
            /// 手机号码 
            /// </summary>
            public string mobile { get; set; }

            /// <summary>
            /// 性别。0表示未定义，1表示男性，2表示女性 
            /// </summary>
            public string gender { get; set; }

            /// <summary>
            /// 邮箱 
            /// </summary>
            public string email { get; set; }

            /// <summary>
            /// 微信号 
            /// </summary>
            public string weixinid { get; set; }

            /// <summary>
            /// 头像url。注：如果要获取小图将url最后的"/0"改成"/64"即可 
            /// </summary>
            public string avatar { get; set; }

            /// <summary>
            /// 关注状态: 1=已关注，2=已冻结，4=未关注 
            /// </summary>
            public int status { get; set; }

            /// <summary>
            ///  扩展属性   "extattr": {"attrs":[{"name":"爱好","value":"旅游"},{"name":"卡号","value":"1234567234"}]} 
            /// </summary>
            public UserJsonAttrs extattr { get; set; }

        }

        public class UserJsonAttrs
        {

            public UserJsonAttrs()
            {
                this.attrs = new List<UserJsonAttrsInfo>();
            }


            /// <summary>
            /// 扩展信息
            /// </summary>
            public List<UserJsonAttrsInfo> attrs { get; set; }
        }

        public class UserJsonAttrsInfo
        {
            /// <summary>
            /// 扩展 name
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// 扩展 value
            /// </summary>
            public string value { get; set; }
        }

    }





    /// <summary>
    /// 将Json格式数据转化成对象
    /// </summary>
    public class JsonHelper
    {
        /// <summary>  
        /// 生成Json格式  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
            }
        }
        /// <summary>  
        /// 获取Json的Model  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="szJson"></param>  
        /// <returns></returns>  
        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }


    }


}






//public class T//:IEquatable<Person>
//{
//    public string _sql = null;
//    public string _sql2 = null;
//    public string _inputdate = null;
//    public string _bankaccountno = null;
//    public string _cName = null;
//    public string _cAreaname = null;

//    public T() { }

//    public T(string sql, string inputdate, string bankaccountno, string cName, string cAreaname, string sql2)
//    {
//        this._sql = sql;
//        this._sql2 = sql2;
//        this._inputdate = inputdate;
//        this._bankaccountno = bankaccountno;
//        this._cName = cName;
//        this._cAreaname = cAreaname;
//    }
//}


