using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class LocationStoreView : BaseMvxWindowsPage
    {
        public new LocationStoreViewModel ViewModel
        {
            get { return (LocationStoreViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public LocationStoreView()
        {
            this.InitializeComponent();
        }
        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
        }
    }
}
