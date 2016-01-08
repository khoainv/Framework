using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SSD.Mobile.Entities;
using System.Threading.Tasks;
using SSD.Mobile.BusinessLogics;
using System;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.Share
{
    public partial class GiamSatViewModel : ReportListBaseViewModel<BanLeModel>, IPageSource<BanLeModel>
	{
        public GiamSatViewModel()
        {
        }
		public override void OnCreate ()
		{
			base.OnCreate ();
		}
        public void ShowBanLeChiTiet()
        {
            ShowViewModel<BanLeListViewModel>();
        }
        private ICommand banLeChiTietCommand;
        public ICommand BanLeChiTietCommand
        {
            get
            {
                return (banLeChiTietCommand = banLeChiTietCommand ?? new MvxCommand(ShowBanLeChiTiet));
            }
        }
        public async Task<ObservableCollection<BanLeModel>> GetPagedOlder(PagedPara id)
        {
            IsBusy = true;
            var lst = BanLeModel.Map(await BanLeBiz.Instance.GetFullPagedOlder(id));
            IsBusy = false;
            return lst;
        }
        public async Task<ObservableCollection<BanLeModel>> GetPagedOlder(long currrentOnID, int count)
        {
            IsBusy = true;
            var id = GetPagedPara(currrentOnID, count);
            try
            {
                var result = await BanLeBiz.Instance.GetFullPagedOlder(id);
                var lst = BanLeModel.MapFull(result);
                return lst;
            }
            catch (Exception ex)
            {
                UXHandler.DisplayAlert("GiamSat Error", ex.Message, AlertButton.OK);
            }
            finally
            {
                IsBusy = false;
            }
            return new ObservableCollection<BanLeModel>();
        }       
    }
}

