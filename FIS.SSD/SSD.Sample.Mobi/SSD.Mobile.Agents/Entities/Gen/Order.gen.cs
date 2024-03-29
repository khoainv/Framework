/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: Order.cs 				   	     
File Description 	: Order is the corresponding data object to tblOrder data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using SSD.Framework;

namespace SSD.Mobile.Entities
{
	public partial class Order :BaseJsonEntity<Order>
	{
		public object Clone()
        {
            return this.MemberwiseClone();
        }
		public const string Table_TblOrder="tblOrder";
		#region Column Names
		
		public const string FIELD_Id="Id";
		public const string FIELD_Account="Account";
		public const string FIELD_BillingAddressId="BillingAddressId";
		public const string FIELD_ShippingAddressId="ShippingAddressId";
		public const string FIELD_OrderStatusId="OrderStatusId";
		public const string FIELD_ShippingStatusId="ShippingStatusId";
		public const string FIELD_PaymentStatusId="PaymentStatusId";
		public const string FIELD_OrderDiscount="OrderDiscount";
		public const string FIELD_OrderTotal="OrderTotal";
		public const string FIELD_Deleted="Deleted";
		public const string FIELD_CreatedOnUtc="CreatedOnUtc";
		#endregion
		
#region Members
protected int id;
protected string account;
protected int billingAddressId;
protected int shippingAddressId;
protected int orderStatusId;
protected int shippingStatusId;
protected int paymentStatusId;
protected decimal orderDiscount;
protected decimal orderTotal;
protected bool deleted;
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
/// Property relating to database column Account(varchar(20),not null)
/// </summary>
public string Account
{
	get { return account; }
	set { account = value; }
}

/// <summary>
/// Property relating to database column BillingAddressId(int,not null)
/// </summary>
public int BillingAddressId
{
	get { return billingAddressId; }
	set { billingAddressId = value; }
}

/// <summary>
/// Property relating to database column ShippingAddressId(int,null)
/// </summary>
public int ShippingAddressId
{
	get { return shippingAddressId; }
	set { shippingAddressId = value; }
}

/// <summary>
/// Property relating to database column OrderStatusId(int,not null)
/// </summary>
public int OrderStatusId
{
	get { return orderStatusId; }
	set { orderStatusId = value; }
}

/// <summary>
/// Property relating to database column ShippingStatusId(int,not null)
/// </summary>
public int ShippingStatusId
{
	get { return shippingStatusId; }
	set { shippingStatusId = value; }
}

/// <summary>
/// Property relating to database column PaymentStatusId(int,not null)
/// </summary>
public int PaymentStatusId
{
	get { return paymentStatusId; }
	set { paymentStatusId = value; }
}

/// <summary>
/// Property relating to database column OrderDiscount(decimal(18,4),not null)
/// </summary>
public decimal OrderDiscount
{
	get { return orderDiscount; }
	set { orderDiscount = value; }
}

/// <summary>
/// Property relating to database column OrderTotal(decimal(18,4),not null)
/// </summary>
public decimal OrderTotal
{
	get { return orderTotal; }
	set { orderTotal = value; }
}

/// <summary>
/// Property relating to database column Deleted(bit,not null)
/// </summary>
public bool Deleted
{
	get { return deleted; }
	set { deleted = value; }
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
