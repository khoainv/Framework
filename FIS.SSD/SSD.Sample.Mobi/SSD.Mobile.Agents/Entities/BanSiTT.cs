/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: BanSiTT.cs 				   	     
File Description 	: BanSiTT is the corresponding data object to tblBanSiTT data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SSD.Mobile.Entities
{
    public class BanSiTTColumnIndexExt
    {
        public BanSiTTColumnIndexExt()
        {
            MaMobile = -1;
            TrangThai = -1;
        }
        public int MaMobile { get; set; }
        public int TrangThai { get; set; }
    }
	public partial class BanSiTT 
	{
        public const string FIELD_MaMobile = "MaMobile";
        public const string FIELD_TrangThai = "TrangThai";
        public string MaMobile { get; set; }
        public int TrangThai { get; set; }
     }
}
