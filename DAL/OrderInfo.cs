using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LD.Common;

namespace LDDC.DAL
{
    /// <summary>
    /// 数据访问类:OrderInfo
    /// </summary>
    public partial class OrderInfo
    {
        public OrderInfo()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(Guid guid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from OrderInfo");
            strSql.Append(" where guid=@guid ");
            SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
            parameters[0].Value = guid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(LDDC.Model.OrderInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OrderInfo(");
            strSql.Append("guid,rGuid,orderDate,eID,eType,rDate,eDate,rUserID,rUser,isTakeout,tDate,tUserID,tUser,osGuid,osState,creatorID,creator,createDate,isValid)");
            strSql.Append(" values (");
            strSql.Append("@guid,@rGuid,@eID,@eType,@rDate,@eDate,@rUserID,@rUser,@isTakeout,@tDate,@tUserID,@tUser,@osGuid,@osState,@creatorID,@creator,@createDate,@isValid)");
            SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@rGuid", SqlDbType.VarChar,60),
                    new SqlParameter("@orderDate", SqlDbType.DateTime),
					new SqlParameter("@eID", SqlDbType.Int,4),
					new SqlParameter("@eType", SqlDbType.VarChar,10),
					new SqlParameter("@rDate", SqlDbType.DateTime),
					new SqlParameter("@eDate", SqlDbType.DateTime),
					new SqlParameter("@rUserID", SqlDbType.VarChar,60),
					new SqlParameter("@rUser", SqlDbType.VarChar,10),
					new SqlParameter("@isTakeout", SqlDbType.Int,4),
					new SqlParameter("@tDate", SqlDbType.DateTime),
					new SqlParameter("@tUserID", SqlDbType.VarChar,30),
					new SqlParameter("@tUser", SqlDbType.VarChar,10),
					new SqlParameter("@osGuid", SqlDbType.Int,4),
					new SqlParameter("@osState", SqlDbType.VarChar,10),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4)};
            parameters[0].Value = Guid.NewGuid();
            parameters[1].Value = model.rGuid;
            parameters[2].Value = model.eID;
            parameters[3].Value = model.eType;
            parameters[4].Value = model.rDate;
            parameters[5].Value = model.eDate;
            parameters[6].Value = model.rUserID;
            parameters[7].Value = model.rUser;
            parameters[8].Value = model.isTakeout;
            parameters[9].Value = model.tDate;
            parameters[10].Value = model.tUserID;
            parameters[11].Value = model.tUser;
            parameters[12].Value = model.osGuid;
            parameters[13].Value = model.osState;
            parameters[14].Value = model.creatorID;
            parameters[15].Value = model.creator;
            parameters[16].Value = model.createDate;
            parameters[17].Value = model.isValid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LDDC.Model.OrderInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OrderInfo set ");
            strSql.Append("rGuid=@rGuid,");
            strSql.Append("rorderDate=@rorderDate,");
            strSql.Append("eID=@eID,");
            strSql.Append("eType=@eType,");
            strSql.Append("rDate=@rDate,");
            strSql.Append("eDate=@eDate,");
            strSql.Append("rUserID=@rUserID,");
            strSql.Append("rUser=@rUser,");
            strSql.Append("isTakeout=@isTakeout,");
            strSql.Append("tDate=@tDate,");
            strSql.Append("tUserID=@tUserID,");
            strSql.Append("tUser=@tUser,");
            strSql.Append("osGuid=@osGuid,");
            strSql.Append("osState=@osState,");
            strSql.Append("creatorID=@creatorID,");
            strSql.Append("creator=@creator,");
            strSql.Append("createDate=@createDate,");
            strSql.Append("isValid=@isValid");
            strSql.Append(" where guid=@guid ");
            SqlParameter[] parameters = {
					new SqlParameter("@rGuid", SqlDbType.VarChar,60),
                    new SqlParameter("@orderDate", SqlDbType.DateTime),
					new SqlParameter("@eID", SqlDbType.Int,4),
					new SqlParameter("@eType", SqlDbType.VarChar,10),
					new SqlParameter("@rDate", SqlDbType.DateTime),
					new SqlParameter("@eDate", SqlDbType.DateTime),
					new SqlParameter("@rUserID", SqlDbType.VarChar,60),
					new SqlParameter("@rUser", SqlDbType.VarChar,10),
					new SqlParameter("@isTakeout", SqlDbType.Int,4),
					new SqlParameter("@tDate", SqlDbType.DateTime),
					new SqlParameter("@tUserID", SqlDbType.VarChar,30),
					new SqlParameter("@tUser", SqlDbType.VarChar,10),
					new SqlParameter("@osGuid", SqlDbType.Int,4),
					new SqlParameter("@osState", SqlDbType.VarChar,10),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4),
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)};
            parameters[0].Value = model.rGuid;
            parameters[1].Value = model.orderDate;
            parameters[2].Value = model.eID;
            parameters[3].Value = model.eType;
            parameters[4].Value = model.rDate;
            parameters[5].Value = model.eDate;
            parameters[6].Value = model.rUserID;
            parameters[7].Value = model.rUser;
            parameters[8].Value = model.isTakeout;
            parameters[9].Value = model.tDate;
            parameters[10].Value = model.tUserID;
            parameters[11].Value = model.tUser;
            parameters[12].Value = model.osGuid;
            parameters[13].Value = model.osState;
            parameters[14].Value = model.creatorID;
            parameters[15].Value = model.creator;
            parameters[16].Value = model.createDate;
            parameters[17].Value = model.isValid;
            parameters[18].Value = model.guid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(Guid guid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from OrderInfo ");
            strSql.Append(" where guid=@guid ");
            SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
            parameters[0].Value = guid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string guidlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from OrderInfo ");
            strSql.Append(" where guid in (" + guidlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LDDC.Model.OrderInfo GetModel(Guid guid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 guid,rGuid,orderDate,eID,eType,rDate,eDate,rUserID,rUser,isTakeout,tDate,tUserID,tUser,osGuid,osState,creatorID,creator,createDate,isValid from OrderInfo ");
            strSql.Append(" where guid=@guid ");
            SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
            parameters[0].Value = guid;

            LDDC.Model.OrderInfo model = new LDDC.Model.OrderInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LDDC.Model.OrderInfo DataRowToModel(DataRow row)
        {
            LDDC.Model.OrderInfo model = new LDDC.Model.OrderInfo();
            if (row != null)
            {
                if (row["guid"] != null && row["guid"].ToString() != "")
                {
                    model.guid = new Guid(row["guid"].ToString());
                }
                if (row["rGuid"] != null)
                {
                    model.rGuid = row["rGuid"].ToString();
                }
                if (row["orderDate"] != null && row["orderDate"].ToString() != "")
                {
                    model.rDate = DateTime.Parse(row["orderDate"].ToString());
                }
                if (row["eID"] != null && row["eID"].ToString() != "")
                {
                    model.eID = int.Parse(row["eID"].ToString());
                }
                if (row["eType"] != null)
                {
                    model.eType = row["eType"].ToString();
                }
                if (row["rDate"] != null && row["rDate"].ToString() != "")
                {
                    model.rDate = DateTime.Parse(row["rDate"].ToString());
                }
                if (row["eDate"] != null && row["eDate"].ToString() != "")
                {
                    model.eDate = DateTime.Parse(row["eDate"].ToString());
                }
                if (row["rUserID"] != null)
                {
                    model.rUserID = row["rUserID"].ToString();
                }
                if (row["rUser"] != null)
                {
                    model.rUser = row["rUser"].ToString();
                }
                if (row["isTakeout"] != null && row["isTakeout"].ToString() != "")
                {
                    model.isTakeout = int.Parse(row["isTakeout"].ToString());
                }
                if (row["tDate"] != null && row["tDate"].ToString() != "")
                {
                    model.tDate = DateTime.Parse(row["tDate"].ToString());
                }
                if (row["tUserID"] != null)
                {
                    model.tUserID = row["tUserID"].ToString();
                }
                if (row["tUser"] != null)
                {
                    model.tUser = row["tUser"].ToString();
                }
                if (row["osGuid"] != null && row["osGuid"].ToString() != "")
                {
                    model.osGuid = int.Parse(row["osGuid"].ToString());
                }
                if (row["osState"] != null)
                {
                    model.osState = row["osState"].ToString();
                }
                if (row["creatorID"] != null)
                {
                    model.creatorID = row["creatorID"].ToString();
                }
                if (row["creator"] != null)
                {
                    model.creator = row["creator"].ToString();
                }
                if (row["createDate"] != null && row["createDate"].ToString() != "")
                {
                    model.createDate = DateTime.Parse(row["createDate"].ToString());
                }
                if (row["isValid"] != null && row["isValid"].ToString() != "")
                {
                    model.isValid = int.Parse(row["isValid"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select guid,rGuid,orderDate,eID,eType,rDate,eDate,rUserID,rUser,isTakeout,tDate,tUserID,tUser,osGuid,osState,creatorID,creator,createDate,isValid ");
            strSql.Append(" FROM OrderInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" guid,rGuid,orderDate,eID,eType,rDate,eDate,rUserID,rUser,isTakeout,tDate,tUserID,tUser,osGuid,osState,rAmount,creatorID,creator,createDate,isValid ");
            strSql.Append(" FROM OrderInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM OrderInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.guid desc");
            }
            strSql.Append(")AS Row, T.*  from OrderInfo T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "OrderInfo";
            parameters[1].Value = "guid";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

