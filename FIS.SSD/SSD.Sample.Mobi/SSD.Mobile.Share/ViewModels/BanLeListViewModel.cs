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
    public partial class BanLeListViewModel : ReportListBaseViewModel<BanLeModel>, IPageSource<BanLeModel>
	{
        public BanLeListViewModel()
        {
        }
		public override void OnCreate ()
		{
			base.OnCreate ();
		}
        public void ShowBanLeChiTiet(BanLeModel item)
        {
            ShowViewModel<BanLeCTViewModel>(item);
        }
        private ICommand banLeChiTietCommand;
        public ICommand BanLeChiTietCommand
        {
            get
            {
                return (banLeChiTietCommand = banLeChiTietCommand ?? new MvxCommand<BanLeModel>((item)=>ShowBanLeChiTiet(item)));
            }
        }
        public async Task<ObservableCollection<BanLeModel>> GetPagedOlder(PagedPara id)
        {
            IsBusy = true;
            var lst = BanLeModel.Map(await BanLeBiz.Instance.GetPagedOlder(id));
            IsBusy = false;
            return lst;
        }
        public async Task<ObservableCollection<BanLeModel>> GetPagedOlder(long currrentOnID, int count)
        {
            IsBusy = true;
            var id = GetPagedPara(currrentOnID, count);
            try
            {
                var lst = BanLeModel.Map(await BanLeBiz.Instance.GetPagedOlder(id));
                return lst;
            }
            catch (Exception ex)
            {
                UXHandler.DisplayAlert("BanLe Error", ex.Message, AlertButton.OK);
            }
            finally
            {
                IsBusy = false;
            }
            return new ObservableCollection<BanLeModel>();
        }       
    }
}

