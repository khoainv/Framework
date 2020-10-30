using SSD.Mobile.Entities;
using SSD.Mobile.Common;
using System;
using System.Collections.Generic;

namespace SSD.Mobile.Share
{
    
    public class OverviewModel : BaseModel
	{
        public static OverviewModel Map(OverView ov)
        {
            return new OverviewModel()
            {
                BanLe = ov.BanLe,
                ChiPhi = ov.ChiPhi,
                CongNo = ov.CongNo,
                NhapHang = ov.NhapHang,
                TamUng = ov.TamUng,
                ThanhToan = ov.ThanhToan,
                ThuKhac = ov.ThuKhac,
                TienMat = ov.TienMat
            };
        }
        public decimal BanLe { get; set; }
        public decimal ThanhToan { get; set; }
        public decimal NhapHang { get; set; }
        public decimal ChiPhi { get; set; }
        public decimal TamUng { get; set; }
        public decimal ThuKhac { get; set; }
        public decimal CongNo { get; set; }

        private decimal _TienMat;

        public decimal TienMat
        {
            get
            {
                return _TienMat;
            }
            set
            {
                SetProperty(ref _TienMat, value);
            }
        }

	}
}

