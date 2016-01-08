using SSD.Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SSD.Mobile.Share
{
    public class TienMatModel : PagedBaseModel
	{
        public static ObservableCollection<TienMatModel> Map(ObservableCollection<TienMat> lstTienMat)
        {
            ObservableCollection<TienMatModel> lst = new ObservableCollection<TienMatModel>();
            foreach (var bl in lstTienMat)
            {
                lst.Add(new TienMatModel()
                {
                    //OnID = bl.OnID,
                    //TongTien = bl.TongTien
                });
            }
            return lst;
        }

        public static TienMatModel Map(TienMat bl)
        {
            return new TienMatModel()
            {
                //OnID = bl.OnID,
                //TongTien = bl.TongTien
            };
        }

        private decimal _TongTien;

        public decimal TongTien
        {
            get
            {
                return _TongTien;
            }
            set
            {
                SetProperty(ref _TongTien, value);
            }
        }
	}
}

