using System;
namespace LDDC.Model
{
	/// <summary>
	/// zd_VegetableType:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class zd_VegetableType
	{
		public zd_VegetableType()
		{}
		#region Model
		private int _id;
		private string _type;
		private string _creatorid;
		private string _creator;
		private DateTime? _createdate= DateTime.Now;
		private int? _isvalid=1;
		private int? _sortno;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
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
		/// <summary>
		/// 
		/// </summary>
		public int? sortNo
		{
			set{ _sortno=value;}
			get{return _sortno;}
		}
		#endregion Model

	}
}

