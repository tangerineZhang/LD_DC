using System;
namespace LDDC.Model
{
	/// <summary>
	/// OrderInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class OrderInfo
	{
		public OrderInfo()
		{}
		#region Model
		private Guid _guid;
		private string _rguid;
        private DateTime? _orderDate;
		private int? _eid;
		private string _etype;
		private DateTime? _rdate;
		private DateTime? _edate;
		private string _ruserid;
		private string _ruser;
		private int? _istakeout;
		private DateTime? _tdate;
		private string _tuserid;
		private string _tuser;
		private int? _osguid;
		private string _osstate;
        private int? _ramount=1;
		private string _creatorid;
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
        public DateTime? orderDate
        {
            set { _orderDate = value; }
            get { return _orderDate; }
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
		public DateTime? rDate
		{
			set{ _rdate=value;}
			get{return _rdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? eDate
		{
			set{ _edate=value;}
			get{return _edate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string rUserID
		{
			set{ _ruserid=value;}
			get{return _ruserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string rUser
		{
			set{ _ruser=value;}
			get{return _ruser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isTakeout
		{
			set{ _istakeout=value;}
			get{return _istakeout;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? tDate
		{
			set{ _tdate=value;}
			get{return _tdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string tUserID
		{
			set{ _tuserid=value;}
			get{return _tuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string tUser
		{
			set{ _tuser=value;}
			get{return _tuser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? osGuid
		{
			set{ _osguid=value;}
			get{return _osguid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string osState
		{
			set{ _osstate=value;}
			get{return _osstate;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int? rAmount
        {
            set { _ramount = value; }
            get { return _ramount; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string creatorID
		{
			set{ _creatorid=value;}
			get{return _creatorid;}
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

