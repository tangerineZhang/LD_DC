using System;
namespace LDDC.Model
{
	/// <summary>
	/// PagePermission:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class PagePermission
	{
		public PagePermission()
		{}
		#region Model
		private Guid _guid;
		private string _userid;
		private string _user;
		private int? _pid;
		private string _permission;
		private string _creatorguid;
		private string _creator;
		private DateTime? _createdate= DateTime.Now;
		private int? _isvalid=1;
		/// <summary>
		/// 
		/// </summary>
		public Guid guid
		{
			set{ _guid=value;}
			get{return _guid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string userID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string user
		{
			set{ _user=value;}
			get{return _user;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? pID
		{
			set{ _pid=value;}
			get{return _pid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Permission
		{
			set{ _permission=value;}
			get{return _permission;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string creatorguid
		{
			set{ _creatorguid=value;}
			get{return _creatorguid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string creator
		{
			set{ _creator=value;}
			get{return _creator;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createDate
		{
			set{ _createdate=value;}
			get{return _createdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isValid
		{
			set{ _isvalid=value;}
			get{return _isvalid;}
		}
		#endregion Model

	}
}

