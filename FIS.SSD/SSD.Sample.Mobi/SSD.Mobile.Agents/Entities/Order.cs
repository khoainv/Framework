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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SSD.Mobile.Entities
{
	public partial class Order 
	{
        public ObservableCollection<OrderItem> Items { get; set; }
     }
}
