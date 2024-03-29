/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: NhapHangCT.cs 				   	     
File Description 	: NhapHangCT is the corresponding data object to tblNhapHangCT data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using SSD.Framework;

namespace SSD.Mobile.Entities
{
	public partial class NhapHangCT :BaseJsonEntity<NhapHangCT>
	{
		public object Clone()
        {
            return this.MemberwiseClone();
        }
		public const string Table_TblNhapHangCT="tblNhapHangCT";
		#region Column Names
		
		public const string FIELD_OnID="OnID";
		public const string FIELD_SKU="SKU";
		public const string FIELD_Ten="Ten";
		public const string FIELD_DVT="DVT";
		public const string FIELD_GiaNhap="GiaNhap";
		public const string FIELD_GiaVon="GiaVon";
		public const string FIELD_SoLuong="SoLuong";
		public const string FIELD_ChietKhau="ChietKhau";
		public const string FIELD_VAT="VAT";
		public const string FIELD_GhiChu="GhiChu";
		public const string FIELD_NhapHangID="NhapHangID";
		public const string FIELD_LocationStoreID="LocationStoreID";
		public const string FIELD_TonDau="TonDau";
		#endregion
		
#region Members
protected long onID;
protected string sKU;
protected string ten;
protected string dVT;
protected decimal giaNhap;
protected decimal giaVon;
protected long soLuong;
protected decimal chietKhau;
protected int vAT;
protected string ghiChu;
protected int nhapHangID;
protected int locationStoreID;
protected long tonDau;
#endregion
#region Public Properties

/// <summary>
/// Property relating to database column OnID(bigint,not null)
/// </summary>
public long OnID
{
	get { return onID; }
	set { onID = value; }
}

/// <summary>
/// Property relating to database column SKU(varchar(20),not null)
/// </summary>
public string SKU
{
	get { return sKU; }
	set { sKU = value; }
}

/// <summary>
/// Property relating to database column Ten(nvarchar(255),not null)
/// </summary>
public string Ten
{
	get { return ten; }
	set { ten = value; }
}

/// <summary>
/// Property relating to database column DVT(nvarchar(20),not null)
/// </summary>
public string DVT
{
	get { return dVT; }
	set { dVT = value; }
}

/// <summary>
/// Property relating to database column GiaNhap(money,not null)
/// </summary>
public decimal GiaNhap
{
	get { return giaNhap; }
	set { giaNhap = value; }
}

/// <summary>
/// Property relating to database column GiaVon(money,not null)
/// </summary>
public decimal GiaVon
{
	get { return giaVon; }
	set { giaVon = value; }
}

/// <summary>
/// Property relating to database column SoLuong(bigint,null)
/// </summary>
public long SoLuong
{
	get { return soLuong; }
	set { soLuong = value; }
}

/// <summary>
/// Property relating to database column ChietKhau(money,null)
/// </summary>
public decimal ChietKhau
{
	get { return chietKhau; }
	set { chietKhau = value; }
}

/// <summary>
/// Property relating to database column VAT(int,null)
/// </summary>
public int VAT
{
	get { return vAT; }
	set { vAT = value; }
}

/// <summary>
/// Property relating to database column GhiChu(nvarchar(255),null)
/// </summary>
public string GhiChu
{
	get { return ghiChu; }
	set { ghiChu = value; }
}

/// <summary>
/// Property relating to database column NhapHangID(int,not null)
/// </summary>
public int NhapHangID
{
	get { return nhapHangID; }
	set { nhapHangID = value; }
}
public virtual NhapHang NhapHang{get;set;}

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
/// Property relating to database column TonDau(bigint,null)
/// </summary>
public long TonDau
{
	get { return tonDau; }
	set { tonDau = value; }
}
#endregion
		
     }
}
