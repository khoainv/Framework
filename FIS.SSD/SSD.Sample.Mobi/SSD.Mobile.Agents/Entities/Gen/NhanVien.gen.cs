/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: NhanVien.cs 				   	     
File Description 	: NhanVien is the corresponding data object to tblNhanVien data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using SSD.Framework;

namespace SSD.Mobile.Entities
{
	public partial class NhanVien :BaseJsonEntity<NhanVien>
	{
		public object Clone()
        {
            return this.MemberwiseClone();
        }
		public const string Table_TblNhanVien="tblNhanVien";
		#region Column Names
		
		public const string FIELD_LocationStoreID="LocationStoreID";
		public const string FIELD_ID="ID";
		public const string FIELD_TenNV="TenNV";
		public const string FIELD_UserName="UserName";
		public const string FIELD_DiaChi="DiaChi";
		public const string FIELD_DienThoai="DienThoai";
		public const string FIELD_SoCMT="SoCMT";
		public const string FIELD_GhiChu="GhiChu";
		public const string FIELD_IsDeleted="IsDeleted";
		public const string FIELD_CreatedBy="CreatedBy";
		public const string FIELD_CreatedAt="CreatedAt";
		public const string FIELD_ModifiedBy="ModifiedBy";
		public const string FIELD_ModifiedAt="ModifiedAt";
		public const string FIELD_SyncDate="SyncDate";
		#endregion
		
#region Members
protected int locationStoreID;
protected int iD;
protected string tenNV;
protected string userName;
protected string diaChi;
protected string dienThoai;
protected string soCMT;
protected string ghiChu;
protected bool isDeleted;
protected string createdBy;
protected DateTime createdAt;
protected string modifiedBy;
protected DateTime modifiedAt;
protected DateTime syncDate;
#endregion
#region Public Properties

/// <summary>
/// Property relating to database column LocationStoreID(int,not null)
/// </summary>
public int LocationStoreID
{
	get { return locationStoreID; }
	set { locationStoreID = value; }
}
public virtual ComLocationStore ComLocationStore{get;set;}

/// <summary>
/// Property relating to database column ID(int,not null)
/// </summary>
public int ID
{
	get { return iD; }
	set { iD = value; }
}

/// <summary>
/// Property relating to database column TenNV(nvarchar(50),null)
/// </summary>
public string TenNV
{
	get { return tenNV; }
	set { tenNV = value; }
}

/// <summary>
/// Property relating to database column UserName(nvarchar(200),null)
/// </summary>
public string UserName
{
	get { return userName; }
	set { userName = value; }
}

/// <summary>
/// Property relating to database column DiaChi(nvarchar(500),null)
/// </summary>
public string DiaChi
{
	get { return diaChi; }
	set { diaChi = value; }
}

/// <summary>
/// Property relating to database column DienThoai(varchar(15),null)
/// </summary>
public string DienThoai
{
	get { return dienThoai; }
	set { dienThoai = value; }
}

/// <summary>
/// Property relating to database column SoCMT(varchar(15),null)
/// </summary>
public string SoCMT
{
	get { return soCMT; }
	set { soCMT = value; }
}

/// <summary>
/// Property relating to database column GhiChu(nvarchar(500),null)
/// </summary>
public string GhiChu
{
	get { return ghiChu; }
	set { ghiChu = value; }
}

/// <summary>
/// Property relating to database column IsDeleted(bit,null)
/// </summary>
public bool IsDeleted
{
	get { return isDeleted; }
	set { isDeleted = value; }
}

/// <summary>
/// Property relating to database column CreatedBy(nvarchar(100),not null)
/// </summary>
public string CreatedBy
{
	get { return createdBy; }
	set { createdBy = value; }
}

/// <summary>
/// Property relating to database column CreatedAt(datetime,not null)
/// </summary>
public DateTime CreatedAt
{
	get { return createdAt; }
	set { createdAt = value; }
}

/// <summary>
/// Property relating to database column ModifiedBy(nvarchar(100),not null)
/// </summary>
public string ModifiedBy
{
	get { return modifiedBy; }
	set { modifiedBy = value; }
}

/// <summary>
/// Property relating to database column ModifiedAt(datetime,not null)
/// </summary>
public DateTime ModifiedAt
{
	get { return modifiedAt; }
	set { modifiedAt = value; }
}

/// <summary>
/// Property relating to database column SyncDate(datetime,not null)
/// </summary>
public DateTime SyncDate
{
	get { return syncDate; }
	set { syncDate = value; }
}
#endregion
		
     }
}
