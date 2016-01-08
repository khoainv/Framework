/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description: Sync7i is project intergarte client SmartNet and web 7i.com.vn
						
File Name	   		: BanLeCT.cs 				   	     
File Description 	: BanLeCT is the corresponding data object to tblBanLeCT data table

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SSD.Mobile.Entities
{
    public class BanLeCTColumnIndexExt
    {
        public BanLeCTColumnIndexExt()
        {
            Ten = -1;
            DVT = -1;
        }
        public int Ten { get; set; }
        public int DVT { get; set; }
    }
    public partial class BanLeCT
    {
        public const string FIELD_Ten = "Ten";
        public const string FIELD_DVT = "DVT";
        public string DVT { get; set; }
        public string Ten { get; set; }

    }
}
