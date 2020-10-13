using System;
namespace LDDC.Model
{
	/// <summary>
	/// HotDishes:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class HotDishes
	{
		public HotDishes()
		{}
		#region Model
		private Guid _guid;
		private string _rguid;
		private string _vguid;
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
		public string vGuid
		{
			set{ _vguid=value;}
			get{return _vguid;}
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

