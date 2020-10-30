using SSD.Mobile.Entities;
using SSD.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SSD.Mobile.Share
{
    public class BanLeCTModel : BaseModel
    {
        public static ObservableCollection<BanLeCTModel> Map(ObservableCollection<BanLeCT> lstBanLe)
        {
            ObservableCollection<BanLeCTModel> lst = new ObservableCollection<BanLeCTModel>();
            foreach (var bl in lstBanLe)
            {
                lst.Add(Map(bl));
            }
            return lst;
        }

        public static BanLeCTModel Map(BanLeCT bl)
        {
            return new BanLeCTModel()
            {
                OnID = bl.OnID,
                LocationStoreID = bl.LocationStoreID,
                BanLeID = bl.BanLeID,
                SKU = bl.SKU,
                SLBan = bl.SLBan,
                DonGia = bl.DonGia,
                GiamGia = bl.GiamGia,
                ThanhTien = bl.ThanhTien,
                VAT = bl.VAT,
                TienVAT = bl.TienVAT,
                CKPhanBo = bl.CKPhanBo,
                GiaBan = bl.GiaBan,
                Ten = bl.GiamGia == 0 ? bl.Ten : bl.Ten+", KM["+bl.GiamGia.ToStringN0()+"]",
                DVT = bl.DVT,
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
        protected int banLeID;

        public int BanLeID
        {
            get { return banLeID; }
            set { banLeID = value; }
        }
        protected string sKU;

        public string SKU
        {
            get { return sKU; }
            set { sKU = value; }
        }

        protected long sLBan;

        public long SLBan
        {
            get { return sLBan; }
            set { sLBan = value; }
        }

        protected decimal donGia;
        public decimal DonGia
        {
            get { return donGia; }
            set { donGia = value; }
        }

        protected decimal giamGia;

        public decimal GiamGia
        {
            get { return giamGia; }
            set { giamGia = value; }
        }

        protected decimal thanhTien;

        public decimal ThanhTien
        {
            get { return thanhTien; }
            set { thanhTien = value; }
        }

        protected int vAT;

        public int VAT
        {
            get { return vAT; }
            set { vAT = value; }
        }
        protected decimal tienVAT;

        public decimal TienVAT
        {
            get { return tienVAT; }
            set { tienVAT = value; }
        }

        protected decimal cKPhanBo;

        public decimal CKPhanBo
        {
            get { return cKPhanBo; }
            set { cKPhanBo = value; }
        }

        protected decimal giaBan;
        public decimal GiaBan
        {
            get { return giaBan; }
            set { giaBan = value; }
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
    public class BanLeModel : PagedBaseModel
	{
        public static ObservableCollection<BanLeModel> Map(ObservableCollection<BanLe> lstBanLe)
        {
            ObservableCollection<BanLeModel> lst = new ObservableCollection<BanLeModel>();
            foreach (var bl in lstBanLe)
            {
                lst.Add(Map(bl));
            }
            return lst;
        }
        
        public static BanLeModel Map(BanLe bl)
        {
            return new BanLeModel()
            {
                OnID = bl.OnID,
                TongTien = bl.TongTien,
                ChietKhau = bl.ChietKhau,
                TienHang = bl.TienHang,
                LocationStoreID = bl.LocationStoreID,
                ID = bl.ID
            };
        }
        public static ObservableCollection<BanLeModel> MapFull(ObservableCollection<BanLe> lstBanLe)
        {
            ObservableCollection<BanLeModel> lst = new ObservableCollection<BanLeModel>();
            foreach (var bl in lstBanLe)
            {
                lst.Add(MapFull(bl));
            }
            return lst;
        }

        public static BanLeModel MapFull(BanLe bl)
        {
            return new BanLeModel()
            {
                OnID = bl.OnID,
                TongTien = bl.TongTien,
                ChietKhau = bl.ChietKhau,
                TienHang = bl.TienHang,
                CreatedAt = bl.CreatedAt,
                CreatedBy = bl.CreatedBy,
                IsDeleted = bl.IsDeleted,
                IsDistribution = bl.IsDistribution,
                LoaiThanhToan = bl.LoaiThanhToan,
                LocationStoreID = bl.LocationStoreID,
                ID = bl.ID,
                NgayTT = bl.NgayTT,
                QuayBanHangID = bl.QuayBanHangID,
                TrangThai = bl.TrangThai,
                ListBanLeCT = BanLeCTModel.Map(bl.LstBanLeCT)
            };
        }
        private ObservableCollection<BanLeCTModel> _ListBanLeCT;

        public ObservableCollection<BanLeCTModel> ListBanLeCT
        {
            get
            {
                return _ListBanLeCT;
            }
            set
            {
                SetProperty(ref _ListBanLeCT, value);
            }
        }
        
        private int _TrangThai;

        public int TrangThai
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
        private int _QuayBanHangID;

        public int QuayBanHangID
        {
            get
            {
                return _QuayBanHangID;
            }
            set
            {
                SetProperty(ref _QuayBanHangID, value);
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
        private string _LoaiThanhToan;

        public string LoaiThanhToan
        {
            get
            {
                return _LoaiThanhToan;
            }
            set
            {
                SetProperty(ref _LoaiThanhToan, value);
            }
        }
        private bool _IsDistribution;

        public bool IsDistribution
        {
            get
            {
                return _IsDistribution;
            }
            set
            {
                SetProperty(ref _IsDistribution, value);
            }
        }
        private bool _IsDeleted;

        public bool IsDeleted
        {
            get
            {
                return _IsDeleted;
            }
            set
            {
                SetProperty(ref _IsDeleted, value);
            }
        }
        private string _CreatedBy;

        public string CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetProperty(ref _CreatedBy, value);
            }
        }
        private DateTime _CreatedAt;

        public DateTime CreatedAt
        {
            get
            {
                return _CreatedAt;
            }
            set
            {
                SetProperty(ref _CreatedAt, value);
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

