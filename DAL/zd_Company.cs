using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
//using Maticsoft.DBUtility;//Please add references
using LD.Common;
using Common;

namespace LDDC.DAL
{
	/// <summary>
	/// 数据访问类:zd_Company
	/// </summary>
	public partial class zd_Company
	{
		public zd_Company()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(Guid guid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from zd_Company");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = guid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LDDC.Model.zd_Company model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into zd_Company(");
			strSql.Append("guid,name,creatorID,creator,createDate,isValid)");
			strSql.Append(" values (");
			strSql.Append("@guid,@name,@creatorID,@creator,@createDate,@isValid)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@name", SqlDbType.VarChar,30),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = model.name;
			parameters[2].Value = model.creatorID;
			parameters[3].Value = model.creator;
			parameters[4].Value = model.createDate;
			parameters[5].Value = model.isValid;

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
		public bool Update(LDDC.Model.zd_Company model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update zd_Company set ");
			strSql.Append("name=@name,");
			strSql.Append("creatorID=@creatorID,");
			strSql.Append("creator=@creator,");
			strSql.Append("createDate=@createDate,");
			strSql.Append("isValid=@isValid");
			strSql.Append(" where sortNo=@sortNo");
			SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.VarChar,30),
					new SqlParameter("@creatorID", SqlDbType.VarChar,30),
					new SqlParameter("@creator", SqlDbType.VarChar,10),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4),
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@sortNo", SqlDbType.Int,4)};
			parameters[0].Value = model.name;
			parameters[1].Value = model.creatorID;
			parameters[2].Value = model.creator;
			parameters[3].Value = model.createDate;
			parameters[4].Value = model.isValid;
			parameters[5].Value = model.guid;
			parameters[6].Value = model.sortNo;

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
		public bool Delete(int sortNo)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from zd_Company ");
			strSql.Append(" where sortNo=@sortNo");
			SqlParameter[] parameters = {
					new SqlParameter("@sortNo", SqlDbType.Int,4)
			};
			parameters[0].Value = sortNo;

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
			strSql.Append("delete from zd_Company ");
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
		public bool DeleteList(string sortNolist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from zd_Company ");
			strSql.Append(" where sortNo in ("+sortNolist + ")  ");
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
		public LDDC.Model.zd_Company GetModel(int sortNo)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 guid,name,creatorID,creator,createDate,isValid,sortNo from zd_Company ");
			strSql.Append(" where sortNo=@sortNo");
			SqlParameter[] parameters = {
					new SqlParameter("@sortNo", SqlDbType.Int,4)
			};
			parameters[0].Value = sortNo;

			LDDC.Model.zd_Company model=new LDDC.Model.zd_Company();
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
		public LDDC.Model.zd_Company DataRowToModel(DataRow row)
		{
			LDDC.Model.zd_Company model=new LDDC.Model.zd_Company();
			if (row != null)
			{
				if(row["guid"]!=null && row["guid"].ToString()!="")
				{
					model.guid= new Guid(row["guid"].ToString());
				}
				if(row["name"]!=null)
				{
					model.name=row["name"].ToString();
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
			strSql.Append("select guid,name,creatorID,creator,createDate,isValid,sortNo ");
			strSql.Append(" FROM zd_Company ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}


        /// <summary>
        /// 获得个人所在公司
        /// </summary>
        public DataSet GetoneList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CASE WHEN ISNULL(u.F3,'')<>'' THEN u.F3 ELSE u.UserCode END as NameNum, ");
            strSql.Append(" CASE WHEN ISNULL(substring(substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3)),0,charindex('/',substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3)))),'')<>'' THEN  ");
            strSql.Append("substring(substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3)),0,charindex('/',substring(o.F3,charindex('/',o.F3)+1,len(o.F3)-charindex('/',o.F3))))  ");
            strSql.Append(" else substring(o.F3,charindex('/',o.F3)+1,len(o.F3))");
            strSql.Append("END as CompanyName,");
            strSql.Append(" u.UserName as UserName,u.UserLoginID as UserId,u.GenderText as Sex,u.MobilePhone as Phone,u.Email as Emile ");
            strSql.Append("from dbo.MDM_User u ");
            strSql.Append(" inner join dbo.MDM_User_Position_Link up on up.UserGUID=u.UserID and up.IsMainPosition=1 and up.Status=1 ");
            strSql.Append("inner join dbo.MDM_PostOrganization_Link po on po.PositionGuid=up.PositionGUID and po.Status=1 ");
            strSql.Append(" inner join dbo.MDM_OrganizationUnit o on o.OrgUnitGUID = po.OrgUnitGUID and o.Status=1 ");
            strSql.Append(" left join dbo.MDM_OrganizationUnit q on q.OrgUnitGUID = po.OrgUnitGUID and o.Status=1 and q.CompanyType=102");
            strSql.Append("left join dbo.MDM_Position as p on p.PositionGUID=po.PositionGuid  ");
            //strSql.Append(" where u.Status=1 and u.UserLoginID = '{0}' ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where u.Status=1 and  " + strWhere);
            }
            return DBHelpers160.DbHelperSQL.Query(strSql.ToString());
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
			strSql.Append(" guid,name,creatorID,creator,createDate,isValid,sortNo ");
			strSql.Append(" FROM zd_Company ");
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
			strSql.Append("select count(1) FROM zd_Company ");
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
				strSql.Append("order by T.sortNo desc");
			}
			strSql.Append(")AS Row, T.*  from zd_Company T ");
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
			parameters[0].Value = "zd_Company";
			parameters[1].Value = "sortNo";
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

