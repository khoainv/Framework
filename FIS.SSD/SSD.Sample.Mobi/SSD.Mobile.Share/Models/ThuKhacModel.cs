using SSD.Mobile.Entities;
using SSD.Mobile.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SSD.Framework;

namespace SSD.Mobile.Share
{
    
    public enum EnumLoaiThu
    {
        [EnumDescription("Thu khác")]
        ThuKhac = 1,
        [EnumDescription("Vay")]
        Vay = 2,
        [EnumDescription("Thu cược vỏ")]
        ThuCuocVo = 3
    }
    public class ThuKhacModel : PagedBaseModel
	{
        public static ObservableCollection<ThuKhacModel> Map(ObservableCollection<ThuKhac> lstThuKhac)
        {
            ObservableCollection<ThuKhacModel> lst = new ObservableCollection<ThuKhacModel>();
            foreach (var bl in lstThuKhac)
            {
                lst.Add(Map(bl));
            }
            return lst;
        }

        public static ThuKhacModel Map(ThuKhac bl)
        {
            return new ThuKhacModel()
            {
                OnID = bl.OnID,
                SoTien = bl.SoTien,
                LyDo = bl.LyDo,
                NgayThu = bl.NgayThu,
                LoaiThu = bl.LoaiThu==0?string.Empty:EnumHelper.GetEnumDescription((EnumLoaiThu)bl.LoaiThu)
            };
        }
        private DateTime _NgayThu;

        public DateTime NgayThu
        {
            get
            {
                return _NgayThu;
            }
            set
            {
                SetProperty(ref _NgayThu, value);
            }
        }
        private string _LoaiThu;

        public string LoaiThu
        {
            get
            {
                return _LoaiThu;
            }
            set
            {
                SetProperty(ref _LoaiThu, value);
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
        private decimal _SoTien;

        public decimal SoTien
        {
            get
            {
                return _SoTien;
            }
            set
            {
                SetProperty(ref _SoTien, value);
            }
        }
	}
}

