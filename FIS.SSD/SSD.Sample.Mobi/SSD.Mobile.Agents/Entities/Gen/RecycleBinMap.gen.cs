/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: RecycleBinMap.cs 				   	     
File Description 	: RecycleBinMap is the corresponding data object to tblRecycleBinMap data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using SSD.Framework;

namespace SSD.Mobile.Entities
{
	public partial class RecycleBinMap :BaseJsonEntity<RecycleBinMap>
	{
		public object Clone()
        {
            return this.MemberwiseClone();
        }
		public const string Table_TblRecycleBinMap="tblRecycleBinMap";
		#region Column Names
		
		public const string FIELD_ID="ID";
		public const string FIELD_BizID="BizID";
		public const string FIELD_BizName="BizName";
		public const string FIELD_ViewAction="ViewAction";
		#endregion
		
#region Members
protected int iD;
protected string bizID;
protected string bizName;
protected string viewAction;
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
/// Property relating to database column BizID(varchar(500),not null)
/// </summary>
public string BizID
{
	get { return bizID; }
	set { bizID = value; }
}

/// <summary>
/// Property relating to database column BizName(nvarchar(200),not null)
/// </summary>
public string BizName
{
	get { return bizName; }
	set { bizName = value; }
}

/// <summary>
/// Property relating to database column ViewAction(varchar(500),null)
/// </summary>
public string ViewAction
{
	get { return viewAction; }
	set { viewAction = value; }
}
#endregion
		
     }
}
