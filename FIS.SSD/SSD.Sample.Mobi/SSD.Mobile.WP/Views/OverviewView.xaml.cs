using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class OverviewView : BaseMvxWindowsPage
    {
        public new OverviewViewModel ViewModel
        {
            get { return (OverviewViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public OverviewView()
        {
            this.InitializeComponent();
        }
        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
    }
}
