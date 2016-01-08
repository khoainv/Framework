/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: OrderItem.cs 				   	     
File Description 	: OrderItem is the corresponding data object to tblOrderItem data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SSD.Mobile.Entities
{
	public partial class OrderItem 
	{
		#region Column Names
		
		public const string FIELD_Name = "Name";
		#endregion

        public string Name { get; set; }
		
     }
}
