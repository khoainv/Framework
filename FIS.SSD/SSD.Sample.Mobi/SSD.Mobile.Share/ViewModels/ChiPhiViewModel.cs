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
    public partial class ChiPhiViewModel : ReportListBaseViewModel<ChiPhiModel>, IPageSource<ChiPhiModel>
	{
        public ChiPhiViewModel()
        {
        }
		public override void OnCreate ()
		{
			base.OnCreate ();
		}

        public async Task<ObservableCollection<ChiPhiModel>> GetPagedOlder(PagedPara id)
        {
            return ChiPhiModel.Map(await ChiPhiBiz.Instance.GetPagedOlder(id));
        }

        public async Task<ObservableCollection<ChiPhiModel>> GetPagedOlder(long currrentOnID, int count)
        {
            IsBusy = true;
            var id = GetPagedPara(currrentOnID, count);
            try
            {
                var lst = ChiPhiModel.Map(await ChiPhiBiz.Instance.GetPagedOlder(id));
                return lst;
            }
            catch (Exception ex)
            {
                UXHandler.DisplayAlert("ChiPhi Error", ex.Message, AlertButton.OK);
            }
            finally
            {
                IsBusy = false;
            }
            return new ObservableCollection<ChiPhiModel>();
        }
    }
}

