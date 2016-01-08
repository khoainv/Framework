/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: NhapHang.cs 				   	     
File Description 	: NhapHang is the corresponding data object to tblNhapHang data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SSD.Mobile.Entities
{
	public partial class NhapHang 
	{
        public ObservableCollection<NhapHangCT> LstNhapHangCT { get; set; }
        public decimal ThanhToan { get; set; }
     }
}
