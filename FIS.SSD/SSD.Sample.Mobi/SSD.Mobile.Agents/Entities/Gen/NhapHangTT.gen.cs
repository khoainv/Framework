/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: NhapHangTT.cs 				   	     
File Description 	: NhapHangTT is the corresponding data object to tblNhapHangTT data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using SSD.Framework;

namespace SSD.Mobile.Entities
{
	public partial class NhapHangTT :BaseJsonEntity<NhapHangTT>
	{
		public object Clone()
        {
            return this.MemberwiseClone();
        }
		public const string Table_TblNhapHangTT="tblNhapHangTT";
		#region Column Names
		
		public const string FIELD_ID="ID";
		public const string FIELD_LocationStoreID="LocationStoreID";
		public const string FIELD_NhapHangID="NhapHangID";
		public const string FIELD_NgayTT="NgayTT";
		public const string FIELD_SoTien="SoTien";
		public const string FIELD_GhiChu="GhiChu";
		#endregion
		
#region Members
protected int iD;
protected int locationStoreID;
protected int nhapHangID;
protected DateTime ngayTT;
protected decimal soTien;
protected string ghiChu;
#endregion
#region Public Properties

/// <summary>
/// Property relating to database column ID(int,not null)
/// </summary>
public int ID
{
	get { return iD; }
	set { iD = value; }
}

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
/// Property relating to database column NhapHangID(int,not null)
/// </summary>
public int NhapHangID
{
	get { return nhapHangID; }
	set { nhapHangID = value; }
}
public virtual NhapHang NhapHang{get;set;}

/// <summary>
/// Property relating to database column NgayTT(date,not null)
/// </summary>
public DateTime NgayTT
{
	get { return ngayTT; }
	set { ngayTT = value; }
}

/// <summary>
/// Property relating to database column SoTien(money,not null)
/// </summary>
public decimal SoTien
{
	get { return soTien; }
	set { soTien = value; }
}

/// <summary>
/// Property relating to database column GhiChu(nvarchar(250),null)
/// </summary>
public string GhiChu
{
	get { return ghiChu; }
	set { ghiChu = value; }
}
#endregion
		
     }
}
