using System;
namespace LDDC.Model
{
	/// <summary>
	/// Dinnertime:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Dinnertime
	{
		public Dinnertime()
		{}
		#region Model
		private Guid _guid;
		private string _rguid;
		private int? _eid;
		private string _etype;
		private DateTime? _ebegtime;
		private DateTime? _eendtime;
		private DateTime? _rbegtime;
		private DateTime? _rendtime;
		private int? _rwhichday;
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
		public string rGuid
		{
			set{ _rguid=value;}
			get{return _rguid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? eID
		{
			set{ _eid=value;}
			get{return _eid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string eType
		{
			set{ _etype=value;}
			get{return _etype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? eBegTime
		{
			set{ _ebegtime=value;}
			get{return _ebegtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? eEndTime
		{
			set{ _eendtime=value;}
			get{return _eendtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? rBegTime
		{
			set{ _rbegtime=value;}
			get{return _rbegtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? rEndTime
		{
			set{ _rendtime=value;}
			get{return _rendtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? rWhichDay
		{
			set{ _rwhichday=value;}
			get{return _rwhichday;}
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

