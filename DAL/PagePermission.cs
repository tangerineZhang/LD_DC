using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LD.Common;
//using Maticsoft.DBUtility;//Please add references
namespace LDDC.DAL
{
	/// <summary>
	/// 数据访问类:PagePermission
	/// </summary>
	public partial class PagePermission
	{
		public PagePermission()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(Guid guid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from PagePermission");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = guid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(LDDC.Model.PagePermission model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into PagePermission(");
			strSql.Append("guid,userID,user,pID,Permission,creatorguid,creator,createDate,isValid)");
			strSql.Append(" values (");
			strSql.Append("@guid,@userID,@user,@pID,@Permission,@creatorguid,@creator,@createDate,@isValid)");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@userID", SqlDbType.VarChar,60),
					new SqlParameter("@user", SqlDbType.VarChar,60),
					new SqlParameter("@pID", SqlDbType.Int,4),
					new SqlParameter("@Permission", SqlDbType.VarChar,100),
					new SqlParameter("@creatorguid", SqlDbType.VarChar,60),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = model.userID;
			parameters[2].Value = model.user;
			parameters[3].Value = model.pID;
			parameters[4].Value = model.Permission;
			parameters[5].Value = model.creatorguid;
			parameters[6].Value = model.creator;
			parameters[7].Value = model.createDate;
			parameters[8].Value = model.isValid;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Update(LDDC.Model.PagePermission model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update PagePermission set ");
			strSql.Append("userID=@userID,");
			strSql.Append("user=@user,");
			strSql.Append("pID=@pID,");
			strSql.Append("Permission=@Permission,");
			strSql.Append("creatorguid=@creatorguid,");
			strSql.Append("creator=@creator,");
			strSql.Append("createDate=@createDate,");
			strSql.Append("isValid=@isValid");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@userID", SqlDbType.VarChar,60),
					new SqlParameter("@user", SqlDbType.VarChar,60),
					new SqlParameter("@pID", SqlDbType.Int,4),
					new SqlParameter("@Permission", SqlDbType.VarChar,100),
					new SqlParameter("@creatorguid", SqlDbType.VarChar,60),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4),
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)};
			parameters[0].Value = model.userID;
			parameters[1].Value = model.user;
			parameters[2].Value = model.pID;
			parameters[3].Value = model.Permission;
			parameters[4].Value = model.creatorguid;
			parameters[5].Value = model.creator;
			parameters[6].Value = model.createDate;
			parameters[7].Value = model.isValid;
			parameters[8].Value = model.guid;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from PagePermission ");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = guid;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string guidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from PagePermission ");
			strSql.Append(" where guid in ("+guidlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public LDDC.Model.PagePermission GetModel(Guid guid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 guid,userID,user,pID,Permission,creatorguid,creator,createDate,isValid from PagePermission ");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = guid;

			LDDC.Model.PagePermission model=new LDDC.Model.PagePermission();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
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
		public LDDC.Model.PagePermission DataRowToModel(DataRow row)
		{
			LDDC.Model.PagePermission model=new LDDC.Model.PagePermission();
			if (row != null)
			{
				if(row["guid"]!=null && row["guid"].ToString()!="")
				{
					model.guid= new Guid(row["guid"].ToString());
				}
				if(row["userID"]!=null)
				{
					model.userID=row["userID"].ToString();
				}
				if(row["user"]!=null)
				{
					model.user=row["user"].ToString();
				}
				if(row["pID"]!=null && row["pID"].ToString()!="")
				{
					model.pID=int.Parse(row["pID"].ToString());
				}
				if(row["Permission"]!=null)
				{
					model.Permission=row["Permission"].ToString();
				}
				if(row["creatorguid"]!=null)
				{
					model.creatorguid=row["creatorguid"].ToString();
				}
				if(row["creator"]!=null)
				{
					model.creator=row["creator"].ToString();
				}
				if(row["createDate"]!=null && row["createDate"].ToString()!="")
				{
					model.createDate=DateTime.Parse(row["createDate"].ToString());
				}
				if(row["isValid"]!=null && row["isValid"].ToString()!="")
				{
					model.isValid=int.Parse(row["isValid"].ToString());
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select guid,userID,user,pID,Permission,creatorguid,creator,createDate,isValid ");
			strSql.Append(" FROM PagePermission ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" guid,userID,user,pID,Permission,creatorguid,creator,createDate,isValid ");
			strSql.Append(" FROM PagePermission ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM PagePermission ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.guid desc");
			}
			strSql.Append(")AS Row, T.*  from PagePermission T ");
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
			parameters[0].Value = "PagePermission";
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

