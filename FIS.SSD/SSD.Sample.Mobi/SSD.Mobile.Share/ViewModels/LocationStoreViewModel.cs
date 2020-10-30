using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SSD.Mobile.Entities;
using System.Threading.Tasks;
using SSD.Mobile.BusinessLogics;
using System;

namespace SSD.Mobile.Share
{
    public partial class LocationStoreViewModel : BaseViewModel<LocationStoreModel>
	{
        public LocationStoreViewModel()
        {
        }
		public override async void OnCreate ()
		{
			base.OnCreate ();

            IsBusy = true;
            try
            {
                 var lst= await LocationStoreBiz.Instance.GetAll();
                 ListModel = LocationStoreModel.Map(lst);
            }
            catch (Exception ex)
            {
                UXHandler.DisplayAlert("Location Error", ex.Message, AlertButton.OK);
            }
            finally
            {
                IsBusy = false;
            }
		}
    }
}

