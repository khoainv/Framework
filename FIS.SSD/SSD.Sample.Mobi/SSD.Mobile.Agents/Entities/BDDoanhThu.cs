/*
Product Name		: Sync7i 	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: BanLe.cs 				   	     
File Description 	: BanLe is the corresponding data object to tblBanLe data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using SSD.Framework;
namespace SSD.Mobile.Entities
{
   
    public class BDDoanhThu : BaseJsonEntity<BDDoanhThu>
    {
        public const string FIELD_SoDonHang = "SoDonHang";
        public const string FIELD_TongTien = "TongTien";

        public const string FIELD_Ngay = "Ngay";
        public const string FIELD_HourSale = "HourSale";
        public const string FIELD_DayOfWeeks = "DayOfWeeks";
        public const string FIELD_Weeks = "Weeks";
        public const string FIELD_DayOfMonths = "DayOfMonths";
        public const string FIELD_Months = "Months";

        public System.DateTime Ngay { get; set; }
        public int HourSale { get; set; }
        public int DayOfWeeks { get; set; }
        public int Weeks { get; set; }
        public int DayOfMonths { get; set; }
        public int Months { get; set; }

        public int SoDonHang { get; set; }
        public decimal TongTien { get; set; }
    }
}
