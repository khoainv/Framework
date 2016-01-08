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
    public partial class TamUngViewModel : ReportListBaseViewModel<TamUngModel>, IPageSource<TamUngModel>
	{
        public TamUngViewModel()
        {
        }
		public override void OnCreate ()
		{
			base.OnCreate ();
		}

        public async Task<ObservableCollection<TamUngModel>> GetPagedOlder(PagedPara id)
        {
            return TamUngModel.Map(await TamUngBiz.Instance.GetPagedOlder(id));
        }
        public async Task<ObservableCollection<TamUngModel>> GetPagedOlder(long currrentOnID, int count)
        {
            IsBusy = true;
            var id = GetPagedPara(currrentOnID, count);
            try
            {
                var lst = TamUngModel.Map(await TamUngBiz.Instance.GetPagedOlder(id));
                return lst;
            }
            catch (Exception ex)
            {
                UXHandler.DisplayAlert("TamUng Error", ex.Message, AlertButton.OK);
            }
            finally
            {
                IsBusy = false;
            }
            return new ObservableCollection<TamUngModel>();
        }
    }
}

