using System;
namespace LDDC.Model
{
	/// <summary>
	/// TicketInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class TicketInfo
	{
		public TicketInfo()
		{}
		#region Model
		private Guid _guid;
		private string _rguid;
		private DateTime? _orderdate;
		private int? _eid;
		private string _etype;
		private string _rrgb1;
		private string _rrgb2;
        private string _rrgb3;
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
		public DateTime? orderDate
		{
			set{ _orderdate=value;}
			get{return _orderdate;}
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
		public string rRgb1
		{
			set{ _rrgb1=value;}
			get{return _rrgb1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string rRgb2
		{
			set{ _rrgb2=value;}
			get{return _rrgb2;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string rRgb3
        {
            set { _rrgb3 = value; }
            get { return _rrgb3; }
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

