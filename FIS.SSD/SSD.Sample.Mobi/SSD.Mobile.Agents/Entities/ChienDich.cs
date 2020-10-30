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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SSD.Mobile.Entities
{
    public partial class ChienDich
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the chien dich CT list.
        /// </summary>
        /// <value>The chien dich CT list.</value>
        public ObservableCollection<ChienDichCT> ChienDichCTList
        {
            get;
            set;
        }
        #endregion
    }
}
