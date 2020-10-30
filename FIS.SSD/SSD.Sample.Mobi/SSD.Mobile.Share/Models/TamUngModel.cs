using SSD.Mobile.Entities;
using SSD.Mobile.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SSD.Framework;

namespace SSD.Mobile.Share
{
    public enum EnumTamUng
    {
        [EnumDescription("Mua hàng")]
        MuaHang = 1,
        [EnumDescription("Tạm ứng")]
        TamUng = 2
    }
    public class TamUngModel : PagedBaseModel
	{
        public static ObservableCollection<TamUngModel> Map(ObservableCollection<TamUng> lstTamUng)
        {
            ObservableCollection<TamUngModel> lst = new ObservableCollection<TamUngModel>();
            foreach (var bl in lstTamUng)
            {

                lst.Add(Map(bl));
            }
            return lst;
        }

        public static TamUngModel Map(TamUng bl)
        {
            return new TamUngModel()
            {
                OnID = bl.OnID,
                SoTienTamUng = bl.SoTienTamUng,
                HoTen = bl.HoTen,
                LyDo = bl.LyDo,
                NgayTamUng = bl.NgayTamUng,
                LoaiTamUng = bl.Loai == 0 ? string.Empty : EnumHelper.GetEnumDescription((EnumTamUng)bl.Loai)
            };
        }
        private string _LoaiTamUng;

        public string LoaiTamUng
        {
            get
            {
                return _LoaiTamUng;
            }
            set
            {
                SetProperty(ref _LoaiTamUng, value);
            }
        }
        private DateTime _NgayTamUng;

        public DateTime NgayTamUng
        {
            get
            {
                return _NgayTamUng;
            }
            set
            {
                SetProperty(ref _NgayTamUng, value);
            }
        }
        private string _HoTen;

        public string HoTen
        {
            get
            {
                return _HoTen;
            }
            set
            {
                SetProperty(ref _HoTen, value);
            }
        }
        private string _LyDo;

        public string LyDo
        {
            get
            {
                return _LyDo;
            }
            set
            {
                SetProperty(ref _LyDo, value);
            }
        }
        private decimal _SoTienTamUng;

        public decimal SoTienTamUng
        {
            get
            {
                return _SoTienTamUng;
            }
            set
            {
                SetProperty(ref _SoTienTamUng, value);
            }
        }
	}
}

