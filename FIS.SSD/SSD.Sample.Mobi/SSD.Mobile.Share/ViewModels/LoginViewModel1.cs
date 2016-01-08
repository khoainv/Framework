using System;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using System.Diagnostics;
using Sync7i.Mobile.BusinessLogics;
using Sync7i.Mobile.BusinessEntities;

namespace Sync7i.Mobile.Share
{
	public class LoginViewModel : BaseViewModel<UserModel> {
        public LoginViewModel()
		{
		}
        public override void OnCreate()
        {
			base.OnCreate();
        }

		private ICommand loginCommand;
		public ICommand LoginCommand {
			get { 
				return (loginCommand = loginCommand ?? new MvxCommand (DoLogin));
			}
		}

		private async void DoLogin () {
			IsBusy = true;
            var status = await UserBiz.Instance.Authen(new BusinessEntities.UserAuthen() 
            { UserId = "admin@7i.com.vn", Password = "abcde12345-", MacAddress = "12345" });
			IsBusy = false;
		}

        private ICommand banLeDetailCommand;
        public ICommand BanLeDetailCommand
        {
            get
            {
                return (banLeDetailCommand = banLeDetailCommand ?? new MvxCommand(BanLeGet));
            }
        }

        private async void BanLeGet()
        {
            IsBusy = true;
            var status = await BanLeBiz.Instance.Get(new IDPara() { ID = 1, LocationStoreID = 1 });
            IsBusy = false;
        }
	}
}


