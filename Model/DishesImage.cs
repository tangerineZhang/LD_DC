using System;
namespace LDDC.Model
{
	/// <summary>
	/// DishesImage:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class DishesImage
	{
		public DishesImage()
		{}
		#region Model
		private Guid _guid;
		private string _vguid;
		private string _imgpath;
		private string _format;
		private string _imgname;
		private decimal? _size;
		private int? _iscoverphoto;
		private DateTime? _createdate= DateTime.Now;
		private int? _isvalid=1;
		private int? _sortno;
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
		public string vGuid
		{
			set{ _vguid=value;}
			get{return _vguid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string imgPath
		{
			set{ _imgpath=value;}
			get{return _imgpath;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string format
		{
			set{ _format=value;}
			get{return _format;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string imgName
		{
			set{ _imgname=value;}
			get{return _imgname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? size
		{
			set{ _size=value;}
			get{return _size;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isCoverphoto
		{
			set{ _iscoverphoto=value;}
			get{return _iscoverphoto;}
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

