/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: IPAccess.cs 				   	     
File Description 	: IPAccess is the corresponding data object to tblIPAccess data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using SSD.Framework;

namespace SSD.Mobile.Entities
{
	public partial class IPAccess :BaseJsonEntity<IPAccess>
	{
		public object Clone()
        {
            return this.MemberwiseClone();
        }
		public const string Table_TblIPAccess="tblIPAccess";
		#region Column Names
		
		public const string FIELD_ID="ID";
		public const string FIELD_UserName="UserName";
		public const string FIELD_IPAddress="IPAddress";
		public const string FIELD_TimeAccess="TimeAccess";
		#endregion
		
#region Members
protected int iD;
protected string userName;
protected string iPAddress;
protected DateTime timeAccess;
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
/// Property relating to database column UserName(varchar(50),not null)
/// </summary>
public string UserName
{
	get { return userName; }
	set { userName = value; }
}

/// <summary>
/// Property relating to database column IPAddress(varchar(50),not null)
/// </summary>
public string IPAddress
{
	get { return iPAddress; }
	set { iPAddress = value; }
}

/// <summary>
/// Property relating to database column TimeAccess(datetime,not null)
/// </summary>
public DateTime TimeAccess
{
	get { return timeAccess; }
	set { timeAccess = value; }
}
#endregion
		
     }
}
