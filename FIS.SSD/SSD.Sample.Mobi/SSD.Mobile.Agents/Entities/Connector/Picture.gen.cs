using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SSD.Mobile.Entities
{
	public partial class Picture 
	{
		#region Column Names
		
		public const string FIELD_Id="Id";
		public const string FIELD_PictureBinary="PictureBinary";
		public const string FIELD_MimeType="MimeType";
		public const string FIELD_SeoFilename="SeoFilename";
		public const string FIELD_IsNew="IsNew";
		#endregion
		
#region Members
protected int id;
protected byte[] pictureBinary;
protected string mimeType;
protected string seoFilename;
protected bool isNew;
#endregion
#region Public Properties

/// <summary>
/// Property relating to database column Id(int,not null)
/// </summary>
public int Id
{
	get { return id; }
	set { id = value; }
}

/// <summary>
/// Property relating to database column PictureBinary(varbinary(MAX),null)
/// </summary>
public byte[] PictureBinary
{
	get { return pictureBinary; }
	set { pictureBinary = value; }
}

/// <summary>
/// Property relating to database column MimeType(nvarchar(40),not null)
/// </summary>
public string MimeType
{
	get { return mimeType; }
	set { mimeType = value; }
}

/// <summary>
/// Property relating to database column SeoFilename(nvarchar(300),null)
/// </summary>
public string SeoFilename
{
	get { return seoFilename; }
	set { seoFilename = value; }
}

/// <summary>
/// Property relating to database column IsNew(bit,not null)
/// </summary>
public bool IsNew
{
	get { return isNew; }
	set { isNew = value; }
}
#endregion
     }
}
