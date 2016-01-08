using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SSD.Mobile.Entities;
using SSD.Mobile.BusinessLogics;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SSD.Mobile.Share
{
    public partial class OverviewViewModel : ReportBaseViewModel<OverviewModel>
	{
		public OverviewViewModel()
        {
        }
		public override async void OnCreate ()
		{
			base.OnCreate ();
            IsBusy = true;
            try
            {
                var para = CurrentPara();
                Model = OverviewModel.Map(await OverViewBiz.Instance.Get(para));
            }
            catch (Exception ex)
            {
                UXHandler.DisplayAlert("Overview Error", ex.Message, AlertButton.OK);
            }
            finally
            {
                IsBusy = false;
            }
		}
        private const string ParameterKey = "Parameter";
        private bool ShowViewModelOverview<TViewModel>(OverviewModel item) where TViewModel : IMvxViewModel
        {
            var text = JsonConvert.SerializeObject(item);
            return ShowViewModel<TViewModel>(new Dictionary<string, string>()
            {
                {ParameterKey, text}
            });
        }
        public void ShowBanLe(OverviewModel item)
        {
            ShowViewModelOverview<BanLeListViewModel>(item);
        }
        private ICommand banLeCommand;
        public ICommand BanLeCommand
        {
            get
            {
                return (banLeCommand = banLeCommand ?? new MvxCommand<OverviewModel>((item)=>ShowBanLe(item)));
            }
        }

        public void ShowThanhToan(OverviewModel item)
        {
            ShowViewModelOverview<ThanhToanViewModel>(item);
        }
        private ICommand thanhToanCommand;
        public ICommand ThanhToanCommand
        {
            get
            {
                return (thanhToanCommand = thanhToanCommand ?? new MvxCommand<OverviewModel>((item) => ShowThanhToan(item)));
            }
        }
        public void ShowChiPhi(OverviewModel item)
        {
            ShowViewModelOverview<ChiPhiViewModel>(item);
        }
        private ICommand chiPhiCommand;
        public ICommand ChiPhiCommand
        {
            get
            {
                return (chiPhiCommand = chiPhiCommand ?? new MvxCommand<OverviewModel>((item)=>ShowChiPhi(item)));
            }
        }
        public void ShowCongNo(OverviewModel item)
        {
            ShowViewModelOverview<CongNoViewModel>(item);
        }
        private ICommand congNoCommand;
        public ICommand CongNoCommand
        {
            get
            {
                return (congNoCommand = congNoCommand ?? new MvxCommand<OverviewModel>((item)=>ShowCongNo(item)));
            }
        }
        public void ShowNhapHang(OverviewModel item)
        {
            ShowViewModelOverview<NhapHangListViewModel>(item);
        }
        private ICommand nhapHangCommand;
        public ICommand NhapHangCommand
        {
            get
            {
                return (nhapHangCommand = nhapHangCommand ?? new MvxCommand<OverviewModel>((item)=>ShowNhapHang(item)));
            }
        }
        public void ShowTamUng(OverviewModel item)
        {
            ShowViewModelOverview<TamUngViewModel>(item);
        }
        private ICommand tamUngCommand;
        public ICommand TamUngCommand
        {
            get
            {
                return (tamUngCommand = tamUngCommand ?? new MvxCommand<OverviewModel>((item) => ShowTamUng(item)));
            }
        }
        public void ShowThuKhac(OverviewModel item)
        {
            ShowViewModelOverview<ThuKhacViewModel>(item);
        }
        private ICommand thuKhacCommand;
        public ICommand ThuKhacCommand
        {
            get
            {
                return (thuKhacCommand = thuKhacCommand ?? new MvxCommand<OverviewModel>((item) => ShowThuKhac(item)));
            }
        }
	}
}

