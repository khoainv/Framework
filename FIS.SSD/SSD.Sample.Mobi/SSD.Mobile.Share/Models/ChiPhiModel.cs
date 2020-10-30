using SSD.Mobile.Entities;
using SSD.Mobile.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SSD.Framework;
using SSD.Framework.Extensions;

namespace SSD.Mobile.Share
{
    public enum EnumLoaiChi
    {
        [EnumDescription("Chi khác")]
        ChiKhac = 1,
        [EnumDescription("Trả vay nợ")]
        TraVayNo = 2,
        [EnumDescription("Trả cược vỏ")]
        TraCuocVo = 3
    }

    public class ChiPhiModel : PagedBaseModel
	{
        public static ObservableCollection<ChiPhiModel> Map(ObservableCollection<ChiPhi> lst)
        {
            ObservableCollection<ChiPhiModel> lstTemp = new ObservableCollection<ChiPhiModel>();
            foreach (var bl in lst)
            {
                lstTemp.Add(Map(bl));
            }
            return lstTemp;
        }

        public static ChiPhiModel Map(ChiPhi bl)
        {
            return new ChiPhiModel()
            {
                OnID = bl.OnID,
                SoTien = bl.SoTien,
                NguoiChi = bl.NguoiChi,
                NoiDung = bl.NoiDung,
                NgayChi = bl.NgayChi,
                LoaiChi = bl.LoaiChi == 0 ? string.Empty : EnumHelper.GetEnumDescription((EnumLoaiChi)bl.LoaiChi)
            };
        }
        private string _LoaiChi;

        public string LoaiChi
        {
            get
            {
                return _LoaiChi;
            }
            set
            {
                SetProperty(ref _LoaiChi, value);
            }
        }
        private DateTime _NgayChi;

        public DateTime NgayChi
        {
            get
            {
                return _NgayChi;
            }
            set
            {
                SetProperty(ref _NgayChi, value);
            }
        }
        private string _NguoiChi;

        public string NguoiChi
        {
            get
            {
                return _NguoiChi;
            }
            set
            {
                SetProperty(ref _NguoiChi, value);
            }
        }
        private string _NoiDung;

        public string NoiDung
        {
            get
            {
                return _NoiDung;
            }
            set
            {
                SetProperty(ref _NoiDung, value);
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

