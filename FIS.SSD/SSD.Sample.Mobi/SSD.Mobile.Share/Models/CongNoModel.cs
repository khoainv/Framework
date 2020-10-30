using SSD.Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SSD.Mobile.Share
{
    public class CongNoModel : PagedBaseModel
	{
        public static ObservableCollection<CongNoModel> Map(ObservableCollection<BanSiTT> lstBanSiTT)
        {
            ObservableCollection<CongNoModel> lst = new ObservableCollection<CongNoModel>();
            foreach (var bl in lstBanSiTT)
            {
                lst.Add(new CongNoModel()
                {
                    OnID = bl.OnID,
                    TongTien = bl.ThanhToan
                });
            }
            return lst;
        }

        public static CongNoModel Map(BanSiTT bl)
        {
            return new CongNoModel()
            {
                OnID = bl.OnID,
                TongTien = bl.ThanhToan
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

