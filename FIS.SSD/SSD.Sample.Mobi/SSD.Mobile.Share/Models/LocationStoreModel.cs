using SSD.Mobile.Entities;
using SSD.Mobile.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SSD.Mobile.Share
{
    public class LocationStoreModel : PagedBaseModel
	{
        public static ObservableCollection<LocationStoreModel> Map(ObservableCollection<ComLocationStore> lst)
        {
            ObservableCollection<LocationStoreModel> lstTemp = new ObservableCollection<LocationStoreModel>();
            foreach (var bl in lst)
            {
                lstTemp.Add(Map(bl));
            }
            return lstTemp;
        }

        public static LocationStoreModel Map(ComLocationStore bl)
        {
            return new LocationStoreModel()
            {
                StoreName = bl.StoreName,
                IpClient = bl.IpClient,
                StoreAddress = bl.StoreAddress,
                ID = bl.ID,
            };
        }
        private string _StoreAddress;

        public string StoreAddress
        {
            get
            {
                return _StoreAddress;
            }
            set
            {
                SetProperty(ref _StoreAddress, value);
            }
        }
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                SetProperty(ref _ID, value);
            }
        }
        
        private string _StoreName;

        public string StoreName
        {
            get
            {
                return _StoreName;
            }
            set
            {
                SetProperty(ref _StoreName, value);
            }
        }
        private string _IpClient;

        public string IpClient
        {
            get
            {
                return _IpClient;
            }
            set
            {
                SetProperty(ref _IpClient, value);
            }
        }
        
	}
}

