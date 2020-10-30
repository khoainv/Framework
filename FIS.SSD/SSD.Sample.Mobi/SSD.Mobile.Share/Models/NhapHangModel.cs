using SSD.Mobile.Entities;
using SSD.Mobile.Common;
using SSD.Framework;
using SSD.Framework.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SSD.Mobile.Share
{
    public enum EnumViewModelNhapHangType
    {
        ThanhToan = 1,
        NhapHang = 2,
        Plan=3,
        CongNo = 4
    }
    public enum EnumTrangThaiCongNo
    {
        [EnumDescription("Đặt hàng")]
        DatHang = 1,
        [EnumDescription("Chưa thanh toán")]
        ChuaThanhToan = 2,
        [EnumDescription("Thanh toán một phần")]
        ThanhToanMotPhan = 3,
        [EnumDescription("Đã thanh toán")]
        DaThanhToan = 4,
        [EnumDescription("Ký gửi")]
        KyGui = 5,
        [EnumDescription("Hủy")]
        Huy = 6
    }
    public class NhapHangCTModel : BaseModel
    {
        public static ObservableCollection<NhapHangCTModel> Map(ObservableCollection<NhapHangCT> lstBanLe)
        {
            ObservableCollection<NhapHangCTModel> lst = new ObservableCollection<NhapHangCTModel>();
            foreach (var bl in lstBanLe)
            {
                lst.Add(Map(bl));
            }
            return lst;
        }

        public static NhapHangCTModel Map(NhapHangCT bl)
        {
            return new NhapHangCTModel()
            {
                OnID = bl.OnID,
                LocationStoreID = bl.LocationStoreID,
                ChietKhau = bl.ChietKhau,
                SKU = bl.SKU,
                DVT = bl.DVT,
                GhiChu = bl.GhiChu,
                GiaNhap = bl.GiaNhap,
                NhapHangID = bl.NhapHangID,
                SoLuong = bl.SoLuong,
                Ten = bl.ChietKhau == 0 ? bl.Ten : bl.Ten + ", CK[" + bl.ChietKhau.ToStringN0() + "]",
                ThanhTien = bl.SoLuong*bl.GiaNhap - bl.ChietKhau
            };
        }
        protected long onID;
        public long OnID
        {
            get { return onID; }
            set { onID = value; }
        }

        protected int locationStoreID;

        public int LocationStoreID
        {
            get { return locationStoreID; }
            set { locationStoreID = value; }
        }
        protected int nhapHangID;

        public int NhapHangID
        {
            get { return nhapHangID; }
            set { nhapHangID = value; }
        }
        protected string sKU;

        public string SKU
        {
            get { return sKU; }
            set { sKU = value; }
        }

        protected long soLuong;

        public long SoLuong
        {
            get { return soLuong; }
            set { soLuong = value; }
        }

        protected decimal chietKhau;

        public decimal ChietKhau
        {
            get { return chietKhau; }
            set { chietKhau = value; }
        }

        protected decimal thanhTien;

        public decimal ThanhTien
        {
            get { return thanhTien; }
            set { thanhTien = value; }
        }

        protected decimal giaNhap;

        public decimal GiaNhap
        {
            get { return giaNhap; }
            set { giaNhap = value; }
        }

        protected string ghiChu;
        public string GhiChu
        {
            get { return ghiChu; }
            set { ghiChu = value; }
        }
        protected string _DVT;
        public string DVT
        {
            get { return _DVT; }
            set { _DVT = value; }
        }
        protected string _Ten;
        public string Ten
        {
            get { return _Ten; }
            set { _Ten = value; }
        }
    }
    public class NhapHangModel : PagedBaseModel
	{
        public static ObservableCollection<NhapHangModel> Map(ObservableCollection<NhapHang> lstNhapHang)
        {
            ObservableCollection<NhapHangModel> lst = new ObservableCollection<NhapHangModel>();
            foreach (var bl in lstNhapHang)
            {
                lst.Add(Map(bl));
            }
            return lst;
        }

        public static NhapHangModel Map(NhapHang bl)
        {
            return new NhapHangModel()
            {
                ID = bl.ID,
                LocationStoreID = bl.LocationStoreID,
                OnID = bl.OnID,
                TongTien = bl.TongTien,
                ChietKhau  =bl.ChietKhau,
                ConNo  =bl.ConNo,
                GhiChu = bl.GhiChu,
                NgayGiao = bl.NgayGiao,
                NgayTT = bl.NgayTT,
                TenNCC = bl.TenNCC,
                ThanhToan = bl.ThanhToan,
                TrangThai = bl.TrangThai == 0 ? string.Empty : EnumHelper.GetEnumDescription((EnumTrangThaiCongNo)bl.TrangThai)
            };
        }
        public static NhapHangModel MapFull(NhapHang bl)
        {
            var chietKhauHang = bl.LstNhapHangCT == null ? 0 : bl.LstNhapHangCT.Sum(x => x.ChietKhau);
            return new NhapHangModel()
            {
                ID = bl.ID,
                LocationStoreID = bl.LocationStoreID,
                OnID = bl.OnID,
                TongTien = bl.TongTien,
                TienHang = bl.TongTien+bl.ChietKhau+chietKhauHang,
                ChietKhauHang = chietKhauHang,
                ChietKhau = bl.ChietKhau,
                ConNo = bl.ConNo,
                GhiChu = bl.GhiChu,
                NgayGiao = bl.NgayGiao,
                NgayTT = bl.NgayTT,
                TenNCC = bl.TenNCC,
                ThanhToan = bl.ThanhToan,
                TrangThai = bl.TrangThai == 0 ? string.Empty : EnumHelper.GetEnumDescription((EnumTrangThaiCongNo)bl.TrangThai),
                ListNhapHangCT = NhapHangCTModel.Map(bl.LstNhapHangCT)
            };
        }
        public EnumViewModelNhapHangType ViewType { get; set; }
        private ObservableCollection<NhapHangCTModel> _ListNhapHangCT;

        public ObservableCollection<NhapHangCTModel> ListNhapHangCT
        {
            get
            {
                return _ListNhapHangCT;
            }
            set
            {
                SetProperty(ref _ListNhapHangCT, value);
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
        private int _LocationStoreID;

        public int LocationStoreID
        {
            get
            {
                return _LocationStoreID;
            }
            set
            {
                SetProperty(ref _LocationStoreID, value);
            }
        }
        private decimal _ThanhToan;

        public decimal ThanhToan
        {
            get
            {
                return _ThanhToan;
            }
            set
            {
                SetProperty(ref _ThanhToan, value);
            }
        }
        private decimal _TienHang;

        public decimal TienHang
        {
            get
            {
                return _TienHang;
            }
            set
            {
                SetProperty(ref _TienHang, value);
            }
        }
        private decimal _ChietKhauHang;

        public decimal ChietKhauHang
        {
            get
            {
                return _ChietKhauHang;
            }
            set
            {
                SetProperty(ref _ChietKhauHang, value);
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

        private string _TenNCC;

        public string TenNCC
        {
            get
            {
                return _TenNCC;
            }
            set
            {
                SetProperty(ref _TenNCC, value);
            }
        }
        private string _TrangThai;

        public string TrangThai
        {
            get
            {
                return _TrangThai;
            }
            set
            {
                SetProperty(ref _TrangThai, value);
            }
        }
        private DateTime _NgayGiao;

        public DateTime NgayGiao
        {
            get
            {
                return _NgayGiao;
            }
            set
            {
                SetProperty(ref _NgayGiao, value);
            }
        }
        private DateTime _NgayTT;

        public DateTime NgayTT
        {
            get
            {
                return _NgayTT;
            }
            set
            {
                SetProperty(ref _NgayTT, value);
            }
        }

        private string _GhiChu;

        public string GhiChu
        {
            get
            {
                return _GhiChu;
            }
            set
            {
                SetProperty(ref _GhiChu, value);
            }
        }

        private decimal _ConNo;

        public decimal ConNo
        {
            get
            {
                return _ConNo;
            }
            set
            {
                SetProperty(ref _ConNo, value);
            }
        }

        private decimal _ChietKhau;

        public decimal ChietKhau
        {
            get
            {
                return _ChietKhau;
            }
            set
            {
                SetProperty(ref _ChietKhau, value);
            }
        }
	}
}

