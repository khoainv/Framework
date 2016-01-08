/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: DMSanPham.cs 				   	     
File Description 	: DMSanPham is the corresponding data object to tblDMSanPham data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SSD.Mobile.Entities
{
	public partial class DMSanPham 
	{
        public string ImageUrl { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<string> ImageResizeUrls { get; set; }

		#region Column Names
		
        public const string FIELD_StoreName = "StoreName";
        public const string FIELD_SLTon = "SLTon";
        public const string FIELD_GiaLe = "GiaLe";
        public const string FIELD_SyncDatePrice = "SyncDatePrice";
		#endregion
		
        public long SLTon{get;set;}
        public string StoreName{get;set;}
        public decimal GiaLe { get; set; }
        public DateTime SyncDatePrice { get; set; }
     }
}
