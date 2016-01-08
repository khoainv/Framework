using System;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using System.Diagnostics;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Entities;
using SSD.Mobile.Common;
using SSD.Framework;
using SSD.Framework.Security;
using SSD.Framework.Exceptions;

namespace SSD.Mobile.Share
{
	public class LoginViewModel : BaseViewModel<UserModel> {
        private readonly INetworkService _NService;
        IPlatform _DeviceDetails;
        public LoginViewModel(INetworkService nservice, IPlatform details)
		{
            _NService = nservice;
            _DeviceDetails = details;
		}
        public override void OnCreate()
        {
			base.OnCreate();
            if (string.IsNullOrWhiteSpace(UserBiz.Instance.UGToken))
            {
                Model = new UserModel();
                if (UserBiz.Instance.User != null)
                {
                    Model.UserName = UserBiz.Instance.User.UserName;
                    Model.Password = UserBiz.Instance.User.Password;
                }
                else
                {
                    Model.UserName = "admin@7i.com.vn";
                    Model.Password = "abcde12345-";
                }
            }
            else ShowHomePage();
        }

        public void ShowHomePage()
        {
            ShowViewModel<MenuViewModel>();
		}
        private ICommand accessCommand;
        public ICommand AccessCommand
        {
			get {
                return (accessCommand = accessCommand ?? new MvxCommand(ShowHomePage));
			}
		}
        public void CleanToken()
        {
            UserBiz.Instance.ResetToken();
        }
        private ICommand cleanTokenCommand;
        public ICommand CleanTokenCommand
        {
			get {
                return (cleanTokenCommand = cleanTokenCommand ?? new MvxCommand(CleanToken));
			}
		}
        
		private ICommand loginCommand;
		public ICommand LoginCommand {
			get { 
				return (loginCommand = loginCommand ?? new MvxCommand (DoLogin));
			}
		}

		private async void DoLogin () {
			IsBusy = true;
            if (!_NService.IsNetworkAvailable())
            {
                UXHandler.DisplayAlert("Network Error", "No network available!", AlertButton.OK);
                return;
            }
			try {
                var status = await UserBiz.Instance.Authen(new UserAuthen(Model.UserName, Model.Password, _NService.MacAddress(),Sync7iConstants.ExpireTimeSpanHours));

                if (status.Successeded)
                {
                    //var lstMenu = await _commonBiz.GetMenu();
                    //if (lstMenu.Where(x=>x.MenuType==MenuType.Monitoring).Count() > 0)
                    //    ShowViewModel<ProjectMonitorViewModel>();
                    ShowHomePage();
				}else
                {
                    UXHandler.DisplayAlert("Login Error", "Login Not Successful!", AlertButton.OK);
                }
            }
            catch (UnknownException ex)
            {
                UXHandler.DisplayAlert("Login Error", ex.Message, AlertButton.OK);
			}finally{
				IsBusy = false;
			}
		}
	}
}


