using System;
namespace LDDC.Model
{
	/// <summary>
	/// SetMeal:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SetMeal
	{
		public SetMeal()
		{}
		#region Model
		private Guid _guid;
		private string _rguid;
		private int _eguid;
		private string _etype;
		private DateTime? _begdate;
		private DateTime? _enddate;
		private string _creatorguid;
		private string _creator;
		private DateTime? _createdate;
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
		public int eGuid
		{
			set{ _eguid=value;}
			get{return _eguid;}
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
		public DateTime? begDate
		{
			set{ _begdate=value;}
			get{return _begdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? endDate
		{
			set{ _enddate=value;}
			get{return _enddate;}
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

