using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
//using Maticsoft.DBUtility;//Please add references
using LD.Common;

namespace LDDC.DAL
{
	/// <summary>
	/// 数据访问类:DishesImage
	/// </summary>
	public partial class DishesImage
	{
		public DishesImage()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(Guid guid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from DishesImage");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = guid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(LDDC.Model.DishesImage model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into DishesImage(");
			strSql.Append("guid,vGuid,imgPath,format,imgName,size,isCoverphoto,createDate,isValid,sortNo)");
			strSql.Append(" values (");
			strSql.Append("@guid,@vGuid,@imgPath,@format,@imgName,@size,@isCoverphoto,@createDate,@isValid,@sortNo)");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@vGuid", SqlDbType.VarChar,60),
					new SqlParameter("@imgPath", SqlDbType.VarChar,200),
					new SqlParameter("@format", SqlDbType.VarChar,12),
					new SqlParameter("@imgName", SqlDbType.VarChar,100),
					new SqlParameter("@size", SqlDbType.Decimal,9),
					new SqlParameter("@isCoverphoto", SqlDbType.Int,4),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4),
					new SqlParameter("@sortNo", SqlDbType.Int,4)};
			parameters[0].Value = Guid.NewGuid();
			parameters[1].Value = model.vGuid;
			parameters[2].Value = model.imgPath;
			parameters[3].Value = model.format;
			parameters[4].Value = model.imgName;
			parameters[5].Value = model.size;
			parameters[6].Value = model.isCoverphoto;
			parameters[7].Value = model.createDate;
			parameters[8].Value = model.isValid;
			parameters[9].Value = model.sortNo;

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
		public bool Update(LDDC.Model.DishesImage model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update DishesImage set ");
			strSql.Append("vGuid=@vGuid,");
			strSql.Append("imgPath=@imgPath,");
			strSql.Append("format=@format,");
			strSql.Append("imgName=@imgName,");
			strSql.Append("size=@size,");
			strSql.Append("isCoverphoto=@isCoverphoto,");
			strSql.Append("createDate=@createDate,");
			strSql.Append("isValid=@isValid,");
			strSql.Append("sortNo=@sortNo");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@vGuid", SqlDbType.VarChar,60),
					new SqlParameter("@imgPath", SqlDbType.VarChar,200),
					new SqlParameter("@format", SqlDbType.VarChar,12),
					new SqlParameter("@imgName", SqlDbType.VarChar,100),
					new SqlParameter("@size", SqlDbType.Decimal,9),
					new SqlParameter("@isCoverphoto", SqlDbType.Int,4),
					new SqlParameter("@createDate", SqlDbType.DateTime),
					new SqlParameter("@isValid", SqlDbType.Int,4),
					new SqlParameter("@sortNo", SqlDbType.Int,4),
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)};
			parameters[0].Value = model.vGuid;
			parameters[1].Value = model.imgPath;
			parameters[2].Value = model.format;
			parameters[3].Value = model.imgName;
			parameters[4].Value = model.size;
			parameters[5].Value = model.isCoverphoto;
			parameters[6].Value = model.createDate;
			parameters[7].Value = model.isValid;
			parameters[8].Value = model.sortNo;
			parameters[9].Value = model.guid;

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
			strSql.Append("delete from DishesImage ");
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
			strSql.Append("delete from DishesImage ");
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
		public LDDC.Model.DishesImage GetModel(Guid guid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 guid,vGuid,imgPath,format,imgName,size,isCoverphoto,createDate,isValid,sortNo from DishesImage ");
			strSql.Append(" where guid=@guid ");
			SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.UniqueIdentifier,16)			};
			parameters[0].Value = guid;

			LDDC.Model.DishesImage model=new LDDC.Model.DishesImage();
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
		public LDDC.Model.DishesImage DataRowToModel(DataRow row)
		{
			LDDC.Model.DishesImage model=new LDDC.Model.DishesImage();
			if (row != null)
			{
				if(row["guid"]!=null && row["guid"].ToString()!="")
				{
					model.guid= new Guid(row["guid"].ToString());
				}
				if(row["vGuid"]!=null)
				{
					model.vGuid=row["vGuid"].ToString();
				}
				if(row["imgPath"]!=null)
				{
					model.imgPath=row["imgPath"].ToString();
				}
				if(row["format"]!=null)
				{
					model.format=row["format"].ToString();
				}
				if(row["imgName"]!=null)
				{
					model.imgName=row["imgName"].ToString();
				}
				if(row["size"]!=null && row["size"].ToString()!="")
				{
					model.size=decimal.Parse(row["size"].ToString());
				}
				if(row["isCoverphoto"]!=null && row["isCoverphoto"].ToString()!="")
				{
					model.isCoverphoto=int.Parse(row["isCoverphoto"].ToString());
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
			strSql.Append("select guid,vGuid,imgPath,format,imgName,size,isCoverphoto,createDate,isValid,sortNo ");
			strSql.Append(" FROM DishesImage ");
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
			strSql.Append(" guid,vGuid,imgPath,format,imgName,size,isCoverphoto,createDate,isValid,sortNo ");
			strSql.Append(" FROM DishesImage ");
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
			strSql.Append("select count(1) FROM DishesImage ");
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
			strSql.Append(")AS Row, T.*  from DishesImage T ");
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
			parameters[0].Value = "DishesImage";
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

