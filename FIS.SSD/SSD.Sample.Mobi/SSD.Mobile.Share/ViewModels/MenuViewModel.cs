using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SSD.Mobile.Entities;
using SSD.Mobile.BusinessLogics;
using System;

namespace SSD.Mobile.Share
{
    public partial class MenuViewModel : BaseViewModel<UserModel>
	{
        public MenuViewModel()
        {
        }
		public override void OnCreate ()
		{
			base.OnCreate ();
            Model = new UserModel();
            Model.UserName = UserBiz.Instance.User == null ? string.Empty : UserBiz.Instance.User.UserName;
		}

        public void ShowOverview()
        {
            ShowViewModel<OverviewViewModel>();
        }
        private ICommand _OverviewCommand;
        public ICommand OverviewCommand
        {
            get
            {
                return (_OverviewCommand = _OverviewCommand ?? new MvxCommand(ShowOverview));
            }
        }

        public void ShowGiamSat()
        {
            ShowViewModel<GiamSatViewModel>();
        }
        private ICommand _GiamSatCommand;
        public ICommand GiamSatCommand
        {
            get
            {
                return (_GiamSatCommand = _GiamSatCommand ?? new MvxCommand(ShowGiamSat));
            }
        }
        public void ShowCuaHang()
        {
            ShowViewModel<LocationStoreViewModel>();
        }
        private ICommand _CuaHangCommand;
        public ICommand CuaHangCommand
        {
            get
            {
                return (_CuaHangCommand = _CuaHangCommand ?? new MvxCommand(ShowCuaHang));
            }
        }
        public void ShowCongNo()
        {
            ShowViewModel<NhapHangCNViewModel>();
        }
        private ICommand _CongNoCommand;
        public ICommand CongNoCommand
        {
            get
            {
                return (_CongNoCommand = _CongNoCommand ?? new MvxCommand(ShowCongNo));
            }
        }
        public void ShowPlan()
        {
            ShowViewModel<NhapHangPlanViewModel>();
        }
        private ICommand _PlanCommand;
        public ICommand PlanCommand
        {
            get
            {
                return (_PlanCommand = _PlanCommand ?? new MvxCommand(ShowPlan));
            }
        }
        public void ShowLogin()
        {
            ShowViewModel<LoginViewModel>();
        }
        private ICommand _LogoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                return (_LogoutCommand = _LogoutCommand ?? new MvxCommand(ShowPlan));
            }
        }
        
	}
}

