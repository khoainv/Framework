using SSD.Mobile.Entities;
using SSD.Mobile.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SSD.Mobile.Share
{

    public class NhapHangCNModel : BaseModel
	{
        private ObservableCollection<NhapHangModel> _ListNhapHang;

        public ObservableCollection<NhapHangModel> ListNhapHang
        {
            get
            {
                return _ListNhapHang;
            }
            set
            {
                SetProperty(ref _ListNhapHang, value);
            }
        }

        private string _Title;

        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                SetProperty(ref _Title, value);
            }
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

