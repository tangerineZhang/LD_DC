using System;
namespace LDDC.Model
{
	/// <summary>
	/// QRCode:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class QRCode
	{
		public QRCode()
		{}
		#region Model
		private Guid _guid;
		private string _rguid;
		private string _code;
		private DateTime? _createdate= DateTime.Now;
		private int? _isvalid;
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
		public string rGuid
		{
			set{ _rguid=value;}
			get{return _rguid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string code
		{
			set{ _code=value;}
			get{return _code;}
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

