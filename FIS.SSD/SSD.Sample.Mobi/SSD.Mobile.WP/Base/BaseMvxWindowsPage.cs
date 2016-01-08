using System;
using System.Net;
using SSD.Mobile.Share;
using Cirrious.MvvmCross.WindowsCommon.Views;
using Windows.UI.Popups;
using Cirrious.MvvmCross.ViewModels;
using Windows.UI.Core;

namespace SSD.Mobile.WP
{
    public class BaseMvxWindowsPage : MvxWindowsPage, IUXHandler
    {
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ViewModel != null && ViewModel is MvxViewModel)
            {
                LoadDataToList();
                var vm = ((BaseViewModel)ViewModel);
                vm.UXHandler = this;
                vm.OnCreate();
            }
        }
        protected virtual void LoadDataToList()
        {

        }
        
        public void DisplayAlert(string title, string message, AlertButton button)
        {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                new MessageDialog(message, title).ShowAsync();
            });
        }
    }
}

