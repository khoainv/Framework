/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: ChienDich.cs 				   	     
File Description 	: ChienDich is the corresponding data object to tblChienDich data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SSD.Mobile.Entities
{
    public partial class OverView
    {
        public decimal BanLe { get; set; }
        public decimal ThanhToan { get; set; }
        public decimal NhapHang { get; set; }
        public decimal ChiPhi { get; set; }
        public decimal TamUng { get; set; }
        public decimal ThuKhac { get; set; }
        public decimal CongNo { get; set; }
        public decimal TienMat { get; set; }
        
    }
}
