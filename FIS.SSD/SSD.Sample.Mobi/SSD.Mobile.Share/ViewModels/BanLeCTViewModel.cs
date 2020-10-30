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
    public partial class BanLeCTViewModel : ReportBaseViewModel<BanLeModel>
	{
        BanLeModel _model;
        public async void Init(BanLeModel model)
        {
            _model = model;
            if (model != null)
            {
                IsBusy = true;
                try
                {
                    var para = new IDPara() { LocationStoreID = model.LocationStoreID, ID = model.ID };
                    Model = BanLeModel.MapFull(await BanLeBiz.Instance.Get(para));
                }
                catch (Exception ex)
                {
                    UXHandler.DisplayAlert("BanLe CT Error", ex.Message, AlertButton.OK);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
		public override void OnCreate ()
		{
			base.OnCreate ();
		}
        private ICommand backCommand;
        public ICommand BackCommand
        {
            get
            {
                return backCommand = backCommand ?? new MvxCommand(()=>ShowViewModel<BanLeListViewModel>());
            }
        }
    }
}

