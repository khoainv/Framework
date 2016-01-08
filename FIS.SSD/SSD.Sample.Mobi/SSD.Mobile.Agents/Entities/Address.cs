/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: Address.cs 				   	     
File Description 	: Address is the corresponding data object to Address data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;

namespace SSD.Mobile.Entities
{
	public partial class Address 
	{
		#region Column Names
		
		public const string FIELD_Id="Id";
		public const string FIELD_FirstName="FirstName";
		public const string FIELD_LastName="LastName";
		public const string FIELD_Email="Email";
		public const string FIELD_Company="Company";
		public const string FIELD_CountryId="CountryId";
		public const string FIELD_StateProvinceId="StateProvinceId";
		public const string FIELD_City="City";
		public const string FIELD_Address1="Address1";
		public const string FIELD_Address2="Address2";
		public const string FIELD_ZipPostalCode="ZipPostalCode";
		public const string FIELD_PhoneNumber="PhoneNumber";
		public const string FIELD_FaxNumber="FaxNumber";
		public const string FIELD_CreatedOnUtc="CreatedOnUtc";
		#endregion
		
#region Members
protected int id;
protected string firstName;
protected string lastName;
protected string email;
protected string company;
protected int countryId;
protected int stateProvinceId;
protected string city;
protected string address1;
protected string address2;
protected string zipPostalCode;
protected string phoneNumber;
protected string faxNumber;
protected DateTime createdOnUtc;
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
/// Property relating to database column FirstName(nvarchar(MAX),null)
/// </summary>
public string FirstName
{
	get { return firstName; }
	set { firstName = value; }
}

/// <summary>
/// Property relating to database column LastName(nvarchar(MAX),null)
/// </summary>
public string LastName
{
	get { return lastName; }
	set { lastName = value; }
}

/// <summary>
/// Property relating to database column Email(nvarchar(MAX),null)
/// </summary>
public string Email
{
	get { return email; }
	set { email = value; }
}

/// <summary>
/// Property relating to database column Company(nvarchar(MAX),null)
/// </summary>
public string Company
{
	get { return company; }
	set { company = value; }
}

/// <summary>
/// Property relating to database column CountryId(int,null)
/// </summary>
public int CountryId
{
	get { return countryId; }
	set { countryId = value; }
}

/// <summary>
/// Property relating to database column StateProvinceId(int,null)
/// </summary>
public int StateProvinceId
{
	get { return stateProvinceId; }
	set { stateProvinceId = value; }
}

/// <summary>
/// Property relating to database column City(nvarchar(MAX),null)
/// </summary>
public string City
{
	get { return city; }
	set { city = value; }
}

/// <summary>
/// Property relating to database column Address1(nvarchar(MAX),null)
/// </summary>
public string Address1
{
	get { return address1; }
	set { address1 = value; }
}

/// <summary>
/// Property relating to database column Address2(nvarchar(MAX),null)
/// </summary>
public string Address2
{
	get { return address2; }
	set { address2 = value; }
}

/// <summary>
/// Property relating to database column ZipPostalCode(nvarchar(MAX),null)
/// </summary>
public string ZipPostalCode
{
	get { return zipPostalCode; }
	set { zipPostalCode = value; }
}

/// <summary>
/// Property relating to database column PhoneNumber(nvarchar(MAX),null)
/// </summary>
public string PhoneNumber
{
	get { return phoneNumber; }
	set { phoneNumber = value; }
}

/// <summary>
/// Property relating to database column FaxNumber(nvarchar(MAX),null)
/// </summary>
public string FaxNumber
{
	get { return faxNumber; }
	set { faxNumber = value; }
}

/// <summary>
/// Property relating to database column CreatedOnUtc(datetime,not null)
/// </summary>
public DateTime CreatedOnUtc
{
	get { return createdOnUtc; }
	set { createdOnUtc = value; }
}
#endregion
		
     }
}
