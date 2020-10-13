﻿using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
//using Maticsoft.DBUtility;//Please add references
using LD.Common;

namespace LDDC.DAL
{
	/// <summary>
	/// 数据访问类:zd_DinnerType
	/// </summary>
	public partial class zd_DinnerType
	{
		public zd_DinnerType()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "zd_DinnerType"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from zd_DinnerType");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LDDC.Model.zd_DinnerType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into zd_DinnerType(");
			strSql.Append("type,creatorID,creator,createDate,isValid,sortNo)");
			strSql.Append(" values (");
			strSql.Append("@type,@creatorID,@creator,@createDate,@isValid,@sortNo)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@type", SqlDbType.VarChar,30),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4),
					new SqlParameter("@sortNo", SqlDbType.Int,4)};
			parameters[0].Value = model.type;
			parameters[1].Value = model.creatorID;
			parameters[2].Value = model.creator;
			parameters[3].Value = model.createDate;
			parameters[4].Value = model.isValid;
			parameters[5].Value = model.sortNo;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
		/// 更新一条数据
		/// </summary>
		public bool Update(LDDC.Model.zd_DinnerType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update zd_DinnerType set ");
			strSql.Append("type=@type,");
			strSql.Append("creatorID=@creatorID,");
			strSql.Append("creator=@creator,");
			strSql.Append("createDate=@createDate,");
			strSql.Append("isValid=@isValid,");
			strSql.Append("sortNo=@sortNo");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@type", SqlDbType.VarChar,30),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4),
					new SqlParameter("@sortNo", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.type;
			parameters[1].Value = model.creatorID;
			parameters[2].Value = model.creator;
			parameters[3].Value = model.createDate;
			parameters[4].Value = model.isValid;
			parameters[5].Value = model.sortNo;
			parameters[6].Value = model.id;

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
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from zd_DinnerType ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from zd_DinnerType ");
			strSql.Append(" where id in ("+idlist + ")  ");
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
		public LDDC.Model.zd_DinnerType GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,type,creatorID,creator,createDate,isValid,sortNo from zd_DinnerType ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			LDDC.Model.zd_DinnerType model=new LDDC.Model.zd_DinnerType();
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
		public LDDC.Model.zd_DinnerType DataRowToModel(DataRow row)
		{
			LDDC.Model.zd_DinnerType model=new LDDC.Model.zd_DinnerType();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["type"]!=null)
				{
					model.type=row["type"].ToString();
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
			strSql.Append("select id,type,creatorID,creator,createDate,isValid,sortNo ");
			strSql.Append(" FROM zd_DinnerType ");
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
			strSql.Append(" id,type,creatorID,creator,createDate,isValid,sortNo ");
			strSql.Append(" FROM zd_DinnerType ");
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
			strSql.Append("select count(1) FROM zd_DinnerType ");
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
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from zd_DinnerType T ");
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
			parameters[0].Value = "zd_DinnerType";
			parameters[1].Value = "id";
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

