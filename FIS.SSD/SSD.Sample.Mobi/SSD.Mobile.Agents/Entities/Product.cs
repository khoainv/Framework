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
using System.Linq;

namespace SSD.Mobile.Entities
{
    public partial class Product
    {
        /// <summary>
        /// Gets or sets the product id.
        /// </summary>
        /// <value>The product id.</value>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SKU.
        /// </summary>
        /// <value>The SKU.</value>
        public string SKU { get; set; }

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        /// <value>The short description.</value>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the full description.
        /// </summary>
        /// <value>The full description.</value>
        public string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets the product template view path.
        /// </summary>
        /// <value>The product template view path.</value>
        public string ProductTemplateViewPath { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords.
        /// </summary>
        /// <value>The meta keywords.</value>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description.
        /// </summary>
        /// <value>The meta description.</value>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title.
        /// </summary>
        /// <value>The meta title.</value>
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the name of the se.
        /// </summary>
        /// <value>The name of the se.</value>
        public string SeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has KM.
        /// </summary>
        /// <value><c>true</c> if this instance has KM; otherwise, <c>false</c>.</value>
        public bool HasKm { get; set; }

        /// <summary>
        /// Gets or sets the product picture.
        /// </summary>
        /// <value>The product picture.</value>
        public Picture PictureProduct { get; set; }

        public List<Picture> PictureProducts { get; set; }

    }
}
