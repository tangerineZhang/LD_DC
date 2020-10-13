﻿using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LD.Common;

namespace LDDC.DAL
{
	/// <summary>
	/// 数据访问类:OrderState
	/// </summary>
	public partial class OrderState
	{
		public OrderState()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(Guid guid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from OrderState");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = guid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(LDDC.Model.OrderState model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into OrderState(");
			strSql.Append("guid,oGuid,sID,state,creatorID,creator,createDate,isValid,sortNo)");
			strSql.Append(" values (");
			strSql.Append("@guid,@oGuid,@sID,@state,@creatorID,@creator,@createDate,@isValid,@sortNo)");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@oGuid", SqlDbType.VarChar,60),
					new SqlParameter("@sID", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.VarChar,10),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4),
					new SqlParameter("@sortNo", SqlDbType.Int,4)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = model.oGuid;
			parameters[2].Value = model.sID;
			parameters[3].Value = model.state;
			parameters[4].Value = model.creatorID;
			parameters[5].Value = model.creator;
			parameters[6].Value = model.createDate;
			parameters[7].Value = model.isValid;
			parameters[8].Value = model.sortNo;

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
		public bool Update(LDDC.Model.OrderState model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update OrderState set ");
			strSql.Append("oGuid=@oGuid,");
			strSql.Append("sID=@sID,");
			strSql.Append("state=@state,");
			strSql.Append("creatorID=@creatorID,");
			strSql.Append("creator=@creator,");
			strSql.Append("createDate=@createDate,");
			strSql.Append("isValid=@isValid,");
			strSql.Append("sortNo=@sortNo");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@oGuid", SqlDbType.VarChar,60),
					new SqlParameter("@sID", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.VarChar,10),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4),
					new SqlParameter("@sortNo", SqlDbType.Int,4),
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)};
			parameters[0].Value = model.oGuid;
			parameters[1].Value = model.sID;
			parameters[2].Value = model.state;
			parameters[3].Value = model.creatorID;
			parameters[4].Value = model.creator;
			parameters[5].Value = model.createDate;
			parameters[6].Value = model.isValid;
			parameters[7].Value = model.sortNo;
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
			strSql.Append("delete from OrderState ");
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
			strSql.Append("delete from OrderState ");
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
		public LDDC.Model.OrderState GetModel(Guid guid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 guid,oGuid,sID,state,creatorID,creator,createDate,isValid,sortNo from OrderState ");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = guid;

			LDDC.Model.OrderState model=new LDDC.Model.OrderState();
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
		public LDDC.Model.OrderState DataRowToModel(DataRow row)
		{
			LDDC.Model.OrderState model=new LDDC.Model.OrderState();
			if (row != null)
			{
				if(row["guid"]!=null && row["guid"].ToString()!="")
				{
					model.guid= new Guid(row["guid"].ToString());
				}
				if(row["oGuid"]!=null)
				{
					model.oGuid=row["oGuid"].ToString();
				}
				if(row["sID"]!=null && row["sID"].ToString()!="")
				{
					model.sID=int.Parse(row["sID"].ToString());
				}
				if(row["state"]!=null)
				{
					model.state=row["state"].ToString();
				}
				if(row["creatorID"]!=null)
				{
					model.creatorID=row["creatorID"].ToString();
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
				if(row["sortNo"]!=null && row["sortNo"].ToString()!="")
				{
					model.sortNo=int.Parse(row["sortNo"].ToString());
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
			strSql.Append("select guid,oGuid,sID,state,creatorID,creator,createDate,isValid,sortNo ");
			strSql.Append(" FROM OrderState ");
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
			strSql.Append(" guid,oGuid,sID,state,creatorID,creator,createDate,isValid,sortNo ");
			strSql.Append(" FROM OrderState ");
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
			strSql.Append("select count(1) FROM OrderState ");
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
			strSql.Append(")AS Row, T.*  from OrderState T ");
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
			parameters[0].Value = "OrderState";
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
