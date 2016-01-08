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
    public partial class CongNoViewModel : ReportBaseViewModel<CongNoModel>, IPageSource<CongNoModel>
	{
        public CongNoViewModel()
        {
        }
		public override void OnCreate ()
		{
			base.OnCreate ();
		}

        public async Task<ObservableCollection<CongNoModel>> GetPagedOlder(PagedPara id)
        {
            if (id.StartIndex > 0)
                return new ObservableCollection<CongNoModel>();
            return CongNoModel.Map(await BanSiTTBiz.Instance.GetList(id));
        }
        public async Task<ObservableCollection<CongNoModel>> GetPagedOlder(long currrentOnID, int count)
        {
            IsBusy = true;
            var id = GetPagedPara(currrentOnID, count);
            try
            {
                if (id.StartIndex > 0)
                    return new ObservableCollection<CongNoModel>();
                return CongNoModel.Map(await BanSiTTBiz.Instance.GetList(id));
            }
            catch (Exception ex)
            {
                UXHandler.DisplayAlert("CongNo Error", ex.Message, AlertButton.OK);
            }
            finally
            {
                IsBusy = false;
            }
            return new ObservableCollection<CongNoModel>();
        }
    }
}

